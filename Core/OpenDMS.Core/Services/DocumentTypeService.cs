using Microsoft.Extensions.Logging;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using System.Data;

namespace OpenDMS.Core.BusinessLogic
{
    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly IDocumentTypeRepository _documentTypeRepository;
        private readonly ILookupTableRepository _lookupTableRepository;
        private readonly IACLService _aclRepository;
        private readonly ITranslationRepository _translationRepository;
        private readonly ILogger<IDocumentTypeService> logger;

        public DocumentTypeService(
            ILogger<IDocumentTypeService> logger,
            IDocumentTypeRepository TP,
            ILookupTableRepository lookup,
           IACLService aclRepo,
            ITranslationRepository translate
            )
        {
            this._documentTypeRepository = TP;
            this._lookupTableRepository = lookup;
            this._aclRepository = aclRepo;
            this._translationRepository = translate;
            this.logger = logger;
        }

        public async Task<DocumentType> GetById(string codice)
        {
            DocumentType r = new DocumentType();
            if (!string.IsNullOrEmpty(codice)) r = await _documentTypeRepository.GetById(codice);
            if (r == null || string.IsNullOrEmpty(r.Id))
            {
                r = new DocumentType();
                r.DescriptionLabel = await _translationRepository.Get("Tipologia", "Descrizione") ?? "";
                r.DocumentDateLabel = await _translationRepository.Get("Tipologia", "Data Documento") ?? "";
                r.DocumentNumberLabel = await _translationRepository.Get("Tipologia", "Nr.Documento") ?? "";
                r.CategoryId = "";
                r.CreationFormKey = "";
                r.ACLId = "";
                r.CategoryId = "";
                r.Description = "";
                r.DetailPage = "";
                r.Direction = 0;
                r.Name = await _translationRepository.Get("Tipologia", string.IsNullOrEmpty(codice) ? "Documento Generico" : "Tipologia non trovata");
            }
            return r;
        }

        public async Task<int> Create(DocumentType docType)
        {
            if (string.IsNullOrEmpty(docType.Name)) throw new InvalidDataException("Non hai indicato un nome per la tipologia");

            if (string.IsNullOrEmpty(docType.Id)) docType.Id = Guid.NewGuid().ToString();

            if (string.IsNullOrEmpty(docType.ACLId) || await _aclRepository.GetById(docType.ACLId) == null)
            {
                if (await _aclRepository.GetById (docType.CategoryId) == null)
                {

                    var category = await _lookupTableRepository.GetById("$CATEGORIES$", docType.CategoryId);

                    var new_acl = new DTOs.CreateOrUpdateACL() { Id = docType.CategoryId, Name = category.Description };
                    await _aclRepository.Insert(new_acl);
                }
                docType.ACLId = docType.CategoryId;
            }
            if (string.IsNullOrEmpty(docType.ACLId)) throw new InvalidDataException("ACL non valida");


            int r = await _documentTypeRepository.Insert(docType);
            //if (r > 0)
            //{
            //    if (docType.ContentType == ContentType.Workflow)
            //    {
            //        Document processDocument = new Document()
            //        {
            //            CompanyId = 0,
            //            Description = docType.Name,
            //            Icon = docType.Icon,
            //            IconColor = docType.IconColor,
            //            ContentType = ContentType.Document,
            //            DocumentDate = DateTime.UtcNow,
            //            DocumentNumber = "",
            //            DocumentTypeId = "$DIAGRAM$",
            //            ACLId = docType.ACLId,
            //            Owner = userInfo.userId,
            //            ExternalId = $"${docType.Id}$"
            //        };
            //        try
            //        {
            //            await documentRepo.Create(processDocument);
            //            await
            //        }
            //        catch (Exception)
            //        {
            //            r = 0;
            //            await _documentTypeRepository.Delete(docType.Id);
            //        }
            //    }
            //}

            return r;
        }
        public async Task<int> Update(DocumentType docType)
        {
            int r = 0;
            if (string.IsNullOrEmpty(docType.Id)) throw new InvalidDataException("Codice non valido");
            if (string.IsNullOrEmpty(docType.Name)) throw new InvalidDataException("E' necessario indicare un nome");
            if (string.IsNullOrEmpty(docType.ACLId) || await _aclRepository.GetById(docType.ACLId) == null)
            {
                if (await _aclRepository.GetById(docType.CategoryId) == null)
                {

                    var category = await _lookupTableRepository.GetById("$CATEGORIES$", docType.CategoryId);

                    var new_acl = new DTOs.CreateOrUpdateACL() { Id = docType.CategoryId, Name = category.Description };
                    await _aclRepository.Insert(new_acl);
                }
                docType.ACLId = docType.CategoryId;
            }
            if (string.IsNullOrEmpty(docType.ACLId)) throw new InvalidDataException("ACL non valida");
            r = await _documentTypeRepository.Update(docType);
            return r;
        }

   

        public async Task<int> Delete(DocumentType docType)
        {
            if (string.IsNullOrEmpty(docType.Id)) throw new InvalidDataException("Codice non valido");
            int r = await _documentTypeRepository.Delete(docType.Id);
            return r;
        }

        public async Task<List<DocumentType>> Select()
        {
            return await _documentTypeRepository.GetAll();
        }

        public async Task<bool> CanCreateRootFolder (UserProfile userInfo)
        {
            var a = await _aclRepository.GetAuthorization("$GLOBAL$", userInfo.userId, ProfileType.User, PermissionType.Profile_CanCreateRootFolder);
            if (a == AuthorizationType.None)
            {
                foreach (var r in userInfo.Roles)
                {
                    a = await _aclRepository.GetAuthorization("$GLOBAL$", r.Id, ProfileType.Role, PermissionType.Profile_CanCreateRootFolder);
                    if (a != AuthorizationType.None) break;
                }

            }
            if (a == AuthorizationType.None)
            {
                foreach (var g in userInfo.Groups)
                {
                    a = await _aclRepository.GetAuthorization("$GLOBAL$", g.Id, ProfileType.Group, PermissionType.Profile_CanCreateRootFolder);
                    if (a != AuthorizationType.None) break;
                }
            }
            return a == AuthorizationType.Granted;
        }

        /// <summary>
        /// Ritorna l'elenco di tipologie in cui l'utente ha il permesso indicato
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="PermissionId"></param>
        /// <returns></returns>
        public async Task<List<DocumentType>> GetByPermission(UserProfile userInfo, string permissionId, string parent = ".", string filter = "")
        {
            //TODO: Verificase se uno dei ruoli ha il permesso di accesso completo alle tipologie
            // A parte SystemUser, gli altri (compreso admin) non devono fare eccezione

            var authorized = (userInfo.userId == SpecialUser.SystemUser || userInfo.IsService);

            List<DocumentType> found = new List<DocumentType>();
            HashSet<string> aclAuthorized = new HashSet<string>();
            var FilteredTypes = await _documentTypeRepository.GetAll();
            if (!String.IsNullOrEmpty(parent))
            {
                if (parent != ".")
                    FilteredTypes = FilteredTypes.Where(t => t.Id.StartsWith(parent + ".")).ToList();
                else
                    FilteredTypes = FilteredTypes.Where(t => !t.Id.Contains(".")).ToList();
            }
            if (!String.IsNullOrEmpty(filter))
                FilteredTypes = FilteredTypes.Where(t => t.Name.Contains(filter)).ToList();
            foreach (var T in FilteredTypes)
            {
                var acl = T.ACLId;
                if (!aclAuthorized.Contains(acl))
                {
                    AuthorizationType A = authorized ? AuthorizationType.Granted : AuthorizationType.None;
                    // verifico se l'utente ha un permesso sull'acl
                    if (A == AuthorizationType.None)
                    {
                        var p = await _aclRepository.GetAuthorization(acl, userInfo.userId, Domain.Enumerators.ProfileType.User, permissionId);
                        if (p == AuthorizationType.Granted)
                            A = p;
                    }
                    //if (A == AuthorizationType.None)
                    //{
                    //    var p = await _aclRepository.GetAuthorization("$GLOBAL$", userInfo.userId, Domain.Enumerators.ProfileType.User, permissionId);
                    //    if (p == AuthorizationType.Granted)
                    //        A = p;
                    //}
                    if (A == AuthorizationType.None)
                    {
                        // verifico se almeno uno dei ruoli dell'utente è autorizzato
                        foreach (var r in userInfo.Roles)
                        {
                            var pr = await _aclRepository.GetAuthorization(acl, r.Id, Domain.Enumerators.ProfileType.Role, permissionId);
                            if (pr == AuthorizationType.Granted)
                            {
                                A = pr;
                                break;
                            }
                            //pr = await _aclRepository.GetAuthorization("$GLOBAL$", r.Id, Domain.Enumerators.ProfileType.Role, permissionId);
                            //if (pr == AuthorizationType.Granted)
                            //{
                            //    A = pr;
                            //    break;
                            //}
                        }
                    }
                    if (A == AuthorizationType.None)
                    {
                        // verifico se almeno uno dei gruppi dell'utente è autorizzato
                        foreach (var g in userInfo.Groups.Where(g=>!(g.Id == null)) )
                        {
                            var p = await _aclRepository.GetAuthorization(acl, g.Id, Domain.Enumerators.ProfileType.Group, permissionId);
                            if (p == AuthorizationType.Granted)
                            {
                                A = p;
                                break;
                            }
                            //else
                            //{
                            //    p = await _aclRepository.GetAuthorization("$GLOBAL$", g.Id, Domain.Enumerators.ProfileType.Group, permissionId);
                            //    if (p == AuthorizationType.Granted)
                            //    {
                            //        A = p;
                            //        break;
                            //    }
                            //}

                            //var ruoloUtenteNelGruppo = await _organizationRepository.GetUser(g, userInfo.userId);
                            //// Se l'utente non ha un ruolo nel gruppo vuol dire che ha un permesso di visibilità trasversale
                            //if (ruoloUtenteNelGruppo == null)
                            //{
                            //    A = permissionId == PermissionType.CanView || permissionId == PermissionType.CanViewContent ? AuthorizationType.Granted : AuthorizationType.None;
                            //}
                            //else
                            //{
                            //    //TODO: recuperare il permesso del ruolo nella ACL
                            //    var pr = await _rolePermissionRepository.GetPermission(ruoloUtenteNelGruppo.RoleId, permissionId);
                            //    if (pr != null) A = pr.Authorization;
                            //}

                            //if (A == AuthorizationType.Granted)
                            //{
                            //    var pg = await _aclRepository.GetAuthorization(acl, g, Domain.Enumerators.ProfileType.Group, permissionId);
                            //    if (pg == AuthorizationType.Granted)
                            //    {
                            //        A = pg;
                            //        break;
                            //    }
                            //}
                        }
                    }
                    if (A == AuthorizationType.Granted)
                        aclAuthorized.Add(acl);
                }

                if (aclAuthorized.Contains(acl))
                    found.Add(T);
            }
            return found;
        }

        public async Task<DocumentType> GetByPermission(string Id, UserProfile userInfo, string permissionId)
        {
            var authorized = (userInfo.userId == SpecialUser.SystemUser || userInfo.IsService);

            DocumentType found = null;
            HashSet<string> aclAuthorized = new HashSet<string>();
            var T = await _documentTypeRepository.GetById(Id);
            {
                var acl = T.ACLId;
                if (!aclAuthorized.Contains(acl))
                {
                    AuthorizationType A = authorized ? AuthorizationType.Granted : AuthorizationType.None;
                    // verifico se l'utente ha un permesso sull'acl
                    if (A == AuthorizationType.None)
                    {
                        var p = await _aclRepository.GetAuthorization(acl, userInfo.userId, Domain.Enumerators.ProfileType.User, permissionId);
                        if (p == AuthorizationType.Granted)
                            A = p;
                    }
                    if (A == AuthorizationType.None)
                    {
                        // verifico se almeno uno dei ruoli dell'utente è autorizzato
                        foreach (var r in userInfo.Roles)
                        {
                            var pr = await _aclRepository.GetAuthorization(acl, r.Id, Domain.Enumerators.ProfileType.Role, permissionId);
                            if (pr == AuthorizationType.Granted)
                            {
                                A = pr;
                                break;
                            }
                        }
                    }
                    if (A == AuthorizationType.None)
                    {
                        // verifico se almeno uno dei gruppi dell'utente è autorizzato
                        foreach (var g in userInfo.Groups.Where(g => !(g.Id == null)))
                        {
                            var p = await _aclRepository.GetAuthorization(acl, g.Id, Domain.Enumerators.ProfileType.Group, permissionId);
                            if (p == AuthorizationType.Granted)
                            {
                                A = p;
                                break;
                            }
               
                        }
                    }
                    if (A == AuthorizationType.Granted)
                        aclAuthorized.Add(acl);
                }

                if (aclAuthorized.Contains(acl))
                    found = (T);
            }
            return found;
        }

        //public async Task<List<LookupTable>> SelectRegistry(List<DocumentType> Tipi)
        //{
        //    List<LookupTable> tt = new List<LookupTable>();
        //    foreach (var T in Tipi.Select(t => { return t.ProtocolRegister; }).Distinct())
        //    {
        //        LookupTable tti = await _lookupTableRepository.GetById("PRO", T);
        //        if (tti == null) continue;
        //        if (string.IsNullOrEmpty(tti.Description))
        //        {
        //            tti.Description = await _translationRepository.Get("Tipologia", "- Nessuno -");
        //            tti.Id = T;
        //        }
        //        tt.Add(tti);
        //    }
        //    tt.Sort(delegate (LookupTable t1, LookupTable t2) { return t1.Description.CompareTo(t2.Description); });
        //    return tt;
        //}

        public async Task<List<LookupTable>> SelectClasses(List<DocumentType> Tipi)
        {
            List<LookupTable> tt = new List<LookupTable>();
            foreach (var T in Tipi.Select(t => { return t.CategoryId; }).Distinct())
            {
                LookupTable tti = await _lookupTableRepository.GetById("$CATEGORIES$", T);
                if (tti == null) continue;
                if (string.IsNullOrEmpty(tti.Description))
                {
                    tti.Description = T;
                    tti.Id = T;
                }
                tt.Add(tti);
            }

            tt.Sort(delegate (LookupTable t1, LookupTable t2) { return t1.Description.CompareTo(t2.Description); });
            return tt;
        }



        public async Task<List<DocumentTypeWorkflow>> GetAllWorkflows(string documentTypeId)
        {
            return await _documentTypeRepository.GetAllWorkflows(documentTypeId);
        }
        public async Task<List<DocumentTypeWorkflow>> GetAllTypesWorkflows()
        {
            return await _documentTypeRepository.GetAllTypesWorkflows();
        }

        public async Task<int> AddWorkflow(DocumentTypeWorkflow definition)
        {
            return await _documentTypeRepository.AddWorkflow(definition);
        }
        public async Task<int> UpdateWorkflow(DocumentTypeWorkflow definition)
        {
            return await _documentTypeRepository.UpdateWorkflow(definition);

        }
        public async Task<int> RemoveWorkflow(DocumentTypeWorkflow definition)
        {
            return await _documentTypeRepository.RemoveWorkflow(definition);
        }
        public async Task<DocumentTypeWorkflow> GetWorkflow(string documentTypeId, string eventName)
        {
            return await _documentTypeRepository.GetWorkflow(documentTypeId, eventName);

        }

    }

}