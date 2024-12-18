using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.Filters;
namespace Web.Pages.Tasks
{
    [Authorization(OpenDMS.Domain.Constants.PermissionType.Task_CanView)]
    public class Index2Model : PageModel
    {

        public Index2Model()
        {
        }

        public async Task OnGet()
        {
        }
    }
}
