using MimeKit;
using OpenDMS.Core.DTOs;

namespace Web.DTOs
{
    public class MailMessage_DTO
    {

        public int? Id { get; set; }
        public string FromAddress { get; set; }
        public List<string> To { get; set; } = new();
        public List<string> CC { get; set; } = new();
        public List<string> CCr { get; set; } = new();
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<DocumentInfo> Attachments { get; set; } = new();
        public List<MimeEntity> SavedAttachments { get; set; } = new();
        public bool IncludePDFPreview { get; set; } = false;
        public bool LinkAttachments { get; set; } = false;

    }
}
