using OpenDMS.Domain.Entities.Users;

namespace OpenDMS.Domain.Repositories;


public interface IContactRepository
{
    Task Insert(Contact businessPartner);
    Task Update(Contact businessPartner);
    Task Delete(Contact businessPartner);
    Task Restore(Contact businessPartner);


    Task<Contact> GetById(int partnerId);
    Task<Contact> GetByEmail(string displayName, string email);
    Task<Contact> GetByPartitaIVA(string partitaIVA, int ignoreThisPartnerId = 0);
    Task<Contact> GetByCodiceFiscale(string codiceFiscale, int ignoreThisPartnerId = 0);
    Task<Contact> GetByCodiceIPA(string codiceIPA);
    Task<Contact> GetByExternalCode(string externalSource, string externalCode, int ignoreThisPartnerId = 0);

    Task<bool> IsValid(int partnerId);

    Task<List<Contact>> SearchByEmail(string email);


    Task<IEnumerable<Contact>> GetContacts(int partnerId);
    Task<IEnumerable<Contact>> GetAddresses(int partnerId);
    Task<IEnumerable<Contact>> GetSubCompanies(int partnerId);
    Task<IEnumerable<ExternalContactIdentifier>> GetExternalSources(int partnerId);


    Task SetMailDomain(string emaildomain, int partnerId, string utente);
    Task ClearDomain(string emaildomain);
    Task<int> GetPartnerByDomain(string emailDomain);

    Task AddOrUpdateRiferimentoAnagraficaEsterna(int partnerId, string IdAnagrafica, string CodiceInAnagrafica, string utente);


}

