using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.Filters;

namespace Web.Pages.BPMMonitor
{
    [Authorization(OpenDMS.Domain.Constants.PermissionType.WorkflowDashboard)]

    public class DocumentTasksModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
