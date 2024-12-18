using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Web.Pages
{
    public class VerificaModel : PageModel
    {

        private readonly IConfiguration config;

        public string EndPointVerifica { get; set; }

        public VerificaModel(IConfiguration config)
        {
            this.config = config;
        }

        public async Task OnGetAsync()
        {
            EndPointVerifica = config.GetSection("Endpoint:Verifica").Value;
        }

    }

}

