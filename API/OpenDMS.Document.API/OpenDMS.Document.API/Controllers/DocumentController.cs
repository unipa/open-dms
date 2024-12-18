using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.DocumentManager.API.DTOs;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;

namespace OpenDMS.DocumentManager.Controllers;

[Authorize]
[ApiController]
[Route("api/document/[controller]")]
public class DocumentController : ControllerBase
{

    private readonly IDocumentService docRepo;
    private readonly ILoggedUserProfile userContext;

    public DocumentController(
        IDocumentService docRepo,
        ILoggedUserProfile userContext
        )
    {
        this.docRepo = docRepo;
        this.userContext = userContext;
    }

    /// <summary>
    /// Ritorna le proprietà di un documento
    /// </summary>
    /// <param name="documentId">Identificativo del documento</param>
    /// <returns></returns>
    [HttpGet("{documentId}")]
    public async Task<DocumentInfo> Get(int documentId)
    {
        var u = userContext.Get();
        var d = await docRepo.Load(documentId, u);
        return d;
    }


    /// <summary>
    /// Crea un nuovo documento/fascicolo
    /// </summary>
    /// <param name="document">Proprietà del documento da creare</param>
    /// <returns>Le proprietà del documento appena creato</returns>
    [HttpPost("CreateAndRead")]
    [RequestSizeLimit(4_000_000_000)]
    public async Task<ActionResult<DocumentInfo>> CreateAndRead(CreateOrUpdateDocument document)
    {
        try
        {
            var u = userContext.Get();
            return await docRepo.CreateAndRead(document, u);
        }
        catch (InvalidDataException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return Forbid(ex.Message);
        }
    }


    /// <summary>
    /// Crea un nuovo documento/fascicolo
    /// </summary>
    /// <param name="document">Proprietà del documento da creare</param>
    /// <returns>Identificativo del documento appena creato</returns>
    [HttpPost]
    [RequestSizeLimit(4_000_000_000)]
    public async Task<ActionResult<int>> Create(CreateOrUpdateDocument document)
    {
        try
        {
            var u = userContext.Get();
            return await docRepo.Create(document, u);
        }
        catch (InvalidDataException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return Forbid(ex.Message);
        }
    }




    /// <summary>
    /// Ritorna un oggetto documento/fascicolo in base ad una tipologia
    /// </summary>
    /// <param name="documentTypeId">Identificativo della tipologia di documento da creare</param>
    /// <param name="contentType">Tipo di contenuto della tipologia di documento da creare</param>
    /// <returns>Le proprietà del documento appena creato</returns>
    [HttpGet("DocumentSchema/{documentTypeId}")]
    public async Task<ActionResult<DocumentInfo>> DocumentSchema(string documentTypeId, ContentType contentType)
    {
        var u = userContext.Get();
        return await docRepo.DocumentSchema(documentTypeId,  u, contentType);
    }




    /// <summary>
    /// Cambia la tipologia del documento/fascicolo indicato dal parametro documentId
    /// </summary>
    /// <param name="documentId">Identificativo del documento da modificare</param>
    /// <param name="documentTypeId">Identificativo della tipologia da assegnare al documento</param>
    /// <returns>Le proprietà del documento appena modificato</returns>
    [HttpPut("{documentId}/{documentTypeId}")]
    public async Task<ActionResult<DocumentInfo>> ChangeType(int documentId, string documentTypeId)
    {
        var u = userContext.Get();
        return await docRepo.ChangeType(documentId, documentTypeId, u);
    }

    /// <summary>
    /// Cambia le proprietà del documento/fascicolo indicato dal parametro documentId
    /// </summary>
    /// <param name="documentId">Identificativo del documento da modificare</param>
    /// <param name="document">proprietà del documento da cambiare</param>
    /// <returns>Le proprietà del documento appena modificato</returns>
    [HttpPut("{documentId}")]
    [RequestSizeLimit(4_000_000_000)]

    public async Task<ActionResult<DocumentInfo>> Update(int documentId, CreateOrUpdateDocument document)
    {
        if (documentId <= 0) return BadRequest();
        var u = userContext.Get();
        var d = await docRepo.Update(documentId, document, u);
        if (d == null) return NotFound(); else return Ok(d);
    }

    /// <summary>
    /// Elimina (o segna come eliminato) un documento
    /// </summary>
    /// <param name="documentId"></param>
    /// <param name="Reason"></param>
    /// <returns></returns>
    [HttpDelete("{documentId}/{Reason}")]
    public async Task<ActionResult> Delete(int documentId, string Reason)
    {
        if (documentId <= 0) return BadRequest();
        var u = userContext.Get();
        await docRepo.Delete(documentId, u, Reason);
        return Ok();
    }

    /// <summary>
    /// Recupera un documento cancellato in modo logico
    /// </summary>
    /// <param name="documentId"></param>
    /// <returns></returns>
    [HttpPut("Restore/{documentId}")]
    public async Task<ActionResult> Restore(int documentId)
    {
        if (documentId <= 0) return BadRequest();
        var u = userContext.Get();
        await docRepo.UnDelete(documentId, u);
        return Ok();
    }


    /// <summary>
    /// Condivide un documento con uno o più profili
    /// </summary>
    /// <param name="documentId">Identificativo del documento da condividere</param>
    /// <param name="shareInformation">Oggetto che descrive i destinatari della condivisione</param>
    /// <returns>true = ok</returns>
    [HttpPost("Share")]
    public async Task<ActionResult<bool>> Share(int documentId, ShareInformation shareInformation)
    {
        if (documentId <= 0) return BadRequest();
        var u = userContext.Get();
        List<ProfileInfo> Users = shareInformation.To.Select(t => new ProfileInfo() { ProfileType = (ProfileType)(int.Parse(t.Substring(0, 1))), ProfileId = t.Substring(1) }).ToList();
        List<ProfileInfo> UsersCC = shareInformation.Cc.Select(t => new ProfileInfo() { ProfileType = (ProfileType)(int.Parse(t.Substring(0, 1))), ProfileId = t.Substring(1) }).ToList();
        return await docRepo.Share(new[] { documentId }, u, Users, UsersCC, shareInformation.RequestType, shareInformation.AssignToAllUsers, shareInformation.Message) ? Ok() : BadRequest();
    }





    /// <summary>
    /// Restituisce i dati relativi a un permesso su un documento tramite Id documento,UserId,UserType e Tipo di permesso. 
    /// </summary>
    /// <param name="documentId">Id del documento.</param>
    /// <param name="profile">Id profilo nella forma <profileType><profileId></param>
    /// <param name="permissionId"> Tipo di permesso. </param>
    /// <returns>Restituisce un oggetto Permission</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Permission))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("Permissions/{documentId}/{profile}/{permission}")]
    public async Task<ActionResult<Permission>> GetByPermission(int documentId, string profile, string permissionId)
    {
        if (documentId <= 0) return BadRequest();
        ProfileType ProfileType = (ProfileType)int.Parse(profile.Substring(0,1));
        string ProfileId = profile.Substring(1);
        try
        {
            return await docRepo.GetProfilePermission(documentId,ProfileType, ProfileId, permissionId);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary>
    /// Restituisce i dati relativi ai permessi su un documento tramite Id documento,UserId e UserType. 
    /// </summary>
    /// <param name="documentId">Id del documento.</param>
    /// <param name="profile">Id utente.</param>
    /// <returns>Restituisce un oggetto Permission</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(ProfilePermissions))]
    [HttpGet("Permissions/{documentId}/{profile}")]
    public async Task<ActionResult<ProfilePermissions>> GetByProfileId(int documentId, string profile)
    {
        if (documentId <= 0) return BadRequest();
        ProfileType ProfileType = (ProfileType)int.Parse(profile.Substring(0,1));
        string ProfileId = profile.Substring(1);
        try
        {
            ProfilePermissions P = await docRepo.GetProfilePermissions(documentId, ProfileType, ProfileId);
            return Ok(P);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }


    /// <summary>
    /// Restituisce l'Id del documento trovato, oppure 0 
    /// </summary>
    /// <param name="folderId">Id del fascicolo padre.</param>
    /// <param name="contentType">Tipo di contenuto da cercare (1=documento, 2=fascicolo)</param>
    /// <param name="externalId">Identificativo univoco del documento da cercare</param>
    /// <returns>Restituisce l'Identificativo del documento trovato</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<DocumentInfo>))]
    [HttpGet("FindInFolderByUniqueId/{folderId}/{contentType}/{externalId}")]
    public async Task<ActionResult<List<DocumentInfo>>> FindInFolderByUniqueId(int folderId, ContentType contentType, string externalId)
    {
        //TODO: Passare DTO con nome permesso
        try
        {
            return Ok(await docRepo.FindInFolderByUniqueId (folderId, externalId, contentType));
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }


    /// <summary>
    /// Restituisce l'Id del documento trovato, oppure 0 
    /// </summary>
    /// <param name="documentType">Id del tipo documento da cercare</param>
    /// <param name="contentType">Tipo di contenuto da cercare (1=documento, 2=fascicolo)</param>
    /// <param name="externalId">Identificativo univoco del documento da cercare</param>
    /// <returns>Restituisce l'Identificativo del documento trovato</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<DocumentInfo>))]
    [HttpGet("FindByUniqueId/{contentType}/{externalId}/{documentType}")]
    public async Task<ActionResult<List<DocumentInfo>>> FindByUniqueId(ContentType contentType, string externalId, string documentType)
    {
        //TODO: Passare DTO con nome permesso
        try
        {
            return Ok(await docRepo.FindByUniqueId (documentType, externalId, contentType));
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }


    /// <summary>
    /// Restituisce i dati relativi ai permessi su un documento tramite Id documento. 
    /// </summary>
    /// <param name="documentId">Id del documento.</param>
    /// <returns>Restituisce una lista di oggetti DocumentPermissions</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<DocumentPermission>))]
    [HttpGet("Permissions/{documentId}")]
    public async Task<ActionResult<List<ProfilePermissions>>> GetByDocument(int documentId)
    {
        //TODO: Passare DTO con nome permesso
        try
        {
            return Ok( await docRepo.GetDocumentPermissions(documentId));
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary> 
    /// Salva un nuovo permesso per un documento.
    /// </summary>
    /// <param name="documentId">Id del documento.</param>
    /// <param name="profile">Id profilo nella forma <profileType><profileId></param>
    /// <param name="permissionId"> Tipo di permesso. </param>
    /// <param name="authorization"> Tipo di autorizzazione </param>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(DocumentPermission))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost("Permissions")]
    public async Task<ActionResult> Set(int documentId, string profile, string permissionId, AuthorizationType authorization)
    {
        if (documentId <= 0) return BadRequest();
        ProfileType ProfileType = (ProfileType)int.Parse(profile.Substring(0, 1));
        string ProfileId = profile.Substring(1);
        var u = userContext.Get();
        var P = await docRepo.GetPermission(documentId, u, permissionId);
        if (P == null || P.Authorization != AuthorizationType.Granted) return BadRequest("Non sei autorizzato a impostare questo permesso");
        try
        {
            await docRepo.SetPermission(documentId, u, ProfileId, ProfileType, permissionId, authorization);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }




    /// <summary>
    /// Chiude un documento/fascicolo
    /// </summary>
    /// <param name="documentId">Identificativo del documento da chiudere</param>
    /// <returns></returns>
    [HttpGet("Close/{folderId}")]
    public async Task<ActionResult> Close(int documentId)
    {
        var u = userContext.Get();
        var P = await docRepo.GetPermission(documentId, u, PermissionType.CanEdit);
        if (P == null || P.Authorization != AuthorizationType.Granted) return BadRequest("Non sei autorizzato a modificare lo stato del documento");
        await docRepo.ChangeStatus(documentId, u, DocumentStatus.Stored);
        return Ok();
    }

    /// <summary>
    /// Riapre un documento/fascicolo precedentemente chiuso
    /// </summary>
    /// <param name="documentId"></param>
    /// <returns></returns>
    [HttpGet("Open/{folderId}")]
    public async Task<ActionResult> Open(int documentId)
    {
        var u = userContext.Get();
        var P = await docRepo.GetPermission(documentId, u, PermissionType.CanEdit);
        if (P == null || P.Authorization != AuthorizationType.Granted) return BadRequest("Non sei autorizzato a modificare lo stato del documento");
        await docRepo.ChangeStatus(documentId, u, DocumentStatus.Active);
        return Ok();
    }

}