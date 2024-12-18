using A3Synch.Models;
using System.Xml.Linq;

namespace A3Synch.Interfacce
{
    public interface IOrganizationNodesDAO
    {
        Task<List<OrganizationNodes>> GetAllOrganizationNodes();
        Task<int> UpdateOrganizationNode(List<OrganizationNodes> node);
        Task<int> MaxRightBound();
        string GetOrganizationNodeId(string externalId);
        DateTime GetOrganizationCreationDate(string nodeId);
        Task<int> GetStartISODate(string id);
        Task<int> GetENDISODate(string id);
        Task<List<OrganizationNodes>> GetOldOrganizationNodes();

	}
}
