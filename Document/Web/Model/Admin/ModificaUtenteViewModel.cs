using System.ComponentModel.DataAnnotations;

namespace Web.Model.Admin
{
    public class ModificaUtenteViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Il Nome è obbligatorio")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Il Cognome è obbligatorio")]
        public string SurName { get; set; }

        [EmailAddress(ErrorMessage = "Inserisci un indirizzo email valido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Il Codice Fiscale è obbligatorio")]
        public string FiscalCode { get; set; }

        [Required(ErrorMessage = "Il Sesso è obbligatorio")]
        public string Sex { get; set; }

        public string? Country { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }
        // Aggiungere altre proprietà necessarie
    }
}
