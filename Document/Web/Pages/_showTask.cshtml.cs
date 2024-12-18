using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using System.Web.Mvc;

namespace Web.Pages
{
    [Authorize]
    public class showTaskModel : PageModel
    {


        public void OnGet()
        {
        }
    }
}
