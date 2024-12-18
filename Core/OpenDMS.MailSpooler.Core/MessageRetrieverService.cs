using MimeKit;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Services;
using OpenDMS.MailSpooler.Core.DTOs;
using OpenDMS.MailSpooler.Core.Interfaces;

namespace OpenDMS.MailSpooler.Core
{
    public class MessageRetrieverService : IMessageRetrieverService
    {
        private readonly IMailEntryService mailEntryService;
        private readonly IMailReaderService mailReader;
        private readonly IDocumentService documentService;
        private readonly IUserService userService;

        public MessageRetrieverService(IMailEntryService mailEntryService,
            IMailReaderService mailReader,
            IDocumentService documentService,
            IUserService userService)
        {
            this.mailEntryService = mailEntryService;
            this.mailReader = mailReader;
            this.documentService = documentService;
            this.userService = userService;
        }


        public async Task<MailDetails> GetMessage(int mailMessageId)
        {
            var mail = await mailEntryService.GetById(mailMessageId);
            MailDetails MailMessage = await GetSingleMessage(mail);

            int parent = MailMessage.ParentId;
            while (parent > 0)
            {
                var M = await GetMessage(parent);
                MailMessage.Previous.Add(M);
                parent = M.ParentId;
            }
            MailMessage.Previous.Reverse();

            foreach (var next in await mailEntryService.GetByParentId(mail.Id))
            {
                var M = await GetSingleMessage(next);
                MailMessage.Next.Add(M);
            }
            return MailMessage;
        }
        private async Task<MailDetails> GetSingleMessage(MailEntry mail)
        {
            MimeMessage msg = await GetContent(mail);


            MailDetails MailMessage = new MailDetails();
            MailMessage.Id = mail.Id;
            var A = MailboxAddress.Parse(mail.InternalMailAddress);

            var c = await userService.FindMailAddress(A.Name, A.Address);
            MailMessage.From.Id = c != null ? c.Id.ToString() : "";
            MailMessage.From.Description = mail.InternalMailAddress;

            MailMessage.Status = mail.Status;
            MailMessage.Subtype = mail.SubType;

            foreach (MailboxAddress M in msg.To)
            {
                var c1 = await userService.FindMailAddress(M.Name, M.Address);
                var L = new LookupTable() { Id = c != null ? c.Id.ToString() : "", Description = M.ToString() };
                MailMessage.To.Add(L);

            }
            foreach (MailboxAddress M in msg.Cc)
            {
                var c1 = await userService.FindMailAddress(M.Name, M.Address);
                var L = new LookupTable() { Id = c != null ? c.Id.ToString() : "", Description = M.ToString() };
                MailMessage.CC.Add(L);

            }
            foreach (MailboxAddress M in msg.Bcc)
            {
                var c1 = await userService.FindMailAddress(M.Name, M.Address);
                var L = new LookupTable() { Id = c != null ? c.Id.ToString() : "", Description = M.ToString() };
                MailMessage.CCr.Add(L);

            }
            MailMessage.Title = msg.Subject;
            MailMessage.ParentId = (mail.ParentId.HasValue ? mail.ParentId.Value : 0);
            MailMessage.Body = msg.HtmlBody;
            MailMessage.Date = msg.Date.ToString("dd/MM/yyyy HH:mm");
            MailMessage.Attachments = new();
            MailMessage.Body = mailEntryService.GetMailHtml(msg, false);
            return MailMessage;
        }

        public async Task<MimeMessage> GetContent(MailEntry entry)
        {
            if (entry.ImageId > 0)
            {
                // recupero l'eml dal documento, prima immagine
                var data = await documentService.GetContent(entry.ImageId);
                //                    var filesystem = image.FileManager;
                //                    var FS = await virtualFileSystemProvider.InstanceOf(filesystem);
                using (var M = new MemoryStream(data))
                {
                    MimeMessage eml = await MimeMessage.LoadAsync(M);
                    return eml;
                }
            }
            return null;
        }


    }
}
