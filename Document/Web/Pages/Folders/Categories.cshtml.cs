using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using Web.DTOs;

namespace Web.Pages.Folders
{

    [Authorize]
    public class CategoriesModel : PageModel
    {
        private readonly ILogger<CategoriesModel> _logger;
        private readonly IDocumentService documentService;
        private readonly ILoggedUserProfile userContext;
        private readonly IDocumentTypeService documentTypeService;
        private readonly ILookupTableService lookupTableService;
        private readonly ISearchService searchRepository;

        public List<FolderInfo> RecentlyViewed { get; set; } = new();
        public List<FolderInfo> LastAdded { get; set; } = new();
        public List<FolderInfo> SearchTypes { get; set; } = new();

        public FileExplorerModel SearchFilters { get; set; } = new();
        public FileExplorerModel AddedFilters { get; set; } = new();


        public CategoriesModel(ILogger<CategoriesModel> logger,
            IDocumentService documentService,
            ILoggedUserProfile userContext,
            IDocumentTypeService documentTypeService,
            ILookupTableService lookupTableService,
            ISearchService searchRepository
            )
        {
            _logger = logger;
            this.documentService = documentService;
            this.userContext = userContext;
            this.documentTypeService = documentTypeService;
            this.lookupTableService = lookupTableService;
            this.searchRepository = searchRepository;
        }

        public async Task OnGet()
        {
            var u = userContext.Get();

            var DFilters = new List<SearchFilter>();
            DFilters = new List<SearchFilter>();
            DFilters.Add(new SearchFilter() { ColumnName = DocumentColumn.ContentType, Operator = OperatorType.EqualTo, Values = new() { ((int)ContentType.Document).ToString() } });
            DFilters.Add(new SearchFilter() { ColumnName = DocumentColumn.Status, Operator = OperatorType.NotEqualTo, Values = new() { ((int)DocumentStatus.Deleted).ToString() } });

            AddedFilters = new FileExplorerModel()
            {
                HideSelection = true,
                Request = new OpenDMS.Core.DTOs.SearchRequest() { ViewId = "doc.LastCreated", Filters = DFilters, OpenOnNewWindow = false, PageIndex = 0, PageSize = 5, OrderBy = new List<SortingColumn>() { new SortingColumn() { ColumnId = DocumentColumn.CreationDate, Descending = true } }},
                Title = "Ultimi Documenti Creati"
            };

            //foreach (var d in await searchRepository.Find(DFilters, u, 5, new List<SortingColumn>() { new SortingColumn() { ColumnId = "CreationDate", Descending = true } }))
            //{
            //    try
            //    {
            //        var doc = await documentService.Load(d, u);
            //        FolderInfo F = new();
            //        F.Name = doc.Description;
            //        F.DocumentType = doc.DocumentTypeName;
            //        F.Icon = doc.Icon;
            //        F.IconColor = doc.IconColor;
            //        //F.Path = doc.Image?.FileName;
            //        F.Id = doc.Id;
            //        F.Nr = doc.DocumentNumberLookupValue;
            //        F.Date = doc.DocumentDate?.ToString("dd/MM/yyyy");
            //        LastAdded.Add(F);
            //    }
            //    catch { };
            //}

            DFilters = new List<SearchFilter>();
            //DFilters.Add(new SearchFilter() { ColumnName = DocumentColumn.ViewDate, Operator = OperatorType.EqualTo, Values = new() { u.userId } });
            DFilters.Add(new SearchFilter() { ColumnName = DocumentColumn.ContentType, Operator = OperatorType.EqualTo, Values = new() { ((int)ContentType.Document).ToString() } });

            SearchFilters = new FileExplorerModel()
            {
                HideSelection = true,
                Request = new OpenDMS.Core.DTOs.SearchRequest() { ViewId="doc.lastviewed", Filters = DFilters, OpenOnNewWindow = true, PageIndex = 0, PageSize = 5, OrderBy = new List<SortingColumn>() { new SortingColumn() { ColumnId = DocumentColumn.ViewDate+u.userId, Descending = true } } },
                Title = "Ultimi Documenti Consultati"
            };

            //foreach (var d in await searchRepository.Find(DFilters, u, 5, new List<SortingColumn>() { new SortingColumn() { ColumnId = "ViewUser", Descending = true } }))
            //{
            //    try
            //    {
            //        var doc = await documentService.Load(d, u);
            //        FolderInfo F = new();
            //        F.Name = doc.Description;
            //        F.DocumentType = doc.DocumentTypeName;
            //        F.Icon = doc.Icon;
            //        F.IconColor = doc.IconColor;
            //        //F.Path = doc.Path;
            //        F.Id = doc.Id;
            //        F.Nr = doc.DocumentNumberLookupValue;
            //        F.Date = doc.DocumentDate?.ToString("dd/MM/yyyy");
            //        RecentlyViewed.Add(F);
            //    }
            //    catch { };
            //}

            FolderInfo F1 = new();

            F1 = new();
            F1.Name = "Tutte le categorie";
            F1.DocumentType = "";
            F1.Icon = "fa fa-tags";
            SearchTypes.Add(F1);

            foreach (var F in (await documentTypeService.GetByPermission(u, PermissionType.CanView, "", "")).Select(t => t.CategoryId).Distinct())
            {
                var L = await lookupTableService.GetById("$CATEGORIES$", F);
                F1 = new();
                F1.Name = L.Description;
                F1.DocumentType = F;
                F1.Nr = L.Id;
                F1.Icon = "";
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