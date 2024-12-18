using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.DTOs
{
    public class UserDetails_DTO
    {
        public string FullName { get; set; } = "";
        //public string Homepage { get; set; } = "";    
        public string? SurName { get; set; } = "";
        public string? FirstName { get; set; } = "";
        public string? BirthDate { get; set; } = "";
        public string? FiscalCode{ get; set; } = "";
        public string? LicTradNum { get; set; } = "";
        public string? IdentityDocumentType { get; set; } = "";
        public string? IdentityDocument { get; set; } = "";
        public string? IdentityDocumentId { get; set; } = "";
        public string? Country { get; set; } = "";
        public string? NotificationMailAddress { get; set; } = "";
        public string? NotificationType { get; set; } = "";
        public string? Notification { get; set; } = "";
        public string? GroupsAndRoles { get; set; } = "";
        public string? Groups { get; set; } = "";
        public string? Roles { get; set; } = "";
        public List<ACLPermission_DTO> Permessi { get; set; } = new List<ACLPermission_DTO>();
        public List<SelectListItem> EmailAddresses { get; set; } = new();
        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";
        public string? Icon { get; set; } = "";

        public Dictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();
    }
}
