using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.DTOs;
using System.Text;



namespace DesktopService.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IServer server;
        private string ClientSecret = "";
        private string Host = "";
        public string[] Addresses { get; set; }
        public string UserName { get; set; }


        public IndexModel(ILogger<IndexModel> logger, IServer server)
        {
            _logger = logger;
            this.server = server;
        }
        private async Task GetSecret()
        {
            ClientSecret = "";
            var clientPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "dss");
            if (!Directory.Exists(clientPath)) Directory.CreateDirectory(clientPath);
            var clientFile = Path.Combine(clientPath, "dss.secret");
            if (System.IO.File.Exists(clientFile))
                ClientSecret = System.IO.File.ReadAllText(clientFile);
            if (string.IsNullOrEmpty(ClientSecret))
            {
                RedirectToPage("/SignIn?host=" + Host + "&returnUrl=" + Request.Path);
                return;
            }
        }

        public async Task OnGet()
        {
            Host = Request.Headers["Referer"].ToString();
            if (!String.IsNullOrEmpty(Host))
            {
                await GetSecret();
            }
            Addresses = server.Features.Get<IServerAddressesFeature>()?.Addresses?.ToArray();
            UserName = Environment.UserName;
        }


    }
}