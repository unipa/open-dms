using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Domain.Entities.OrganizationUnits
{
    public class Role
    {
        [StringLength(64)]
        public string Id { get; set; }

        [StringLength(255)]
        public string RoleName { get; set; }

        /// <summary>
        /// Indica che il ruolo proviene da una sorgente esterna
        /// e non può essere modificato
        /// </summary>
        [StringLength(64)]
        public string ExternalApp { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public bool Deleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

    }
}
