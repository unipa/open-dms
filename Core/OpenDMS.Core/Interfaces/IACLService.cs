using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;

namespace OpenDMS.Core.Interfaces
{
    public interface IACLService
    {
        Task<int> AddPermission(ACLPermission aclauth);
        Task<int> ChangePermission(ACLPermission aclauth);
        Task<ACL> Delete(string Id);
        Task<List<ACL>> GetAll();
        Task<List<ACLPermission>> GetAllPermissions(string aclId);
        Task<ACL> GetById(string Id);
        Task<AuthorizationType> GetAuthorization(string aclId, UserProfile userInfo, string permissionId);
        Task<AuthorizationType> GetAuthorization(string aclId, string profileId, ProfileType profileType, string permissionId);
      Task<ACLPermission> GetPermission(string aclId, string profileId, ProfileType profileType, string permissionId);
        Task<bool> HasPermission(string aclId, string profileId, ProfileType profileType, string permissionId);
        Task<ACL> Insert(CreateOrUpdateACL acl);
        Task<int> RemovePermission(ACLPermission aclauth);
        Task<ACL> Update(CreateOrUpdateACL acl);
    }
}