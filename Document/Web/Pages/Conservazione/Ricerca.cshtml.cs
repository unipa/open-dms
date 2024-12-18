using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class RicercaModel : PageModel
    {
        private readonly IConfiguration config;

        public string EndPointRicercaArchivio{ get; set; }

        public string EndPointRicercaDoc { get; set; }
        public RicercaModel(IConfiguration config)
        {
            this.config = config;
        }

        public async Task OnGetAsync()
        {
            EndPointRicercaArchivio = config.GetSection("Endpoint:RicercaArchivio").Value;

            EndPointRicercaDoc = config.GetSection("Endpoint:RicercaArchivio").Value;
        }
    }
}
