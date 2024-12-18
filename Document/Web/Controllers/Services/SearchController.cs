using Microsoft.AspNetCore.Mvc;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using OpenDMS.Domain.Repositories;
using System.Text;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Models;
using OpenDMS.Core.Managers;
using java.nio.channels;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using com.googlecode.mp4parser.boxes.apple;
using Net.Pkcs11Interop.Common;

namespace Web.Controllers.Services;

[Authorize]
[ApiController]
[Route("internalapi/ui/[controller]")]
public class SearchController : Controller
{
    private readonly ILogger logger;
    private readonly ILoggedUserProfile userContext;
    private readonly IUISettingsRepository uISettingsRepository;
    private readonly IViewServiceResolver viewMapper;
    private readonly IViewManager viewManager;

    public SearchController(ILogger<SearchController> logger,
        ILoggedUserProfile userContext,
        IUISettingsRepository uISettingsRepository,
        IViewServiceResolver viewMapper,
        IViewManager viewManager)
    {
        this.logger = logger;
        this.userContext = userContext;
        this.uISettingsRepository = uISettingsRepository;
        this.viewMapper = viewMapper;
        this.viewManager = viewManager;
    }


    /// <summary>
    /// Questo metodo fornisce la struttura di una vista di dati (colonne) e la prima pagina di risultati.
    /// Va richiamato solo al variare dei filtri o per un refresh.
    /// Per ottenere altre pagine di risultati <see cref="GetPage(SearchRequest)"/>
    /// </summary>
    /// <param name="request">Fornisce informazioni relative al tipo di dati da visualizzare, la dimensione delle pagine, i filtri da applicare </param>
    /// <returns>Ritorna un oggetto <see cref="SearchResult"/> che contiene l'elenco di tutte le colonne visualizzabili</returns>
    [HttpPost("get", Name = "New")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<SearchResult> Get(SearchRequest request)
    {
        var u = userContext.Get();
        string userId = u.userId;
        var view = await viewManager.Get(request.ViewId, u.userId);
        var style = await uISettingsRepository.Get(u.userId, "ViewStyle." + view.ViewId);
        if (!string.IsNullOrEmpty(style)) view.ViewStyle = (ViewStyle)int.Parse(style);
        var service = await viewMapper.GetSearchService(request.ViewId);
        return await service.Get(view, request, u);
    }

    /// <summary>
    /// Questo metodo fornisce la struttura di una vista di dati (colonne) e la prima pagina di risultati.
    /// Va richiamato solo al variare dei filtri o per un refresh.
    /// Per ottenere altre pagine di risultati <see cref="GetPage(SearchRequest)"/>
    /// </summary>
    /// <param name="request">Fornisce informazioni relative al tipo di dati da visualizzare, la dimensione delle pagine, i filtri da applicare </param>
    /// <returns>Ritorna un oggetto <see cref="SearchResult"/> che contiene l'elenco di tutte le colonne visualizzabili</returns>
    [HttpPost("export", Name = "Export")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<string> Export(SearchRequest request)
    {
        var u = userContext.Get();
        string userId = u.userId;
        request.PageSize = 1;
        var view = await viewManager.Get(request.ViewId, u.userId);
        var style = await uISettingsRepository.Get(u.userId, "ViewStyle." + view.ViewId);
        if (!string.IsNullOrEmpty(style)) view.ViewStyle = (ViewStyle)int.Parse(style);
        var service = await viewMapper.GetSearchService(request.ViewId);
        // Limito il numero di risultati
        request.PageSize = 32768;
        var result =  await service.Get(view, request, u);

        var Separatore = ";";
        StringBuilder csv = new StringBuilder();
        for (int ck = 0; ck < view.KeyFields.Index.Count; ck++)
        {
            var f = view.Columns.FirstOrDefault(c => c.Id == view.KeyFields.Index[ck]);
            if (f != null)
            {
                var t = f.Settings.Title;
                if (string.IsNullOrEmpty(t))
                    t = f.Description;
                if (string.IsNullOrEmpty(t))
                    t = f.Id;
                csv.Append(Clean(t.Replace(Separatore, "")) + Separatore);
            }
        }

        for (int c = 0; c < view.Columns.Count; c++)
        {
            if (view.Columns[c].Settings.Visible &&
                view.Columns[c].DataType != ColumnDataType.Action &&
                view.Columns[c].DataType != ColumnDataType.Icon &&
                view.Columns[c].DataType != ColumnDataType.Image)
            {
                var t = view.Columns[c].Settings.Title?.Replace(Separatore, "");
                csv.Append(Clean(t) + Separatore);
            }
        }
        csv.AppendLine();
        foreach (var row in result.Page.Rows)
        {
            for (int ck = 0; ck < view.KeyFields.Index.Count; ck++)
            {
                var f = view.Columns.FirstOrDefault(c => c.Id == view.KeyFields.Index[ck]);
                if (f != null)
                {
                    var t = row.Keys[ck].Replace(Separatore, "");
                    csv.Append(Clean(t) + Separatore);
                }
            }

            int i = 0;
            for (int c = 0; c < view.Columns.Count; c++)
            {
                if (view.Columns[c].Settings.Visible)
                {
                    if (
                        view.Columns[c].DataType != ColumnDataType.Action &&
                        view.Columns[c].DataType != ColumnDataType.Icon &&
                        view.Columns[c].DataType != ColumnDataType.Image
                        )
                    {
                        var f = row.Columns[i].Description?.Replace(Separatore, "") + "";
                        if (view.Columns[c].DataType == ColumnDataType.Date && f == "01/01/0001")
                            f = "";
                        if (view.Columns[c].DataType == ColumnDataType.Number && f != null & f.Length > 1)
                        {
                            if (f.EndsWith(" B") || f.EndsWith(" KB") || f.EndsWith(" MB") || f.EndsWith(" GB"))
                            {
                                if (double.TryParse(f.Substring(0, f.Length - 2), out var n))
                                {
                                    if (f.EndsWith(" B"))
                                        f = n.ToString();
                                    if (f.EndsWith(" KB"))
                                        f = (n * 1000).ToString();
                                    if (f.EndsWith(" MB"))
                                        f = (n * 1000_000).ToString();
                                    if (f.EndsWith(" GB"))
                                        f = (n * 1000_000_000).ToString();
                                }
                            }
                        }
                        csv.Append(Clean(f) + Separatore);
                    }
                    i++;
                }
            }
            csv.AppendLine();
        }

        return csv.ToString();

    }
    
    private string Clean(string s) {  return s.Replace("\n", " ").Replace("\r", "").Replace("\t", ""); }


    /// <summary>
    /// Questo metodo fornisce una pagina di risultati relativa ad una vista di dati precedentemente ricevuta.
    /// Va richiamato dopo il metodo <see cref="Get(SearchRequest)"/> per ottenre una nuova pagina (anche la prima) 
    /// </summary>
    /// <param name="request">Fornisce informazioni relative al tipo di dati da visualizzare, la dimensione delle pagine, il numero di pagina e i filtri da applicare </param>
    /// <returns>Ritorna un oggetto <see cref="SearchResultPage"/> che contiene l'elenco dei dati della pagina richiesta</returns>

    [HttpPost("page")]
    public async Task<SearchResultPage> GetPage(SearchRequest request)
    {
        var u = userContext.Get();
        string userId = u.userId;
        var view = await viewManager.Get(request.ViewId, userId);
        var style = await uISettingsRepository.Get(u.userId, "ViewStyle." + view.ViewId);
        if (!string.IsNullOrEmpty(style)) view.ViewStyle = (ViewStyle)int.Parse(style);
        var service = await viewMapper.GetSearchService(request.ViewId);
        return await service.GetPage(view, request, u);
    }


    [HttpGet("changerowState")]
    public async Task<bool> ChangeRowState(string viewId, RowId rowId, RowState newState)
    {
        var u = userContext.Get();
        string userId = u.userId;
        var service = await viewMapper.GetSearchService(viewId);
        return await service.ChangeRowState(rowId, newState, u);
    }

    [HttpGet("changevisibility")]
    public async Task<bool> ChangeVisibility(string ViewId, string columnId, bool show)
    {
        var u = userContext.Get();
        string userId = u.userId;
        var changed = await viewManager.ChangeVisibility(ViewId, userId, columnId, show);
        return changed;
    }

    [HttpPost("changesorting")]
    public async Task<SearchResult> ChangeSorting(SearchRequest request, string columnId, SortingType sortingType)
    {
        var u = userContext.Get();
        string userId = u.userId;
        var changed = await viewManager.ChangeSorting(request.ViewId, userId, columnId, sortingType);
        return await Get(request);
    }

    //[HttpPost("changeaggregation")]
    //public async Task<SearchResult> ChangeAggregation(SearchRequest request, string columnId, AggregateType aggregateType)
    //{
    //    var changed = await viewManager.ChangeAggregation(request.ViewId, userId, columnId, aggregateType);
    //    return await Get(request);
    //}

    [HttpGet("resizecolumn")]
    public async Task<bool> ResizeColumn(string ViewId, string columnId, string Width)
    {
        var u = userContext.Get();
        string userId = u.userId;
        var changed = await viewManager.ChangeWidth (ViewId, userId, columnId, Width);
        return changed;
    }
    [HttpGet("renamecolumn")]
    public async Task<bool> RenameColumn(string ViewId, string columnId, string Name)
    {
        var u = userContext.Get();
        string userId = u.userId;
        var view = await viewManager.Get(ViewId, u.userId);
        var col = view.Columns.FirstOrDefault(c => c.Id == columnId);
        col.Settings.Title = string.IsNullOrEmpty(Name) ? col.Description : Name;
        col.Settings.Visible = true;
        return await viewManager.Save(view, u.userId);
    }
    [HttpGet("aggregatecolumn")]
    public async Task<bool> AggregateColumn(string ViewId, string columnId, AggregateType aggregate)
    {
        var u = userContext.Get();
        string userId = u.userId;
        var view = await viewManager.Get(ViewId, u.userId);
        var col = view.Columns.FirstOrDefault(c => c.Id == columnId);
        col.Settings.AggregateType = aggregate;
        return await viewManager.Save(view, u.userId);
    }

    [HttpGet("movecolumn")]
    public async Task<bool> MoveColumn(string ViewId, string fromColumnId, string toColumnId)
    {
        var u = userContext.Get();
        string userId = u.userId;
        var changed = await viewManager.MoveColumn(ViewId, userId, fromColumnId, toColumnId);
        return changed;
    }


    [HttpGet("resetcolumns")]
    public async Task<bool> Reset(string ViewId)
    {
        var u = userContext.Get();
        var changed = await viewManager.Reset(ViewId, u.userId);
        return true;
    }

    [HttpGet("setdefaultcolumns")]
    public async Task<bool> SetDefault(string ViewId)
    {
        var u = userContext.Get();
        var view = await viewManager.Get(ViewId, u.userId);
        return await viewManager.Save(view, SpecialUser.SystemUser);
    }


    [HttpPost("updatecolumns")]
    public async Task<bool> UpdateColumns(ViewProperties updatedView)
    {
        var u = userContext.Get();
        return await  viewManager.Save(updatedView, u.userId);
    }

    [HttpGet("sortcolumn")]
    public async Task<SortingType> ToggleSort(string ViewId, string ColumnId)
    {
        var u = userContext.Get();
        var view = await viewManager.Get(ViewId, u.userId);
        var col = view.Columns.FirstOrDefault(c=>c.Id == ColumnId);
        var old = col.Settings.SortType;
        var sort = col.Settings.SortType == SortingType.Ascending ? SortingType.Descending : SortingType.Ascending;
        view.Columns.ForEach(c => c.Settings.SortType = SortingType.None);
        col.Settings.SortType = sort;
        return  (await viewManager.Save(view, u.userId)) ? sort : old;
    }



}
