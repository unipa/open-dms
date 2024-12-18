using Microsoft.Extensions.DependencyInjection;
using OpenDMS.Domain.Entities;
using OpenDMS.MailSpooler.Core.Interfaces;

namespace OpenDMS.MailSpooler.Core
{
    public class AuthenticatorFactory : IAuthenticatorFactory
    {
        private readonly IServiceProvider serviceProvider;

        public AuthenticatorFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IAuthenticator GetAuthenticator(AuthenticationType Oauth2Type)
        {
            return serviceProvider.GetServices<IAuthenticator>().FirstOrDefault(s => s.AuthType == Oauth2Type);
        }
    }
}
