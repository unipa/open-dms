using Microsoft.Extensions.Logging;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.OrganizationUnits;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Repositories;
using System.Data;

namespace OpenDMS.Core.BusinessLogic;

public class RoleService : IRoleService
{
    private readonly ILogger<IRoleService> logger;
    //private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly IRoleRepository _repository;

    public RoleService(ILogger<IRoleService> logger, IRoleRepository roleRepo)//, IRolePermissionRepository rolePermissionRepo)
    {
        this.logger = logger;
      //  this._rolePermissionRepository = rolePermissionRepo;
        this._repository = roleRepo;
    }


    /// <summary>
    /// Ottiene l'elenco dei ruoli gestiti 
    /// </summary>
    /// <returns></returns>
    public async Task<List<Role>> GetAll(bool IncludeDeleted = false)
    {
        try
        {
            return await _repository.GetAll(IncludeDeleted);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GetAll");
            throw;
        }
    }

    /// <summary> 
    /// Crea un nuovo ruolo
    /// </summary>
    /// <param name="roleId">Oggetto Role da creare</param>
    /// <param name="roleName">Oggetto Role da creare</param>
    /// <returns></returns>
    public async Task<Role> Create(Role R)
    {
        try
        {
            if (string.IsNullOrEmpty(R.RoleName)) R.RoleName = R.Id;
            if (string.IsNullOrEmpty(R.Id)) throw new ArgumentNullException(R.Id);

            var foundWithSameName = await _repository.GetByName(R.RoleName);
            if (foundWithSameName != null) throw new DuplicateNameException(R.RoleName);

            var found = await _repository.GetById(R.Id);
            if (found == null)
            {
                return await _repository.Insert(R);
            }
            else throw new DuplicateNameException(R.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Create");
            throw;
        }
    }


    /// <summary>
    /// Rinomina un ruolo
    /// </summary>
    /// <param name="roleId">Oggetto Role da rinominare</param>
    /// <param name="roleName">Oggetto Role da rinominare</param>
    /// <returns></returns>
    public async Task<Role> Rename(Role R)
    {
        try
        {
            if (string.IsNullOrEmpty(R.RoleName)) R.RoleName = R.Id;
            if (string.IsNullOrEmpty(R.Id)) throw new ArgumentNullException(R.Id);

            var foundWithSameName = await _repository.GetByName(R.RoleName);
            if (foundWithSameName != null && foundWithSameName.Id != R.Id) throw new DuplicateNameException(R.RoleName);

            var found = await _repository.GetById(R.Id);
            if (found != null)
            {
                found.RoleName = R.RoleName;
                return await _repository.Rename(found);
            }
            else throw new KeyNotFoundException(R.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Rename");
            throw;
        }
    }

    /// <summary>
    /// Elimina (logicamente) un ruolo
    /// </summary>
    /// <param name="roleId">Oggetto Role da cancellare</param>
    /// <returns>1= ruolo cancellato</returns>
    public async Task<int> Delete(string roleId)
    {
        try
        {
            if (string.IsNullOrEmpty(roleId)) throw new ArgumentNullException(roleId);


            if ((await _repository.GetById(roleId)) != null)
            {
                return await _repository.Delete(roleId);
            }
            else throw new KeyNotFoundException(roleId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Delete");
            throw;
        }
    }
    /// <summary>
    /// Ripristina un ruolo eliminato
    /// </summary>
    /// <param name="roleId">Oggetto Role da ripristinare</param>
    /// <returns>1= ruolo cancellato</returns>
    public async Task<int> Restore(string roleId)
    {
        try
        {
            if (string.IsNullOrEmpty(roleId)) throw new ArgumentNullException(roleId);

            if ((await _repository.GetById(roleId)) != null)
            {
                return await _repository.Restore(roleId);
            }
            else throw new KeyNotFoundException(roleId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Restore");
            throw;
        }
    }

    /// <summary>
    /// Ritorna un ruolo attraverso il nome
    /// </summary>
    /// <param name="roleName">Oggetto Role da recuperare</param>
    /// <returns>1= ruolo cancellato</returns>
    public async Task<Role> GetByName(string roleName)
    {
        try
        {
            return (await _repository.GetByName(roleName));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GetByName");
            throw;
        }
    }
    /// <summary>
    /// Ritorna un ruolo attraverso l'id
    /// </summary>
    /// <param name="roleId">Oggetto Role da recuperare</param>
    /// <returns>1= ruolo cancellato</returns>
    public async Task<Role> GetById(string roleId)
    {
        try
        {
            roleId = roleId.Split("\\")[0];
            return (await _repository.GetById(roleId));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GetById");
            throw;
        }
    }


    public async Task<List<Role>> Find(string SearchText, int MaxResults = 0)
    {
        return await _repository.Find(SearchText, MaxResults);

    }


    ///// <summary>
    ///// Ottiene uno specifico permesso tramite Id del ruolo e Id del permesso
    ///// </summary>
    ///// <param name="roleId">Id del ruolo</param>
    ///// <param name="permissionId">Id del permesso</param>
    ///// <returns></returns>
    //public async Task<AuthorizationType> GetAuthorization(string roleId, string permissionId)
    //{
    //    try
    //    {
    //        if (string.IsNullOrEmpty(roleId)) throw new ArgumentNullException(roleId);
    //        if ((await _repository.GetById(roleId)) == null) throw new ArgumentNullException(roleId);

    //        return (await _rolePermissionRepository.GetPermission(roleId, permissionId))?.Authorization ?? AuthorizationType.None;
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.LogError(ex, "GetAuthorization");
    //        throw;
    //    }
    //}







    ///// <summary>
    ///// Ottiene i permessi associati a un ruolo tramite RoleId. 
    ///// </summary>
    ///// <param name="roleId"> Id del ruolo </param>
    ///// <returns></returns>
    //public async Task<List<RolePermission>> GetPermissions(string roleId)
    //{
    //    try
    //    {
    //        if (string.IsNullOrEmpty(roleId)) throw new ArgumentNullException(roleId);
    //        if ((await _repository.GetById(roleId)) == null) throw new ArgumentNullException(roleId);

    //        return await _rolePermissionRepository.GetPermissions(roleId);
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.LogError(ex, "GetPermissions");
    //        throw;
    //    }
    //}

    ///// <summary>
    ///// Ottiene uno specifico permesso tramite Id del ruolo e Id del permesso
    ///// </summary>
    ///// <param name="roleId">Id del ruolo</param>
    ///// <param name="permissionId">Id del permesso</param>
    ///// <returns></returns>
    //public async Task<RolePermission> GetPermission(string roleId, string permissionId)
    //{
    //    try
    //    {
    //        if (string.IsNullOrEmpty(roleId)) throw new ArgumentNullException(roleId);
    //        if ((await _repository.GetById(roleId)) == null) throw new ArgumentNullException(roleId);

    //        return (await _rolePermissionRepository.GetPermission(roleId, permissionId));
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.LogError(ex, "GetPermission");
    //        throw;
    //    }
    //}

    ///// <summary> 
    ///// Metodo per associare(salvare) un nuovo permesso ad un ruolo
    ///// </summary>
    ///// <param name="bd">Oggetto RolePermission da creare</param>
    ///// <returns></returns>
    //public async Task<RolePermission> AddPermission(RolePermission R)
    //{
    //    try
    //    {
    //        if (string.IsNullOrEmpty(R.RoleId)) throw new ArgumentNullException(R.RoleId);
    //        if ((await _repository.GetById(R.RoleId)) == null) throw new ArgumentNullException(R.RoleId);

    //        var res = await _rolePermissionRepository.Create(R);
    //        return res > 0 ? R : null;
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.LogError(ex, "AddPermission");
    //        throw;
    //    }
    //}

    ///// <summary> 
    ///// Metodo per associare(salvare) un nuovo permesso ad un ruolo
    ///// </summary>
    ///// <param name="bd">Oggetto RolePermission da creare</param>
    ///// <returns></returns>
    //public async Task<RolePermission> ChangePermission(RolePermission R)
    //{
    //    try
    //    {
    //        if (string.IsNullOrEmpty(R.RoleId)) throw new ArgumentNullException(R.RoleId);
    //        if ((await _repository.GetById(R.RoleId)) == null) throw new ArgumentNullException(R.RoleId);

    //        var res = await _rolePermissionRepository.Update(R);
    //        return res > 0 ? R : null;
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.LogError(ex, "ChangePermission");
    //        throw;
    //    }
    //}

    ///// <summary>
    ///// Metodo per eliminare un permesso associato ad un ruolo 
    ///// </summary>
    ///// <param name="bd">Oggetto RolePermission da eliminare</param>
    ///// <returns></returns>
    //public async Task<RolePermission> RemovePermission(RolePermission R)
    //{
    //    if (string.IsNullOrEmpty(R.RoleId)) throw new ArgumentNullException(R.RoleId);

    //    try
    //    {
    //        var result = await _rolePermissionRepository.Delete(R);
    //        return result > 0 ? R : null;
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.LogError(ex, "RemovePermission");
    //        throw;
    //    }
    //}
}