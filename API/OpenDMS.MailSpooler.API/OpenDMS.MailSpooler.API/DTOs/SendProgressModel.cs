namespace OpenDMS.MailSpooler.API.DTOs
{
    public class SendProgressModel
    {
        public int Queued { get; set; }
        public int Sent { get; set; }
        public int Failed { get; set; }
    }
}
