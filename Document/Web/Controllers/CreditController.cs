using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using OpenDMS.Core.Interfaces;
using sun.misc;
using System.Diagnostics;
using Web.BL.Interface;
using Web.Model.Admin;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]/[action]")]
    public class CreditController : Controller
    {
        private readonly ILoggedUserProfile userProfile;
        private readonly IDistributedCache cache;
        private readonly IAdminLeftMenuBL _bl;


        public CreditController(ILoggedUserProfile userProfile)
        {
            this.userProfile = userProfile;
        }

        public async Task<IActionResult> Index()
        {
            return View();

        }
    }
}