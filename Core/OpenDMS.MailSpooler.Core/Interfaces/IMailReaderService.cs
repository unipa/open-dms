using MimeKit;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Models;
using OpenDMS.MailSpooler.Core.DTOs;
using OpenDMS.MailSpooler.Core.Reader.Models;

namespace OpenDMS.MailSpooler.Core.Interfaces
{
    public interface IMailReaderService
    {
        Task<IList<DownloaderResult>> Delete(Mailbox mailbox, UserProfile userProfile);
        Task<IList<DownloaderResult>> Read(Mailbox mailbox, UserProfile userProfile);
        Task<MimeMessage> GetMessage(Mailbox mailbox, MailEntry entry);
        Task<MailDetails> GetMessage(Mailbox mailbox, int mailMessageId);

    }
}