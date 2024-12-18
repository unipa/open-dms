using AutoMapper;
using Core.DigitalSignature;
using Elmi.Core.FileConverters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Exceptions;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.DigitalSignature;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using OpenDMS.PdfManager;
using System.Text;
using System.Xml;
using static System.Net.WebRequestMethods;

namespace OpenDMS.Core.BusinessLogic;

public class DocumentService : IDocumentService
{



    private readonly IDocumentRepository _repository;
    private readonly ICustomFieldService _dataTypeRepository;
    private readonly IDocumentTypeService _documentTypeRepository;
    private readonly IACLService _aclRepository;
    private readonly IUserGroupRepository userGroupRepo;
    private readonly IRoleRepository roleRepository;

    //private readonly IOrganizationRepository _organizationRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly ILookupTableRepository _lookupTableRepository;
    private readonly IFileConvertFactory fileConvertFactory;
    private readonly IUserService userContext;
    private readonly IEventManager _eventDispatcher;
    private readonly IDocumentNotificationService _notificationService;
    private readonly Domain.Repositories.IHistoryRepository historyRepository;
    private readonly IVirtualFileSystemProvider _filesystemProvider;
    private readonly IDataTypeFactory _dataTypeFactory;
    private readonly IConfiguration configuration;
    private readonly ILogger<IDocumentService> logger;

    private readonly bool _softDelete = false;
    private readonly string _filePrefix = "";


    private readonly string _filesystemType = "";
    private readonly string _rootPath = "";
    private readonly int _protocolLength = 7;


    private Dictionary<string, FieldType> metacache = new Dictionary<string, FieldType>();

    public DocumentService(
        IDocumentRepository docRepo,
        ICustomFieldService metaRepo,
        IDocumentTypeService docTypeRepo,
        IACLService aclRepo,
    //    IOrganizationRepository organizationRepo,
        IUserGroupRepository userGroupRepo,
        IRoleRepository roleRepository,
        //        IRolePermissionRepository rolePermissionRepo,
        ICompanyRepository companyRepo,
        ILookupTableRepository tableRepo,
        IFileConvertFactory fileConvertFactory,
        IUserService userContext,
        IVirtualFileSystemProvider filesystemFactory,
        IDocumentNotificationService notificationService,
        Domain.Repositories.IHistoryRepository historyRepository,
        IEventManager eventDispatcher,
        IDataTypeFactory lookupService,
        IConfiguration configuration,
        ILogger<IDocumentService> logger
        )
    {
        this.logger = logger;
        this._repository = docRepo;
        this._dataTypeRepository = metaRepo;
        this._documentTypeRepository = docTypeRepo;
        this._aclRepository = aclRepo;
        this.userGroupRepo = userGroupRepo;
        this.roleRepository = roleRepository;
        // this._organizationRepository = organizationRepo;
        //  this._rolePermissionRepository = rolePermissionRepo;
        this._eventDispatcher = eventDispatcher;
        this._notificationService = notificationService;
        this.historyRepository = historyRepository;
        this._filesystemProvider = filesystemFactory;
        this._dataTypeFactory = lookupService;
        this.configuration = configuration;
        this._companyRepository = companyRepo;
        this._lookupTableRepository = tableRepo;
        this.fileConvertFactory = fileConvertFactory;
        this.userContext = userContext;
        _softDelete = configuration[StaticConfiguration.CONST_DOCUMENTS_SOFTDELETE] != "";
        _filesystemType = configuration[StaticConfiguration.CONST_DOCUMENTS_FILESYSTEMTYPE];

        _rootPath = configuration[StaticConfiguration.CONST_DOCUMENTS_ROOLFOLDER];
        _filePrefix = configuration[StaticConfiguration.CONST_DOCUMENTS_FILEPREFIX];
        if (string.IsNullOrEmpty(_filePrefix)) _filePrefix = "doc";
        string plen = configuration[StaticConfiguration.CONST_PROTOCOL_LENGTH];
        if (string.IsNullOrEmpty(plen)) plen = "7";
        _protocolLength = int.Parse(plen);

    }

    public async Task ClearIndexing()
    {
        await _repository.ClearIndexing();
    }

    public async Task<bool> Exists(int documentId)
    {
        var doc = await _repository.GetById(documentId);
        return doc != null && doc.Id == documentId;
    }
    public IEnumerable<int> GetContentsToPreview()
    {
        return _repository.GetContentsToPreview();
    }
    //public IEnumerable<int> GetContentsToPreserve()
    //{
    //    return _repository.GetContentsToPreserve();
    //}
    public IEnumerable<int> GetContentsToIndex()
    {
        return _repository.GetContentsToIndex();
    }

    public async Task<int> FindInFolderByUniqueId(int folderId, string externalId, ContentType contentType)
    {
        return await _repository.FindInFolderByUniqueId(null, externalId, folderId, contentType);
    }
    public async Task<int> FindByUniqueId(string docType, string externalId, ContentType contentType)
    {
        return await _repository.FindByUniqueId(docType, externalId, contentType);
    }
    public async Task<int> FindByUniqueId(string externalId)
    {
        return await _repository.FindByUniqueId("", externalId, ContentType.Any);
    }


    public async Task<DocumentVersion> GetPublished(int documentId)
    {
        var images = (await _repository.GetImages(documentId));
        var image = images.Where(i => i.SendingStatus == JobStatus.Completed).FirstOrDefault();
        //if (image == null) image = images.FirstOrDefault();
        if (image != null)
        {

            DocumentVersion v = new DocumentVersion();
            v.CreationDate = image.CreationDate;
            v.FileName = image.FileName;
            v.VersionNumber = image.VersionNumber;
            v.RevisionNumber = image.RevisionNumber;
            v.FileExtension = System.IO.Path.GetExtension(image.FileName);
            //            v.Protected = d.PreviewStatus == BatchProcessStatus.Completed;
            v.SendingStatus = image.SendingStatus;
            v.PreservationStatus = image.PreservationStatus;
            v.SignatureStatus = image.SignatureStatus;
            v.FileSize = image.FileSize;
            v.Owner = image.Owner;
            v.Id = documentId;
            v.ImageId = image.Id;
            return v;
        }
        else return null;
    }

    public async Task<int> GetByDocumentTypeAndNumber(string documentTypeId, string documentNumber)
    {

        return await _repository.GetByDocumentTypeAndNumber(documentTypeId, documentNumber);
    }

    public async Task<DocumentInfo> DocumentSchema(string DocumentTypeId, UserProfile userInfo, ContentType ContentType = ContentType.Document)
    {
        DocumentInfo nd = new DocumentInfo();
        var docType = await _documentTypeRepository.GetById(DocumentTypeId);
        AuthorizationType P = AuthorizationType.None;
        if (DocumentTypeId is null)
        {
            docType.Name = ContentType == ContentType.Document ? "Documento Generico" : "Fascicolo Generico";
            docType.DescriptionLabel = "Descrizione";
            docType.DocumentDateLabel = ContentType == ContentType.Document ? "Data Documento" : "Data Apertura";
            docType.DocumentNumberLabel = ContentType == ContentType.Document ? "Nr.Documento" : "Identificativo";
            docType.InitialStatus = DocumentStatus.Active;
            P = (await _aclRepository.GetAuthorization("$GLOBAL$", userInfo, PermissionType.Profile_CanCreateGenericDocument));
        }
        docType.ContentType = ContentType;

        if (userInfo.userId == SpecialUser.SystemUser || userInfo.IsService)
            P = AuthorizationType.Granted;

        //var roles = userInfo.Roles;
        if (P == AuthorizationType.None)
        {
            P = await _aclRepository.GetAuthorization(docType.ACLId, userInfo, PermissionType.CanCreate);
        }
        ThrowIfNotAuthorized(P, "a creare documenti di tipo ", userInfo, docType.Name);

        nd.DocumentType = docType;
        nd.ContentType = docType.ContentType;
        nd.DocumentNumberFieldType = new FieldType() { Id = "", DataType = docType.DocumentNumberDataType, Title = docType.DocumentNumberLabel };// docType.DocumentNumberDataType;
        nd.Owner = docType.Owner;
        nd.DocumentStatus = docType.InitialStatus;
        nd.Reserved = docType.Reserved;
        nd.ACLId = docType.ACLId;
        nd.PersonalData = docType.PersonalData;
        if (nd.ContentType != ContentType.Folder)
            nd.FolderId = 0;
        nd.CreationDate = DateTime.UtcNow;
        nd.DocumentDate = DateTime.UtcNow.Date;

        FieldType NumberType = null;
        IDataTypeManager nfieldmanager = null;
        var n = nd.DocumentNumberFormattedValue;
        if (!string.IsNullOrEmpty(docType.DocumentNumberDataType))
        {
            // decodifico il numero documento
            NumberType = await _dataTypeRepository.GetById(docType.DocumentNumberDataType);
            nfieldmanager = await _dataTypeFactory.Instance(NumberType.DataType);
            nd.DocumentNumberFieldType = NumberType;
            nd.DocumentNumberFieldType.ControlType = nfieldmanager.ControlType;

            //nd.ControlType = fieldmanager.ControlType;

            if (!nfieldmanager.IsCalculated)
            {
                List<string> lvalues = new List<string>();
                List<string> fvalues = new List<string>();
                foreach (var v in nd.DocumentNumber.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    var ftv = await nfieldmanager.Lookup(NumberType, v);
                    lvalues.Add(ftv.LookupValue);
                    fvalues.Add(ftv.FormattedValue);
                }
                nd.DocumentNumberLookupValue = string.Join(",", lvalues);
                nd.DocumentNumberFormattedValue = string.Join(",", fvalues);
            }
        };

        var exp = nd.ExpirationDate;
        switch (docType.ExpirationStrategy)
        {
            case ExpirationStrategy.None: nd.ExpirationDate = DateTime.MaxValue; break;
            case ExpirationStrategy.DocumentDate:
                if (nd.DocumentDate.HasValue)
                    nd.ExpirationDate = nd.DocumentDate.Value.AddDays(docType.ExpirationDays); break;
            case ExpirationStrategy.CreationDate: nd.ExpirationDate = DateTime.UtcNow.AddDays(docType.ExpirationDays); break;
            case ExpirationStrategy.Content: if (nd.Id <= 0) nd.ExpirationDate = DateTime.MaxValue; break;
            default:
                if (nd.ExpirationDate <= DateTime.MinValue)
                    nd.ExpirationDate = DateTime.MaxValue;
                break;
        }
        nd.FieldList = new List<DocumentFieldInfo>();
        int index = 0;
        // Imposto i campi della nuova tipologia
        if (docType.Fields != null)
        {
            foreach (var tfield in docType.Fields.Where(t => !t.Deleted).OrderBy(T => T.FieldIndex))
            //for (int i = 0; i < docType.FieldCount; i++)
            {
                string fieldType = tfield.FieldTypeId;
                FieldType M = null;
                if (fieldType != null && metacache.ContainsKey(fieldType))
                    M = metacache[fieldType];
                else
                {
                    M = await _dataTypeRepository.GetById(fieldType);
                    if (M == null)
                    {
                        M = new FieldType()
                        {
                            Id = null,
                            DefaultValue = tfield.DefaultValue,
                            DataType = "",
                            Tag = false,
                            Encrypted = false
                        };
                    }
                    if (fieldType != null)
                        metacache[fieldType] = M;
                }
                var DefaultValue = !string.IsNullOrEmpty(tfield.DefaultValue) ? tfield.DefaultValue : M.DefaultValue;
                index++;
                DocumentFieldInfo ff = new DocumentFieldInfo();
                ff = new DocumentFieldInfo();
                ff.FieldTypeId = M.Id;
                ff.Value = DefaultValue + "";
                ff.Encrypted = M.Encrypted || tfield.Encrypted;
                ff.FieldIdentifier = tfield.FieldName;
                ff.Tag = M.Tag || tfield.Tag;
                ff.DataType = M.DataType;
                ff.Title = tfield.Title;
                if (string.IsNullOrEmpty(ff.Title))
                {
                    ff.Title = M.Title;
                    tfield.Title = M.Title;
                }
                ff.CustomProperties = M.CustomProperties;
                ff.FieldIndex = index; // tfield.FieldIndex;
                ff.Customized = false;
                nd.FieldList.Add(ff);

                var fieldmanager = await _dataTypeFactory.Instance(M.DataType);
                ff.ControlType = fieldmanager.ControlType;
                if (!fieldmanager.IsCalculated)
                {
                    List<string> lvalues = new List<string>();
                    List<string> fvalues = new List<string>();
                    List<string> values = new List<string>();
                    if (ff.Value != null)
                        foreach (var v in ff.Value.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                        {
                            var ftv = await fieldmanager.Lookup(M, v);
                            values.Add(ftv.Value);
                            lvalues.Add(ftv.LookupValue);
                            fvalues.Add(ftv.FormattedValue);
                        }
                    ff.Value = string.Join(",", values);
                    ff.LookupValue = string.Join(",", lvalues);
                    ff.FormattedValue = string.Join(",", fvalues);
                }
            }
        }
        nd.Image = null;// new DocumentImage();
        return nd;
    }

    public async Task<int> GetUserFolder(UserProfile userId)
    {
        var doc = await _repository.FindByUniqueId(null, "$USERS$", ContentType.Folder);
        if (doc == 0)
        {
            var newdoc = new Document()
            {
                CompanyId = 0,
                ContentType = ContentType.Folder,
                Description = "Fascicolo Utenti",
                Icon = "fa fa-users",
                IconColor = "violet",
                DocumentNumber = "Utenti",
                DocumentDate = DateTime.UtcNow,
                DocumentStatus = DocumentStatus.Active,
                Reserved = true,
                ExternalId = "$USERS$",
                Owner = SpecialUser.SystemUser
            };
            await _repository.Create(newdoc);
            await _repository.SetPermission(newdoc.Id, SpecialUser.AdminRole, ProfileType.Role, PermissionType.CanView, AuthorizationType.Granted);
            //await _repository.SetPermission(newdoc.Id, SpecialUser.AdminRole, ProfileType.Role, PermissionType.CanViewContent, AuthorizationType.Granted);
            doc = newdoc.Id;
        }
        var CanHavePersonalFolder = (await _aclRepository.GetAuthorization("$GLOBAL$", userId, PermissionType.Profile_CanHavePersonalFolder) == AuthorizationType.Granted);

        var userDoc = await _repository.FindInFolderByUniqueId(null, "$USER." + userId.userId + "$", doc, ContentType.Folder);
        if (userDoc == 0)
        {
            var Name = await userContext.GetName(userId.userId);
            var NewDoc = new Document()
            {
                CompanyId = 0,
                ContentType = ContentType.Folder,
                Description = Name,
                DocumentNumber = userId.userId,
                DocumentNumberDataType = "$us",
                DocumentFormattedNumber = Name,
                DocumentStatus = DocumentStatus.Active,
                DocumentDate = DateTime.UtcNow,
                Icon = "fa fa-home",
                IconColor = "coral",
                Reserved = true,
                ExternalId = "$USER." + userId.userId + "$",
                FolderId = doc,
                Owner = userId.userId
            };
            await _repository.Create(NewDoc);
            userDoc = NewDoc.Id;
            if (CanHavePersonalFolder)
                await SetPermissions(NewDoc.Id, userId, userId.userId, ProfileType.User, new Dictionary<string, AuthorizationType>
                {
                    { PermissionType.CanView, AuthorizationType.Granted },
                    { PermissionType.CanViewContent, AuthorizationType.Granted },
                    { PermissionType.CanCreate, AuthorizationType.Granted },
                    { PermissionType.CanAddContent, AuthorizationType.Granted },
                    { PermissionType.CanRemoveContent, AuthorizationType.Granted },
                    { PermissionType.CanViewRegistry, AuthorizationType.Granted },
                });
            else
                await SetPermissions(NewDoc.Id, userId, userId.userId, ProfileType.User, new Dictionary<string, AuthorizationType>
                {
                    { PermissionType.CanView, AuthorizationType.Granted },
                    { PermissionType.CanViewContent, AuthorizationType.Granted }
                });
        }
        if (CanHavePersonalFolder)
        {
            var userDoc2 = await _repository.FindInFolderByUniqueId(null, "$USER." + userId.userId + ".STARRED$", userDoc, ContentType.Folder);
            if (userDoc2 == 0)
            {

                var starred = new Document()
                {
                    CompanyId = 0,
                    ContentType = ContentType.Folder,
                    Description = "I Miei Documenti",
                    DocumentStatus = DocumentStatus.Active,
                    ExternalId = "$USER." + userId.userId + ".STARRED$",
                    DocumentDate = DateTime.UtcNow,
                    Icon = "fa fa-star",
                    IconColor = "orange",
                    FolderId = userDoc,
                    Reserved = true,
                    Owner = userId.userId
                };
                await _repository.Create(starred);
                await SetPermissions(starred.Id, userId, userId.userId, ProfileType.User, new Dictionary<string, AuthorizationType>
                    {
                        { PermissionType.CanView, AuthorizationType.Granted },
                        { PermissionType.CanViewContent, AuthorizationType.Granted },
                        { PermissionType.CanCreate, AuthorizationType.Granted },
                        { PermissionType.CanAddContent, AuthorizationType.Granted },
                        { PermissionType.CanRemoveContent, AuthorizationType.Granted },
                        { PermissionType.CanShare, AuthorizationType.Granted },
                        { PermissionType.CanViewRegistry, AuthorizationType.Granted },
                        { PermissionType.CanAuthorize, AuthorizationType.Granted },
                    });
            }
        }
        return userDoc;
    }
    public async Task<Document> Get(int documentId)
    {
        if (documentId < 0)
            throw new KeyNotFoundException();
        // Recupero l'archivio dei fascicoli aziendali, padre di tutti i fascicoli
        Document doc = new Document();
        if (documentId == 0)
        {
            doc = new Document();
            doc.ContentType = ContentType.Folder;
            doc.DocumentTypeId = "";
            doc.Description = "Archivio Fascicoli";
            doc.Icon = "fa fa-building";
        }
        else
        {
            doc = await _repository.GetById(documentId);
            ThrowIfNotExists(doc, documentId);
        }
        return doc;
    }

    public async Task<DocumentInfo> Load(int documentId, UserProfile userInfo)
    {
        DocumentInfo f = new DocumentInfo();
        if (documentId == 0)
        {
            f = new DocumentInfo();
            f.ContentType = ContentType.Folder;
            f.DocumentTypeName = "Fascicolo Generico";
            f.Description = "Archivio Fascicoli";
            f.Icon = "fa fa-building";
        }
        else
        {
            var doc = await Get(documentId);
            f = await _Load(doc);
            f.Path = await FullPath(doc, userInfo);

        }
        return f;
    }



    private async Task<DocumentInfo> _Load(Document doc)
    {

        var config = new MapperConfiguration(cfg => cfg.CreateMap<Document, DocumentInfo>());
        Mapper mp = new Mapper(config);
        var d = mp.Map<DocumentInfo>(doc);
        //if (doc.ImageId.HasValue && d.Image == null)
        //    d.Image = await _repository.GetImage(doc.ImageId.Value);
        //else
        //    d.Image = null;

        d.Company = await _companyRepository.GetById(doc.CompanyId);
        d.DocumentType = await _documentTypeRepository.GetById(doc.DocumentTypeId);
        d.DocumentType.ContentType = doc.ContentType;
        d.DocumentTypeName = (string.IsNullOrEmpty(doc.DocumentTypeId) ? (doc.ContentType == ContentType.Document ? "Documento Generico" : doc.FolderId > 0 ? "Sotto-Fascicolo" : "Fascicolo Condiviso") : d.DocumentType.Name);
        d.DocumentTypeCategory = d.DocumentType.CategoryId;
        d.DocumentNumberFieldType = new FieldType() { Id = "", DataType = d.DocumentType.DocumentNumberDataType, Title = d.DocumentType.DocumentNumberLabel };// docType.DocumentNumberDataType;
        d.DocumentNumber = doc.DocumentNumber ?? "";
        d.DocumentNumberFormattedValue = doc.DocumentFormattedNumber;
        d.DocumentNumberLookupValue = d.DocumentNumber;
        FieldType NumberType = null;
        IDataTypeManager nfieldmanager = null;
        var n = doc.DocumentNumberDataType;
        if (!string.IsNullOrEmpty(doc.DocumentNumberDataType))
        {
            // decodifico il numero documento
            NumberType = await _dataTypeRepository.GetById(d.DocumentType.DocumentNumberDataType);
            if (NumberType == null) NumberType = new FieldType();
            nfieldmanager = await _dataTypeFactory.Instance(NumberType.DataType);
            d.DocumentNumberFieldType = NumberType;
            d.DocumentNumberFieldType.ControlType = nfieldmanager.ControlType;
            if (!nfieldmanager.IsCalculated)
            {
                List<string> lvalues = new List<string>();
                List<string> fvalues = new List<string>();
                if (d.DocumentNumber != null)
                {
                    foreach (var v in d.DocumentNumber.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                    {
                        var ftv = await nfieldmanager.Lookup(NumberType, v);
                        lvalues.Add(ftv.LookupValue);
                        fvalues.Add(ftv.FormattedValue);
                    }
                }
                d.DocumentNumberLookupValue = string.Join(",", lvalues);
                d.DocumentNumberFormattedValue = string.Join(",", fvalues);
            }
        };
        d.Description = (doc.Description ?? "").Replace("{DocumentFormattedNumber}", doc.DocumentFormattedNumber ?? "");
        var index = 0;
        d.FieldList = new List<DocumentFieldInfo>();
        if (d.DocumentType.Fields != null)
            foreach (var f in d.DocumentType.Fields.Where(t => !t.Deleted).OrderBy(t => t.FieldIndex))
            {
                index++;
                var ff = new DocumentFieldInfo();
                ff.FieldTypeId = f.FieldTypeId;
                ff.Value = f.DefaultValue + "";
                ff.LookupValue = "";
                ff.FormattedValue = "";
                ff.Encrypted = f.Encrypted;
                ff.FieldIdentifier = f.FieldName;
                ff.LastUpdate = DateTime.UtcNow;
                ff.LastUpdateUser = f.LastUpdateUser;
                ff.Title = f.Title;
                ff.FieldType = f.FieldType;
                //if (f.FieldIndex > index) index = f.FieldIndex;
                ff.FieldIndex = index; // f.FieldIndex;
                ff.Tag = f.Tag;
                d.FieldList.Add(ff);
            }
        Dictionary<string, FieldType> CachedFields = new Dictionary<string, FieldType>();
        foreach (var f in await _repository.GetFields(d.Id))
        {
            DocumentFieldInfo ff = !String.IsNullOrEmpty(f.FieldName) ? d.FieldList.Find(i => i.FieldIdentifier == f.FieldName) : null;
            if (ff == null) ff = d.FieldList.Find(i => i.FieldTypeId == f.FieldTypeId);

            if (ff == null)
            {
                ff = new DocumentFieldInfo();
                ff.FieldTypeId = f.FieldTypeId;
                ff.Encrypted = f.Encrypted;
                ff.FieldIdentifier = f.FieldName;
                ff.LastUpdate = f.LastUpdate;
                ff.LastUpdateUser = f.LastUpdateUser;
                ff.Customized = true;
                index++;
                ff.FieldIndex = index;
                if (CachedFields.ContainsKey(ff.FieldTypeId))
                {
                    ff.FieldType = CachedFields[ff.FieldTypeId];
                }
                else
                {
                    ff.FieldType = await _dataTypeRepository.GetById(ff.FieldTypeId);
                    CachedFields[ff.FieldTypeId] = ff.FieldType;
                }
                ff.Tag = f.Tag;
                d.FieldList.Add(ff);
            }
            if (string.IsNullOrEmpty(ff.FieldType.ColumnWidth))
                ff.FieldType.ColumnWidth = "100%";
            if (f.Blob != null)
            {
                ff.Value += f.Blob.Value;
            }
            else
            {
                ff.Value += f.Value;
            }
            ff.FormattedValue += f.FormattedValue;
            ff.LookupValue += f.LookupValue;
        }

        var ext = d.Image != null ? Path.GetExtension(d.Image.FileName) : "";

        var iindex = 0;
        switch (ext)
        {
            case ".doc":
            case ".odt":
            case ".docx":
                iindex = 1;
                break;
            case ".xls":
            case ".xlsx":
                iindex = 2;
                break;
            case ".ppt":
            case ".pptx":
                iindex = 3;
                break;
            case ".mp4":
            case ".ogg":
                iindex = 4;
                break;
            case ".wav":
                iindex = 5;
                break;
            case ".log":
            case ".xml":
            case ".txt":
            case ".htm":
            case ".html":
                iindex = 6;
                break;
            case ".pdf":
                iindex = 7;
                break;
            case ".bpmn":
                iindex = 8;
                break;
            case ".dmn":
                iindex = 9;
                break;
            case ".formio":
            case ".formjs":
            case ".formhtml":
                iindex = 10;
                break;
            case ".zip":
                iindex = 11;
                break;
            case ".msg":
            case ".eml":
                iindex = 12;
                break;
            case ".bmp":
            case ".jpg":
            case ".jpeg":
            case ".png":
            case ".tiff":
                iindex = 15;
                break;
            case "":
                iindex = 13;
                break;

            default:
                break;
        }

        if (string.IsNullOrEmpty(d.Icon))
        {
            if (d.ContentType == ContentType.Folder)
            {
                iindex = 14;
            }
            else
            if (d.ContentType == ContentType.Workflow)
            {
                iindex = 15;
            }
            d.Icon = FileTypeIcons[iindex];
        }

        if (string.IsNullOrEmpty(d.IconColor))
        {
            if (d.DocumentType != null && d.DocumentType.Id == "$D")
                d.IconColor = "var(--primary-fg-02)";
            else
                d.IconColor = FileTypeColors[iindex];
        }

        d.RecipientList = new List<DocumentRecipientInfo>();
        foreach (var DR in await _repository.GetRecipients(d.Id))
        {
            string Name = ((int)DR.ProfileType).ToString() + DR.ProfileId;
            if (DR.ProfileType == ProfileType.MailAddress)
            {
                var contact = await userContext.GetDigitalAddressById(int.Parse(DR.ProfileId));
                if (contact != null)
                    Name = contact.Name + " <" + contact.Address + ">";
            }
            else
                Name = "#" + Name;
            DocumentRecipientInfo R = new DocumentRecipientInfo(DR, Name);
            d.RecipientList.Add(R);
        }
        foreach (var r in doc.Referents.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            var l = new LookupTable() { Id = r, Description = await userContext.GetProfileName(r) };
            d.ReferentList.Add(l);
        }
        foreach (var r in doc.ReferentsCC.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            var l = new LookupTable() { Id = r, Description = await userContext.GetProfileName(r) };
            d.ReferentListCC.Add(l);
        }
        //d.Path = await FullPath(doc, userInfo);

        if (!String.IsNullOrEmpty(doc.ProtocolNumber))
        {
            d.Protocol = new ProtocolInfo();
            d.Protocol.Number = doc.ProtocolNumber;
            d.Protocol.ExternalProtocolURL = doc.ExternalProtocolURL;
            d.Protocol.Date = doc.ProtocolDate?.Date ?? DateTime.MinValue;
        }

        return d;
    }

    private static List<string> FileTypeIcons = new()
        {
            "fa fa-file",   //0
            "fa fa-file-word-o",
            "fa fa-file-excel-o",
            "fa fa-file-powerpoint-o",
            "fa fa-film",
            "fa fa-file-audio-o",   // 5
            "fa fa-file-text-o",
            "fa fa-file-pdf-o",
            "fa fa-cogs",
            "fa fa-question-circle",
            "fa fa-table",   // 10
            "fa fa-file-archive-o",
            "fa fa-envelope",
            "fa fa-warning",
            "fa fa-folder",
            "fa fa-image",   // 15
            "fa fa-edit"
        };

    private static List<string> FileTypeColors = new()
        {
            "#aab",
            "skyblue",
            "limegreen",
            "coral",
            "firebrick",
            "crimson",
            "#aab",
            "firebrick",
            "#4bf",
            "#4bf",
            "#777",
            "#aab",
            "#4bf",
            "orange",
            "orange",
            "brown",
            "gray"
        };

    public async Task<DocumentField> AddField(DocumentInfo document, DocumentTypeField DocumentTypeField, string DefaultValue)
    {
        if (DocumentTypeField.FieldTypeId == "$$i" || DocumentTypeField.FieldTypeId == "$$n")
            DefaultValue = DefaultValue.ToString().Replace(".", "").Replace(",", ".");

        var Field = new DocumentField()
        {
            Customized = true,
            DocumentId = document.Id,
            DocumentTypeId = DocumentTypeField.DocumentTypeId,
            FieldTypeId = DocumentTypeField.FieldTypeId,
            FieldIndex = DocumentTypeField.FieldIndex,
            FieldName = DocumentTypeField.FieldName,
            Value = DefaultValue
        };

        string fieldType = Field.FieldTypeId;
        FieldType M = null;
        if (fieldType != null && metacache.ContainsKey(fieldType))
            M = metacache[fieldType];
        else
        {
            M = await _dataTypeRepository.GetById(fieldType);
            if (M == null)
            {
                M = new FieldType()
                {
                    DataType = "$$t"
                };
            }
            if (fieldType != null)
                metacache[fieldType] = M;
        }




        var fieldmanager = await _dataTypeFactory.Instance(M.DataType);
        if (!fieldmanager.IsCalculated)
        {
            List<string> lvalues = new List<string>();
            List<string> fvalues = new List<string>();
            List<string> values = new List<string>();
            if (Field.Value != null)
                foreach (var v in Field.Value.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    var ftv = await fieldmanager.Lookup(M, v);
                    values.Add(ftv.Value);
                    lvalues.Add(ftv.LookupValue);
                    fvalues.Add(ftv.FormattedValue);
                }
            var value = string.Join(",", values);
            Field.Value = value;
            Field.LookupValue = string.Join(",", lvalues);
            Field.FormattedValue = string.Join(",", fvalues);
        }
        await _repository.AddField(Field);
        return Field;
    }
    public async Task<DocumentField> UpdateField(DocumentInfo document, DocumentTypeField DocumentTypeField, string Value)
    {
        if (Value == null) Value = "";
        if (DocumentTypeField.FieldTypeId == "$$i" || DocumentTypeField.FieldTypeId == "$$n")
            Value = Value.ToString().Replace(".", "").Replace(",", ".");
        var fields = await _repository.GetFields(document.Id);
        var Field = fields == null ? null : fields.FirstOrDefault(f => f.FieldName != null && f.FieldName.Equals(DocumentTypeField.FieldName, StringComparison.InvariantCultureIgnoreCase));
        var isNew = (Field == null);
        if (isNew)
        {
            Field = new DocumentField()
            {
                Customized = true,
                DocumentId = document.Id,
                DocumentTypeId = DocumentTypeField.DocumentTypeId,
                FieldTypeId = DocumentTypeField.FieldTypeId,
                FieldIndex = DocumentTypeField.FieldIndex,
                FieldName = DocumentTypeField.FieldName,
                Value = Value
            };
        }
        string fieldType = Field.FieldTypeId;
        FieldType M = null;
        if (fieldType != null && metacache.ContainsKey(fieldType))
            M = metacache[fieldType];
        else
        {
            M = await _dataTypeRepository.GetById(fieldType);
            if (M == null)
            {
                M = new FieldType()
                {
                    DataType = "$$t"
                };
            }
            if (fieldType != null)
                metacache[fieldType] = M;
        }
        var fieldmanager = await _dataTypeFactory.Instance(M.DataType);
        if (!fieldmanager.IsCalculated)
        {
            List<string> lvalues = new List<string>();
            List<string> fvalues = new List<string>();
            List<string> values = new List<string>();
            if (Field.Value != null)
                foreach (var v in Value?.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    var ftv = await fieldmanager.Lookup(M, v);
                    values.Add(ftv.Value);
                    lvalues.Add(ftv.LookupValue);
                    fvalues.Add(ftv.FormattedValue);
                }
            var value = string.Join(",", values);
            Field.Value = value;
            Field.LookupValue = string.Join(",", lvalues);
            Field.FormattedValue = string.Join(",", fvalues);
            if (Field.FormattedValue.Length > 255) Field.FormattedValue = Field.FormattedValue.Substring(0, 255);
            if (Field.LookupValue.Length > 255) Field.LookupValue = Field.LookupValue.Substring(0, 255);
        }
        if (isNew)
        {
            if (fieldmanager.IsBlob)
            {
                Field.Blob = new()
                {
                    Document = Field.Document,
                    DocumentType = Field.DocumentType,
                    DocumentId = Field.DocumentId,
                    DocumentTypeId = Field.DocumentTypeId,
                    FieldIndex = Field.FieldIndex,
                    FieldName = Field.FieldName,
                    FieldTypeId = Field.FieldTypeId,
                    LastUpdate = Field.LastUpdate,
                    LastUpdateUser = Field.LastUpdateUser,
                    Value = Field.Value
                };
                Field.Value = "";
            }
            await _repository.AddField(Field);
        }
        else
        {
            if (fieldmanager.IsBlob)
            {
                Field.Value = "";
            }
            else
            {
                if (Field.Value.Length > 255) Field.Value = Field.Value.Substring(0, 255);
            }
            await _repository.UpdateField(Field);
        }
        return Field;
    }

    public async Task<FolderExportModel> GetFolderContentRecursive(DocumentInfo folder, UserProfile user)
    {
        var folderContent = new FolderExportModel
        {
            DocumentInfo = folder

        };
        byte[] byteArrayContent = Array.Empty<byte>();
        FileContent fileContent = new FileContent();

        // Recupera gli ID dei documenti contenuti nel fascicolo
        var documentIds = await GetFolderDocuments(folder.Id, user);

        foreach (var documentId in documentIds)
        {
            // Carica ogni documento
            var document = await Get(documentId);
            var documentInfo = await Load(documentId, user);

            if (document.DocumentStatus == DocumentStatus.Deleted)
                continue;

            // Se il documento è un fascicolo, recupera il suo contenuto ricorsivamente
            if (document.ContentType == ContentType.Folder)
            {
                var subFolderContent = await GetFolderContentRecursive(documentInfo, user);
                folderContent.SubDocuments.Add(subFolderContent);

                if (document.Image != null)
                {
                    if (!byteArrayContent.IsNullOrEmpty())
                    {
                        var imageBase64 = Convert.ToBase64String(byteArrayContent);

                        fileContent.DataIsInBase64 = true;
                        fileContent.FileData = imageBase64;
                        fileContent.FileName = document.Image.FileName;
                        fileContent.LinkToContent = true;
                    }
                }
            }
            else
            {
                if (document.Image != null)
                {
                    byteArrayContent = await GetContent(document.Image.Id);

                    if (!byteArrayContent.IsNullOrEmpty())
                    {
                        var imageBase64 = Convert.ToBase64String(byteArrayContent);

                        fileContent.DataIsInBase64 = true;
                        fileContent.FileData = imageBase64;
                        fileContent.FileName = document.Image.FileName;
                        fileContent.LinkToContent = true;
                    }
                }

                folderContent.SubDocuments.Add(new FolderExportModel
                {
                    DocumentInfo = documentInfo,
                    Content = fileContent,
                    Permissions = await GetDocumentPermissions(documentId),
                    Links = await Links(documentId, user, false),
                    Attachments = await Links(documentId, user, true)
                });
            }
        }

        folderContent.Permissions = await GetDocumentPermissions(folder.Id);
        folderContent.Links = await Links(folder.Id, user, false);
        folderContent.Attachments = await Links(folder.Id, user, true);

        return folderContent;
    }

    public async Task ProcessImportedFolderFirstPass(int rootFolderId, FolderExportModel folderExportModel, UserProfile user, Dictionary<int, int> documentIdMap)
    {
        var document = folderExportModel.DocumentInfo;

        var create = new CreateOrUpdateDocument
        {
            CompanyId = document.Company.Id,
            ExternalId = string.Empty,
            ContentType = document.ContentType,
            DocumentTypeId = document.DocumentType.Id,
            Owner = document.Owner,
            ACLId = document.ACLId,
            MasterDocumentId = document.MasterDocumentId,
            Status = document.DocumentStatus,
            IconColor = document.IconColor.Equals("firebrick", StringComparison.OrdinalIgnoreCase) ? "#B22222" : document.IconColor,
            Icon = document.Icon,
            Description = document.Description,
            DocumentNumber = document.DocumentNumber,
            DocumentDate = document.DocumentDate,
            ExpirationDate = document.ExpirationDate,
            FolderId = rootFolderId,
            ReferentList = document.ReferentList.IsNullOrEmpty() ? string.Empty : string.Join(", ", document.ReferentList.Select(referent => referent.Id)),
            ReferentListCC = document.ReferentListCC.IsNullOrEmpty() ? string.Empty : string.Join(", ", document.ReferentListCC.Select(referent => referent.Id)),
            Reserved = document.Reserved,
            PersonalData = document.PersonalData,
            Authorize = folderExportModel.Permissions.IsNullOrEmpty() ? string.Empty : string.Join(", ", folderExportModel.Permissions.Select(referent => referent.Profile)),
            FieldList = document.FieldList.Select(field => new AddOrUpdateDocumentField
            {
                FieldIndex = field.FieldIndex,
                FieldName = field.FieldIdentifier,
                FieldTypeId = field.FieldTypeId,
                Value = field.Value
            }).ToList(),
            Content = folderExportModel.Content,
        };

        int newDocumentId = await Create(create, user);

        // Salva il mapping tra Id e newDocumentId
        documentIdMap[document.Id] = newDocumentId;

        // Processa i sotto-documenti ricorsivamente
        foreach (var subDocument in folderExportModel.SubDocuments)
        {
            await ProcessImportedFolderFirstPass(newDocumentId, subDocument, user, documentIdMap);
        }
    }

    public async Task ProcessImportedFolderSecondPass(FolderExportModel folderExportModel, UserProfile user, Dictionary<int, int> documentIdMap)
    {
        var documentId = documentIdMap[folderExportModel.DocumentInfo.Id];

        // Prepara l'oggetto di aggiornamento per link e allegati
        var update = new UpdateDocumentLinksAndAttachments
        {
            DocumentId = documentId,
            Attachments = folderExportModel.Attachments
                .Select(a => documentIdMap.ContainsKey(a.Id) ? documentIdMap[a.Id] : a.Id)
                .ToArray(),

            LinkTo = folderExportModel.Links
                .Select(l => documentIdMap.ContainsKey(l.Id) ? documentIdMap[l.Id] : l.Id)
                .ToArray()
        };

        // Aggiungi gli allegati
        foreach (var attachmentId in update.Attachments)
        {
            try
            {
                await AddLink(documentId, attachmentId, user, IsAttachment: true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante l'aggiunta dell'allegato {attachmentId} al documento {documentId}: {ex.Message}");
            }
        }

        // Aggiungi i link
        foreach (var linkId in update.LinkTo)
        {
            try
            {
                await AddLink(documentId, linkId, user, IsAttachment: false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante l'aggiunta del link {linkId} al documento {documentId}: {ex.Message}");
            }
        }

        // Processa i sotto-documenti ricorsivamente
        foreach (var subDocument in folderExportModel.SubDocuments)
        {
            await ProcessImportedFolderSecondPass(subDocument, user, documentIdMap);
        }
    }

    public async Task<int> Create(CreateOrUpdateDocument document, UserProfile userInfo)
    {
        var doc = await CreateAndRead(document, userInfo);
        return doc == null || doc.Id < 0 ? 0 : doc.Id;
    }

    public async Task<DocumentInfo> CreateAndRead(CreateOrUpdateDocument document, UserProfile userInfo)
    {
        //var Id = await Create(document, userInfo);
        //return Id > 0 ? await Load(Id, userInfo) : null;
        string UserId = userInfo.userId.ToLower();
        if (document.CompanyId <= 0) document.CompanyId = 1;
        // Validate Input
        {
            var company = await _companyRepository.GetById(document.CompanyId);
            if (company == null)
            {
                logger.LogError("CompanyId non valida");
                throw new ArgumentNullException("Company non valida");
            }
        }
        if (document.ExpirationDate.HasValue && document.DocumentDate.HasValue && document.ExpirationDate.Value < document.DocumentDate.Value)
        {
            logger.LogError("La data di scadenza non può essere antecedente alla data del documento");
            throw new InvalidDataException("La data di scadenza non può essere antecedente alla data del documento");
        }
        DocumentType tp = new DocumentType() { Id = null };
        if (!String.IsNullOrEmpty(document.DocumentTypeId))
        {
            tp = await _documentTypeRepository.GetById(document.DocumentTypeId);
            if (tp == null)
            {
                logger.LogError("Tipologia documentale non trovata");
                throw new ArgumentNullException("Tipologia documentale non trovata");
            }
        }
        if (!document.ExpirationDate.HasValue)
            document.ExpirationDate = DateTime.MaxValue;

        //TODO: Se ExternalId NELLA TIPOLOGIA NON E' VUOTO lo compongo.
        //if (string.IsNullOrEmpty(document.ExternalId))
        //{
        //    document.ExternalId = document.DocumentTypeId.Replace("$","") + "-" + (document.DocumentDate?.Year.ToString() ?? DateTime.UtcNow.Year.ToString()) + (String.IsNullOrEmpty(document.DocumentNumber) ? "" : "-" + document.DocumentNumber);
        //}

        if (!string.IsNullOrEmpty(document.ExternalId))
        {
            var doc = await _repository.FindByUniqueId(tp.Id, document.ExternalId, document.ContentType);
            if (doc > 0)
            {
                if (!document.FailIfExists)
                {
                    var updatedDoc = await Update(doc, document, userInfo);
                    return updatedDoc;
                }
                else
                {
                    logger.LogError($"Il documento #{document.ExternalId} è già presente in archivio con Id={doc}");
                    throw new DuplicateDocumentException(doc, document.ExternalId);
                }
            }
        }


        Document folder = null;
        if (document.FolderId.HasValue && document.FolderId.Value != 0)
        {
            var fid = document.FolderId.Value;
            if (fid < 0)
            {
                fid = await GetUserFolder(await userContext.GetUserProfile(document.Owner));
                document.FolderId = fid;
            }
            if (fid > 0)
            {
                folder = await _repository.GetById(fid);
                if (folder == null || folder.ContentType != ContentType.Folder)
                    throw new KeyNotFoundException($"Fascicolo #{fid} non trovato");
            }
        }
        var config = new MapperConfiguration(cfg => cfg.CreateMap<CreateOrUpdateDocument, Document>());
        Mapper mp = new Mapper(config);
        var D = mp.Map<Document>(document);
        if (String.IsNullOrEmpty(D.Owner))
            D.Owner = userInfo.userId;
        D.DocumentStatus = document.Status;
        if ((int)D.DocumentStatus == 0) D.DocumentStatus = DocumentStatus.Active;

        if (String.IsNullOrEmpty(document.ACLId))
            D.ACLId = document.ACLId;
        await SynchronizeDocumentTypeSchema(D, tp);

        foreach (var f in document.FieldList)
        {
            await UpdateField(D, f, userInfo);
        }
        await LookupFields(D);

        AuthorizationType p = AuthorizationType.None;
        if (D.DocumentTypeId is null)
        {

            if (folder != null)
            {
                // verifico se ho il permesso di inserimento contenuti sul fascicolo padre
                p = await GetAuthorization(folder, userInfo, PermissionType.CanAddContent);
            }
            else
            {
                p = AuthorizationType.Granted;
            }
            if (p == AuthorizationType.Granted)
            {
                if (userInfo.userId == SpecialUser.SystemUser || userInfo.IsService)
                    p = AuthorizationType.Granted;
                else
                {
                    // Verifico se ho i permessi per creare un documento generico o un fascicolo condiviso
                    if (D.ContentType == ContentType.Document || folder != null)
                    {
                        p = (await _aclRepository.GetAuthorization("", userInfo, PermissionType.Profile_CanCreateGenericDocument));
                    }
                    else
                    {
                        p = (await _aclRepository.GetAuthorization("", userInfo, PermissionType.Profile_CanCreateRootFolder));
                    }
                }
            }
            else
            {
                // Non sono autorizzato ad inserire il documento nel fascicolo indicato
                // Ignoro il fascicolo e proseguo
                if (folder != null)
                {
                    folder = null;
                    p = AuthorizationType.Granted;
                }
            }
        }
        else
            // Verifico i permessi di creazione per la tipologia indicata
            p = await GetAuthorization(D, userInfo, PermissionType.CanCreate);
        var ok = p == AuthorizationType.Granted; // posso creare un nuovo documento
        if (!ok)
        {
            var Errori = UserId + " non è autorizzato a creare documenti di tipologia: " + tp.Name;
            logger.LogError(Errori);
            throw new UnauthorizedAccessException(Errori);
        }

        // Aggiungo i referenti
        List<string> refs = new List<string>();
        List<string> refpcs = new List<string>();
        foreach (var r in document.ReferentList.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            ProfileType ptype = (ProfileType)int.Parse(r.Substring(0, 1));
            var pid = r.Substring(1);
            if (pid.StartsWith("@") && ptype == ProfileType.Group)
            {
                var Group = await userGroupRepo.GetByExternalId(pid.Substring(1));
                if (Group != null) pid = Group.Id;
            }
            if (pid.StartsWith("@") && ptype == ProfileType.Role)
            {
                var role = await roleRepository.GetByName(pid.Substring(1));
                if (role != null) pid = role.Id;
            }


            if (refs.IndexOf(r.Substring(0, 1) + pid) < 0)
            {
                refs.Add(r.Substring(0, 1) + pid);
            }
        }
        foreach (var r in document.ReferentListCC.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            ProfileType ptype = (ProfileType)int.Parse(r.Substring(0, 1));
            var pid = r.Substring(1);
            if (pid.StartsWith("@") && ptype == ProfileType.Group)
            {
                var Group = await userGroupRepo.GetByExternalId(pid.Substring(1));
                if (Group != null) pid = Group.Id;
            }
            if (pid.StartsWith("@") && ptype == ProfileType.Role)
            {
                var role = await roleRepository.GetByName(pid.Substring(1));
                if (role != null) pid = role.Id;
            }
            if (refs.IndexOf(r.Substring(0, 1) + pid) < 0 && refpcs.IndexOf(r.Substring(0, 1) + pid) < 0)
            {
                refpcs.Add(r.Substring(0, 1) + pid);
            }
        }
        D.Referents = string.Join(",", refs);
        D.ReferentsCC = string.Join(",", refpcs);
        try
        {
            using (var T = _repository.BeginTransaction())
            {
                try
                {
                    await _repository.Create(D);
                    await Authorize(D, userInfo, D.Owner);
                    if (!D.Owner.Equals(UserId))
                        await Authorize(D, userInfo, UserId);
                    await SetParentFolderPermissions(D, userInfo);
                    if (document.Authorize != null)
                        foreach (var a in document.Authorize.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                        {
                            ProfileType ptype = (ProfileType)int.Parse(a.Substring(0, 1));
                            string pid = a.Substring(1);
                            if (pid.StartsWith("@") && ptype == ProfileType.Group)
                            {
                                var Group = await userGroupRepo.GetByExternalId(pid.Substring(1));
                                if (Group != null) pid = Group.Id;
                            }
                            if (pid.StartsWith("@") && ptype == ProfileType.Role)
                            {
                                var role = await roleRepository.GetByName(pid.Substring(1));
                                if (role != null) pid = role.Id;
                            }
                            await SetPermissions(D.Id, userInfo, pid, ptype, new() {
                                { PermissionType.CanView, AuthorizationType.Granted },
                                { PermissionType.CanViewContent, AuthorizationType.Granted }
                            });
                        }
                    if (document.Attachments != null)
                        foreach (var l in document.Attachments)
                        {
                            await _repository.AddLink(D.Id, l, UserId, true);
                            if (folder != null)
                                await _repository.AddToFolder(D.Id, folder.Id, UserId, false);
                        }

                    if (document.LinkTo != null)
                        foreach (var l in document.LinkTo)
                            await _repository.AddLink(D.Id, l, UserId, false);

                    if (document.AttachTo != null)
                        foreach (var l in document.AttachTo)
                            await _repository.AddLink(l, D.Id, UserId, true);

                    await T.CommitAsync();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Document.Create");
                    await T.RollbackAsync();
                }
            };
            if (D.Id > 0)
            {
                var Doc = await _Load(D);

                if (_eventDispatcher != null)
                {
                    if (document.ProcessVariables == null)
                        document.ProcessVariables = new Dictionary<string, object>();

                    // Se creo un documento in bozza, l'evento inziale è differente
                    if (document.Status == DocumentStatus.Draft)
                    {
                        await _eventDispatcher.Publish(new DocumentEventMessage(Doc, userInfo, EventType.CreationAsDraft, document.ProcessVariables));
                    }
                    else
                    {
                        await _eventDispatcher.Publish(new DocumentEventMessage(Doc, userInfo, EventType.Creation, document.ProcessVariables));
                    }
                }
                if (folder != null)
                {
                    if (D.ContentType != ContentType.Folder)
                        await AddToFolder(folder.Id, new() { D.Id }, userInfo, false);
                }

                //                if (document.Content == null || string.IsNullOrEmpty(document.Content.FileData))
                //                    await AddDefaultContent(D, tp, userInfo);
                //                else

                await AdjustFoldersDeadLine(D, userInfo);

                if (document.Content != null && !string.IsNullOrEmpty(document.Content.FileData))
                {
                    var InputExtension = Path.GetExtension(document.Content.FileName).ToLower();
                    var ShouldBeconverted = (D.DocumentStatus == DocumentStatus.Active && tp.ConvertToPDF && (InputExtension != ".pdf"));
                    Doc.Image = await _AddContent(D, tp, userInfo, document.Content, !ShouldBeconverted);

                    if (ShouldBeconverted)
                    {

                        var converter = await fileConvertFactory.Get(InputExtension, ".pdf");
                        if (converter != null)
                        {
                            using (var src = new MemoryStream(Encoding.Default.GetBytes(document.Content.FileData)))
                            {
                                try
                                {
                                    var dest = await converter.Convert(InputExtension, src);
                                    if (dest != null)
                                    {
                                        var outputdata = ((MemoryStream)dest).ToArray();
                                        document.Content.FileData = Convert.ToBase64String(outputdata);
                                        document.Content.FileName = Path.ChangeExtension(document.Content.FileName, ".pdf");
                                        document.Content.DataIsInBase64 = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }

                        }
                        Doc.Image = await _AddContent(D, tp, userInfo, document.Content);
                    }
                }
                if (document.Status == DocumentStatus.Active)
                {
                    await Notify(D, UserId, refs, refpcs, NotificationConstants.CONST_TEMPLATE_REFERENT, userInfo, ActionRequestType.None);

                    var template = NotificationConstants.CONST_TEMPLATE_NOTIFY;
                    if (!String.IsNullOrEmpty(document.NotificationTemplate)) template = document.NotificationTemplate;
                    await Notify(D, UserId, document.NotifyTo.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList(), document.NotifyCC.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList(), template, userInfo);
                }
                return Doc;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Document.Create", document);
            throw;
        }
        return null;
    }




    public async Task<DocumentInfo> ChangeType(int documentId, string newTypeId, UserProfile userInfo)
    {
        string UserId = userInfo.userId.ToLower();
        DocumentType tp = new DocumentType() { Id = "" };
        if (!String.IsNullOrEmpty(newTypeId))
        {
            tp = await _documentTypeRepository.GetById(newTypeId);
            if (tp == null)
                throw new InvalidDataException("Tipologia documentale non trovata");
        }
        Document D = await _repository.GetById(documentId);
        var pe = await GetAuthorization(D, userInfo, PermissionType.CanEdit);
        var ok = pe == AuthorizationType.Granted; // posso editare il documento ?
        if (!ok)
        {
            var Errori = UserId + " non è autorizzato a modificare questo documento";
            logger.LogError(Errori);
            throw new UnauthorizedAccessException(Errori);
        }
        //        if (D.ProtocolRegister != tp.ProtocolRegister)
        //            throw new InvalidDataException("La tipologia selezionata è associata ad un registro di protocollo differente");

        D.Fields = await _repository.GetFields(documentId);
        D.Icon = tp.Icon;
        D.IconColor = tp.IconColor;

        Dictionary<string, string> Changed = new Dictionary<string, string>();
        await SynchronizeDocumentTypeSchema(D, tp, Changed);

        if (Changed.Count > 0)
        {
            // Verifico i permessi sulla nuova tipologia
            var p = await GetAuthorization(D, userInfo, PermissionType.CanCreate);
            ok = p == AuthorizationType.Granted; // posso creare un nuovo documento
            if (!ok)
            {
                var Errori = UserId + " non è autorizzato a creare documenti di tipologia: " + tp.Name;
                logger.LogError(Errori);
                throw new UnauthorizedAccessException(Errori);
            }

            try
            {
                await _repository.Update(D);
                await _repository.SaveFields(D.Id, D.Fields);
                //           await docRepo.SaveContacts(D.Id, Contacts);
                await _repository.SaveChanges();
                var Doc = await _Load(D);

                if (_eventDispatcher != null) await _eventDispatcher.Publish(new DocumentEventMessage(Doc, userInfo, EventType.Update, new Dictionary<string, object> { { "PropertiesChanged", Changed } }));

                await AdjustFoldersDeadLine(D, userInfo);
                return Doc;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Document.ChangeType", D);
                throw;
            }
        }
        return null;
    }

    public async Task<DocumentInfo> Protocol(int documentId, ProtocolInfo info, UserProfile userInfo)
    {
        Document D = await _repository.GetById(documentId);
        ThrowIfNotExists(D, documentId);
        D.ProtocolCustomProperties = "";
        D.ProtocolDate = info.Date;
        D.ProtocolStatus = JobStatus.Completed;
        D.ProtocolNumber = info.Number;
        D.ProtocolCustomProperties = info.Register;
        D.Image.PreviewStatus = JobStatus.Queued;
        D.ExternalProtocolUid = info.ProtocolUser;
        D.ExternalProtocolURL = info.ExternalProtocolURL;
        try
        {
            await _repository.Update(D);
            await Authorize(D, userInfo, userInfo.userId);
            await _repository.SaveChanges();

            if (D.Id > 0)
            {
                var Doc = await _Load(D);
                await UpdatePreviewStatus(D.Image.Id, Domain.Enumerators.JobStatus.Queued, UserProfile.SystemUser());
                if (_eventDispatcher != null) await _eventDispatcher.Publish(new DocumentEventMessage(Doc, userInfo, EventType.Protocol, new Dictionary<string, object> { { "Protocol", info } }));
                return Doc;
            }
        }
        catch (Exception)
        {
            throw;
        }
        throw new KeyNotFoundException();
    }


    public async Task<DocumentInfo> Update(int documentId, CreateOrUpdateDocument document, UserProfile userInfo)
    {
        Dictionary<string, string> Changed = new Dictionary<string, string>();
        string UserId = userInfo.userId.ToLower();
        {
            var company = await _companyRepository.GetById(document.CompanyId);
            if (company == null) throw new ArgumentNullException("Company non valida");
        }
        DocumentType tp = new DocumentType() { Id = null };
        if (!String.IsNullOrEmpty(document.DocumentTypeId))
        {
            tp = await _documentTypeRepository.GetById(document.DocumentTypeId);
            if (tp == null)
                throw new KeyNotFoundException("Tipologia documentale non trovata");
        }
        if (document.ExpirationDate.HasValue && document.DocumentDate.HasValue && document.ExpirationDate.Value < document.DocumentDate.Value)
            throw new InvalidDataException("La data di scadenza non può essere antecedente alla data del documento");
        Document newFolder = null;

        Document D = await _repository.GetById(documentId);
        if ((D.ExternalId != document.ExternalId))
        {
            if (!string.IsNullOrEmpty(document.ExternalId))
            {
                var doc = await _repository.FindByUniqueId(tp.Id, document.ExternalId, document.ContentType);
                if (doc > 0 && doc != documentId)
                {
                    if (document.FailIfExists)
                    {
                        logger.LogError($"Il documento #{document.ExternalId} è già presente in archivio con Id={doc}");
                        throw new DuplicateDocumentException(doc, document.ExternalId);
                    }
                    //                    else
                    //                    {
                    //                        documentId = doc;
                    //                        D = await _repository.GetById(documentId);
                    //                    }
                }
            }
            Changed.Add("ID Univoco Globale", D.ExternalId);
            D.ExternalId = document.ExternalId;
        }

        Document folder = null;
        if (document.FolderId.HasValue && document.FolderId.Value != 0)
        {
            var fid = document.FolderId.Value;
            if (fid < 0)
            {
                fid = await GetUserFolder(await userContext.GetUserProfile(document.Owner));
                document.FolderId = fid;
            }
            if (fid > 0)
            {
                folder = await _repository.GetById(fid);
                if (folder == null || folder.ContentType != ContentType.Folder) throw new KeyNotFoundException($"Fascicolo #{fid} non trovato");
            }
        }

        if (document.MasterDocumentId != D.MasterDocumentId)
            throw new InvalidDataException("Non è possibile modificare il riferimento al documento originale");

        if (document.DocumentTypeId != null && tp.Id != null && document.DocumentTypeId.ToLower() != tp.Id.ToLower())
            throw new InvalidDataException("Non è possibile modificare la tipologia del documento");
        if (document.DocumentTypeId == null)
        {
            document.DocumentTypeId = D.DocumentTypeId;
            tp = await _documentTypeRepository.GetById(D.DocumentTypeId);
        }
        var fieldsOnStorage = await _repository.GetFields(documentId);
        D.Fields = fieldsOnStorage;
        D.Icon = document.Icon;
        D.IconColor = document.IconColor + "";
        //       var oldValues = new string[Math.Max(NTp.FieldCount, nd.Fields.Count)];

        var roles = userInfo.Roles;
        var pe = (await GetAuthorization(D, userInfo, PermissionType.CanEdit));
        var oldStatus = (D.DocumentStatus);
        var IsDraft = (D.DocumentStatus == DocumentStatus.Draft) && (string.Compare(userInfo.userId, D.Owner, true) == 0);
        // permetto la modifica del documento se sto caricando un file proveniente da form e ho il permesso per aggiornare l'immagine
        var CanAddContent = await GetAuthorization(D, userInfo, PermissionType.CanAddContent);
        var IsForm = (!String.IsNullOrEmpty(tp.CreationFormKey)) && (CanAddContent==AuthorizationType.Granted) && (document.Content != null) && (document.Content?.FileData?.Length > 0);
        var ok = (IsForm) || (IsDraft) || pe == AuthorizationType.Granted; // posso editare il documento ?
        if (!ok)
        {
            var Errori = UserId + " non è autorizzato a modificare questo documento";
            logger.LogError(Errori);
            throw new UnauthorizedAccessException(Errori);
        }
        bool DocumentNumberTypeChanged = (D.DocumentNumberDataType + "") != (tp.DocumentNumberDataType + "");
        if (String.IsNullOrEmpty(document.ACLId))
            D.ACLId = document.ACLId;

        await SynchronizeDocumentTypeSchema(D, tp, Changed);

        foreach (var f in document.FieldList)
        {
            await UpdateField(D, f, userInfo, Changed);
        }
        if (D.DocumentNumber != document.DocumentNumber || DocumentNumberTypeChanged)
        {
            Changed.Add(string.IsNullOrEmpty(tp.DocumentNumberLabel) ? "Nr.Documento" : tp.DocumentNumberLabel, D.DocumentFormattedNumber);
            D.DocumentNumber = document.DocumentNumber;
            DocumentNumberTypeChanged = true;
        }
        if (D.DocumentStatus != document.Status)
        {
            Changed.Add("Stato", document.Status.ToString());
            D.DocumentStatus = document.Status;
        }
        await LookupFields(D);

        // Verifico i campi modificati

        if (D.Description != document.Description)
        {
            Changed.Add(string.IsNullOrEmpty(tp.DescriptionLabel) ? "Descrizione" : tp.DescriptionLabel, D.Description);
            D.Description = document.Description;
        }
        DateTime oldDS = D.ExpirationDate.HasValue ? D.ExpirationDate.Value : DateTime.MinValue;
        DateTime newDS = document.ExpirationDate.HasValue ? document.ExpirationDate.Value : DateTime.MinValue;
        if (oldDS != newDS)
        {
            var label = "Data Scadenza";
            if (Changed.ContainsKey(label)) Changed.Remove(label);
            if (D.ExpirationDate.HasValue)
                Changed.Add(label, D.ExpirationDate.Value.ToString("dd/MM/yyyy"));
            else
                Changed.Add(label, "");
            if (document.ExpirationDate.HasValue)
                D.ExpirationDate = document.ExpirationDate;
            else
                D.ExpirationDate = null;
        }


        DateTime oldDD = D.DocumentDate.HasValue ? D.DocumentDate.Value : DateTime.MinValue;
        DateTime newDD = document.DocumentDate.HasValue ? document.DocumentDate.Value : DateTime.MinValue;
        if (oldDD != newDD)
        {
            var label = string.IsNullOrEmpty(tp.DocumentDateLabel) ? "Data Documento" : tp.DocumentDateLabel;
            if (D.DocumentDate.HasValue)
                Changed.Add(label, D.DocumentDate.Value.ToString("dd/MM/yyyy"));
            else
                Changed.Add(label, "");
            if (document.DocumentDate.HasValue)
                D.DocumentDate = document.DocumentDate;
            else
                D.DocumentDate = null;
        }
        if (D.CompanyId != document.CompanyId)
        {
            Changed.Add("ID Ambiente", D.CompanyId.ToString());
            D.CompanyId = document.CompanyId;
        }
        bool FolderChanged = folder != null && (D.FolderId != folder.Id);
        if (FolderChanged)
        {
            var fid = D.FolderId.Value;
            if (fid > 0)
            {
                newFolder = await _repository.GetById(fid);
                if (newFolder == null || newFolder.ContentType != ContentType.Folder) throw new KeyNotFoundException($"Fascicolo #{fid} non trovato");
                Changed.Add("Fascicolo", newFolder.Description + " (" + fid.ToString() + ")");
            }
            else
            {
                Changed.Add("Fascicolo", "");

            }
            D.FolderId = folder.Id;
        }


        // Recupero i referenti da aggiungere e da rimuovere
        List<string> refs = D.Referents.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        List<string> refsToNotify = refs;
        List<string> ReferentiAggiunti = new List<string>();
        foreach (var r in document.ReferentList.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            ProfileType ptype = (ProfileType)int.Parse(r.Substring(0, 1));
            var pid = r.Substring(1);
            if (pid.StartsWith("@") && ptype == ProfileType.Group)
            {
                var Group = await userGroupRepo.GetByExternalId(pid.Substring(1));
                if (Group != null) pid = Group.Id;
            }
            if (pid.StartsWith("@") && ptype == ProfileType.Role)
            {
                var role = await roleRepository.GetByName(pid.Substring(1));
                if (role != null) pid = role.Id;
            }
            if (refs.IndexOf(r.Substring(0, 1) + pid) < 0)
            {
                refs.Add(r.Substring(0, 1) + pid);
                ReferentiAggiunti.Add(r.Substring(0, 1) + pid);
            }
        }
        List<string> ReferentiRimossi = new List<string>();
        for (var i = refs.Count - 1; i >= 0; i--)
        {
            var r = refs[i];
            if (document.ReferentList.IndexOf(r) < 0)
            {
                ReferentiRimossi.Add(r);
                refsToNotify.Remove(r);
                refs.Remove(r);
            }
        }

        List<string> refscc = D.ReferentsCC.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

        List<string> refsCCToNotify = refscc;
        List<string> ReferentiPCAggiunti = new List<string>();
        foreach (var r in document.ReferentListCC.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            ProfileType ptype = (ProfileType)int.Parse(r.Substring(0, 1));
            var pid = r.Substring(1);
            if (pid.StartsWith("@") && ptype == ProfileType.Group)
            {
                var Group = await userGroupRepo.GetByExternalId(pid.Substring(1));
                if (Group != null) pid = Group.Id;
            }
            if (pid.StartsWith("@") && ptype == ProfileType.Role)
            {
                var role = await roleRepository.GetByName(pid.Substring(1));
                if (role != null) pid = role.Id;
            }
            if (refs.IndexOf(r.Substring(0, 1) + pid) < 0 && refscc.IndexOf(r.Substring(0, 1) + pid) < 0)
            {
                ReferentiPCAggiunti.Add(r.Substring(0, 1) + pid);
                refscc.Add(r.Substring(0, 1) + pid);
            }
        }
        List<string> ReferentiPCRimossi = new List<string>();
        for (var i = refscc.Count - 1; i >= 0; i--)
        {
            var r = refscc[i];
            if (document.ReferentList.IndexOf(r) < 0)
            {
                ReferentiPCRimossi.Add(r);
                refsCCToNotify.Remove(r);
                refscc.Remove(r);
            }
        }

        D.Referents = string.Join(",", refs);
        D.ReferentsCC = string.Join(",", refscc);

        try
        {
            if (D.Fields.Count > 0)
                D.Fields.ForEach(m => { m.DocumentId = D.Id; m.DocumentTypeId = D.DocumentTypeId; });
            else
                D.Fields = null;


            if (document.Attachments != null)
                foreach (var l in document.Attachments)
                {
                    try
                    {
                        if (await _repository.AddLink(D.Id, l, UserId, true) > 0)
                            if (folder != null)
                                await _repository.AddToFolder(D.Id, folder.Id, UserId, false);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Document.Update:AddAttachments");
                    }
                }

            if (document.LinkTo != null)
                foreach (var l in document.LinkTo)
                {
                    try
                    {
                        await _repository.AddLink(D.Id, l, UserId, false);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Document.Update:AddLinkTo");
                    }
                }

            if (document.AttachTo != null)
                foreach (var l in document.AttachTo)
                    try
                    {
                        await _repository.AddLink(l, D.Id, UserId, true);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Document.Update:AddAttachTo");
                    }


            await _repository.Update(D);
            await Authorize(D, userInfo, D.Owner);
            if (!D.Owner.Equals(UserId))
                await Authorize(D, userInfo, UserId);
            if (IsDraft)
            {
                await SetPermission(D.Id, userInfo, UserId, ProfileType.User, PermissionType.CanAddContent, AuthorizationType.Granted);
            }
            await SetParentFolderPermissions(D, userInfo);
            if (document.Authorize != null)
                foreach (var a in document.Authorize.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    ProfileType ptype = (ProfileType)int.Parse(a.Substring(0, 1));
                    string pid = a.Substring(1);
                    if (pid.StartsWith("@") && ptype == ProfileType.Group)
                    {
                        var Group = await userGroupRepo.GetByExternalId(pid.Substring(1));
                        if (Group != null) pid = Group.Id;
                    }
                    if (pid.StartsWith("@") && ptype == ProfileType.Role)
                    {
                        var role = await roleRepository.GetByName(pid.Substring(1));
                        if (role != null) pid = role.Id;
                    }
                    await SetPermission(D.Id, userInfo, pid, ptype, PermissionType.CanView, AuthorizationType.Granted);
                    await SetPermission(D.Id, userInfo, pid, ptype, PermissionType.CanViewContent, AuthorizationType.Granted);
                }

            //await AddDefaultContent(D, tp, userInfo);
            await _repository.SaveChanges();
            if (D.Id > 0)
            {
                var Doc = await _Load(D);
                if (document.ProcessVariables == null)
                    document.ProcessVariables = new Dictionary<string, object>();
                document.ProcessVariables.Add("PropertiesChanged", Changed);
                var variables = new Dictionary<string, object>(document.ProcessVariables.AsEnumerable());
                if (_eventDispatcher != null)
                {
                    // se provengo da uno stato Draft e non ho mai generato l'evento di Creazione
                    if ((oldStatus == DocumentStatus.Draft && document.Status == DocumentStatus.Active) && !historyRepository.Any(D.Id, EventType.Creation))
                    {
                        await _eventDispatcher.Publish(new DocumentEventMessage(Doc, userInfo, EventType.Creation, document.ProcessVariables));
                    }
                    else
                    {
                        await _eventDispatcher.Publish(new DocumentEventMessage(Doc, userInfo, EventType.Update, document.ProcessVariables));
                    }

                }

                if (document.Status == DocumentStatus.Active)
                {
                    await Notify(D, UserId, ReferentiAggiunti, ReferentiPCAggiunti, NotificationConstants.CONST_TEMPLATE_REFERENT, userInfo, ActionRequestType.None);
                    await Notify(D, UserId, ReferentiRimossi, ReferentiPCRimossi, NotificationConstants.CONST_TEMPLATE_NO_REFERENT, userInfo, ActionRequestType.None);
                    // Notifico il documento ad eventuali destinatari

                    await Notify(D, UserId, refsToNotify, refsCCToNotify, NotificationConstants.CONST_TEMPLATE_REFERENT_CHANGES, userInfo, ActionRequestType.None);

                    var template = NotificationConstants.CONST_TEMPLATE_NOTIFY;
                    if (!String.IsNullOrEmpty(document.NotificationTemplate)) template = document.NotificationTemplate;
                    await Notify(D, UserId, document.NotifyTo.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList(), document.NotifyCC.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList(), template, userInfo);
                }

                if (document.Content != null && !string.IsNullOrEmpty(document.Content.FileData))
                {
                    Doc.Image = await _AddContent(D, tp, userInfo, document.Content);
                }
                if (FolderChanged)
                {
                    await RemoveFromFolder(folder.Id, new List<int> { D.Id }, userInfo);
                    if (newFolder != null && newFolder.Id > 0)
                        await AddToFolder(newFolder.Id, new List<int> { D.Id }, userInfo);
                }
                await AdjustFoldersDeadLine(D, userInfo);
                return Doc;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Document.Update", document);
            throw;
        }
        return null;
    }

    private async Task DeleteSingleDocument(Document d, UserProfile user, string motivation, bool recursive)
    {
        // Elimino tutti i documenti in esso contenuti
        if (recursive && d.ContentType == ContentType.Folder)
        {
            var ContentIdList = await _repository.GetFolderContent(d.Id);
            foreach (var cid in ContentIdList)
            {
                var c = await _repository.GetById(cid);
                await DeleteSingleDocument(c, user, motivation, recursive);
            }
        }
        int ret = await _repository.Delete(d, user.userId, motivation, _softDelete);
        if (ret > 0)
        {
            d.DocumentStatus = DocumentStatus.Active;
            var Doc = await _Load(d);

            if (_eventDispatcher != null) await _eventDispatcher.Publish(new DocumentEventMessage(Doc, user, EventType.Delete, new Dictionary<string, object> { { "Justification", motivation } }));
        }

    }

    public async Task Delete(int documentId, UserProfile user, string motivation = "", bool recursive = true)
    {
        var d = await _repository.GetById(documentId);
        ThrowIfNotExists(d, documentId);
        var P = await GetAuthorization(d, user, PermissionType.CanDelete);
        ThrowIfNotAuthorized(P, "a cancellare il documento ", user, d.Id.ToString());
        using (var T = _repository.BeginTransaction())
        {
            // Elimino tutti i documenti in esso contenuti
            if (recursive && d.ContentType == ContentType.Folder)
            {
                var ContentIdList = await _repository.GetFolderContent(d.Id);
                //foreach (var c in ContentIdList)
                //{

                //}
            }
            int ret = await _repository.Delete(d.Id, user.userId, motivation, _softDelete);
            if (ret > 0)
            {
                await _repository.Commit(T);
                d.DocumentStatus = DocumentStatus.Active;
                var Doc = await _Load(d);

                if (_eventDispatcher != null) await _eventDispatcher.Publish(new DocumentEventMessage(Doc, user, EventType.Delete, new Dictionary<string, object> { { "Justification", motivation } }));
                if (!String.IsNullOrEmpty(d.Referents))
                    await Notify(d, user.userId, d.Referents.Split(',').ToList(), d.ReferentsCC.Split(',').ToList(), NotificationConstants.CONST_TEMPLATE_REFERENT_CHANGES, user, ActionRequestType.None);

            }
        }
    }

    public async Task UnDelete(int documentId, UserProfile user)
    {
        var d = await _repository.GetById(documentId);
        ThrowIfNotExists(d, documentId); int ret = 0;
        await _repository.Restore(documentId);
        if (ret > 0)
        {
            d.DocumentStatus = DocumentStatus.Active;
            var Doc = await _Load(d);

            if (_eventDispatcher != null) await _eventDispatcher.Publish(new DocumentEventMessage(Doc, user, EventType.UnDelete, new Dictionary<string, object> { }));
            if (!String.IsNullOrEmpty(d.Referents))
                await Notify(d, user.userId, d.Referents.Split(',').ToList(), d.ReferentsCC.Split(',').ToList(), NotificationConstants.CONST_TEMPLATE_REFERENT_CHANGES, user, ActionRequestType.None);
        }
    }
    public async Task ChangeStatus(int documentId, UserProfile UserInfo, DocumentStatus newStatus)
    {
        if (newStatus == DocumentStatus.Deleted)
            await Delete(documentId, UserInfo);
        else
        {
            var d = await _repository.GetById(documentId);
            ThrowIfNotExists(d, documentId);
            var oldStatus = d.DocumentStatus;
            d.DocumentStatus = newStatus;
            await _repository.Update(d);
            await _repository.SaveChanges();
            var Doc = await _Load(d);

            if (_eventDispatcher != null) await _eventDispatcher.Publish(new DocumentEventMessage(Doc, UserInfo, EventType.ChangeStatus, new Dictionary<string, object>() { { "OldStatus", oldStatus }, { "NewStatus", newStatus } }));
        }
    }


    //FASCICOLI
    public async Task<List<BatchErrorResult>> AddToFolder(int folderId, List<int> documentList, UserProfile userInfo, bool moveTofolder = false)
    {

        List<BatchErrorResult> results = new List<BatchErrorResult>();
        try
        {
            var folder = await _repository.GetById(folderId);
            ThrowIfNotExists(folder, folderId);
            var P = await GetAuthorization(folder, userInfo, PermissionType.CanViewContent);
            ThrowIfNotAuthorized(P, "ad accedere al documento ", userInfo, folder.Id.ToString());
            var P2 = await GetAuthorization(folder, userInfo, PermissionType.CanAddContent);
            ThrowIfNotAuthorized(P2, "ad aggiungere contenuti al documento ", userInfo, folder.Id.ToString());

            var PermessiAllegato = await _repository.GetPermissionsByDocumentId(folderId);
            //using (var T = _repository.BeginTransaction())
            //{
            foreach (var d in documentList)
            {
                try
                {
                    var allegato = await _repository.GetById(d);
                    ThrowIfNotExists(allegato, d);
                    {
                        var docP = await GetAuthorization(allegato, userInfo, PermissionType.CanViewContent);
                        ThrowIfNotAuthorized(docP, "ad accedere al documento", userInfo, d.ToString());
                        await _repository.AddToFolder(d, folderId, userInfo.userId, moveTofolder);

                        foreach (var A in PermessiAllegato.Where(p => p.PermissionId == PermissionType.CanView || p.PermissionId == PermissionType.CanViewContent || allegato.ContentType == ContentType.Folder))
                            await _repository.SetPermission(d, A.ProfileId, A.ProfileType, A.PermissionId, A.Authorization);

                        await _repository.SaveChanges();

                        var DocumentAllegato = await _Load(allegato);
                        if (!String.IsNullOrEmpty(folder.Referents))
                            await Notify(folder, userInfo.userId, folder.Referents.Split(',').ToList(), folder.ReferentsCC.Split(',').ToList(), NotificationConstants.CONST_TEMPLATE_REFERENT_CHANGES, userInfo, ActionRequestType.None);

                        if (_eventDispatcher != null) await _eventDispatcher.Publish(new DocumentEventMessage(DocumentAllegato, userInfo, EventType.AddToFolder, new Dictionary<string, object>() { { "Folder", folder }, { "Moved", moveTofolder } }));
                    }
                }
                catch (Exception ex)
                {
                    BatchErrorResult r = new BatchErrorResult(d, ex);
                    results.Add(r);
                }
            }
        }
        catch (Exception ex)
        {
            BatchErrorResult r = new BatchErrorResult(folderId, ex);
            results.Add(r);
        }
        //    await _repository.Commit(T);
        //};
        return results;
    }
    public async Task<List<BatchErrorResult>> RemoveFromFolder(int folderId, List<int> documentList, UserProfile userInfo)
    {
        List<BatchErrorResult> results = new List<BatchErrorResult>();
        var folder = await _repository.GetById(folderId);
        ThrowIfNotExists(folder, folderId);
        var P = await GetAuthorization(folder, userInfo, PermissionType.CanViewContent);
        ThrowIfNotAuthorized(P, "ad accedere al documento ", userInfo, folder.Id.ToString());
        var P2 = await GetAuthorization(folder, userInfo, PermissionType.CanRemoveContent);
        ThrowIfNotAuthorized(P2, "a rimuovere contenuti dal documento ", userInfo, folder.Id.ToString());
        foreach (var d in documentList)
        {
            try
            {
                var doc = await _repository.GetById(d);
                ThrowIfNotExists(doc, d);
                var docP = await GetAuthorization(doc, userInfo, PermissionType.CanViewContent);
                ThrowIfNotAuthorized(docP, "ad accedere al documento", userInfo, d.ToString());
                await _repository.RemoveFromFolder(d, folderId);
                await _repository.SaveChanges();
                var Doc = await _Load(doc);

                if (_eventDispatcher != null) await _eventDispatcher.Publish(new DocumentEventMessage(Doc, userInfo, EventType.RemoveFromFolder, new Dictionary<string, object> { { "Folder", folder } }));
                if (!String.IsNullOrEmpty(folder.Referents))
                    await Notify(folder, userInfo.userId, folder.Referents.Split(',').ToList(), folder.ReferentsCC.Split(',').ToList(), NotificationConstants.CONST_TEMPLATE_REFERENT_CHANGES, userInfo, ActionRequestType.None);

            }
            catch (Exception ex)
            {
                BatchErrorResult r = new BatchErrorResult(d, ex);
                results.Add(r);
            }
        }
        return results;
    }
    public async Task<List<int>> GetFolderDocuments(int folderId, UserProfile userInfo)
    {
        var doc = await _repository.GetById(folderId);
        ThrowIfNotExists(doc, folderId);
        var P = await GetAuthorization(doc, userInfo, PermissionType.CanViewContent);
        ThrowIfNotAuthorized(P, "ad accedere al contenuto del documento", userInfo, folderId.ToString());

        List<int> docs = new List<int>();
        foreach (var d in await _repository.GetFolderContent(folderId))
        {
            var fdoc = await _repository.GetById(d);
            if (fdoc != null)
            {
                var Pd = await GetAuthorization(fdoc, userInfo, PermissionType.CanViewContent);
                if (Pd == AuthorizationType.Granted) { docs.Add(d); }
            }
        }
        return docs;
    }



    //RELAZIONI
    public async Task<bool> AddLink(int documentId, int AttachmentId, UserProfile userInfo, bool IsAttachment = false)
    {
        var d = await _repository.GetById(documentId);
        ThrowIfNotExists(d, documentId);
        var P = IsAttachment ? await GetAuthorization(d, userInfo, PermissionType.CanAddContent) : AuthorizationType.Granted;
        ThrowIfNotAuthorized(P, "ad accedere al documento ", userInfo, documentId.ToString());

        int Attached = await _repository.AddLink(d.Id, AttachmentId, userInfo.userId, IsAttachment);
        if (Attached > 0)
        {
            // Allineo i permessi dell'allegato con quelli del documento principale
            List<int> Allegati = await _repository.GetLinks(documentId, IsAttachment);
            var PermessiAllegato = await _repository.GetPermissionsByDocumentId(documentId);
            foreach (var A in PermessiAllegato)
                await SetPermissions(Allegati, userInfo, A.ProfileId, A.ProfileType, A.PermissionId, A.Authorization);

            await _repository.SaveChanges();
            if (_eventDispatcher != null)
            {
                var attachment = await _repository.GetById(AttachmentId);
                var Doc = await _Load(d);
                await _eventDispatcher.Publish(new DocumentEventMessage(Doc, userInfo, IsAttachment ? EventType.AddAttach : EventType.AddLink, new Dictionary<string, object>() { { "Attachment", attachment } }));
            }
            if (!String.IsNullOrEmpty(d.Referents) && IsAttachment)
                await Notify(d, userInfo.userId, d.Referents.Split(',').ToList(), d.ReferentsCC.Split(',').ToList(), NotificationConstants.CONST_TEMPLATE_REFERENT_CHANGES, userInfo, ActionRequestType.None);

            return true;
        }

        return false;
    }
    public async Task<bool> RemoveLink(int documentId, int AttachmentId, UserProfile userInfo, bool IsAttachment = false)
    {
        var d = await _repository.GetById(documentId);
        ThrowIfNotExists(d, documentId);
        var P = IsAttachment ? await GetAuthorization(d, userInfo, PermissionType.CanAddContent) : AuthorizationType.Granted;
        ThrowIfNotAuthorized(P, "ad accedere al documento ", userInfo, documentId.ToString());

        var done = await _repository.RemoveLink(documentId, AttachmentId, IsAttachment);
        if (done > 0)
        {
            if (_eventDispatcher != null)
            {
                var attachment = await _repository.GetById(AttachmentId);
                var Doc = await _Load(d);
                await _eventDispatcher.Publish(new DocumentEventMessage(Doc, userInfo, IsAttachment ? EventType.AttachRemoved : EventType.LinkRemoved, new Dictionary<string, object>() { { "Attachment", attachment } }));
            }
            if (!String.IsNullOrEmpty(d.Referents) && IsAttachment)
                await Notify(d, userInfo.userId, d.Referents.Split(',').ToList(), d.ReferentsCC.Split(',').ToList(), NotificationConstants.CONST_TEMPLATE_REFERENT_CHANGES, userInfo, ActionRequestType.None);
        }
        return done > 0;

    }
    public async Task<List<DocumentLink>> Links(int documentId, UserProfile userInfo, bool IsAttachment = false)
    {
        if (documentId <= 0) return new List<DocumentLink>();
        var d = await _repository.GetById(documentId);
        ThrowIfNotExists(d, documentId);
        var P = IsAttachment ? await GetAuthorization(d, userInfo, PermissionType.CanViewContent) : AuthorizationType.Granted;
        ThrowIfNotAuthorized(P, "ad accedere al documento ", userInfo, documentId.ToString());
        List<DocumentLink> found = new List<DocumentLink>();
        foreach (var id in await _repository.GetLinks(documentId, IsAttachment))
        {
            var link = await _repository.GetById(id);
            var docType = await _documentTypeRepository.GetById(link.DocumentTypeId);
            var Plink = IsAttachment ? await GetAuthorization(link, userInfo, PermissionType.CanView) : AuthorizationType.Granted;
            if (Plink == AuthorizationType.Granted && link.DocumentStatus != DocumentStatus.Deleted)
            {
                DocumentLink l = new DocumentLink();
                l.Id = link.Id;
                l.Status = link.DocumentStatus;
                l.DocumentNumber = link.DocumentFormattedNumber;
                l.DocumentDate = link.DocumentDate;
                l.Description = link.Description;
                l.DocumentType = docType.Name;
                if (link.ImageId.HasValue)
                {
                    var Link_Image = await _repository.GetImage(link.ImageId.Value);
                    l.VersionNumber = Link_Image.VersionNumber;
                    l.RevisionNumber = Link_Image.RevisionNumber;
                    l.FileExtension = Path.GetExtension(Link_Image.FileName);
                    l.FileName = Link_Image.FileName;
                    l.FileSize = Link_Image.FileSize;
                    l.ImageId = link.ImageId.Value;
                }
                if (!String.IsNullOrEmpty(link.ProtocolNumber))
                {
                    //TODO: Formattare il protocollo
                    l.Protocol = link.ProtocolNumber;
                }
                found.Add(l);
            }
        }
        return found;

    }

    public async Task<List<DocumentLink>> LinkedIn(int documentId, UserProfile userInfo, bool IsAttachment = false)
    {
        if (documentId <= 0) return new List<DocumentLink>();
        var d = await _repository.GetById(documentId);
        ThrowIfNotExists(d, documentId);
        var P = IsAttachment ? await GetAuthorization(d, userInfo, PermissionType.CanViewContent) : AuthorizationType.Granted;
        ThrowIfNotAuthorized(P, "ad accedere al documento ", userInfo, documentId.ToString());
        List<DocumentLink> found = new List<DocumentLink>();
        foreach (var id in await _repository.GetLinkedToDocuments(documentId, IsAttachment))
        {
            var link = await _repository.GetById(id);
            var Plink = IsAttachment ? await GetAuthorization(link, userInfo, PermissionType.CanView) : AuthorizationType.Granted;
            if (Plink == AuthorizationType.Granted && link.DocumentStatus != DocumentStatus.Deleted)
            {
                DocumentLink l = new DocumentLink();
                l.Id = link.Id;
                l.Status = link.DocumentStatus;
                l.DocumentNumber = link.DocumentFormattedNumber;
                l.DocumentDate = link.DocumentDate;
                l.Description = link.Description;
                l.DocumentType = link.DocumentType?.Name ?? "Document Generico";
                if (link.ImageId.HasValue)
                {
                    var Link_Image = await _repository.GetImage(link.ImageId.Value);
                    l.VersionNumber = Link_Image.VersionNumber;
                    l.RevisionNumber = Link_Image.RevisionNumber;
                    l.FileExtension = Path.GetExtension(Link_Image.FileName);
                    l.FileName = Link_Image.FileName;
                    l.FileSize = Link_Image.FileSize;
                    l.ImageId = link.ImageId.Value;
                }
                if (!String.IsNullOrEmpty(link.ProtocolNumber))
                {
                    //TODO: Formattare il protocollo
                    l.Protocol = link.ProtocolNumber;
                }
                found.Add(l);
            }
        }
        return found;

    }


    public async Task<Permission> GetPermission(int documentId, UserProfile userInfo, string permissionId)
    {
        var d = await _repository.GetById(documentId);

        var AValue = (d == null) ? AuthorizationType.Denied : await GetAuthorization(d, userInfo, permissionId);
        return new Permission(permissionId, permissionId, AValue);
    }
    public async Task<List<Permission>> GetPermissions(int documentId, UserProfile userInfo)
    {
        var p = await _repository.GetPermissionByProfileId(documentId, userInfo.userId, ProfileType.User);
        return p.Select(p => new Permission() { PermissionId = p.PermissionId, Authorization = p.Authorization, Label = _lookupTableRepository.GetById("$PERMISSIONS$", p.PermissionId).GetAwaiter().GetResult().Description }).ToList();
    }






    public async Task SetPermission(
        int documentId,
        UserProfile userInfo,
        string ProfileId,
        ProfileType ProfileType,
        string permissionId,
        AuthorizationType authorization, bool RaiseEvents = true)
    {
        // Esco se il permesso da impostare è già impostato
        // Questo controllo permette di evitare eventuali loop tra i documenti
        var Permesso = await _repository.GetPermission(documentId, ProfileId, ProfileType, permissionId);
        if (Permesso != null && Permesso.Authorization == authorization) return;

        await _repository.SetPermission(documentId, ProfileId, ProfileType, permissionId, authorization);
        if (_eventDispatcher != null && RaiseEvents)
        {
            var doc = await Load(documentId, userInfo);
            await _eventDispatcher.Publish(new DocumentEventMessage(doc, userInfo, EventType.Authorize, new Dictionary<string, object>() { { "Profile", ((int)ProfileType).ToString() + ProfileId }, { "Permission", permissionId }, { "Authorization", authorization } }));
        }


        // Se il documento è un fascicolo, recupero il contenuto e aggiorno i loro permessi 
        // Non controllo il contentType perchè se il documento non è un fascicolo la GetFolderContent
        // restituirà un elenco vuoto
        var documentList = await _repository.GetFolderContent(documentId);
        if (documentList != null && documentList.Count > 0)
        {
            foreach (var docId in documentList)
            {
                // Diffondo tutti i permessi sui sotto-fascicoli mentre sui documenti al loro interno solo quelli di visibilità
                var ct = await _repository.GetContentType(documentId);
                if (ct == ContentType.Folder || permissionId == PermissionType.CanView || permissionId == PermissionType.CanViewContent)
                    await SetPermission(docId, userInfo, ProfileId, ProfileType, permissionId, authorization, RaiseEvents);
            }

            //            await SetPermissions(documentList, userInfo, ProfileId, ProfileType, permissionId, authorization);
        }
        // Recupero gli allegati e aggiorno i loro permessi 
        documentList = await _repository.GetLinks(documentId, true);
        if (documentList != null && documentList.Count > 0)
        {
            await SetPermissions(documentList, userInfo, ProfileId, ProfileType, permissionId, authorization);
        }
    }

    public async Task SetPermissions(
        List<int> documentIdList,
        UserProfile userInfo,
        string ProfileId,
        ProfileType ProfileType,
        string permissionId,
        AuthorizationType authorization)
    {
        foreach (var id in documentIdList)
        {
            await SetPermission(id, userInfo, ProfileId, ProfileType, permissionId, authorization);
        }
    }

    public async Task SetPermissions(
        int documentId,
        UserProfile userInfo,
        string ProfileId,
        ProfileType ProfileType,
        Dictionary<string, AuthorizationType> Permissions)
    {
        foreach (var p in Permissions)
        {
            await SetPermission(documentId, userInfo, ProfileId, ProfileType, p.Key, p.Value);
        }
    }

    public async Task SetPermissions(
        List<int> documentIdList,
        UserProfile userInfo,
        string ProfileId,
        ProfileType ProfileType,
        Dictionary<string, AuthorizationType> Permissions)
    {
        foreach (var id in documentIdList)
        {
            await SetPermissions(id, userInfo, ProfileId, ProfileType, Permissions);
        }
    }

    public async Task RemovePermissions(int documentId, ProfileType profileType, string profileId)
    {
        await _repository.RemovePermissions(documentId, profileType, profileId);
    }

    public async Task<Permission> GetProfilePermission(int documentId, ProfileType profileType, string profileId, string permissionId)
    {
        AuthorizationType AValue = AuthorizationType.None;
        var p = await _repository.GetPermission(documentId, profileId, profileType, permissionId);
        if (p != null) AValue = p.Authorization;
        return new Permission(permissionId, permissionId, AValue);
    }
    public async Task<ProfilePermissions> GetProfilePermissions(int documentId, ProfileType profileType, string profileId)
    {
        ProfilePermissions P = new ProfilePermissions();
        P.ProfileId = profileId;
        P.ProfileType = profileType;
        P.ProfileName = await userContext.GetProfileName(profileId, profileType);
        P.Permissions = (await _repository.GetPermissionByProfileId(documentId, profileId, profileType))
                            .Select(p => new Permission() { PermissionId = p.PermissionId, Authorization = p.Authorization, Label = _lookupTableRepository.GetById("$PERMISSIONS$", p.PermissionId).GetAwaiter().GetResult().Description }).ToList();
        return P;
    }
    public async Task<List<ProfilePermissions>> GetDocumentPermissions(int documentId)
    {
        var p = await _repository.GetPermissionsByDocumentId(documentId);
        return p.GroupBy(g => new { g.ProfileId, g.ProfileType })
            .Select(g => new ProfilePermissions()
            {
                ProfileId = g.Key.ProfileId,
                ProfileType = g.Key.ProfileType,
                ProfileName = userContext.GetProfileName(g.Key.ProfileId, g.Key.ProfileType).GetAwaiter().GetResult(),
                Permissions = g
                                .Select(p => new Permission()
                                {
                                    PermissionId = p.PermissionId,
                                    Authorization = p.Authorization,
                                    Label = _lookupTableRepository.GetById("$PERMISSIONS$", p.PermissionId).GetAwaiter().GetResult().Description
                                }).ToList()
            }).ToList();
    }




    // CONTENUTI
    private async Task<DocumentImage> _AddContent(Document document, DocumentType tp, UserProfile userInfo, FileContent content, bool createNewVersion = true, bool checkIn = false)
    {

        if (document.DocumentStatus != DocumentStatus.Active)
            createNewVersion = false;

        string UserId = userInfo.userId;
        var LastVersion = 0;
        var LastRevision = 0;
        JobStatus SendingStatus = JobStatus.NotNeeded;

        var currentImage = new DocumentImage();
        bool CheckedOut = false;
        if (document.ImageId.HasValue)
        {
            currentImage = await _repository.GetImage(document.ImageId.Value);
            if (tp.ToBeSigned && currentImage.SignatureStatus == JobStatus.Completed && currentImage.SendingStatus == JobStatus.Running)
            {
                string msg = UserId + " ha tentato di sovrascrivere un documento in corso di spedizione";
                logger.LogError(msg);
                throw new UnauthorizedAccessException(msg);
            }
            LastVersion = currentImage.VersionNumber;
            LastRevision = currentImage.RevisionNumber;
            CheckedOut = !String.IsNullOrEmpty(currentImage.CheckOutUser);
        }

        if (content.FileData == null || content.FileData.Length == 0 && !content.LinkToContent)
            throw new InvalidDataException("Il contenuto indicato è vuoto");

        var fileData = content.DataIsInBase64 ? Convert.FromBase64String(content.FileData) : Encoding.UTF8.GetBytes(content.FileData);
        string FileNameOriginale = content.FileName;
        string fext = Path.GetExtension(FileNameOriginale).ToLower();

        var StampAdded = false;
        var company = await _companyRepository.GetById(document.CompanyId);
        var IsPdf = fext.EndsWith(".pdf");
        int LastImageId = document.ImageId.HasValue ? document.ImageId.Value : 0;

        var Versione = LastVersion + (createNewVersion ? 1 : 0);
        var Revisione = (createNewVersion ? 0 : LastRevision + 1);


        var IsBpmn = tp.ContentType == ContentType.Workflow;
        if (IsBpmn)
        {
            var BPMNID = document.DocumentNumber.ToString();
            if (string.IsNullOrEmpty(BPMNID)) BPMNID = document.ExternalId;
            // Aggiusto l'Id del processo
            XmlDocument doc = new XmlDocument();
            var xmlString = Encoding.UTF8.GetString(fileData);
            try
            {
                doc.LoadXml(xmlString);
                var mgr = new XmlNamespaceManager(doc.NameTable);
                mgr.AddNamespace("bpmn", "http://www.omg.org/spec/BPMN/20100524/MODEL");
                var nodes = doc.SelectNodes("//bpmn:process", mgr);
                if (nodes.Count > 0)
                {
                    var oldValue = nodes[0].Attributes["id"].Value;
                    if (BPMNID != oldValue)
                    {
                        //nodes[0].Attributes["name"].Value = document.Description;
                        fileData = Encoding.UTF8.GetBytes(xmlString.Replace("\"" + oldValue + "\"", "\"" + BPMNID + "\""));
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        var IsDmn = tp.ContentType == ContentType.DMN;
        if (IsDmn)
        {
            var BPMNID = document.DocumentNumber.ToString();
            if (string.IsNullOrEmpty(BPMNID)) BPMNID = document.ExternalId;
            // Aggiusto l'Id del processo
            XmlDocument doc = new XmlDocument();
            var xmlString = Encoding.UTF8.GetString(fileData);
            doc.LoadXml(xmlString);
            var mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("", "http://www.omg.org/spec/BPMN/20100524/MODEL");
            var nodes = doc.DocumentElement.SelectNodes("//*[local-name()='decision']", mgr);
            if (nodes.Count > 0)
            {
                var oldValue = nodes[0].Attributes["id"].Value;
                if (BPMNID != oldValue)
                {
                    //nodes[0].Attributes["name"].Value = document.Description;
                    fileData = Encoding.UTF8.GetBytes(xmlString.Replace("\"" + oldValue + "\"", "\"" + BPMNID + "\""));
                }
            }
        }


        DocumentImage R = new DocumentImage();
        R.Hash = MessageDigest.HashString(MessageDigest.HashType.SHA1, fileData);
        bool IsNew = R.Hash != currentImage.Hash;

        // Se l'Hash è diversa...
        // Creo il record su Images se sto creando una nuova versione rispetto ad una revisione precedente
        // che non sia la prima (rev=0)
        if (IsNew || (createNewVersion && LastRevision > 0))
        {
            // Verifica se l'immagine è già stata archiviata su un altro documento
            DocumentImage FileStored = null; // await docRepo.GetImageByHash(R.Hash);
            //R.Id = -1;
            R.VersionNumber = Versione;
            R.RevisionNumber = Revisione;
            R.OriginalPath = FileNameOriginale;
            R.OriginalFileName = Path.GetFileName(FileNameOriginale);

            R.FileManager = tp.FileManager + "";
            R.CreationDate = DateTime.UtcNow;
            R.FileSize = fileData.Length;
            R.PreviewStatus = !IsPdf ? JobStatus.Queued : JobStatus.Completed;
            R.PreservationStatus = tp.ToBePreserved ? JobStatus.Queued : JobStatus.NotNeeded;
            R.SignatureStatus = tp.ToBeSigned ? JobStatus.Queued : JobStatus.NotNeeded;
            R.SendingStatus = tp.ToBePublished ? JobStatus.Queued : JobStatus.NotNeeded;// LastImageId <= 0 || SendingStatus != JobStatus.NotNeeded && SendingStatus != JobStatus.Completed ? JobStatus.Queued : SendingStatus;
            R.Owner = userInfo.userId;
            R.FileNameHash = MessageDigest.HashText(MessageDigest.HashType.SHA256, R.OriginalFileName);
            if (FileStored != null)
            {
                R.FileName = FileStored.FileName;
                R.FileManager = FileStored.FileManager;
                R.Signatures = FileStored.Signatures;
            }
            else
            {
                R.FileName = "";
                await GetInternalFileName(document, company, R);
                var fileManager = await _filesystemProvider.InstanceOf(R.FileManager);
                using (var M = new MemoryStream(fileData))
                {
                    var Info = M.GetSignatureInfo();
                    R.Signatures = Info.Count();
                    if (await fileManager.WriteFromStream(R.FileName, M))
                    {
                        FileStored = R;
                    }
                }
            }

            if (FileStored != null)
            {


                var i = 0;
                using (var T = _repository.BeginTransaction())
                {
                    try
                    {
                        R.Id = await _repository.AddImage(document.Id, R, userInfo.userId);
                        document.ImageId = R.Id;
                        if (!String.IsNullOrEmpty(document.ProtocolNumber) && document.ProtocolImageId == 0)
                            document.ProtocolImageId = R.Id;
                        await _repository.Update(document);
                        await _repository.SaveChanges();
                        document.Image = R;

                        T.Commit();
                        if (_eventDispatcher != null)
                        {
                            var Doc = await _Load(document);

                            if (createNewVersion)
                                await _eventDispatcher.Publish(new DocumentEventMessage(Doc, userInfo, EventType.AddVersion, new Dictionary<string, object>() { { "Content", R } }));
                            else
                                if (checkIn)
                                await _eventDispatcher.Publish(new DocumentEventMessage(Doc, userInfo, EventType.CheckIn, new Dictionary<string, object>() { { "Content", R } }));
                            else
                                await _eventDispatcher.Publish(new DocumentEventMessage(Doc, userInfo, EventType.AddRevision, new Dictionary<string, object>() { { "Content", R } }));
                            //if (R.Signatures > 0)
                            //    await _eventDispatcher.Publish(new DocumentEventMessage(Doc, userInfo, EventType.AddDigitalSignature, new Dictionary<string, object>() { { "Content", R } }));
                        }
                        if (!String.IsNullOrEmpty(document.Referents))
                            await Notify(document, UserId, document.Referents.Split(',').ToList(), document.ReferentsCC.Split(',').ToList(), NotificationConstants.CONST_TEMPLATE_REFERENT_CHANGES, userInfo, ActionRequestType.None);
                    }
                    catch (Exception ex)
                    {
                        //if (_notificationService != null) await _notificationService.NotifyException(userInfo.userId, ex, document);
                        logger.LogError(ex, "AddContent", R);
                        T.Rollback();
                        throw;
                    }
                }
            }
            else throw new Exception("non è stato possibile memorizzare il file: " + R.FileName);
        }
        else
        {
            var Doc = await _Load(document);
            if (CheckedOut)
            {
                R.CheckOutUser = "";
                await _repository.CheckIn(R.Id, userInfo.userId, true);

                if (_eventDispatcher != null) await _eventDispatcher.Publish(new DocumentEventMessage(Doc, userInfo, EventType.CheckIn, new Dictionary<string, object>() { { "Content", R } }));
            }
            if (_eventDispatcher != null) await _eventDispatcher.Publish(new DocumentEventMessage(Doc, userInfo, EventType.NoContentUpdate, new Dictionary<string, object>() { { "Content", R } }));
            R = currentImage;
        }
        return R;
    }
    public async Task<DocumentImage> AddContent(int documentId, UserProfile userInfo, FileContent content, bool createNewVersion = true, bool checkIn = false)
    {
        var document = await _repository.GetById(documentId);
        ThrowIfNotExists(document, documentId);

        if (document.DocumentStatus != DocumentStatus.Active)
            createNewVersion = false;

        var P = ((string.Compare(userInfo.userId, document.Owner, true) == 0) && document.ImageId == null) || (document.DocumentStatus == DocumentStatus.Draft && string.Compare(userInfo.userId, document.Owner, true) == 0)
            ? AuthorizationType.Granted
            : await GetAuthorization(document, userInfo, PermissionType.CanAddContent);
        ThrowIfNotAuthorized(P, "ad aggiungere contenuti al documento", userInfo, document.Id.ToString());

        var tp = new DocumentType() { Id = null };

        if (document.ContentType == ContentType.Folder)
        {
            CreateOrUpdateDocument doc = new CreateOrUpdateDocument();
            doc.FolderId = document.Id;
            doc.Description = Path.GetFileName(content.FileName);
            doc.DocumentDate = DateTime.UtcNow.Date;
            doc.ContentType = ContentType.Document;
            var folder = await Create(doc, userInfo);
            document = await _repository.GetById(folder);
            documentId = folder;
        }

        if (document.DocumentTypeId != null)
        {
            tp = await _documentTypeRepository.GetById(document.DocumentTypeId);
        }
        return await _AddContent(document, tp, userInfo, content, createNewVersion, checkIn);
    }


    public async Task<DocumentImage> CreateTempContent(string UserId, FileContent content)
    {


        if (content.FileData == null || content.FileData.Length == 0 && !content.LinkToContent)
            throw new InvalidDataException("Il contenuto indicato è vuoto");
        var tp = new DocumentType() { Id = null };

        var fileData = content.DataIsInBase64 ? Convert.FromBase64String(content.FileData) : Encoding.UTF8.GetBytes(content.FileData);

        string FileNameOriginale = content.FileName;
        string fext = Path.GetExtension(FileNameOriginale).ToLower();
        var IsPdf = fext == ".pdf";

        var Versione = 1;
        var Revisione = 0;


        DocumentImage R = new DocumentImage();
        R.Hash = MessageDigest.HashString(MessageDigest.HashType.SHA1, fileData);

        // Verifica se l'immagine è già stata archiviata su un altro documento
        DocumentImage FileStored = null; // await docRepo.GetImageByHash(R.Hash);
        R.Id = -1;
        R.PreviewStatus = !IsPdf ? JobStatus.Queued : JobStatus.NotNeeded;
        R.VersionNumber = Versione;
        R.RevisionNumber = Revisione;
        R.OriginalPath = FileNameOriginale;
        R.OriginalFileName = Path.GetFileName(FileNameOriginale);

        R.FileManager = tp.FileManager + "";
        R.CreationDate = DateTime.UtcNow;
        R.FileSize = fileData.Length;

        //R.PreservationStatus = tp.ToBePreserved ? JobStatus.Queued : JobStatus.NotNeeded;
        R.SignatureStatus = tp.ToBeSigned ? JobStatus.Queued : JobStatus.NotNeeded;
        R.SendingStatus = JobStatus.NotNeeded;// LastImageId <= 0 || SendingStatus != JobStatus.NotNeeded && SendingStatus != JobStatus.Completed ? JobStatus.Queued : SendingStatus;
        R.Owner = UserId;
        R.FileNameHash = MessageDigest.HashText(MessageDigest.HashType.SHA256, R.OriginalFileName);
        R.FileName = FileStored.FileName;
        R.FileManager = FileStored.FileManager;
        R.Signatures = FileStored.Signatures;

        string NewFileName = "FORM_" + Guid.NewGuid().ToString() + Path.GetExtension(content.FileName);
        R.FileName = Path.Combine(R.CreationDate.Year.ToString(), R.CreationDate.Month.ToString(), NewFileName);

        var fileManager = await _filesystemProvider.InstanceOf(R.FileManager);

        var i = 0;
        using (var T = _repository.BeginTransaction())
        {
            try
            {
                R.Id = await _repository.AddImage(0, R, UserId);
                await _repository.SaveChanges();

                T.Commit();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "CreateTempContent", R);
                T.Rollback();
                throw;
            }
        }
        return R;
    }


    //public async Task AddPreview(int documentId, int imageId, UserProfile userInfo, FileContent content)
    //{
    //    var document = await _repository.GetById(documentId);
    //    ThrowIfNotExists(document, documentId);

    //    if (content.FileData == null || content.FileData.Length == 0 && !content.LinkToContent)
    //        throw new InvalidDataException("Il contenuto indicato è vuoto");

    //    string UserId = userInfo.userId;

    //    var currentImage = await _repository.GetImage(imageId);
    //    string PreviewFileName = currentImage.FileName + Path.GetExtension(content.FileName).ToLower();
    //    string fext = Path.GetExtension(PreviewFileName).ToLower();

    //    var fileData = content.DataIsInBase64 ? Convert.FromBase64String(content.FileData) : Encoding.UTF8.GetBytes(content.FileData);


    //    var IsPDF = (fext == ".pdf");

    //    if (document.ProtocolNumber > 0 && IsPDF)
    //    {
    //        var company = await _companyRepository.GetById(document.CompanyId);
    //        var tp = new DocumentType() { Id = null };
    //        if (document.DocumentTypeId != null)
    //            tp = await _documentTypeRepository.GetById(document.DocumentTypeId);
    //        var stamp = await GetProtocolStamp(document, userInfo, tp, company);
    //        if (stamp != null && stamp.Length > 0)
    //        {
    //            var StampAdded = await TryToAddProtocolStamp(tp, content, stamp);
    //            IsPDF = StampAdded;
    //        }
    //    }

    //    var fileManager = await _filesystemProvider.InstanceOf(currentImage.FileManager);
    //    using (var M = new MemoryStream(fileData))
    //    {
    //        if (await fileManager.WriteFromStream(PreviewFileName, M))
    //        {
    //            await _repository.ChangePreviewState(imageId, IsPDF ? JobStatus.Completed : JobStatus.Failed);
    //        }
    //    }
    //}



    public async Task<bool> RemoveContent(int documentId, UserProfile userInfo)
    {
        var d = await _repository.GetById(documentId);
        ThrowIfNotExists(d, documentId);
        if (d.ImageId == null) return false;
        DocumentImage R = await _repository.GetImage(d.ImageId.Value);
        ThrowIfNotExists(R, d.ImageId.Value);
        if (R.Id == d.ProtocolImageId)
            ThrowIfNotAuthorized(AuthorizationType.Denied, "a rimuovere il contenuto protocollato del documento", userInfo, d.Id.ToString());

        var P = await GetAuthorization(d, userInfo, PermissionType.CanRemoveContent);
        ThrowIfNotAuthorized(P, "a rimuovere il contenuto del documento", userInfo, d.Id.ToString());
        await _repository.RemoveImage(d.Id, R.Id);

        //if (R.Id == d.ImageId)
        //{
        //    var newcurrent = await _repository.GetLastImage(d.Id);
        //    d.ImageId = newcurrent != null ? newcurrent.Id : null;
        //    await _repository.Update(d);
        //}
        var FName = R.FileName;
        var file = await _filesystemProvider.InstanceOf(R.FileManager);
        try
        {
            if (await file.Delete(FName))
                if (Path.GetExtension(FName).ToLower() != ".pdf")
                {
                    var pdfExtension = Path.ChangeExtension(FName, ".pdf");
                    if (await file.Exists(pdfExtension))
                        await file.Delete(pdfExtension);
                }
        }
        catch { };
        var tp = await _documentTypeRepository.GetById(d.DocumentTypeId);
        var Doc = await _Load(d);

        if (_eventDispatcher != null) await _eventDispatcher.Publish(new DocumentEventMessage(Doc, userInfo, R.RevisionNumber == 0 ? EventType.RemoveVersion : EventType.RemoveRevision, new Dictionary<string, object>() { { "Content", R } }));
        //        if (d.ImageId == null) await AddDefaultContent(d, tp, userInfo);
        return true;
    }
    public async Task<byte[]> GetContent(int imageId)
    {
        if (imageId <= 0)
            ThrowIfNotExists(null, imageId);

        var R = await _repository.GetImage(imageId);
        ThrowIfNotExists(R, imageId);

        var fileManager = await _filesystemProvider.InstanceOf(R.FileManager);
        var file = await fileManager.ReadAllBytes(R.FileName);
        return file;
    }

    public async Task<byte[]> GetPreview(DocumentImage image)
    {
        var f = await _filesystemProvider.InstanceOf(image.FileManager);

        if (!image.FileName.EndsWith(".pdf") && await f.Exists(image.FileName + ".pdf"))
        {
            return await f.ReadAllBytes(image.FileName + ".pdf");
        }
        return null;
    }


    public async Task<byte[]> CheckOut(int imageId, string user)
    {
        if (imageId <= 0)
            ThrowIfNotExists(null, imageId);
        var R = await _repository.GetImage(imageId);
        ThrowIfNotExists(R, imageId);

        if (!String.IsNullOrEmpty(R.CheckOutUser) && R.CheckOutUser != user)
            throw new Exception("Il documento è bloccato in modifica dall'utente " + R.CheckOutUser);

        R.CheckOutUser = user;
        await _repository.UpdateImage(R);
        var fileManager = await _filesystemProvider.InstanceOf(R.FileManager);
        var file = await fileManager.ReadAllBytes(R.FileName);
        return file;

    }
    public async Task CheckIn(int imageId, string user, bool ForceCheckIn)
    {
        if (imageId <= 0)
            ThrowIfNotExists(null, imageId);
        var R = await _repository.GetImage(imageId);
        ThrowIfNotExists(R, imageId);

        if (!String.IsNullOrEmpty(R.CheckOutUser) && R.CheckOutUser != user && !ForceCheckIn)
            throw new Exception("Il documento è bloccato in modifica dall'utente " + R.CheckOutUser);

        R.CheckOutUser = "";
        await _repository.UpdateImage(R);
    }

    public async Task<DocumentImage> GetContentInfo(int imageId)
    {
        if (imageId <= 0)
            ThrowIfNotExists(null, imageId);
        var R = await _repository.GetImage(imageId);
        ThrowIfNotExists(R, imageId);
        return R;
    }


    public async Task<List<int>> GetDocumentsFromContentId(int imageId, UserProfile userInfo)
    {
        List<int> list = new List<int>();
        if (imageId <= 0) return list;

        var documentIds = await _repository.GetDocumentsByImage(imageId);
        foreach (var id in documentIds)
        {
            var d = await _repository.GetById(id);
            if (d != null)
            {
                var P = await GetAuthorization(d, userInfo, PermissionType.CanViewContent);
                if (P == AuthorizationType.Granted)
                {
                    list.Add(d.Id);
                }
            }
        }
        return list;
    }

    public async Task<List<DocumentVersion>> Images(int documentId, UserProfile userInfo)
    {
        if (documentId <= 0) return new List<DocumentVersion>();
        var doc = await _repository.GetById(documentId);
        ThrowIfNotExists(doc, documentId);
        var P = await GetAuthorization(doc, userInfo, PermissionType.CanViewContent);
        ThrowIfNotAuthorized(P, "ad accedere al contenuto del documento", userInfo, documentId.ToString());

        var Lista = new List<DocumentVersion>();
        foreach (var d in await _repository.GetImages(documentId))
        {
            //TODO: Verificare i permessi 
            DocumentVersion v = new DocumentVersion();
            v.CreationDate = d.CreationDate;
            v.FileName = d.FileName;
            v.VersionNumber = d.VersionNumber;
            v.RevisionNumber = d.RevisionNumber;
            v.FileExtension = System.IO.Path.GetExtension(d.FileName);
            //            v.Protected = d.PreviewStatus == BatchProcessStatus.Completed;
            v.SendingStatus = d.SendingStatus;
            //v.PreservationStatus = d.PreservationStatus;
            v.SignatureStatus = d.SignatureStatus;
            v.FileSize = d.FileSize;
            v.Owner = d.Owner;
            v.Id = documentId;
            v.ImageId = d.Id;
            Lista.Add(v);
        }
        return Lista;
    }
    public async Task View(DocumentInfo document, UserProfile userInfo)
    {
        if (document.Id > 0)
        {
            //var Doc = await Load(documentId, userInfo);
            await _eventDispatcher.Publish(new DocumentEventMessage(document, userInfo, EventType.View, null));
        }
    }

    public async Task<List<int>> GetDocumentFolders(int documentId, UserProfile userInfo)
    {
        if (documentId <= 0) return new List<int>();
        var doc = await _repository.GetById(documentId);
        ThrowIfNotExists(doc, documentId);
        var P = await GetAuthorization(doc, userInfo, PermissionType.CanViewContent);
        ThrowIfNotAuthorized(P, "ad accedere al documento", userInfo, documentId.ToString());

        List<int> docs = new List<int>();
        foreach (var d in await _repository.GetDocumentFolders(documentId))
        {
            var fdoc = await _repository.GetById(d);
            if (fdoc != null)
            {
                var Pd = await GetAuthorization(fdoc, userInfo, PermissionType.CanViewContent);
                if (Pd == AuthorizationType.Granted) { docs.Add(d); }
            }
        }
        return docs;
    }


    public async Task<int> UpdateSendingStatus(int imageId, JobStatus status, UserProfile userInfo)
    {
        int r = await _repository.ChangeSendingState(imageId, status);
        if (r > 0)
        {
            var documentIds = await _repository.GetDocumentsByImage(imageId);
            if (_eventDispatcher != null)
                foreach (var d in documentIds)
                {
                    var Doc = await Load(d, userInfo);
                    await _eventDispatcher.Publish(new DocumentEventMessage(Doc, userInfo, status == JobStatus.Completed ? EventType.Publish : EventType.PrepareForSending, new Dictionary<string, object>() { { "NewStatus", status } }));
                }
        }
        return r;
    }
    //public async Task<int> UpdatePreservationStatus(int imageId, JobStatus status, string PDA, UserProfile userInfo)
    //{
    //    int r = await _repository.ChangePreservationState(imageId, status, PDA);
    //    if (r > 0)
    //    {
    //        var documentIds = await _repository.GetDocumentsByImage(imageId);
    //        if (_eventDispatcher != null)
    //            foreach (var d in documentIds)
    //            {
    //                var Doc = await Load(d, userInfo);
    //                await _eventDispatcher.Publish(new DocumentEventMessage(Doc, userInfo, EventType.Preservation, new Dictionary<string, object>() { { "NewStatus", status } }));
    //            }
    //    }
    //    return r;
    //}

    public async Task<int> UpdateIndexingStatus(int imageId, JobStatus status, UserProfile userInfo)
    {
        int r = await _repository.ChangeIndexingState(imageId, status);
        return r;
    }
    public async Task<int> UpdatePreviewStatus(int imageId, JobStatus status, UserProfile userInfo)
    {
        int r = await _repository.ChangePreviewState(imageId, status);
        return r;
    }

    public async Task<int> UpdateSignatureStatus(int imageId, JobStatus status, UserProfile userInfo, string SignatureSession = "")
    {
        int r = 0;
        if (!string.IsNullOrEmpty(SignatureSession))
            r = await _repository.ChangeSignatureState(imageId, status, userInfo.userId, SignatureSession);
        if (_eventDispatcher != null)
        {
            var image = await _repository.GetImage(imageId);
            if (image.SignatureStatus == JobStatus.Completed)
            {
                var documentIds = await _repository.GetDocumentsByImage(imageId);
                foreach (var d in documentIds)
                {
                    var Doc = await Load(d, userInfo);
                    var oldStatus = Doc.DocumentStatus;
                    await _eventDispatcher.Publish(new DocumentEventMessage(Doc, userInfo, EventType.AddDigitalSignature, new Dictionary<string, object>() { { "OldStatus", oldStatus }, { "NewStatus", status } }));
                }
            }
        }
        return r;
    }


    // CONDIVISIONI
    public async Task<int> Publish(int imageId, UserProfile userInfo)
    {
        return await UpdateSendingStatus(imageId, JobStatus.Completed, userInfo);
    }
    public async Task<bool> Share(int[] documentIds, UserProfile userInfo, List<ProfileInfo> To, List<ProfileInfo> Cc, ActionRequestType RequestType, bool AssignToAllUsers, string Title, string Message = "")
    {
        DocumentNotification N = new DocumentNotification();

        foreach (var documentId in documentIds)
        {
            var d = await _repository.GetById(documentId);
            if (d != null)
            {
                //            ThrowIfNotExists(d, documentId);
                var P = await GetAuthorization(d, userInfo, PermissionType.CanViewContent);
                //ThrowIfNotAuthorized(P, "ad accedere al documento", userInfo, documentId.ToString());
                if (P == AuthorizationType.Granted)
                {
                    N.Documents.Add(d.Id);
                    N.CompanyId = d.CompanyId;
                }
            }
        }
        if (N.Documents.Count > 0)
        {
            //        if (d == null) return false;
            if (To.Count <= 0) return false;

            N.NotificationDate = DateTime.UtcNow;
            N.AssignToAllUsers = AssignToAllUsers;
            N.Sender = userInfo.userId;
            //N.TemplateId = NotificationConstants.CONST_TEMPLATE_NOTIFY;
            N.Title = Title;
            N.RequestType = RequestType;
            N.Message = Message;
            N.To = To;
            N.CC = Cc;

            if (_notificationService != null) await _notificationService.Notify(N, userInfo);
            return true;
        }
        return false;
    }

    //RICERCHE
    public async Task<List<int>> GetExpiredDocuments(int Top = 50)
    {
        return await _repository.GetExpiredDocuments(Top);
    }

    //public async Task<int> Count(UserProfile userInfo, List<SearchFilter> filters)
    //{
    //    return await _repository.Count(userInfo, filters);    
    //}
    //public async Task<List<int>> Find(UserProfile userInfo, List<SearchFilter> filters)
    //{
    //    return await _repository.Find(userInfo, filters);
    //}




    //METODI PRIVATI 
    private async Task<AuthorizationType> GetAuthorization(Document d, UserProfile userInfo, string permissionId)
    {
        if (userInfo.userId == SpecialUser.SystemUser || userInfo.IsService)
            return AuthorizationType.Granted;

        string UserName = userInfo.userId.ToLower();
        //UserACLs userACL = new UserACLs(userInfo, d.ACLId);
        // Elenco permessi per ruolo
        //Dictionary<string, AuthorizationType> Roles = new Dictionary<string, AuthorizationType>();
        //Dictionary<string, AuthorizationType> Groups = new Dictionary<string, AuthorizationType>();

        // Verifico se l'utente ha il permesso sul documento
        AuthorizationType AValue = AuthorizationType.None;
        var p = await _repository.GetPermission(d.Id, UserName, ProfileType.User, permissionId);
        if (p != null) AValue = p.Authorization;

        // Verifico se l'utente ha un permesso sull'ACL
        if (AValue == AuthorizationType.None)
        {
            var pg = await _aclRepository.GetAuthorization(d.ACLId, UserName, ProfileType.User, permissionId);
            if (pg == AuthorizationType.Granted)
                AValue = pg;
        }

        // Verifico i permessi sul documento per i ruoli dell'utente
        if (AValue == AuthorizationType.None)
        {
            foreach (var r in userInfo.Roles)
            {
                AuthorizationType auth = AuthorizationType.None;
                var pu = AuthorizationType.None;
                // Verifico se esiste un permesso per il ruolo sul documento
                var ru = await _repository.GetPermission(d.Id, r.Id, ProfileType.Role, permissionId);
                if (ru != null)
                {
                    pu = ru.Authorization;
                    if (pu == AuthorizationType.Granted)
                    {
                        AValue = pu;
                        break;
                    }
                }
                if (auth == AuthorizationType.None)
                {
                    // ...verifico se il ruolo è autorizzato sull'ACL
                    var pg = await _aclRepository.GetAuthorization(d.ACLId, r.Id, ProfileType.Role, permissionId);
                    if (pg == AuthorizationType.Granted)
                    {
                        AValue = pg;
                        break;
                    }
                }
            }
        }
        if (AValue == AuthorizationType.None)
        {
            foreach (var r in userInfo.GlobalRoles)
            {
                AuthorizationType auth = AuthorizationType.None;
                var pu = AuthorizationType.None;
                // Verifico se esiste un permesso per il ruolo sul documento
                var ru = await _repository.GetPermission(d.Id, r.Id, ProfileType.Role, permissionId);
                if (ru != null)
                {
                    pu = ru.Authorization;
                    if (pu == AuthorizationType.Granted)
                    {
                        AValue = pu;
                        break;
                    }
                }
                if (auth == AuthorizationType.None)
                {
                    // ...verifico se il ruolo è autorizzato sull'ACL
                    var pg = await _aclRepository.GetAuthorization(d.ACLId, r.Id, ProfileType.Role, permissionId);
                    if (pg == AuthorizationType.Granted)
                    {
                        AValue = pg;
                        break;
                    }
                }
            }
        }

        // Recupero i permessi dei ruoli assegnati ai gruppi
        if (AValue == AuthorizationType.None)
        {
            foreach (var g in userInfo.Groups)
            {
                AuthorizationType auth = AuthorizationType.None;
                // Verifico se esiste un permesso per il gruppo sul documento
                var ru = await _repository.GetPermission(d.Id, g.Id, ProfileType.Group, permissionId);
                if (ru != null)
                {
                    var pu = ru.Authorization;
                    if (pu != AuthorizationType.None)
                        auth = pu;
                }

                // Se il ruolo utente è autorizzato...
                if (auth == AuthorizationType.None && !String.IsNullOrEmpty(d.ACLId))
                {
                    // ...verifico se il gruppo è autorizzato sull'ACL
                    var pg = await _aclRepository.GetAuthorization(d.ACLId, g.Id, ProfileType.Group, permissionId);
                    auth = pg;
                }
                //}
                if (auth == AuthorizationType.Granted)
                {
                    AValue = auth;
                    break;
                }
            }
        }
        return AValue;
    }

    private async Task Notify(Document d, string Sender, List<string> Users, List<string> UsersPC, string TemplateID, UserProfile userProfile, ActionRequestType RequestType = ActionRequestType.Generic)
    {
        if (Users.Count <= 0 && UsersPC.Count <= 0) return;
        DocumentNotification N = new DocumentNotification();
        N.NotificationDate = DateTime.UtcNow;
        N.Sender = Sender;
        N.Message = d.Description;
        N.Title = d.DocumentType?.Description;
        N.TemplateId = TemplateID;
        N.CompanyId = d.CompanyId;
        N.Documents.Add(d.Id);
        List<ProfileInfo> pinfo = new();
        foreach (var u in Users)
        {
            ProfileType ptype = (ProfileType)int.Parse(u.Substring(0, 1));
            string pid = u.Substring(1);
            if (pid.StartsWith("@") && ptype == ProfileType.Group)
            {
                var Group = await userGroupRepo.GetByExternalId(pid.Substring(1));
                if (Group != null) pid = Group.Id;
            }
            if (pid.StartsWith("@") && ptype == ProfileType.Role)
            {
                var role = await roleRepository.GetByName(pid.Substring(1));
                if (role != null) pid = role.Id;
            }
            var p = new ProfileInfo() { ProfileId = pid, ProfileType = ptype };
            pinfo.Add(p);
        }
        N.To = pinfo;

        List<ProfileInfo> pinfocc = new();
        foreach (var u in UsersPC)
        {
            if (string.IsNullOrEmpty(u))
            {
                continue;
            }
            ProfileType ptype = (ProfileType)int.Parse(u.Substring(0, 1));
            string pid = u.Substring(1);
            if (pid.StartsWith("@") && ptype == ProfileType.Group)
            {
                var Group = await userGroupRepo.GetByExternalId(pid.Substring(1));
                if (Group != null) pid = Group.Id;
            }
            if (pid.StartsWith("@") && ptype == ProfileType.Role)
            {
                var role = await roleRepository.GetByName(pid.Substring(1));
                if (role != null) pid = role.Id;
            }
            var p = new ProfileInfo() { ProfileId = pid, ProfileType = ptype };
            pinfocc.Add(p);
        }
        N.CC = pinfocc;
        N.RequestType = RequestType; // N.To.Count > 0 ? ActionRequestType.Generic : ActionRequestType.None;
        if (_notificationService != null) await _notificationService.Notify(N, userProfile);
    }

    private void ThrowIfNotExists(object o, int id)
    {
        if (o == null)
        {
            var Errori = $"documento #{id} non trovato.";
            logger.LogError(Errori);
            throw new KeyNotFoundException(Errori);
        }
    }
    private void ThrowIfNotAuthorized(AuthorizationType auth, string message, UserProfile userInfo, string documentId)
    {
        if (auth != AuthorizationType.Granted)
        {
            var Errori = $"{userInfo.userId} non è autorizzato {message} #{documentId}";
            logger.LogError(Errori);
            throw new UnauthorizedAccessException(Errori);
        }
    }


    private async Task Authorize(Document doc, UserProfile user, string UserId)
    {
        var auths = new Dictionary<string, AuthorizationType>();
        if (string.IsNullOrEmpty(doc.DocumentTypeId))
            auths = new()
            {
                { PermissionType.CanViewContent, AuthorizationType.Granted },
                { PermissionType.CanView, AuthorizationType.Granted },
                { PermissionType.CanShare, AuthorizationType.Granted },
                { PermissionType.CanAuthorize, AuthorizationType.Granted },
                { PermissionType.CanAddContent, AuthorizationType.Granted },
                { PermissionType.CanRemoveContent, AuthorizationType.Granted },
                { PermissionType.CanDelete, AuthorizationType.Granted },
                { PermissionType.CanEdit, AuthorizationType.Granted },
                { PermissionType.CanViewRegistry, AuthorizationType.Granted }
            };
        else
            auths = new()
            {
                { PermissionType.CanViewContent, AuthorizationType.Granted },
                { PermissionType.CanView, AuthorizationType.Granted },
                { PermissionType.CanShare, AuthorizationType.Granted }
            };
        await SetPermissions(doc.Id, user, UserId, ProfileType.User, auths);
    }


    private async Task SynchronizeDocumentTypeSchema(Document nd, DocumentType NTp, Dictionary<string, string> Changed = null)
    {
        if (nd.DocumentTypeId != NTp.Id && nd.DocumentTypeId != null)
        {
            if (Changed != null) Changed.Add("Tipologia Documentale", nd.DocumentTypeId + "");
            nd.DocumentTypeId = NTp.Id;
        }
        nd.DocumentType = NTp;
        if (nd.ContentType != NTp.ContentType && !String.IsNullOrEmpty(NTp.Id))
        {
            if (Changed != null) Changed.Add("Tipo Documento", nd.ContentType.ToString());
            nd.ContentType = NTp.ContentType;
        }
        bool DocumentNumberTypeChanged = (nd.DocumentNumberDataType + "") != (NTp.DocumentNumberDataType + "");
        if (DocumentNumberTypeChanged)
        {
            //if (Changed != null) Changed.Add("Tipo Numero Documento", (nd.DocumentNumberDataType + "").ToString());
            nd.DocumentNumberDataType = NTp.DocumentNumberDataType;
        }

        if (string.IsNullOrEmpty(nd.Owner))
        {
            if (nd.Owner != NTp.Owner && !String.IsNullOrEmpty(NTp.Id))
            {
                if (Changed != null) Changed.Add("Proprietario", nd.Owner);
                nd.Owner = NTp.Owner;
            }
        }
        var isnew = nd.Id <= 0;
        if (isnew)
        {
            if (nd.DocumentStatus == 0)
                nd.DocumentStatus = NTp.InitialStatus; // .Draft ? DocumentStatus.Draft : DocumentStatus.Active;

            if (NTp.Reserved)
                nd.Reserved = NTp.Reserved;

        }

        if ((int)nd.DocumentStatus == 0) nd.DocumentStatus = DocumentStatus.Active;
        if (string.IsNullOrEmpty(nd.ACLId)) nd.ACLId = NTp.ACLId;
        if (NTp.PersonalData)
            nd.PersonalData = NTp.PersonalData;
        if (NTp.Reserved)
            nd.Reserved = NTp.Reserved;
        if (nd.ContentType != ContentType.Folder)
            nd.FolderId = 0;

        var exp = nd.ExpirationDate.HasValue ? nd.ExpirationDate.Value : DateTime.MaxValue;
        switch (NTp.ExpirationStrategy)
        {
            case ExpirationStrategy.None: nd.ExpirationDate = DateTime.MaxValue; break;
            case ExpirationStrategy.DocumentDate:
                if (nd.DocumentDate > DateTime.MinValue)
                    nd.ExpirationDate = nd.DocumentDate?.AddDays(NTp.ExpirationDays);
                else
                    nd.ExpirationDate = DateTime.MaxValue;
                break;
            case ExpirationStrategy.CreationDate: nd.ExpirationDate = nd.CreationDate.AddDays(NTp.ExpirationDays); break;
            case ExpirationStrategy.ProtocolDate: nd.ExpirationDate = !String.IsNullOrEmpty(nd.ProtocolNumber) ? nd.ProtocolDate.Value.AddDays(NTp.ExpirationDays) : DateTime.MaxValue; break;
            case ExpirationStrategy.Content: if (nd.Id <= 0) nd.ExpirationDate = DateTime.MaxValue; break;
            default:
                if (nd.ExpirationDate <= DateTime.MinValue)
                    nd.ExpirationDate = DateTime.MaxValue;
                break;
        }
        if (nd.ExpirationDate.HasValue && Math.Truncate(exp.Subtract(nd.ExpirationDate.Value).TotalSeconds) != 0)
        {
            if (Changed != null) Changed.Add("Data Scadenza", nd.ExpirationDate.Value.ToString("dd/MM/yyyy HH:mm"));
        }

        if (nd.Fields == null)
        {
            nd.Fields = new List<DocumentField>();
        }
        else
        {
            nd.Fields.ForEach(a => a.Customized = true);
        }
        //     int findex = 0;
        if (NTp.Fields != null)
        {
            // Mi assicuro di avere i campi della nuova tipologia
            foreach (var tfield in NTp.Fields.Where(t => !t.Deleted && !string.IsNullOrEmpty(t.FieldName)).OrderBy(T => T.FieldIndex))
            //for (int i = 0; i < NTp.FieldCount; i++)
            {
                string fieldType = tfield.FieldTypeId;
                FieldType M = null;
                if (fieldType != null && metacache.ContainsKey(fieldType))
                    M = metacache[fieldType];
                else
                {
                    M = await _dataTypeRepository.GetById(fieldType);
                    if (M == null)
                    {
                        M = new FieldType()
                        {
                            DefaultValue = tfield.DefaultValue,
                            Tag = false,
                            Encrypted = false
                        };
                    }
                    if (fieldType != null)
                        metacache[fieldType] = M;
                }
                var DefaultValue = !string.IsNullOrEmpty(tfield.DefaultValue) ? tfield.DefaultValue : M.DefaultValue;
                var Field = nd.Fields.Find(m => m.FieldName == tfield.FieldName);
                var found = Field != null;
                if (!found)
                {
                    // La chiave non esiste, quindi la creo
                    Field = new DocumentField()
                    {
                        FieldTypeId = M.Id,
                        FieldName = tfield.FieldName,
                        Value = DefaultValue
                    };
                    nd.Fields.Add(Field);
                }
                Field.FieldTypeId = M.Id;
                Field.Tag = M.Tag || tfield.Tag;
                Field.Encrypted = M.Encrypted || tfield.Encrypted;
                Field.Customized = false;
                Field.DocumentId = nd.Id;
                //Field.FieldIndex =  tfield.FieldIndex;
                Field.Document = nd;
                Field.DocumentType = nd.DocumentType;
                Field.DocumentTypeId = nd.DocumentTypeId;
            }
        }

        // Imposto le chiavi personalizzate
        for (int i = 0; i < nd.Fields.Count; i++)
        {
            var Field = nd.Fields[i];
            Field.FieldIndex = i + 1;
            if (Field.Customized)
            {
                string fieldType = Field.FieldTypeId + "";
                FieldType M = null;
                if (fieldType != null && metacache.ContainsKey(fieldType))
                    M = metacache[fieldType];
                else
                {
                    M = await _dataTypeRepository.GetById(fieldType);
                    if (fieldType != null && M != null)
                        metacache[fieldType] = M;
                }
                if (M != null)
                {
                    Field.Encrypted = M.Encrypted;
                    Field.Tag = M.Tag;
                }
                else
                {
                    Field.Value = "";
                    Field.FieldTypeId = null;
                    Field.LookupValue = "";
                }
            }
        }
        // Rimuovo le chiavi libere vuote o non tipizzate
        for (int i = nd.Fields.Count - 1; i >= 0; i--)
        {
            if (nd.Fields[i].Customized
                && (nd.Fields[i].FieldTypeId is null
                    || nd.Fields[i].FieldTypeId.StartsWith("$")
                    || nd.Fields[i].FieldTypeId == ""
                    || string.IsNullOrEmpty(nd.Fields[i].Value)
                    )
                )
                nd.Fields.RemoveAt(i);
        }
    }


    private async Task UpdateField(Document newDocument, AddOrUpdateDocumentField newField, UserProfile userInfo, Dictionary<string, string> Changed = null)
    {
        if (newDocument.DocumentType == null) return;
        if (string.IsNullOrEmpty(newDocument.DocumentType.Id)) return;

        if (newField.Value is null) newField.Value = "";

        if (newDocument.DocumentType.Fields == null) newDocument.DocumentType.Fields = new List<DocumentTypeField>();
        if (newDocument.Fields == null) newDocument.Fields = new List<DocumentField>();

        string fieldName = newField.FieldName;
        if (string.IsNullOrEmpty(fieldName) && newField.FieldIndex > 0)
        {
            fieldName = newDocument.DocumentType.Fields.FirstOrDefault(f => f.FieldIndex == newField.FieldIndex)?.FieldName ?? "";
        }
        if (string.IsNullOrEmpty(fieldName))
        {
            // campo non trovato.. esco senza inserirlo
            return;
        }

        var Field = newDocument.Fields.FirstOrDefault(f => f.FieldName != null && f.FieldName.Equals(fieldName, StringComparison.InvariantCultureIgnoreCase));
        var docTypeField = newDocument.DocumentType.Fields.FirstOrDefault(f => f.FieldName != null && f.FieldName.Equals(fieldName, StringComparison.InvariantCultureIgnoreCase));
        if (docTypeField == null)
        {
            // creo il campo nella tipologia
            docTypeField = new DocumentTypeField()
            {
                DocumentTypeId = newDocument.DocumentTypeId,
                FieldName = fieldName,
                DefaultValue = "",
                Editable = true,
                Encrypted = false,
                FieldTypeId = newField.FieldTypeId ?? "$$t",
                Mandatory = false,
                Preservable = false,
                Tag = false,
                Title = fieldName,
                Width = "150px",
                FieldIndex = newDocument.DocumentType.FieldCount + 1
            };
            newDocument.DocumentType.Fields.Add(docTypeField);
            await _documentTypeRepository.Update(newDocument.DocumentType);
        }
        if (Field == null)
        {
            int index = newDocument.Fields.Count > 0 ? newDocument.Fields.Max(f => f.FieldIndex) : 0;
            Field = new DocumentField()
            {
                FieldTypeId = newField.FieldTypeId,
                Document = newDocument,
                DocumentType = newDocument.DocumentType,
                FieldName = fieldName,
                FieldIndex = index + 1,
                Customized = docTypeField == null,
                Value = null
            };
            newDocument.Fields.Add(Field);
        }
        //TODO: Il campo dovrebbe essere formattato dal gestore del metadato
        if (newField.FieldTypeId == "$$i" || newField.FieldTypeId == "$$n")
            newField.Value = newField.Value.ToString().Replace(".", "").Replace(",", ".");


        if (newField.Value != Field.Value)
        {
            Field.LastUpdate = DateTime.UtcNow;
            Field.LastUpdateUser = userInfo.userId;
            if (Changed != null)
            {
                string fieldType = newField.FieldTypeId;
                FieldType M = null;
                if (fieldType != null && metacache.ContainsKey(fieldType))
                    M = metacache[fieldType];
                else
                {
                    M = await _dataTypeRepository.GetById(fieldType);
                    if (fieldType != null && M != null)
                        metacache[fieldType] = M;
                }
                var title = docTypeField == null ? M.Title : docTypeField.Title;
                if (string.IsNullOrEmpty(title)) title = "Metadato " + Field.FieldIndex.ToString();
                if (!Changed.ContainsKey(title))
                    Changed.Add(title, Field.LookupValue + " (" + Field.Value + ")");
            }
            Field.Value = newField.Value;
        }
    }


    private async Task LookupFields(Document nd)
    {
        var NTp = nd.DocumentType;
        FieldType NumberType = null;
        IDataTypeManager nfieldmanager = null;
        var n = nd.DocumentNumber ?? "";
        if (!string.IsNullOrEmpty(nd.DocumentNumberDataType))
        {
            // decodifico il numero documento
            NumberType = await _dataTypeRepository.GetById(nd.DocumentNumberDataType);
            nfieldmanager = await _dataTypeFactory.Instance(NumberType.DataType);
            if (!nfieldmanager.IsCalculated)
            {
                List<string> lvalues = new List<string>();
                List<string> fvalues = new List<string>();
                if (!string.IsNullOrEmpty(nd.DocumentNumber))
                    foreach (var v in nd.DocumentNumber.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                    {
                        var ftv = await nfieldmanager.Lookup(NumberType, v);
                        lvalues.Add(ftv.LookupValue);
                        fvalues.Add(ftv.FormattedValue);
                    }
                if (string.IsNullOrEmpty(NTp.DescriptionLabel)) nd.Description = string.Join(",", lvalues);
                nd.DocumentFormattedNumber = string.Join(",", fvalues);
            }
        }
        else
        {
            nd.DocumentFormattedNumber = n;
        }

        for (int i = 0; i < nd.Fields.Count; i++)
        {
            var Field = nd.Fields[i];
            Field.FieldIndex = i + 1;
            string fieldType = Field.FieldTypeId;
            FieldType M = null;
            if (fieldType != null && metacache.ContainsKey(fieldType))
                M = metacache[fieldType];
            else
            {
                M = await _dataTypeRepository.GetById(fieldType);
                if (M == null)
                {
                    M = new FieldType()
                    {
                        DataType = "$$t"
                    };
                }
                if (fieldType != null)
                    metacache[fieldType] = M;
            }
            var fieldmanager = await _dataTypeFactory.Instance(M.DataType);
            if (fieldmanager.IsBlob && Field.Blob == null)
            {
                Field.Blob = new ()
                {
                    DocumentId = Field.DocumentId,
                    Document = Field.Document,
                    DocumentType = Field.DocumentType,
                    DocumentTypeId = Field.DocumentTypeId,
                    FieldIndex = Field.FieldIndex,
                    FieldName = Field.FieldName,
                    FieldTypeId = Field.FieldTypeId,
                    LastUpdate = Field.LastUpdate,
                    LastUpdateUser = Field.LastUpdateUser,
                    Value = ""
                };
            };

            if (!fieldmanager.IsCalculated)
            {
                Field.LookupValue = "";
                Field.FormattedValue = "";


                var value = Field.Value;
                if (value != null)
                {
                    List<string> lvalues = new List<string>();
                    List<string> fvalues = new List<string>();
                    List<string> values = new List<string>();
                    List<string> fieldValues = new List<string>();
                    if (Field.Tag && !fieldmanager.IsBlob)
                    {
                        fieldValues = value.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
                    }
                    else
                    {
                        fieldValues.Add(value);
                    }
                    foreach (var v in fieldValues)
                    {
                        var ftv = await fieldmanager.Lookup(M, v);
                        values.Add(ftv.Value);
                        lvalues.Add(ftv.LookupValue);
                        fvalues.Add(ftv.FormattedValue);
                    }
                    value = string.Join(",", values);
                    Field.LookupValue = string.Join(",", lvalues);
                    Field.FormattedValue = string.Join(",", fvalues);
                    if (Field.LookupValue.Length > 255) Field.LookupValue = Field.LookupValue.Substring(0, 255);
                    if (Field.FormattedValue.Length > 255) Field.FormattedValue = Field.FormattedValue.Substring(0, 255);
                    if (!fieldmanager.IsBlob)
                    {
                        Field.Value = value;
                        // Fix dimensione 
                        if (Field.Value.Length > 255) Field.Value = Field.Value.Substring(0, 255);
                    }
                    else
                    {
                        Field.Value = "";
                        Field.Blob.Value = value;
                    }
                }
            }
        }

        // Elaboro i campi calcolati
        for (int i = 0; i < nd.Fields.Count; i++)
        {
            FieldType M = null;
            var fieldType = nd.Fields[i].FieldTypeId;
            if (fieldType != null && metacache.ContainsKey(fieldType))
                M = metacache[fieldType];
            else
            {
                M = await _dataTypeRepository.GetById(fieldType);
                if (M == null)
                {
                    M = new FieldType()
                    {
                        DataType = "$$t"
                    };
                }
                if (fieldType != null)
                    metacache[fieldType] = M;
            }

            var fieldmanager = await _dataTypeFactory.Instance(M.DataType);
            if (fieldmanager.IsCalculated)
            {
                List<string> lvalues = new List<string>();
                List<string> fvalues = new List<string>();
                if (nd.Fields[i].Value != null)
                    foreach (var v in nd.Fields[i].Value.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                    {
                        var ftv = await fieldmanager.Calculate(M, v, nd);
                        lvalues.Add(ftv.LookupValue);
                        fvalues.Add(ftv.FormattedValue);
                    }
                nd.Fields[i].LookupValue = string.Join(",", lvalues);
                if (nd.Fields[i].LookupValue.Length > 255) nd.Fields[i].LookupValue = nd.Fields[i].LookupValue.Substring(0, 255);
                nd.Fields[i].FormattedValue = string.Join(",", fvalues);
                if (nd.Fields[i].FormattedValue.Length > 255) nd.Fields[i].FormattedValue = nd.Fields[i].FormattedValue.Substring(0, 255);
            }
        }

        if (!string.IsNullOrEmpty(nd.DocumentNumberDataType))
        {
            // decodifico il numero documento
            if (nfieldmanager.IsCalculated)
            {
                List<string> lvalues = new List<string>();
                List<string> fvalues = new List<string>();
                if (nd.DocumentNumber != null)
                    foreach (var v in nd.DocumentNumber.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                    {
                        var ftv = await nfieldmanager.Calculate(NumberType, v, nd);
                        lvalues.Add(ftv.LookupValue);
                        fvalues.Add(ftv.FormattedValue);
                    }

                if (string.IsNullOrEmpty(NTp.DescriptionLabel)) nd.Description = string.Join(",", lvalues);
                nd.DocumentFormattedNumber = string.Join(",", fvalues);
                if (nd.DocumentFormattedNumber.Length > 255) nd.DocumentFormattedNumber = nd.DocumentFormattedNumber.Substring(0, 255);
            }
        }

    }





    private async Task GetInternalFileName(Document d, Company C, DocumentImage R)
    {
        // Estraggo l'estensione completa del file (es. .pdf.p7m) e cerco di escludere le estensioni più lunghe di 4
        string filename = R.OriginalFileName;
        string lfilename = filename.ToLower();

        int NrVersione = R.VersionNumber;
        int NrRevisione = R.RevisionNumber;
        //var tp = d.DocumentType;
        //var b = C;

        string x = "";
        string ext = "";
        do
        {
            x = Path.GetExtension(lfilename);
            if (!string.IsNullOrEmpty(x))
            {
                // recupero l'ultima estensione
                if (x != ".")
                {
                    ext = x + ext;
                    lfilename = lfilename.Substring(0, lfilename.Length - x.Length);
                }
                else x = "";
            }
            else x = "";
        } while (!string.IsNullOrEmpty(x));


        //TODO: Gestire il nome del file nel formato FatturaPA

        string NewFileName = _filePrefix + d.Id.ToString().PadLeft(11, '0') + "-" + NrVersione + "_" + NrRevisione + ext;
        NewFileName = Path.Combine(R.CreationDate.Year.ToString(), R.CreationDate.Month.ToString(), NewFileName);
        R.FileName = NewFileName;
    }
    private async Task AddDefaultContent(Document d, DocumentType tp, UserProfile userInfo)
    {
        if (d.ImageId is null || d.ImageId <= 0)
        {
            try
            {
                var Doc = await _Load(d);
                if (_eventDispatcher != null) await _eventDispatcher.Publish(new DocumentEventMessage(Doc, userInfo, EventType.DefaultContent, null));
            }
            catch (Exception ex)
            {
                //if (_notificationService != null) await _notificationService.NotifyException(userInfo.userId, ex, d);
            };
        }
    }
    public async Task<List<LookupTable>> FullPath(Document r2, UserProfile userInfo)
    {
        bool Primo = true;
        var uid = userInfo;
        List<LookupTable> percorsi = new List<LookupTable>();
        Document r = new Document();
        HashSet<int> precedenti = new HashSet<int>();
        if (r2.ContentType == ContentType.Folder)
        {
            if (r2.FolderId.HasValue && r2.FolderId.Value != 0)
            {
                precedenti.Add(r2.Id);
                r = await _repository.GetById(r2.FolderId.Value);
                if (r.Description != null) r.Description = r.Description.Replace("{DocumentFormattedNumber}", r.DocumentFormattedNumber);

                var p = await GetAuthorization(r, uid, PermissionType.CanViewContent);
                Primo = false;
                do
                {
                    if (r.Id > 0 && p == AuthorizationType.Granted && !precedenti.Contains(r.Id))
                    {
                        precedenti.Add(r.Id);
                        // Ignoro il fascicolo con il nome dell'utente
                        //if (r.FolderId >= 0)
                        percorsi.Add(new LookupTable() { Id = r.Id.ToString(), Description = r.Description });
                        // Passo al padre
                        if (r.FolderId.HasValue && r.FolderId.Value > 0)
                        {
                            r = await _repository.GetById(r.FolderId.Value);
                            r.Description = r.Description.Replace("{DocumentFormattedNumber}", r.DocumentFormattedNumber);
                            p = await GetAuthorization(r, uid, PermissionType.CanViewContent);
                        }
                        else
                        {
                            string t = r.DocumentTypeId;
                            int np = 0;
                            r = new Document();
                            r.DocumentTypeId = t;
                            r.Id = np;
                        }
                    }
                } while (r.Id > 0 && p == AuthorizationType.Granted && !precedenti.Contains(r.Id));
            }
            // Se non ho letto neanche il primo fascicolo
            if (!Primo)
            {
                //                if (r.Id == 0 && r.ContentType == ContentType.Folder)
                //                {
                //                    percorsi.Add(new LookupTable() { Id = r.Id.ToString(), Description = Constants.DocumentConstants.CONST_COMPANY_FOLDERS });
                //                }
                //                else
                //                {
                //                    percorsi.Add(new LookupTable() { Id = r.Id.ToString(), Description = Constants.DocumentConstants.CONST_PERSONAL_FOLDERS });
                //                }
            }
            //r2.Path = dt;
        }
        if (percorsi.Count > 1) percorsi.Reverse();
        return percorsi;
    }
    private async Task AdjustFoldersDeadLine(Document d, UserProfile userInfo)
    {
        if (d.DocumentType == null) return;
        // Aggiorno la data di scadenza dei fascicoli in cui è inserito il documento che deve avere una scadenza
        if (d.ContentType != ContentType.Folder && d.DocumentType.ExpirationStrategy != ExpirationStrategy.None && d.ExpirationDate > DateTime.MinValue)
        {
            // Verifico scadenza fascicolo
            logger.LogDebug("Verifico scadenza fascicoli per documento:" + d.Id.ToString() + " - " + d.ExpirationDate.ToString());
            List<int> rlist = await _repository.GetDocumentFolders(d.Id);
            foreach (var r in rlist)
            {
                // Ignoro le cartelle e arrivo fino al fascicolo che contiene il documento
                int id = r;
                var F = await _repository.GetById(id);
                HashSet<int> fascicoliVisitati = new HashSet<int>();
                while (F != null && F.Id > 0 && F.ContentType == ContentType.Folder && (F.DocumentStatus == DocumentStatus.Active || F.DocumentStatus == DocumentStatus.Draft))
                {
                    if (!fascicoliVisitati.Contains(F.Id))
                    {
                        fascicoliVisitati.Add(F.Id);
                        logger.LogDebug("Verifico scadenza fascicolo:" + F.Id.ToString() + " - " + F.ExpirationDate.ToString());
                        var tpf = await _documentTypeRepository.GetById(F.DocumentTypeId);
                        // Se il fascicolo scade insieme al primo documento
                        // aggiorno la sua data di scadenza
                        if (tpf.ExpirationStrategy == ExpirationStrategy.Content)
                            await SetFolderDeadLine(F, userInfo);

                        if (F.FolderId.HasValue)
                        {
                            id = F.FolderId.Value;
                            F = await _repository.GetById(id);
                        }
                        else
                            F = null;
                    }
                    else F = null;
                }
            }
        }

    }
    private async Task SetFolderDeadLine(Document F, UserProfile userInfo)
    {
        var docs = await _repository.GetFolderContent(F.Id);
        DateTime ExpirationDate = F.ExpirationDate.HasValue ? F.ExpirationDate.Value.Date : DateTime.MinValue.Date;
        DateTime MinData = ExpirationDate;
        if (docs.Count > 0)
        {
            List<int> Documenti = new List<int>();
            foreach (var d in docs)
            {
                Document doc = await _repository.GetById(d);
                if (doc.ExpirationDate < F.ExpirationDate)
                {
                    if (doc.ExpirationDate < MinData) MinData = doc.ExpirationDate.HasValue ? doc.ExpirationDate.Value : DateTime.MinValue;
                    Documenti.Add(doc.Id);
                }
            }
            if (MinData.Date != ExpirationDate)
            {
                F.ExpirationDate = MinData;
                await _repository.Update(F);
                var Doc = await _Load(F);
                if (_eventDispatcher != null) await _eventDispatcher.Publish(new DocumentEventMessage(Doc, userInfo, EventType.ExpirationUpdated, new Dictionary<string, object>() { { "PreviousExpirationDate", ExpirationDate }, { "ExpirationDate", MinData } }));
            }
        }
    }
    private async Task SetParentFolderPermissions(Document d, UserProfile userInfo)
    {
        // Se non sto protocollando un documento di tipologia riservata (senza lista autorizzazione)
        if (!d.Reserved)
        {
            // se è un sotto-fascicolo recupero i permessi dei fascicoli padri
            if (d.FolderId.HasValue && d.ContentType == ContentType.Folder)
            {
                //var Permessi = (await _repository.GetPermissionsByDocumentId(d.FolderId.Value)).Where(p => p.PermissionId == PermissionType.CanView || p.PermissionId == PermissionType.CanViewContent);
                var Permessi = (await _repository.GetPermissionsByDocumentId(d.FolderId.Value));
                foreach (var PermessiFascicolo in Permessi)
                    await SetPermission(d.Id, userInfo, PermessiFascicolo.ProfileId, PermessiFascicolo.ProfileType, PermessiFascicolo.PermissionId, PermessiFascicolo.Authorization);
            }
        }
    }

    private async Task<bool> TryToAddProtocolStamp(DocumentType tp, FileContent content, byte[] stamp)
    {
        decimal LeftPosition = 0, TopPosition = 0;
        switch (tp.LabelPosition)
        {
            case LabelPosition.InAltoASinistra:
                break;
            case LabelPosition.InAltoADestra:
                LeftPosition = 100;
                break;
            case LabelPosition.InBassoASinistra:
                TopPosition = 100;
                break;
            case LabelPosition.InBassoADestra:
                LeftPosition = 100; TopPosition = 100;
                break;
            case LabelPosition.Personalizzata:
                LeftPosition = tp.LabelX; TopPosition = tp.LabelY;
                break;
            default: return false;
        }

        var exit = await AddStamp(content, stamp, 1, LeftPosition, TopPosition);

        return exit;
    }
    private async Task<bool> AddStamp(FileContent content, byte[] stamp, int pageIndex, decimal LeftPosition, decimal TopPosition)
    {
        // Il timbro/immagine viene inserito sulla versione PDF del documento
        // Verifico se è il documento è un pdf, altrimenti provo a caricare la preview PDF
        // se la preview PDF non esiste ritorno un errore
        // Aggiungo il timbro
        // Archivio la nuova Preview
        //
        // come registro la nuova versione senza sovrascrivere il vecchio documento in formato originale ?
        //
        // es. Fattura,XML -> aggiungo timbro di Protocollo + note contabilità
        // ipotesi 1: memorizzo una nuova revisione che punta sempre allo stesso file, ma la preview la chiamo in modo diverso
        //        es. img123456.xml  ->  doc001-12345
        //        .1.1.pdf

        var filename = content.FileName;
        string ext = Path.GetExtension(filename).ToLower();
        if (ext == ".pdf")
        {
            //var FilePreview = FilePreviewFactory.Get(image, filename);
            //if (FilePreview.CanAddImage)
            //    FilePreview.AddImage()
            return true;
        }
        return false;
    }

    public async Task<List<int>> GetImagesBySignatureSession(string userName, string signatureSession)
    {
        return await _repository.GetImageOnSignatureSession(userName, signatureSession);
    }

    public async Task AddBlankSignField(DocumentInfo doc, UserProfile userInfo, int pageIndex, float xPercentage, float yPercentage, string nomeCampoFirma)
    {
        using (MemoryStream fileStream = new MemoryStream(await GetContent((int)doc.Image.Id)))
        {
            PDFFile pdfFile = new PDFFile(fileStream);
            using (var content = (MemoryStream)pdfFile.AddBlankSignature(nomeCampoFirma, pageIndex, xPercentage, yPercentage, 25, 5))
            {
                var FC = new FileContent()
                {
                    DataIsInBase64 = true,
                    FileData = Convert.ToBase64String(content.ToArray()),
                    FileName = doc.Image.FileName
                };
                await AddContent(doc.Id, userInfo, FC, false);
            }
        }
    }

    public async Task<List<FieldPosition>> GetBlankSignFields(Document doc)
    {
        List<FieldPosition> Fields = new();
        using (MemoryStream fileStream = new MemoryStream(await GetContent((int)doc.ImageId)))
        {
            PDFFile pdfFile = new PDFFile(fileStream);
            Fields = pdfFile.BlankSignatures();
    
            return Fields;
        }
    }

    public async Task<bool> RemoveBlankSignField(DocumentInfo doc, UserProfile userInfo, string FieldName)
    {
        using (MemoryStream fileStream = new MemoryStream(await GetContent(doc.Image.Id)))
        {
            PDFFile pdfFile = new PDFFile(fileStream);
            using (var content = (MemoryStream)pdfFile.RemoveBlankSignField(FieldName))
            {
                var FC = new FileContent()
                {
                    DataIsInBase64 = true,
                    FileName = Convert.ToBase64String(content.ToArray())
                };
                await AddContent(doc.Id, userInfo, FC, false);
            }
        }
        return true;
    }


    public async Task<DocumentImage> AddContentFromTemplate(int documentId, string templateKey, string Variables, string OutputExtension = "")
    {
        var docId = documentId;
        var templateId = await FindByUniqueId(null, templateKey, Domain.Enumerators.ContentType.Template);
        if ((docId > 0) && templateId > 0)
        {
            UserProfile su = UserProfile.SystemUser();
            var d = await Get(templateId);
            var contentinfo = await GetContentInfo(d.ImageId.Value);
            byte[] datacontent = null;
            datacontent = await GetContent(contentinfo.Id);
            if (datacontent == null)
            {
                throw new FileNotFoundException("Impossibile accedere al file: " + contentinfo.FileName);
            }

            var dataString = System.Text.Encoding.UTF8.GetString(datacontent);
            var outputFilename = Path.GetFileName(contentinfo.OriginalFileName);
            dataString = HtmlTemplateParser.Parse(dataString, Variables)
                .Replace("{{yyyy-MM-dd}}", DateTime.Now.ToString("yyyy-MM-dd"))
                .Replace("{{YYYY-MM-DD}}", DateTime.Now.ToString("yyyy-MM-dd"))
                .Replace("{{yyyy-mm-dd}}", DateTime.Now.ToString("yyyy-MM-dd"))
                .Replace("{{Today}}", DateTime.Now.ToString("dd/MM/yyyy"))
                .Replace("{{Date}}", DateTime.Now.ToString("dd/MM/yyyy"))
                .Replace("{{Now}}", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss"));

            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return cert!.Verify();
                };

            var image = configuration["Endpoint:FrontEnd"];
            if (image.EndsWith("/")) image = image.Substring(0, image.Length - 1);
            int i = dataString.IndexOf("\"/images/");
            while (i >= 0)
            {
                string img = "data:image/jpeg;base64, ";
                var j = dataString.IndexOf("\"", i + 1);
                var url = dataString.Substring(i + 1, j - i - 1);

                HttpClient httpClient = new HttpClient(handler);
                try
                {
                    var response = await httpClient.GetAsync(image + url);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        img += Convert.ToBase64String(await response.Content.ReadAsByteArrayAsync());
                        dataString = dataString.Substring(0, i) + "\"" + img + "\"" + dataString.Substring(j + 1);
                    }
                    else
                        dataString = dataString.Substring(0, i) + "\"\"" + dataString.Substring(j + 1);
                }
                catch (Exception ex)
                {
                    dataString = dataString.Substring(0, i) + "\"\"" + dataString.Substring(j + 1);
                    logger.LogError("Errore in download logo: " + ex.Message);

                }

                i = dataString.IndexOf("\"/images/");
            }
            try
            {
                HttpClient httpClient2 = new HttpClient(handler);
                var response2 = await httpClient2.GetAsync(image + "/css/site.css");
                if (response2.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var css = (await response2.Content.ReadAsStringAsync());
                    dataString = "<!DOCTYPE html><html><head><meta charset='UTF-16'/><style>\n" + css + "</style>\n</head><body>" + dataString + "</body></html>";
                }
            }
            catch (Exception ex)
            {
                dataString = "<!DOCTYPE html><html><head><meta charset='UTF-16'/><style>\n</style>\n</head><body>" + dataString + "</body></html>";
                logger.LogError("Errore in download CSS: " + ex.Message);
            }

            var data = System.Text.Encoding.UTF8.GetBytes(dataString);
            DocumentImage imageId = new();
            //TODO: conversione in pdf
            if (!String.IsNullOrEmpty(OutputExtension))
            {
                imageId = await ConvertTo(docId, OutputExtension);
            }
            else
            {
                dataString = Convert.ToBase64String(data);
                var content = new FileContent()
                {
                    DataIsInBase64 = true,
                    FileData = dataString,
                    FileName = outputFilename,
                    LinkToContent = false
                };
                int newDocumentId = docId;
                imageId = await AddContent(docId, su, content, false);
            }
            return imageId;
        }
        return null;
    }


    public async Task<DocumentImage> ConvertTo(int documentId, String OutputExtension)
    {
        var docId = documentId;
        if (docId > 0)
        {
            UserProfile su = UserProfile.SystemUser();
            //TODO: conversione in pdf
            if (!String.IsNullOrEmpty(OutputExtension))
            {
                var d = await Get(docId);
                var contentinfo = await GetContentInfo(d.ImageId.Value);
                byte[] datacontent = null;
                datacontent = await GetContent(contentinfo.Id);
                if (datacontent == null)
                {
                    throw new FileNotFoundException("Impossibile accedere al file: " + contentinfo.FileName);
                }

                var outputFilename = Path.GetFileName(contentinfo.OriginalFileName);
                var InputExtension = Path.GetExtension(contentinfo.FileName).ToLower();
                var converter = await fileConvertFactory.Get(InputExtension, OutputExtension);
                if (converter != null)
                {
                    using (var src = new MemoryStream(datacontent))
                    {
                        try
                        {
                            var dest = await converter.Convert(InputExtension, src);
                            if (dest != null)
                            {
                                var a = ((MemoryStream)dest).ToArray();
                                if (a.Length > 0)
                                {
                                    datacontent = a;
                                    outputFilename = Path.ChangeExtension(outputFilename, OutputExtension);

                                    var dataString = Convert.ToBase64String(datacontent);
                                    var content = new FileContent()
                                    {
                                        DataIsInBase64 = true,
                                        FileData = dataString,
                                        FileName = outputFilename,
                                        LinkToContent = false
                                    };
                                    var imageId = await AddContent(docId, su, content, false);
                                    return imageId;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogError("Errore in conversione Template -> PDF: " + ex.Message);
                        }
                    }
                }
            }
        }
        return null;
    }


    /*
        public async Task<DocumentDTO> New(DocumentType documentTypeId, int folderId, UserInfo userInfo)
        {
            DocumentDTO d = new DocumentDTO();
            d.CreationDate = DateTime.UtcNow;
            d.FolderId = folderId;
            d.UserInfo = userInfo;
            await ChangeType(d, documentTypeId);

            //var P = await GetAuthorizationsByUser(d, userInfo);
            //d.Permissions = P;
            d.Path = await FullPath(d,userInfo);
            return d;
        }

    


        public async Task<Document> Clone(int documentId, UserInfo userInfo)
        {
            Document nd = await Load(documentId, userInfo);
            nd.Id = 0;
            nd.MasterDocumentId = documentId;
            foreach (var F in nd.Fields)
            {
                F.DocumentId = 0;
            }
            return nd;
        }
   


 
        public async Task Delete(Document d, string Motivo, UserInfo User, bool DeleteContent = true)
        {
            int ret = 0;
            string UserId = User.userId;
            var P = await GetAuthorizationByUser(d, User, PermissionId.CanDelete);
            var roles = User.Roles;
            bool ok = P.Authorization == AuthorizationValue.Granted
                || roles.IndexOf("Admin") >= 0
                || roles.IndexOf("Gestore Banca Dati " + d.CompanyId) > 0;

            if (ok)
            {
                try
                {
                    bool soft = await appSettings.Get("Document.Delete") + "" != "S" || d.ContentType == ContentType.Folder;

                    await docRepo.Delete(d.Id, User.userId, Motivo, soft);
                    await eventDispatcher.Publish(new DocumentEventMessage(User, DocumentEventType.Delete, d, null));

                    d.DocumentStatus = DocumentStatus.Deleted;

                    if (DeleteContent)
                    {
                        List<int> figli = await docRepo.GetFolderContent(d.Id);
                        foreach (var f in figli)
                        {
                            var d2 = await docRepo.GetById(f);
                            await Delete(d2, Motivo, User, DeleteContent);
                        }
                        try
                        {
                            if (d.ImageId.HasValue)
                            {
                                var R = await docRepo.GetImage(d.ImageId.Value);
                                var  fileManager = filesystemFactory.InstanceOf(R.FileManager);
                                //fileManager.Encryption = d.Criptato;
                                await fileManager.Delete(R.FileName, soft);
                                string file = R.FileName + ".pdf";
                                if (await fileManager.Exists(file))
                                    await fileManager.Delete(file, soft);

                                file = R.FileName + ".xml";
                                if (await fileManager.Exists(file))
                                    await fileManager.Delete(file, soft);

                                file = R.FileName + ".html";
                                if (await fileManager.Exists(file))
                                    await fileManager.Delete(file, soft);
                            }
                        }
                        catch (Exception exf)
                        {
                            logger.LogError("File DELETE", exf);
                            throw;
                        }
                    }

                }
                catch (Exception ex)
                {
                    logger.LogError("Errore in DELETE su DOCUM00F", ex);
                    throw;
                }

            }

        }
   


        public async Task<List<int>> Copies(int Doc)
        {
            return await docRepo.Copies(Doc);
        }
        public async Task AllDocumentsInFolder(int DocId, List<int> docs, UserInfo User)
        {
            if (docs.IndexOf(DocId) < 0)
            {
                var Doc = await docRepo.GetById(DocId);

                string UserId = User.userId;
                var roles = User.Roles;
                var P = await GetAuthorizationByUser(Doc, User, PermissionId.CanRead);
                bool ok = P.Authorization == AuthorizationValue.Granted
                    || roles.IndexOf("Admin") >= 0
                    || roles.IndexOf("Gestore Banca Dati " +Doc.CompanyId) > 0;

                if (ok)
                {
                    docs.Add(DocId);
                    List<int> atts = await docRepo.GetFolderContent(DocId);
                    if (atts != null)
                        foreach (int d in atts)
                            await AllDocumentsInFolder(d, docs, User);
                }
            }
        }


        public async Task<int> CopyTo(int FolderId, List<int> Docs, UserInfo userInfo, bool move = false)
        {
            int done = 0;
            var F = await docRepo.GetById(FolderId);
    //        var P = await GetAuthorizationByUser(F, userInfo, PermissionId.CanRead);
            foreach (var d in Docs)
            {
                var doc = await docRepo.GetById(d);

                var roles = userInfo.Roles;
                var docP = await GetAuthorizationByUser(doc, userInfo, PermissionId.CanRead);
                bool ok =docP.Authorization == AuthorizationValue.Granted
                    || roles.IndexOf("Admin") >= 0
                    || roles.IndexOf("Gestore Banca Dati " + doc.CompanyId) > 0;

                if (doc.Id > 0 && ok)
                {
                    try
                    {
                        await docRepo.AddToFolder(d, FolderId, userInfo.userId, move);
                        await docRepo.SaveChanges();
                        await eventDispatcher.Publish(new FolderEventMessage(userInfo, FolderEventType.InsertIntoFolder, doc, new Dictionary<string, object>() { { "FolderId", FolderId } }));
                        done += 1;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return done;
        }
 
        public async Task<List<int>> Attachments(int Doc)
        {
            if (Doc <= 0) return new List<int>();
            return await docRepo.GetLinks (Doc, true);
        }
        public async Task<List<int>> AttachedTo(int Doc)
        {
            return await docRepo.GetLinkedToDocuments (Doc, true);
        }


        public async Task<List<int>> Links(int Doc, UserInfo userInfo)
        {
            return await docRepo.GetLinks(Doc,false);
        }
        public async Task<List<int>> LinkedTo(int Doc, UserInfo userInfo)
        {
            return await docRepo.GetLinkedToDocuments(Doc,false);
        }
        public async Task<int> AddLink(UserInfo userInfo, int Doc, int Link)
        {

            int done = await docRepo.AddLink(Doc, Link, userInfo.userId);
            var d = await docRepo.GetById(Doc);
            await eventDispatcher.Publish(new LinkEventMessage(userInfo, LinkEventType.AddLink, d, new Dictionary<string, object>() { { "LinkId", Link } }));
            return done;
        }
        public async Task<int> RemoveLink(UserInfo userInfo, int Doc, int Link)
        {
            int done = await docRepo.RemoveLink(Doc, Link);
            var d = await docRepo.GetById(Doc);
            await eventDispatcher.Publish(new LinkEventMessage(userInfo, LinkEventType.LinkRemoved, d, new Dictionary<string, object>() { { "LinkId", Link } }));
            return done;
        }





   



        public async Task<int> AddImage(Document d, string filename, byte[] data, UserInfo userInfo)
        {
            return await _AddImage(d, filename, data,  userInfo, true);
        }

        public async Task<int> AddRevision(Document d,  byte[] data, UserInfo userInfo)
        {
            return await _AddImage(d, d.Image.FileName, data, userInfo, false);
        }
   

        public async Task Preserve(Document d, JobStatus NSX, string PDA, UserInfo userInfo)
        {
            await docRepo.ChangePreservationState(d.ImageId.Value, NSX, PDA);
            if (NSX == JobStatus.Done)
                await eventDispatcher.Publish(new ContentEventMessage(userInfo, ContentEventType.Preserve, d, new Dictionary<string, object>() { { "PDA",PDA }, {"Status", NSX  } }));

        }
        public async Task CancelPreservation(string PDA, UserInfo userInfo)
        {
            await docRepo.DeletePDA(PDA);
        }
        public async Task Sign(Document d, JobStatus NSX, UserInfo userInfo)
        {
            if (NSX == JobStatus.Done)
            {
                await eventDispatcher.Publish(new ContentEventMessage(userInfo, ContentEventType.AddDigitalSignature, d, new Dictionary<string, object>() { { "Status", NSX } }));
            }
        }
        public async Task PreservationSign(Document d, JobStatus NSX, UserInfo userInfo)
        {
            await docRepo.ChangeSignatureState(d.ImageId.Value, NSX);
            if (NSX == JobStatus.Done)
            {
                await eventDispatcher.Publish(new ContentEventMessage(userInfo, ContentEventType.AddPreservationSignature, d, new Dictionary<string, object>() { { "Status", NSX } }));
            }
        }
        public async Task BioSign(Document d, string firmatario, UserInfo userInfo)
        {
            await eventDispatcher.Publish(new ContentEventMessage(userInfo, ContentEventType.AddBiometricalSignature, d, new Dictionary<string, object>() { { "Firmatario", firmatario } }));
        }
        public async Task Send(Document r, JobStatus NSX, UserInfo userInfo)
        {
            if (r.ImageId > 0)
            {
                await docRepo.ChangeSendingState(r.ImageId.Value, NSX);
                if (NSX == JobStatus.Ignored)
                    await eventDispatcher.Publish(new ContentEventMessage(userInfo, ContentEventType.ExcludeFromSending, r, new Dictionary<string, object>() { { "Status", NSX } }));
                else if (NSX == JobStatus.ToBeDone)
                    await eventDispatcher.Publish(new ContentEventMessage(userInfo, ContentEventType.IncludeInSending, r, new Dictionary<string, object>() { { "Status", NSX } }));
            }

        }
        public async Task<bool> CheckOut(Document d, UserInfo userInfo, bool Test)
        {
            var r = await docRepo.CheckOut(d.ImageId.Value, userInfo.userId, Test);
            if (r)
                await eventDispatcher.Publish(new ContentEventMessage(userInfo, ContentEventType.CheckOut, d, new Dictionary<string, object>() { { "Test", Test } }));
            return r;
        }
        public async Task<bool> CheckIn(Document d, UserInfo userInfo, bool force)
        {
            var r = await docRepo.CheckIn(d.ImageId.Value, userInfo.userId, force);
            if (r)
                await eventDispatcher.Publish(new ContentEventMessage(userInfo, ContentEventType.CheckIn, d, new Dictionary<string, object>() { { "Force", force } }));
            return r;
        }








        private async Task GetDocumentAndAllAttachments(int Doc, List<int> docs)
        {
            if (docs.IndexOf(Doc) < 0)
            {
                docs.Add(Doc);
                List<int> atts = await docRepo.GetLinks(Doc,true);
                if (atts != null)
                    foreach (int d in atts)
                        await GetDocumentAndAllAttachments(d, docs);
            }
        }
     
        private async Task GetInternalFileName(Document d, DocumentImage R)
        {
            // Estraggo l'estensione completa del file (es. .pdf.p7m) e cerco di escludere le estensioni più lunghe di 4
            string filename = R.OriginalFileName;
            string lfilename = filename.ToLower();

            int NrVersione = R.VersionNumber;
            int NrRevisione = R.RevisionNumber;
            var tp = d.DocumentType;
            var b = d.Company;

            string x = "";
            string ext = "";
            do
            {
                x = Path.GetExtension(lfilename);
                if (!string.IsNullOrEmpty(x))
                {
                    // recupero l'ultima estensione
                    if (x != ".")
                    {
                        ext = x + ext;
                        lfilename = lfilename.Substring(0, lfilename.Length - x.Length);
                    }
                    else x = "";
                }
                else x = "";
            } while (!string.IsNullOrEmpty(x));

            string NewFileName = "";
            if (tp.Id.ToLower().StartsWith("td") && tp.Id.Length == 4 ||
                tp.Channel == "W"  || tp.Channel == "S"  )
            {
                string piva = await appSettings.Get(d.CompanyId, "PartitaIVA");

                if (!lfilename.StartsWith(piva) && piva.Length > 3)
                {
                    NewFileName = TemplateManager.GetFEFileName(piva, b.Codice, ".xml" + (ext.EndsWith(".p7m") ? ".p7m" : ""));
                }
                else
                {
                    string f = lfilename;
                    if (f.StartsWith("temp_"))
                    {
                        f = f.Substring(5);
                        int i = f.IndexOf("_");
                        if (i >= 0)
                            f = f.Substring(i + 1);
                    }
                    NewFileName = f;
                }
            }
            else
                NewFileName = "DOC" + b.Codice + "-" + d.Id.ToString().PadLeft(11, '0') + "-" + NrVersione.ToString() + "." + NrRevisione.ToString().PadLeft(3, '0') + ext;
            R.FileName = NewFileName;
        }
 
 
        private string Validate(Document d)
        {
            if (d.Description == null) d.Description = "";
            if (d.Description.Length > 250) d.Description = d.Description.Substring(0, 250);
            if (d.ACLId == null) d.ACLId = "";
            if (d.ACLId.Length > 64) d.ACLId = d.ACLId.Substring(0, 64);
            if (d.Owner == null) d.Owner = "";
            if (d.Owner.Length > 64) d.Owner = d.Owner.Substring(0, 64);
            //            if (d.Mittente == null) d.Mittente = "";
            //            if (d.Mittente.Length > 250) d.Mittente = d.Mittente.Substring(0, 250);
            //            if (d.Computer == null) d.Computer = "";
            //            if (d.Computer.Length > 64) d.Computer = d.Computer.Substring(0, 64);
            //            if (d.Commento == null) d.Commento = "";
            //            if (d.Commento.Length > 250) d.Commento = d.Commento.Substring(0, 250);
            //if (d.Immagine.Filename == null) d.Immagine.Filename = "";

            //            if (d.ProtocolNumberEsterno == null) d.ProtocolNumberEsterno = "";
            //            if (d.ProtocolNumberEsterno.Length > 250) d.ProtocolNumberEsterno = d.ProtocolNumberEsterno.Substring(0, 250);



            return "";
        }
  
   
   

        private async Task<DocumentImage> SaveImage(Document d, UserInfo userInfo, string FileName, byte[] data, bool CreateNewVersion)
        {
            int Id = d.Id;
            int LastImageId = d.ImageId.HasValue ? d.ImageId.Value : 0;



            DocumentImage R = new DocumentImage();
            R.Hash = MessageDigest.HashString(MessageDigest.HashType.SHA1, data);

            JobStatus SendingStatus = JobStatus.ToBeDone;
            int Versione = 0;
            int Revisione = 0;

            bool IsNew = LastImageId <= 0;
            if (!IsNew)
            {
                var im = await docRepo.GetImage(LastImageId);
                Versione = im.VersionNumber;
                Revisione = im.RevisionNumber;
                SendingStatus = im.SendingStatus;
                IsNew = !im.Hash.Equals(R.Hash);
                if (!IsNew) R = im;
            }

            // Se l'Hash è diversa...
            if (IsNew)
            {
                bool FileStored = false;
                // Verifica se l'immagine è già stata archiviata su un altro documento
                // ricercando per Hash
                // TOOD:
                if (!FileStored)
                {
                    var tp = d.DocumentType;

                    R.Id = -1;
                    R.PreviewStatus = BatchProcessStatus.NotStarted;
                    R.VersionNumber = Versione <= 0 || CreateNewVersion ? Versione + 1 : Versione;
                    R.RevisionNumber = CreateNewVersion ? 0 : Revisione + 1;
                    R.OriginalPath = FileName;
                    R.OriginalFileName = Path.GetFileName(FileName);

                    R.FileManager = tp.FileManager;
                    R.CreationDate = DateTime.UtcNow;
                    R.FileManager = tp.FileManager;
                    R.FileSize = data.Length;

                    R.PreservationStatus = tp.ToBePreserved ? JobStatus.ToBeDone : JobStatus.None;
                    R.SignatureStatus = tp.ToBeSigned ? JobStatus.ToBeDone : JobStatus.None;
                    R.SendingStatus = LastImageId <= 0 || SendingStatus != JobStatus.None && SendingStatus != JobStatus.Done ? JobStatus .ToBeDone: SendingStatus;
                    R.Owner = userInfo.userId;
                    R.FileNameHash = MessageDigest.HashText(MessageDigest.HashType.SHA256, Path.GetFileName(FileName));
                    R.FileName = "";
                    await GetInternalFileName(d, R);
                    var fileManager = filesystemFactory.InstanceOf(R.FileManager);
                    using (var M = new MemoryStream(data))
                    {
                        var Info = M.GetSignatureInfo();
                        R.Signatures = Info.Count();
                        if (await fileManager.WriteFromStream(R.FileName, M))
                        {
                            FileStored = true;
                        }
                    }
                }
                if (FileStored)
                {
                    R.Id = await docRepo.AddImage(d, R, userInfo.userId);
                    if (CreateNewVersion)
                        await eventDispatcher.Publish(new ContentEventMessage(userInfo, ContentEventType.AddVersion, d, new Dictionary<string, object>() { { "Content", R } }));
                    else
                        await eventDispatcher.Publish(new ContentEventMessage(userInfo, ContentEventType.RemoveVersion, d, new Dictionary<string, object>() { { "Content", R } }));
                }
            }
            else
            {
                await eventDispatcher.Publish(new ContentEventMessage(userInfo, ContentEventType.TryStoringTheSameImage, d, new Dictionary<string, object>() { { "Content", R } }));
            }
            return R;
        }



        private async Task<int> _AddImage(Document d, string filename, byte[] data, UserInfo userInfo, bool NewVersion)
        {
            string UserId = userInfo.userId;
            var roles = userInfo.Roles;
            var P = await GetAuthorizationByUser(d, userInfo, PermissionId.CanAddImage);
            bool ok = P.Authorization == AuthorizationValue.Granted
                || roles.IndexOf("Admin") >= 0
                || roles.IndexOf("Gestore Banca Dati " + d.CompanyId) > 0;

            if (!ok)
            {
                string msg = UserId + " ha tentato di sovrascrivere il documento elettronico: " + tp.Name + " nr." + d.DocumentNumber + " (#" + d.Id.ToString() + ") senza averne i permessi.";
                logger.LogWarning(msg);

                return CONST_ADDIMAGE_CANNOTADD;
            }


            if (data == null || data.Length == 0) return CONST_ADDIMAGE_EMPTYFILE;

            string FileNameOriginale = filename;
            string fext = Path.GetExtension(filename).ToLower();

            var b = d.Company;
            var tp = d.DocumentType;


            //TODO: Verifico se il documento è da compilare
            if (d.DocumentTypeId.ToLower() != "$t$")
                TemplateManager.Fill(b, d, tp, data);
            //TODO: Verifico se si tratta di FE e se devo modificare il progressivo interno
            if (!fext.EndsWith(".pdf"))
            {
                TemplateManager.FixFE(d, data);
            }

            // Produco il pdf eventualmente etichettato
            // Se il file originale era un pdf, prendo il nuovo

            //string pdffile = PreviewManager.CreatePreview(d, tp, PageIndex, X, Y, User, filename, Stamp);
            //    if (!String.IsNullOrEmpty(pdffile) && (fext.EndsWith(".pdf")))
            //    {
            //        filename = pdffile;
            //        pdffile = "";
            //        tempfile = true;
            //    }





            var R = new DocumentImage ();
            if (d.ImageId.HasValue)
                R = await docRepo.GetImage(d.ImageId.Value);


            bool RdC = userInfo.Roles.Contains("Responsabile Conservazione")
                || userInfo.Roles.Contains("Delegato Conservazione");

            bool admin = userInfo.Roles.Contains("Admin")
                || userInfo.Roles.Contains("Gestore Banca Dati " + d.CompanyId)
                || !userInfo.Roles.Contains("User");

            if (R.PreservationStatus == JobStatus.Done)
            {
                logger.LogWarning("L'utente " + UserId + " ha tentato di sovrascrivere il documento: " + d.Id.ToString() + " che risulta in conservazione.");
                return CONST_ADDIMAGE_PERSISTED;
            }



            return await _InternalAddImage(d, userInfo, filename, data, NewVersion);
        }


        private async Task<int> _InternalAddImage(Document d, UserInfo userInfo, string filename, byte[] data, bool CreateNewVersion)
        {
            int Id = 0;
            try
            {
                var R = await SaveImage(d, userInfo, filename, data, CreateNewVersion);
                if (R.Id > 0)
                {
                    d.ImageId = R.Id;
                    d.Image = R;
                }
                Id = R.Id;
            }
            catch (Exception ex)
            {
                Id = CONST_ADDIMAGE_DBERROR;
                logger.LogError("Impossibile scrivere l'immagine del doc." + d.Id.ToString() + ": " + ex.Message);

                throw;
            }
            return Id;
        }





        public async Task<bool> Overlay(Document d, int Page, decimal X, decimal Y, UserInfo userInfo, string Image, bool Log = false, bool forcePDF = false)
        {
            bool ok = false;
            var R = new DocumentImage();
            if (d.ImageId.HasValue) R = await docRepo.GetImage(d.ImageId.Value);
            if (R.Id > 0)
            {
                bool OnPreview = false;
                string fname = R.FileName;
                OnPreview = Path.GetExtension(fname).ToLower() != ".pdf" && !forcePDF;

                var FM = filesystemFactory.InstanceOf(R.FileManager);
                string pdffile = Path.ChangeExtension(fname, ".pdf");
                try
                {
                    FileProperty F = null;
                    using (var Source = FM.ReadAsStream(pdffile))
                    {
                        F = FilePropertyHelper.Get(Source);
                    }
                    if (F != null)
                    { 
                        using (var PdfStream = F.OverlayFile(Page, Image, (double)X, (double)Y, true, true))
                        {
                            if (OnPreview)
                            {
                                if (FM.WriteFromStream (pdffile, PdfStream))
                                    ok = true;
                            }
                            else
                            {
                                int r = _InternalAddImage(d, userInfo, fname, PdfStream, false);
                                ok = r > 0;
                            }
                        }
                    }
                } catch (Exception ex)
                {
                    logger.LogError(ex, "Overlay");
                    ok = false;
                }
            };
            return ok;
        }



    */

}
