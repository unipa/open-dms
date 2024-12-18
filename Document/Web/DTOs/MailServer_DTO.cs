using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenDMS.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Web.DTOs
{
    public class MailServer_DTO
    {
        public int Id { get; set; }
        public string Domain { get; set; }

        public string InboxServer { get; set; }

        public int InboxServerPort { get; set; }

        public bool InboxSSL { get; set; }

        public InboxProtocol InboxProtocol { get; set; }

        public string SMTPServer { get; set; }

        public int SMTPServerPort { get; set; }

        public bool SMTPServerSSL { get; set; }

        public MailServerStatus Status { get; set; }
        public MailType MailType { get; set; }

        public AuthenticationType AuthenticationType { get; set; }

        public string TenantID { get; set; }
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }

        public MailServer_DTO()
        {
            Domain = "";
            InboxServer = "";
            InboxServerPort = 0;
            InboxSSL = false;
            InboxProtocol = InboxProtocol.Pop3;
            SMTPServer = "";
            SMTPServerPort = 0;
            SMTPServerSSL = false;
            Status = MailServerStatus.Active;
            MailType = MailType.Mail;
        }

    }

}
