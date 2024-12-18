using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Infrastructure.Repositories
{
    public class MailboxRepository : IMailboxRepository
    {
        private readonly ApplicationDbContext ds;
        private readonly IApplicationDbContextFactory contextFactory;

        public MailboxRepository(IApplicationDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
            this.ds = (ApplicationDbContext)contextFactory.GetDbContext();
        }

        public async Task<IList<Mailbox>> GetAll(string userId)
        {

            return ds.Mailboxes.Include(m => m.MailServer).AsNoTracking().ToList().Where(m => m.UserId == userId || 
            !String.IsNullOrEmpty(m.SendEnabledProfiles) ||
            !String.IsNullOrEmpty(m.ReadOnlyProfiles) ||
            !String.IsNullOrEmpty(m.DraftEnabledProfiles)).ToList();
        }

        public async Task<Mailbox> GetById(int MailboxId)
        {
            return await ds.Mailboxes.Include(m=>m.MailServer).AsNoTracking().FirstOrDefaultAsync(c => c.Id == MailboxId);
        }

        public async Task<Mailbox> GetByAddress(string Address)
        {
            return await ds.Mailboxes.Include(m => m.MailServer).AsNoTracking().FirstOrDefaultAsync(c => c.MailAddress.ToLower() == Address.ToLower());
        }

        public async Task UnlockFreezed()
        {
            await ds.Mailboxes
                .AsNoTracking()
                .Where(w => !string.IsNullOrEmpty(w.ReaderWorkerId) && w.NextReaderDate < DateTime.UtcNow.AddMinutes(-1))
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.ReaderWorkerId, ""));
        }


        public async Task<Mailbox> TakeNext(string workerId, DateTime ExpirationDate)
        {
            await UnlockFreezed();
            bool taken = false;
            Mailbox e = null;
            bool found = false;
            while (!taken)
            {
                e = await ds.Mailboxes.Include(m => m.MailServer).AsNoTracking().OrderBy(o => o.NextReaderDate)
                    .FirstOrDefaultAsync(w => w.EnableDownload && (w.NextReaderDate <= DateTime.UtcNow || (w.NextReaderDate == null)) && (string.IsNullOrEmpty(w.ReaderWorkerId) || w.ReaderWorkerId == workerId));
                found = e != null;
                if (found)
                {
                    var u = await ds.Mailboxes.AsNoTracking()
                        .Where(w => (string.IsNullOrEmpty(w.ReaderWorkerId) || w.ReaderWorkerId == workerId) && w.Id == e.Id)
                        .ExecuteUpdateAsync(s => s.SetProperty(p => p.ReaderWorkerId, workerId).SetProperty(p => p.NextReaderDate, ExpirationDate));
                    taken = u > 0;
                }
                else
                    taken = true;
            }
            return found ? e : null;
        }



        public async Task<int> Insert(Mailbox mailbox)
        {
            var m = mailbox.MailServer;
            mailbox.MailServer = null;
            try
            {
                ds.Mailboxes.Add(mailbox);
                var r = await ds.SaveChangesAsync();
                mailbox.MailServer = m;
                return r;
            }
            finally
            {
                ds.Entry<Mailbox>(mailbox).State = EntityState.Detached;
                ds.Entry<MailServer>(mailbox.MailServer).State = EntityState.Detached;
            }
        }

        public async Task<int> Update(Mailbox mailbox)
        {
            var m = mailbox.MailServer;
            mailbox.MailServer = null;
            try { 
                ds.Mailboxes.Update(mailbox);
                var r = await ds.SaveChangesAsync();
                mailbox.MailServer = m;
                return r;
            }
            finally
            {
                ds.Entry<Mailbox>(mailbox).State = EntityState.Detached;
                ds.Entry<MailServer>(mailbox.MailServer).State = EntityState.Detached;
            }
        }

    public async Task<int> Delete(Mailbox mailbox)
        {
            var m = mailbox.MailServer;
            mailbox.MailServer = null;
            try { 
                ds.Mailboxes.Remove(mailbox);
                var r = await ds.SaveChangesAsync();
                mailbox.MailServer = m;
                return r;
            }
            finally
            {
                ds.Entry<Mailbox>(mailbox).State = EntityState.Detached;
                ds.Entry<MailServer>(mailbox.MailServer).State = EntityState.Detached;
            }
        }
    }


}
