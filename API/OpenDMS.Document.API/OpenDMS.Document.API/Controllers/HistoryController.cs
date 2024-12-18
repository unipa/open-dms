using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;

namespace OpenDMS.DocumentManager.Controllers;

[Authorize]
[ApiController]
[Route("api/document/[controller]")]
public class HistoryController : ControllerBase
{

    private readonly IHistoryRepository historyRepo;
    private readonly ILoggedUserProfile userContext;
    private readonly IDocumentService documentService;

    public HistoryController(IHistoryRepository historyRepo, ILoggedUserProfile userContext, IDocumentService documentService)
    {
        this.historyRepo = historyRepo;
        this.userContext = userContext;
        this.documentService = documentService;
    }


    /// <summary>
    /// Metodo per ottenere i dati relativi ad una HistoryEntry tramite Id. 
    /// </summary>
    /// <param name="historyEntryId"> Id della HistoryEntry da cercare </param>
    /// <returns></returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(HistoryEntry))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{historyEntryId}")]
    public async Task<ActionResult<HistoryEntry>> GetById(int historyEntryId)
    {
        try
        {
            var result = await historyRepo.GetById(historyEntryId);
            return result == null ? NotFound("La HistoryEntry indicata non è stata trovata.") : Ok(result);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary>
    /// Metodo per ottenere i dati relativi a tutte le HistoryEntry che rispettano i filtri passati dall'oggetto HistoryFilters. 
    /// </summary>
    /// <param name="filters">Oggetto HistoryFilters per passare i filtri.</param>
    /// <returns></returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IList<HistoryEntry>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("GetByFilters")]
    public async Task<ActionResult<IList<HistoryEntry>>> GetByFilters(HistoryFilters filters)
    {
        var u = userContext.Get();
        var P = await documentService.GetPermission(filters.DocumentId, u, PermissionType.CanViewRegistry);
        if (P == null || P.Authorization != Domain.Enumerators.AuthorizationType.Granted)
            return BadRequest("Non sei autorizzato a visualizzare la cronistoria di questo documento");

        try
        {
            return Ok(await historyRepo.GetByFilters(filters));
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

  

    /// <summary>
    /// Metodo per il numero di HistoryEntry in base ai filtri passati tramite l'oggetto HistoryFilters.
    /// </summary>
    /// <param name="filters">Oggetto HistoryFilters per passare i filtri.</param>
    /// <returns></returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("Count")]
    public async Task<ActionResult<int>> Count(HistoryFilters filters)
    {
        var u = userContext.Get();
        var P = await documentService.GetPermission(filters.DocumentId, u, PermissionType.CanViewRegistry);
        if (P == null || P.Authorization != Domain.Enumerators.AuthorizationType.Granted)
            return BadRequest("Non sei autorizzato a visualizzare la cronistoria di questo documento");
        try
        {
            return await historyRepo.Count(filters);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}