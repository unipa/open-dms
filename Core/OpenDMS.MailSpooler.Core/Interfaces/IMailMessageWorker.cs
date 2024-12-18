namespace OpenDMS.MailSpooler.Core.Interfaces
{
    public interface IMailMessageWorker
    {
        public bool StartListenForNewMailMessages();
        public bool StopListenForNewMailMessages();
    }
}
