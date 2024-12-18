using A3Synch.DAO;
using A3Synch.Interfacce;
using A3Synch.Models;
using OpenDMS.A3Synch.API.Utility;

namespace A3Synch.BL
{
    //public class ContactsBL : IContactsBL
    //{
        
    //    private readonly IContactsDAO _contactsdao;
    //    private readonly IUtils _utils;
        

    //    public ContactsBL(IUtils utils, IContactsDAO contactsdao)
    //    {
    //        _contactsdao = contactsdao;
    //        _utils = utils;
    //    }

    //    public async Task<int> SynchContactsInDb(List<Members> all_members)
    //    {
    //        //all_members = all_members.DistinctBy(x => new { x.codice_fiscale}).ToList();
    //        List<Contacts> contacts = new List<Contacts>();
    //        foreach (var member in all_members)
    //        {
    //            Contacts contact = new Contacts();
    //            var fullname = member.denominazione;
    //            if (string.IsNullOrEmpty(fullname))
    //                fullname = member.nome + " " + member.cognome;
    //            contact.FullName = fullname;
    //            contact.FriendlyName = contact.FullName;
    //            contact.SearchName = contact.FullName.Trim().Replace(".", "").Replace("-", "").Replace(",", "").Replace("'", "").Replace("\"", "").Replace("", "");
    //            contact.FiscalCode = member.codice_fiscale;
    //            contact.Sex = member.sesso;
    //            contact.SurName = member.cognome;
    //            contact.FirstName = member.nome;
    //            contact.CreationDate = DateTime.UtcNow;
    //            contact.LastUpdate = DateTime.UtcNow;
    //            contacts.Add(contact);

    //        }
    //        int res = await _contactsdao.UpdateContacts(contacts);
    //        return res;
    //    }
    //    public void ResetStatus()
    //    {
    //        SharedVariables.elaborated_contacts_counter = 0;
    //        SharedVariables.total_contacts_counter = 0;
    //    }



    //}
}
