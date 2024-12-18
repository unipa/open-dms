using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using OpenDMS.Core.Filters;
using OpenDMS.Core.Interfaces;
using sun.misc;
using System.Diagnostics;
using Web.BL.Interface;
using Web.Model.Admin;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace Web.Controllers.Admin
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("Admin/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly ILoggedUserProfile userProfile;
        private readonly ILogger<HomeController> _logger;
        private readonly IDistributedCache cache;
        private readonly IAdminLeftMenuBL _bl;


        public HomeController(ILoggedUserProfile userProfile, ILogger<HomeController> logger, IDistributedCache cache, IAdminLeftMenuBL bl)
        {
            this.userProfile = userProfile;
            _logger = logger;
            this.cache = cache;
            _bl = bl;
        }

        [Authorization(":admin")]
        public async Task<IActionResult> Index()
        {
            var VM = new HomeViewModel();
            var u = userProfile.Get();
            VM.UserId = u.userId;

            //ottengo tutte le tabelle
            VM.Aree = await _bl.GetAreas(u);

            if (VM.Aree.Count == 0)
            {
                VM.ErrorMessage = "Non hai accesso a nessuna area amministrativa";
                VM.Icon = "fa fa-alert";
            }

            return View(VM);

        }

        [Authorize]
        public async Task Logout()
        {
            var u = userProfile.Get();
            string id = u.userId;
            await cache.RemoveAsync("userProfile-" + id);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme); //, new AuthenticationProperties { RedirectUri = "/Home/Logout-Complete" });
            _logger.LogInformation("Logout: " + id);

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}