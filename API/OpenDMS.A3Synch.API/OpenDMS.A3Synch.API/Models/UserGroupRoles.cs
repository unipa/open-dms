using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A3Synch.Models
{
    public class UserGroupRoles
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(64)]
        public string? UserGroupId { get; set; }

        [StringLength(64)]
        public string UserId { get; set; }

        public int StartISODate { get; set; }

        public int EndISODate { get; set; }
        [StringLength(64)]
        public string RoleId { get; set; }
        
    }

}