using Microsoft.AspNetCore.Mvc.Rendering;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;

namespace Web.Model
{
    public class SharePanelViewModel
    {
        public string? Type { get; set; } = "";
        public List<SelectListItem> Actions { get; set; } = new();
        public string ErrorMessage { get; set; } = "";
        public string DocId { get; internal set; }
        public List<Company> Companies { get; set; } = new List<Company>();
        public int CompanyId{ get; set; }

        public string Message { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Action { get; set; }
        public string Selected { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int ParentTaskId { get; set; }
    }
}


