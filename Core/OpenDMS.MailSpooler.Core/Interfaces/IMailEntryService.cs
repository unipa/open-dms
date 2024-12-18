using MimeKit;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Models;
using OpenDMS.MailSpooler.Core.DTOs;

namespace OpenDMS.MailSpooler.Core.Interfaces
{
    public interface IMailEntryService
    {
        Task<MailEntry> Add(string GUID, Mailbox mailbox, string userId, MimeMessage eml, DateTime SendDate, MailDirection Direction, MailStatus Status, string Uidl = "");
        Task<MailEntry> ChangeStatus(int entryId, string userId, MailStatus status, String ErrorMessage = "");
        Task<MailEntry> ChangeStatus(MailEntry entryId, string userId, MailStatus status, String ErrorMessage = "");
        Task<int> Count (MailMessagesFilter filtro, UserProfile userProfile);
        Task<int> Count(MailStatus status, MailDirection direction);
        Task<MailEntry> GetById(int entryId);
        Task<MailEntry> GetByDocumentId(int documentId);

        Task<MailEntry> GetByMessageId(string messageId, int mailboxId);

        Task<List<MailEntry>> GetByParentId(int entryId);
        Task<MimeMessage> GetContent(MailEntry entry);
        Task<List<MailEntry>> GetEntries(MailMessagesFilter filtro, UserProfile userProfile);
        Task<List<MailEntry>> GetEntriesToDelete(int mailboxId, string folderName, int GracePeriod);
        Task<DateTime> GetLastReceived(int mailboxId);
        Task<Mailbox> GetMailBox(int mailboxId);
        string GetMailHtml(MimeMessage eml, bool includeHeader = true);
        Task<MailDetails> GetMessage(int mailMessageId);
        Task<bool> Take(int entryId, string workerId, DateTime ExpirationTime);
        Task<MailEntry> TakeNext(string workerId, DateTime ExpirationTime);
        Task<MailEntry> Update(MailEntry Mail, Mailbox mailbox, string userId, MimeMessage eml, DateTime SendDate, MailStatus Status);
    }
}