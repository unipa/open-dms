using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.Filters;
namespace Web.Pages.Tasks
{
    [Authorization(OpenDMS.Domain.Constants.PermissionType.Task_CanView)]
    public class IndexModel : PageModel
    {

        public IndexModel()
        {
        }

        public async Task OnGet()
        {
        }
    }
}
