using A3Synch.Models;

namespace A3Synch.Interfacce
{
    public interface IRolesBL
    {
        Task<int> SynchRolesInDb(List<Members> all_members);
        void ResetStatus();
    }
}
