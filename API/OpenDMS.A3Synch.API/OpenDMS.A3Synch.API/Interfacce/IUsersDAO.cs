using A3Synch.BL;
using A3Synch.Models;
using OpenDMS.Domain.Entities;

namespace A3Synch.Interfacce
{
    public interface IUsersDAO
    {
        //Task<int> UpdateUsers(List<Users> users);
        Task<int> Update(Contacts contact, bool isKeycloakBL = false);
        Task<int> Add(Users user, Contacts contact, bool isKeycloakBL = false);
        Task<int> Add(Contacts contact, bool isKeycloakBL = false);

        Task<List<Users>> GetAllUsers();
        //Task<string> GetContactId(string id);
        Task<Contacts?> GetContact(string id);

        Task<Users?> GetUser(string id);
    }
}
