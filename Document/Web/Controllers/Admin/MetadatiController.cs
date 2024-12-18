using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Filters;
using Web.Model.Admin;

namespace Web.Controllers.Admin
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorization(OpenDMS.Domain.Constants.PermissionType.CanAdminMeta)]
    [Route("Admin/[controller]/[action]")]
    public class MetadatiController : Controller
    {
        private readonly ILogger<MetadatiController> _logger;
        private readonly IConfiguration _configuration;

        public MetadatiController(ILogger<MetadatiController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var VM = new MetadatiViewModel();

                VM.endpoint = (string)_configuration.GetValue(typeof(string), "Endpoint:AdminService");

                VM.token = await HttpContext.GetTokenAsync("access_token");

                return View(VM);

            }
            catch (Exception ex)
            {
                _logger.LogError("MetadatiController -> Index -> THROW an Exception : " + ex.Message);
                throw ex;
            }

        }
    }
}