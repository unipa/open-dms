using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class ResponsabileDelegatiModel : PageModel
    {

        private readonly IConfiguration config;

        public string EndPointRespDelegati { get; set; }
        public string EndPointRespDelegatiDetails { get; set; }


        public ResponsabileDelegatiModel(IConfiguration config)
        {
            this.config = config;
        }

        public async Task OnGetAsync()
        {

            EndPointRespDelegati = config.GetSection("Endpoint:ResponsabileDelegati").Value;

            EndPointRespDelegatiDetails = config.GetSection("Endpoint:ResponsabileDelegatiDetails").Value;

        }





    }
}


