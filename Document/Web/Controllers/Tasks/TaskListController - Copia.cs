//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using OpenDMS.Core.DTOs;
//using OpenDMS.Core.Interfaces;
//using OpenDMS.Domain.Constants;
//using OpenDMS.Domain.Entities.Settings;
//using OpenDMS.Domain.Enumerators;
//using OpenDMS.Domain.Models;
//using OpenDMS.Domain.Repositories;
//using OpenDMS.Domain.Services;
//using Web.DTOs;

//namespace OpenDMS.Task2.API.Controllers
//{

//    [Authorize]
//    [ApiController]
//    [Route("/Tasks2/[action]")]
//    public class Tasks2Controller : Controller
//    {
//        private readonly IConfiguration configuration;
//        private readonly IUserTaskService service;
//        private readonly ICompanyService company;
//        private readonly ILookupTableRepository lookup;
//        private readonly IACLService aclService;
//        private readonly ILoggedUserProfile userProfile;
//        private readonly IAppSettingsRepository appSettings;
//        private readonly IUserService userContext;

//        public Tasks2Controller(
//            IConfiguration configuration,
//            IUserTaskService service,
//            ICompanyService company,
//            ILookupTableRepository lookup,
//            IACLService aclService,
//            ILoggedUserProfile userProfile,
//            IAppSettingsRepository appSettings,
//            IUserService userContext)
//        {
//            this.configuration = configuration;
//            this.service = service;
//            this.company = company;
//            this.lookup = lookup;
//            this.aclService = aclService;
//            this.userProfile = userProfile;
//            this.appSettings = appSettings;
//            this.userContext = userContext;
//        }

//        public async Task<IActionResult> Index()
//        {
//            try
//            {

//                return View();

//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//    }
//}
