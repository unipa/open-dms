using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;

namespace Web.Pages
{

    [Authorize]
    public class NewProcessModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IDocumentService documentService;
        private readonly ISearchService searchService;
        private readonly IDocumentTypeService documentTypeService;
        private readonly IUserService userService;
        private readonly ILookupTableService tableService;
        private readonly IACLService aclService;
        private readonly ILoggedUserProfile userContext;
        private readonly IUISettingsRepository uISettingsRepository;
        private readonly IConfiguration _config;

        public NewProcessModel(ILogger<IndexModel> logger,
            IDocumentService documentService,
            ISearchService searchService,
            IDocumentTypeService documentTypeService,
            IUserService userService,
            ILookupTableService tableService,
            IACLService aclService,
            ILoggedUserProfile userContext,
            IUISettingsRepository uISettingsRepository,
            IConfiguration config)
        {
            _logger = logger;
            this.documentService = documentService;
            this.searchService = searchService;
            this.documentTypeService = documentTypeService;
            this.userService = userService;
            this.tableService = tableService;
            this.aclService = aclService;
            this.userContext = userContext;
            this.uISettingsRepository = uISettingsRepository;
            _config = config;
        }

        //public bool CanSendMail { get; set; }
        //public bool CanCreateTask { get; set; }
        //public bool CanCreateMessage  { get; set; }
        public string? ErrorMessage { get; set; } = "";
        public int FolderId { get; set; } = 0;

        public List<Document> Processi { get; set; } = new List<Document>();

        public async Task OnGet(int FolderId = 0)
        {
            var u = userContext.Get();
            try
            {
                List<SearchFilter> filters = new();
                filters.Add(new SearchFilter() { ColumnName = DocumentColumn.ContentType, Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { ((int)OpenDMS.Domain.Enumerators.ContentType.Document).ToString() } });
                filters.Add(new SearchFilter() { ColumnName = DocumentColumn.DocumentType, Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { "$DIAGRAM$" } });
                filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Status, Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { ((int)DocumentStatus.Active).ToString() } });
                //TODO: rimettere la riga sottostante
                filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Sending, Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { ((int)JobStatus.Completed).ToString() } });
                Processi = new();
                foreach (var d in await searchService.Find(filters, u, 25))
                {
                    var p = await documentService.GetPermission(d, u, PermissionType.CanExecute);
                    if (p != null && p.Authorization == AuthorizationType.Granted)
                    {
                        var doc = await documentService.Get(d);
                        Processi.Add(doc);
                    }
                }
                this.FolderId = FolderId;

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message.ToString();
            }
        }
    }
}