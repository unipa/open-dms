
namespace OpenDMS.Core.DTOs
{
    public class MoveOrganizationNode
    {
        public string userGroupId { get; set; } = "";
        public string NewParentUserGroupId { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Modalità di chiusura del nodo
        /// 1. Riassegnazione Tasks al nodo padre
        /// 2. Riassegnazione Tasks ad un nodo specifico (ClosingProfile)
        /// 3. Riassegnazione Tasks ad un ruolo specifico (ClosingProfile)
        /// 4. Completamento Automatico dei Tasks
        /// </summary>
        public int TaskReallocationStrategy { get; set; }

        public string TaskReallocationProfile { get; set; }

    }
}
