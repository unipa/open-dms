using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.DocumentManager.API.DTOs;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using System.Text;

namespace OpenDMS.DocumentManager.Controllers;

[Authorize]
[ApiController]
[Route("api/document/[controller]")]
public class ContentController : ControllerBase
{

    private readonly IDocumentService docRepo;
    private readonly ILoggedUserProfile userContext;

    public ContentController(
        IDocumentService docRepo,
        ILoggedUserProfile userContext
        )
    {
        this.docRepo = docRepo;
        this.userContext = userContext;
    }

    /// <summary>
    /// Aggiunge un nuovo contenuto (file) ad un documento, creando una nuova Versione
    /// </summary>
    /// <param name="documentId">Identificativo univoco del documento</param>
    /// <param name="content">Oggetto che contiene il nome file, la dimensione e il contenuto del file in base64</param>
    /// <returns>Oggetto DocumentImage</returns>
    [HttpPost("{documentId:int}")]
    [RequestSizeLimit(4_000_000_000)]
    public async Task<ActionResult<DocumentImage>> AddContent(int documentId, FileContent content)
    {

        if (documentId <= 0) return BadRequest("Documento non trovato");
        var u = userContext.Get();
        //var u = new UserProfile() { userId = "admin" };
        var i = await docRepo.AddContent(documentId, u, content, true);
        if (i != null)
            return Ok(i);
        else
            return BadRequest("Errore durante il salvataggio. Verificare i dati inseriti");
    }

    /// <summary>
    /// Aggiunge un nuovo contenuto (file) ad un documento, creando una Revisione della Versione corrente
    /// </summary>
    /// <param name="documentId">Identificativo univoco del documento</param>
    /// <param name="content">Oggetto che contiene il nome file, la dimensione e il contenuto del file in base64</param>
    /// <returns>Oggetto DocumentImage</returns>
    [HttpPut("{documentId:int}")]
    [RequestSizeLimit(4_000_000_000)]
    public async Task<ActionResult<DocumentImage>> NewRevision(int documentId, FileContent content)
    {
        if (documentId <= 0) return BadRequest("Documento non trovato");
        var u = userContext.Get();

        var i = await docRepo.AddContent(documentId, u, content, false);
        if (i != null)
            return Ok(i);
        else
            return BadRequest("Errore durante il salvataggio. Verificare i dati inseriti");
    }

    /// <summary>
    /// Rimuove l'ultima revisione del contenuto di un documento
    /// </summary>
    /// <param name="documentId">Identificativo univoco del documento</param>
    /// <returns>true = ok</returns>
    [HttpDelete("{documentId:int}")]
    public async Task<ActionResult<bool>> DeleteContent(int documentId)
    {
        if (documentId <= 0) return BadRequest("Documento non trovato");
        var u = userContext.Get();

        var i = await docRepo.RemoveContent(documentId, u);
        return Ok(i);
    }


    /// <summary>
    /// Ritorna il file corrente associato ad un documento
    /// </summary>
    /// <param name="documentId">Identificativo univoco del documento</param>
    /// <returns>File in streaming</returns>
    [HttpGet("download/{documentId:int}")]
    public async Task<ActionResult> Get(int documentId)
    {
        var u = userContext.Get();
        var d = await docRepo.Load(documentId, u);
        if (d == null || d.Image == null) return NotFound(); ;

        var content = await docRepo.GetContent(d.Image.Id);
        return new FileContentResult(content, "application/octet-stream") { FileDownloadName = Path.Combine(d.Image.OriginalFileName) };
    }


    /// <summary>
    /// Ritorna una versione/revisione specifica di file associato ad un documento
    /// </summary>
    /// <param name="documentId">Identificativo univoco del documento</param>
    /// <param name="versionNumber">Numero di Versione del contenuto</param>
    /// <param name="revisionNumber">Numero di Revisione del contenuto</param>
    /// <returns>File in streaming</returns>
    [HttpGet("download/{documentId:int}/{versionNumber:int}/{revisionNumber:int}")]
    public async Task<ActionResult> GetImage(int documentId, int versionNumber, int revisionNumber)
    {
        var u = userContext.Get();
        var d = await docRepo.Images(documentId, u);
        if (d == null) return NotFound();
        d = d.Where(i => i.VersionNumber == versionNumber && i.RevisionNumber == revisionNumber).ToList();
        if (d == null) return NotFound();
        var content = await docRepo.GetContent(d[0].Id);
        return new FileContentResult(content, "application/octet-stream") { FileDownloadName = Path.Combine(d[0].FileName) };
    }

    /// <summary>
    /// Ritorna l'elenco delle versioni e revisioni di un documento
    /// </summary>
    /// <param name="documentId">Identificativo univoco del documento</param>
    /// <returns>Elenco di oggetto DocumentVersion</returns>
    [HttpGet("{documentId:int}")]
    public async Task<ActionResult<List<DocumentVersion>>> GetAll(int documentId)
    {
        if (documentId <= 0) return BadRequest("Documento non trovato");
        var u = userContext.Get();

        var i = await docRepo.Images(documentId, u);
        return Ok(i);
    }


    /// <summary>
    /// Restituisce un diagramma di workflow tramite Id
    /// </summary>
    /// <param name="documentId"> Id del diagramma workflow </param>
    /// <returns>Restituisce l'oggetto diagram in formato BPMN</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{documentId}/CheckOut")]
    public async Task<ActionResult<CheckOutContent>> CheckOut(int documentId)
    {
        try
        {
            CheckOutContent result = new CheckOutContent();
            var u = userContext.Get();
            if (documentId <= 0) return BadRequest(nameof(documentId) + " non può essere vuoto");
            var document = await docRepo.Load(documentId, u);
            if (document != null)
            {
                if (document.Image != null)
                {
                    result.Content = Encoding.UTF8.GetString(await docRepo.GetContent(document.Image.Id));
                    result.VersionNumber = document.Image.VersionNumber;
                    result.RevisionNumber = document.Image.RevisionNumber;
                    result.CreationDate = document.Image.CreationDate;
                    result.LastUpdateUser = document.Image.Owner;
                    result.FileName = document.Image.OriginalFileName;
                    result.Extension = Path.GetExtension(document.Image.OriginalFileName);
                    result.Published = document.Image.SendingStatus == JobStatus.Completed;
                }
                return Ok(result);
            }
            else return NotFound();
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary>
    /// Aggiorna il contenuto di un documento
    /// </summary>
    /// <param name="documentId"> Id del documento da aggiornare</param>
    /// <param name="content"> contenuto da aggiornare</param>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("{documentId}/CheckIn")]
    [RequestSizeLimit(2_000_000_000)]
    public async Task<ActionResult> CheckIn(int documentId, CheckInContent content)
    {
        try
        {
            if (content == null) return BadRequest("Parametro nullo o mal formattato");
            var u = userContext.Get();
            var document = await docRepo.Load(documentId, u);
            if (document != null)
            {
                if (document.Image != null)
                {
                    FileContent FC = new();
                    FC.FileData = content.Content;
                    FC.DataIsInBase64 = false;
                    FC.FileName = document.Image.FileName;
                    var documentImage = await docRepo.AddContent(documentId, u, FC, false); //!content.ToBePublished
                    if (documentImage != null)
                    {
                        //if (!String.IsNullOrEmpty(content.PreviewInBase64))
                        //{
                        //    FileContent FCP = new();
                        //    FCP.FileData = content.PreviewInBase64;
                        //    FCP.DataIsInBase64 = true;
                        //    if (!content.PreviewExtension.StartsWith(".")) content.PreviewExtension = "." + content.PreviewExtension;
                        //    FCP.FileName = documentImage.FileName + content.PreviewExtension.ToLower();
                        //    //TODO: Capire cosa fare se ritorna errore
                        //    await docRepo.AddPreview(documentId, documentImage.Id, u, FC);
                        //}
                    }
                    return documentImage != null ? Ok() : BadRequest();
                }
                else return NotFound();
            }
            else return NotFound();
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }



}