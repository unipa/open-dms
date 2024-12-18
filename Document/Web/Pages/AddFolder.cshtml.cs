using com.sun.org.apache.bcel.@internal.generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using System.Collections.Generic;
using Web.Pages.Folders;

namespace Web.Pages
{
    [Authorize]
    public class AddFolderModel : PageModel
    {
        private readonly IDocumentService documentService;
        private readonly IDocumentTypeService documentTypeService;
        private readonly IACLService aCLService;
        private readonly ISearchService searchRepository;
        private readonly ILoggedUserProfile userContext;


 //       public List<FolderInfo> RecentlyViewed { get; set; } = new();
        public List<FolderInfo> LastAdded { get; set; } = new();
        public List<FolderInfo> FolderTypes { get; set; } = new();
        public List<FolderInfo> PersonalFolder { get; set; } = new();
        public List<FolderInfo> SharedFolder { get; set; } = new ();

        public string Selected { get; set; } = "";
        public string Documents { get; set; } = "";

        public AddFolderModel(IDocumentService documentService,
            IDocumentTypeService documentTypeService,
            IACLService aCLService,
            ISearchService searchRepository,
            ILoggedUserProfile userContext)
        {
            this.documentService = documentService;
            this.documentTypeService = documentTypeService;
            this.aCLService = aCLService;
            this.searchRepository = searchRepository;
            this.userContext = userContext;
        }

        public async Task OnGet(string id)
        {
            var u = userContext.Get();
            Documents = id;
            var idlist = id.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            Selected = idlist.Length.ToString() + " document" + (idlist.Length > 1 ? "i" : "o");  
            //u = UserProfile.SystemUser();
            bool isAdmin = u.userId == SpecialUser.SystemUser || u.Roles.Select(s => s.Id).Contains("admin");
            var personal = await documentService.GetUserFolder(u);

            var Filters = new List<SearchFilter>();
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.ContentType, Operator = OperatorType.EqualTo, Values = new() { ((int)ContentType.Folder).ToString() } });
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Status, Operator = OperatorType.EqualTo, Values = new() { ((int)DocumentStatus.Active).ToString() } });
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Parent, Operator = OperatorType.EqualTo, Values = new() { personal.ToString() } });
            foreach (var d in await searchRepository.Find(Filters, u, 10, new List<SortingColumn>() { new SortingColumn() { ColumnId = DocumentColumn.Description, Descending = false } }))
            {
                var doc = await documentService.Load(d, u);
                FolderInfo F = new();
                F.Name = doc.Description;
                F.DocumentType = doc.DocumentTypeName;
                F.Icon = doc.Icon;
                F.IconColor = doc.IconColor;
                F.Path = doc.Path;
                F.Id = doc.Id;
                F.Nr = doc.DocumentNumberLookupValue;
                F.Date = doc.CreationDate.ToString("dd/MM/yyyy HH:mm");
                PersonalFolder.Add(F);
            }

            Filters = new List<SearchFilter>();
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.ContentType, Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { ((int)ContentType.Folder).ToString() } });
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.DocumentType, Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { null } });
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Status, Operator = OperatorType.EqualTo, Values = new() { ((int)DocumentStatus.Active).ToString() } });
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Parent, Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { "0" } });
            foreach (var d in await searchRepository.Find(Filters, u, 5, new List<SortingColumn>() { new SortingColumn() { ColumnId = DocumentColumn.Description, Descending = false } }))
            {
                var doc = await documentService.Load(d, u);
                FolderInfo F = new();
                F.Name = doc.Description;
                F.DocumentType = doc.DocumentTypeName;
                F.Icon = doc.Icon;
                F.IconColor = doc.IconColor;
                F.Path = doc.Path;
                F.Id = doc.Id;
                F.Nr = doc.DocumentNumberLookupValue;
                F.Date = doc.CreationDate.ToString("dd/MM/yyyy HH:mm");
                SharedFolder.Add(F);
            }

            var folders = (await documentTypeService.GetByPermission(u, PermissionType.CanView, "")).Where(f => f.ContentType == ContentType.Folder);
            foreach (var doc in folders)
            {
                FolderInfo F = new();
                F.Name = doc.Name;
                F.DocumentType = doc.Id;
                F.Icon = doc.Icon;
                F.IconColor = doc.IconColor;
                F.Id = 0;
                F.Nr = "";
                F.Date = "";
                FolderTypes.Add(F);
            }

            Filters = new List<SearchFilter>();
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.ContentType, Operator = OperatorType.EqualTo, Values = new() { ((int)ContentType.Folder).ToString() } });
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Status, Operator = OperatorType.EqualTo, Values = new() { ((int)DocumentStatus.Active).ToString() } });
            foreach (var d in await searchRepository.Find(Filters, u, 5, new List<SortingColumn>() { new SortingColumn() { ColumnId = DocumentColumn.CreationDate, Descending = true } }))
            {
                var doc = await documentService.Load(d, u);
                FolderInfo F = new();
                F.Name = doc.Description;
                F.DocumentType = doc.DocumentTypeName;
                F.Icon = doc.Icon;
                F.IconColor = doc.IconColor;
                F.Path = doc.Path;
                F.Id = doc.Id;
                F.Nr = doc.DocumentNumberLookupValue;
                F.Date = doc.CreationDate.ToString("dd/MM/yyyy HH:mm");
                LastAdded.Add(F);
            }

        }

        public class FolderProperty
        {
            public DocumentInfo Info { get; set; }
            public List<FolderInfo> Folders { get; set; } = new();
            public bool CanAddContent { get; set; }
            public string? DocumentType { get; set; }
        }

        public async Task<IActionResult> OnGetNavigate(int ParentId)
        {
            var u = userContext.Get();
            var Folder = new FolderProperty();
            Folder.Info = await documentService.Load(ParentId, u);
            Folder.CanAddContent = (await documentService.GetPermission(ParentId, u, PermissionType.CanAddContent))?.Authorization == AuthorizationType.Granted;
            //u = UserProfile.SystemUser();
            bool isAdmin = u.userId == SpecialUser.SystemUser || u.Roles.Select(s => s.Id).Contains("admin");
            var Filters = new List<SearchFilter>();
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.ContentType, Operator = OperatorType.EqualTo, Values = new() { ((int)ContentType.Folder).ToString() } });
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Status, Operator = OperatorType.EqualTo, Values = new() { ((int)DocumentStatus.Active).ToString() } });
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Parent, Operator = OperatorType.EqualTo, Values = new() { ParentId.ToString() } });
            foreach (var d in await searchRepository.Find(Filters, u, 255, new List<SortingColumn>() { new SortingColumn() { ColumnId = DocumentColumn.Description, Descending = false } }))
            {
                var doc = await documentService.Load(d, u);
                FolderInfo F = new();
                F.Name = doc.Description;
                F.DocumentType = doc.DocumentTypeName;
                F.Icon = string.IsNullOrEmpty(doc.Icon) ? "fa fa-folder" : doc.Icon;
                F.IconColor = string.IsNullOrEmpty(doc.IconColor) ? "orange" :  doc.IconColor;
                F.Path = doc.Path;
                F.Id = doc.Id;
                F.Nr = doc.DocumentNumberLookupValue;
                F.Date = doc.CreationDate.ToString("dd/MM/yyyy HH:mm");
                Folder.Folders.Add(F);
            }
            return new JsonResult( Folder);
        }

        public async Task<IActionResult> OnGetSearch(string DocType)
        {
            var u = userContext.Get();
            var Folder = new FolderProperty();
            Folder.DocumentType = DocType;
            var tipo = await documentTypeService.GetById(DocType);
            Folder.Info = new DocumentInfo()
            {
                Description = tipo.Name,
                Icon = tipo.Icon,
                IconColor = tipo.IconColor,
                DocumentType = tipo
            };
            Folder.CanAddContent = (await aCLService.GetAuthorization(tipo.ACLId, u, PermissionType.CanCreate)) == AuthorizationType.Granted;
            //u = UserProfile.SystemUser();
            var Filters = new List<SearchFilter>();
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.ContentType, Operator = OperatorType.EqualTo, Values = new() { ((int)ContentType.Folder).ToString() } });
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Status, Operator = OperatorType.EqualTo, Values = new() { ((int)DocumentStatus.Active).ToString() } });
            Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.DocumentType, Operator = OperatorType.EqualTo, Values = new() { DocType } });
            foreach (var d in await searchRepository.Find(Filters, u, 255, new List<SortingColumn>() { new SortingColumn() { ColumnId = DocumentColumn.Description, Descending = false } }))
            {
                var doc = await documentService.Load(d, u);
                FolderInfo F = new();
                F.Name = doc.Description;
                F.DocumentType = doc.DocumentTypeName;
                F.Icon = string.IsNullOrEmpty(doc.Icon) ? "fa fa-folder" : doc.Icon;
                F.IconColor = string.IsNullOrEmpty(doc.IconColor) ? "orange" : doc.IconColor;
                F.Path = doc.Path;
                F.Id = doc.Id;
                F.Nr = doc.DocumentNumberLookupValue;
                F.Date = doc.CreationDate.ToString("dd/MM/yyyy HH:mm");
                Folder.Folders.Add(F);
            }
            return new JsonResult(Folder);
        }


        public async Task<IActionResult> OnGetAddToFolder(int Id, string Documents)
        {
            var u = userContext.Get();
            var documentList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(Documents);
            return new JsonResult (await documentService.AddToFolder(Id, documentList, u, false));
        }
    }
}
