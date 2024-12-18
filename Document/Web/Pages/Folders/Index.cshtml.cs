using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.CustomPages;
using OpenDMS.CustomPages.Models;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using Web.DTOs;

namespace Web.Pages.Folders
{

    public class FolderInfo
    {
        public string DocumentType { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Documents { get; set; }
        public string Icon { get; set; }
        public string IconColor { get; set; }
        public string Nr { get; set; }
        public string Date { get; set; }
        public List<LookupTable> Path { get; set; } = new();

    }
    
    [Authorize]
    public class FolderModel : PageModel
    {
        private readonly ILogger<FolderModel> _logger;
        private readonly IDocumentService documentService;
        private readonly ILoggedUserProfile userContext;
        private readonly IDocumentTypeService documentTypeService;
        private readonly IACLService aclService;
        private readonly ISearchService searchRepository;
        private readonly ICustomPageProvider customPageProvider;

        //public List<DocumentType> NewFolders { get; set; } = new();

        //        public List<FolderInfo> RecentlyViewed { get; set; } = new();
        //        public List<FolderInfo> LastAdded { get; set; } = new();
        public List<FolderInfo> FolderTypes { get; set; } = new();
        public List<FolderInfo> PersonalFolder { get; set; } = new();
        public List<FolderInfo> SharedFolder { get; set; } = new();

        public FileExplorerModel LastAdded { get; set; } = new();
        public FileExplorerModel RecentlyViewed { get; set; } = new();

        public List<CustomPageDTO> MenuItems { get; set; }

        //public bool CanCreateDocument { get; set; }
        //public bool CanCreateRootFolder { get; set; }



        public FolderModel(ILogger<FolderModel> logger,
            IDocumentService documentService,
            ILoggedUserProfile userContext,
            IDocumentTypeService documentTypeService,
            IACLService aclService,
            ISearchService searchRepository,
            ICustomPageProvider customPageProvider
            )
        {
            _logger = logger;
            this.documentService = documentService;
            this.userContext = userContext;
            this.documentTypeService = documentTypeService;
            this.aclService = aclService;
            this.searchRepository = searchRepository;
            this.customPageProvider = customPageProvider;
        }

        public async Task OnGet()
        {
            //Console.WriteLine(DateTime.Now.ToString("HH:MM:ss.fff") + " -> GET");
            var u = userContext.Get();
            if (u == null) Forbid();

            var personal = await documentService.GetUserFolder(u);
            //Console.WriteLine(DateTime.Now.ToString("HH:MM:ss.fff") + " -> GETUSERFOLDER");
            MenuItems = (await customPageProvider.GetPages(u,"Dashboard")).Where(m => m.Alignment >= 0).ToList();

            var Filters = new List<SearchFilter>();
            var su = UserProfile.SystemUser();
            Filters.Add(new SearchFilter() { ColumnName =DocumentColumn.ContentType, Operator = OperatorType.EqualTo, Values = new() { ((int)ContentType.Folder).ToString() } });
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Status, Operator = OperatorType.EqualTo, Values = new() { ((int)DocumentStatus.Active).ToString() } });
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Parent, Operator = OperatorType.EqualTo, Values = new() { personal.ToString() } });
            foreach (var d in await searchRepository.Find(Filters, u, 10, new List<SortingColumn>() { new SortingColumn() { ColumnId = DocumentColumn.Description, Descending = false } }))
            {
                try
                {
                    var doc = await documentService.Get(d);
                    FolderInfo F = new();
                    F.Name = doc.Description;
                    F.DocumentType = doc.DocumentType?.Name ?? ""; //.DocumentTypeName;
                    F.Icon = doc.Icon;
                    F.IconColor = doc.IconColor;
                    //F.Path = doc.Path;
                    F.Id = doc.Id;
                    //F.Nr = doc.DocumentNumberLookupValue;
                    //F.Date = doc.CreationDate.ToString("dd/MM/yyyy HH:mm");
                    PersonalFolder.Add(F);
                }
                catch { };
            }
            //Console.WriteLine(DateTime.Now.ToString("HH:MM:ss.fff") + " -> PERSONALFOLDER");

            Filters = new List<SearchFilter>();
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.ContentType, Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { ((int)ContentType.Folder).ToString() } });
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.DocumentType, Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { null } });
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Status, Operator = OperatorType.EqualTo, Values = new() { ((int)DocumentStatus.Active).ToString() } });
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Parent, Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { "0" } });
            foreach (var d in await searchRepository.Find(Filters, u, 50, new List<SortingColumn>() { new SortingColumn() { ColumnId = DocumentColumn.Description, Descending = false } }))
            {
                try
                {
                    var doc = await documentService.Get(d);
                    FolderInfo F = new();
                    F.Name = doc.Description;
                    F.DocumentType = doc.DocumentType?.Name ?? ""; //.DocumentTypeName;
                    F.Icon = doc.Icon;
                    F.IconColor = doc.IconColor;
                    //F.Path = doc.Path;
                    F.Id = doc.Id;
                    //F.Nr = doc.DocumentNumberLookupValue;
                    //F.Date = doc.CreationDate.ToString("dd/MM/yyyy HH:mm");
                    SharedFolder.Add(F);
                }
                catch { };
            }
            //Console.WriteLine(DateTime.Now.ToString("HH:MM:ss.fff") + " -> SHAREDFOLDER");


            Filters = new List<SearchFilter>();
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.ContentType, Operator = OperatorType.EqualTo, Values = new() { ((int)ContentType.Folder).ToString() } });
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Status, Operator = OperatorType.EqualTo, Values = new() { ((int)DocumentStatus.Active).ToString() } });

            LastAdded = new FileExplorerModel()
            {
                HideSelection = true,
                DisableSorting = true,
                Request = new OpenDMS.Core.DTOs.SearchRequest()
                {
                    ViewId = "doc.lastcreated",
                    Filters = Filters,
                    OpenOnNewWindow = false,
                    PageIndex = 0,
                    PageSize = 5,
                    OrderBy = new List<SortingColumn>() { new SortingColumn() { ColumnId = DocumentColumn.CreationDate, Descending = true } }
                },
                Title = "" // "Ultimi Documenti Creati"
            };

     
            Filters = new List<SearchFilter>();
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.ContentType, Operator = OperatorType.EqualTo, Values = new() { ((int)ContentType.Folder).ToString() } });
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.ViewDate+u.userId, Operator = OperatorType.EqualTo, Values = new() { u.userId } });
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Status, Operator = OperatorType.EqualTo, Values = new() { ((int)DocumentStatus.Active).ToString() } });


            RecentlyViewed = new FileExplorerModel()
            {
                HideSelection = true,
                DisableSorting = true,
                Request = new OpenDMS.Core.DTOs.SearchRequest() 
                { 
                    ViewId = "doc.lastviewed", 
                    Filters = Filters,
                    OpenOnNewWindow = false, 
                    PageIndex = 0, 
                    PageSize = 5, 
                    OrderBy = new List<SortingColumn>() { new SortingColumn() { ColumnId = DocumentColumn.ViewDate+u.userId, Descending = true } } 
                },
                Title = ""// "Ultimi Documenti Consultati"
            };

      
            var folders = (await documentTypeService.GetByPermission(u, PermissionType.CanView, "")).Where(f => f.ContentType == ContentType.Folder);
            foreach (var doc in folders)
            {
                FolderInfo F = new();
                var nomeplurale = doc.Name.Replace("Fascicolo", "Archivio");

                F.Name =  nomeplurale;
                F.DocumentType = doc.Id;
                F.Icon = doc.Icon;
                F.IconColor = doc.IconColor;
                F.Id = 0;
                F.Nr = "";
                F.Date = "";
                FolderTypes.Add(F);
            }

        }


        public async Task<IActionResult> OnGetFolders(SearchRequest sr)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            List<DocumentInfo> docs = new List<DocumentInfo>();
            foreach (var d in await searchRepository.Find(sr.Filters, u, 5))
            {
                var doc = await documentService.Load(d, u);
                docs.Add(doc);
            }
            return new JsonResult(docs);
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