using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenDMS.Domain.Entities.Mails
{
    [Index(nameof(Domain), IsUnique = true)]
    public class MailServer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [StringLength(255)]
        public string Domain { get; set; }

        [StringLength(255)]
        public string InboxServer { get; set; }

        public int InboxServerPort { get; set; }

        public bool InboxSSL { get; set; }

        public InboxProtocol InboxProtocol { get; set; }

        [StringLength(255)]
        public string SMTPServer { get; set; }

        public int SMTPServerPort { get; set; }

        public bool SMTPServerSSL { get; set; }

        public DateTime? LastConnection { get; set; }

        public string LastConnectionStatus { get; set; }

        public MailServerStatus Status { get; set; }

        public MailType MailType { get; set; }

        public AuthenticationType AuthenticationType { get; set; }

        [StringLength(255)]
        public string TenantID { get; set; }
        [StringLength(255)]
        public string ClientID { get; set; }
        [StringLength(255)]
        public string ClientSecret { get; set; }

    }
}
