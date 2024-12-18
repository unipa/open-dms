using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.Filters;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Settings;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Web.Controllers.Settings;

[Authorize]
[Authorization(":admin")]
[ApiController]
[Route("internalapi/[controller]")]
public class LookupTableController : ControllerBase
{

    private readonly ILookupTableService lookupTableRepo;

    public LookupTableController(ILookupTableService lookupTableRepo)
    {
        this.lookupTableRepo = lookupTableRepo;
    }


    /// <summary>
    /// Restituisce i dati relativi ad una LookupTable tramite Id e TableId. 
    /// </summary>
    /// <param name="TableId"> Id della tabella </param>
    /// <param name="Id"> Id </param>
    /// <returns>Restituisce un oggetto LookupTable</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(LookupTable))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{TableId}/{Id}")]
    public async Task<ActionResult<LookupTable>> GetById(string TableId, string Id)
    {
        try
        {
            if (string.IsNullOrEmpty(Id)) return BadRequest(nameof(Id) + " non pu� essere vuoto");
            if (string.IsNullOrEmpty(TableId)) return BadRequest(nameof(TableId) + " non pu� essere vuoto");

            TableId = TableId.ToUpper();
            Id = Id.ToUpper();

            var result = await lookupTableRepo.GetById(TableId, Id);
            return result == null ? NotFound("La LookupTable indicata non � stata trovata.") : Ok(result);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary>
    /// Restituisce i dati relativi a tutte le LookupTable associate ad un TableId. 
    /// </summary>
    /// <param name="TableId"> Id della tabella </param>
    /// <returns>Restituisce una lista di oggetti LookupTable</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<LookupTable>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{TableId}")]
    public async Task<ActionResult<List<LookupTable>>> GetAll(string TableId)
    {
        try
        {
            if (string.IsNullOrEmpty(TableId)) return BadRequest(nameof(TableId) + " non pu� essere vuoto");

            TableId = TableId.ToUpper();

            var result = await lookupTableRepo.GetAll(TableId);
            return result.Count == 0 ? Ok(new List<LookupTable>()) : Ok(result);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary> 
    /// Salvare una nuova LookupTable.
    /// </summary>
    /// <param name="bd">Oggetto LookupTable da creare</param>
    /// <returns>Restituisce l'oggetto LookupTable creato</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(LookupTable))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    public async Task<ActionResult<LookupTable>> Insert(LookupTable bd)
    {
        try
        {
            bd.Id = bd.Id.ToUpper();
            bd.TableId = bd.TableId.ToUpper();

            var exist = await lookupTableRepo.GetById(bd.TableId, bd.Id);
            if (exist.Description.Equals("#" + bd.Id))
            {

                int res = await lookupTableRepo.Insert(bd);
                return res > 0 ? Ok(await lookupTableRepo.GetById(bd.TableId, bd.Id)) : BadRequest("L'inserimento non � andato a buon fine.");
            }
            else return Conflict("Non pu� essere inserita una LookupTable con un TableId gi� esistente, usa un altro TableId oppure se vuoi modificarla usa il metodo PUT.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Modifica una specifica LookupTable identificata tramite Id.
    /// </summary>
    /// <param name="bd">Oggetto LookupTable da modificare.</param>
    /// <returns>Restituisce l'oggetto LookupTable modificato.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(LookupTable))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut]
    public async Task<ActionResult<LookupTable>> Update(LookupTable bd)
    {
        try
        {
            bd.Id = bd.Id.ToUpper();
            bd.TableId = bd.TableId.ToUpper();

            var exist = await lookupTableRepo.GetById(bd.TableId, bd.Id);
            if (!exist.Description.Equals("#" + bd.Id))
            {
                exist.Annotation = bd.Annotation;
                exist.Description = bd.Description;
                int res = await lookupTableRepo.Update(exist);
                return res > 0 ? Ok(await lookupTableRepo.GetById(bd.TableId, bd.Id)) : BadRequest("L'aggiornamento non � andato a buon fine.");
            }
            else return NotFound("La LookupTable indicata non � stata trovata");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Elimina una LookupTable identificata tramite Id.
    /// </summary>
    /// <param name="TableId"></param>
    /// <param name="Id"></param>
    /// <returns>Ritorna 200 in caso di successo, 404 in caso di NotFound e 400 in caso di fallimento.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{TableId}/{Id}")]
    public async Task<ActionResult<string>> Delete(string TableId, string Id)
    {
        try
        {
            if (string.IsNullOrEmpty(Id)) return BadRequest(nameof(Id) + " non pu� essere vuoto");
            if (string.IsNullOrEmpty(TableId)) return BadRequest(nameof(TableId) + " non pu� essere vuoto");

            TableId = TableId.ToUpper();
            Id = Id.ToUpper();

            var exist = await lookupTableRepo.GetById(TableId, Id);
            if (!exist.Description.Equals("#" + Id))
            {
                var result = await lookupTableRepo.Delete(exist);
                return result > 0 ? Ok() : BadRequest("Eliminazione non � andata a buon fine.");
            }
            else return NotFound("La LookupTable indicata non � stata trovata");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}