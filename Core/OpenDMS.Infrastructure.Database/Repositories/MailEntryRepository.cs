using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;


namespace OpenDMS.Infrastructure.Repositories
{
    public class MailEntryRepository : IMailEntryRepository
    {
        private readonly ApplicationDbContext ds;
        private readonly IApplicationDbContextFactory contextFactory;
        public MailEntryRepository(IApplicationDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
            this.ds = (ApplicationDbContext)contextFactory.GetDbContext();
        }
        public async Task<MailEntry> GetById(int mailEntryId)
        {
            return await ds.MailEntries.AsNoTracking().FirstOrDefaultAsync(c => c.Id == mailEntryId);
        }
        public async Task<MailEntry> GetByDocumentId(int documentId)
        {
            return await ds.MailEntries.AsNoTracking().FirstOrDefaultAsync(c => c.DocumentId == documentId);
        }

        public async Task<MailEntry> GetByMessageId(string messageId, int mailboxId)
        {
            return await ds.MailEntries.AsNoTracking().FirstOrDefaultAsync(c => c.MessageUID.ToLower() == messageId.ToLower() && c.MailboxId== mailboxId);
        }
        public async Task<List<MailEntry>> GetMessagesById(string messageId)
        {
            return await ds.MailEntries.AsNoTracking().Where(c => c.MessageUID.ToLower() == messageId.ToLower()).ToListAsync();
        }

        public async Task<List<MailEntry>> GetByParentId(int mailEntryId)
        {
            return await ds.MailEntries.AsNoTracking().Where(c => c.ParentId == mailEntryId).ToListAsync();
        }

        public async Task<int> Insert(MailEntry mailEntry)
        {
            try
            {
                ds.MailEntries.Add(mailEntry);
                var r = await ds.SaveChangesAsync();
                return r;
            }
            finally
            {
                ds.Entry<MailEntry>(mailEntry).State = EntityState.Detached;
            }
        }
        public async Task<int> Count(MailStatus status, MailDirection direction)
        {
            return await ds.MailEntries.AsNoTracking().Where(m => m.Status == status && m.Direction == direction).CountAsync();
        }

        public async Task<DateTime> GetLastReceived(int mailboxId)
        {
            var e = await ds.MailEntries.AsNoTracking().Where(e=>e.Direction == MailDirection.Inbound && e.MailboxId == mailboxId).OrderByDescending (e=>e.MessageDate).FirstOrDefaultAsync();
            return e != null ? e.MessageDate.HasValue ? e.MessageDate.Value : DateTime.MinValue : DateTime.MinValue;
        }
        public async Task<List<MailEntry>> GetMessagesToDelete(int mailboxId, string folderName, int GracePeriod)
        {
            var e = await ds.MailEntries.AsNoTracking().Where(e =>
                e.MailboxId == mailboxId &&
                e.IMAPFolder == folderName &&
                (
                    (e.Status == MailStatus.Deleted && e.DeletionDate.HasValue && e.DeletionDate.Value.AddDays(GracePeriod) < DateTime.UtcNow) 
                    ||
                    (e.DocumentId > 0 && e.ClaimDate.HasValue && e.ClaimDate.Value.AddDays(GracePeriod) < DateTime.UtcNow)
                )
                ).ToListAsync();
            return e;
        }


        public async Task<List<MailEntry>> GetMailEntries(MailMessagesFilter messageFilter)
        {
            IQueryable<MailEntry> mailEntries = ds.MailEntries.AsQueryable<MailEntry>();
            if (messageFilter != null)
            {
                //PER LO SPOOLER NON SI CONSIDERA IL MAILADDRESS NEL FILTRAGGIO
                if (!String.IsNullOrEmpty(messageFilter.internalAddress))
                    mailEntries = mailEntries.Where(m => m.InternalMailAddress == messageFilter.internalAddress);

                if (messageFilter.mailboxId.Length >0)
                {
                    mailEntries = mailEntries.Where(m => messageFilter.mailboxId.Contains(m.MailboxId));
                }

                if (messageFilter.mailDirection != null)
                {
                    mailEntries = mailEntries.Where(m => m.Direction == messageFilter.mailDirection);
                }
                if (messageFilter.mailStatus != null)
                    mailEntries = mailEntries.Where(m => m.Status == messageFilter.mailStatus);

                if (messageFilter.dateFrom != null)
                    mailEntries = mailEntries.Where(m => m.MessageDate > messageFilter.dateFrom);

                if (messageFilter.dateTo != null)
                    mailEntries = mailEntries.Where(m => m.MessageDate < messageFilter.dateTo);

                if (!String.IsNullOrEmpty(messageFilter.externalAddress))
                    mailEntries = mailEntries.Where(m => m.ExternalMailAddress == messageFilter.externalAddress);

                if (!String.IsNullOrEmpty(messageFilter.descriptionText))
                    mailEntries = mailEntries.Where(m => (m.ExternalMailAddress.Contains(messageFilter.descriptionText)) || (m.InternalMailAddress.Contains(messageFilter.descriptionText)) || (m.MessageTitle.Contains(messageFilter.descriptionText)));
            }
            mailEntries = mailEntries.OrderByDescending(m=>m.Id);
            int skip = messageFilter.pageIndex * messageFilter.pageSize;
            int take = messageFilter.pageSize;
            if (skip > 0) mailEntries = mailEntries.Skip(skip);
            if (take > 0) mailEntries = mailEntries.Take(take);
            return mailEntries.AsNoTracking().ToList();
        }

        public async Task<int> Count(MailMessagesFilter messageFilter)
        {
            IQueryable<MailEntry> mailEntries = ds.MailEntries.AsQueryable<MailEntry>();
            if (messageFilter != null)
            {
                //PER LO SPOOLER NON SI CONSIDERA IL MAILADDRESS NEL FILTRAGGIO
                if (!String.IsNullOrEmpty(messageFilter.internalAddress))
                {
                    if (messageFilter.internalAddress.Contains(','))
                        mailEntries = mailEntries.Where(m => messageFilter.internalAddress.Split(',',StringSplitOptions.RemoveEmptyEntries).Contains( m.InternalMailAddress));
                    else
                        mailEntries = mailEntries.Where(m => m.InternalMailAddress == messageFilter.internalAddress);

                }

                if (messageFilter.mailboxId.Length > 0)
                {
                    mailEntries = mailEntries.Where(m => messageFilter.mailboxId.Contains( m.MailboxId ));
                }

                if (messageFilter.mailDirection != null)
                {
                    mailEntries = mailEntries.Where(m => m.Direction == messageFilter.mailDirection);
                }
                if (messageFilter.mailStatus != null)
                    mailEntries = mailEntries.Where(m => m.Status == messageFilter.mailStatus);

                if (messageFilter.dateFrom != null)
                    mailEntries = mailEntries.Where(m => m.MessageDate > messageFilter.dateFrom);

                if (messageFilter.dateTo != null)
                    mailEntries = mailEntries.Where(m => m.MessageDate < messageFilter.dateTo);

                if (!String.IsNullOrEmpty(messageFilter.externalAddress))
                    mailEntries = mailEntries.Where(m => m.ExternalMailAddress == messageFilter.externalAddress);

                if (!String.IsNullOrEmpty(messageFilter.descriptionText))
                    mailEntries = mailEntries.Where(m => (m.ExternalMailAddress.Contains(messageFilter.descriptionText)) || (m.MessageTitle.Contains(messageFilter.descriptionText)));
            }
            return mailEntries.AsNoTracking().Count();
        }

        public async Task<int> Update(MailEntry mailEntry)
        {
            try { 
                ds.MailEntries.Update(mailEntry);
                var r = await ds.SaveChangesAsync();
                return r;
            }
            finally
            {
                ds.Entry<MailEntry>(mailEntry).State = EntityState.Detached;
            }
        }
        public async Task<int> Delete(int mailentryId)
        {
            var c = await ds.MailEntries.AsNoTracking().FirstOrDefaultAsync(c => c.Id == mailentryId);
            try
            {
                ds.MailEntries.Remove(c);
                var r = await ds.SaveChangesAsync();
                return r;
            }   
            finally
            {
                ds.Entry<MailEntry>(c).State = EntityState.Detached;
            }
        }

        public async Task UnlockFreezed()
        {
            await ds.MailEntries.AsNoTracking()
                .Where(w => !string.IsNullOrEmpty(w.WorkerId) && w.LastRunningUpdate < DateTime.UtcNow.AddMinutes(-1))
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.WorkerId, ""));
        }


        public async Task<bool> Take (int id, string workerId, DateTime ExpirationDate)
        {
            var u = await ds.MailEntries
                .AsNoTracking()
                .Where(w => (string.IsNullOrEmpty(w.WorkerId) || w.WorkerId == workerId) && w.Id == id)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.WorkerId, workerId).SetProperty(p => p.LastRunningUpdate, ExpirationDate));
            return (u > 0);
        }
        public async Task<MailEntry> TakeNext(string workerId, DateTime ExpirationDate)
        {
            await UnlockFreezed();
            bool taken = false;
            MailEntry e = null;
            bool found = false;
            while (!taken)
            {
                e = await ds.MailEntries.AsNoTracking().OrderBy(o=>o.MessageDate).ThenBy(o=>o.Id)
                    .FirstOrDefaultAsync(w => (string.IsNullOrEmpty(w.WorkerId) || w.WorkerId == workerId)
                        && (w.MessageDate <= DateTime.UtcNow)
                        && (w.Status == MailStatus.Sending || (w.Status == MailStatus.Failed && w.RetryValue > 0))
                        );
                found = e != null;
                if (found)
                    taken = (await Take(e.Id, workerId, ExpirationDate));
                else
                    taken = true;
            }
            return found ? e : null; 
        }

        public async Task<int> Release (int id)
        {
            return await ds.MailEntries.AsNoTracking()
                .Where(w => !string.IsNullOrEmpty(w.WorkerId) && w.Id == id)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.WorkerId, ""));
        }
    }
}
