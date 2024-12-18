using A3Synch.Models;

namespace A3Synch.Interfacce
{
    public interface IUserGroupRolesDAO
    {
        //DAO per UserGroupRoles
        Task<int> UpdateUserGroupRoles(List<UserGroupRoles> userGroupsRolesList);
        Task<List<UserGroupRoles>> GetAllUserGroupsRoles();
        Task<List<UserGroupRoles>> GetUserGroupsRoles(string UserId);

    }
}
