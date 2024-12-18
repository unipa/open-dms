using RemoteSignInfocert.Context;
using RemoteSignInfocert.Interfaces;
using RemoteSignInfocert.Models;

namespace RemoteSignInfocert.DAO
{
    public class UserDAO : IUserDAO
    {
        private readonly UserDbContext _ctx;
        public UserDAO(UserDbContext ctx)
        {
            _ctx = ctx;
        }

        public UserModel GetDatiUtenteFromUserName(string UserName)
        {

            return _ctx.Utenti.FirstOrDefault(p => p.UserName == UserName);
            //return new UserModel() { UserName = UserName, Alias = "CLSGNN82C08G273H", NomeCompleto = "Giovanni Calascibetta" };
        }

        public bool SaveDatiUtente(UserModel model)
        {
            var ut = GetDatiUtenteFromUserName(model.UserName);

            if (ut == null)
                _ctx.Add(model);
            else
            {
                ut.Alias = model.Alias;
                ut.NomeCompleto = model.NomeCompleto;
                _ctx.Update(ut);
            }

            _ctx.SaveChanges();
            return true;
        }


    }
}
