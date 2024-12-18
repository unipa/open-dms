using OpenDMS.Domain.Entities.Mails;

namespace OpenDMS.Domain.Repositories;


/// <summary>
/// Descrizione di riepilogo per MailServerDAO
/// </summary>
public interface IMailServerRepository
{
    Task<MailServer> GetById(int id);
    Task<MailServer> GetByDomain(string MailDomain);

    Task<int> Insert(MailServer bd);
    Task<int> Update(MailServer bd);
    Task<int> Delete(int id);
    Task<IList<MailServer>> GetAll();

}


