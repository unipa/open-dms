using A3Synch.DAO;
using A3Synch.Interfacce;
using A3Synch.Models;
using OpenDMS.A3Synch.API.Utility;

namespace A3Synch.BL
{
    public class UsersBL : IUsersBL
    {
        private readonly IUsersDAO _usersdao;
        private readonly IUtils _utils;
        

        public UsersBL( IUtils utils,IUsersDAO usersdao)
        {
            _usersdao = usersdao;
            _utils = utils;
        }

        public async Task<int> SynchUsersInDb(List<Members> all_members,bool isKeycloakBL = false)
        {
            //all_members = all_members.DistinctBy(x => new { x.codice_fiscale }).ToList();
            List<Users> users = new List<Users>();
            List<Contacts> contacts = new List<Contacts>();
            if (!isKeycloakBL)
            {
                SharedVariables.total_users_counter = all_members.Count;
            }
            foreach (var member in all_members)
            {
                var user = await _usersdao.GetUser (member.username);
                if (user == null)
                {
                    Contacts contact = new Contacts();
                    var fullname = member.denominazione;
                    if (string.IsNullOrEmpty(fullname))
                        fullname = member.nome + " " + member.cognome;
                    contact.FullName = fullname;
                    contact.FriendlyName = contact.FullName;
                    contact.SearchName = string.IsNullOrEmpty(contact.FullName) ? " " : contact.FullName.Trim().Replace(".", "").Replace("-", "").Replace(",", "").Replace("'", "").Replace("\"", "");
                    contact.FiscalCode = member.codice_fiscale;
                    contact.Sex = member.sesso;
                    contact.SurName = member.cognome;
                    contact.FirstName = member.nome;
                    contact.CreationDate = DateTime.UtcNow;
                    contact.LastUpdate = DateTime.UtcNow;
                    
                    user = new Users();
                    user.Id = member.username;
                    user.ContactId = contact.Id;
                    user.CreationDate = DateTime.UtcNow;
                    user.LastUpdate = DateTime.UtcNow;
                    await _usersdao.Add(user, contact, isKeycloakBL);
                } else
                {
                    var contact = await _usersdao.GetContact(user.ContactId);
                    if (contact.Annotation == "ERROR")
                    {
                        Contacts new_contact = new Contacts();
                        var fullname = member.denominazione;
                        if (string.IsNullOrEmpty(fullname))
                        new_contact.FullName = fullname;
                        new_contact.FriendlyName = contact.FullName;
                        new_contact.SearchName = string.IsNullOrEmpty(contact.FullName) ? " " : contact.FullName.Trim().Replace(".", "").Replace("-", "").Replace(",", "").Replace("'", "").Replace("\"", "");
                        new_contact.FiscalCode = member.codice_fiscale;
                        new_contact.Sex = member.sesso;
                        new_contact.SurName = member.cognome;
                        new_contact.FirstName = member.nome;
                        new_contact.CreationDate = DateTime.UtcNow;
                        new_contact.LastUpdate = DateTime.UtcNow;
                        await _usersdao.Add(new_contact, isKeycloakBL);
                    }
                    else
                    {
                        var fullname = member.nome + " " + member.cognome;
                        contact.FullName = fullname;
                        contact.FriendlyName = contact.FullName;
                        contact.SearchName = string.IsNullOrEmpty(contact.FullName) ? " " : contact.FullName.Trim().Replace(".", "").Replace("-", "").Replace(",", "").Replace("'", "").Replace("\"", "");
                        contact.FiscalCode = member.codice_fiscale;
                        contact.Sex = member.sesso;
                        contact.SurName = member.cognome;
                        contact.FirstName = member.nome;
                        contact.LastUpdate = DateTime.UtcNow;
                        await _usersdao.Update(contact, isKeycloakBL);
                    }
                    
                }
                users.Add(user);

            }
            return 1;
        }

        public void ResetStatus()
        {
            SharedVariables.elaborated_users_counter = 0;
            SharedVariables.total_users_counter = 0;
        }

        public async Task<List<Users>> GetAllUsers()
        {
            return await _usersdao.GetAllUsers();
        }

    }
}
