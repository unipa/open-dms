using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;

namespace Web.Controllers.Identity;

[Authorize]
[ApiController]
[Route("internalapi/ui")]
public class UISettingController : ControllerBase
{

    private readonly IUISettingsRepository userSettingRepo;
    private readonly IUserService userService;

    public UISettingController(IUISettingsRepository uiSettingRepo, IUserService userService)
    {
        this.userSettingRepo = uiSettingRepo;
        this.userService = userService;
    }

 

    /// <summary>
    /// Restituisce i dati relativi ad un UISetting tramite userId e Name. 
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
            var result = await userSettingRepo.Get(User.Identity.Name, Name);
            return Ok(result);
        }
        catch (Exception ex) { return BadRequest(""); }
    }

 
    ///// <summary>
    ///// Aggiungere o modifica un UISetting.
    ///// </summary>
    ///// <param name="bd"> Oggetto UserSetting da aggiungere o modificare</param>
    ///// <returns>Restituisce il valore del setting creato.</returns>
    //[ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[HttpGet("Setting/{Name}/{value}")]
    //public async Task<ActionResult<string>> Set(string Name, string value)
    //{
    //    try
    //    {
    //        var ContactId = User.Identity.Name;
    //        await userSettingRepo.Set(ContactId, Name, value);
    //        var res = await userSettingRepo.Get(ContactId, Name);
    //        return Ok(res);
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}

    /// <summary>
    /// Aggiungere o modifica un UISetting.
    /// </summary>
    /// <param name="name"> Oggetto UserSetting da aggiungere o modificare</param>
    /// <returns>Restituisce il valore del setting creato.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("Setting")]
    public async Task<ActionResult<string>> Set2(string name, string value)
    {
        try
        {
            var ContactId = User.Identity.Name;
            await userSettingRepo.Set(ContactId, name, value);
            var res = await userSettingRepo.Get(ContactId, name);
            return Ok(res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }



}