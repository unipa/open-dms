using System.ComponentModel.DataAnnotations;

namespace A3Synch.Models
{
    public class RolesKC
    {
        [StringLength(64)]
        public string id { get; set; }

        [StringLength(255)]
        public string name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdate { get; set; }

    }

}