using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text.Encodings.Web;
using System.Web;

namespace DesktopService.Pages
{
    public class SignInModel : PageModel
    {
        private readonly IServer server;

        public string ReturnURL { get; set; } = "";
        public string Host { get; set; } = "";
        public string Port { get; set; } = "";

        public SignInModel(IServer server)
        {
            this.server = server;
        }

        public void OnGet(string host, string returnUrl)
        {

            //            var server = host.Services.GetRequiredService<IServer>();
            //var addressFeature = server.Features.Get<IServerAddressesFeature>();
            //foreach (var address in addressFeature.Addresses)
            //{
           var address = Request.Host;
            //var uri = address. new Uri(address);
            Port = (address.Port ?? 80).ToString(); // uri.Port.ToString();
            //    break;
            //}
            Host =HttpUtility.UrlDecode(host);
            ReturnURL = HttpUtility.UrlEncode( returnUrl);
        }
    }
}
