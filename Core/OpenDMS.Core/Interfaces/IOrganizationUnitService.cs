using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Entities.OrganizationUnits;
using OpenDMS.Domain.Models;

namespace OpenDMS.Core.Interfaces
{
    public interface IOrganizationUnitService
    {
        Task<OrganizationNodeInfo> AddOrganizationNode(CreateOrUpdateOrganizationNode organization, string userId);
        Task<int> AddUser(UserInGroup userInGroup);
        Task<int> DeleteOrganizationNode(string userGroupId, int StartISODate, DateTime EndDate);
        Task<int> EditUser(UserInGroup userInGroup);
        Task<OrganizationNodeInfo> GetById(string userGroupId, int StartISODate = 0);
        Task<OrganizationNodeInfo> GetByExternalId(string ExternalId, int StartISODate = 0);
        Task<UserGroup> GetGroup(string groupId);
        Task<List<UserInGroup>> GetGroupsByUser(string userId);
        Task<List<OrganizationNodeTree>> GetOrganizationTree(int StartISODate = 0, string rootNode = "");
        Task<List<UserInGroup>> GetUsers(string userGroupId, int StartISODate = 0);
        Task<OrganizationNodeInfo> MoveOrganizationNode(MoveOrganizationNode organization);
        Task<int> RemoveUser(UserInGroup userInGroup);
        Task<OrganizationNodeInfo> UpdateOrganizationNode(string userGroupId, CreateOrUpdateOrganizationNode organization);
        Task<List<UserGroup>> Find(string SearchText, int MaxResults = 0);
        Task<List<UserGroupRole>> GetUsersByFilter(UserFilter filter);
        Task<OrganizationNodeInfo> GetByName(string shortName, int StartISODate = 0);
    }
}