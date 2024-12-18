using Microsoft.Extensions.Logging;
using MimeKit;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using OpenDMS.Infrastructure.Repositories;
using OpenDMS.MailSpooler.Core.DTOs;
using OpenDMS.MailSpooler.Core.Helper;
using OpenDMS.MailSpooler.Core.Interfaces;
using System.Text;
using static iText.IO.Util.IntHashtable;


namespace OpenDMS.MailSpooler.Core
{
    public class MailEntryService : IMailEntryService
    {
        private readonly ILogger<MailEntryService> logger;
        private readonly IMailEntryRepository mailRepo;
        private readonly IMailboxRepository mailboxRepository;
        private readonly IDocumentService documentService;
        private readonly IDocumentTypeService documentTypeService;
        private readonly IUserService userService;

        public MailEntryService(
            IMailEntryRepository MailRepo,
            IMailboxRepository mailboxRepository,
            IDocumentService documentService,
            IDocumentTypeService documentTypeService,
            IUserService userService,
            ILogger<MailEntryService> logger
            )
        {
            this.logger = logger;
            this.mailRepo = MailRepo;
            this.mailboxRepository = mailboxRepository;
            this.documentService = documentService;
            this.documentTypeService = documentTypeService;
            this.userService = userService;
        }

        /// <summary>
        /// Aggiunge un MailEntry e, in base allo stato, archivia un documento con il file EML
        /// </summary>
        /// <param name="mailbox"></param>
        /// <param name="userProfile"></param>
        /// <param name="eml"></param>
        /// <param name="SendDate"></param>
        /// <param name="Direction"></param>
        /// <param name="Status"></param>
        /// <param name="Uidl"></param>
        /// <returns></returns>
        public async Task<MailEntry> Add(string GUID, Mailbox mailbox, string userId, MimeMessage eml, DateTime SendDate, MailDirection Direction, MailStatus Status, string Uidl = "")
        {
            //TODO: verificare se l'utente può aggiungere una entry
            var server = mailbox.MailServer;
            if (server == null) return null;

            int AttachmentsCount = eml.BodyParts.Where(b => b.IsAttachment).Count();
            string ToAddress = eml.To.Count > 0 ? eml.To.ToString() : "";
            string FromAddress = eml.From.Count > 0 ? eml.From[0].ToString() : mailbox.MailAddress;

            MailEntry Mail = new MailEntry();
            var datiCert = eml.GetDatiCert();
            // Determina il tipo di email per consentire all'inspector di scegliere il corretto Archiver...
            if (datiCert != null)
            {
                Mail.SubType = (MailSubType)((int)datiCert.MailType);
            }
            else
            {
                Mail.SubType = MailSubType.Message;
            }
            Mail.MailType = server.MailType;
            Mail.RetryValue = 3;
            Mail.CreationDate = DateTime.UtcNow;
            Mail.MessageDate =  SendDate;
            Mail.Direction = Direction;
            Mail.ExternalMailAddress = Direction == MailDirection.Inbound ? FromAddress : ToAddress;
            Mail.InternalMailAddress = Direction != MailDirection.Inbound ? FromAddress : ToAddress;
            Mail.LastException = "";
            Mail.MessageId = eml.MessageId;
            Mail.MessageUID = GUID;
            Mail.UIDL = Uidl;
            Mail.MessageTitle = eml.Subject;
            if (!String.IsNullOrEmpty( Mail.MessageTitle) && (Mail.MessageTitle.Length > 64)) Mail.MessageTitle = Mail.MessageTitle.Substring(0, 61)+"...";
            Mail.NumberOfAttachments = AttachmentsCount;
            Mail.WorkerId = "";
            Mail.MailServerId = server.Id;
            Mail.MailboxId = mailbox.Id;
            Mail.LastRunningUpdate = DateTime.UtcNow;
            Mail.Status = Status;
            if (Status != MailStatus.Received)
            {
                Mail.MessageDate = eml.Date.UtcDateTime;
                await SaveContent(Mail, eml, mailbox, userId);
                Mail.ArchivingDate = DateTime.UtcNow;
                Mail.ClaimDate = DateTime.UtcNow;
                Mail.ClaimUser = userId;
            }

            await mailRepo.Insert(Mail);
            return Mail;
        }

        /// <summary>
        /// Aggiorna il contenuto di una Entry (il messaggio)
        /// </summary>
        /// <param name="Mail"></param>
        /// <param name="mailbox"></param>
        /// <param name="userId"></param>
        /// <param name="eml"></param>
        /// <param name="SendDate"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public async Task<MailEntry> Update(MailEntry Mail, Mailbox mailbox, string userId, MimeMessage eml, DateTime SendDate, MailStatus Status)
        {
            //TODO: verificare se l'utente può aggiungere una entry
            var server = mailbox.MailServer;
            if (server == null) return null;

            int AttachmentsCount = eml.BodyParts.Where(b => b.IsAttachment).Count();
            string ToAddress = eml.To.Count > 0 ? eml.To.ToString() : "";
            string FromAddress = eml.From.Count > 0 ? eml.From[0].ToString() : mailbox.MailAddress;

            var datiCert = eml.GetDatiCert();
            // Determina il tipo di email per consentire all'inspector di scegliere il corretto Archiver...
            if (datiCert != null)
            {
                Mail.SubType = (MailSubType)((int)datiCert.MailType);
            }
            else
            {
                Mail.SubType = MailSubType.Message;
            }
            Mail.MailType = server.MailType;
            Mail.RetryValue = 3;
            Mail.MessageDate = SendDate;
            Mail.ExternalMailAddress = Mail.Direction == MailDirection.Inbound ? FromAddress : ToAddress;
            Mail.InternalMailAddress = Mail.Direction != MailDirection.Inbound ? FromAddress : ToAddress;
            Mail.LastException = "";
            Mail.MessageId = eml.MessageId;
            Mail.MessageTitle = eml.Subject;
            Mail.NumberOfAttachments = AttachmentsCount;
            Mail.WorkerId = "";
            Mail.MailServerId = server.Id;
            Mail.MailboxId = mailbox.Id;
            Mail.LastRunningUpdate = DateTime.UtcNow;
            Mail.Status = Status;
            if (Status != MailStatus.Received)
            {
                await SaveContent(Mail, eml, mailbox, userId);
                Mail.ArchivingDate = DateTime.UtcNow;
                Mail.ClaimDate = DateTime.UtcNow;
                Mail.ClaimUser = userId;
            }
            await mailRepo.Update(Mail);
            return Mail;
        }

        public async Task<MailEntry> ChangeStatus(int entryId, string userId, MailStatus status, string ErrorMessage = "")
        {
            var entry = await mailRepo.GetById(entryId);
            return await ChangeStatus(entry, userId, status, ErrorMessage);
        }

        public async Task<MailEntry> ChangeStatus(MailEntry entry, string userId, MailStatus status, string ErrorMessage = "")
        {
            entry.ClaimDate = DateTime.UtcNow;
            if (status == MailStatus.Claimed)
            {
                entry.ClaimUser = userId;
                entry.ClaimDate = DateTime.UtcNow;
            }
            else
            if (status == MailStatus.Archived)
            {
                entry.ArchivingUser = userId;
                entry.ArchivingDate = DateTime.UtcNow;
            }
            else
            if (status == MailStatus.Deleted)
            {
                entry.DeletionUser = userId;
                entry.DeletionDate = DateTime.UtcNow;
            }
            else
            if (status == MailStatus.Sending)
            {
                entry.TransmissionDate = DateTime.UtcNow;
                if (entry.ImageId > 0) await documentService.UpdateSendingStatus(entry.ImageId, Domain.Enumerators.JobStatus.Running, UserProfile.SystemUser());
            }
            else
            if (status == MailStatus.Sent)
            {
                entry.TransmissionDate = DateTime.UtcNow;
                entry.WorkerId = "";
                if (entry.ImageId > 0) await documentService.UpdateSendingStatus(entry.ImageId, Domain.Enumerators.JobStatus.Completed, UserProfile.SystemUser());
            }
            else
            if (status == MailStatus.Failed)
            {
                entry.TransmissionDate = DateTime.UtcNow.AddMinutes(1);
                entry.WorkerId = "";
                entry.RetryValue--;
                entry.LastException = ErrorMessage;
                if (entry.ImageId > 0) await documentService.UpdateSendingStatus(entry.ImageId, Domain.Enumerators.JobStatus.Failed, UserProfile.SystemUser());
            } else
            if (status == MailStatus.Spam)
            {
                entry.ClaimUser = userId;
            }
            entry.Status = status;
            await mailRepo.Update(entry);
            return entry;
        }
  
        public async Task<MailEntry> GetById(int entryId)
        {
            var entry = await mailRepo.GetById(entryId);
            return entry;
        }
        public async Task<MailEntry> GetByDocumentId(int documentId)
        {
            var entry = await mailRepo.GetByDocumentId(documentId);
            return entry;
        }

        public async Task<List<MailEntry>> GetByParentId(int entryId)
        {
            var entry = await mailRepo.GetByParentId(entryId);
            return entry;
        }
        public async Task<MailEntry> GetByMessageId(string messageId, int mailboxId)
        {
            var entry = await mailRepo.GetByMessageId (messageId, mailboxId);
            return entry;
        }
        public async Task<Mailbox> GetMailBox(int mailboxId)
        {
            var entry = await mailboxRepository.GetById(mailboxId);
            return entry;
        }






        //public async Task<MailEntry> NewEntry(Mailbox mailbox, int referTo = 0, bool forward = false)
        //{
        //    MimeMessage eml = referTo > 0 ? await GetMimeMessage(referTo) : new MimeMessage();
        //    string PreviousMessage = "<br/><hr/>" + GetMailHtml(eml);
        //    if (forward)
        //    {
        //        eml.To.Clear();
        //        eml.Cc.Clear();
        //        eml.Bcc.Clear();
        //    }
        //    else
        //    {
        //        // reply
        //        eml.To.Clear();
        //        eml.To.Add(eml.From[0]);
        //    }
        //    eml.From.Clear();
        //    eml.From.Add(new MailboxAddress(mailbox.DisplayName, mailbox.MailAddress));
        //    eml.Subject = referTo <= 0 ? "" : (forward ? "I:" : "Re:") + eml.Subject;
        //    var multipart = new Multipart("mixed");
        //    eml.Body = multipart;
        //    var part = new TextPart(MimeKit.Text.TextFormat.Html) { Text = PreviousMessage };
        //    multipart.Add(part);
        //    foreach (var att in eml.BodyParts.Where(b => b.IsAttachment))
        //    {
        //        multipart.Add(att);
        //    }
        //    return await CreateNewEntry(mailbox, eml, DateTime.UtcNow, MailDirection.Outbound, MailStatus.Draft);
        //}


        public async Task<bool> Take(int entryId, string workerId, DateTime ExpirationTime)
        {
            return await mailRepo.Take(entryId, workerId, ExpirationTime);
        }
        public async Task<MailEntry> TakeNext(string workerId, DateTime ExpirationTime)
        {

            return await mailRepo.TakeNext(workerId, ExpirationTime);
        }

        public async Task<List<MailEntry>> GetEntries(MailMessagesFilter filtro, UserProfile userProfile)
        {
            return await mailRepo.GetMailEntries(filtro);
        }
        public async Task<int> Count (MailMessagesFilter filtro, UserProfile userProfile)
        {
            return await mailRepo.Count(filtro);
        }

        public async Task<int> Count(MailStatus status, MailDirection direction)
        {
            return await mailRepo.Count(status, direction);
        }


        public string GetMailHtml(MimeMessage eml, bool includeHeader = true)
        {
            string message = eml.GetTextBody(MimeKit.Text.TextFormat.Html);
            StringBuilder sb = new();
            if (includeHeader)
            {
                sb.Append("<b>Da:</b> " + eml.From[0].ToString() + "<br/>");
                sb.Append("<b>Inviato:</b> " + eml.Date.ToString("dd/MM/yyyy HH:mm:ss") + "<br/>");
                sb.Append("<b>A:</b> " + eml.To.ToString() + "<br/>");
                sb.Append("<b>CC:</b> " + eml.Cc.ToString() + "<br/>");
                sb.Append("<b>CCr:</b> " + eml.Bcc.ToString() + "<br/>");
                sb.Append("<b>Oggetto:</b> " + eml.Subject + "<br/></br>");
            }
            sb.Append(message + "</br>");
            return sb.ToString();
        }
        public async Task<MailDetails> GetMessage(int mailMessageId)
        {
            var mail = await mailRepo.GetById(mailMessageId);
            MailDetails MailMessage = await GetSingleMessage(mail);

            int parent = MailMessage.ParentId;
            while (parent > 0)
            {
                var M = await GetMessage(parent);
                MailMessage.Previous.Add(M);
                parent = M.ParentId;
            }
            MailMessage.Previous.Reverse();

            foreach (var next in await mailRepo.GetByParentId(mail.Id))
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
            MailMessage.Body = GetMailHtml(msg, false);
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


        public async Task<DateTime> GetLastReceived(int mailboxId)
        {
            return await mailRepo.GetLastReceived(mailboxId);
        }
        public async Task<List<MailEntry>> GetEntriesToDelete(int mailboxId, string folderName, int GracePeriod)
        {
            if (GracePeriod < 0) GracePeriod = 0;
            return await mailRepo.GetMessagesToDelete(mailboxId, folderName, GracePeriod);
        }


        private async Task SaveContent(MailEntry entry, MimeMessage eml, Mailbox mailbox, string userId)
        {
            string docType = mailbox.DocumentType;
            DocumentType tp = null;
            if (!string.IsNullOrEmpty(docType))
                tp = await documentTypeService.GetById(docType);
            if (tp == null)
            {
                docType = entry.MailType == MailType.Mail ? "$MAIL-" : "$PEC-";
                docType += entry.Direction == MailDirection.Inbound ? "INBOUND$" : "OUTBOUND$";
                tp = await documentTypeService.GetById(docType);
                if (tp == null)
                {
                    docType = entry.MailType == MailType.Mail ? "$MAIL$" : "$PEC$";
                    tp = await documentTypeService.GetById(docType);
                    if (tp == null)
                    {
                        docType = "$MAIL$";
                        tp = await documentTypeService.GetById(docType);
                    }
                }
            }
            CreateOrUpdateDocument createOrUpdateDocument = new CreateOrUpdateDocument();
            createOrUpdateDocument.DocumentDate = entry.MessageDate ?? DateTime.UtcNow;
            createOrUpdateDocument.DocumentTypeId = tp.Id;
            createOrUpdateDocument.CompanyId = mailbox.CompanyId;
            createOrUpdateDocument.Status = tp.InitialStatus; // Domain.Enumerators.DocumentStatus.Draft;
            createOrUpdateDocument.ContentType = Domain.Enumerators.ContentType.Document;
            createOrUpdateDocument.Description = entry.MessageTitle;
            createOrUpdateDocument.ExternalId = mailbox.MailAddress + ":" + entry.MessageUID;

            if (entry.MailType == MailType.Mail)
            {
                createOrUpdateDocument.Icon = "fa fa-envelope-o";
            }
            else
            {
                createOrUpdateDocument.Icon = "fa fa-envelope";
                createOrUpdateDocument.IconColor = "firebrick";
            }
            
            createOrUpdateDocument.FolderId = 0;
            createOrUpdateDocument.Owner = mailbox.UserId;
            createOrUpdateDocument.NotifyTo = "";
            createOrUpdateDocument.NotifyCC = "";

            List<string> ToAuthorize = new List<string>() { "0" + mailbox.UserId };
            if (ToAuthorize.Find(f => string.Compare(f, "0" + userId, true) == 0) == null) ToAuthorize.Add("0" + userId);


            createOrUpdateDocument.Authorize = "0"+ mailbox.UserId + "," + "0" + userId;
            createOrUpdateDocument.Authorize = "0"+ mailbox.UserId + "," + "0" + userId;
            if (!string.IsNullOrEmpty(mailbox.ReadOnlyProfiles))
            {
                foreach (var a in mailbox.ReadOnlyProfiles.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    if (ToAuthorize.Find(f => string.Compare(f, a, true) == 0) == null) ToAuthorize.Add(a);
                }
            }

            if (entry.Direction == MailDirection.Outbound)
            {
                if (!string.IsNullOrEmpty(mailbox.SendEnabledProfiles))
                {
                    foreach (var a in mailbox.SendEnabledProfiles.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                    {
                        if (ToAuthorize.Find(f => string.Compare(f, a, true) == 0) == null) ToAuthorize.Add(a);
                    }
                }

                //createOrUpdateDocument.Authorize += "," + mailbox.SendEnabledProfiles;
                if (entry.Status == MailStatus.Draft)
                {
                    if (!string.IsNullOrEmpty(mailbox.DraftEnabledProfiles))
                    {
                        foreach (var a in mailbox.DraftEnabledProfiles.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                        {
                            if (ToAuthorize.Find(f => string.Compare(f, a, true) == 0) == null) ToAuthorize.Add(a);
                        }
                    }
                    //createOrUpdateDocument.Authorize += "," + mailbox.DraftEnabledProfiles;
                }
            }
            createOrUpdateDocument.Authorize = string.Join(",", ToAuthorize);

            createOrUpdateDocument.Content = new();
            using (var Mem = new MemoryStream())
            {
                eml.WriteTo(Mem);//.WriteToAsync(M);
                createOrUpdateDocument.Content.FileData = Encoding.UTF8.GetString(Mem.ToArray());
            }
            createOrUpdateDocument.Content.DataIsInBase64 = false;
            createOrUpdateDocument.Content.FileName = entry.MessageUID + ".eml";
            createOrUpdateDocument.Content.ExtractAttachment = entry.Direction == MailDirection.Inbound;
            if (!mailbox.SaveAsDocument)
            {
                if (entry.Direction == MailDirection.Inbound)
                {
                    createOrUpdateDocument.NotifyTo = "0" + userId;
                    createOrUpdateDocument.NotificationTemplate = NotificationConstants.CONST_TEMPLATE_NEWMAIL;
                }
            }
            var u = UserProfile.SystemUser();
            int doc = 0;
            if (entry.DocumentId <= 0)
            {
                doc = await documentService.FindByUniqueId(tp.Id, createOrUpdateDocument.ExternalId, createOrUpdateDocument.ContentType);
                //                if (doc > 0)
                //                    d = await documentService.Get(doc);
                //                else
                if (doc <= 0)
                    doc = await documentService.Create(createOrUpdateDocument, u);
            }
            else
            {
                var updoc = await documentService.Update(entry.DocumentId, createOrUpdateDocument, u);
                if (updoc != null) doc = updoc.Id;
            }
            if (doc == 0)  return;
            var d = await documentService.Get(doc);
            if (d == null || !d.ImageId.HasValue) return;
            if (entry.ParentId.HasValue)
            {
                MailEntry parent = await GetById(entry.ParentId.Value);
                var masterdocumentid = parent.DocumentId;
                // Collego il documento di risposta a quello iniziale
                // Lo registro come allegato perchè quasi sicuramente, il master è incluso nel msg
                await documentService.AddLink(d.Id, masterdocumentid, u, true);
            }

            entry.DocumentId = d.Id;
            entry.ImageId = d.Image.Id;
            entry.FileManager = d.Image.FileManager;
            entry.FilePath = d.Image.FileName;
            entry.FileHash = d.Image.Hash;
            entry.ProtocolNumber = d.ProtocolNumber;
            entry.ProtocolURL = d.ExternalProtocolURL;

            if (entry.Direction == MailDirection.Inbound)
            {
                entry.Status = MailStatus.Claimed;
                entry.ClaimDate = DateTime.UtcNow;
                entry.ClaimUser = userId;
            }
        
        }

    }
}