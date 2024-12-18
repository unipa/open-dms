using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using OpenDMS.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Web.Model.Admin
{
    public class OrganigrammaViewModel
    {
        public List<OrganizationNodeTree> Tree { get; set; }
        public int StartISODate { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public string Icon { get; set; }
//        public string host { get; set; }
//        public string ApiHost { get; set; }
//        public string token { get; set; }
    }

    public class MoveOrganizationNode_DTO
    {
        [Required]
        public string userGroupId { get; set; } = "";
        [Required(ErrorMessage = "Il campo NewParentUserGroupId è obbligatorio.")]
        public string NewParentUserGroupId { get; set; } = "";
        [Required(ErrorMessage = "Il campo StartDate è obbligatorio.")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Il campo EndDate è obbligatorio.")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Modalità di chiusura del nodo
        /// 1. Riassegnazione Tasks al nodo padre
        /// 2. Riassegnazione Tasks ad un nodo specifico (ClosingProfile)
        /// 3. Riassegnazione Tasks ad un ruolo specifico (ClosingProfile)
        /// 4. Completamento Automatico dei Tasks
        /// </summary>
        [Required(ErrorMessage = "Il campo TaskReallocationStrategy è obbligatorio.")]
        public int TaskReallocationStrategy { get; set; }
        [ValidateNever]
        public string TaskReallocationProfile { get; set; }

    }

    public class UserInGroup_DTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserGroupId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string RoleId { get; set; }
        [Required]
        public string RoleName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class CreateOrUpdateOrganizationNode_DTO
    {
        [Required]
        public string? ParentUserGroupId { get; set; }
        [Required]
        public string ShortName { get; set; } = "";
        [Required]
        public string Name { get; set; } = "";
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [ValidateNever]
        public bool Visible { get; set; }

        /// <summary>
        /// Modalità di chiusura del nodo
        /// 1. Riassegnazione Tasks al nodo padre
        /// 2. Riassegnazione Tasks ad un nodo specifico (ClosingProfile)
        /// 3. Riassegnazione Tasks ad un ruolo specifico (ClosingProfile)
        /// 4. Completamento Automatico dei Tasks
        /// </summary>
        [Required]
        public int TaskReallocationStrategy { get; set; }
        [ValidateNever]
        public string TaskReallocationProfile { get; set; }
        [ValidateNever]
        public string ExternalId { get; set; }

        /// <summary>
        /// Modalità di notifica dei task
        /// 1. Utenti abilitati al ruolo "ViewInbox"
        /// 2. Utenti con ruolo specifico (NotificationProfile)
        /// 3. Tutti gli utenti del gruppo
        /// 4. Sotto-strutture
        /// </summary>
        [Required]
        public int NotificationStrategy { get; set; }
        [ValidateNever]
        public string NotificationProfile { get; set; }

        /// <summary>
        /// Modalità di notifica CC dei task
        /// 1. Utenti abilitati al ruolo "ViewInbox"
        /// 2. Utenti con ruolo specifico (NotificationProfile)
        /// 3. Tutti gli utenti del gruppo
        /// 4. Tutti gli utenti e sotto-strutture
        [Required]
        public int NotificationStrategyCC { get; set; }
        [ValidateNever]
        public string NotificationProfileCC { get; set; }

    }

}
