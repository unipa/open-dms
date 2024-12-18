using OpenDMS.Domain.Entities;
using System;

namespace OpenDMS.MailSpooler.Core
{
    public class OAUTHState
    {
        public MailType Mailtype { get; set; }
        public int MailboxID { get; set; }
        public AuthenticationType OauthType { get; set; }
        public String eMail { get; set; }
        public String UserName { get; set; }
        public String ReturnURL { get; set; }
        public String ExternalURLLogin { get; set; }
    }

}
