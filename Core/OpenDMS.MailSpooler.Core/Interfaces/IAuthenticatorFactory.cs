using OpenDMS.Domain.Entities;

namespace OpenDMS.MailSpooler.Core.Interfaces
{
    public interface IAuthenticatorFactory
    {
        IAuthenticator GetAuthenticator(AuthenticationType Oauth2Type);
    }
}