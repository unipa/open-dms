using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Filters;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Services;

namespace Web.Controllers.Documents;

[Authorize]
[ApiController]
[Route("internalapi/[controller]")]
public class DocumentTypeController : ControllerBase
{

    private readonly IDocumentTypeService doctypeService;
    private readonly ILoggedUserProfile userContext;
    private readonly IUserService userService;

    public DocumentTypeController(IDocumentTypeService doctypeService,
        ILoggedUserProfile userContext,
        IUserService userService)
    {
        this.doctypeService = doctypeService;
        this.userContext = userContext;
        this.userService = userService;
    }


    /// <summary>
    /// Restituisce i dati relativi ad un DocumentType tramite Id del tipo documentale. 
    /// </summary>
    /// <param name="Id"> Id del tipo documentale. </param>
    /// <returns>Restituisce un oggetto DocumentType.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(DocumentType))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{Id}")]
    public async Task<ActionResult<DocumentType>> GetById(string Id)
    {
        try
        {
            if (string.IsNullOrEmpty(Id)) return BadRequest(nameof(Id) + " non può essere vuoto");
            var result = await doctypeService.GetById(Id);
            return result == null ? Ok(new DocumentType()) : Ok(result);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }


    /// <summary>
    /// Restituisce le tipologie accessibili ad un utente.
    /// Sono accessibili le tipologie associate alle ACL in cui l'utente � inserito direttamente o tramite un gruppo
    /// </summary>
    /// <returns>Restituisce una lista di oggetti DocumentType.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<DocumentType>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("GetByUser")]
    public async Task<ActionResult<List<DocumentType>>> GetByUser(string userId, string permissionId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId)) userId = User.Identity.Name;
            var userInfo = await userService.GetUserProfile(userId);
            var result = await doctypeService.GetByPermission(userInfo, permissionId);
            return result == null ? NotFound("Non sono state trovate tipologie accessibili a queste ACLs") : Ok(result);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }


    ///// <summary>
    ///// Restituisce le tipologie creabili ad un utente.
    ///// </summary>
    ///// <param name="bd"> Utente/ACL/Ruolo/Gruppo di cui si vuole ottenere le tipologie creabili. </param>
    ///// <returns>Restituisce una lista di oggetti DocumentType.</returns>
    //[ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<DocumentType>))]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[HttpPost("GetCreatableByUser")]
    //public async Task<ActionResult<List<DocumentType>>> GetCreatableByUser(UserACLs bd)
    //{
    //    try
    //    {

    //        var result = await docTypeRepo.GetCreatableByUser(bd);
    //        return result == null ? NotFound("Non sono state trovate tipologie creabili a queste ACLs") : Ok(result);
    //    }
    //    catch (Exception ex) { return BadRequest(ex.Message); }
    //}

    /// <summary>
    /// Restituisce la lista delle tipologie documentali. 
    /// </summary>
    /// <returns>Restituisce una lista di oggetti DocumentType.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<DocumentType>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    public async Task<ActionResult<List<DocumentType>>> GetAll()
    {
        {
            try
            {
                var userInfo = userContext.Get();
                var result = await doctypeService.GetByPermission(userInfo, PermissionType.CanView);
                return result == null ? NotFound("Non sono state trovate tipologie accessibili a queste ACLs") : Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }

    /// <summary>
    /// Restituisce la lista delle tipologie documentali sotto forma di oggetto LookupTable in base al content type. 
    /// </summary>
    /// <param name="ct">Content Type ( 68 = Document, 70=Folder, 87=Workflow, 72=Form, 81=Query, 82=Report, 0=Any) </param>
    /// <returns>Restituisce una lista di oggetti DocumentType_GetDTO.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<DocumentType>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("GetTypes/{ct}")]
    public async Task<ActionResult<List<LookupTable>>> GetTypes(ContentType ct)
    {
        try
        {
            var userInfo = userContext.Get();
            var types = await doctypeService.GetByPermission(userInfo, PermissionType.CanView);
            if (ct != ContentType.Any)
                types = types.Where(dt => dt.ContentType == ct).ToList();
            List<LookupTable> result = new List<LookupTable>();
            foreach (var type in types)
            {
                if (string.IsNullOrEmpty(type.Icon)) {
                    type.Icon =
                        type.ContentType == ContentType.Document ? "fa fa-image" :
                        type.ContentType == ContentType.Folder? "fa fa-folder" :
                        type.ContentType == ContentType.Template ? "fa fa-edit" :
                        type.ContentType == ContentType.Workflow ? "fa fa-cogs" :
                        type.ContentType == ContentType.Form ? "fa fa-table" :
                        type.ContentType == ContentType.DMN ? "fa fa-question-circle" :
                        "";
                }
                result.Add(new LookupTable { Id = type.Id, Description = type.Name, Annotation = type.Icon, TableId = type.CategoryId });
            }

            return Ok(result);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary> 
    ///Salva una nuova tipologia documentale.
    /// </summary>
    /// <param name="documentType">Oggetto DocumentType(tipologia documentale) da creare</param>
    /// <returns>Restituisce l'oggetto DocumentType creato.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(DocumentType))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    public async Task<ActionResult<DocumentType>> Insert(DocumentType documentType)
    {
        try
        {
            var exist = await doctypeService.GetById(documentType.Id);
            if (string.IsNullOrEmpty(exist.Id))
            {
                int res = await doctypeService.Create(documentType);
                return res > 0 ? Ok(await doctypeService.GetById(documentType.Id)) : BadRequest("L'inserimento non � andato a buon fine.");
            }
            else return Conflict("Non pu� essere inserita una tipologia documentale con un Id gi� esistente, usa un altro Id oppure se vuoi modificarla usa il metodo PUT.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message + "; Verifica che l'ACLid che stai inserendo esiste.");
        }
    }

    /// <summary>
    /// Modifica una specifica tipologia documentale.
    /// </summary>
    /// <param name="documentType">Oggetto DocumentType(tipologia documentale) da modificare </param>
    /// <returns>Restituisce 1 se la modifica � andata a buon fine.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut]
    public async Task<ActionResult<int>> Update( DocumentType documentType)
    {
        try
        {
            var exist = await doctypeService.GetById(documentType.Id);
            if (exist != null)
            {
                //exist.ContentType = bd.ContentType;
                //exist.Name = bd.Name;
                //exist.GroupName = bd.GroupName;
                //exist.Owner = bd.Owner;
                //exist.Annotation = bd.Annotation;
                //exist.Icon = bd.Icon;
                //exist.IconColor = bd.IconColor;
                //exist.Draft = bd.Draft;
                //exist.Internal = bd.Internal;
                //exist.BatchScan = bd.BatchScan;
                //exist.ToBeSigned = bd.ToBeSigned;
                //exist.ToBePreserved = bd.ToBePreserved;
                //exist.ToBeIndexed = bd.ToBeIndexed;
                ///* ACQUISIZIONE */
                //exist.ArchivingStrategy = bd.ArchivingStrategy;
                //exist.TemplateId = bd.TemplateId;
                //exist.MaxVersions = bd.MaxVersions;
                //exist.FileNamingTemplate = bd.FileNamingTemplate;
                //exist.ConvertToPDFA = bd.ConvertToPDFA;
                ///* SICUREZZA */
                //exist.ACLId = bd.ACLId;
                //exist.FileManager = bd.FileManager;
                //exist.PersonalData = bd.PersonalData;
                //exist.Reserved = bd.Reserved;
                ///* DATI PROTOCOLLO */
                //exist.ProtocolDirection = bd.ProtocolDirection;
                //exist.ProtocolRegister = bd.ProtocolRegister;
                //exist.LabelPosition = bd.LabelPosition;
                //exist.LabelX = bd.LabelX;
                //exist.LabelY = bd.LabelY;
                ///* SCADENZA */
                //exist.ExpirationStrategy = bd.ExpirationStrategy;
                //exist.ExpirationOwner = bd.ExpirationOwner;
                //exist.ExpirationDays = bd.ExpirationDays;
                //exist.ExpirationTolerance = bd.ExpirationTolerance;
                ///* DISTRIBUZIONE */
                //exist.ProtocolChannel = bd.ProtocolChannel;
                //exist.AddressFieldIndex = bd.AddressFieldIndex;
                //exist.IncludeAttachments = bd.IncludeAttachments;
                //exist.SendOnlySignedFiles = bd.SendOnlySignedFiles;
                //exist.IncludePreview = bd.IncludePreview;
                ///* METADATI */
                //exist.DescriptionLabel = bd.DescriptionLabel;
                //exist.DocumentDateLabel = bd.DocumentDateLabel;
                //exist.DocumentNumberLabel = bd.DocumentNumberLabel;
                //exist.DocumentNumberDataType = bd.DocumentNumberDataType;

                //exist.Fields = bd.Fields;

                //controllo se esiste l'acl
                int res = await doctypeService.Update(documentType);
                return Ok(res);
            }
            else return NotFound("La tipologia documentale indicata non � stata trovata.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Elimina una tipologia documentale identificata tramite Id.
    /// </summary>
    /// <param name="Id">Id del tipo documentale.</param>
    /// <returns>Ritorna 200 in caso di successo, 404 in caso di NotFound e 400 in caso di fallimento.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{Id}")]
    public async Task<ActionResult<string>> Delete(string Id)
    {
        try
        {
            var exist = await doctypeService.GetById(Id);
            if (exist != null)
            {
                var result = await doctypeService.Delete(exist);
                return result > 0 ? Ok("La tipologia documentale indicata � stata eliminata con successo") : BadRequest("Eliminazione non � andata a buon fine.");
            }
            else return NotFound("La tipologia documentale indicata non � stata trovata");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// Restituisce la lista dei workflow associati alla tipologia documentale indicata. 
    /// </summary>
    /// <param name="documentTypeId"> Id del tipo documentale. </param>
    /// <returns>Restituisce una lista di oggetti DocumentTypeWorkflow.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<DocumentTypeWorkflow>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("Workflow")]
    public async Task<ActionResult<List<DocumentTypeWorkflow>>> GetAllWorkflows(string documentTypeId)
    {
        try
        {
            var result = await doctypeService.GetAllWorkflows(documentTypeId);
            return result.Count() == 0 ? NotFound("Non sono stati trovati workflow associati alla tipologia indicata") : Ok(result);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary>
    /// Restituisce la lista dei workflow di tutte le tipologie documentali. 
    /// </summary>
    /// <returns>Restituisce una lista di oggetti DocumentTypeWorkflow.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<DocumentTypeWorkflow>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("Workflow/GetAll")]
    public async Task<ActionResult<List<DocumentTypeWorkflow>>> GetAllTypesWorkflows()
    {
        try
        {
            var result = await doctypeService.GetAllTypesWorkflows();
            return result.Count() == 0 ? NotFound("Non sono stati trovati workflow") : Ok(result);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary>
    /// Restituisce i dati relativi ad un workflow associato al tipo documentale e all'evento indicati. 
    /// </summary>
    /// <param name="documentTypeId"> Id del tipo documentale. </param>
    /// <param name="eventName"> Nome dell'evento che avvia il processo. </param>
    /// <returns>Restituisce un oggetto DocumentTypeWorkflow.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(DocumentTypeWorkflow))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("Workflow/{documentTypeId}/{eventName}")]
    public async Task<ActionResult<DocumentTypeWorkflow>> GetWorkflow(string documentTypeId, string eventName)
    {
        try
        {
            if (string.IsNullOrEmpty(documentTypeId)) return BadRequest(nameof(documentTypeId) + " non può essere vuoto");
            var result = await doctypeService.GetWorkflow(documentTypeId, eventName);
            return result == null ? Ok(new DocumentTypeWorkflow()) : Ok(result);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary> 
    ///Salva una nuovo workflow associato alla tipologia documentale.
    /// </summary>
    /// <param name="bd">Oggetto DocumentTypeWorkflow da creare</param>
    /// <returns>Restituisce 1 se la creazione è andata a buon fine.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(DocumentTypeWorkflow))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost("Workflow")]
    public async Task<ActionResult<DocumentTypeWorkflow>> AddWorkflow(DocumentTypeWorkflow bd)
    {
        try
        {
            var exist = await doctypeService.GetWorkflow(bd.DocumentTypeId, bd.EventName);
            if (exist == null)
            {
                int res = await doctypeService.AddWorkflow(bd);
                return res > 0 ? Ok(await doctypeService.GetWorkflow(bd.DocumentTypeId, bd.EventName)) : BadRequest("L'inserimento non è andato a buon fine.");
            }
            else return Conflict("Non può essere inserito un nuovo workflow associato tipologia documentale e all'evento indicato.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Modifica uno specifico workflow associato alla tipologia documentale e all'evento indicato.
    /// </summary>
    /// <param name="bd">Oggetto DocumentTypeWorkflow da modificare </param>
    /// <returns>Restituisce 1 se la modifica è andata a buon fine.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("Workflow")]
    public async Task<ActionResult<int>> UpdateWorkflow([FromBody] DocumentTypeWorkflow bd)
    {
        try
        {
            var exist = await doctypeService.GetWorkflow(bd.DocumentTypeId, bd.EventName);
            if (exist != null)
            {
                int res = await doctypeService.UpdateWorkflow(bd);
                return Ok(res);
            }
            else return NotFound("Il workflow associato alla tipologia documentale e all'evento indicato non è stato trovato.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Elimina un workflow associato alla tipologia documentale e all'evento indicato.
    /// </summary>
    /// <param name="documentTypeId"> Id del tipo documentale. </param>
    /// <param name="eventName"> Nome dell'evento che avvia il processo. </param>
    /// <returns>Ritorna 200 in caso di successo, 404 in caso di NotFound e 400 in caso di fallimento.</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("Workflow/{documentTypeId}/{eventName}")]
    public async Task<ActionResult<string>> RemoveWorkflow(string documentTypeId, string eventName)
    {
        try
        {
            var exist = await doctypeService.GetWorkflow(documentTypeId, eventName);
            if (exist != null)
            {
                var result = await doctypeService.RemoveWorkflow(exist);
                return result > 0 ? Ok("Il workflow associato alla tipologia documentale e all'evento indicato è stato eliminato con successo") : BadRequest("L'eliminazione non è andata a buon fine.");
            }
            else return NotFound("Il workflow associato alla tipologia documentale e all'evento indicato non è stato trovato");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


}