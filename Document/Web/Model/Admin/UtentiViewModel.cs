using OpenDMS.Domain.Entities.Users;

namespace Web.Model.Admin
{
    public class UtentiViewModel
    {
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public string Icon { get; set; }
        public List<User> utenti { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
