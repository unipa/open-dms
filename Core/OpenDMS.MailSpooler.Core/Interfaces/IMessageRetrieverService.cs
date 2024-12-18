using MimeKit;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.MailSpooler.Core.DTOs;

namespace OpenDMS.MailSpooler.Core.Interfaces
{
    public interface IMessageRetrieverService
    {
        Task<MimeMessage> GetContent(MailEntry entry);
        Task<MailDetails> GetMessage(int mailMessageId);
    }
}