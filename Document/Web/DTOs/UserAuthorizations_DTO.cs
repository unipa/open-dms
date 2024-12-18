using Microsoft.AspNetCore.Mvc.Rendering;
using OpenDMS.Domain.Entities.Settings;

namespace Web.DTOs
{
    public class UserAuthorizations_DTO
    {
        public List<LookupTable> Groups { get; set; } = new();
        public List<LookupTable> Roles { get; set; } = new ();
        public List<ACLPermission_DTO> Permessi { get; set; } = new List<ACLPermission_DTO>();

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
    }
}
