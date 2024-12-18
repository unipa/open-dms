using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Filters;
using Web.Model.Admin;

namespace Web.Controllers.Admin
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorization(OpenDMS.Domain.Constants.PermissionType.CanAdminDatasources)]
    [Route("Admin/[controller]/[action]")]
    public class DatabaseEsterniController : Controller
    {
        private readonly ILogger<DatabaseEsterniController> _logger;
        private readonly IConfiguration _configuration;

        public DatabaseEsterniController(ILogger<DatabaseEsterniController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var VM = new DatabaseEsterniViewModel();

                VM.endpoint = _configuration["Endpoint:AdminService"];

                VM.token = await HttpContext.GetTokenAsync("access_token");

                return View(VM);
            }
            catch (Exception ex)
            {
                _logger.LogError("DatabaseEsterniController -> Index -> THROW an Exception : " + ex.Message);
                throw ex;
            }

        }

    }
}