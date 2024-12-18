using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using OpenDMS.Domain.Entities.Schemas;
using System.ComponentModel.DataAnnotations;
using Web.DTOs;

namespace Web.Model.Admin
{
    public class PermessiViewModel
    {
        [ValidateNever]
        public List<ACL> ACLs { get; set; }

        [StringLength(64)]
        [RegularExpression("^[\\w-.@!]+$", ErrorMessage = "L'ACLId può contenere solo lettere e numeri e i caratteri speciali '_' , '-' , '.' , '@' e '!'.")]

        [ValidateNever]
        public string Id { get; set; }
        [StringLength(128)]
        [RegularExpression("^[\\w-.@!]+$", ErrorMessage = "Il nome può contenere solo lettere e numeri e i caratteri speciali '_' , '-' , '.' , '@' e '!'.")]
        public string Name { get; set; }
        [ValidateNever]
        public string Description { get; set; }
        [ValidateNever]
        public DateTime CreationDate { get; set; }
        [ValidateNever]
        public IList<ACLPermission_DTO> Authorizations { get; set; } = new List<ACLPermission_DTO>();

        [ValidateNever]
        public string ErrorMessage { get; set; }
        [ValidateNever]
        public string SuccessMessage { get; set; }
        [ValidateNever]
        public string Icon { get; set; }
        [ValidateNever]
        public string ProfileId { get; set; } //solo per trasportare il model state error dall'action 'AggiungiAutorizzazione' alla view principale
    }

}
