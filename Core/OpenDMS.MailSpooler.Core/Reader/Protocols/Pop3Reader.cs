using iText.Commons.Datastructures;
using MailKit;
using MailKit.Net.Pop3;
using MailKit.Search;
using MimeKit;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.MailSpooler.Core.Interfaces;
using OpenDMS.MailSpooler.Receiver.models;

namespace OpenDMS.MailSpooler.Core.Reader.Protocols
{
    public class Pop3Reader : IMailReaderProtocol
    {
        private Pop3Client clPop;
        private IList<UniqueId> uids;
        private Mailbox mailbox;
        private int nextUid = 0;
        private string currentUid;
        private int current;
        private MimeMessage eml;
        private CancellationToken _token;
        private Dictionary<string, int> Indici = new();

        public MailDirection direction => MailDirection.Inbound;
        public InboxProtocol Protocol { get; set; } = InboxProtocol.Pop3;

        public string UID => currentUid;


        public Pop3Reader(Mailbox mailbox, CancellationToken token)
        {
            clPop = new Pop3Client();
            clPop.Timeout = 120000;
            this.mailbox = mailbox;
            this._token = token;
        }
        public void Dispose()
        {
            if (clPop != null)
            {
                clPop.Dispose();
            }
        }

        public async Task Connect()
        {
            await clPop.ConnectAsync(mailbox.MailServer.InboxServer, mailbox.MailServer.InboxServerPort, mailbox.MailServer.InboxSSL, _token);
        }

        public async Task Authenticate(IAuthenticator authenticator)
        {
            authenticator.Authenticate (clPop, mailbox);
        }

        public async Task Disconnect()
        {
            if (clPop.IsConnected)
            await clPop.DisconnectAsync(true, _token);
        }

        public string[] GetFolders()
        {
            return new string[] { "" };
        }

        public async Task<int> Retrieve(DateTime FromDate, string folderName = "")
        {
            nextUid = 0;
            //var header = clPop.GetMessageHeaders(i, _token);
            //uids = clPop.se folder.Search(SearchQuery.DeliveredAfter(FromDate.AddDays(-1)));
            return clPop.Count;

        }

        public async Task<DateTime?> GetNext()
        {
            if (nextUid >= uids.Count) return null;
            current = nextUid;
            currentUid  = await clPop.GetMessageUidAsync (current, _token);

            nextUid++;

            var header = await clPop.GetMessageHeadersAsync (current, _token);
            DateTimeOffset dtMail = DateTimeOffset.Now;

            var dt = header[HeaderId.Date] ?? header[HeaderId.DateReceived] ?? "";
            var msg = await clPop.GetMessageAsync(current, _token);
            Indici.Add(msg.MessageId, current);

            if (!MimeKit.Utils.DateUtils.TryParse(dt, out dtMail))
            {
                dtMail = msg.Date;
            }
            return dtMail.DateTime;
        }

        public async Task<MimeMessage> GetMessage()
        {
            if (eml == null)
                eml = await clPop.GetMessageAsync(current, _token);
            clPop.NoOp(_token);
            return eml;
        }

        public async Task<MimeMessage> GetMessage(string uid, string folderName = "")
        {
            if (!Indici.ContainsKey(uid)) return null;
            var indice = Indici[uid];
            var msg = await clPop.GetMessageAsync(indice, _token);
            clPop.NoOp(_token);
            return msg;
        }

        public async Task<int> Delete(string uid, string folderName)
        {
            if (!Indici.ContainsKey(uid)) return 0;
            var indice = Indici[uid];
            await clPop.DeleteMessageAsync (indice, _token);
            clPop.NoOp(_token);
            return 1;
        }
    }
}
