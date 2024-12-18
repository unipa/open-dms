using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Mails;

namespace Web.Controllers.Services;


[ApiExplorerSettings(IgnoreApi = true)]
[Authorize]
[ApiController]
[Route("internalapi/[controller]")]
public class MailboxController : ControllerBase
{
    private readonly ILoggedUserProfile loggedUserProfile;
    private readonly IMailboxService mailboxService;

    public MailboxController(
        ILoggedUserProfile loggedUserProfile,
        IMailboxService mailboxService)
    {
        this.loggedUserProfile = loggedUserProfile;
        this.mailboxService = mailboxService;
    }


    /// <summary>
    /// Restituisce i dati relativi ad una Mailbox tramite Id. 
    /// </summary>
    /// <param name="Id"> Id della Mailbox da cercare </param>
    /// <returns>Restituisce un oggetto Mailbox</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Mailbox))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("ById/{Id}")]
    public async Task<ActionResult<Mailbox>> GetById(int Id)
    {
        var u = loggedUserProfile.Get();
        try
        {
            var result = await mailboxService.GetById(Id);
            return result != null && (result.UserId == u.userId || u.Roles.Select(s => s.Id).Contains("admin")) ? Ok(result) : NotFound();
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary>
    /// Restituisce i dati relativi ad una Mailbox tramite Id. 
    /// </summary>
    /// <param name="Id"> Id della Mailbox da cercare </param>
    /// <returns>Restituisce un oggetto Mailbox</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Mailbox))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("ByAddress/{Address}")]
    public async Task<ActionResult<Mailbox>> GetByAddress(string Address)
    {
        var u = loggedUserProfile.Get();
        try
        {
            var result = await mailboxService.GetByAddress(Address);
            return result != null && (result.UserId == u.userId || u.Roles.Select(s => s.Id).Contains("admin")) ? Ok(result) : NotFound();
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary>
    /// Restituisce i dati relativi a tutte le Mailbox. 
    /// </summary>
    /// <returns>Restituisce una lista di oggetti Mailbox</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IList<Mailbox>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<ActionResult<IList<Mailbox>>> GetAll()
    {
        try
        {
            var u = loggedUserProfile.Get();
            return Ok(await mailboxService.GetAll(u));
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary> 
    /// Aggiunge una nuova Mailbox.
    /// </summary>
    /// <param name="bd">Oggetto Mailbox da creare</param>
    /// <returns>Restituisce l'oggetto Mailbox appena inserito</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Mailbox))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    public async Task<ActionResult<Mailbox>> Insert(Mailbox bd)
    {
        try
        {
            var u = loggedUserProfile.Get();
            if (bd.UserId != u.userId) throw new Exception("Non sei autorizzato a modificare questa configurazione");
            if (await mailboxService.GetById(bd.Id) == null)
            {
                bd.Id = 0;
                await mailboxService.Create(bd);
                return Ok(await mailboxService.GetById(bd.Id));
            }
            else return Conflict("Non può essere inserita una Mailbox con un Id già esistente, usa un altro Id oppure se vuoi modificarla usa il metodo PUT.");
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                if (ex.InnerException.Message.Contains("Duplicate entry")) return Conflict(ex.InnerException.Message);
            }

            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Modifica una specifica Mailbox, identificato tramite l'id contenuto nel body.
    /// </summary>
    /// <param name="bd">Oggetto Mailbox da modificato </param>
    /// <returns>Restituisce l'oggetto Mailbox modificato</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Mailbox))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut]
    public async Task<ActionResult<Mailbox>> Update(Mailbox bd)
    {
        try
        {
            var u = loggedUserProfile.Get();
            if (bd.UserId != u.userId) throw new Exception("Non sei autorizzato a modificare questa configurazione");

            var exist = await mailboxService.GetById(bd.Id);
            if (exist != null)
            {
                exist.MailAddress = bd.MailAddress;
                exist.DisplayName = bd.DisplayName;
                exist.UserSignature = bd.UserSignature;
                exist.MailServerId = bd.MailServerId;
                exist.Account = bd.Account;
                exist.Password = bd.Password;
                exist.TokenId = bd.TokenId;
                exist.RefreshToken = bd.RefreshToken;
                exist.Validated = bd.Validated;
                exist.UserId = bd.UserId;
                exist.CompanyId = bd.CompanyId;
                exist.ReadOnlyProfiles = bd.ReadOnlyProfiles;
                exist.SendEnabledProfiles = bd.SendEnabledProfiles;
                exist.DraftEnabledProfiles = bd.DraftEnabledProfiles;
                exist.EnableDownload = bd.EnableDownload;
                exist.DeleteDownloadedMail = bd.DeleteDownloadedMail;
                exist.DownloadImapFolders = bd.DownloadImapFolders;
                exist.SaveToImapFolder = bd.SaveToImapFolder;
                exist.FirstReceivingMessageDate = bd.FirstReceivingMessageDate;
                exist.DaysToRead = bd.DaysToRead;
                exist.GracePeriod = bd.GracePeriod;

                await mailboxService.Update(exist);
                return Ok(await mailboxService.GetById(bd.Id));
            }
            else return NotFound("La Mailbox indicata non è stata trovata");
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                if (ex.InnerException.Message.Contains("Duplicate entry")) return Conflict(ex.InnerException.Message);
            }

            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Elimina una Mailbox.
    /// </summary>
    /// <param name="Id">Id della Mailbox da eliminare</param>
    /// <returns>Ritorna 200 in caso di successo e 400 in caso di fallimento.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{Id}")]
    public async Task<ActionResult> Delete(int Id)
    {
        try
        {
            var u = loggedUserProfile.Get();
            var bd = await mailboxService.GetById(Id);
            if (bd.UserId != u.userId) throw new Exception("Non sei autorizzato a modificare questa configurazione");

            await mailboxService.Delete(Id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}