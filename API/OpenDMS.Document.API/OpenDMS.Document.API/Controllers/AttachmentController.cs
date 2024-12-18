using Microsoft.AspNetCore.Mvc;
using OpenDMS.Domain.Models;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using OpenDMS.Core.Interfaces;

namespace OpenDMS.DocumentManager.Controllers;

[Authorize]
[ApiController]
[Route("api/document/[controller]")]
public class AttachmentController : ControllerBase
{

    private readonly IDocumentService docRepo;
    private readonly ILoggedUserProfile userContext;

    public AttachmentController(
        IDocumentService docRepo,
        ILoggedUserProfile userContext
        )
    {
        this.docRepo = docRepo;
        this.userContext = userContext;
    }



    //RELAZIONI
    /// <summary>
    /// Ritorna l'elenco degli allegati di un documento
    /// </summary>
    /// <param name="documentId">Identificativo univoco del documento</param>
    /// <returns></returns>
    [HttpGet("{documentId:int}")]
    public async Task<ActionResult<List<DocumentLink>>> GetAttachments(int documentId)
    {
        if (documentId <= 0) return BadRequest("Documento non trovato");
        var u = userContext.Get();
        return await docRepo.Links(documentId, u, true);
    }

    /// <summary>
    /// Aggiunge un documento preesistente come allegato di un altro
    /// </summary>
    /// <param name="documentId">Identificativo del documento principale</param>
    /// <param name="attachmentId">Identificativo del documento da allegare</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<bool>> AddAttachment(int documentId, int attachmentId)
    {
        if (documentId <= 0) return BadRequest("Documento non trovato");
        var u = userContext.Get();
        return await docRepo.AddLink(documentId, attachmentId, u, true);
    }

    /// <summary>
    /// Rimuove un allegato da un documento 
    /// L'allegato non viene cancellato ma semplicemente scollegato dal documento principale
    /// </summary>
    /// <param name="documentId">Identificativo del documento principale</param>
    /// <param name="attachmentId">Identificativo dell'allegato da rimuovere</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<ActionResult<bool>> RemoveAttachment(int documentId, int attachmentId)
    {
        if (documentId <= 0) return BadRequest("Documento non trovato");
        var u = userContext.Get();
        return await docRepo.RemoveLink(documentId, attachmentId, u, true);
    }









}