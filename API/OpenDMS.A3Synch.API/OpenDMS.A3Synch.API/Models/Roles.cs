using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A3Synch.Models
{
    [Table("Roles")]
    public class Roles
    {
        [StringLength(64)]
        public string Id { get; set; }

        [StringLength(255)]
        public string? RoleName { get; set; }

        public string? ExternalApp { get; set; }
        public DateTime CreationDate { get; set; }

        public bool Deleted { get; set; } = false;
        public bool? DeletionDate { get; set; } = null;

        public DateTime LastUpdate { get; set; }

    }

}