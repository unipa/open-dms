using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.OrganizationUnits
{
    [Index(nameof(ExternalId), nameof(ExternalApp), IsUnique = true)]
    public class UserGroup
    {
        [StringLength(64)]
        public string Id { get; set; } // = Guid.NewGuid().ToString();

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string ShortName { get; set; }

        public DateTime? ClosingDate { get; set; }

        public DateTime CreationDate { get; set; }

        [StringLength(64)]
        public string ClosingUser { get; set; }

        [StringLength(64)]
        public string CreationUser { get; set; }

        [StringLength(255)]
        public string ExternalId { get; set; }
        [StringLength(255)]
        public string ExternalApp { get; set; }

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


        /// <summary>
        /// Indica se il gruppo è visibile nelle operazioni di smistamento
        /// </summary>
        public bool Visible { get; set; }

        public bool Closed { get; set; }



        public UserGroup()
        {
            Closed = false;
            ClosingDate = DateTime.UtcNow;
            CreationDate = DateTime.UtcNow;
            Name = "";
            ExternalId = "";
            ShortName = "";
            ClosingUser = "";
            CreationUser = "";
            Visible = true;
        }
    }
}