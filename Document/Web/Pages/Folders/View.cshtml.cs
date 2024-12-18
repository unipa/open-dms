using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Repositories;
using System.Text;
using Web.DTOs;

namespace Web.Pages.Folders
{
    public class FilterItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public int Badge { get; set; }
        public bool IsFolder { get; set; }
        public string BadgeClass { get; set; } // CSS per eventuali errori
        public List<FilterItem> Items { get; set; } = new();
        public string Index { get; set; } = "";
        public string ViewType { get; set; } = "";
    }


    [Authorize]
    public class FolderViewModel : PageModel
    {
        private readonly ILogger<FolderViewModel> _logger;
        private readonly IDocumentService documentService;
        private readonly ILoggedUserProfile userContext;
        private readonly IDocumentTypeService documentTypeService;
        private readonly ILookupTableService lookupTableService;
        private readonly IACLService aclService;
        private readonly IAppSettingsRepository appSettings;
        private readonly ISearchService searchRepository;

        public string DocumentTypeName { get; set; }
        public int FolderId { get; set; }
        public int ViewId { get; set; }
        public string GroupName { get; set; }
        public string GroupType { get; set; }
        public string Icon { get; set; }
        public List<FilterItem> FilterList { get; set; } = new();

        public List<DocumentType> AddButton { get; set; } = new List<DocumentType>();

        public FileExplorerModel FileFilters { get; set; } = new();
        public List<LookupTable> GlobalFilters { get; set; } = new();

        public bool CanEditDashboard { get; set; } = false;
        public bool CanEditView { get; set; } = false;

        public FolderViewModel(ILogger<FolderViewModel> logger,
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

        public async Task<IActionResult> OnGet(int? Id, string? Uid, int viewId = 0)
        {
            var u = userContext.Get();
            DocumentTypeName = "Tutti i documenti";
            GroupName = "Tutte";
            GroupType = "";
            if (!String.IsNullOrEmpty(Uid))
            {
                Id = await documentService.FindByUniqueId(null, Uid, ContentType.Folder);
            }
            FolderId = Id.Value;
            ViewId = viewId;
            try
            {
                HashSet<string> Tipologie = new HashSet<string>();
                var contents = await documentService.GetFolderDocuments(FolderId, u);
                foreach (var cid in contents)
                {
                    var doc = await documentService.Get(cid);
                    if (doc.ContentType == ContentType.Folder && doc.DocumentStatus == DocumentStatus.Active)
                    {
                        FilterItem lt = new FilterItem()
                        {
                            Id = doc.Id,
                            Title = doc.Description,
                            Icon = doc.Icon,
                            IsFolder = true,
                            Index = doc.DocumentNumber
                        };
                        if (string.IsNullOrEmpty(lt.Icon)) lt.Icon = "fa fa-folder";
                        var views = await documentService.GetFolderDocuments(cid, u);
                        foreach (var vid in views)
                        {
                            var vdoc = await documentService.Get(vid);
                            if (vdoc.ImageId.HasValue)
                            {
                                var docdata = await documentService.GetContent(vdoc.ImageId.Value);
                                try
                                {

                                    if (vdoc.DocumentStatus == DocumentStatus.Active && vdoc.Image.FileName.ToLower().EndsWith(".report"))
                                    {
                                        var jsonstring = Encoding.Default.GetString(docdata);
                                        var json = System.Text.Json.JsonSerializer.Deserialize<ReportFile>(jsonstring);

                                        FilterItem vlt = new FilterItem()
                                        {
                                            Id = vdoc.Id,
                                            Title = vdoc.Description,
                                            Icon = string.IsNullOrEmpty(json.Icon) ? vdoc.Icon : json.Icon,
                                            IsFolder = vdoc.ContentType == ContentType.Folder,
                                            Index = vdoc.DocumentNumber
                                        };
                                        if (string.IsNullOrEmpty(vlt.Icon))
                                            if (vlt.IsFolder)
                                                vlt.Icon = "fa fa-folder";
                                            else
                                                vlt.Icon = "fa fa-dashboard";

                                        if (ViewId == vdoc.Id)
                                        {
                                            Icon = vlt.Icon;
                                            GroupName = vlt.Title;
                                            GroupType = lt.Title;
                                        }
                                        lt.Items.Add(vlt);
                                        foreach (var v in json.Views)
                                        {
                                            var f = v.Filters.FirstOrDefault(f => f.ColumnName == "Document.DocumentType");
                                            if (f != null)
                                            {
                                                foreach (var t in f.Values)
                                                {
                                                    if (!Tipologie.Contains(t))
                                                        Tipologie.Add(t);
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                        if (lt.Items.Count > 0)
                        {
                            lt.Items = lt.Items.OrderBy(d => d.Index).ThenBy(d => d.Id).ToList();
                            FilterList.Add(lt);
                        }
                    }
                }
                Icon = "fa fa-file-pdf-o";
                FilterList = FilterList.OrderBy(d => d.Index).ThenBy(d => d.Id).ToList();
                if (viewId == 0 && FilterList.Count > 0)
                {

                    ViewId = FilterList[0].Items[0].Id;
                    Icon = FilterList[0].Items[0].Icon;
                    GroupName = FilterList[0].Title;
                    GroupType = FilterList[0].Items[0].Title;
                }


                if (Tipologie.Count > 0)
                {
                    foreach (var t in Tipologie)
                    {
                        var docType = await documentTypeService.GetByPermission(t, u, PermissionType.CanCreate);
                        if (docType != null)
                        {
                            AddButton.Add(docType);
                        }
                    }
                }

                //var viewId = "doc.all";
                //DocumentType doctype = null;
                //FileFilters = await ControllerUtility.GetFilters(Request, "", doctype);
                //if (FileFilters.Request.PageSize == 0)
                //    FileFilters.Request.PageSize = int.Parse("0" + (await appSettings.Get("Document.List.PageSize")));
                //if (FileFilters.Request.PageSize == 0)
                //{
                //    FileFilters.Request.PageSize = 25;
                //}
                //if (FileFilters.Request.OrderBy.Count == 0)
                //{
                //    FileFilters.Request.OrderBy.Add(new SortingColumn() { ColumnId = DocumentColumn.CreationDate, Descending = true });
                //}
            }
            catch
            {
                return this.Forbid();
            }
            CanEditDashboard = (await documentService.GetPermission(FolderId, u, PermissionType.CanEdit)).Authorization == AuthorizationType.Granted || (await documentService.GetPermission(FolderId, u, PermissionType.CanAddContent)).Authorization == AuthorizationType.Granted; ;  
            CanEditView = (await documentService.GetPermission(ViewId, u, PermissionType.CanEdit)).Authorization == AuthorizationType.Granted || (await documentService.GetPermission(ViewId, u, PermissionType.CanAddContent)).Authorization == AuthorizationType.Granted; 
            return Page();
        }

    }
}