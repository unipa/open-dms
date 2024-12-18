using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RemoteSignInfocert.Interfaces;
using RemoteSignInfocert.Models;

namespace RemoteSignInfocert.Pages
{
    [IgnoreAntiforgeryToken]
    public class EditUserModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUserDAO _dao;
        public UserModel? Utente { get; set; }
        public string SuccessMessage { get; set; } = "";
        public string ErrorMessage { get; set; } = "";
        public EditUserModel(ILogger<IndexModel> logger, IUserDAO dao)
        {
            _logger = logger;
            _dao = dao;
        }

        public void OnGet(string UserName)
        {
            if (UserName == null) this.ErrorMessage = "Il parametro UserName non può essere vuoto.";
            Utente = _dao.GetDatiUtenteFromUserName(UserName);
            if (Utente == null)
            {
                Utente = new UserModel();
                Utente.UserName = UserName;
            }
            this.ErrorMessage = ErrorMessage;
            this.SuccessMessage = SuccessMessage;
        }

        public void OnPost(UserModel user)
        {
            try
            {
                user.Alias = user.Alias.Trim();
                user.UserName= user.UserName.Trim();
                _dao.SaveDatiUtente(user);
                Utente = user;
                this.SuccessMessage = "Dati salvati correttamente.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Si è verificato un errore durante il salvataggio: " + ex);
                this.ErrorMessage = "Si è verificato i lseguente errore: " + ex.Message;
                Utente = new UserModel();
                Utente.UserName = (user.UserName ?? null);
            }
        }

    }

}
