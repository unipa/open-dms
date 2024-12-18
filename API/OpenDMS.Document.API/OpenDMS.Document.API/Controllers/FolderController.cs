using Microsoft.AspNetCore.Mvc;
using OpenDMS.Domain.Enumerators;
using Microsoft.AspNetCore.Authorization;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Constants;
using System.Text.Json;
using OpenDMS.Domain.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using DocumentFormat.OpenXml.Office2010.Word;

namespace OpenDMS.DocumentManager.Controllers;

//[Authorize]
[ApiController]
[Route("api/document/[controller]")]
public class FolderController : ControllerBase
{

    private readonly IDocumentService docRepo;
    private readonly ILoggedUserProfile userContext;

    public FolderController(
        IDocumentService docRepo,
        ILoggedUserProfile userContext
        )
    {
        this.docRepo = docRepo;
        this.userContext = userContext;
    }

    /// <summary>
    /// Ritorna le proprietà di un fascicolo
    /// </summary>
    /// <param name="folderId"></param>
    /// <returns></returns>
    [HttpGet("{folderId}")]
    public async Task<ActionResult<DocumentInfo>> Get(int folderId)
    {
        var u = userContext.Get();
        var d = await docRepo.Load(folderId, u);
        if (d.ContentType != ContentType.Folder) return NotFound();
        return Ok(d);
    }


    /// <summary>
    /// Aggiunge uno o più documenti esistenti ad un fascicolo
    /// </summary>
    /// <param name="folderId">Identificativo del fascicolo</param>
    /// <param name="documents">elenco di identificativi dei documenti da aggiungere</param>
    /// <param name="moveToFolder">true=sposta i documenti su questo fascicolo</param>
    /// <returns>elenco di errori riscontrati</returns>
    [HttpPost]
    public async Task<ActionResult<List<BatchErrorResult>>> AddToFolder(int folderId, List<int> documents, bool moveToFolder = false)
    {
        var u = userContext.Get();
        return await docRepo.AddToFolder(folderId, documents, u, moveToFolder);
    }

    /// <summary>
    /// Rimuove uno o più documenti esistenti ad un fascicolo
    /// </summary>
    /// <param name="folderId">Identificativo del fascicolo</param>
    /// <param name="documents">elenco di identificativi dei documenti da rimuovere</param>
    /// <returns>elenco di errori riscontrati</returns>
    [HttpDelete]
    public async Task<ActionResult<List<BatchErrorResult>>> RemoveFromFolder(int folderId, List<int> documents)
    {
        var u = userContext.Get();
        return await docRepo.RemoveFromFolder(folderId, documents, u);
    }

    /// <summary>
    /// Ritorna l'elenco degli identificatori dei documenti presenti all'interno di un fascicolo
    /// </summary>
    /// <param name="folderId">Identificativo del fascicolo</param>
    /// <returns>elenco di id di documenti</returns>
    [HttpGet("documents/{folderId}")]
    public async Task<ActionResult<List<int>>> GetFolderDocuments(int folderId)
    {
        var u = userContext.Get();
        var d = await docRepo.GetFolderDocuments(folderId, u);
        return d;
    }

    /// <summary>
    /// Ritorna l'elenco dei fascicoli in cui è contenuto un documento/fascicolo
    /// </summary>
    /// <param name="documentId">Identificativo del documento/fascicolo</param>
    /// <returns>elenco di identificativi di fascicoli</returns>
    [HttpGet("parents/{documentId}")]
    public async Task<ActionResult<List<int>>> GetDocumentFolders(int documentId)
    {
        var u = userContext.Get();
        var d = await docRepo.GetDocumentFolders(documentId, u);
        return d;
    }

    [HttpGet("Close/{folderId}")]
    public async Task<ActionResult> Close(int folderId)
    {
        var u = userContext.Get();
        var d = await docRepo.Load(folderId, u);
        if (d.ContentType != ContentType.Folder) return NotFound();
        var P = await docRepo.GetPermission(folderId, u, PermissionType.CanEdit);
        if (P == null || P.Authorization != AuthorizationType.Granted) return BadRequest("Non sei autorizzato a modificare lo stato del documento");

        await docRepo.ChangeStatus(folderId, u, DocumentStatus.Stored);
        return Ok();
    }

    [HttpGet("Open/{folderId}")]
    public async Task<ActionResult> Open(int folderId)
    {
        var u = userContext.Get();
        var d = await docRepo.Load(folderId, u);
        if (d.ContentType != ContentType.Folder) return NotFound();
        var P = await docRepo.GetPermission(folderId, u, PermissionType.CanEdit);
        if (P == null || P.Authorization != AuthorizationType.Granted) return BadRequest("Non sei autorizzato a modificare lo stato del documento");
        await docRepo.ChangeStatus(folderId, u, DocumentStatus.Active);
        return Ok();
    }

    /// <summary>
    /// Ritorna un file JSON in cui sono contenute le informazioni di un fascilo e il suo contenuto
    /// </summary>
    /// <param name="folderId">Identificativo del fascicolo</param>
    /// <returns>Ritorna un file JSON in cui sono contenute le informazioni di un fascilo e il suo contenuto</returns>
    [HttpGet("{folderId}/export")]
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
    /// Ritorna un file JSON in cui sono contenute le informazioni di un fascilo e il suo contenuto
    /// </summary>
    /// <param name="rootFolderId">Identificativo del fascicolo</param>
    /// <returns>Ritorna un file JSON in cui sono contenute le informazioni di un fascilo e il suo contenuto</returns>
    [HttpPost("{rootFolderId}/import")]
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