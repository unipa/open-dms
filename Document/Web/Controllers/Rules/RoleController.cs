using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Filters;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.OrganizationUnits;
using OpenDMS.Domain.Enumerators;


namespace Web.Controllers.Security;


[Authorize]
[Authorization(":admin")]
[ApiController]
[Route("internalapi/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleService roleRepo;

    public RoleController(IRoleService roleRepo)
    {
        this.roleRepo = roleRepo;
    }



    /// <summary>
    /// Ottiene l'elenco dei ruoli gestiti 
    /// </summary>
    /// <param name="IncludeDeleted">Indica se restitruire anche i ruoli cancellati nel tempo</param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<Role>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<Role>>> GetAll(bool IncludeDeleted = false)
    {
        try
        {
            return await roleRepo.GetAll(IncludeDeleted);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary> 
    /// Crea un nuovo ruolo
    /// </summary>
    /// <param name="roleId">Oggetto Role da creare</param>
    /// <param name="roleName">Oggetto Role da creare</param>
    /// <returns></returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    public async Task<ActionResult<Role>> Create(Role role)
    {
        try
        {
            if (string.IsNullOrEmpty(role.RoleName)) role.RoleName= role.Id;
            if (string.IsNullOrEmpty(role.Id)) return BadRequest(nameof(role.Id) + " non può essere vuoto");
            var foundWithSameName = await roleRepo.GetByName(role.RoleName);
            if (foundWithSameName != null) return NotFound("Esiste un altro ruolo con lo stesso nome");

            var found = await roleRepo.GetById(role.Id);
            if (found == null)
            {
                found = new Role() { Id = role.Id, RoleName = role.RoleName };
                return await roleRepo.Create(found);
            }
            else return Conflict("Il ruolo esiste già");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// Rinomina un ruolo
    /// </summary>
    /// <param name="roleId">Oggetto Role da rinominare</param>
    /// <param name="roleName">Oggetto Role da rinominare</param>
    /// <returns></returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Role))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut]
    public async Task<ActionResult<Role>> Rename(Role role)
    {
        try
        {
            if (string.IsNullOrEmpty(role.RoleName)) role.RoleName = role.Id;
            if (string.IsNullOrEmpty(role.Id)) return BadRequest(nameof(role.Id) + " non può essere vuoto");
            var foundWithSameName = await roleRepo.GetByName(role.RoleName);
            if (foundWithSameName != null && foundWithSameName.Id != role.Id) return NotFound("Esiste un altro ruolo con lo stesso nome");

            var found = await roleRepo.GetById(role.Id);
            if (found != null)
            {
                found.RoleName = role.RoleName;
                return await roleRepo.Rename(found);
            }
            else return NotFound("Il ruolo indicato non esiste");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Elimina (logicamente) un ruolo
    /// </summary>
    /// <param name="roleId">Oggetto Role da cancellare</param>
    /// <returns>1= ruolo cancellato</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{roleId}")]
    public async Task<ActionResult<int>> Delete(string roleId)
    {
        try
        {
            if (string.IsNullOrEmpty(roleId)) return BadRequest(nameof(roleId) + " non può essere vuoto");

            if (await roleRepo.GetById(roleId) != null)
            {
                return await roleRepo.Delete(roleId);
            }
            else return NotFound("Il ruolo indicato non esiste");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /// <summary>
    /// Ripristina un ruolo eliminato
    /// </summary>
    /// <param name="roleId">Oggetto Role da ripristinare</param>
    /// <returns>1= ruolo cancellato</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("restore/{roleId}")]
    public async Task<ActionResult<int>> Restore(string roleId)
    {
        try
        {
            if (string.IsNullOrEmpty(roleId)) return BadRequest(nameof(roleId) + " non può essere vuoto");

            if (await roleRepo.GetById(roleId) != null)
            {
                return await roleRepo.Restore(roleId);
            }
            else return NotFound("Il ruolo indicato non esiste");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Ritorna un ruolo attraverso il nome
    /// </summary>
    /// <param name="roleName">Oggetto Role da recuperare</param>
    /// <returns>1= ruolo cancellato</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Role))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("name/{roleName}")]
    public async Task<ActionResult<Role>> GetByName(string roleName)
    {
        try
        {
            if (string.IsNullOrEmpty(roleName)) return BadRequest(nameof(roleName) + " non può essere vuoto");

            return await roleRepo.GetByName(roleName);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /// <summary>
    /// Ritorna un ruolo attraverso l'id
    /// </summary>
    /// <param name="roleId">Oggetto Role da recuperare</param>
    /// <returns>1= ruolo cancellato</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Role))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("{roleId}")]
    public async Task<ActionResult<Role>> GetById(string roleId)
    {
        try
        {
            if (string.IsNullOrEmpty(roleId)) return BadRequest(nameof(roleId) + " non può essere vuoto");

            return await roleRepo.GetById(roleId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    ///// <summary>
    ///// Ottiene i permessi associati a un ruolo tramite RoleId. 
    ///// </summary>
    ///// <param name="roleId"> Id del ruolo </param>
    ///// <returns></returns>
    //[ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IList<RolePermission>))]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[HttpGet("Permissions/{id}")]
    //public async Task<ActionResult<List<PermissionAuthorization>>> GetPermissions(string roleId)
    //{
    //    try
    //    {
    //        if (string.IsNullOrEmpty(roleId)) return BadRequest(nameof(roleId) + " non può essere vuoto");
    //        if (roleRepo.GetById(roleId) == null) return BadRequest(nameof(roleId) + " non trovato");

    //        return Ok(await roleRepo.GetPermissions(roleId));
    //    }
    //    catch (Exception ex) { return BadRequest(ex.Message); }
    //}

    ///// <summary>
    ///// Ottiene uno specifico permesso tramite Id del ruolo e Id del permesso
    ///// </summary>
    ///// <param name="roleId">Id del ruolo</param>
    ///// <param name="permissionId">Id del permesso</param>
    ///// <returns></returns>
    //[ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[HttpGet("Permission/{roleId}/{permissionId}")]
    //public async Task<ActionResult<AuthorizationType>> GetAuthorization(string roleId, string permissionId)
    //{
    //    try
    //    {
    //        if (string.IsNullOrEmpty(roleId)) return BadRequest(nameof(roleId) + " non può essere vuoto");
    //        if (roleRepo.GetById(roleId) == null) return BadRequest(nameof(roleId) + " non trovato");

    //        var result = await roleRepo.GetPermission(roleId, permissionId);
    //        return result == null ? NotFound("Il permesso associato al ruolo indicato non è stato trovato.") : Ok(result.Authorization);
    //    }
    //    catch (Exception ex) { return BadRequest(ex.Message); }
    //}

    ///// <summary> 
    ///// Metodo per associare(salvare) un nuovo permesso ad un ruolo
    ///// </summary>
    ///// <param name="bd">Oggetto RolePermission da creare</param>
    ///// <returns></returns>
    //[ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status409Conflict)]
    //[HttpPost("Permission")]
    //public async Task<ActionResult<RolePermission>> AddPermission(string roleId, string permissionId, AuthorizationType authorization)
    //{
    //    try
    //    {
    //        if (string.IsNullOrEmpty(roleId)) return BadRequest(nameof(roleId) + " non può essere vuoto");
    //        if (roleRepo.GetById(roleId) == null) return BadRequest(nameof(roleId) + " non trovato");

    //        if (await roleRepo.GetPermission(roleId, permissionId.ToString()) == null)
    //        {
    //            RolePermission R = new RolePermission() { RoleId = roleId, PermissionId = permissionId, Authorization = authorization };
    //            return await roleRepo.AddPermission(R);
    //        }
    //        else return Conflict("Il permesso indicato esiste già");
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}


    ///// <summary>
    ///// Metodo per eliminare un permesso associato ad un ruolo 
    ///// </summary>
    ///// <param name="bd">Oggetto RolePermission da eliminare</param>
    ///// <returns></returns>
    //[ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[HttpDelete("Permission/{roleId}/{permissionId}")]
    //public async Task<ActionResult<RolePermission>> RemovePermission(string roleId, string permissionId)
    //{
    //    if (string.IsNullOrEmpty(roleId)) return BadRequest(nameof(roleId) + " non può essere vuoto");
    //    if (roleRepo.GetById(roleId) == null) return BadRequest(nameof(roleId) + " non trovato");

    //    try
    //    {
    //        var R = await roleRepo.GetPermission(roleId, permissionId.ToString());
    //        if (R != null)
    //        {
    //            var result = await roleRepo.Delete(roleId);
    //            return result > 0 ? Ok(R) : null;
    //        }
    //        else return NotFound("Il permesso associato al ruolo indicato non è stato trovato");
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}
}