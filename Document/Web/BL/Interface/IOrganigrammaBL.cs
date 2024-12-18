using OpenDMS.Domain.Entities.OrganizationUnits;
using OpenDMS.Domain.Models;
using Web.Model.Admin;

namespace Web.BL.Interface
{
    public interface IOrganigrammaBL
    {
        Task AddUser(UserInGroup_DTO bd);
        Task ChangeRoleUser(UserInGroup_DTO bd);
        Task<OrganizationNodeInfo> CreateOrganizationNode(CreateOrUpdateOrganizationNode_DTO bd);
        Task<List<UserGroupRole>> GetAllByRole(string roleId, string filter);

        Task<IEnumerable<OrganizationNodeTree>> GetOrganizationTree(int StartISODate = 0);
        Task<OrganizationNodeInfo> MoveOrganizationNode(MoveOrganizationNode_DTO bd);
        Task<int> RemoveOrganizationNode(string userGroupId, DateTime EndDate, int StartISODate);
        Task RemoveUser(UserInGroup_DTO bd);
        Task<OrganizationNodeInfo> UpdateOrganizationNode(string userGroupId, CreateOrUpdateOrganizationNode_DTO bd);
    }
}