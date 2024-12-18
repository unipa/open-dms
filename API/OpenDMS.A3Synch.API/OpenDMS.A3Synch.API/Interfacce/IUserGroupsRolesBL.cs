using A3Synch.Models;

namespace A3Synch.Interfacce
{
    public interface IUserGroupRolesBL
    {
        Task<int> SynchUserGroupsRolesInDb(List<Members> all_members);
        void ResetStatus();
    }
}
