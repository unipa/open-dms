using OpenDMS.Domain.Entities.OrganizationUnits;
using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Core.Interfaces
{
    public interface IRoleService
    {
        Task<Role> Create(Role R);
        Task<int> Delete(string roleId);
        Task<List<Role>> GetAll(bool IncludeDeleted = false);
        Task<Role> GetById(string roleId);
        Task<Role> GetByName(string roleName);
        Task<Role> Rename(Role R);
        Task<int> Restore(string roleId);
        Task<List<Role>> Find(string SearchText, int MaxResults = 0);
        //Task<AuthorizationType> GetAuthorization(string roleId, string permissionId);
        //Task<RolePermission> AddPermission(RolePermission R);
        //Task<RolePermission> ChangePermission(RolePermission R);
        //Task<RolePermission> GetPermission(string roleId, string permissionId);
        //Task<List<RolePermission>> GetPermissions(string roleId);
        //Task<RolePermission> RemovePermission(RolePermission R);

    }
}