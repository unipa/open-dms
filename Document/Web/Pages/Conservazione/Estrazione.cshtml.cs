using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Web.Pages
{
    public class EstrazioneModel : PageModel
    {
        private readonly IConfiguration config;

        public string EndPointEstrazione { get; set; }

        public EstrazioneModel(IConfiguration config)
        {
            this.config = config;
        }

        public async Task OnGetAsync()
        {
            EndPointEstrazione = config.GetSection("Endpoint:Estrazione").Value;
        }
    }
}

