using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Domain.Repositories
{
    public interface IACLRepository
    {
        Task<int> Delete(ACL bd);
        Task<int> Insert(ACL bd);
        Task<int> Update(ACL bd);
        Task<ACL> GetById(string acl);
        Task<List<ACL>> GetAll();
        Task<List<ACL>> GetByProfile(ProfileType profileType, string profileId);


        Task<int> AddPermission(ACLPermission bd);
        Task<int> ChangePermission(ACLPermission bd);
        Task<int> RemovePermission(ACLPermission bd);
        Task<AuthorizationType> GetAuthorization(string acl, string userId, ProfileType userType, string permissionId);
        Task<ACLPermission> GetPermission(string acl, string userId, ProfileType userType, string permissionId);
        Task<List<ACLPermission>> GetAllPermissions(string acl);
        Task<List<ACLPermission>> GetByProfile(string acl, ProfileType profileType, string profileId);
    }
}