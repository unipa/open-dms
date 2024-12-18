using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Repositories;

namespace OpenDMS.UserManager.API.Controllers;

[Authorize]
[ApiController]
[Route("api/identity/[controller]")]
public class UserSettingController : ControllerBase
{

    private readonly IUserSettingsRepository userSettingRepo;

    public UserSettingController(IUserSettingsRepository userSettingRepo)
    {
        this.userSettingRepo = userSettingRepo;
    }

    /// <summary>
    /// Restituisce l'id dell'utente loggato
    /// </summary>
    /// <returns>Restituisce il valore del setting.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("Logged")]
    public async Task<ActionResult<string>> Current()
    {
        try
        {
            var userId = User.Identity.Name;
            return userId;
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }
    /// <summary>
    /// Restituisce l'id dell'utente loggato
    /// </summary>
    /// <returns>Restituisce il valore del setting.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("Logged/Roles")]
    public async Task<ActionResult<string>> Roles()
    {
        try
        {
            var userId = string.Join(", ", User.Claims.Where(c => c.Type == "roles").Select(r => r.Value));
            return userId;
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary>
    /// Restituisce i dati relativi ad un UserSetting tramite userId e Name. 
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
            var result = await userSettingRepo.Get(userId, Name);
            return string.IsNullOrEmpty(result) ? NotFound("Il Settings indicato non � stato trovato.") : Ok(result);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary>
    /// Restituisce i dati relativi ad un UserSetting tramite Company, userId, Name. 
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
            var result = await userSettingRepo.Get(Company, userId, Name);
            return string.IsNullOrEmpty(result) ? NotFound("Il Settings indicato non � stato trovato.") : Ok(result);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary>
    /// Aggiungere o modifica un UserSetting.
    /// </summary>
    /// <param name="bd"> Oggetto UserSetting da aggiungere o modificare</param>
    /// <returns>Restituisce il valore del setting creato.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<string>> Set(UserSetting bd)
    {
        try
        {
            bd.ContactId = User.Identity.Name;
            await userSettingRepo.Set(bd.CompanyId, bd.ContactId, bd.AttributeId, bd.Value);
            var res = await userSettingRepo.Get(bd.CompanyId, bd.ContactId, bd.AttributeId);
            return Ok(res/*await userSettingRepo.Get(bd.CompanyId, bd.UserId, bd.Name)*/);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Restituisce i dati relativi a tutti gli UserSetting associati a una Company e un UserId. 
    /// </summary>
    /// <param name="Company">Id della Company.</param>
    /// <returns>Restituisce una lista di oggetti UserSetting.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<UserSetting>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<ActionResult<List<UserSetting>>> GetAll(int CompanyId)
    {
        try
        {
            var UserId = User.Identity.Name;
            return Ok(await userSettingRepo.GetAll(CompanyId, UserId));
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

}