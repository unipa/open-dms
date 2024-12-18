using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web.Mvc;

namespace Web.Pages
{
    [Authorize]
    public class TaskModel : PageModel
    {


        public void OnGet()
        {
        }
    }
}
