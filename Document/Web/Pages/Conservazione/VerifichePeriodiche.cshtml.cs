using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class VerifichePeriodicheModel : PageModel
    {
        private readonly IConfiguration config;

        public string EndPointVerifichePeriodiche { get; set; }

        public VerifichePeriodicheModel(IConfiguration config)
        {
            this.config = config;
        }

        public async Task OnGetAsync()
        {
            EndPointVerifichePeriodiche = config.GetSection("Endpoint:VerifichePeriodiche").Value;
        }
    }
}
