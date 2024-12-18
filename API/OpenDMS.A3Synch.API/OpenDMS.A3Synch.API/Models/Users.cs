using System.ComponentModel.DataAnnotations;

namespace A3Synch.Models
{
    public class Users
    {
        [StringLength(64)]
        public string Id { get; set; }

        [StringLength(64)]
        public string ContactId { get; set; }

        public DateTime CreationDate { get; set; }

        public bool Deleted { get; set; } = false;
        public bool? DeletionDate { get; set; } = null;

        public DateTime LastUpdate { get; set; }
    }

}