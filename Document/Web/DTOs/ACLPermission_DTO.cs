using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenDMS.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;

namespace Web.DTOs
{
    public class ACLPermission_DTO
    {
        public ACLPermission_DTO()
        {
        }

        public ACLPermission_DTO(string profileId, string permissionName, AuthorizationType authorization)
        {
            ProfileId = profileId;
            PermissionName = permissionName;
            Authorization = authorization;
        }

        [ValidateNever]
        public string ACLId { get; set; } = "";
        [Required(ErrorMessage = "Non è stato indicato un utente/gruppo/ruolo a cui assegnare l'autorizzazione.")]
        public string ProfileId { get; set; }
        [ValidateNever]
        public string ProfileName { get; set; }
        [ValidateNever]
        public ProfileType ProfileType { get; set; }
        [ValidateNever]
        public string PermissionId { get; set; }
        [ValidateNever]
        public string PermissionName { get; set; }
        [ValidateNever]
        public AuthorizationType Authorization { get; set; }

        public string? AuthorizationText
        {
            get
            {
                return ElencoAuthorization.FirstOrDefault(p => p.Value == Authorization.ToString())?.Text;
            }
        }
        public List<SelectListItem> ElencoAuthorization { get; } = new List<SelectListItem>
        {
        new SelectListItem { Value = "1", Text = "Concesso" },
        new SelectListItem { Value = "2", Text = "Negato" },
        };
    }
}
