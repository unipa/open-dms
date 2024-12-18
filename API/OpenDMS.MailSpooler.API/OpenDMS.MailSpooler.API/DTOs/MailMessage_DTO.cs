namespace OpenDMS.MailSpooler.API.DTOs
{
    public class MailMessage_DTO
    {

        public string Id { get; set; }
        public string FromAddress { get; set; }
        public List<string> To { get; set; } = new();
        public List<string> CC { get; set; } = new();
        public List<string> CCr { get; set; } = new();
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<int> Attachments { get; set; } = new();
        public bool IncludePDFPreview { get; set; } = false;

    }
}
