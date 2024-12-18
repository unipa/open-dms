using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.OrganizationUnits;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Users
{
    public class User
    {
        [StringLength(128)]
        public string Id { get; set; }

        [StringLength(64)]
        public string ContactId { get; set; }

        /// <summary>
        /// Indica da quale sorgente esterna proviene l'utente
        /// </summary>
        [StringLength(64)]
        public string ExternalApp { get; set; } = "";

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public bool Deleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

        public virtual Contact Contact { get; set; }

        public virtual List<UserGroupRole> UserGroups { get; set; }

        // Recapito Email per le notifiche
        public string Email { get; set; } = "";

        public static User SystemUser()
        {
            var u = new User();
            u.Id = SpecialUser.SystemUser;
            u.ContactId = null;
            u.Contact = new Contact() { FriendlyName = "Utente di Sistema", SearchName = "Utente di Sistema", FullName = "Utente di Sistema" };
            return u;
        }

    }
}
