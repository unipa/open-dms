using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenDMS.Domain.Entities.Settings;
using System.ComponentModel.DataAnnotations;

namespace Web.Model.Admin
{
    public class TemplateNotificheViewModel
    {
        [ValidateNever]
        public string MinWarning { get; set; } = "0";
        [ValidateNever]
        public string MinError { get; set; } = "0";
        [ValidateNever]
        public string Expiration{ get; set; } = "0";


        [ValidateNever]
        public List<Company> Companies { get; set; } = new List<Company> { };

        [ValidateNever]
        public string Template { get; set; } = string.Empty;

        [ValidateNever]
        public int CompanyId { get; set; } = 0;

        [ValidateNever]
        public string Title { get; set; } = string.Empty;

        [ValidateNever]
        public string Body { get; set; } = string.Empty;

        [ValidateNever]
        public string ErrorMessage { get; set; }
        [ValidateNever]
        public string SuccessMessage { get; set; }
        [ValidateNever]
        public string Icon { get; set; }

        [ValidateNever]
        public string? CompaniesText
        {
            get
            {
                return ElencoCompanies.FirstOrDefault(p => p.Value == Companies.ToString())?.Text;
            }
        }
        [ValidateNever]
        public List<SelectListItem> ElencoCompanies
        {
            get
            {
                var list = new List<SelectListItem>();
                list.Add(new SelectListItem() { Value = "0", Text = "Tutte" });
                list.AddRange(Companies.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Description }).ToList());
                return list;
            }
        }

    }

}
