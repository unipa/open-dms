using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Filters;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Repositories;
using OpenDMS.MultiTenancy.Interfaces;

namespace Web.Controllers.Settings;

//[Authorize]
[Authorize]
[ApiController]
[Route("internalapi/[controller]")]
public class ExternalDatasourceController : ControllerBase
{
    private readonly IDataSourceProvider dataSourceProvider;

    public ExternalDatasourceController(IDataSourceProvider dataSourceProvider, 
        IApplicationDbContextFactory connectionFactory)
    {
        this.dataSourceProvider = dataSourceProvider;
    }


    /// <summary>
    /// Restituisce un oggetto ExternalDatasource(che identifica una sorgente dati esterna) identificata tramite l'Id.
    /// </summary>
    /// <param name="Id"> Id sorgente dati esterna.</param>
    /// <returns>Restituisce un oggetto ExternalDatasource.</returns>
    [Authorization(":admin")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(ExternalDatasource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("{Id}")]
    public async Task<ActionResult<ExternalDatasource>> Get(string Id)
    {
        try
        {
            var result = new ExternalDatasource();
            result = await dataSourceProvider.Get(Id);
            return result == null ? new ExternalDatasource() : result;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Restituisce la lista delle sorgenti dati esterne.
    /// </summary>
    /// <returns>Restituisce una lista di oggetti ExternalDatasource.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<ExternalDatasource>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet]
    [Authorization(":admin")]
    public async Task<ActionResult<List<ExternalDatasource>>> List()
    {
        try
        {
            var dict = await dataSourceProvider.GetAll();
            List<ExternalDatasource> Results = dict.Values.ToList();
            return Ok(Results);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Testa la connessione di una sorgente dati esterna
    /// </summary>
    /// <param name="Source"> Sorgente esterna da testare </param>
    /// <returns>Restituisce una stringa "" in caso di successo oppure una stringa con dentro l'errore in caso di fallimento.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [HttpPost("Test")]
    [Authorization(":admin")]

    public async Task<ActionResult<string>> Test(ExternalDatasource Source)
    {
        var errorMessage = "";
        try
        {
            await dataSourceProvider.TestConnection(Source);
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }
        return errorMessage;
    }

    /// <summary>
    /// Salva o aggiorna una sorgente dati esterna 
    /// </summary>
    /// <param name="Source"> Sorgentee esterna da aggiornare o aggiungere </param>
    /// <returns>Restituisce l'oggetto ExternalDatasource creato.</returns>

    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(ExternalDatasource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    [Authorization(":admin")]

    public async Task<ActionResult<ExternalDatasource>> Save(ExternalDatasource Source)
    {
        try
        {
            int res = await dataSourceProvider.Set(Source.Name, Source);
            return res > 0 ? Ok(await dataSourceProvider.Get(Source.Id)) : BadRequest("L'inserimento non è andato a buon fine.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Cancella una sorgente dati esterna tramite Id
    /// </summary>
    /// <param name="Id"> Id della sorgente dati esterna</param>
    /// <returns>Ritorna 200 in caso di successo, 404 in caso di NotFound e 400 in caso di fallimento.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{Id}")]
    [Authorization(":admin")]

    public async Task<ActionResult<string>> Delete(string Id)
    {
        try
        {
            var exist = await dataSourceProvider.Get(Id);
            if (exist != null)
            {
                var result = await dataSourceProvider.Delete(Id);
                return result > 0 ? Ok("successo") : BadRequest("Eliminazione non riuscita");
            }
            else return NotFound("La sorgente dati esterna selezionata non è stata trovata");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Esegue una query e ritorna i primi <paramref name="pageSize"/> risultati
    /// </summary>
    /// <param name="Id"> Id della sorgente dati esterna</param>
    /// <param name="Query"> Sql da eseguire</param>
    /// <param name="pageSize"> Nr.masismo record da leggere</param>
    /// <param name="pageIndex"> Nr.pagina da leggere</param>
    /// <returns>Ritorna 200 in caso di successo, 404 in caso di NotFound e 400 in caso di fallimento.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("Query")]
    [Authorize]

    public async Task<ActionResult<List<string[]>>> Query(string Id, string Query, int pageSize=50, int pageIndex=0)
    {
        try
        {
            return Ok(await dataSourceProvider.Query(Id, Query, pageSize, pageIndex));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }



}