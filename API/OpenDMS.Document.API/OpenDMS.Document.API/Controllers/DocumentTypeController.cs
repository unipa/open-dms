using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;

namespace OpenDMS.DocumentManager.API.Controllers;

[Authorize]
[ApiController]
[Route("api/admin/[controller]")]
public class DocumentTypeController : ControllerBase
{

    private readonly IDocumentTypeService doctypeService;
    private readonly IUserService userContext;

    public DocumentTypeController(IDocumentTypeService doctypeService, IUserService userContext)
    {
        this.doctypeService = doctypeService;
        this.userContext = userContext;
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
            if (string.IsNullOrEmpty(Id)) return BadRequest(nameof(Id) + " non pu� essere vuoto");
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
            if (string.IsNullOrEmpty(userId)) userId = this.User.Identity.Name; 
            var  userInfo = await userContext.GetUserProfile(userId);
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
                var userId = this.User.Identity?.Name??"";
                var userInfo = await userContext.GetUserProfile(userId);
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
            var userId = this.User.Identity.Name;
            var userInfo = await userContext.GetUserProfile(userId);
            var types = await doctypeService.GetByPermission(userInfo, PermissionType.CanViewContent);
            if (ct != ContentType.Any)
                types = types.Where(dt => dt.ContentType == ct).ToList();
            List<LookupTable> result = new List<LookupTable>();
            foreach (var type in types)
            {
                if (type.Icon == null) { type.Icon = "fa fa-tag"; }
                result.Add(new LookupTable { Id = type.Id, Description= type.Name, Annotation= type.Icon, TableId= type.CategoryId });
            }

            return Ok(result);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

}