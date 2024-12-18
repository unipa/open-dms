using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Core.DTOs
{
    public class CreateOrUpdateOrganizationNode
    {
        public string? ParentUserGroupId { get; set; }
        public string ShortName { get; set; } = "";
        public string Name { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool Visible { get; set; }

        /// <summary>
        /// Modalità di chiusura del nodo
        /// 1. Riassegnazione Tasks al nodo padre
        /// 2. Riassegnazione Tasks ad un nodo specifico (ClosingProfile)
        /// 3. Riassegnazione Tasks ad un ruolo specifico (ClosingProfile)
        /// 4. Completamento Automatico dei Tasks
        /// </summary>
        public int TaskReallocationStrategy { get; set; }

        public string TaskReallocationProfile { get; set; }
        public string ExternalId { get; set; }

        /// <summary>
        /// Modalità di notifica dei task
        /// 1. Utenti abilitati al ruolo "ViewInbox"
        /// 2. Utenti con ruolo specifico (NotificationProfile)
        /// 3. Tutti gli utenti del gruppo
        /// 4. Sotto-strutture
        /// </summary>
        public int NotificationStrategy { get; set; }
        public string NotificationProfile { get; set; }

        /// <summary>
        /// Modalità di notifica CC dei task
        /// 1. Utenti abilitati al ruolo "ViewInbox"
        /// 2. Utenti con ruolo specifico (NotificationProfile)
        /// 3. Tutti gli utenti del gruppo
        /// 4. Tutti gli utenti e sotto-strutture
        public int NotificationStrategyCC { get; set; }

    }
}
