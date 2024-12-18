using OpenDMS.Domain.Entities.Users;

namespace Web.Model.Admin
{
    public class DettaglioUtenteViewModel
    {
        public User? utente { get; set;}
        public string? BirthDate { get; set;}
        public string? Country { get; set;}
        public string? ErrorMessage { get; set;}
        public string? SuccessMessage { get; set;}
    }
}
