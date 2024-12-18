using System;

namespace OpenDMS.MailSpooler.Core
{
    public class OAUTHToken : OAUTHState
    {
        public String MailType { get; set; }
        public String Token { get; set; }
        public String RefreshToken { get; set; }
    }
}
