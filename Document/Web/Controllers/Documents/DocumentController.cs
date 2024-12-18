using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenDMS.core.Mappers;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Exceptions;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using Web.DTOs;

namespace Web.Controllers.Documents;

[Authorize]
[ApiController]
[Route("internalapi/document")]
public class DocumentController : ControllerBase
{

    private readonly IDocumentService  docRepo;
    private readonly IHistoryRepository historyRepository;
    private readonly IUserTaskService userTaskService;
    private readonly IUserService userService;
    private readonly ILookupTableService lookupService;
    private readonly ILoggedUserProfile userContext;

    public DocumentController(
        IDocumentService docRepo,
        IHistoryRepository historyRepository,
        IUserTaskService userTaskService,
        IUserService userService,
        ILookupTableService lookupService,
        ILoggedUserProfile userContext
        )
    {
        this.docRepo = docRepo;
        this.historyRepository = historyRepository;
        this.userTaskService = userTaskService;
        this.userService = userService;
        this.lookupService = lookupService;
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
    [HttpPost]
    public async Task<ActionResult<DocumentInfo>> Create(CreateOrUpdateDocument document)
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
        catch (DuplicateDocumentException ex)
        {
            return Ok (new { Id = -1, Description = ex.Message, MasterDocumentId = ex.DocumentId });
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
    /// <returns>Le proprietà del documento appena creato</returns>
    [HttpGet("DocumentSchema/{documentTypeId}")]
    public async Task<ActionResult<DocumentInfo>> DocumentSchema(string documentTypeId)
    {
        var u = userContext.Get();
        return await docRepo.DocumentSchema(documentTypeId, u);
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
    public async Task<ActionResult<DocumentInfo>> Update(int documentId, CreateOrUpdateDocument document)
    {
        if (documentId <= 0) return BadRequest();

        try
        {
            var u = userContext.Get();
            var d = await docRepo.Update(documentId, document, u);
            // Se il documento proviene da Form, aggiorno le variabili dei processi associati

            if (d == null) return NotFound(); else return Ok(d);
        }
        catch (InvalidDataException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (DuplicateDocumentException ex)
        {
            return Ok(new { Id = -1, Description = ex.Message, MasterDocumentId = ex.DocumentId });
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
            return BadRequest(ex.Message);
        }
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
        try
        {
            await docRepo.Delete(documentId, u, Reason);
            return Ok();

        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
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
    /// <param name="documentIds">Identificativo del o dei documenti da condividere</param>
    /// <param name="shareInformation">Oggetto che descrive i destinatari della condivisione</param>
    /// <returns>true = ok</returns>
    [HttpPost("Share")]
    public async Task<ActionResult<bool>> Share(string documentIds, ShareInformation shareInformation)
    {
        if (string.IsNullOrEmpty(documentIds)) return BadRequest();
        var u = userContext.Get();
        List<ProfileInfo> Users = shareInformation.To.Select(t => new ProfileInfo() { ProfileType = (ProfileType)int.Parse(t.Substring(0, 1)), ProfileId = t.Substring(1) }).ToList();
        List<ProfileInfo> UsersCC = shareInformation.Cc.Select(t => new ProfileInfo() { ProfileType = (ProfileType)int.Parse(t.Substring(0, 1)), ProfileId = t.Substring(1) }).ToList();
        var docarray = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(documentIds);

        //var docarray = documentIds.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        int ok = 0;
        int error = 0;
        //foreach (var id  in docarray)
        //{
        //    if (id > 0)
                if (await docRepo.Share(docarray.ToArray(), u, Users, UsersCC, shareInformation.RequestType, shareInformation.AssignToAllUsers, shareInformation.Title, shareInformation.Message))
                    ok++;
                else
                    error++;
        //}
        return error == 0 ? Ok() : BadRequest();
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
        ProfileType ProfileType = (ProfileType)int.Parse(profile.Substring(0, 1));
        string ProfileId = profile.Substring(1);
        try
        {
            return await docRepo.GetProfilePermission(documentId, ProfileType, ProfileId, permissionId);
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
        ProfileType ProfileType = (ProfileType)int.Parse(profile.Substring(0, 1));
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
            return Ok(await docRepo.FindInFolderByUniqueId(folderId, externalId, contentType));
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
            return Ok(await docRepo.FindByUniqueId(string.IsNullOrEmpty(documentType) ? null : documentType, externalId, contentType));
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
            return Ok((await docRepo.GetDocumentPermissions(documentId)).Where(p => p.ProfileId != SpecialUser.SystemUser).ToList());
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
    /// Salva un nuovo permesso per un documento.
    /// </summary>
    /// <param name="documentId">Id del documento.</param>
    /// <param name="profile">Id profilo nella forma <profileType><profileId></param>
    /// <param name="permissionId"> Tipo di permesso. </param>
    /// <param name="authorization"> Tipo di autorizzazione </param>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(DocumentPermission))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpGet("RemovePermissions")]
    public async Task<ActionResult> RemovePermissions(int documentId, string profile)
    {
        if (documentId <= 0) return BadRequest();
        ProfileType ProfileType = (ProfileType)int.Parse(profile.Substring(0, 1));
        string ProfileId = profile.Substring(1);
        var u = userContext.Get();
        var P = await docRepo.GetPermission(documentId, u, PermissionType.CanAuthorize);
        if (P == null || P.Authorization != AuthorizationType.Granted) return BadRequest("Non sei autorizzato a impostare questo permesso");
        try
        {
            await docRepo.RemovePermissions (documentId, ProfileType, ProfileId);
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



    [HttpGet("Folders/{documentId}")]
    public async Task<IActionResult> GetFolders(int documentId)
    {
        var u = userContext.Get();
        //u = UserProfile.SystemUser();
        var Folders = new List<DocumentInfo>();
        foreach (var f in await docRepo.GetDocumentFolders(documentId, u))
            Folders.Add(await docRepo.Load(f, u));
        return new JsonResult(Folders);
    }


    [HttpGet("History/{documentId}")]
    public async Task<IActionResult> GetHistories(int documentId, int PageIndex, string SearchText, int Events)
    {
        var e = new List<string>();
        switch (Events)
        {
            case 1: // smistamento / condivisione
                e.Add(EventType.Share);
                e.Add(EventType.Print);
                e.Add(EventType.Download);
                e.Add(EventType.Authorize);
                break;
            case 2: // firme 
                e.Add(EventType.AddBiometricalSignature);
                e.Add(EventType.AddDigitalSignature);
                e.Add(EventType.AddUserSignature);
                e.Add(EventType.AddRemoteDigitalSignature);
                break;
            case 3: // spedizioni 
                e.Add(EventType.EMail);
                e.Add(EventType.Publish);
                e.Add(EventType.ExcludeFromSending);
                e.Add(EventType.PrepareForSending);
                break;
            case 4: // modifiche 
                e.Add(EventType.Preservation);
                e.Add(EventType.Protocol);
                e.Add(EventType.Creation);
                e.Add(EventType.Classify);
                e.Add(EventType.Update);
                e.Add(EventType.Delete);
                e.Add(EventType.UnDelete);
                e.Add(EventType.ChangeStatus);
                break;
            case 5: // versioni 
                e.Add(EventType.AddSignatureField);
                e.Add(EventType.AddPreservationSignature);
                e.Add(EventType.AddProtocolSign);
                e.Add(EventType.AddStamp);
                e.Add(EventType.AddTextToContent);
                e.Add(EventType.AddTextToContent);
                e.Add(EventType.AddCheckSign);
                e.Add(EventType.EraseContent);
                e.Add(EventType.HighlightContent);
                e.Add(EventType.ObscureContent);

                e.Add(EventType.CheckIn);
                e.Add(EventType.CheckOut);
                e.Add(EventType.AddVersion);
                e.Add(EventType.AddRevision);
                e.Add(EventType.RemoveRevision);
                e.Add(EventType.RemoveVersion);
                break;
            case 6: // fascicolazioni 
                e.Add(EventType.AttachRemoved);
                e.Add(EventType.AddAttach);
                e.Add(EventType.AddToFolder);
                e.Add(EventType.RemoveFromFolder);
                e.Add(EventType.AddLink);
                e.Add(EventType.LinkRemoved);
                break;
            case 7: // visualizzazione 
                e.Add(EventType.View);
                e.Add(EventType.RunProcess);
                break;
            default:
                break;
        }
        HistoryFilters filters = new HistoryFilters()
        {
            DocumentId = documentId,
            PageSize = 25,
            Search = SearchText,
            PageIndex = PageIndex,
            Events = e
        };
        var entries = await historyRepository.GetByFilters(filters);
        var history = new List<HistoryItem>();
        foreach (var entry in entries)
        {
            HistoryItem historyItem = new HistoryItem();
            historyItem.UserId = entry.UserId;
            historyItem.Sender = await userService.GetName(entry.UserId);
            var l = await lookupService.GetById("$EVENTS$", entry.EventType.ToString());
            historyItem.Description = entry.Description;
            historyItem.EventDateTime = entry.CreationDate.Date == DateTime.Now.Date ? "Oggi" + entry.CreationDate.ToString(" HH:mm") : entry.CreationDate.ToString("dd/MM/yyyy HH:mm");
            historyItem.EventType = string.IsNullOrEmpty(l.Description) ? entry.EventType.Replace("Document.", "") : l.Description;
            historyItem.ProcessName = entry.WorkflowId;
            historyItem.TaskId = entry.TaskId;
            historyItem.DeputyUser = await userService.GetName(entry.DeputyUserId);
            historyItem.To = entry.Recipients.Where(r => !r.CC).Select(r => userService.GetProfileName(r.ProfileId, r.ProfileType).GetAwaiter().GetResult()).ToList();
            historyItem.CC = entry.Recipients.Where(r => r.CC).Select(r => userService.GetProfileName(r.ProfileId, r.ProfileType).GetAwaiter().GetResult()).ToList();
            historyItem.Details = new HistoryDetailFormatter().Format(entry);
            history.Add(historyItem);
        }

        return new JsonResult(history);
    }

    [HttpGet("Attachments/{documentId}")]
    public async Task<IActionResult> GetAttachments(int documentId)
    {
        var u = userContext.Get();
        //u = UserProfile.SystemUser();
        var Attachments = await docRepo.Links(documentId, u, true);
        return new JsonResult(Attachments);
    }

    [HttpGet("AttachmentOf/{documentId}")]
    public async Task<IActionResult> GetAttachmentsOf(int documentId)
    {
        var u = userContext.Get();
        //u = UserProfile.SystemUser();
        var Attachments = await docRepo.LinkedIn(documentId, u, true);
        return new JsonResult(Attachments);
    }

    [HttpGet("Attachments/{documentId}/Remove/{attachmentId}")]
    public async Task<IActionResult> OnGetRemoveAttachment(int documentId, int attachmentId)
    {
        var u = userContext.Get();
        //u = UserProfile.SystemUser();
        if (documentId != attachmentId)
        {
            var errors = await docRepo.RemoveLink(documentId, attachmentId, u, true);
            if (!errors)
            {
                return new JsonResult("Impossibile rimuovere l'allegato");
            }
        }
        return await GetAttachments(documentId);
    }

    [HttpGet("MyTasks/{documentId}")]
    public async Task<IActionResult> OnGetTasks(int documentId)
    {
        var u = userContext.Get();
        List<UserTaskInfo> list = new List<UserTaskInfo>();
        foreach (var i in await userTaskService.GetByDocument(documentId, u))
        {
            list.Add(await userTaskService.GetById(i.Id, u));
        }
        return new JsonResult(list);
    }

    [HttpGet("Tasks/{documentId}")]
    public async Task<IActionResult> GetBPMs(int documentId)
    {
        var u = await userService.GetUserProfile(SpecialUser.SystemUser);
        List<UserTaskInfo> Active = new List<UserTaskInfo>();
        List<UserTaskInfo> Closed = new List<UserTaskInfo>();
        foreach (var i in await userTaskService.GetAllDocumentTasks(documentId, u))
        {
            var ut = await userTaskService.GetById(i.Id, u);
            if (ut != null)
            {
                //if (!string.IsNullOrEmpty(ut.TaskItemInfo.Process.Id))
                if (i.Status != ExecutionStatus.Executed && i.Status != ExecutionStatus.Validated)
                    Active.Add(await userTaskService.GetById(i.Id, u));
                else
                    Closed.Add(await userTaskService.GetById(i.Id, u));
            }
        }

        return new JsonResult(new { active = Active, closed = Closed });
    }

    [HttpGet("Links/{documentId}")]
    public async Task<IActionResult> GetLinks(int documentId)
    {
        var u = userContext.Get();
        //u = UserProfile.SystemUser();
        var Links = await docRepo.Links(documentId, u, false);
        return new JsonResult(Links);
    }
    [HttpGet("Link/{documentId}/Remove/{linkId}")]
    public async Task<IActionResult> GetRemoveLink(int documentId, int linkId)
    {
        var u = userContext.Get();
        //u = UserProfile.SystemUser();
        if (documentId != linkId)
        {
            var errors = await docRepo.RemoveLink(documentId, linkId, u, false);
            if (!errors)
            {
                return new JsonResult("Impossibile rimuovere il collegamento");
            }
        }
        return await GetLinks(documentId);
    }


    [HttpGet("Versions/{documentId}")]
    public async Task<IActionResult> GetVersions(int documentId)
    {
        var u = userContext.Get();
        //u = UserProfile.SystemUser();
        var Versions = await docRepo.Images(documentId, u);
        return new JsonResult(Versions);
    }
    [HttpGet("Versions/{documentId}/Restore/{imageId}")]
    public async Task<IActionResult> GetRestoreVersion(int documentId, int imageId)
    {
        var u = userContext.Get();
        //u = UserProfile.SystemUser();
        var Document = await docRepo.Load(documentId, u);
        if (Document.Image.Id != imageId)
        {
            var contentInfo = await docRepo.GetContentInfo(imageId);
            FileContent content = new FileContent() { FileName = contentInfo.OriginalFileName, FileData = Convert.ToBase64String(await docRepo.GetContent(imageId)), LinkToContent = false };
            await docRepo.AddContent(documentId, u, content, true);
        }
        return await GetLinks(documentId);
    }

    /// <summary>
    /// Ritorna un file JSON in cui sono contenute le informazioni di un fascicolo e il suo contenuto
    /// </summary>
    /// <param name="folderId">Identificativo del fascicolo</param>
    /// <returns>Ritorna un file JSON in cui sono contenute le informazioni di un fascilo e il suo contenuto</returns>
    [HttpGet("folders/{folderId}/export")]
    public async Task<ActionResult> ExportFolder(int folderId)
    {
        var user = new UserProfile();
        user.userId = SpecialUser.SystemUser;

        // Carica il fascicolo principale
        var folder = await docRepo.Load(folderId, user);

        if (folder == null || folder.ContentType != ContentType.Folder)
            return NotFound();

        // Recupera il contenuto del fascicolo in maniera ricorsiva
        var folderWithContent = await docRepo.GetFolderContentRecursive(folder, user);

        // Serializza il fascicolo con il contenuto
        var json = JsonConvert.SerializeObject(folderWithContent, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        var fileBytes = System.Text.Encoding.UTF8.GetBytes(json);

        Response.Headers.Add("Content-Disposition", $"attachment; filename=export_folder_{folderId}.json");

        return File(fileBytes, "application/json");
    }

    /// <summary>
    /// Ritorna OK se l'importazione è andata a buon fine
    /// </summary>
    /// <param name="rootFolderId">Identificativo del fascicolo in cui verranno importate le informazioni del fascilo esportato</param>
    /// <param name="file">File JSON in cui sono contenute le informazioni del fascicolo da importare</param>
    /// <returns>Ritorna OK se l'importazione è andata a buon fine</returns>
    [HttpPost("folders/{rootFolderId}/import")]
    public async Task<ActionResult> ImportFolder(int rootFolderId, IFormFile file)
    {
        var user = new UserProfile();
        user.userId = SpecialUser.SystemUser;

        if (file == null || file.Length == 0)
        {
            return BadRequest("Il file è vuoto o non è stato fornito.");
        }

        if (!file.ContentType.Equals("application/json", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Il formato del file non è JSON.");
        }

        try
        {
            using var reader = new StreamReader(file.OpenReadStream());
            var jsonContent = await reader.ReadToEndAsync();

            // Deserializza il contenuto JSON in un oggetto FolderExportModel
            var folderExportModel = JsonConvert.DeserializeObject<FolderExportModel>(jsonContent);

            if (folderExportModel == null)
            {
                return BadRequest("Il contenuto del file JSON non è valido.");
            }

            // Crea un mapping per tenere traccia degli ID nuovi e quelli vecchi
            var documentIdMap = new Dictionary<int, int>();

            //Creazione dei documenti
            await docRepo.ProcessImportedFolderFirstPass(rootFolderId, folderExportModel, user, documentIdMap);

            //Aggiornamento di link e allegati
            await docRepo.ProcessImportedFolderSecondPass(folderExportModel, user, documentIdMap);

            return Ok("Importazione completata con successo.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Errore durante l'importazione del file: {ex.Message}");
        }
    }




}