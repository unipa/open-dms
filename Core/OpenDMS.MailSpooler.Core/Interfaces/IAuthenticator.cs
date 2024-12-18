using MailKit;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Mails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.MailSpooler.Core.Interfaces
{
    public interface IAuthenticator
    {
        AuthenticationType AuthType { get; }
        void Authenticate(IMailService server, Mailbox mailbox, string _redirectURI = "");// OAUTHCredential credential);
        OAUTHToken RefreshToken(Mailbox mailbox);
        OAUTHToken GetToken(string BD, string UserName, string mailAddress);
        bool SetToken(OAUTHToken token);
        bool EmptyToken(OAUTHToken token);
        OAUTHToken AcquireIntercativeToken(string MailAddress);
        OAUTHToken AcquireSilentToken(string MailAddress);

        OAUTHToken AcquireCode(string code, Mailbox mailbox);

        string GetOAUTH_URL(OAUTHState stato, Mailbox mailbox);
    }
}
