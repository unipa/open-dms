using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using System.Text;
using Web.DTOs;


namespace Web.Pages.Shared.Components.FolderView
{



    public class QueryExplorerViewComponent : ViewComponent
    {
        private readonly IDocumentService documentService;
        private readonly ILoggedUserProfile userContext;
        private readonly IViewManager viewManager;
        private readonly IUserService userService;
        private readonly IUserSettingsRepository userSettings;
        private readonly IUISettingsRepository uiSettingsRepository;
        private readonly IACLService aCLService;
        private readonly IViewServiceResolver viewMapper;




        public bool CanHandleDocuments { get; set; }
        public bool CanSign { get; set; }
        public bool CanDelete{ get; set; }
        public bool HasMailAddress { get; set; }


        public QueryExplorerViewComponent(IDocumentService documentService,
            ILoggedUserProfile userContext,
            IViewManager viewManager,
            IUserService userService,
            IUserSettingsRepository userSettings,
            IUISettingsRepository uiSettingsRepository,
            IACLService aCLService,
            IViewServiceResolver viewMapper)
        {
            this.documentService = documentService;
            this.userContext = userContext;
            this.viewManager = viewManager;
            this.userService = userService;
            this.userSettings = userSettings;
            this.uiSettingsRepository = uiSettingsRepository;
            this.aCLService = aCLService;
            this.viewMapper = viewMapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int Id)
        {
            var data = new ReportData();
            data.Definition = new ReportFile();
            data.Data = new();
            var u = userContext.Get();
            var user = await userService.GetById(u.userId);

            ViewData["HasMailAddress"] = ((await userService.GetAllContactDigitalAddress(u.userId)).Where(x => !x.Deleted).Count() > 0)
                    && (await aCLService.GetAuthorization("", u, PermissionType.Profile_CanSendMail) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted);
            ViewData["CanSign"] = u.Roles.Select(s => s.Id).Contains("admin") 
                    || !String.IsNullOrEmpty(await userSettings.Get(user.ContactId, OpenDMS.Domain.Constants.UserAttributes.CONST_REMOTESIGNATURE_SERVICE))
                    || !String.IsNullOrEmpty(await userSettings.Get(user.ContactId, OpenDMS.Domain.Constants.UserAttributes.CONST_OTPSIGNATURE_SERVICE))
                    || !String.IsNullOrEmpty(await userSettings.Get(user.ContactId, OpenDMS.Domain.Constants.UserAttributes.CONST_FEASIGNATURE_VENDOR));
            ViewData["CanHandleDocuments"] = u.Roles.Select(s => s.Id).Contains("admin") 
                    || (await aCLService.GetAuthorization("", u, PermissionType.Profile_CanHandleDocuments) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted);
            ViewData["CanDelete"] = u.Roles.Select(s => s.Id).Contains("admin") 
                    || (await aCLService.GetAuthorization("", u, PermissionType.CanDelete) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted);

            //u = UserProfile.SystemUser();
            if (Id > 0)
            {
                var doc = await documentService.Get(Id);
                var docdata = await documentService.GetContent(doc.ImageId.Value);
                try
                {
                    var jsonstring = Encoding.Default.GetString(docdata);
                    var json = System.Text.Json.JsonSerializer.Deserialize<ReportFile>(jsonstring);
                    data.Definition = json;
                    foreach (var v in json.Views)
                    {
                        var view = new ViewProperties();
                        
                        // Determina il nome della vista
                        var tp = v.Filters.FirstOrDefault (f => f.ColumnName == DocumentColumn.DocumentType);
                        string viewPrefix = tp != null ? "doc.type."+tp.Values[0] : "doc.ReportFile" + Id.ToString();
                        view.ViewId = viewPrefix;
                        
                        // Determina la modalità di visualizzazione
                        var style = await uiSettingsRepository.Get(u.userId, "ViewStyle." + view.ViewId);
                        if (!String.IsNullOrEmpty(style)) view.ViewStyle = (ViewStyle)int.Parse(style);

                        // Imposta la richiesta iniziale 
                        SearchRequest sr = new SearchRequest();
                        sr.Filters = v.Filters;
                        sr.PageSize = v.PageSize;
                        sr.OpenOnNewWindow = !json.DetailPanel;
                        sr.OrderBy = v.Columns.Where(c => c.SortType != SortingType.None).Select(s => new SortingColumn() { ColumnId = s.ColumnName, Descending = s.SortType == SortingType.Descending }).ToList();
                        sr.ViewId = view.ViewId;
                        //                    var service = await viewMapper.GetSearchService(view.ViewId);
                        //                    var d = await service.Get(view, sr, u);
                        //                    d.Title = v.Title;
                        //                    d.HideSelection = true;

                        var fem = new FileExplorerModel();
                        //fem.Title = v.Title;
                        fem.ShowRemoveFromFolder = false;
                        fem.ShowDelete = true;
                        fem.Request.PageSize = 0;
                        fem.Request = sr;
                        fem.Filters = 0;
                        fem.Request.ViewId = view.ViewId;
                        data.Data.Add(fem);
                    }
                }
                catch (Exception ex) {
                    data.Definition.Title = ex.Message;
                    data.Data = new();
                };
            }
            ViewData["FolderId"] = 0;

            return View(data);
        }


    }
}
