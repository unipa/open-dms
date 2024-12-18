using MimeKit;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Domain.Services
{
    public interface IMailSenderService
    {

        Task<CreateOrUpdateMailMessage> CreateNewMessage(Mailbox mailbox, UserProfile userProfile, List<int> Attachments);
        Task<CreateOrUpdateMailMessage> CreateNewMessage(Mailbox mailbox, UserProfile userProfile, int referTo = 0, bool forward = false);
        Task<MailEntry> Save(CreateOrUpdateMailMessage MailMessage, UserProfile userProfile);
        Task<CreateOrUpdateMailMessage> GetById(int entryId, UserProfile userProfile);
  

        Task<MailEntry> SendMail(int entryId, UserProfile userProfile, string workerId = "Interactive");
        Task<MailEntry> SendMail(MailEntry entry, UserProfile userProfile, string workerId = "Interactive");
    }
}
