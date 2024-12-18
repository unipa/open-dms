
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using Web.DTOs;
using Web.Utilities;

namespace Web.Pages.Dashboard
{

    [Authorize]
    public class DocumentsDashboardModel : PageModel
    {
        private readonly ILogger<DocumentsDashboardModel> _logger;
        private readonly IDocumentService documentService;
        private readonly ILoggedUserProfile userContext;
        private readonly IDocumentTypeService documentTypeService;
        private readonly ILookupTableService lookupTableService;
        private readonly IACLService aclService;
        private readonly IAppSettingsRepository appSettings;
        private readonly ISearchService searchRepository;

        public string DocumentTypeName { get; set; }
        public DocumentType DocumentType { get; set; }
        public string DocumentTypeId { get; set; }
        public string GroupName { get; set; }
        public string GroupType { get; set; }
        public string Icon { get; set; }
        public List<LookupTable> FilterList { get; set; } = new();


        public FileExplorerModel FileFilters { get; set; } = new();

        public DocumentsDashboardModel(ILogger<DocumentsDashboardModel> logger,
            IDocumentService documentService,
            ILoggedUserProfile userContext,
            IDocumentTypeService documentTypeService,
            ILookupTableService lookupTableService,
            IACLService aclService,
            IAppSettingsRepository appSettings,
            ISearchService searchRepository
            )
        {
            _logger = logger;
            this.documentService = documentService;
            this.userContext = userContext;
            this.documentTypeService = documentTypeService;
            this.lookupTableService = lookupTableService;
            this.aclService = aclService;
            this.appSettings = appSettings;
            this.searchRepository = searchRepository;
        }

        public async Task OnGet(string DocumentType)
        {
            //token =  await this.HttpContext.GetTokenAsync("access_token");
            var u = userContext.Get();
            DocumentTypeName = "Tutti i documenti";
            GroupName = "Tutte";
            GroupType = "";
            DocumentTypeId = DocumentType;

            Icon = "fa fa-file-pdf-o";
            var viewId = "doc.all";

            DocumentType doctype = null;

            if (!String.IsNullOrEmpty(DocumentType))
            {
                doctype = await documentTypeService.GetById(DocumentType);
                DocumentTypeName = doctype.Name;
                GroupType = doctype.CategoryId;
                var L = await lookupTableService.GetById("$CATEGORIES$", doctype.CategoryId);
                GroupName = string.IsNullOrEmpty(L.Id) ? Web.Constant.Labels.VirtualFolder : L.Description;
            }
            this.DocumentType = doctype;
            FileFilters = await ControllerUtility.GetFilters(Request, "", doctype);
            if (FileFilters.Request.PageSize == 0)
                FileFilters.Request.PageSize = int.Parse("0" + (await appSettings.Get("Document.List.PageSize")));
            if (FileFilters.Request.PageSize == 0)
            {
                FileFilters.Request.PageSize = 25;
            }
            if (FileFilters.Request.OrderBy.Count == 0)
            {
                FileFilters.Request.OrderBy.Add(new SortingColumn() { ColumnId = DocumentColumn.CreationDate, Descending = true });
            }
        }

    }
}