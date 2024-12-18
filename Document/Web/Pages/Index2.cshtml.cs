using Core.DigitalSignature.Pkcs11;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using Web.DTOs;

namespace Web.Pages
{
    [Authorize]
    public class IndexModel2 : PageModel
    {
        private readonly IDocumentService documentService;
        private readonly ILoggedUserProfile userContext;
        private readonly IUISettingsRepository uiSettingsRepository;
        private readonly IUserService userService;


        public int DocumentId { get; set; }
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
        public bool CanSend { get; set; }
        public bool CanDeploy { get; set; }
        public bool CanAuthorize { get; set; }
        public bool CanViewRegistry { get; set; }
        public bool CanViewTask { get; set; }
        public bool CanSign { get; set; }

        public bool HasSignature { get; set; }
        public bool HasVisto { get; set; }
        public string DocumentTypeName { get; set; }

        public string DocumentStatusIcon { get; set; }

        public ArchivingStrategy ArchivingStrategy { get; set; }

        public SignatureInfo[] Signatures { get; set; } = new SignatureInfo[0];



        public IndexModel2(IDocumentService documentService,
            ILoggedUserProfile userContext,
            IUISettingsRepository uiSettingsRepository,
            IUserService userService
            )
        {
            this.documentService = documentService;
            this.userContext = userContext;
            this.uiSettingsRepository = uiSettingsRepository;
            this.userService = userService;
        }

        public async Task<IActionResult> OnGet(int? id, string uid, ContentType contentyType = ContentType.Form)
        {
            var u = userContext.Get();
            var DocumentId = id ?? 0;
            if (!String.IsNullOrEmpty(uid))
            {
                DocumentId = await documentService.FindByUniqueId(null, uid, contentyType);
            }
            if (DocumentId <= 0)
            {
                var home = await uiSettingsRepository.Get(u.userId, "HomePage");
                if (string.IsNullOrEmpty(home) || home=="/") home = "Folders/Index";
                return Redirect (home);
            }
            var CanView = (await documentService.GetPermission(DocumentId, u, PermissionType.CanView)).Authorization == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;
            if (!CanView)
                return Unauthorized();

            var Document = await documentService.Get(DocumentId);

            var p = Document.DocumentType?.DetailPage;
            if (string.IsNullOrEmpty(p))
                p = "Details/Index";

            return Redirect(p + this.HttpContext.Request.QueryString.ToString());
        }


  
    }
}