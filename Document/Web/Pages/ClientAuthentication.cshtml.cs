using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using System.Web;

namespace Web.Pages
{
    [Authorize]
    public class ClientAuthenticationModel : PageModel
    {
        private readonly ILoggedUserProfile userContext;
        private readonly IUserSettingsRepository userSettingsRepository;
        private readonly IUserService userService;

        public string Port { get; set; }
        public string ClientSecret { get; set; }
        public string returnURL { get; set; }
        public string AppName { get; set; }
        public string Host { get; set; }

        public ClientAuthenticationModel(
            ILoggedUserProfile userContext,
            IUserSettingsRepository userSettingsRepository,
            IUserService userService
            )
        {
            this.userContext = userContext;
            this.userSettingsRepository = userSettingsRepository;
            this.userService = userService;
        }

        public async Task OnGet(string appName, string port, string fallbackURL)
        {
            var u = userContext.Get();
            var user = await userService.GetById(u.userId);
            var clientSecret = Guid.NewGuid().ToString();
            await userSettingsRepository.Set(user.ContactId, "ClientSecret." + appName.ToLowerInvariant(), clientSecret);
            ClientSecret = clientSecret + "-" + user.Id;
            returnURL = HttpUtility.UrlEncode( fallbackURL);
            AppName = appName;
            Port = port;
            Host ="https://"+Request.Host.Value;
        }
    }
}
