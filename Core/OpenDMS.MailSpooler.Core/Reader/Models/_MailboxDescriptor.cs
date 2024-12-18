using OpenDMS.Domain.Entities.Mails;

namespace OpenDMS.MailSpooler.Receiver.models
{
    public class MailboxDescriptor 
    {
        public Mailbox MailBox { get; set; }
        public MailServer MailServer { get; set; }

        public CancellationToken Token { get; set; }
    }
}
