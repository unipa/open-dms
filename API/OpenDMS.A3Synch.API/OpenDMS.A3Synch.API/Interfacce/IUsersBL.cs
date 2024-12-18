using A3Synch.BL;
using A3Synch.Models;

namespace A3Synch.Interfacce
{
    public interface IUsersBL
    {
        Task<int> SynchUsersInDb(List<Members> all_members, bool isKeycloakBL=false);
        Task<List<Users>> GetAllUsers();

        void ResetStatus();
    }
}
