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
public class LinkController : ControllerBase
{

    private readonly IDocumentService docRepo;
    private readonly ILoggedUserProfile userContext;

    public LinkController(
        IDocumentService docRepo,
        ILoggedUserProfile userContext
        )
    {
        this.docRepo = docRepo;
        this.userContext = userContext;
    }



    //RELAZIONI
    /// <summary>
    /// Ritorna l'elenco dei links di un documento
    /// </summary>
    /// <param name="documentId">Identificativo univoco del documento</param>
    /// <returns></returns>
    [HttpGet("{documentId:int}")]
    public async Task<ActionResult<List<DocumentLink>>> GetLinks(int documentId)
    {
        if (documentId <= 0) return BadRequest("Documento non trovato");
        var u = userContext.Get();
        return await docRepo.Links(documentId, u, false);
    }

    /// <summary>
    /// Aggiunge un documento preesistente come link di un altro
    /// </summary>
    /// <param name="documentId">Identificativo del documento principale</param>
    /// <param name="attachmentId">Identificativo del documento da linkare</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<bool>> AddLink(int documentId, int attachmentId)
    {
        if (documentId <= 0) return BadRequest("Documento non trovato");
        var u = userContext.Get();
        return await docRepo.AddLink(documentId, attachmentId, u, false);
    }

    /// <summary>
    /// Rimuove un link da un documento 
    /// Il link non viene cancellato ma semplicemente scollegato dal documento principale
    /// </summary>
    /// <param name="documentId">Identificativo del documento principale</param>
    /// <param name="attachmentId">Identificativo del link da rimuovere</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<ActionResult<bool>> RemoveLink(int documentId, int attachmentId)
    {
        if (documentId <= 0) return BadRequest("Documento non trovato");
        var u = userContext.Get();
        return await docRepo.RemoveLink(documentId, attachmentId, u, false);
    }









}