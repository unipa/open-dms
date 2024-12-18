//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using MimeKit;
//using OpenDMS.Domain.Constants;
//using OpenDMS.Domain.DigitalSignature;
//using OpenDMS.Domain.Entities;
//using OpenDMS.Domain.Infrastructure;
//using OpenDMS.Domain.Models;
//using OpenDMS.Domain.Repositories;
//using OpenDMS.Domain.Services;
//using OpenDMS.Infrastructure.Repositories;
//using OpenDMS.MailSpooler.Core.DTOs;
//using OpenDMS.MailSpooler.Core.Helper;
//using OpenDMS.MailSpooler.Core.Interfaces;
//using System.Text;


//namespace OpenDMS.MailSpooler.Core
//{
//    public class MailSpoolerService : IMailSpoolerService
//    {
//        private readonly ILogger<MailSpoolerService> logger;
//        private readonly IMailEntryRepository mailRepo;
//        private readonly IMailboxRepository mailboxRepository;
//        private readonly IVirtualFileSystemProvider fileSystemProvider;
//        private readonly IDocumentRepository documentService;
//        private readonly IConfiguration config;
//        private readonly IMailArchiver mailArchiver;
//        private readonly IMailReader mailReader;
//        private readonly IAppSettingsRepository appSettingsRepository;
//        private readonly IMailSender mailsender;
//        private readonly IUserService userService;

//        public MailSpoolerService(
//            IMailEntryRepository MailRepo,
//            IMailboxRepository mailboxRepository,
//            IVirtualFileSystemProvider FileSystemProvider,
//            IDocumentRepository documentService,
//            IMailArchiver mailArchiver,
//            IMailReader MailReader,
//            IMailSender MailSender,
//            IUserService userService,
//            ILogger<MailSpoolerService> logger,
//            IConfiguration config,
//            IAppSettingsRepository appSettingsRepository
//            )
//        {
//            this.logger = logger;
//            this.mailRepo = MailRepo;
//            this.mailboxRepository = mailboxRepository;
//            this.fileSystemProvider = FileSystemProvider;
//            this.documentService = documentService;
//            this.config = config;
//            this.mailArchiver = mailArchiver;
//            this.mailReader = MailReader;
//            this.appSettingsRepository = appSettingsRepository;
//            this.mailsender = MailSender;
//            this.userService = userService;
//        }

//        public async Task<MailEntry> Save(CreateOrUpdateMailMessage MailMessage)
//        {
//            MailEntry Mail = null;
//            bool Sending = MailMessage.SendDate <= DateTime.UtcNow && MailMessage.Status == MailStatus.Sending;
//            string SendingError = "";
//            string iUrl = await appSettingsRepository.Get("URL.Internal") ?? "";
//            string eUrl = await appSettingsRepository.Get("URL.External") ?? "";
//            string aUrl = await appSettingsRepository.Get("URL.App") ?? "";
//            Mailbox mbox = await mailboxRepository.GetById(MailMessage.MailboxId);
//            var server = mbox.MailServer;
//            if (server != null)
//            {
//                try
//                {
//                    string to = "";
//                    if (MailMessage.To.Count > 0) to = MailboxAddress.Parse(MailMessage.To[0]).Address;
//                    else
//                        if (MailMessage.CC.Count > 0) to = MailboxAddress.Parse(MailMessage.CC[0]).Address;
//                    else
//                            if (MailMessage.CCr.Count > 0) to = MailboxAddress.Parse(MailMessage.CCr[0]).Address;

//                    MimeKit.MimeMessage eml = new MimeKit.MimeMessage();
//                    eml.MessageId = Guid.NewGuid().ToString();
//                    eml.From.Add(new MailboxAddress(mbox.DisplayName, mbox.MailAddress));
//                    eml.Subject = MailMessage.Subject;
//                    foreach (var a in MailMessage.To)
//                    {
//                        var ad = MailboxAddress.Parse(a);
//                        eml.To.Add(new MailboxAddress(ad.Name, ad.Address));
//                    }
//                    foreach (var a in MailMessage.CC)
//                    {
//                        var ad = MailboxAddress.Parse(a);
//                        eml.Cc.Add(new MailboxAddress(ad.Name, ad.Address));
//                    }
//                    foreach (var a in MailMessage.CCr)
//                    {
//                        var ad = MailboxAddress.Parse(a);
//                        eml.Bcc.Add(new MailboxAddress(ad.Name, ad.Address));
//                    }
//                    var multipart = new Multipart("mixed");
//                    eml.Body = multipart;
//                    var part = new TextPart(MimeKit.Text.TextFormat.Html) { Text = MailMessage.Body };
//                    multipart.Add(part);
//                    foreach (var docId in MailMessage.Attachments)
//                    {
//                        var doc = await documentService.GetById(docId);
//                        if (doc != null && doc.ImageId > 0)
//                        {
//                            DocumentImage immagineDoc = doc.Image;
//                            if (immagineDoc == null)
//                                immagineDoc = await documentService.GetImage(Convert.ToInt32(doc.ImageId));
//                            if (MailMessage.LinkAttachments)
//                            {
//                                part.Text += "<hr/>L'allegato '<b>" + doc.DocumentType + "</b>:" + doc.Description + "' è disponibile ai seguenti link: <a target='_blank' href='" + iUrl + "?id=" + doc.Id + "'>Intranet</a>" + (!string.IsNullOrEmpty(eUrl) ? " | <a target='_blank' href='" + eUrl + "?id=" + doc.Id + "'>Internet</a>" : "");
//                            }
//                            else
//                            {
//                                var f = await fileSystemProvider.InstanceOf(immagineDoc.FileManager);
//                                if (await f.Exists(immagineDoc.FileName))
//                                {
//                                    //using (
//                                    var data = await f.ReadAsStream(immagineDoc.FileName);
//                                    {
//                                        //var image = await documentService.GetImage(doc.ImageId.Value);
//                                        string path = immagineDoc.OriginalFileName;
//                                        var attachment = new MimePart()
//                                        {
//                                            Content = new MimeContent(data, ContentEncoding.Default),
//                                            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
//                                            ContentTransferEncoding = ContentEncoding.Base64,
//                                            FileName = Path.GetFileName(path)
//                                        };
//                                        multipart.Add(attachment);
//                                    }
//                                    if (MailMessage.IncludePDFPreview && !immagineDoc.FileName.EndsWith(".pdf") && await f.Exists(immagineDoc.FileName + ".pdf"))
//                                    {
//                                        var data2 = await f.ReadAsStream(immagineDoc.FileName + ".pdf");
//                                        {
//                                            //var image = await documentService.GetImage(doc.ImageId.Value);
//                                            string path = immagineDoc.OriginalFileName + ".pdf";
//                                            var attachment = new MimePart()
//                                            {
//                                                Content = new MimeContent(data2, ContentEncoding.Default),
//                                                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
//                                                ContentTransferEncoding = ContentEncoding.Base64,
//                                                FileName = Path.GetFileName(path)
//                                            };
//                                            multipart.Add(attachment);
//                                        }
//                                    }

//                                }
//                            }
//                        }
//                        else
//                        {
//                            //TODO: Segnalare che un allegato è stato rimosso
//                            part.Text += "<hr/>ATTENZIONE: Un allegato non è stato spedito perchè non più presente nel sistema (Id:" + docId + ")";
//                        }
//                    }
//                    eml.Headers.Add("_dms_documents_", Newtonsoft.Json.JsonConvert.SerializeObject(MailMessage.Attachments));

//                    if (MailMessage.EntryId > 0)
//                    {
//                        Mail = await UpdateEntry(MailMessage.EntryId, mbox, eml, MailMessage.SendDate, MailMessage.Status);
//                    }
//                    else
//                    {
//                        Mail = await CreateNewEntry(mbox, eml, MailMessage.SendDate, MailDirection.Outbound, MailMessage.Status);
//                    }

//                    // Invia immediatamente la mail
//                    if (Sending)
//                    {
//                        var entry = await SendMail(Mail);
//                        if (entry != null) await mailArchiver.Archive(entry, mbox.UserId, eml);
//                    }
//                }
//                catch (Exception Ex)
//                {
//                    SendingError = Ex.Message;
//                }
//            }
//            else
//            {
//                SendingError = "Nessun mailserver configurato per: " + mbox.MailAddress;
//            }
//            if (!String.IsNullOrEmpty(SendingError))
//            {
//                logger.LogError("SendMail: " + SendingError);
//                await Fail(Mail, SendingError);
//            }
//            return Mail; 
//        }
//        public async Task<MailEntry> Fail (MailEntry entry, string ErrorMessage)
//        {
//            entry.LastException = ErrorMessage;
//            entry.TransmissionDate = DateTime.UtcNow;
//            entry.RetryValue += 1;
//            entry.LastRunningUpdate = DateTime.UtcNow;
//            entry.Status = MailStatus.Failed;
//            entry.WorkerId = "";
//            await mailRepo.Update(entry);
//            return entry;
//        }

//        public async Task<MailEntry> CreateNewEntry(Mailbox mailbox, MimeMessage eml, DateTime sendDate, MailDirection Direction = MailDirection.Outbound, MailStatus Status = MailStatus.Sending, string Uidl = "")
//        {
//            var server = mailbox.MailServer;
//            if (server != null)
//            {
//                string fileHash = "";
//                string GUID = Guid.NewGuid().ToString();
//                int AttachmentsCount = eml.BodyParts.Where(b => b.IsAttachment).Count();
//                string ToAddress = eml.To.Count > 0 ? eml.To[0].ToString() : "";
//                string FromAddress = eml.From.Count > 0 ? eml.From[0].ToString() : "";
//                string filePath = "";
//                var filesystem = config[StaticConfiguration.CONST_MAILSERVICE_FILESYSTEMTYPE] + "";

//                if (server.InboxProtocol != InboxProtocol.IMAP)
//                {
//                    var rootfolder = config[StaticConfiguration.CONST_DOCUMENTS_ROOLFOLDER];
//                    var mailfolder = config[StaticConfiguration.CONST_MAILSERVICE_ROOTFOLDER];

//                    if (string.IsNullOrEmpty(rootfolder))
//                        rootfolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files");
//                    if (string.IsNullOrEmpty(mailfolder))
//                        mailfolder = "emails";

//                    //string InternalAddress = Direction == MailDirection.Inbound ? ToAddress MailboxAddress.Parse(eml.From[0].ToString()).Address;
//                    filePath = Path.Combine(rootfolder,
//                        mailfolder,
//                        mailbox.MailAddress,
//                        Direction.ToString(),
//                        DateTime.UtcNow.Year.ToString(),
//                        DateTime.UtcNow.Month.ToString("00"),
//                        DateTime.UtcNow.Day.ToString("00"),
//                        GUID + ".eml");
//                    var FS = await fileSystemProvider.InstanceOf(filesystem);
//                    using (var M = new MemoryStream())
//                    {
//                        eml.WriteTo(M);
//                        M.Seek(0, SeekOrigin.Begin);
//                        var array = M.ToArray();
//                        fileHash = MessageDigest.HashString(MessageDigest.HashType.SHA256, array);
//                        //TODO: Verificare se ha scritto
//                        await FS.WriteAllBytes(filePath, array);
//                    }
//                }

//                MailEntry Mail = new MailEntry();

//                var datiCert = eml.GetDatiCert();

//                // Determina il tipo di email per consentire all'inspector di scegliere il corretto Archiver...
//                if (datiCert != null)
//                {
//                    Mail.SubType =(MailSubType)((int)datiCert.MailType);
//                }
//                else
//                {
//                    Mail.SubType = MailSubType.Message;
//                }


//                Mail.FileManager = filesystem;
//                Mail.Status = Status; //  Sent ? MailStatus.Sent : MailMessage.Interactive ? MailStatus.Failed : MailStatus.Queued;
//                Mail.RetryValue = 0;
//                Mail.ArchivingDate = DateTime.UtcNow;
//                Mail.CreationDate = DateTime.UtcNow;
//                Mail.MessageDate = sendDate;
//                Mail.Direction = Direction;
//                Mail.ExternalMailAddress = Direction == MailDirection.Inbound ? FromAddress : ToAddress;
//                Mail.InternalMailAddress = FromAddress;
//                Mail.FilePath = filePath;
//                Mail.FileHash = fileHash;
//                Mail.LastException = "";
//                Mail.MailType = server.MailType;
//                Mail.MessageId = eml.MessageId;
//                Mail.MessageUID = GUID;
//                Mail.UIDL = Uidl;
//                // ricavare il tipo di messaggio dall'eml
//                Mail.MessageTitle = eml.Subject;
//                Mail.NumberOfAttachments = AttachmentsCount;
//                Mail.WorkerId = "";
//                Mail.MailServerId = server.Id;
//                Mail.MailboxId = mailbox.Id;
//                Mail.LastRunningUpdate = null;
//                await mailRepo.Insert(Mail);
//                return Mail;
//            }
//            return null;
//        }
//        public async Task<MailEntry> UpdateEntry(int entryId, Mailbox mailbox, MimeMessage eml, DateTime sendDate,  MailStatus Status = MailStatus.Sending)
//        {
//            var server = mailbox.MailServer;
//            var Mail = await mailRepo.GetById(entryId);
//            string filePath = Mail.FilePath;
//            int AttachmentsCount = eml.BodyParts.Where(b => b.IsAttachment).Count();
//            string ToAddress = eml.To.Count > 0 ? eml.To[0].ToString() : "";
//            string FromAddress = eml.From.Count > 0 ? eml.From[0].ToString() : "";

//            string fileHash = "";
//            var filesystem = Mail.FileManager;
//            var FS = await fileSystemProvider.InstanceOf(filesystem);
//            using (var M = new MemoryStream())
//            {
//                eml.WriteTo(M);
//                M.Seek(0, SeekOrigin.Begin);
//                var array = M.ToArray();
//                fileHash = MessageDigest.HashString(MessageDigest.HashType.SHA256, array);
//                //TODO: Verificare se ha scritto
//                await FS.WriteAllBytes(filePath, array);
//            }

//            Mail.Status = Status;
//            Mail.RetryValue = 0;
//            Mail.MessageDate = sendDate;
//            Mail.ExternalMailAddress = Mail.Direction == MailDirection.Inbound ? FromAddress : ToAddress;
//            Mail.InternalMailAddress = FromAddress;
//            Mail.FilePath = filePath;
//            Mail.FileHash = fileHash;
//            Mail.LastException = "";
//            Mail.MessageTitle = eml.Subject;
//            Mail.NumberOfAttachments = AttachmentsCount;
//            Mail.WorkerId = "";
//            Mail.MailServerId = server.Id;
//            Mail.MailboxId = mailbox.Id;
//            Mail.LastRunningUpdate = null;
//            await mailRepo.Update(Mail);
//            return Mail;
//        }

//        public async Task<MailEntry> SendMail(int entryId, string workerId = "Interactive")
//        {
//            var entry = await mailRepo.GetById(entryId);
//            return await SendMail(entry, workerId);
//        }
//        public async Task<MailEntry> SendMail(MailEntry entry, string workerId="Interactive")
//        {
//            if (entry.Direction != MailDirection.Outbound) return null;
//            if (string.IsNullOrEmpty(entry.FilePath)) return null;
//            if (!string.IsNullOrEmpty(entry.WorkerId)) return null;

//            try
//            {
//                entry.Status = MailStatus.Sending;
//                entry.WorkerId = workerId;
//                entry.LastRunningUpdate = DateTime.UtcNow;
//                await mailRepo.Update(entry);
//                var FS = await fileSystemProvider.InstanceOf(entry.FileManager);
//                using (var M = await FS.ReadAsStream(entry.FilePath))
//                {

//                    var eml = MimeMessage.Load(M);
//                    await mailsender.SendMail(eml);
//                    entry.Status = MailStatus.Sent;
//                    entry.TransmissionDate = DateTime.UtcNow;
//                    entry.LastRunningUpdate = DateTime.UtcNow;
//                    entry.WorkerId = "";
//                    await mailRepo.Update(entry);

//                    // questa operazione viene effettuata in asincrono
//                    // await mailArchiver.Archive(entry.Id);
//                }
//            }
//            catch (Exception ex)
//            {
//                entry.LastException = ex.Message;
//                entry.LastRunningUpdate = DateTime.UtcNow;
//                entry.Status = MailStatus.Failed;
//                entry.RetryValue += 1;
//                entry.WorkerId = "";
//                await mailRepo.Update(entry);
//            }
//            return entry;
//        }

//        public async Task<MimeMessage> GetMimeMessage(int entryId)
//        {
//            MailEntry entry = await mailRepo.GetById(entryId);
//            if (entry.DocumentId > 0)
//            {
//                // recupero l'eml dal documento, prima immagine
//                var images = await documentService.GetImages(entry.DocumentId);
//                var image = images.LastOrDefault();
//                if (image != null)
//                {
//                    var filesystem = image.FileManager;
//                    var FS = await fileSystemProvider.InstanceOf(filesystem);
//                    using (var M = await FS.ReadAsStream(image.FileName))
//                    {
//                        MimeMessage eml = await MimeMessage.LoadAsync(M);
//                        return eml;
//                    }
//                }
//            }
//            else
//            {
//                var mbox = await mailboxRepository.GetById(entry.MailboxId);
//                var eml = await mailReader.GetMessage(mbox, entry);
//                if (eml != null)
//                {
//                    return eml;
//                }
//            }
//            return null;
//        }

//        public async Task<MailEntry> NewEntry(Mailbox mailbox, int referTo = 0, bool forward = false)
//        {
//            MimeMessage eml = referTo > 0 ? await GetMimeMessage(referTo) : new MimeMessage();
//            string PreviousMessage = "<br/><hr/>"+GetMailHtml(eml);
//            if (forward)
//            {
//                eml.To.Clear();
//                eml.Cc.Clear();
//                eml.Bcc.Clear();
//            }
//            else
//            {
//                // reply
//                eml.To.Clear();
//                eml.To.Add(eml.From[0]);
//            }
//            eml.From.Clear();
//            eml.From.Add(new MailboxAddress(mailbox.DisplayName, mailbox.MailAddress));
//            eml.Subject = referTo <= 0 ? "" : (forward ? "I:" : "Re:") + eml.Subject;
//            var multipart = new Multipart("mixed");
//            eml.Body = multipart;
//            var part = new TextPart(MimeKit.Text.TextFormat.Html) { Text = PreviousMessage };
//            multipart.Add(part);
//            foreach (var att in eml.BodyParts.Where(b => b.IsAttachment))
//            {
//                multipart.Add(att);
//            }
//            return await CreateNewEntry(mailbox, eml, DateTime.UtcNow, MailDirection.Outbound, MailStatus.Draft);
//        }

//        public async Task<MailEntry> Claim (int entryId, string userId)
//        {
//            return await mailArchiver.Archive(entryId, userId, await GetMimeMessage(entryId));
//        }
//        public async Task<MailEntry> ChangeStatus  (int entryId, string userId, MailStatus status)
//        {
//            var entry = await mailRepo.GetById(entryId);
//            entry.ClaimUser = userId;
//            entry.Status = status;
//            await mailRepo.Update(entry);
//            return entry;
//        }
//        public async Task<MailEntry> TakeNext(string workerId, DateTime ExpirationTime)
//        {
//            return await mailRepo.TakeNext(workerId, ExpirationTime);
//        }
//        public async Task<bool> Take(int entryId, string workerId, DateTime ExpirationTime)
//        {
//            return await mailRepo.Take(entryId, workerId, ExpirationTime);
//        }


//        public async Task<List<MailEntry>> getMailEntriesList(MailMessagesFilter filtro)
//        {
//            return await mailRepo.GetMailEntries(filtro);
//        }
//        public async Task<int> Count(MailStatus status, MailDirection direction)
//        {
//            return await mailRepo.Count(status, direction);
//        }


//        private string GetMailHtml(MimeMessage eml, bool includeHeader = true)
//        {
//            string message = eml.GetTextBody(MimeKit.Text.TextFormat.Html);
//            StringBuilder sb = new();
//            if (includeHeader)
//            {
//                sb.Append("<b>Da:</b> " + eml.From[0].ToString() + "<br/>");
//                sb.Append("<b>Inviato:</b> " + eml.Date.ToString("dd/MM/yyyy HH:mm:ss") + "<br/>");
//                sb.Append("<b>A:</b> " + eml.To.ToString() + "<br/>");
//                sb.Append("<b>CC:</b> " + eml.Cc.ToString() + "<br/>");
//                sb.Append("<b>CCr:</b> " + eml.Bcc.ToString() + "<br/>");
//                sb.Append("<b>Oggetto:</b> " + eml.Subject + "<br/></br>");
//            }
//            sb.Append(message +"</br>");
//            return sb.ToString();
//        }


//        public async Task<MailDetails> GetMessage(int mailMessageId)
//        {
//            var mail = await mailRepo.GetById(mailMessageId);
//            MailDetails MailMessage = await GetSingleMessage(mail);

//            int parent = MailMessage.ParentId;
//            while (parent > 0)
//            {
//                var M = await GetMessage(parent);
//                MailMessage.Previous.Add(M);
//                parent = M.ParentId;
//            }
//            MailMessage.Previous.Reverse();

//            foreach (var next in await mailRepo.GetByParentId(mail.Id))
//            {
//                var M = await GetSingleMessage(next);
//                MailMessage.Next.Add(M);
//            }
//            return MailMessage;
//        }

//        private async Task<MailDetails> GetSingleMessage(MailEntry mail)
//        {
//            //var mbox = await mailboxRepository.GetById(mail.MailboxId);
////            var fs = await fileSystemProvider.InstanceOf(mail.FileManager);
////            var file = await fs.ReadAsStream(mail.FilePath);

//            MimeMessage msg = await GetMimeMessage(mail.Id); 


//            MailDetails MailMessage = new MailDetails();
//            MailMessage.Id = mail.Id;
//            var A = MailboxAddress.Parse(mail.InternalMailAddress);

//            var c = await userService.FindMailAddress(A.Name, A.Address);
//            MailMessage.From.Id = c != null ? c.Id.ToString() : "";
//            MailMessage.From.Description = mail.InternalMailAddress;

//            MailMessage.Status = mail.Status;
//            MailMessage.Subtype = mail.SubType;

//            foreach (MailboxAddress M in msg.To)
//            {
//                var c1 = await userService.FindMailAddress(M.Name, M.Address);
//                var L = new LookupTable() { Id = c != null ? c.Id.ToString() : "", Description = M.ToString() };
//                MailMessage.To.Add(L);

//            }
//            foreach (MailboxAddress M in msg.Cc)
//            {
//                var c1 = await userService.FindMailAddress(M.Name, M.Address);
//                var L = new LookupTable() { Id = c != null ? c.Id.ToString() : "", Description = M.ToString() };
//                MailMessage.CC.Add(L);

//            }
//            foreach (MailboxAddress M in msg.Bcc)
//            {
//                var c1 = await userService.FindMailAddress(M.Name, M.Address);
//                var L = new LookupTable() { Id = c != null ? c.Id.ToString() : "", Description = M.ToString() };
//                MailMessage.CCr.Add(L);

//            }
//            MailMessage.Title = msg.Subject;
//            MailMessage.ParentId = (mail.ParentId.HasValue ? mail.ParentId.Value : 0);
//            MailMessage.Body = msg.HtmlBody;
//            MailMessage.Date = msg.Date.ToString("dd/MM/yyyy HH:mm");
//            MailMessage.Attachments = new();
//            MailMessage.Body = GetMailHtml(msg, false);
//            return MailMessage;
//        }

//        public async Task<DateTime> GetLastReceived(int mailboxId)
//        {
//            return await mailRepo.GetLastReceived(mailboxId);
//        }

//        public async Task<List<MailEntry>> GetMessagesToDelete(int mailboxId, string folderName, int GracePeriod)
//        {
//            if (GracePeriod < 0) GracePeriod = 0;   
//            return await mailRepo.GetMessagesToDelete(mailboxId, folderName, GracePeriod);
//        }
//    }
//}