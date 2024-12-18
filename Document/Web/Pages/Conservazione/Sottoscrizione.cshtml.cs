using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Web.Pages
{
    
    public class SottoscrizioneModel : PageModel
    {
        private readonly IConfiguration config;

        public string EndPoint { get; set; }

        public SottoscrizioneModel(IConfiguration config)
        {
            this.config = config;
        }

        public async Task OnGetAsync()
        {
            EndPoint = config.GetSection("Endpoint:Sottoscrizione").Value;
        }
    }




}
