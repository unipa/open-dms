using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Web.Model.Admin
{
    public class BancaDatiViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Il campo Descrizione non può essere nullo."),RegularExpression("^[\\w -.@!]+$", ErrorMessage = "La descrizione può contenere solo lettere, numeri, spazie e i caratteri speciali '_' , '-' , '.' , '@' e '!'.")]
        public string? Description { get; set; } = "";
        [RegularExpression("^[\\w-.@!]+$", ErrorMessage = "Il tema può contenere solo lettere e numeri e i caratteri speciali '_' , '-' , '.' , '@' e '!'.")]
        public string? Theme { get; set; } = "";
        public string? Logo { get; set; } = "";
        [RegularExpression("^[\\w-.@!]+$", ErrorMessage = "La descrizione può contenere solo lettere e numeri e i caratteri speciali '_' , '-' , '.' , '@' e '!'.")]
        public string? ERP { get; set; } = "";
        [RegularExpression("^[\\w-.@!]+$", ErrorMessage = "La descrizione può contenere solo lettere e numeri e i caratteri speciali '_' , '-' , '.' , '@' e '!'.")]
        public string? AOO { get; set; } = "";
        public bool OffLine { get; set; }

        public string Codice { get { return Id.ToString().PadLeft(3, '0'); } }
        [ValidateNever]
        public string ErrorMessage { get; set; }
        [ValidateNever]
        public string SuccessMessage { get; set; }
        [ValidateNever]
        public string Icon { get; set; }


    }
}
