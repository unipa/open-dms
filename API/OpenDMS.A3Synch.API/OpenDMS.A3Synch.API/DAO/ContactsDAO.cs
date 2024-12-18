using Microsoft.EntityFrameworkCore;
using A3Synch.Interfacce;
using A3Synch.Models;
using OpenDMS.A3Synch.API.Utility;

namespace A3Synch.DAO
{
    //public class ContactsDAO : IContactsDAO
    //{
    //    private readonly Interfacce.IContext _ctx;
    //    private readonly ILogger<ContactsDAO> _logger;

    //    public ContactsDAO(Interfacce.IContext ctx, ILogger<ContactsDAO> logger)
    //    {
    //        _ctx = ctx;
    //        _logger = logger;
    //    }
    //    public async Task<int> SaveContact(Contacts contact, bool add = true)
    //    {
    //        try
    //        {

    //                if (existingContact != null)
    //                {
    //                // Aggiorna i campi dell'oggetto esistente
    //                contact.ParentId = contact.ParentId;
    //                contact.ContactType = contact.ContactType;
    //                contact.FriendlyName = contact.FriendlyName;
    //                contact.SearchName = contact.SearchName;
    //                contact.CreationUser = contact.CreationUser;
    //                contact.LastUpdate = contact.LastUpdate;
    //                contact.UpdateErrors = contact.UpdateErrors;
    //                contact.Annotation = contact.Annotation;

    //                    SharedVariables.elaborated_contacts_counter++;
    //                }
    //                else
    //                {
    //                    _ctx.Contacts.Add(contact);
    //                    SharedVariables.elaborated_contacts_counter++;
    //                }
    //            }

    //            await _ctx.SaveChangesAsync();

    //            // Detach tutti gli oggetti
    //            foreach (var contact in contacts)
    //            {
    //                _ctx.Entry(contact).State = EntityState.Detached;
    //            }

    //            return 1;
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError("Errore in ContactsDAO, errore: " + ex.Message);
    //            throw new Exception("Errore in ContactsDAO, errore: " + ex.Message);
    //        }
    //    }

    //    //Funzione che prende in input una lista di Contacts e li inserisce nel DB se non esistono, altrimenti aggiorna i record presenti.
    //    public async Task<int> UpdateContacts(List<Contacts> contacts)
    //    {
    //        try
    //        {
                
    //            SharedVariables.total_contacts_counter += contacts.Count;
    //            foreach (var contact in contacts)
    //            {
    //                // Controlla se esiste un record con lo stesso id nel database
    //                var existingContact = await _ctx.Contacts.SingleOrDefaultAsync(c => c.FiscalCode == contact.FiscalCode);


    //                if (existingContact != null)
    //                {
    //                    // Aggiorna i campi dell'oggetto esistente
    //                    existingContact.ParentId = contact.ParentId;
    //                    existingContact.ContactType = contact.ContactType;
    //                    existingContact.FriendlyName = contact.FriendlyName;
    //                    existingContact.SearchName = contact.SearchName;
    //                    existingContact.CreationUser = contact.CreationUser;
    //                    existingContact.LastUpdate = contact.LastUpdate;
    //                    existingContact.UpdateErrors = contact.UpdateErrors;
    //                    existingContact.Annotation = contact.Annotation;

    //                    SharedVariables.elaborated_contacts_counter++;
    //                }
    //                else
    //                {
    //                    _ctx.Contacts.Add(contact);
    //                    SharedVariables.elaborated_contacts_counter++;
    //                }
    //            }

    //            await _ctx.SaveChangesAsync();

    //            // Detach tutti gli oggetti
    //            foreach (var contact in contacts)
    //            {
    //                _ctx.Entry(contact).State = EntityState.Detached;
    //            }

    //            return 1;
    //        }
    //        catch(Exception ex)
    //        {
    //            _logger.LogError("Errore in ContactsDAO, errore: " + ex.Message);
    //            throw new Exception("Errore in ContactsDAO, errore: " + ex.Message);
    //        }
    //    }


    //    public async Task<List<Contacts>> GetAllContacts()
    //    {
    //        return await _ctx.Contacts.ToListAsync();
    //    }


    //    //Funzione che dato in input un codice fiscale mi ritorna il contactID
    //    public async Task<string> GetContactsId(string fiscalCode)
    //    {
    //        try
    //        {
    //            var contact = await _ctx.Contacts.FirstOrDefaultAsync(c => c.FiscalCode == fiscalCode);
    //            return contact?.Id;
    //        }
    //        catch(Exception ex)
    //        {
    //            _logger.LogError("Errore in ContactsDAO, errore: " + ex.Message);
    //            throw new Exception("Errore in ContactsDAO, errore: " + ex.Message);
    //        }

    //    }
    //}
}
