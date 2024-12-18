using Microsoft.AspNetCore.Mvc;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace OpenDMS.DocumentManager.Controllers;

[Authorize]
[ApiController]
[Route("/api/ui/[controller]")]
public class SearchController : Controller
{
    private readonly ILogger logger;
    private readonly ILoggedUserProfile userContext;
    private readonly IViewServiceResolver viewMapper;
    private readonly IViewManager viewManager;

    public SearchController(ILogger<SearchController> logger,
        ILoggedUserProfile userContext, 
        IViewServiceResolver viewMapper,
        IViewManager viewManager)
    {
        this.logger = logger;
        this.userContext = userContext;
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
    [HttpPost("get", Name ="New")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            public async Task<SearchResult> Get(SearchRequest request)
    {
        var u = userContext.Get();
        string userId = u.userId;
        var view = await viewManager.Get(request.ViewId, u.userId);
        var service = await viewMapper.GetSearchService(request.ViewId);
        return await service.Get(view, request, u);
    }

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
        var service = await viewMapper.GetSearchService(request.ViewId);
        return await service.GetPage(view, request, u);
    }


    [HttpPost("changerowState")]
    public async Task<bool> ChangeRowState(string viewId, RowId rowId, RowState newState)
    {
        var u = userContext.Get();
        string userId = u.userId;
        var service = await viewMapper.GetSearchService(viewId);
        return await service.ChangeRowState(rowId, newState, u);
    }

    [HttpPost("changevisibility")]
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


    [HttpPost("movecolumn")]
    public async Task<bool> MoveColumn(string ViewId, string fromColumnId, string toColumnId)
    {
        var u = userContext.Get();
        string userId = u.userId;
        var changed = await viewManager.MoveColumn(ViewId, userId, fromColumnId, toColumnId);
        return changed;
    }

}
