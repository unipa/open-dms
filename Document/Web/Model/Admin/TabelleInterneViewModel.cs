using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using OpenDMS.Domain.Entities.Settings;
using System.ComponentModel.DataAnnotations;

namespace Web.Model.Admin
{
    public class TabelleInterneViewModel
    {
        [ValidateNever]
        public List<LookupTable> Tables { get; set; } = new List<LookupTable>();
        [ValidateNever]
        public List<LookupTable> LookupTables { get; set; } = new List<LookupTable>();
        [Required(ErrorMessage ="Il campo TableId non può essere vuoto.")]
        public string TableId { get; set; }
        [ValidateNever]
        public string Id { get; set; }
        [ValidateNever]
        public string Description { get; set; }
        [ValidateNever]
        public string Annotation { get; set; }

        [ValidateNever]
        public string ErrorMessage { get; set; }
        [ValidateNever]
        public string SuccessMessage { get; set; }
        [ValidateNever]
        public string Icon { get; set; }

    }
}
