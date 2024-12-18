using OpenDMS.Domain.Entities.Mails;

namespace OpenDMS.Infrastructure.Repositories
{
    public interface IMailboxRepository
    {
        Task<int> Delete(Mailbox mailbox);
        Task<IList<Mailbox>> GetAll(string userId);
        Task<Mailbox> GetById(int MailboxId);
        Task<Mailbox> GetByAddress(string Address);
        Task<int> Insert(Mailbox mailbox);
        Task<int> Update(Mailbox mailbox);
        Task<Mailbox> TakeNext(string WorkerId, DateTime NextUpdate);
        Task UnlockFreezed();

    }
}