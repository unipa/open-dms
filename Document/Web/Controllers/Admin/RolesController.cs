using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Filters;
using OpenDMS.Domain.Models;

namespace Web.Controllers.Admin
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorization(OpenDMS.Domain.Constants.PermissionType.CanAdminRoles)]
    [Route("Admin/[controller]/[action]")]
    public class RolesController : Controller
    {
        private readonly ILogger<RolesController> _logger;

        public RolesController(ILogger<RolesController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                return View();

            }
            catch (Exception ex)
            {
                _logger.LogError("TipologieController -> Index -> THROW an Exception : " + ex.Message);
                throw ex;
            }

        }



    }
}