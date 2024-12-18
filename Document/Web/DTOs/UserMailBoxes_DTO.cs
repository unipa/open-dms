using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Entities.Settings;

namespace Web.DTOs
{
    public class UserMailBoxes_DTO
    {
        public List<Mailbox> Mailboxes { get; set; } = new();
        public List<Company> Companies { get; set; } = new();
        public List<MailServer> MailServers{ get; set; } = new();

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
    }
}
