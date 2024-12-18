using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Models;

namespace OpenDMS.Domain.Repositories;

public interface IMailEntryRepository
{
    Task<int> Delete(int mailentryId);
    Task<MailEntry> GetById(int mailEntryId);
    Task<MailEntry> GetByDocumentId(int documentId);

    Task<MailEntry> GetByMessageId(string messageId, int mailboxId);
    Task<List<MailEntry>> GetMessagesById(string messageId);

    Task<List<MailEntry>> GetByParentId(int mailEntryId);
    Task<int> Insert(MailEntry mailEntry);
    Task<int> Update(MailEntry mailEntry);
    Task<int> Count (MailStatus status, MailDirection direction);
    Task<int> Count(MailMessagesFilter messageFilter);

    Task<List<MailEntry>> GetMailEntries(MailMessagesFilter messageFilter);
    Task UnlockFreezed();
    Task<bool> Take(int id, string workerId, DateTime ExpirationDate);
    Task<MailEntry> TakeNext(string workerId, DateTime ExpirationDate);
    Task<int> Release(int id);
    Task<DateTime> GetLastReceived(int mailboxId);
    Task<List<MailEntry>> GetMessagesToDelete(int mailboxId, string folderName, int GracePeriod);
}