using MailKit;
using Microsoft.Extensions.Logging;
using MimeKit;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Models;
using OpenDMS.Infrastructure.Repositories;
using OpenDMS.MailSpooler.Core.DTOs;
using OpenDMS.MailSpooler.Core.Interfaces;
using OpenDMS.MailSpooler.Core.Reader.Models;
using OpenDMS.MailSpooler.Core.Reader.Protocols;
using System.Security.Cryptography;
using static MailKit.Net.Imap.ImapMailboxFilter;

namespace OpenDMS.MailSpooler.Core
{
    public  class MailReaderService : IMailReaderService
    {
        protected readonly IAuthenticatorFactory authenticatorFactory;
        protected readonly ILogger<MailReaderService> logger;
        protected readonly IMailboxRepository mailboxRepo;
        protected readonly IMailEntryService mailEntryService;

        protected const int MAX_MESSAGE_MARK_FOR_DELETION = 500;
        protected MailboxAddress SPAM_ADDRESS = new MailboxAddress("SPAM", "spam@localhost.local");
        protected CancellationToken _token;

        public MailReaderService(
            IMailboxRepository mailboxRepository,
            IMailEntryService mailEntryService,
            IAuthenticatorFactory authenticatorFactory,
            ILogger<MailReaderService> logger
            )
        {
            mailboxRepo = mailboxRepository;
            this.mailEntryService = mailEntryService;
            this.authenticatorFactory = authenticatorFactory;
            this.logger = logger;
            //_spamdao = new SpamAddressRepository();
        }

        public async Task<IList<DownloaderResult>> Read(Mailbox mailbox, UserProfile userProfile)
        {
            IList<DownloaderResult> result = new List<DownloaderResult>();
//            logger.LogInformation($"Iniziate le attività per {mailbox.MailAddress} su {mailbox.MailServer.InboxServer} ...");
            try
            {
                using (var downloader = GetReader(mailbox))
                {
                    try
                    {
                        await downloader.Connect();
                        logger.LogInformation($"Connessione riuscita al server: {mailbox.MailServer.InboxServer}:{mailbox.MailServer.SMTPServerPort}(SSL:{mailbox.MailServer.InboxSSL}) per la casella: {mailbox.MailAddress}");
                    }
                    catch (Exception ex)
                    {
                        logger.LogInformation($"Connessione non riuscita al server: {mailbox.MailServer.InboxServer}:{mailbox.MailServer.SMTPServerPort}(SSL:{mailbox.MailServer.InboxSSL}) per la casella: {mailbox.MailAddress}: {ex.Message}");
                        throw;
                    }
                    var authenticator = GetAuthenticator(mailbox);
                    try
                    {

                        await downloader.Authenticate(authenticator);
                        logger.LogInformation($"Autenticazione riuscita al server: {mailbox.MailServer.InboxServer}:{mailbox.MailServer.SMTPServerPort}(SSL:{mailbox.MailServer.InboxSSL}) per la casella: {mailbox.MailAddress}");
                    }
                    catch (Exception ex)
                    {
                        var authToken = authenticator.RefreshToken(mailbox);
                        if (authToken != null)
                        {
                            try
                            {

                                mailbox.TokenId = authToken.Token;
                                mailbox.RefreshToken = authToken.RefreshToken;
                                await mailboxRepo.Update(mailbox);
                                await downloader.Authenticate(authenticator);
                            }
                            catch (Exception ex2)
                            {
                                logger.LogError($"Aggiornamento Token non riuscito sulla casella: {mailbox.MailAddress}: {ex2.Message}");
                                throw;
                            }
                        }
                        else
                        {
                            logger.LogInformation($"Autenticazione non riuscita al server: {mailbox.MailServer.InboxServer}:{mailbox.MailServer.SMTPServerPort}(SSL:{mailbox.MailServer.InboxSSL}) per la casella: {mailbox.MailAddress}: {ex.Message}");
                            throw;
                        }
                    }

                    DateTime ArchiviaDal = mailbox.FirstReceivingMessageDate?.Date ?? DateTime.MinValue;
                    DateTime FromDate = await mailEntryService.GetLastReceived(mailbox.Id);
                    if (FromDate == DateTime.MinValue)
                        FromDate = ArchiviaDal;

                    if (FromDate != DateTime.MinValue)
                        FromDate = FromDate.AddDays(-1 * (mailbox.DaysToRead));

                    logger.LogInformation($"Avvio lettura messaggi a partire dalla data: {FromDate.ToShortDateString}");
                    var folders = downloader.GetFolders();
                    foreach (var folder in folders)
                    {
                        if (_token.IsCancellationRequested)
                            break;
                        var Lista = new DownloaderResult();
                        result.Add(Lista);
                        Lista.Folder = folder;
                        Lista.FirstDate = FromDate;

                        logger.LogInformation($"Apertura della cartella {folder} nella casella: {mailbox.MailAddress}");
                        var read = await downloader.Retrieve(FromDate, folder);
                        logger.LogInformation($"Trovati {read} messaggi sulla cartella {folder} della casella: {mailbox.MailAddress}");
                        Lista.Letti = read;

                        var MsgDate = await downloader.GetNext();
                        while (MsgDate != null)
                        {
                            if (MsgDate >= FromDate)
                            {
                                var msg = await downloader.GetMessage();
                                var MessageId = msg.MessageId;
                                if (!string.IsNullOrEmpty(MessageId) && MessageId.Length > 255)
                                    MessageId = MessageId.Substring(0, 254);
                                if (string.IsNullOrEmpty(MessageId))
                                {
                                    msg.MessageId = GetFakeMessageId(msg);
                                    logger.LogError($"Messaggio #{downloader.UID} senza Id. Generato Fake #{MessageId}");
                                    MessageId = msg.MessageId;
                                }


                                //
                                //if (!PluginFactory.Instance.GetRetrieverPlugins().All(p => p.PreDownload(msg, mailbox)))
                                //    continue;
                                var e = await mailEntryService.GetByMessageId(MessageId, mailbox.Id);
                                if (e == null)
                                {
                                    e = await mailEntryService.Add(MessageId, mailbox, userProfile.userId, msg, MsgDate.Value, downloader.direction, downloader.direction == MailDirection.Inbound ? mailbox.SaveAsDocument ? MailStatus.Archived : MailStatus.Received : MailStatus.Sent, downloader.UID);
                                    if (e != null)
                                    {
                                        Lista.Scaricati += 1;
                                        Lista.Messaggi.Add(e.Id);
                                    }
                                    else
                                    {
                                        logger.LogError($"Impossibile scaricare il messaggio #{downloader.UID} - Casella: {mailbox.MailAddress}");
                                    }
                                }
                            }
                            MsgDate = await downloader.GetNext();

                        }
                    }
                    await UpdateReaderStatus(mailbox, "");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Errore durante l'accesso al server: {mailbox.MailServer.InboxServer}:{mailbox.MailServer.SMTPServerPort}(SSL:{mailbox.MailServer.InboxSSL}) per la casella: {mailbox.MailAddress}: {ex.Message}");
                await UpdateReaderStatus(mailbox, ex.Message);
            }
            return result;
        }
        public async Task<IList<DownloaderResult>> Delete(Mailbox mailbox, UserProfile userProfile)
        {
            IList<DownloaderResult> result = new List<DownloaderResult>();
            logger.LogInformation($"Iniziate le attività di pulitura per {mailbox.MailAddress} su {mailbox.MailServer.InboxServer} ...");
            try
            {
                using (var downloader = GetReader(mailbox))
                {
                    try
                    {
                        await downloader.Connect();
                        logger.LogInformation($"Connessione riuscita al server: {mailbox.MailServer.InboxServer}:{mailbox.MailServer.SMTPServerPort}(SSL:{mailbox.MailServer.InboxSSL}) per la casella: {mailbox.MailAddress}");
                    }
                    catch (Exception ex)
                    {
                        logger.LogInformation($"Connessione non riuscita al server: {mailbox.MailServer.InboxServer}:{mailbox.MailServer.SMTPServerPort}(SSL:{mailbox.MailServer.InboxSSL}) per la casella: {mailbox.MailAddress}: {ex.Message}");
                        throw;
                    }
                    try
                    {
                        var authenticator = GetAuthenticator(mailbox);
                        await downloader.Authenticate(authenticator);
                        logger.LogInformation($"Autenticazione riuscita al server: {mailbox.MailServer.InboxServer}:{mailbox.MailServer.SMTPServerPort}(SSL:{mailbox.MailServer.InboxSSL}) per la casella: {mailbox.MailAddress}");
                    }
                    catch (Exception ex)
                    {
                        logger.LogInformation($"Autenticazione non riuscita al server: {mailbox.MailServer.InboxServer}:{mailbox.MailServer.SMTPServerPort}(SSL:{mailbox.MailServer.InboxSSL}) per la casella: {mailbox.MailAddress}: {ex.Message}");
                        throw;
                    }
                    foreach (var folder in downloader.GetFolders())
                    {
                        if (_token.IsCancellationRequested)
                            break;
                        var Lista = new DownloaderResult();
                        result.Add(Lista);
                        Lista.Folder = folder;
                        logger.LogInformation($"Apertura della cartella {folder} nella casella: {mailbox.MailAddress}");
                        foreach (var e in await mailEntryService.GetEntriesToDelete(mailbox.Id, folder, mailbox.GracePeriod))
                        {
                            try
                            {
                                if (await downloader.Delete(e.UIDL, folder) > 0)
                                {
                                    await mailEntryService.ChangeStatus(e,  userProfile.userId, MailStatus.Purged);
                                }
                            } catch (Exception ex) {
                                await mailEntryService.ChangeStatus(e, userProfile.userId, MailStatus.Purged, ex.Message);
                            }
                        }
                        await downloader.Delete("", SpecialFolder.Trash.ToString());
                    }
                    await UpdateDeleteStatus(mailbox, "");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Errore durante l'accesso al server: {mailbox.MailServer.InboxServer}:{mailbox.MailServer.SMTPServerPort}(SSL:{mailbox.MailServer.InboxSSL}) per la casella: {mailbox.MailAddress}: {ex.Message}");
                await UpdateDeleteStatus(mailbox, ex.Message);
            }
            return result;
        }
        public async Task<MimeMessage> GetMessage(Mailbox mailbox, MailEntry entry)
        {
            MimeMessage eml = await mailEntryService.GetContent(entry);
            if (eml == null)
            {
                using (var downloader = GetReader(mailbox))
                {
                    try
                    {
                        await downloader.Connect();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    var authenticator = GetAuthenticator(mailbox);
                    try
                    {
                        await downloader.Authenticate(authenticator);
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            var authToken = authenticator.RefreshToken(mailbox);
                            mailbox.TokenId = authToken.Token;
                            mailbox.RefreshToken = authToken.RefreshToken;
                            await mailboxRepo.Update(mailbox);
                            await downloader.Authenticate(authenticator);
                        } catch (Exception ex2)
                        {
                            logger.LogInformation($"Autenticazione non riuscita al server: {mailbox.MailServer.InboxServer}:{mailbox.MailServer.SMTPServerPort}(SSL:{mailbox.MailServer.InboxSSL}) per la casella: {mailbox.MailAddress}: {ex2.Message}");
                            throw;
                        }
                    }
                    eml = await downloader.GetMessage(mailbox.MailServer.InboxProtocol == InboxProtocol.IMAP ? entry.UIDL : entry.MessageId, entry.IMAPFolder);
                };
            }
            return eml;
        }

        public async Task<MailDetails> GetMessage(Mailbox mailbox, int mailMessageId)
        {
            var mail = await mailEntryService.GetById(mailMessageId);

            MailDetails MailMessage = await GetSingleMessage(mailbox, mail);

            int parent = MailMessage.ParentId;
            while (parent > 0)
            {
                var M = await GetMessage(mailbox, parent);
                MailMessage.Previous.Add(M);
                parent = M.ParentId;
            }
            MailMessage.Previous.Reverse();

            foreach (var next in await mailEntryService.GetByParentId(mail.Id))
            {
                var M = await GetSingleMessage(mailbox, next);
                MailMessage.Next.Add(M);
            }
            return MailMessage;
        }
        private async Task<MailDetails> GetSingleMessage(Mailbox mailbox, MailEntry mail)
        {
            MimeMessage msg = null;
            MailDetails MailMessage = new MailDetails();
            try
            {
                if (mail.ImageId > 0)
                    msg = await mailEntryService.GetContent(mail);
                else
                    msg = await GetMessage(mailbox, mail);


                MailMessage.Id = mail.Id;
                var A = msg.From;

                LookupTable c = null;// await userService.FindMailAddress(A.Name, A.Address);
                MailMessage.From.Id = c != null ? c.Id.ToString() : "";
                MailMessage.From.Description = msg.From.ToString();

                MailMessage.Status = mail.Status;
                MailMessage.Subtype = mail.SubType;

                foreach (MailboxAddress M in msg.To)
                {
                    LookupTable c1 = null;// await userService.FindMailAddress(M.Name, M.Address);
                    var L = new LookupTable() { Id = c != null ? c.Id.ToString() : "", Description = M.ToString() };
                    MailMessage.To.Add(L);

                }
                foreach (MailboxAddress M in msg.Cc)
                {
                    LookupTable c1 = null;//await userService.FindMailAddress(M.Name, M.Address);
                    var L = new LookupTable() { Id = c != null ? c.Id.ToString() : "", Description = M.ToString() };
                    MailMessage.CC.Add(L);

                }
                foreach (MailboxAddress M in msg.Bcc)
                {
                    LookupTable c1 = null;//await userService.FindMailAddress(M.Name, M.Address);
                    var L = new LookupTable() { Id = c != null ? c.Id.ToString() : "", Description = M.ToString() };
                    MailMessage.CCr.Add(L);

                }
                MailMessage.Title = msg.Subject;
                MailMessage.ParentId = (mail.ParentId.HasValue ? mail.ParentId.Value : 0);
                MailMessage.Body = msg.HtmlBody;
                MailMessage.Date = msg.Date.ToString("dd/MM/yyyy HH:mm");
                MailMessage.DocumentId = mail.DocumentId;
                MailMessage.Date = msg.Date.ToString("dd/MM/yyyy HH:mm");
                MailMessage.Attachments = new();
                foreach (var a in msg.Attachments)
                    MailMessage.Attachments.Add(new LookupTable() { Description = ((MimePart)a).FileName, Id = a.ContentId, TableId = "" });
                MailMessage.Body = mailEntryService.GetMailHtml(msg, false);//.Replace("<style ", "<xstyle").Replace("<style>", "<xstyle>").Replace("</style>", "</xstyle>");
            } catch (Exception ex)
            {
                MailMessage.Body = ex.Message;
                MailMessage.Title = "Si è verificato un errore!";
                MailMessage.Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                MailMessage.From = new LookupTable() { Id = "", Description = mailbox.MailAddress };
            }
            return MailMessage;
        }


        protected IMailReaderProtocol GetReader(Mailbox mailbox)
        {
            switch (mailbox.MailServer.InboxProtocol)
            {
                case InboxProtocol.None:
                    break;
                case InboxProtocol.Pop3:
                    return new Pop3Reader(mailbox, _token);
                case InboxProtocol.IMAP:
                    return new ImapReader(mailbox, _token);
                    break;
                case InboxProtocol.FileSystemImport:
                    break;
                default:
                    break;
            }
            return null;
        }
        protected string GetFakeMessageId(MimeMessage msg)
        {
            using (SHA256 m = SHA256.Create("sha256"))
            {
                using (var M = new MemoryStream())
                {
                    msg.WriteTo(M);
                    M.Position = 0;
                    var res = m.ComputeHash(M.ToArray());
                    return "<FAKE-" + string.Concat(res.Select(x => x.ToString("X2"))) + ">";
                }
            }
            return "";
        }
        protected async Task UpdateReaderStatus(Mailbox mailbox, string Errors)
        {
            mailbox.LastReceivingError = Errors;
            mailbox.LastReceivingDate = DateTime.UtcNow;
            mailbox.ReaderWorkerId = "";
            await mailboxRepo.Update(mailbox);
        }
        protected async Task UpdateDeleteStatus(Mailbox mailbox, string Errors)
        {
            mailbox.LastDeletionDate = DateTime.UtcNow;
            mailbox.EraserWorkerId = "";
            await mailboxRepo.Update(mailbox);
        }
        protected IAuthenticator GetAuthenticator(Mailbox mailbox)
        {
            return authenticatorFactory.GetAuthenticator(mailbox.MailServer.AuthenticationType);
        }
        //protected MimeMessage CheckFrom(MimeMessage msg)
        //{
        //    try
        //    {
        //        var frm = msg.From;
        //    }
        //    catch (Exception)
        //    {
        //        msg.Headers[HeaderId.From] = "";
        //    }
        //    return msg;
        //}

    }
}
