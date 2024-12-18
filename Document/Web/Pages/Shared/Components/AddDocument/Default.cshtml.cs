using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Services;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Entities.Schemas;

namespace Web.Pages.Shared.Components.AddDocument
{
    public class AddDocument : ViewComponent
    {
        private readonly IConfiguration _config;
        private readonly IDocumentTypeService documentTypeService;
        private readonly IACLService aclService;
        private readonly IDocumentService documentService;
        private readonly ISearchService searchService;
        private readonly IUserService userService;
        private readonly ILoggedUserProfile userContext;

        //public List<DocumentType> NewFolders { get; set; } = new();

        public bool CanCreate { get; set; }


        //public bool CanCreateDocument { get; set; }
        //public bool CanCreateRootFolder { get; set; }

        //public bool CanCreateGenericDocument { get; set; }
        //public bool CanSendMail { get; set; }
        //public bool CanAddAttachment { get; set; }

        public int ParentId { get; set; } = 0;
        //public string DocumentType { get; set; } 
        //public string DocumentTypeName { get; set; }
        //public ContentType ContentType { get; set; }

        public AddDocument(IConfiguration config,
            IDocumentTypeService documentType, 
            IACLService aclService,
            IDocumentService documentService,
            ISearchService searchService,
            IUserService userService,
            ILoggedUserProfile usercontext)
        {
            _config = config;
            this.documentTypeService = documentType;
            this.aclService = aclService;
            this.documentService = documentService;
            this.searchService = searchService;
            this.userService = userService;
            this.userContext = usercontext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var u = userContext.Get();
            //ContentType = ContentType.Folder;

            if (Request.Query.ContainsKey("id") && int.TryParse(Request.Query["id"], out int id))
            {
                ParentId = id;
            }
            //if (Request.Query.ContainsKey("DocumentType"))
            //{
            //    DocumentType = Request.Query["DocumentType"].ToString();
            //    var type = await documentTypeService.GetById(DocumentType);
            //    DocumentTypeName = type.Name;
            //    ContentType = type.ContentType;
            //}
            int types = 0;

            var CanCreateRootFolder = await aclService.GetAuthorization("", u, PermissionType.Profile_CanCreateRootFolder) == AuthorizationType.Granted;
            var CanCreateGenericDocument = (await aclService.GetAuthorization("", u, PermissionType.Profile_CanCreateGenericDocument) == AuthorizationType.Granted);
            if (ParentId > 0)
            {
                var doc = await documentService.Get(ParentId);
                var CanAddContent = ((await documentService.GetPermission(ParentId, u, PermissionType.CanAddContent)).Authorization == AuthorizationType.Granted) && doc.DocumentStatus != DocumentStatus.Stored && doc.DocumentStatus != DocumentStatus.Deleted;
                CanCreateRootFolder = doc.ContentType == ContentType.Folder && CanAddContent;

                if (CanCreateGenericDocument)
                    CanCreateGenericDocument = CanAddContent && doc.ContentType == ContentType.Folder;

                if (doc.ContentType == ContentType.Folder)
                    types = (await documentTypeService.GetByPermission(u, PermissionType.CanCreate, "", ""))
                        .Count(t => t.ContentType != ContentType.Folder
                                   && !t.Internal
                                );
            }
            else
            {
                types = (await documentTypeService.GetByPermission(u, PermissionType.CanCreate, "", ""))
                    .Count(t => t.ContentType != ContentType.Folder
                               && !t.Internal
                            );

            }
            CanCreate = types > 0 || CanCreateRootFolder || CanCreateGenericDocument;
            return View(this);
        }
    }

}
