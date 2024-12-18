using A3Synch.Models;

namespace A3Synch.Interfacce
{
    public interface IContactsDAO
    {
        Task<int> UpdateContacts(List<Contacts> contacts);
        Task<List<Contacts>> GetAllContacts();
        Task<string> GetContactsId(string fiscalCode);
    }
}
