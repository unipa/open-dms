using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class DistribuzioneModel : PageModel
    {

        private readonly IConfiguration config;

        public string EndPointDistribuzione { get; set; }


        public DistribuzioneModel(IConfiguration config)
        {
            this.config = config;
        }

        public async Task OnGetAsync()
        {
            EndPointDistribuzione = config.GetSection("Endpoint:Distribuzione").Value;
        }
    }


}

