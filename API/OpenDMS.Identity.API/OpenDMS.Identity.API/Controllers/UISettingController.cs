using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Repositories;

namespace OpenDMS.UserManager.API.Controllers;

[Authorize]
[ApiController]
[Route("api/identity/[controller]")]
public class UISettingController : ControllerBase
{

    private readonly IUISettingsRepository uiSettingRepo;

    public UISettingController(IUISettingsRepository uiSettingRepo)
    {
        this.uiSettingRepo = uiSettingRepo;
    }


    /// <summary>
    /// Restituisce i dati relativi ad un UISetting tramite Company, UserId e Name. 
    /// </summary>
    /// <param name="Company">Id della Company.</param>
    /// <param name="Name">Nome del setting.</param>
    /// <returns>Restituisce il valore del setting.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{Company}/{userId}/{Name}")]
    public async Task<ActionResult<string>> Get(int Company, string Name)
    {
        try
        {
            var userId = User.Identity.Name;
            var result = await uiSettingRepo.Get(Company, userId, Name);
            return string.IsNullOrEmpty(result) ? NotFound("Il Settings indicato non � stato trovato.") : Ok(result);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary>
    /// Restituisce i dati relativi ad un UISetting tramite UserId e Name. 
    /// </summary>
    /// <param name="Name">Nome del setting.</param>
    /// <returns>Restituisce il valore del setting.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{userId}/{Name}")]
    public async Task<ActionResult<string>> Get(string Name)
    {
        try
        {
            var userId = User.Identity.Name;
            var result = await uiSettingRepo.Get(userId, Name);
            return string.IsNullOrEmpty(result) ? NotFound("Il Settings indicato non � stato trovato.") : Ok(result);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary>
    /// Aggiungere o modifica un UISetting.
    /// </summary>
    /// <param name="bd">Oggetto UISetting da aggiungere.</param>
    /// <returns>Restituisce il valore del setting creato.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<string>> Set(UISetting bd)
    {
        try
        {
            bd.UserId = User.Identity.Name;
            await uiSettingRepo.Set(bd.CompanyId, bd.UserId, bd.Name, bd.Value);
            return Ok(await uiSettingRepo.Get(bd.CompanyId, bd.UserId, bd.Name));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Restituisce i dati relativi a tutti gli UISetting associati a una Company e a un Id Utente. 
    /// </summary>
    /// <param name="Company">Id della Company.</param>
    /// <returns>Restituisce una lista di oggetti UISetting.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<UISetting>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<ActionResult<List<UISetting>>> GetAll(int Company)
    {
        try
        {
            var userID = User.Identity.Name;
            return Ok(await uiSettingRepo.GetAll(Company, userID));
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

}