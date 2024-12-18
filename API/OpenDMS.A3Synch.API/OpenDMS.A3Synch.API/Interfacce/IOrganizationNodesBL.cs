using A3Synch.Models;
using OpenDMS.A3Synch.API.Models;

namespace A3Synch.Interfacce
{
    public interface IOrganizationNodesBL
    {
        Task<int> SyncOrganigrammaNodes(List<Struttura> allUnits);

		void ResetStatus();

        //PER DEBUG
        //Task<List<OrganizationNodes>> GetAllOrganizationNodesFromDb();



    }
}
