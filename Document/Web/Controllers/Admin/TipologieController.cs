using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Filters;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Models;

namespace Web.Controllers.Admin
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorization(OpenDMS.Domain.Constants.PermissionType.CanAdminDocumentTypes)]
    [Route("Admin/[controller]/[action]")]
    public class TipologieController : Controller
    {
        private readonly ILogger<TipologieController> _logger;
        private readonly IFormService formService;
        private readonly IDocumentService documentService;
        private readonly IDocumentTypeService documentTypeService;
        private readonly IConfiguration _configuration;

        public TipologieController(ILogger<TipologieController> logger,
            IFormService formService,
            IDocumentService documentService,
            IDocumentTypeService documentTypeService,
            IConfiguration configuration)
        {
            _logger = logger;
            this.formService = formService;
            this.documentService = documentService;
            this.documentTypeService = documentTypeService;
            _configuration = configuration;
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
        public async Task<IActionResult> Forms()
        {
            try
            {
                UserProfile u = new UserProfile();
                u.userId = SpecialUser.SystemUser;
                return new JsonResult(await formService.GetAll(u));
            }
            catch (Exception ex)
            {
                _logger.LogError("TipologieController -> Index -> THROW an Exception : " + ex.Message);
                throw ex;
            }

        }

    }
}