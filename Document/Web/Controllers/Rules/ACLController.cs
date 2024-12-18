using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Filters;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Enumerators;

namespace Web.Controllers.Security;

/// <summary>
/// Le ACL definizione liste di autorizzazioni da associare alle tipologie documentali.
/// Ogni tipologia documentale è associata ad una ACL che definisce quali utenti, ruoli o gruppi (profili) possono utilizzarla
/// Una ACL può essere associata a più tipologie. 
/// I permessi di utilizzo della tipologia (es. visibilità, creazione, modifica, ecc..) vengono definiti sul ruolo associato
/// Associando un gruppo si concedono i permessi agli utenti presenti nel gruppo
/// Associando un utente si concedono tutti i permessi sulla tipologia
/// </summary>
[Authorize]
[Authorization(":admin")]
[ApiController]
[Route("internalapi/[controller]")]
public class ACLController : ControllerBase
{

    private readonly IACLService _aclService;
    //    private readonly UserService userService;

    /// <summary>
    /// Le ACL definizione liste di autorizzazioni da associare alle tipologie documentali.
    /// Ogni tipologia documentale è associata ad una ACL che definisce quali utenti, ruoli o gruppi (profili) possono utilizzarla
    /// Una ACL può essere associata a più tipologie. 
    /// I permessi di utilizzo della tipologia (es. visibilità, creazione, modifica, ecc..) vengono definiti sul ruolo associato
    /// Associando un gruppo si concedono i permessi agli utenti presenti nel gruppo
    /// Associando un utente si concedono tutti i permessi sulla tipologia
    /// </summary>
    /// 
    public ACLController(IACLService aclService) //, UserService userService)
    {
        _aclService = aclService;
        //        this.userService = userService;
    }


    /// <summary>
    /// Recupera il nome di una ACL attraverso il codice Identificativo.
    /// Se l'ACL ha degli utenti/ruoli/gruppi associati, vengono restituiti anche questi
    /// </summary>
    /// <param name="Id">Identificativo della ACL da cercare</param>
    /// <returns></returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(ACL))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{Id}")]
    public async Task<ActionResult<ACL>> GetById(string Id)
    {
        try
        {
            var result = new ACL();
            result = await _aclService.GetById(Id);
            return result == null ? NotFound("ACL non trovato") : Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Recupera l'elenco di tutte le ACL definite sul tenant
    /// </summary>
    /// <returns>Elenco di ACL con i rispettivi utenti/ruoli/gruppi autorizzati</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IEnumerable<ACL>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ACL>>> GetAll()
    {
        try
        {
            var list = await _aclService.GetAll();
            return Ok(list);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary> 
    /// Crea una nuova ACL fornendo un Identificativo e un Nome
    /// </summary>
    /// <remarks>Se l'Identificativo viene fornito vuoto, il sistema genererà un GUID.</remarks>
    /// <param name="acl">Identificativo e Nome della ACL da creare</param>
    /// <returns>Oggetto ACL creato</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(ACL))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    public async Task<ActionResult<ACL>> Insert(CreateOrUpdateACL acl)
    {
        try
        {
            if (await _aclService.GetById(acl.Id) == null)
            {
                ACL A = await _aclService.Insert(acl);
                return A != null ? Ok(A) : BadRequest("L'inserimento non è andato a buon fine.");
            }
            else return Conflict("Non può essere inserita una ACL con un id già presente.");

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Aggiorna il nome di una ACL
    /// </summary>
    /// <param name="acl">Identificativo e Nome della ACL da rinominare</param>
    /// <returns>Oggetto ACL aggiornato</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(ACL))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut]
    public async Task<ActionResult<ACL>> Update(CreateOrUpdateACL acl)
    {
        try
        {
            var exist = await _aclService.GetById(acl.Id);
            if (exist != null)
            {
                var A = await _aclService.Update(acl);
                return A != null ? Ok(A) : BadRequest("L'aggiornamento non è andato a buon fine.");
            }
            else return NotFound("L'ACL selezionato non è stato trovato");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// Cancella una ACL con tutti i riferimenti ad utenti/ruoli/gruppi associati.
    /// Una ACL associata ad almeno una Tipologia non può essere cancellata.
    /// </summary>
    /// <param name="Id">Identificativo della ACL da cancellare</param>
    /// <returns>Oggetto ACL eliminato</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(ACL))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{Id}")]
    public async Task<ActionResult<ACL>> Delete(string Id)
    {
        try
        {
            var exist = await _aclService.GetById(Id);
            if (exist != null)
            {
                var result = await _aclService.Delete(Id);
                return result != null ? Ok(result) : BadRequest("Eliminazione non riuscita");
            }
            else return NotFound("la ACL selezionata non è stata trovata");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Aggiunge un utente/ruolo/gruppo ad una ACL
    /// </summary>
    /// <param name="aclauth">Identificativo della ACL, Codice Profile e Tipo Profilo da autorizzare</param>
    /// <returns>Oggetto ACLPermission aggiunto</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(ACLPermission))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost("AddPermission")]
    public async Task<ActionResult<ACLPermission>> AddPermission(ACLPermission aclauth)
    {
        try
        {
            if (!await _aclService.HasPermission(aclauth.ACLId, aclauth.ProfileId, aclauth.ProfileType, aclauth.PermissionId))
            {
                int res = await _aclService.AddPermission(aclauth);
                if (res > 0) return Ok(await _aclService.GetPermission(aclauth.ACLId, aclauth.ProfileId, aclauth.ProfileType, aclauth.PermissionId));
                else return BadRequest("L'inserimento non è andato a buon fine.");
            }
            else return Conflict("Non può essere inserito un permesso con un id già presente");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Modifica un profilo autorizzativo da una ACL
    /// </summary>
    /// <param name="aclauth">Identificativo della ACL, Codice Profile e Tipo Profilo da autorizzare</param>
    /// <returns>Oggetto ACLPermission aggiunto</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("changePermission")]
    public async Task<ActionResult<int>> ChangePermission(ACLPermission aclauth)
    {
        try
        {
            var result = await _aclService.ChangePermission(aclauth);
            return result;
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }


    /// <summary>
    /// Rimuove un profilo autorizzativo da una ACL
    /// </summary>
    /// <param name="aclauth">Identificativo della ACL, Codice Profile e Tipo Profilo da autorizzare</param>
    /// <returns>Oggetto ACLPermission aggiunto</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("RemovePermission")]
    public async Task<ActionResult<int>> RemovePermission(ACLPermission aclauth)
    {
        try
        {
            var exist = await _aclService.GetPermission(aclauth.ACLId, aclauth.ProfileId, aclauth.ProfileType, aclauth.PermissionId);
            if (exist != null)
            {
                var result = await _aclService.RemovePermission(exist);
                return result > 0 ? Ok(result) : BadRequest("Eliminazione non riuscita");
            }
            else return NotFound("L'ACL selezionato non è stato trovato");
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }



    /// <summary>
    /// Ritorna l'elenco dei profili abilitati su una ACL.
    /// </summary>
    /// <param name="aclId">Identificativo univoco della ACL</param>
    /// <returns>Elenco di oggetti ACLPermission</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<ACLPermission>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("GetAllPermissions")]
    public async Task<ActionResult<List<ACLPermission>>> GetPermissions(string aclId)
    {
        try
        {
            //            List<ACLPermission_DTO> items = new List<ACLPermission_DTO>();
            //            foreach (var p in await _aclService.GetAllPermissions(aclId))
            //            {
            //                ACLPermission_DTO P = new ACLPermission_DTO();
            //                P.ACLId = aclId;
            //                P.PermissionId = p.PermissionId;
            //                P.Authorization = p.Authorization;
            //                P.ProfileId = p.ProfileId;
            //                P.ProfileType = p.ProfileType;
            ////                P.ProfileName = await userService.GetName()
            //            }
            var items = await _aclService.GetAllPermissions(aclId);
            return Ok(items);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// Verifica se un profilo è presente in una ACL 
    /// </summary>
    /// <param name="aclId">Identificativo univoco della ACL</param>
    /// <param name="profileId">Identificativo univoco del profilo</param>
    /// <param name="profileType">Tipo di profilo</param>
    /// <returns>true=profilo trovato</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("HasPermission")]
    public async Task<ActionResult<bool>> HasPermission(string aclId, string profileId, ProfileType profileType, string permissionId)
    {
        try
        {
            var result = new ACLPermission();
            result = await _aclService.GetPermission(aclId, profileId, profileType, permissionId);
            return Ok(result != null);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


}


