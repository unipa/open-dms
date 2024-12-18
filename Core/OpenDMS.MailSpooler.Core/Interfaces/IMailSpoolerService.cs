//using MimeKit;
//using OpenDMS.Domain.Entities;
//using OpenDMS.Domain.Models;
//using OpenDMS.MailSpooler.Core.DTOs;

//namespace OpenDMS.MailSpooler.Core.Interfaces
//{
//    public interface IMailSpoolerService
//    {
//        Task<MailEntry> Save(CreateOrUpdateMailMessage MailMessage);
//        Task<MailEntry> Fail(MailEntry entry, string ErrorMessage);
//        Task<MailEntry> CreateNewEntry(Mailbox mailbox, MimeMessage eml, DateTime sendDate, MailDirection Direction = MailDirection.Outbound, MailStatus Status = MailStatus.Sending, string Uidl = "");
//        Task<MailEntry> UpdateEntry(int entryId, Mailbox mailbox, MimeMessage eml, DateTime sendDate, MailStatus Status = MailStatus.Sending);
//        Task<MailEntry> SendMail(int entryId, string workerId = "Interactive");
//        Task<MailEntry> SendMail(MailEntry entry, string workerId = "Interactive");
//        Task<MailEntry> NewEntry(Mailbox mailbox, int referTo = 0, bool forward = false);
//        Task<MailEntry> TakeNext(string workerId, DateTime ExpirationTime);
//        Task<MailEntry> Claim(int entryId, string userId);
//        Task<MailEntry> ChangeStatus(int entryId, string userId, MailStatus status);
//        Task<bool> Take(int entryId, string workerId, DateTime ExpirationTime);

//        Task<List<MailEntry>> getMailEntriesList(MailMessagesFilter filtro);
//        Task<int> Count(MailStatus status, MailDirection direction);
//        Task<MimeMessage> GetMimeMessage(int entryId);
//        Task<MailDetails> GetMessage(int mailMessageId);
//        Task<DateTime> GetLastReceived(int mailboxId);
//        Task<List<MailEntry>> GetMessagesToDelete(int mailboxId, string folderName, int GracePeriod);



//    }
//}