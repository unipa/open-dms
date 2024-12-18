using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.MailSpooler.Core.Interfaces;
using System.Text.RegularExpressions;

namespace OpenDMS.MailSpooler.Core.Reader.Protocols
{
    public class ImapReader : IMailReaderProtocol, IDisposable
    {
        private ImapClient clImap;
        private IList<UniqueId> uids;
        private Mailbox mailbox;
        private readonly CancellationToken token;
        private int nextUid = 0;
        private IMailFolder folder = null;
        private UniqueId currentUid;
        private MimeMessage eml;
        private CancellationToken _token;

        public InboxProtocol Protocol { get; set; } = InboxProtocol.IMAP;

        public ImapReader(Mailbox mailbox, CancellationToken token)
        {
            clImap = new ImapClient();
            clImap.Timeout = 120000;
            this.mailbox = mailbox;
            _token = token;
        }

        public MailDirection direction { get {
                return (folder.Attributes & FolderAttributes.Sent) != 0 ? MailDirection.Outbound : MailDirection.Inbound;
            } }
        public string UID { get {
                return currentUid.ToString();
            } }


        public async Task Connect()
        {
            await clImap.ConnectAsync(mailbox.MailServer.InboxServer, mailbox.MailServer.InboxServerPort, mailbox.MailServer.InboxSSL, _token);
        }
        public async Task Authenticate(IAuthenticator authenticator)
        {
                authenticator.Authenticate (clImap, mailbox);
        }
        public async Task Disconnect()
        {
            if (clImap.IsConnected)
                await clImap.DisconnectAsync(true, _token);
        }
        public string[] GetFolders()
        {
            string imapFolderString = mailbox.DownloadImapFolders ?? "";
            return imapFolderString.Split('|');
        }
        public async Task<int> Retrieve(DateTime FromDate, string folderName = "")
        {
            nextUid = 0;
            string f = folderName;
            if (folder != null) folder.Close();

            if (!string.IsNullOrEmpty(f) && f.ToUpper() != "INBOX")
                folder = clImap.GetFolder(f);
            else
                folder = clImap.Inbox;

            folder.Open(FolderAccess.ReadOnly);
            uids = folder.Search(SearchQuery.DeliveredAfter(FromDate));
            return uids.Count;
        }
        public async Task<DateTime?> GetNext()
        {
            if (nextUid >= uids.Count) return null;
            currentUid = uids[nextUid];
            nextUid++;
            eml = null;
            var header = await folder.GetHeadersAsync(currentUid, _token);
            DateTimeOffset dtMail = DateTimeOffset.Now;
            var dt = header[HeaderId.Date] ?? header[HeaderId.DateReceived] ?? "";
            if (!MimeKit.Utils.DateUtils.TryParse(dt, out dtMail))
            {
                eml = await folder.GetMessageAsync(currentUid, _token);
                dtMail = eml.Date;
            }
            return dtMail.DateTime;
        }
        public async Task<MimeMessage> GetMessage()
        {
            if (eml == null)
                eml = await folder.GetMessageAsync(currentUid, _token);
            clImap.NoOp(_token);
            return eml;
        }
        public async Task<MimeMessage> GetMessage(string uid, string folderName = "")
        {
            if (folder == null || folder.FullName != folderName)
            {
                if (String.IsNullOrEmpty(folderName))
                    folder = clImap.Inbox;
                else
                {
                    folder = await clImap.GetFolderAsync(folderName ?? "");
                }
                folder.Open(FolderAccess.ReadOnly, _token);
            }
            var msg = await folder.GetMessageAsync(UniqueId.Parse(uid), _token);
            clImap.NoOp(_token);
            return msg;
        }
        public async Task<int> Delete(string uid, string folderName)
        {
            int Deleted = 0;
            if (!string.IsNullOrEmpty(uid))
            {
                if (folder.FullName != folderName)
                {
                    folder = await clImap.GetFolderAsync(folderName);
                    folder.Open(FolderAccess.ReadOnly, _token);
                }
                else
                    folder.Open(FolderAccess.ReadWrite, _token);

                var storicoFolder = mailbox.SaveToImapFolder;
                if (!String.IsNullOrEmpty(storicoFolder))
                {
                    var savefolder = await GetFolder (storicoFolder);
                    await folder.MoveToAsync(UniqueId.Parse(uid), savefolder, _token);
                }
                else
                {
                    //Verifico i giorni da tenere online
                    folder.AddFlags(UniqueId.Parse(uid), MessageFlags.Deleted, true, _token);
                    folder.Expunge(_token);
                }
                Deleted++;
            }
            else
            {
                var listaUID = folder.Search(SearchQuery.All, _token) ?? new List<UniqueId>();
                if (listaUID.Count > 0)
                {
                    foreach (var id in listaUID)
                    {
                        folder.SetFlags(id, MessageFlags.Deleted, true, _token);
                        Deleted++;
                    }
                    folder.Expunge(_token);
                }
            }
            clImap.NoOp(_token);
            return Deleted;
        }

        public void Dispose()
        {
                clImap.Dispose();
        }


        private async Task<IMailFolder> GetFolder(string folderName)
        {
            IMailFolder result = null;
            if (!string.IsNullOrEmpty(folderName))
            {
                var now = DateTime.Now;
                folderName = Regex.Replace(folderName, @"\[DATA_ORA\]", now.ToString("yyyyMMdd_HHmmss"), RegexOptions.IgnoreCase);
                folderName = Regex.Replace(folderName, @"\[DATA\]", now.ToString("yyyyMMdd"), RegexOptions.IgnoreCase);
                folderName = Regex.Replace(folderName, @"\[OGGI\]", now.ToString("yyyyMMdd"), RegexOptions.IgnoreCase);
                folderName = Regex.Replace(folderName, @"\[ANNO\]", now.ToString("yyyy"), RegexOptions.IgnoreCase);
                folderName = Regex.Replace(folderName, @"\[MESE\]", now.ToString("MM"), RegexOptions.IgnoreCase);
                folderName = Regex.Replace(folderName, @"\[GIORNO\]", now.ToString("dd"), RegexOptions.IgnoreCase);

                //P.Mirto
                //Perchè non usa GetFolder ?
                //result = clImap.Inbox.GetSubfolders().FirstOrDefault(p => p.Name.Equals(storico, StringComparison.InvariantCultureIgnoreCase));
                result = await clImap.GetFolderAsync(folderName, _token);
                if (result == null)
                    result = await clImap.Inbox.CreateAsync(folderName, true, _token);
            }
            return result;
        }
    }
}


        //public async Task<IList<DownloaderResult>> Download(Mailbox mailbox)
        //{
        //    IList<DownloaderResult> result = new List<DownloaderResult>();
        //    logger.LogInformation($"IMAP: Iniziate le attività per {mailbox.MailAddress} su {mailbox.MailServer.InboxServer} ...");
        //    using (var clImap = GetImapClient())
        //    {
        //        try
        //        {
        //            clImap.Connect(mailbox.MailServer.InboxServer, mailbox.MailServer.InboxServerPort, mailbox.MailServer.InboxSSL, _token);

        //            if (clImap.IsConnected)
        //                logger.LogInformation($"Connessione riuscita al server: {mailbox.MailServer.InboxServer}:{mailbox.MailServer.SMTPServerPort}(SSL:{mailbox.MailServer.InboxSSL}) per la casella: {mailbox.MailAddress}");

        //            var authenticator = GetAuthenticator(mailbox);

        //            try
        //            {
        //                authenticator.Authenticate(clImap, mailbox);
        //            }
        //            catch (Exception)
        //            {
        //                try
        //                {
        //                    var authToken = authenticator.RefreshToken(mailbox);
        //                    if (authToken != null)
        //                    {
        //                        mailbox.TokenId = authToken.Token;
        //                        mailbox.RefreshToken = authToken.RefreshToken;
        //                        await mailboxRepo.Update(mailbox);
        //                        //credential.Oauth2Token = authToken.Token;
        //                        authenticator.Authenticate(clImap, mailbox);
        //                    }
        //                    else throw new Exception("Auth Token null");
        //                }
        //                catch (Exception ex1)
        //                {
        //                    throw (ex1);
        //                }
        //            }
        //            if (clImap.IsAuthenticated)
        //                logger.LogInformation($"Autenticazione riuscita sul server: {mailbox.MailServer.InboxServer}:{mailbox.MailServer.SMTPServerPort}(SSL:{mailbox.MailServer.InboxSSL}) per la casella: {mailbox.MailAddress}");

        //            DateTime ArchiviaDal = mailbox.FirstReceivingMessageDate?.Date ?? DateTime.MinValue;
        //            DateTime UltimaLettura = await mailEntryRepo.GetLastReceived(mailbox.Id);
        //            logger.LogInformation($"Avvio lettura messaggi a partire dalla data: {UltimaLettura.ToShortDateString}");

        //            //IMailFolder storicoFolder = GetStoricoFolder(clImap, mailbox);
        //            //if (storicoFolder != null)
        //            //{
        //            //    logger.LogInformation($"I messaggi acquisiti per la casella: {mailbox.MailAddress} saranno spostati su: {storicoFolder.FullName}");
        //            //}
        //            // TODO: Fare qualcosa per bloccare la lettura della mailbox
        //            await UpdateMailbox(mailbox, "");
        //            IList<UniqueId> uids;

        //            IMailFolder folder; // = clImap.Inbox;
        //            string imapFolderString = mailbox.DownloadImapFolders;

        //            bool InboxLetta = false;
        //            foreach (var f in imapFolderString.Split('|'))
        //            {
        //                if (!string.IsNullOrEmpty(f) && f.ToUpper() != "INBOX")
        //                    folder = clImap.GetFolder(f);
        //                else
        //                {
        //                    // Evito di rileggere la inbox se dovessero esserci più valori vuoti
        //                    if (InboxLetta) continue;
        //                    folder = clImap.Inbox;
        //                    InboxLetta = true;
        //                }
        //                logger.LogInformation($"Apertura cartella {f} per la casella: {mailbox.MailAddress}");

        //                var Lista = new DownloaderResult();
        //                result.Add(Lista);
        //                Lista.Folder = f;
        //                folder.Open(FolderAccess.ReadOnly);
        //                if (UltimaLettura == DateTime.MinValue)
        //                    UltimaLettura = ArchiviaDal;
        //                Lista.FirstDate = ArchiviaDal;
        //                uids = folder.Search(SearchQuery.DeliveredAfter(ArchiviaDal.AddDays(-1)));

        //                totalMessagesCount = uids.Count;
        //                Lista.Letti = totalMessagesCount;

        //                logger.LogInformation($"Trovati {uids.Count} messaggi da controllare per la casella: {mailbox.MailAddress}");

        //                var Direzione = MailDirection.Inbound;
        //                if ((folder.Attributes & FolderAttributes.Sent) != 0)
        //                    Direzione = MailDirection.Outbound;

        //                foreach (var uid in uids)
        //                {

        //                    if (_token.IsCancellationRequested)
        //                        break;

        //                    var header = folder.GetHeaders(uid, _token);

        //                    var MessageId = header[HeaderId.MessageId];
        //                    if (!string.IsNullOrEmpty(MessageId) && MessageId.Length > 255)
        //                        MessageId = MessageId.Substring(0, 254);

        //                    if (string.IsNullOrEmpty(MessageId))
        //                    {
        //                        MessageId = GetFakeMessageId(header);
        //                        logger.LogError($"Messaggio #{uid} senza Id. Generato Fake #{MessageId}");
        //                    }

        //                    DateTimeOffset dtMail = DateTimeOffset.Now;
        //                    var dt = header[HeaderId.Date] ?? header[HeaderId.DateReceived] ?? "";
        //                    if (!MimeKit.Utils.DateUtils.TryParse(dt, out dtMail))
        //                    {
        //                        var msg = folder.GetMessage(uid, _token);
        //                        dtMail = msg.Date;
        //                    }

        //                    if (dtMail.UtcDateTime >= ArchiviaDal.ToUniversalTime().Date)
        //                    {
        //                        var e = await mailEntryRepo.GetByMessageId(MessageId, mailbox.Id);
        //                        if (e == null)
        //                        {           //Inietto i miei plugin
        //                            var msg = await folder.GetMessageAsync(uid, _token);
        //                            msg.MessageId = MessageId;
        //                            //
        //                            //if (!PluginFactory.Instance.GetRetrieverPlugins().All(p => p.PreDownload(msg, mailbox)))
        //                            //    continue;

        //                            //P.Mirto
        //                            //Aggiunto folder (f) in salvataggio messaggio.
        //                            //f per inbox è vuoto o "INBOX"
        //                            e = await mailSpooler.CreateNewEntry(mailbox, msg, dtMail.DateTime, Direzione, MailStatus.Received, uid.ToString());
        //                            if (e != null)
        //                            {
        //                                Lista.Scaricati += 1;
        //                                Lista.Messaggi.Add(e.Id);
        //                                newMessagesCount++;
        //                            }
        //                            else
        //                            {
        //                                logger.LogError($"Impossibile scaricare il messaggio #{uid} - MessageId: {MessageId}' - Casella: {mailbox.MailAddress}");
        //                                //ErrorHandler.Instance.SetError(mailbox.BD, mailbox.User, mailbox.eMail, ModuleEnum.Retriever, string.Format("Mail con UIDL: '{0}' NON scaricata", eMailEntry.UIDL));
        //                            }
        //                        }
        //                    }
        //                    currentMessagesCount++;
        //                    clImap.NoOp(_token);
        //                }

        //            }
        //            mailbox.LastReceivingDate = DateTime.UtcNow;
        //            mailbox.LastReceivingError = "";
        //            await mailboxRepo.Update(mailbox);
        //            //ConfigurationManager.UserSettings[mailbox.BD, mailbox.User, mailbox.Tipo + "_PrimoAvvio"] = "N";
        //            //SetLastConnection(mailbox.eMail);
        //            //ErrorHandler.Instance.ClearError(mailbox.eMail, ModuleEnum.Retriever);
        //            clImap.Disconnect(true, _token);
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.LogError("Casella IMAP: " + mailbox.MailAddress + " - Errore: " + ex.Message, ex);
        //            await UpdateMailbox(mailbox, ex.Message);
        //            throw ex;
        //        }
        //    }
        //    return result;
        //}






        //private void EmptyFolder(ImapClient clImap, SpecialFolder specialFolder)
        //{
        //    string folderString = EnumHelper<SpecialFolder>.GetEnumDescription(specialFolder);
        //    try
        //    {
        //        var folder = clImap.GetFolder(specialFolder);
        //        folder.Open(FolderAccess.ReadWrite, _token);
        //        var listaUID = folder.Search(SearchQuery.All, _token) ?? new List<UniqueId>();
        //        int Deleted = 0;
        //        if (listaUID.Count > 0)
        //        {
        //            logger.LogInformation($"Trovati {listaUID.Count} messaggi da cancellare nella cartella: {folderString}");
        //            foreach (var uid in listaUID)
        //            {
        //                folder.SetFlags(uid, MessageFlags.Deleted, true, _token);
        //                Deleted++;
        //            }
        //            folder.Expunge(_token);
        //            logger.LogInformation($"Eliminati {Deleted} messaggi dalla cartella: {folderString}");
        //        }
        //        else logger.LogInformation($"Cartella {folderString} vuota");
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError("ERRORE IN EmptyFolder. Ex: " + ex.Message, ex);
        //    }
        //}

 
      


        //private ImapClient GetImapClient()
        //{
        //    ImapClient clImap;
        //    clImap = new ImapClient();
        //    clImap.Timeout = 120000;

        //    return clImap;
        //}
