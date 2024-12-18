using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Models;

namespace OpenDMS.Core.Interfaces
{
    public interface IMailboxService
    {
        Task<int> Create(Mailbox mailbox);
        Task<int> Delete(int mailboxId);
        Task<IList<Mailbox>> GetAll(UserProfile u);
        Task<Mailbox> GetById(int MailboxId);
        Task<Mailbox> GetByAddress(string Address);
        Task<int> Update(Mailbox mailbox);
        Task<Mailbox> TakeNext(string WorkerId, DateTime NextUpdate);
        Task Release(Mailbox mailbox);

    }
}