using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Components;

namespace Web.Pages
{
    public class ConservazioneModel : PageModel
    {
        private readonly IConfiguration config;

        public string EndPointConservazione { get; set; }
       


        public ConservazioneModel(IConfiguration config)
        {
            this.config = config;
        }

        public async Task OnGetAsync()
        {


        }
    }
}
