using A3Synch.Models;

namespace A3Synch.Interfacce
{
    public interface IUserGroupsBL
    {
        Task<int> GetAllOrganizationsPages(List<Struttura> allUnits);
        void ResetStatus();
    }
}
