using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.ViewModel.ColumnTypes;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.QueryBuilder;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using Web.DTOs;

namespace Web.Pages.Shared.Components.FileExplorer
{
    public class FileExplorerViewComponent : ViewComponent
    {
        private readonly IDocumentService documentService;
        private readonly ILoggedUserProfile userContext;
        private readonly IViewManager viewManager;
        private readonly IUserService userService;
        private readonly IUserSettingsRepository userSettings;
        private readonly IUISettingsRepository uiSettingsRepository;
        private readonly IQueryBuilder queryBuilder;
        private readonly IACLService aCLService;
        private readonly IViewServiceResolver viewMapper;


        public bool CanHandleDocuments { get; set; }
        public bool CanSign { get; set; }
        public bool CanDelete{ get; set; }
        public bool HasMailAddress { get; set; }

        public SearchRequest SearchRequest { get; set; }

        public FileExplorerViewComponent(IDocumentService documentService,
            ILoggedUserProfile userContext,
            IViewManager viewManager,
            IUserService userService,
            IUserSettingsRepository userSettings,
            IUISettingsRepository uiSettingsRepository,
            IQueryBuilder queryBuilder,
            IACLService aCLService,
            IViewServiceResolver viewMapper)
        {
            this.documentService = documentService;
            this.userContext = userContext;
            this.viewManager = viewManager;
            this.userService = userService;
            this.userSettings = userSettings;
            this.uiSettingsRepository = uiSettingsRepository;
            this.queryBuilder = queryBuilder;
            this.aCLService = aCLService;
            this.viewMapper = viewMapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(FileExplorerModel sr)
        {
            var data = new SearchResult();
            var u = userContext.Get();
            var user = u.UserInfo;
            ViewData["SearchRequest"] = sr.Request;

            List<string>Auths = new();

            ViewData["HasMailAddress"] = false;

            var maillist = await userService.GetAllContactDigitalAddress(u.userId);
            ViewData["HasMailAddress"] = (maillist != null) ? (maillist.Where(x => !x.Deleted).Count() > 0) 
                    && (await aCLService.GetAuthorization("", u, PermissionType.Profile_CanSendMail) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted) 
                    : false;
            var IsAdmin = u.Roles.Count > 0 ? u.Roles.Select(s => s.Id).Contains(SpecialUser.AdminRole) : false;
            ViewData["CanSign"] = IsAdmin
                    || (user != null && !String.IsNullOrEmpty(await userSettings.Get(user.ContactId, OpenDMS.Domain.Constants.UserAttributes.CONST_REMOTESIGNATURE_SERVICE)))
                    || (user != null && !String.IsNullOrEmpty(await userSettings.Get(user.ContactId, OpenDMS.Domain.Constants.UserAttributes.CONST_OTPSIGNATURE_SERVICE)))
                    || (user != null && !String.IsNullOrEmpty(await userSettings.Get(user.ContactId, OpenDMS.Domain.Constants.UserAttributes.CONST_FEASIGNATURE_VENDOR)));
            ViewData["CanHandleDocuments"] = IsAdmin 
                    || (await aCLService.GetAuthorization("", u, PermissionType.Profile_CanHandleDocuments) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted);
            ViewData["CanDelete"] = IsAdmin 
                    || (await aCLService.GetAuthorization("", u, PermissionType.CanDelete) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted);


            if (!string.IsNullOrEmpty(sr.Request.ViewId))
            {
                var view = await viewManager.Get(sr.Request.ViewId, u.userId);

                var style = await uiSettingsRepository.Get(u.userId, "ViewStyle." + view.ViewId);
                if (!String.IsNullOrEmpty(style)) view.ViewStyle = (ViewStyle)int.Parse(style);

                if (!view.Columns.Any(c => c.Settings.SortType != SortingType.None))
                {
                    sr.Request.OrderBy.Add(new SortingColumn() { ColumnId = DocumentColumn.CreationDate, Descending = true });
                }
                else
                {
                    foreach (var c in view.Columns)
                    {
                        if (c.Settings.SortType != SortingType.None)
                            sr.Request.OrderBy.Add(new SortingColumn() { ColumnId = c.Id, Descending = c.Settings.SortType == SortingType.Descending });

                    }

                }

                var service = await viewMapper.GetSearchService(sr.Request.ViewId);
                data = await service.Get(view, sr.Request, u);
                if (sr.DisableSorting)
                {
                    data.View.Columns.ForEach(c => { c.IsSortable = false; c.Settings.SortType = SortingType.None; });
                }
                foreach (var c in view.Columns)
                {
                    if (false)
                    //if (c.IsGroupable)
                    //if ( data.Filters.Count < 5 && (c.IsGroupable || ((c is GenericAvatarColumn || c is GenericLookupColumn) && (c.DataType == ColumnDataType.Text) || (c.DataType == ColumnDataType.Avatar)))) 
                    {
                        var f = new FilterGroupResult() { ColumnId = c.Id, Label = c.Settings.Title };
                        var total = 0;
                        queryBuilder.Clear();
                        queryBuilder.Map(c.Id);
                        queryBuilder.Map(DocumentColumn.Id, AggregateType.Count);
                        foreach (var fi in sr.Request.Filters)
                        {
                            queryBuilder.Filter(fi.ColumnName, fi.Operator, fi.Values.ToArray());
                        }
                        foreach (var r in queryBuilder.Build())
                        {
                            var names = queryBuilder.GetFields(c.Id);
                            var val = "A" + (names.Length + 1).ToString(); //  queryBuilder.GetFields(DocumentColumn.Id, AggregateType.Count);
                            var cv = r[names[0]].ToString();
                            var cd = r[names[names.Length - 1]].ToString();
                            var v = r.AsInteger(val, 0);
                            var i = new GroupedResult();
                            i.Total = v;
                            total += v;
                            i.Value = cv;
                            i.Description = cd;
                            if (string.IsNullOrEmpty(i.Description))
                                i.Description = "<vuoto>";
                            else
                            {
                                if (c is GenericAvatarColumn && !string.IsNullOrEmpty(cv))
                                    i.Description = await userService.GetName(cv);
                            }
                            f.Values.Add(i);
                        }
                        if (f.Values.Count > 0)
                        {

                            f.Values = f.Values.OrderBy(f => f.Description).ToList();
                            var i = new GroupedResult() { Value = "", Description = "<Tutti>", Total = total };
                            f.Values.Insert(0, i);

                            data.Filters.Add(f);
                        }
                    }
                }
            }

            var folder = sr.Request.Filters.FirstOrDefault(f => f.ColumnName == DocumentColumn.Parent);
            ViewData["FolderId"] = 0;
            if (folder != null) ViewData["FolderId"] = int.Parse( folder.Values[0]);
            data.Title = sr.Title;
            data.HideSelection = sr.HideSelection;

            if ((bool)ViewData["CanSign"] == true)
                Auths.Add("SIGN");
            if ((bool)ViewData["HasMailAddress"] == true)
                Auths.Add("MAIL");
            if (IsAdmin)
                Auths.Add("ADMIN");
            if ((bool)ViewData["CanDelete"] == true)
                Auths.Add("DELETE");
            if ((int)ViewData["FolderId"] > 0)
                Auths.Add("UNFOLD");

            ViewData["Auths"] = String.Join(",", Auths);
            return View(data);
        }

        public static string Translate(AggregateType A)
        {
            switch (A)
            {
                case AggregateType.None:
                    return "Nessuno";
                    break;
                case AggregateType.Count:
                    return "Conteggio";
                    break;
                case AggregateType.Sum:
                    return "Somma";
                    break;
                case AggregateType.Min:
                    return "Valore Minimo";
                    break;
                case AggregateType.Max:
                    return "Valore Massimo";
                    break;
                case AggregateType.Average:
                    return "Valore Medio";
                    break;
                default:
                    return "";
                    break;
            }
        }
    }
}
