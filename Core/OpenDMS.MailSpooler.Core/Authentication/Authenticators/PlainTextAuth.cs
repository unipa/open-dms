using MailKit;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.MailSpooler.Core;
using OpenDMS.MailSpooler.Core.Interfaces;
using System;

namespace Authenticator
{
    public class PlainTextAuth : IAuthenticator
    {
        public AuthenticationType AuthType => AuthenticationType.UserCredentials;

        public OAUTHToken AcquireCode(string code, string statoString)
        {
            return null;
        }

        public OAUTHToken AcquireCode(string code, Mailbox credential)
        {
            return null;
        }

        public OAUTHToken AcquireIntercativeToken(string MailAddress)
        {
            return null;
        }

        public OAUTHToken AcquireSilentToken(string MailAddress)
        {
            return null;
        }

        public void Authenticate(IMailService server, Mailbox credential, string redirectURU = "")
        {
            if (!String.IsNullOrEmpty(credential.UserId))
                server.Authenticate(System.Text.Encoding.ASCII, credential.UserId, credential.Password);
        }

        public bool EmptyToken(OAUTHToken token)
        {
            return true;
        }

        public string GetOAUTH_URL(OAUTHState stato, Mailbox credential)
        {
            return "";
        }

        public OAUTHToken GetToken(string BD, string UserName, string MailAddress)
        {
            return null;
        }

        public OAUTHToken RefreshToken(Mailbox authToken)
        {
            return null;
        }


        //public void SetConfig(OAUTHConfig mailboxDescriptor)
        //{
        //    //throw new NotImplementedException();
        //}

        public bool SetToken(OAUTHToken token)
        {
            return true;
        }

    }
}
