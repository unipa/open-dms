using A3Synch.Models;

namespace A3Synch.Interfacce
{
    public interface IContactsBL
    {
        Task<int> SynchContactsInDb(List<Members> all_members);
        void ResetStatus();
    }
}
