using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;

namespace Web.Pages.Folders
{

    [Authorize]
    public class DocumentTypesModel : PageModel
    {
        private readonly ILogger<DocumentTypesModel> _logger;
        private readonly IDocumentService documentService;
        private readonly ILoggedUserProfile userContext;
        private readonly IDocumentTypeService documentTypeService;
        private readonly ILookupTableService lookupTableService;
        private readonly IACLService aclService;
        private readonly ISearchService searchRepository;

        public List<FolderInfo> SearchTypes { get; set; } = new();

        public string GroupName { get; set; }

        public DocumentTypesModel(ILogger<DocumentTypesModel> logger,
            IDocumentService documentService,
            ILoggedUserProfile userContext,
            IDocumentTypeService documentTypeService,
            ILookupTableService lookupTableService,
            IACLService aclService,
            ISearchService searchRepository
            )
        {
            _logger = logger;
            this.documentService = documentService;
            this.userContext = userContext;
            this.documentTypeService = documentTypeService;
            this.lookupTableService = lookupTableService;
            this.aclService = aclService;
            this.searchRepository = searchRepository;
        }

        public async Task OnGet(string GroupType)
        {
            var u = userContext.Get();
            if (!String.IsNullOrEmpty(GroupType))
            {
                var L = await lookupTableService.GetById("$CATEGORIES$", GroupType);
                if (L.Id == GroupType)
                    GroupName = L.Description;
            }
            if (string.IsNullOrEmpty(GroupName)) GroupName = "Tutte le categorie";
            FolderInfo F1 = null; // new();
            //F1 = new();
            //F1.Name = "Tutti le tipologie";
            //F1.DocumentType = "";
            //F1.Icon = "fa fa-list-th";
            //SearchTypes.Add(F1);

            foreach (var F in (await documentTypeService.GetByPermission(u, PermissionType.CanView, "", "")).Where(t=>t.CategoryId==GroupType || string.IsNullOrEmpty(GroupType)))
            {
                F1 = new();
                F1.Name = F.Name;
                F1.DocumentType = F.Id;
                F1.Nr = F.Id;
                F1.Icon = F.Icon;
                SearchTypes.Add(F1);
            }

            //F1 = new();
            //F1.Name = "Data Archiviazione";
            //F1.DocumentType = $"CreationDate";
            //F1.Id = -1;
            //F1.Icon = "fa fa-calendar";
            //SearchTypes.Add(F1);

            //F1 = new();
            //F1.Name = "Scadenzario";
            //F1.DocumentType = $"Expired";
            //F1.Icon = "fa fa-clock-o";
            //SearchTypes.Add(F1);

            //F1 = new();
            //F1.Name = "Cestino";
            //F1.DocumentType = $"Documents?status=" + ((int)DocumentStatus.Deleted).ToString();
            //F1.Icon = "fa fa-clock-o";
            //F1.Nr = (await Count(u, DocumentStatus.Deleted)).ToString();
            //SearchTypes.Add(F1);
        }
        public async Task<int> Count(UserProfile u, DocumentStatus status)
        {
            var DFilters = new List<SearchFilter>();
            DFilters.Add(new SearchFilter() { ColumnName = DocumentColumn.Status, Operator = OperatorType.EqualTo, Values = new() { ((int)status).ToString() } });
            return await searchRepository.Count(DFilters, u);
        }

        public string FormatFileSize(long fileSize)
        {
            if (fileSize < 1000)
            {
                return fileSize.ToString() + " B";
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