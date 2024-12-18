using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Core.DTOs
{
    public class PermissionAuthorization
    {
        public string permissionId { get; set; }
        public AuthorizationType Authorization { get; set; }

    }
}
