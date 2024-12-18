using OpenDMS.Domain.Entities.OrganizationUnits;

namespace OpenDMS.Domain.Models
{
    public class OrganizationNodeInfo 
    {
        public string UserGroupId { get; set; } = "";
        public string? ParentUserGroupId { get; set; } = null;
        public string ShortName { get; set; } = "";
        public string Name { get; set; } = "";
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
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
        public string ClosingNote { get; set; }
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

        public int LeftBound { get; set; }
        public int RightBound { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;


        public OrganizationNodeInfo()
        {
            
        }

        public OrganizationNodeInfo(OrganizationNode O)
        {
            this.UserGroupId = O.UserGroupId;
            this.ExternalId = O.UserGroup?.ExternalId;
            this.StartDate = O.StartISODate > 0 ? new DateTime(O.StartISODate / 10000, O.StartISODate / 100 % 100, O.StartISODate % 100) : null;
            this.EndDate = O.EndISODate > 0 && O.EndISODate < 99999999 ? new DateTime(O.EndISODate / 10000, O.EndISODate / 100 % 100, O.EndISODate % 100) : null;
            this.LastUpdate = O.LastUpdate;
            this.CreationDate = O.CreationDate;
            this.ClosingNote = O.ClosingNote;
            this.ParentUserGroupId = O.ParentUserGroupId;
            this.LeftBound = O.LeftBound;
            this.RightBound = O.RightBound;
            this.Name = O.UserGroup.Name;
            if (O.UserGroup != null)
            {
                this.NotificationProfile = O.UserGroup?.NotificationProfile;
                this.NotificationStrategy = O.UserGroup.NotificationStrategy;
                this.NotificationStrategyCC = O.UserGroup.NotificationStrategyCC;
                this.ShortName = O.UserGroup.ShortName;
            }
            this.TaskReallocationProfile = O.TaskReallocationProfile;
            this.TaskReallocationStrategy = O.TaskReallocationStrategy;
            this.Visible = O.UserGroup.Visible;
        }
    }
}