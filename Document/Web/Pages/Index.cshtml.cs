using Core.DigitalSignature;
using Core.DigitalSignature.Pkcs11;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.core.Mappers;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using Web.DTOs;
using Web.Utilities;

namespace Web.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IDocumentService documentService;
        private readonly IACLService aclService;
        private readonly ILoggedUserProfile userContext;
        private readonly IUserService userService;
        private readonly IUISettingsRepository uiSettingsRepository;
        private readonly IAppSettingsRepository appSettings;


        private readonly IUserTaskService userTaskService;
        private readonly IHistoryRepository historyRepo;
        private readonly ILookupTableService lookupService;


        private static Dictionary<int, SignatureInfo[]> SignatureCache  = new ();



        public int DocumentId { get; set;  }
        // public string? token { get; set; }
        public string Viewer { get; set; }
        public string Editor { get; set; }
        public string FileContent { get; set; }
        public bool ModalView { get; set; }
        public int imageId { get; set; }

        public DocumentInfo Document { get; set; }
        public FileExplorerModel FileFilters { get; set; }

        //public IList<HistoryEntry> HistoryEntries { get; set; } = null;
        public List<DocumentVersion> Versions { get; set; }
        public int FoldersCount { get; set; }
        public int AttachmentsCount { get; set; }
        public int AttachmentsOfCount { get; set; }
        public int LinksCount { get; set; }
        public bool CanView { get; set; }
        public bool CanViewContent { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanAddContent { get; set; }
        public bool CanRemoveContent { get; set; }
        public bool CanShare { get; set; }
        public bool CanEditForm { get; set; }
        public bool CanEditData { get; set; }
        public bool CanSend{ get; set; }
        public bool CanDeploy{ get; set; }
        public bool CanAuthorize{ get; set; }
        public bool CanViewRegistry { get; set; }
        public bool CanViewTask { get; set; }
        public bool CanSign { get; set; }
        public bool IsAdmin { get; set; }

        public bool HasSignature { get; set; }
        public bool HasVisto { get; set; }
        public string DocumentTypeName { get; set; }

        public string DocumentStatusIcon { get; set; }

        public ArchivingStrategy ArchivingStrategy { get; set; }

        public SignatureInfo[] Signatures { get; set; } = new SignatureInfo[0];


        public IndexModel(ILogger<IndexModel> logger,
            IDocumentService documentService,
            ILoggedUserProfile userContext,
            ILookupTableService lookupService,
            IACLService aclService,
            IUserService userService,
            IUISettingsRepository uiSettingsRepository,
            IUserTaskService userTaskService,
            IHistoryRepository historyRepo,
            IAppSettingsRepository appSettings
            )
        {
            _logger = logger;
            this.documentService = documentService;
            this.userContext = userContext;
            this.lookupService = lookupService;
            this.aclService = aclService;
            this.userService = userService;
            this.uiSettingsRepository = uiSettingsRepository;
            this.userTaskService = userTaskService;
            this.historyRepo = historyRepo;
            this.appSettings = appSettings;
        }

        public async Task<IActionResult> OnGet(int? id)
        {
            var u = userContext.Get();
            if (id == null)
            {
                var uid = Request.Query["UID"];
                if (!string.IsNullOrEmpty(uid))
                {
                    var did = await documentService.FindByUniqueId(null, uid, ContentType.Document);
                    if (did <= 0)
                        did = await documentService.FindByUniqueId(null, uid, ContentType.Folder);
                    if (did > 0)
                    {
                        return Redirect("/Index/?id=" + did);
                    }
                }
            }
            DocumentId = id ?? 0;
            if (DocumentId <= 0)
            {
                var home = id == null ? await uiSettingsRepository.Get(u.userId, "HomePage") : "";
                if (string.IsNullOrEmpty(home) || home == "/") home = "/Folders/Index";
                return Redirect (home);
            }
            CanView = (await documentService.GetPermission(DocumentId, u, PermissionType.CanView)).Authorization == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;
            if (!CanView)
            {
                return this.Forbid(); //.Unauthorized();
            }

            //token =  await this.HttpContext.GetTokenAsync("access_token");
            //u = UserProfile.SystemUser();
            Document = await documentService.Load(DocumentId, u);
            Versions = await documentService.Images(DocumentId, u);
            if (Request.Query.ContainsKey("modalview"))
            {
                ModalView = Request.Query["modalView"] == "S";
            }
            if (Request.Query.ContainsKey("imageId"))
            {
                ModalView = true;
                imageId = int.Parse("0"+Request.Query["imageId"]);
                if (imageId > 0)
                // Devo mostrare una versione specifica
                    Document.Image = await documentService.GetContentInfo(imageId);
            }
            AttachmentsCount = (await documentService.Links(DocumentId, u, true)).Count();
            AttachmentsOfCount = (await documentService.LinkedIn(DocumentId, u, true)).Count();

            LinksCount = (await documentService.Links(DocumentId, u, false)).Count();
            //var P = await documentService.GetPermissions(DocumentId, u);
            //HistoryEntries = await GetHistory(0, "", 0);
            FoldersCount = (await documentService.GetDocumentFolders(DocumentId, u)).Count();

            //var CanHandleDocuments = u.Roles.Select(s => s.Id).Contains("admin")
            //        || (await aclService.GetAuthorization("", u, PermissionType.Profile_CanHandleDocuments) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted);

            //PermissionsCount = (await documentService.GetDocumentPermissions(DocumentId)).Count();

            IsAdmin = u.GlobalRoles.Select(s => s.Id).Contains("admin");

            CanViewContent = (await documentService.GetPermission(DocumentId, u, PermissionType.CanViewContent)).Authorization == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;
            CanEdit =  (IsAdmin || (await documentService.GetPermission(DocumentId, u, PermissionType.CanEdit)).Authorization == OpenDMS.Domain.Enumerators.AuthorizationType.Granted);
            CanDelete = (IsAdmin || (await documentService.GetPermission(DocumentId, u, PermissionType.CanDelete)).Authorization == OpenDMS.Domain.Enumerators.AuthorizationType.Granted);
            CanAddContent = (await documentService.GetPermission(DocumentId, u, PermissionType.CanAddContent)).Authorization == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;
            CanRemoveContent= (await documentService.GetPermission(DocumentId, u, PermissionType.CanRemoveContent)).Authorization == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;
            CanAuthorize = (await documentService.GetPermission(DocumentId, u, PermissionType.CanAuthorize)).Authorization == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;
            CanShare =  (await documentService.GetPermission(DocumentId, u, PermissionType.CanShare)).Authorization == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;
            CanSend = CanShare 
                && ((await userService.GetAllContactDigitalAddress(u.userId)).Where(x => !x.Deleted).Count() > 0)
                && (await aclService.GetAuthorization("", u, PermissionType.Profile_CanSendMail) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted);

            CanEditForm = false;
            var HasForm = !String.IsNullOrEmpty(Document.DocumentType.CreationFormKey);
            // 1. Devo provenire da form
            // 2. Devo essere un documento
            // 3. Non devo essere firmato
            // 4. Devo avere il permesso di aggiungere immagini 
            //        e il file deve essere un html oppure devo essere il proprietario
            //    oppure 
            //    Ho salvato come bozza e sono il proprietario
            if (HasForm)
            {
                if (Document.Image != null && Document.Image.Signatures == 0)
                {
                    if (Document.DocumentType.ContentType == ContentType.Document)
                    {
                        if (
                            ((CanAddContent || CanEdit) && (u.userId.Equals(Document.Owner, StringComparison.InvariantCultureIgnoreCase)))
                            || 
                            (Document.DocumentStatus == DocumentStatus.Draft && (u.userId.Equals(Document.Owner, StringComparison.InvariantCultureIgnoreCase)))
                           )
                        {
                            CanEditForm = true;
                        }
                    }
                }
            }
            CanEditData = false;
            if (Document.ContentType == OpenDMS.Domain.Enumerators.ContentType.Form || Document.ContentType == OpenDMS.Domain.Enumerators.ContentType.Template)
                if (Document.Image.FileName.ToLower().EndsWith(".html")
                    || Document.Image.FileName.ToLower().EndsWith(".report")
                    || Document.Image.FileName.ToLower().EndsWith(".htm")
                    || Document.Image.FileName.ToLower().EndsWith(".txt")
                    || Document.Image.FileName.ToLower().EndsWith(".xml")
                    || Document.Image.FileName.ToLower().EndsWith(".json")
                    || Document.Image.FileName.ToLower().EndsWith(".md")
                    )
                    CanEditData = true;

            CanDeploy = (await documentService.GetPermission(DocumentId, u, PermissionType.CanPublish)).Authorization == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;
            CanViewRegistry = (await documentService.GetPermission(DocumentId, u, PermissionType.CanViewRegistry)).Authorization == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;
            CanViewTask = (await aclService.GetAuthorization("", u, PermissionType.Task_CanView)) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;


            CanSign = (/*CanHandleDocuments &&*/ (await aclService.GetAuthorization("", u, PermissionType.Profile_CanHaveSignature)) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted); ;
            HasSignature = CanSign;
            if (CanSign)
            {
                if (userService.GetUserStamp(u.userId, ProfileType.User, "Sign") == null &&
                    userService.GetUserStamp(u.userId, ProfileType.User, "Visto") == null)
                        HasSignature = false;

//                var user = await userService.GetById(u.userId);
//                var remoteSignurl = await userService.Settings.Get(user.ContactId, OpenDMS.Domain.Constants.UserAttributes.CONST_REMOTESIGNATURE_SERVICE);
//                var otpserviceurl = await userService.Settings.Get(user.ContactId, OpenDMS.Domain.Constants.UserAttributes.CONST_OTPSIGNATURE_SERVICE);
//                var feaserviceurl = await userService.Settings.Get(user.ContactId, OpenDMS.Domain.Constants.UserAttributes.CONST_FEASIGNATURE_VENDOR);
//                CanSign = !String.IsNullOrEmpty(remoteSignurl + otpserviceurl + feaserviceurl) || HasSignature;
            }
            //CanCreate = (await documentTypeService.GetByPermission(u, PermissionType.CanCreate, "", "")).Count > 0;
            //CanCreateDocument = await documentTypeService.CanCreateRootFolder(u); 
            //CanAddAttachment = CanAddContent && (Document.Image == null || !(Document.Image.FileName.ToLower().EndsWith(".eml")));
            DocumentTypeName = Document.DocumentTypeName;
            switch (Document.DocumentStatus)
            {
                case DocumentStatus.Draft:
                    DocumentStatusIcon = "fa fa-pencil";
                    break;
                case DocumentStatus.Stored:
                    DocumentStatusIcon = "fa fa-lock";
                    break;
                case DocumentStatus.Deleted:
                    DocumentStatusIcon = "fa fa-trash-o";
                    break;
                default:
                    break;
            }
            if (Document.ContentType == ContentType.Folder)
            {
                var personal = await documentService.GetUserFolder(u);
                if (Document.Path.FirstOrDefault(p => p.Id == personal.ToString()) != null)
                    DocumentTypeName = "Fascicolo Personale";
                Viewer = "Viewer_Files";
                Editor = "";
            }
            else
            {
                if (Document.Image != null)
                {
                    string internalextension = "";
                    try
                    {
                        if (SignatureCache.ContainsKey(Document.Image.Id))
                            Signatures = SignatureCache[Document.Image.Id];
                        else
                        {
                            using (var content = new MemoryStream(await documentService.GetContent(Document.Image.Id)))
                            {
                                Signatures = content.GetSignatureInfo();
                                internalextension = content.FindFileType();
                                SignatureCache[Document.Image.Id] = Signatures;
                            }
                        }
                    }
                    catch {
                        //TODO: segnalare che il file non è leggibile
                    };

                    var Folders = await documentService.GetDocumentFolders(Document.Id, u);
                    if (Folders.Count > 0)
                    {
                        var f = await documentService.Load(Folders[Folders.Count -1], u);
                        Document.Path = f.Path;
                        Document.Path.Add(new LookupTable() { Id = f.Id.ToString(), Description = f.Description });
                    };

                    bool signed = false;
                    string name = Document.Image?.FileName ?? "";
                    var ext = Path.GetExtension(name).ToLower();
                    while (ext.Length > 0 && (ext.EndsWith(".m7m") || ext.EndsWith(".p7m") || ext.EndsWith(".tsa") || ext.EndsWith(".tsd")))
                    {
                        name = Path.GetFileNameWithoutExtension(name) ?? "";
                        ext = Path.GetExtension(name).ToLower();
                        signed = true;
                    }

                    //var ext = Path.GetExtension(Document.Image?.FileName).ToLower();
                    //while (ext.Length > 0 && (ext.EndsWith(".m7m") || ext.EndsWith(".p7m") || ext.EndsWith(".tsa") || ext.EndsWith(".tsd")))
                    //{
                    //    ext = ext.Substring(0, ext.Length - 4);
                    //    signed = true;
                    //}
                    // se non ho estensione, caso tipico dei file p7m, provo a ricavare il tipo di file dal contenuto
                    if (signed && string.IsNullOrEmpty (ext))
                    {
                        if (Signatures.Length > 0)
                            ext = Signatures[Signatures.Length - 1].FileExtension;
                        else
                            ext = internalextension;
                    }
                    switch (ext)
                    {
                        case ".report":
                        case ".txt":
                        case ".htm":
                        case ".html":
                        case ".log":
                        case ".xml":
                        case ".json":
                        case ".md":
//                            if ((Document.ContentType == ContentType.Form || Document.ContentType == ContentType.Template))
//                                Viewer = "Viewer_FORMHTML";
//                            else
                            Viewer = "Viewer_HTML";
                            Editor = "";
                            break;
                        case ".eml":
                        case ".msg":
                            Viewer = "Viewer_EML";
                            Editor = "";
                            break;
                        case ".bmp":
                        case ".jpg":
                        case ".jpeg":
                        case ".png":
                        case ".tiff":
                            Viewer = "Viewer_Image";
                            Editor = "";
                            break;

                        case ".bpmn":
                            Viewer = "Viewer_BPMN";
                            Editor = "modeler";
                            break;
                        case ".form":
                        case ".formio":
                        case ".formjs":
                            Viewer = "Viewer_FORM";
                            Editor = "form";
                            break;
                        case ".formhtml":
                            Viewer = "Viewer_HTML";
                            Editor = "form";
                            break;
                        case ".dmn":
                            Viewer = "Viewer_DMN";
                            Editor = "dmn";
                            break;
                        case ".zip":
                        case ".rar":
                        case ".7z":
                        case ".tar":
                            Viewer = "Viewer_Zip";
                            Editor = "";
                            break;
                        case ".mp4":
                        case ".avi":
                        case ".ogg":
                            Viewer = "Viewer_Video";
                            Editor = "";
                            break;
                        default:
                            Viewer = "Viewer_PDF";
                            Editor = "";
                            break;
                    }
                } else
                {
                    Viewer = "Viewer_NoFile";
                    Editor = "";
                }
            }

            FileFilters = await ControllerUtility.GetFilters(Request, "", Document.DocumentType);
            if (FileFilters.Request.PageSize == 0)
                FileFilters.Request.PageSize = int.Parse("0" + (await appSettings.Get("Document.List.PageSize")));
            if (FileFilters.Request.PageSize == 0)
            {
                FileFilters.Request.PageSize = 25;
            }

            FileFilters.ShowRemoveFromFolder = CanRemoveContent;
            FileFilters.ShowDelete = true;
            if (DocumentId > 0)
                await documentService.View(Document, u);

            return Page();
        }


        public async Task<IActionResult> OnGetFolders (int documentId)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            var Folders = new List<DocumentInfo>();
            foreach (var f in await documentService.GetDocumentFolders(documentId, u))
                Folders.Add(await documentService.Load(f, u));
            return new JsonResult(Folders);
        }


        public async Task<IActionResult> OnGetHistories(int documentId, int PageIndex, string SearchText, int Events)
        {
            var e = new List<string>();
            switch (Events)
            {
                case 1: // smistamento / condivisione
                    e.Add(EventType.Share);
                    e.Add(EventType.Print);
                    e.Add(EventType.Download);
                    e.Add(EventType.Authorize);
                    break;
                case 2: // firme 
                    e.Add(EventType.AddBiometricalSignature);
                    e.Add(EventType.AddDigitalSignature);
                    e.Add(EventType.AddUserSignature);
                    e.Add(EventType.AddRemoteDigitalSignature);
                    break;
                case 3: // spedizioni 
                    e.Add(EventType.EMail);
                    e.Add(EventType.Publish);
                    e.Add(EventType.ExcludeFromSending);
                    e.Add(EventType.PrepareForSending);
                    break;
                case 4: // modifiche 
                    e.Add(EventType.Preservation);
                    e.Add(EventType.Protocol);
                    e.Add(EventType.Creation);
                    e.Add(EventType.Classify);
                    e.Add(EventType.Update);
                    e.Add(EventType.Delete);
                    e.Add(EventType.UnDelete);
                    e.Add(EventType.ChangeStatus);
                    break;
                case 5: // versioni 
                    e.Add(EventType.AddSignatureField);
                    e.Add(EventType.AddPreservationSignature);
                    e.Add(EventType.AddProtocolSign);
                    e.Add(EventType.AddStamp);
                    e.Add(EventType.AddTextToContent);
                    e.Add(EventType.AddTextToContent);
                    e.Add(EventType.AddCheckSign);
                    e.Add(EventType.EraseContent);
                    e.Add(EventType.HighlightContent);
                    e.Add(EventType.ObscureContent);

                    e.Add(EventType.CheckIn);
                    e.Add(EventType.CheckOut);
                    e.Add(EventType.AddVersion);
                    e.Add(EventType.AddRevision);
                    e.Add(EventType.RemoveRevision);
                    e.Add(EventType.RemoveVersion);
                    break;
                case 6: // fascicolazioni 
                    e.Add(EventType.AttachRemoved);
                    e.Add(EventType.AddAttach);
                    e.Add(EventType.AddToFolder);
                    e.Add(EventType.RemoveFromFolder);
                    e.Add(EventType.AddLink);
                    e.Add(EventType.LinkRemoved);
                    break;
                case 7: // visualizzazione 
                    e.Add(EventType.View);
                    e.Add(EventType.RunProcess);
                    break;
                default:
                    break;
            }
            HistoryFilters filters = new HistoryFilters()
            {
                DocumentId = documentId,
                PageSize = 25,
                Search = SearchText,
                PageIndex = PageIndex,
                Events = e
            };
            var entries = await historyRepo.GetByFilters(filters);
            var history = new List<HistoryItem>();
            foreach (var entry in entries)
            {
                HistoryItem historyItem = new HistoryItem();
                historyItem.UserId = entry.UserId;
                historyItem.Sender = await userService.GetName(entry.UserId);
                var l = await lookupService.GetById("$EVENTS$", entry.EventType.ToString());
                historyItem.Description = entry.Description;
                historyItem.EventDateTime = entry.CreationDate.Date==DateTime.Now.Date ? "Oggi"+entry.CreationDate.ToString(" HH:mm") : entry.CreationDate.ToString("dd/MM/yyyy HH:mm");
                historyItem.EventType = string.IsNullOrEmpty(l.Description) ? entry.EventType.Replace("Document.", "") : l.Description;
                historyItem.ProcessName = entry.WorkflowId;
                historyItem.TaskId = entry.TaskId;
                historyItem.DeputyUser = await userService.GetName(entry.DeputyUserId);
                historyItem.To = entry.Recipients.Where(r => !r.CC).Select(r => userService.GetProfileName(r.ProfileId, r.ProfileType).GetAwaiter().GetResult()).ToList();
                historyItem.CC = entry.Recipients.Where(r => r.CC).Select(r => userService.GetProfileName(r.ProfileId, r.ProfileType).GetAwaiter().GetResult()).ToList();
                historyItem.Details = new HistoryDetailFormatter().Format(entry);
                history.Add(historyItem);
            }

            return new JsonResult(history);
        }

        public async Task<IActionResult> OnGetAttachments(int documentId)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            var Attachments = await documentService.Links(documentId, u, true);
            return new JsonResult(Attachments);
        }
        public async Task<IActionResult> OnGetAttachmentsOf(int documentId)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            var Attachments = await documentService.LinkedIn(documentId, u, true);
            return new JsonResult(Attachments);
        }
        public async Task<IActionResult> OnGetRemoveAttachment(int documentId, int attachmentId)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            if (documentId != attachmentId)
            {
                var errors = await documentService.RemoveLink (documentId, attachmentId, u, true);
                if (!errors)
                {
                    return new JsonResult("Impossibile rimuovere l'allegato");
                }
            }
            return await OnGetAttachments(documentId);
        }

        public async Task<IActionResult> OnGetTasks(int documentId)
        {
            var u = userContext.Get();
            List<UserTaskInfo> list = new List<UserTaskInfo>();
            foreach (var i in  await userTaskService.GetByDocument(documentId, u))
            {
                list.Add(await userTaskService.GetById(i.Id, u));
            }
            return new JsonResult(list);
        }

        public async Task<IActionResult> OnGetBPMs(int documentId)
        {
            var u = await userService.GetUserProfile(SpecialUser.SystemUser);
            List<UserTaskInfo> Active = new List<UserTaskInfo>();
            List<UserTaskInfo> Closed = new List<UserTaskInfo>();
            foreach (var i in await userTaskService.GetAllDocumentTasks(documentId, u))
            {
                var ut = await userTaskService.GetById(i.Id, u);
                if (ut != null)
                {
                    //if (!string.IsNullOrEmpty(ut.TaskItemInfo.Process.Id))
                    if (i.Status != ExecutionStatus.Executed && i.Status != ExecutionStatus.Validated)
                        Active.Add(await userTaskService.GetById(i.Id, u));
                    else
                        Closed.Add(await userTaskService.GetById(i.Id, u));
                }
            }

            return new JsonResult(new { active= Active, closed= Closed });
        }

        public async Task<IActionResult> OnGetLinks(int documentId)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            var Links = await documentService.Links(documentId, u, false);
            return new JsonResult(Links);
        }
        public async Task<IActionResult> OnGetRemoveLink(int documentId, int linkId)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            if (documentId != linkId)
            {
                var errors = await documentService.RemoveLink(documentId, linkId, u, false);
                if (!errors)
                {
                    return new JsonResult("Impossibile rimuovere il collegamento");
                }
            }
            return await OnGetLinks(documentId);
        }
        
        
        public async Task<IActionResult> OnGetVersions(int documentId)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            var Versions = await documentService.Images(documentId, u);
            return new JsonResult(Versions);
        }
        public async Task<IActionResult> OnGetRestoreVersion(int documentId, int imageId)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            var Document = await documentService.Load(documentId, u);
            if (Document.Image.Id != imageId)
            {
                var contentInfo = await documentService.GetContentInfo(imageId);
                FileContent content = new FileContent() { FileName = contentInfo.OriginalFileName, FileData= Convert.ToBase64String(await documentService.GetContent(imageId)), LinkToContent=false };
                await documentService.AddContent(documentId, u, content, true);
            }
            return await OnGetLinks(documentId);
        }


        public async Task<IActionResult> OnGetPermissions(int documentId)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            var P = (await documentService.GetDocumentPermissions(documentId)).Where(p=>p.ProfileId != SpecialUser.SystemUser).ToList() ;
            return new JsonResult(P);
        }







        public string FormatFileSize (long fileSize)
        {
            if (fileSize < 1000)
            {
                return fileSize.ToString()+" B";
            }
            else
                if (fileSize < 1000_000)
                {
                    return (fileSize / 1000).ToString("##0") + " K";
                }
                else
                if (fileSize < 1000_000_000)
                {
                    return (fileSize / 1000_000).ToString("##0") + " M";
                }
            else
                {
                    return (fileSize / 1000_000_000).ToString("##0") + " G";
                }
        }


  
    }
}