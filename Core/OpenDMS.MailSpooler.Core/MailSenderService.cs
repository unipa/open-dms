using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using OpenDMS.MailSpooler.Core.Interfaces;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace OpenDMS.MailSpooler.Core
{
    public class MailSenderService : IMailSenderService
    {
        private readonly IMailboxService _mailboxService;
        private readonly IMailEntryService mailEntryService;
        private readonly IAuthenticatorFactory _authenticatorFactory;
        private readonly IAppSettingsRepository appSettingsRepository;
        private readonly IDocumentService documentService;
        private readonly ILogger<MailSenderService> _logger;
        public MailSenderService(
            IMailboxService mailboxService,
            IMailEntryService mailEntryService,
            IAuthenticatorFactory authenticatorFactory,
            IAppSettingsRepository appSettingsRepository,
            IDocumentService documentService,
            ILogger<MailSenderService> logger)
        {
            _mailboxService = mailboxService;
            this.mailEntryService = mailEntryService;
            _authenticatorFactory = authenticatorFactory;
            this.appSettingsRepository = appSettingsRepository;
            this.documentService = documentService;
            _logger = logger;
        }


        public async Task<MailEntry> SendMail(int entryId, UserProfile userProfile, string workerId = "Interactive")
        {
            MailEntry entry = await mailEntryService.GetById(entryId);
            return await SendMail(entry, userProfile, workerId);
        }
        public async Task<MailEntry> SendMail(MailEntry entry, UserProfile userProfile, string workerId = "Interactive")
        {
            if (entry.Direction != MailDirection.Outbound) return null;
            if (!string.IsNullOrEmpty(entry.WorkerId)) return null;
            int timeOut = 3;
            try
            {
                //DateTime NextCheck = DateTime.UtcNow.AddMinutes(timeOut);
                //if (!await mailEntryService.Take(entry.Id, workerId, NextCheck)) return null;

                await mailEntryService.ChangeStatus (entry, SpecialUser.SystemUser, MailStatus.Sending);
                var eml = await mailEntryService.GetContent(entry);
                if (eml != null)
                    await _SendMail(eml);
                await mailEntryService.ChangeStatus(entry, SpecialUser.SystemUser, MailStatus.Sent);
            }
            catch (Exception ex)
            {
                await mailEntryService.ChangeStatus(entry, SpecialUser.SystemUser, MailStatus.Failed, ex.Message);
            }
            return entry;
        }



        public async Task<CreateOrUpdateMailMessage> CreateNewMessage(Mailbox mailbox, UserProfile userProfile, List<int> Attachments)
        {
            CreateOrUpdateMailMessage e = new CreateOrUpdateMailMessage();
            MailEntry entry = new MailEntry();
            MimeMessage eml = new MimeMessage();
            e.Attachments = Attachments;
            e.Body = "";
            e.SendDate = DateTime.UtcNow;
            e.Status = MailStatus.Draft;
            e.EntryId = 0;
            e.MailboxId = mailbox.Id;
            e.IncludePDFPreview = entry.IncludePDFPreview;
            e.LinkAttachments = entry.LinkAttachments;
            return e;
        }
        public async Task<CreateOrUpdateMailMessage> CreateNewMessage(Mailbox mailbox, UserProfile userProfile, int referTo = 0, bool forward = false)
        {
            CreateOrUpdateMailMessage e = new CreateOrUpdateMailMessage();
            MailEntry entry = referTo > 0 ? await mailEntryService.GetById(referTo) : new MailEntry();
            MimeMessage eml = await mailEntryService.GetContent(entry);

            if (!forward && referTo > 0)
            {
                e.To = entry.ExternalMailAddress.Split(",").ToList();
                e.CC = entry.ExternalMailAddressCC.Split(",").ToList();
                e.CCr = entry.ExternalMailAddressCCr.Split(",").ToList();
            }
            e.Subject = referTo <= 0 ? "" : (forward ? "I:" : "Re:") + entry.MessageTitle;
            string PreviousMessage = "<br/><hr/>" + mailEntryService.GetMailHtml(eml, true);
            e.Body = PreviousMessage;

            e.SendDate = DateTime.UtcNow;
            e.Status = MailStatus.Draft;
            e.EntryId = 0;
            e.MailboxId = mailbox.Id;
            e.IncludePDFPreview = entry.IncludePDFPreview;
            e.LinkAttachments = entry.LinkAttachments;

            return e;
        }
        public async Task<MailEntry> Save(CreateOrUpdateMailMessage MailMessage, UserProfile userProfile)
        {
            var Mail = MailMessage.EntryId > 0 ? await mailEntryService.GetById(MailMessage.EntryId) : new MailEntry();

            string iUrl = await appSettingsRepository.Get("URL.Internal") ?? "";
            string eUrl = await appSettingsRepository.Get("URL.External") ?? "";
            string aUrl = await appSettingsRepository.Get("URL.App") ?? "";

            Mailbox mailbox = await _mailboxService.GetById(MailMessage.MailboxId);
            var FromAddress = String.IsNullOrEmpty(mailbox.DisplayName) ? new MailboxAddress(userProfile.UserInfo.Contact.FullName, mailbox.MailAddress).ToString() : new MailboxAddress(mailbox.DisplayName, mailbox.MailAddress).ToString();
            string GUID = (Mail.Id <= 0) ? Guid.NewGuid().ToString() : Mail.MessageId;
            try
            {
                MimeKit.MimeMessage eml = new MimeKit.MimeMessage();
                eml.MessageId = GUID;
                eml.From.Add(MailboxAddress.Parse(FromAddress));
                eml.Date = MailMessage.SendDate <= DateTime.MinValue ? DateTime.UtcNow : MailMessage.SendDate;
                eml.Subject = MailMessage.Subject;
                foreach (var a in MailMessage.To)
                {
                    var ad = MailboxAddress.Parse(a);
                    eml.To.Add(new MailboxAddress(ad.Name, ad.Address));
                }
                foreach (var a in MailMessage.CC)
                {
                    var ad = MailboxAddress.Parse(a);
                    eml.Cc.Add(new MailboxAddress(ad.Name, ad.Address));
                }
                foreach (var a in MailMessage.CCr)
                {
                    var ad = MailboxAddress.Parse(a);
                    eml.Bcc.Add(new MailboxAddress(ad.Name, ad.Address));
                }
                var multipart = new Multipart("mixed");
                var part = new TextPart(MimeKit.Text.TextFormat.Html) { Text = MailMessage.Body.Replace("{internal-url}", iUrl).Replace("{external-url}", eUrl) };
                multipart.Add(part);
                if (MailMessage.Attachments != null)
                {
                    foreach (var docId in MailMessage.Attachments)
                    {
                        var doc = await documentService.Get(docId);
                        if (doc != null && doc.ImageId > 0)
                        {
                            DocumentImage immagineDoc = doc.Image;
                            if (immagineDoc == null)
                                immagineDoc = await documentService.GetContentInfo(Convert.ToInt32(doc.ImageId));
                            if (MailMessage.LinkAttachments)
                            {
                                part.Text += "<hr/>L'allegato '<b>" + doc.DocumentType + "</b>:" + doc.Description + "' è disponibile ai seguenti link: <a target='_blank' href='" + iUrl + "?id=" + doc.Id + "'>Intranet</a>" + (!string.IsNullOrEmpty(eUrl) ? " | <a target='_blank' href='" + eUrl + "?id=" + doc.Id + "'>Internet</a>" : "");
                            }
                            else
                            {
                                var data = await documentService.GetContent(immagineDoc.Id);
                                var att = new MemoryStream(data);
                                {
                                    string path = immagineDoc.OriginalFileName;
                                    var attachment = new MimePart()
                                    {
                                        Content = new MimeContent(att, ContentEncoding.Default),
                                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                                        ContentTransferEncoding = ContentEncoding.Base64,
                                        FileName = Path.GetFileName(path)
                                    };
                                    multipart.Add(attachment);
                                }
                                if (MailMessage.IncludePDFPreview)
                                {
                                    var pdfdata = await documentService.GetPreview(immagineDoc);
                                    if (pdfdata != null)
                                    {
                                        var pdfstreamdata = new MemoryStream(pdfdata);
                                        {
                                            //var image = await documentService.GetImage(doc.ImageId.Value);
                                            string path = immagineDoc.OriginalFileName + ".pdf";
                                            var attachment = new MimePart()
                                            {
                                                Content = new MimeContent(pdfstreamdata, ContentEncoding.Default),
                                                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                                                ContentTransferEncoding = ContentEncoding.Base64,
                                                FileName = Path.GetFileName(path)
                                            };
                                            multipart.Add(attachment);
                                        }
                                    }
                                }


                            }
                        }
                        else
                        {
                            //TODO: Segnalare che un allegato è stato rimosso
                            part.Text += "<hr/>ATTENZIONE: Un allegato non è stato spedito perchè non più presente nel sistema (Id:" + docId + ")";
                        }
                    }
                    eml.Headers.Add("_dms_documents_", Newtonsoft.Json.JsonConvert.SerializeObject(MailMessage.Attachments));
                }
                eml.Body = multipart;

                if (Mail.Id <= 0)
                    Mail = await mailEntryService.Add (GUID, mailbox, userProfile.userId, eml, MailMessage.SendDate, MailDirection.Outbound, MailMessage.Status);
                else
                    Mail = await mailEntryService.Update (Mail, mailbox, userProfile.userId, eml, MailMessage.SendDate, MailMessage.Status);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save: " + ex.Message);
                throw;
            }
            return Mail;
        }
        public async Task<CreateOrUpdateMailMessage> GetById(int entryId, UserProfile userProfile)
        {
            CreateOrUpdateMailMessage e = new CreateOrUpdateMailMessage();
            MailEntry Mail = await mailEntryService.GetById(entryId);

            Mailbox mailbox = await _mailboxService.GetById(Mail.MailboxId);

            e.SendDate = Mail.MessageDate ?? DateTime.UtcNow;
            e.Subject = Mail.MessageTitle;
            e.Status = Mail.Status;
            e.EntryId = entryId;
            e.MailboxId = Mail.MailboxId;
            e.IncludePDFPreview = Mail.IncludePDFPreview;
            e.LinkAttachments = Mail.LinkAttachments;
            e.FromUser = Mail.Direction == MailDirection.Inbound ? Mail.ExternalMailAddress : Mail.InternalMailAddress;
            e.To = (Mail.Direction != MailDirection.Inbound ? Mail.ExternalMailAddress : Mail.InternalMailAddress).Split(",").ToList();
            e.CC = Mail.ExternalMailAddressCC.Split(",").ToList();
            e.CCr = Mail.ExternalMailAddressCCr.Split(",").ToList();

            if (Mail.ImageId > 0)
            {
                var image = await documentService.GetContent(Mail.ImageId);
                var M = new MemoryStream(image);
                MimeMessage eml = await MimeMessage.LoadAsync(M);

                e.FromUser = eml.From.ToString();
                e.To = new List<string>(eml.To.Select(a => a.ToString()));
                e.CC = new List<string>(eml.Cc.Select(a => a.ToString()));
                e.CCr = new List<string>(eml.Bcc.Select(a => a.ToString()));
                e.Body = eml.GetTextBody(MimeKit.Text.TextFormat.Html);
                e.Attachments = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(eml.Headers["_dms_documents_"]);
            }
            return e;
        }


        private async Task _SendMail(MimeMessage Mail)
        {
            Mailbox mailbox = await _mailboxService.GetByAddress(((MimeKit.MailboxAddress)Mail.From.First()).Address);
            if (mailbox == null)
                throw new Exception("Mailbox non trovata (" + ((MimeKit.MailboxAddress)Mail.From.First()).Address + ")");
            await _SendMail(Mail, mailbox);
        }
        private async Task _SendMail(MimeMessage Mail, Mailbox mailbox)
        {
            MailServer mailServer = mailbox.MailServer; // await _mailServer.GetById(mailbox.MailServerId);
            if (mailServer == null) return;
            //if ((AuthenticationType)mailServer.AuthenticationType != AuthenticationType.UserCredentials && (AuthenticationType)mailServer.AuthenticationType != AuthenticationType.None &&  String.IsNullOrEmpty(mailbox.TokenId))
            //    throw new Exception(String.Format("Oauth Token vuoto per l'indirizzo: {0} - Utente: {1}", mailbox.MailAddress, mailbox.Account));


            MailKit.Net.Smtp.SmtpClient Smtp = new MailKit.Net.Smtp.SmtpClient();
            Smtp.ServerCertificateValidationCallback = CertificateValidationCallback;
            Smtp.Connect(mailServer.SMTPServer, mailServer.SMTPServerPort, mailServer.SMTPServerSSL ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTlsWhenAvailable);
            if (Smtp.IsConnected)
            {
                _logger.LogInformation($"Connessione riuscita al server: {mailServer.SMTPServer} ");
                try
                {
                    if (mailServer.AuthenticationType != AuthenticationType.None)
                    {
                        var authenticator = _authenticatorFactory.GetAuthenticator(mailServer.AuthenticationType);
                        if (authenticator != null)
                        {
                            try
                            {
                                authenticator.Authenticate(Smtp, mailbox);
                                _logger.LogInformation($"Autenticazione riuscita sul server: {mailServer.SMTPServer}");
                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    var authToken = authenticator.RefreshToken(mailbox);
                                    if (authToken != null)
                                    {
                                        mailbox.TokenId = authToken.Token;
                                        mailbox.RefreshToken = authToken.RefreshToken;
                                        await _mailboxService.Update(mailbox);
                                        //credential.Oauth2Token = authToken.Token;
                                        authenticator.Authenticate(Smtp, mailbox);
                                        _logger.LogInformation($"Autenticazione riuscita sul server: {mailServer.SMTPServer}");
                                    }
                                    else throw new Exception("Auth Token null");
                                }
                                catch (Exception ex1)
                                {
                                    throw (ex1);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                    throw new Exception("Errore di autenticazione  --> " + ex.Message, ex);
                }
                try
                {
                    Smtp.Send(Mail);
                    _logger.LogInformation($"Mail mandata correttamente");
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
            else
                throw new Exception($"Impossibile connettersi al server {mailServer.SMTPServer}");
        }


        #region UTILS
        private bool CertificateValidationCallback(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        #endregion
    }
}


