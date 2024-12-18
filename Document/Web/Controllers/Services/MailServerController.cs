using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenDMS.Core.Filters;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Repositories;
using OpenDMS.MailSpooler.Core;
using OpenDMS.MailSpooler.Core.Interfaces;
using Web.DTOs;

namespace Web.Controllers.Services;

//DPM
[ApiExplorerSettings(IgnoreApi = true)]
[Authorize]
[ApiController]
[Route("internalapi/[controller]")]
public class MailServerController : ControllerBase
{

    private readonly IMailServerRepository mailServerRepository;
    private readonly IAuthenticatorFactory _authFact;

    public MailServerController(IMailServerRepository mailServerRepository, IAuthenticatorFactory authFact)
    {
        this.mailServerRepository = mailServerRepository;
        this._authFact = authFact;
    }


    /// <summary>
    /// Restituisce i dati relativi ad un MailServer tramite Id. 
    /// </summary>
    /// <param name="Id"> Id del MailServer da cercare </param>
    /// <returns>Restituisce un oggetto MailServer</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(MailServer))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("{Id}")]
    public async Task<ActionResult<MailServer>> GetById(int Id)
    {
        try
        {
            var result = await mailServerRepository.GetById(Id);
            return result == null ? Ok(null) : Ok(result);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary>
    /// Restituisce i dati relativi a tutte i MailServer. 
    /// </summary>
    /// <returns>Restituisce una lista di oggetti MailServer</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IList<MailServer>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<ActionResult<IList<MailServer>>> GetAll()
    {
        try
        {
            return Ok(await mailServerRepository.GetAll());
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary> 
    /// Aggiunge un nuovo MailServer.
    /// </summary>
    /// <param name="bd">Oggetto MailServer da creare</param>
    /// <returns>Restituisce l'oggetto MailServer appena inserito</returns>
    [Authorization(":admin")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(MailServer))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    public async Task<ActionResult<MailServer>> Insert(MailServer_DTO bd)
    {
        try
        {
            if (await mailServerRepository.GetById(bd.Id) == null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<MailServer_DTO, MailServer>());
                var mp = new Mapper(config);
                var insert = mp.Map<MailServer>(bd);
                insert.Id = 0;
                var r = await mailServerRepository.Insert(insert);
                return r > 0 ? Ok(await mailServerRepository.GetById(insert.Id)) : BadRequest("L'inserimento non è andato a buon fine.");
            }
            else return Conflict("Non può essere inserita un MailServer con un Id già esistente, usa un altro Id oppure se vuoi modificarla usa il metodo PUT.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Modifica uno specifico MailServer, identificato tramite l'id contenuto nel body.
    /// </summary>
    /// <param name="bd">Oggetto MailServer modificato </param>
    /// <returns>Restituisce l'oggetto MailServer modificato</returns>
    [Authorization(":admin")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(MailServer))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut]
    public async Task<ActionResult<MailServer>> Update(MailServer_DTO bd)
    {
        try
        {
            var exist = await mailServerRepository.GetById(bd.Id);
            if (exist != null)
            {
                exist.Domain = bd.Domain;
                exist.InboxServer = bd.InboxServer;
                exist.InboxServerPort = bd.InboxServerPort;
                exist.InboxSSL = bd.InboxSSL;
                exist.InboxProtocol = bd.InboxProtocol;
                exist.SMTPServer = bd.SMTPServer;
                exist.SMTPServerPort = bd.SMTPServerPort;
                exist.SMTPServerSSL = bd.SMTPServerSSL;
                exist.Status = bd.Status;
                exist.MailType = bd.MailType;
                exist.AuthenticationType = bd.AuthenticationType;
                exist.TenantID = bd.TenantID;
                exist.ClientID = bd.ClientID;
                exist.ClientSecret = bd.ClientSecret;

                await mailServerRepository.Update(exist);
                return Ok(await mailServerRepository.GetById(bd.Id));
            }
            else return BadRequest("Il MailServer indicato non è stato trovato");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Elimina un MailServer.
    /// </summary>
    /// <param name="Id">Id del MailServer da eliminare</param>
    /// <returns>Ritorna 200 in caso di successo e 400 in caso di fallimento.</returns>
    [Authorization(":admin")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpDelete("{Id}")]
    public async Task<ActionResult> Delete(int Id)
    {
        try
        {
            await mailServerRepository.Delete(Id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    //[AllowAnonymous]
    //[ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Mailbox))]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[HttpGet("GetOAuthUrlByDomain/{Address}")]

    //public async Task<ActionResult> GetOAuthUrlByDomain(string Address)
    //{
    //    try
    //    {
    //        var mailServer = await mailServerRepository.GetByDomain(Address.Split("@")[1]);
    //        if (mailServer.AuthenticationType == AuthenticationType.None || mailServer.AuthenticationType == AuthenticationType.UserCredentials)
    //            return Ok("");

    //        var mailbox = new Mailbox()
    //        {
    //            MailServer = mailServer,
    //            MailAddress = Address,
    //            UserId = HttpContext.User.Identity.Name
    //            // RedirectURI = "https://localhost:7001/Services/Oauth/Index"
    //        };

    //        var _auth = _authFact.GetAuthenticator(mailServer.AuthenticationType);
    //        return Ok(_auth.GetOAUTH_URL(
    //        new OAUTHState()
    //        {   
    //            MailboxID = 
    //            Mailtype =  mailServer.MailType,
    //            OauthType = mailServer.AuthenticationType,
    //            eMail = Address,
    //            UserName = HttpContext.User.Identity.Name
    //        }
    //        , mailbox));
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}

    //[AllowAnonymous]
    //[ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Mailbox))]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[HttpGet("GetOAuthAccessData/{Domain}/{Code}/{State}")]
    //public async Task<ActionResult<OAUTHToken>> GetOAuthAccessData(string domain, string code, string state)
    //{
    //    try
    //    {
           



    //        var mailServer = await mailServerRepository.GetByDomain(domain);
    //        if (mailServer.AuthenticationType == AuthenticationType.None || mailServer.AuthenticationType == AuthenticationType.UserCredentials)
    //            return Ok(false);

    //        var mailbox = new Mailbox()
    //        {
    //            MailServer = mailServer,
    //            MailAddress = "none@"+domain,
    //            UserId = HttpContext.User.Identity.Name
    //            // RedirectURI = "https://localhost:7001/Services/Oauth/Index"
    //        };

    //        var _auth = _authFact.GetAuthenticator(mailServer.AuthenticationType);
    //        var token = _auth.AcquireCode(code, mailbox);
    //        if (token != null)
    //            return Ok(token);
    //        else
    //            return NotFound("Nessun token ottenuto");
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}
   
}