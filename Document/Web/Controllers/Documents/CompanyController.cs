using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Settings;

namespace Web.Controllers.Documents;

//DPM
//[Authorize]

[Authorize]
[ApiController]
[Route("internalapi/[controller]")]
public class CompanyController : ControllerBase
{

    private readonly ICompanyService companyRepo;

    public CompanyController(ICompanyService companyRepo)
    {
        this.companyRepo = companyRepo;
    }


    /// <summary>
    /// Restituisce i dati relativi ad una Company tramite Id. 
    /// </summary>
    /// <param name="Id"> Id della Company da cercare </param>
    /// <returns>Restituisce un oggetto Company</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Company))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{Id}")]
    public async Task<ActionResult<Company>> GetById(int Id)
    {
        try
        {
            var result = await companyRepo.GetById(Id);
            return result == null ? NotFound("La Company indicata non è stata trovata.") : Ok(result);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary>
    /// Restituisce i dati relativi a tutte le Company. 
    /// </summary>
    /// <returns>Restituisce una lista di oggetti Company</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IList<Company>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<ActionResult<IList<Company>>> GetAll()
    {
        try
        {
            return Ok(await companyRepo.GetAll());
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary> 
    /// Aggiunge una nuova Company.
    /// </summary>
    /// <param name="bd">Oggetto Company da creare</param>
    /// <returns>Restituisce l'oggetto Company appena inserito</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Company))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    public async Task<ActionResult<Company>> Insert(Company bd)
    {
        try
        {
            if (await companyRepo.GetById(bd.Id) == null)
            {
                bd.Id = 0;
                await companyRepo.Create(bd);
                return Ok(await companyRepo.GetById(bd.Id));
            }
            else return Conflict("Non può essere inserita una Company con un Id già esistente, usa un altro Id oppure se vuoi modificarla usa il metodo PUT.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Modifica una specifica Company, identificato tramite l'id contenuto nel body.
    /// </summary>
    /// <param name="bd">Oggetto Company da modificato </param>
    /// <returns>Restituisce l'oggetto Company modificato</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Company))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut]
    public async Task<ActionResult<Company>> Update(Company bd)
    {
        try
        {
            var exist = await companyRepo.GetById(bd.Id);
            if (exist != null)
            {
                exist.Description = bd.Description;
                exist.Theme = bd.Theme;
                exist.Logo = bd.Logo;
                exist.ERP = bd.ERP;
                exist.AOO = bd.AOO;
                exist.OffLine = bd.OffLine;
                await companyRepo.Update(exist);
                return Ok(await companyRepo.GetById(bd.Id));
            }
            else return NotFound("La Company indicata non è stata trovata");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Elimina una Company.
    /// </summary>
    /// <param name="Id">Id della Company da eliminare</param>
    /// <returns>Ritorna 200 in caso di successo, 404 in caso di NotFound e 400 in caso di fallimento.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{Id}")]
    public async Task<ActionResult> Delete(int Id)
    {
        try
        {
            var exist = await companyRepo.GetById(Id);
            if (exist != null)
            {
                await companyRepo.Delete(exist);
                return Ok();
            }
            else return NotFound("La Company indicata non è stata trovata");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}