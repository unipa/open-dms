using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;

namespace Web.DTOs
{
    public class _ACLPermission_DTO
    {
        public string ACLId { get; set; } = "";

        public string ProfileName { get; set; }
        public string ProfileId { get; set; }
        public ProfileType ProfileType { get; set; }

        //[StringLength(64)]
        public string PermissionId { get; set; }
        public string PermissionName { get; set; }

        public AuthorizationType Authorization { get; set; }

    }
}
