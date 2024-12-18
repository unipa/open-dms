using RemoteSignInfocert.Models;

namespace RemoteSignInfocert.Interfaces
{
    public interface IUserDAO
    {
        UserModel GetDatiUtenteFromUserName(string UserName);
        bool SaveDatiUtente(UserModel model);
    }
}
