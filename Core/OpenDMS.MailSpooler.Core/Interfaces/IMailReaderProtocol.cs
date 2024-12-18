using MimeKit;
using OpenDMS.Domain.Entities;

namespace OpenDMS.MailSpooler.Core.Interfaces
{
    public interface IMailReaderProtocol : IDisposable
    {
        InboxProtocol Protocol { get; }

        MailDirection direction { get; }
        string UID { get; }
        Task Connect();
        Task Authenticate(IAuthenticator authenticator);
        Task Disconnect();
        public string[] GetFolders();
        Task<int> Retrieve(DateTime FromDate, string folderName = "");
        Task<DateTime?> GetNext();
        Task<MimeMessage> GetMessage();
        Task<MimeMessage> GetMessage(string uid, string folderName = "");
        Task<int> Delete(string uid, string folderName);

    }
}
