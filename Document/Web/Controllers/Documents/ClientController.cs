using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;

namespace Web.Controllers.Documents;


[ApiController]
[Route("internalapi/client")]
public class ClientController : ControllerBase
{

    private readonly IDocumentService docRepo;
    private readonly IUserService userService;
    private readonly IUserSettingsRepository userSettingsRepository;
    private readonly ILoggedUserProfile userContext;

    public ClientController(
        IDocumentService docRepo,
        IUserService userService,
        IUserSettingsRepository userSettingsRepository,
        ILoggedUserProfile userContext
        )
    {
        this.docRepo = docRepo;
        this.userService = userService;
        this.userSettingsRepository = userSettingsRepository;
        this.userContext = userContext;
    }

    private async Task<UserProfile> GetUser(string clientSecret)
    {
        var a = clientSecret.IndexOf('-');
        if (a <= 0) throw new UnauthorizedAccessException("Utente non autorizzato all'accesso");
        string appName = clientSecret.Substring(0, a).ToLowerInvariant();
        clientSecret = clientSecret.Substring(a + 1);
        if (string.IsNullOrEmpty(clientSecret)) throw new UnauthorizedAccessException("Utente non autorizzato all'accesso");
        string uid = clientSecret.Substring(clientSecret.LastIndexOf('-') + 1);
        if (string.IsNullOrEmpty(uid)) throw new UnauthorizedAccessException("Utente non autorizzato all'accesso");
        clientSecret = clientSecret.Substring(0, clientSecret.LastIndexOf('-'));
        var user = await userService.GetById(uid);
        if (user == null) throw new UnauthorizedAccessException("Utente non autorizzato all'accesso");
        var oldclientSecret = await userSettingsRepository.Get(user.ContactId, "ClientSecret." + appName);
        if (oldclientSecret != clientSecret) throw new UnauthorizedAccessException("Utente non autorizzato all'accesso");
        return await userService.GetUserProfile(uid);
    }

    /// <summary>
    /// Ritorna le proprietà di un documento
    /// </summary>
    /// <param name="documentId">Identificativo del documento</param>
    /// <returns></returns>
    [HttpGet("Get")]
    public async Task<DocumentInfo> Get(string clientSecret, int documentId)
    {
        var u = await GetUser(clientSecret);
        var d = await docRepo.Load(documentId, u);
        return d;
    }

    [HttpGet("GetSetting")]
    public async Task<string> GetSetting(string clientSecret, string Setting)
    {
        var u = await GetUser(clientSecret);
        var user = await userService.GetById(u.userId);
        var s = await userSettingsRepository.Get(user.ContactId, Setting);
        return s;
    }
    [HttpGet("SetSetting")]
    public async Task SetSetting(string clientSecret, string Setting, string Value)
    {
        var u = await GetUser(clientSecret);
        var user = await userService.GetById(u.userId);
        await userSettingsRepository.Set(user.ContactId, Setting, Value);
    }

    [HttpGet("GetContent")]
    public async Task<ActionResult> GetImage(string clientSecret, int imageId)
    {
        var Image = await docRepo.GetContentInfo(imageId);
        if (Image == null) return NotFound();
        var content = await docRepo.GetContent(imageId);
        return new FileContentResult(content, "application/octet-stream") { FileDownloadName = Path.Combine(Image.OriginalFileName) };
    }

    [HttpGet("CheckOut")]
    public async Task<ActionResult> CheckOut(string clientSecret, int imageId)
    {
        var Image = await docRepo.GetContentInfo(imageId);
        if (Image == null) return NotFound();
        var u = await GetUser(clientSecret);
        var content = await docRepo.CheckOut(imageId, u.userId);
        return new FileContentResult(content, "application/octet-stream") { FileDownloadName = Path.Combine(Image.OriginalFileName) };
    }
    [HttpPost("Upload")]
    [RequestSizeLimit(4_000_000_000)]
    public async Task<ActionResult<DocumentImage>> AddContent(string clientSecret, int documentId, FileContent content)
    {

        if (documentId <= 0) return BadRequest("Documento non trovato");
        var u = await GetUser(clientSecret);
        var i = await docRepo.AddContent(documentId, u, content, true);
        if (i != null)
            return Ok(i);
        else
            return BadRequest("Errore durante il salvataggio. Verificare i dati inseriti");
    }

    [HttpPost("CheckIn")]
    [RequestSizeLimit(4_000_000_000)]
    public async Task<ActionResult<DocumentImage>> CheckIn(string clientSecret, int documentId, FileContent content)
    {

        if (documentId <= 0) return BadRequest("Documento non trovato");
        var u = await GetUser(clientSecret);
        var image = await docRepo.AddContent(documentId, u, content, false, true);
        if (image != null)
        {
//            await docRepo.CheckIn(image.Id, u.userId, true);
            return Ok(image);
        }
        else
            return BadRequest("Errore durante il salvataggio. Verificare i dati inseriti");
    }

    [HttpPost("Sign")]
    [RequestSizeLimit(4_000_000_000)]
    public async Task<ActionResult<DocumentImage>> Sign(string clientSecret, int documentId, int imageId, FileContent content)
    {

        var u = await GetUser(clientSecret);
        var doc = await docRepo.Load(documentId, u);
        if (doc.Image.Id != imageId)
        {
            await docRepo.UpdateSignatureStatus(imageId, JobStatus.Aborted, u, DateTime.UtcNow.ToString());
            return BadRequest("Il documento è stato sostituto da una versione più aggiornata");
        }

        var i = await docRepo.AddContent(documentId, u, content, false);
        if (i != null && i.Id != imageId)
        {
            //await docRepo.UpdateSignatureStatus(imageId, JobStatus.Queued, u, "");
            await docRepo.UpdateSignatureStatus(i.Id, JobStatus.Completed, u, DateTime.UtcNow.ToString());
            return Ok(i);
        }
        else
            return BadRequest("Errore durante il salvataggio. Verificare i dati inseriti");
    }




}