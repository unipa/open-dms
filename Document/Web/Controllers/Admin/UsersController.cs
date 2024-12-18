using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Filters;

namespace Web.Controllers.Admin
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorization(OpenDMS.Domain.Constants.PermissionType.CanAdminRoles)]
    [Route("Admin/[controller]/[action]")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
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