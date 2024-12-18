using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenDMS.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Web.Model.Admin
{
    public class MailServerViewModel
    {
        [ValidateNever]
        public List<MailServer_DTO> MailServers { get; set; } = new List<MailServer_DTO>();
        [ValidateNever]
        public int Id { get; set; }
        //[RegularExpression("^[\\w]+[.][\\w]+$", ErrorMessage = "Formato del dominio non valido. Il dominio internet fornito non è nel formato corretto. Si prega di inserire un indirizzo valido, ad esempio: \"esempio.com\". Assicurarsi di non includere \"http://\" o \"https://\" nel dominio. ")]
        [Required(ErrorMessage = "Il dominio è obbligatorio.")]
        public string Domain { get; set; }

        [Required(ErrorMessage = "Il MailServer è obbligatorio.")]
        public string InboxServer { get; set; }
        [Required]
        public int InboxServerPort { get; set; }

        public bool InboxSSL { get; set; }

        public InboxProtocol InboxProtocol { get; set; }

        public string? InboxProtocolText
        {
            get
            {
                return ElencoInboxProtocol.FirstOrDefault(p => p.Value == InboxProtocol.ToString())?.Text;
            }
        }
        public List<SelectListItem> ElencoInboxProtocol { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = InboxProtocol.Pop3.ToString(), Text = "Pop3" },
            new SelectListItem { Value = InboxProtocol.IMAP.ToString(), Text = "IMAP" },
            new SelectListItem { Value = InboxProtocol.FileSystemImport.ToString(), Text = "FileSystemImport" },
        };

        [Required(ErrorMessage = "Il MailServer è obbligatorio.")]
        public string SMTPServer { get; set; }

        [Required]
        public int SMTPServerPort { get; set; }

        public bool SMTPServerSSL { get; set; }

        public MailServerStatus Status { get; set; }

        [ValidateNever]
        public string? StatusText
        {
            get
            {
                return ElencoStatus.FirstOrDefault(p => p.Value == ElencoStatus.ToString())?.Text;
            }
        }
        public List<SelectListItem> ElencoStatus { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = MailServerStatus.Active.ToString(), Text = "Active" },
            new SelectListItem { Value = MailServerStatus.Disabled.ToString(), Text = "Disabled" },
        };
        public MailType MailType { get; set; }

        [ValidateNever]
        public string? MailTypeText
        {
            get
            {
                return ElencoMailType.FirstOrDefault(p => p.Value == MailType.ToString())?.Text;
            }
        }
        public List<SelectListItem> ElencoMailType { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = MailType.Mail.ToString(), Text = "Mail" },
            new SelectListItem { Value = MailType.PEC.ToString(), Text = "PEC" },
        };

        public AuthenticationType AuthenticationType { get; set; }

        [ValidateNever]
        public string? AuthenticationTypeText
        {
            get
            {
                return ElencoAuthenticationType.FirstOrDefault(p => p.Value == AuthenticationType.ToString())?.Text;
            }
        }
        public List<SelectListItem> ElencoAuthenticationType { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = AuthenticationType.None.ToString(), Text = "Nessuna" },
            new SelectListItem { Value = AuthenticationType.UserCredentials.ToString(), Text = "UserCredentials" },
            new SelectListItem { Value = AuthenticationType.Microsoft_OAuth.ToString(), Text = "Microsoft_OAuth" },
            new SelectListItem { Value = AuthenticationType.Google_OAuth.ToString(), Text = "Google_OAuth" },
        };
        [ValidateNever]
        public string TenantID { get; set; }
        [ValidateNever]
        public string ClientID { get; set; }
        [ValidateNever]
        public string ClientSecret { get; set; }

        [ValidateNever]
        public string ErrorMessage { get; set; }
        [ValidateNever]
        public string SuccessMessage { get; set; }
        [ValidateNever]
        public string Icon { get; set; }

    }

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
