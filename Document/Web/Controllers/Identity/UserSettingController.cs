using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;

namespace Web.Controllers.Identity;

[Authorize]
[ApiController]
[Route("internalapi/userSetting")]
public class UserSettingController : ControllerBase
{

    private readonly IUserSettingsRepository userSettingRepo;
    private readonly IUserService userService;

    public UserSettingController(IUserSettingsRepository userSettingRepo, IUserService userService)
    {
        this.userSettingRepo = userSettingRepo;
        this.userService = userService;
    }

    /// <summary>
    /// Restituisce l'id dell'utente loggato
    /// </summary>
    /// <returns>Restituisce il valore del setting.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
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
    [HttpGet("Roles")]
    public async Task<ActionResult<string>> Roles()
    {
        try
        {
            var user = await userService.GetUserProfile(User.Identity.Name);
            var roles = string.Join(", ", user.Roles);
            return roles;
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
    [HttpGet("Setting/{Name}")]
    public async Task<ActionResult<string>> Get(string Name)
    {
        try
        {
            var user = await userService.GetById(User.Identity.Name);
            var result = await userSettingRepo.Get(user.ContactId, Name);
            return Ok(result);
        }
        catch (Exception ex) { return BadRequest(""); }
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
    [HttpGet("Setting/{Company}/{Name}")]
    public async Task<ActionResult<string>> Get(int Company, string Name)
    {
        try
        {
            var user = await userService.GetById(User.Identity.Name);
            var result = await userSettingRepo.Get(Company, user.ContactId, Name);
            return  Ok(result);
        }
        catch (Exception ex) { return BadRequest(""); }
    }

    /// <summary>
    /// Aggiungere o modifica un UserSetting.
    /// </summary>
    /// <param name="bd"> Oggetto UserSetting da aggiungere o modificare</param>
    /// <returns>Restituisce il valore del setting creato.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("Setting")]
    public async Task<ActionResult<string>> Set(UserSetting bd)
    {
        try
        {
            var user = await userService.GetById(User.Identity.Name);
            bd.ContactId = user.ContactId;
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
    [HttpGet("Settings")]
    public async Task<ActionResult<List<UserSetting>>> GetAll(int CompanyId)
    {
        try
        {
            var user = await userService.GetById(User.Identity.Name);
            return Ok(await userSettingRepo.GetAll(CompanyId, user.ContactId));
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

}