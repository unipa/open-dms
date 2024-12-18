
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Filters;
using OpenDMS.Domain.Constants;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Services;
using Web.DTOs;
using Web.Model.Admin;
using OpenDMS.Domain.Entities.Users;
using Web.BL.Interface;
using OpenDMS.Core.BusinessLogic;
using MailKit;
using Web.Model.Customize;
using OpenDMS.Infrastructure.Repositories;
using OpenDMS.Domain.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Web.Controllers.Admin
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorization(OpenDMS.Domain.Constants.PermissionType.CanAdminRoles)]
    [Route("Admin/[controller]")]
    public class UtentiController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UtentiController> _logger;
        private readonly IMailboxService _mailboxService;
        private readonly ICustomizeBL _customizeBL;
        private readonly IACLService _aclService;
        private readonly IMailServerRepository _mailServerRepository;

        public UtentiController(IUserService userService, ILogger<UtentiController> logger, IMailboxService mailboxService, ICustomizeBL customizeBL, IACLService aclService, IMailServerRepository mailServerRepository)
        {
            _userService = userService;
            _logger = logger;
            _mailboxService = mailboxService;
            _customizeBL = customizeBL;
            _aclService = aclService;
            _mailServerRepository = mailServerRepository;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Index(string search = "", int pageSize = 5, int pageNumber = 1)
        {
            var utentiTotale = await _userService.GetTotalCountOfFilteredUsers(search, false);

            var utentiPaginati = await _userService.GetFilteredAndPaginatedUsers(search, pageSize, pageNumber, false);

            var viewModel = new UtentiViewModel
            {
                utenti = utentiPaginati,
                PageNumber = pageNumber,
                TotalPages = (int)Math.Ceiling(utentiTotale / (double)pageSize)
            };

            ViewBag.PageSize = pageSize;
            ViewBag.Search = search;

            return View(viewModel);
        }

        [HttpGet("{id}/[action]")]
        public async Task<IActionResult> Dettaglio([FromRoute] string id)
        {
            var vm = new UserDetails_DTO();

            try
            {

                var User = await _userService.GetById(id);

                vm.FullName = User.Contact.FullName;
                vm.SurName = User.Contact.SurName;
                vm.FirstName = User.Contact.FirstName;
                vm.FiscalCode = User.Contact.FiscalCode;
                vm.LicTradNum = User.Contact.LicTradNum;
                vm.BirthDate = await _userService.GetAttribute(id, UserAttributes.CONST_BIRTHDATE);


                vm.Country = await _userService.GetAttribute(id, UserAttributes.CONST_COUNTRY);

                vm.NotificationMailAddress = "Nessuno";

                var mboxId = await _userService.GetAttribute(id, UserAttributes.CONST_NOTIFICATION_MAIL);
                if (int.TryParse(mboxId, out int mid))
                {
                    var mailbox = await _mailboxService.GetById(mid);
                    vm.NotificationMailAddress = mailbox.MailAddress;
                }
                vm.NotificationType = await _userService.GetAttribute(id, UserAttributes.CONST_NOTIFICATION_TYPE);
                switch (vm.NotificationType)
                {
                    case "APP":
                        vm.Notification = "App"; break;
                    case "MAIL":
                        vm.Notification = "Email"; break;
                    case "NO":
                        vm.Notification = "Nessuna"; break;
                    default:
                        vm.Notification = "Impostazione Globale"; break;
                }

                if (!string.IsNullOrEmpty(TempData["SuccessMessage"] as string))
                {
                    vm.SuccessMessage = TempData["SuccessMessage"] as string;
                    vm.Icon = "fa fa-check";
                }

                if (!string.IsNullOrEmpty(TempData["ErrorMessage"] as string))
                {
                    vm.ErrorMessage = TempData["ErrorMessage"] as string;
                    vm.Icon = "fa fa-alert";
                }
            }
            catch (Exception ex)
            {
                vm.ErrorMessage = "Errore durante il caricamento delle informazioni. Errore: " + ex.Message + " ; ";
                vm.Icon = "fa fa-alert";
            }

            ViewBag.id = id;
            return View(vm);
        }

        [HttpGet("{id}/[action]")]
        public async Task<IActionResult> Info([FromRoute] string id)
        {
            var vm = new UserDetails_DTO();

            try
            {

                var User = await _userService.GetById(id);

                vm.FullName = User.Contact.FullName;
                vm.SurName = User.Contact.SurName;
                vm.FirstName = User.Contact.FirstName;
                vm.FiscalCode = User.Contact.FiscalCode;
                vm.LicTradNum = User.Contact.LicTradNum;
                vm.BirthDate = await _userService.GetAttribute(id, UserAttributes.CONST_BIRTHDATE);


                vm.Country = await _userService.GetAttribute(id, UserAttributes.CONST_COUNTRY);

                vm.NotificationMailAddress = "Nessuno";

                var mboxId = await _userService.GetAttribute(id, UserAttributes.CONST_NOTIFICATION_MAIL);
                if (int.TryParse(mboxId, out int mid))
                {
                    var mailbox = await _mailboxService.GetById(mid);
                    vm.NotificationMailAddress = mailbox.MailAddress;
                }
                vm.NotificationType = await _userService.GetAttribute(id, UserAttributes.CONST_NOTIFICATION_TYPE);
                switch (vm.NotificationType)
                {
                    case "APP":
                        vm.Notification = "App"; break;
                    case "MAIL":
                        vm.Notification = "Email"; break;
                    case "NO":
                        vm.Notification = "Nessuna"; break;
                    default:
                        vm.Notification = "Impostazione Globale"; break;
                }

                if (!string.IsNullOrEmpty(TempData["SuccessMessage"] as string))
                {
                    vm.SuccessMessage = TempData["SuccessMessage"] as string;
                    vm.Icon = "fa fa-check";
                }

                if (!string.IsNullOrEmpty(TempData["ErrorMessage"] as string))
                {
                    vm.ErrorMessage = TempData["ErrorMessage"] as string;
                    vm.Icon = "fa fa-alert";
                }

            }
            catch (Exception ex)
            {
                vm.ErrorMessage = "Errore durante il caricamento delle informazioni. Errore: " + ex.Message + " ; ";
                vm.Icon = "fa fa-alert";
            }

            ViewBag.id = id;
            return View(vm);
        }

        [HttpGet("{id}/[action]")]
        public async Task<IActionResult> Email([FromRoute] string id)
        {
            var u = await _userService.GetUserProfile(id);
            var vm = new UserMailBoxes_DTO();
            vm.Mailboxes = (await _mailboxService.GetAll(u)).ToList();
            vm.Companies = u.Companies;// (await companyService.GetAll()).ToList();
            vm.MailServers = (await _mailServerRepository.GetAll()).ToList();

            ViewBag.id = id;
            return View(vm);
        }

        [HttpGet("{id}/[action]")]
        public async Task<IActionResult> Firme([FromRoute] string id)
        {
            var u = await _userService.GetUserProfile(id);
            var vm = new FirmeViewModel();
            try
            {
                if (!string.IsNullOrEmpty(TempData["SuccessMessage"] as string))
                {
                    vm.SuccessMessage = TempData["SuccessMessage"] as string;
                    vm.Icon = "fa fa-check";
                }

                if (!string.IsNullOrEmpty(TempData["ErrorMessage"] as string))
                {
                    vm.ErrorMessage = TempData["ErrorMessage"] as string;
                    vm.Icon = "fa fa-alert";
                }
                vm.CanHaveHandwrittenSignature = (await _aclService.GetAuthorization("$GLOBAL$", u, PermissionType.Profile_CanHaveSignature)) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;

                vm.CanHaveRemoteSignature = (await _aclService.GetAuthorization("$GLOBAL$", u, PermissionType.Profile_CanHaveRemoteSignature)) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;

                vm.RemoteSignService = await _userService.GetAttribute(u.userId, UserAttributes.CONST_REMOTESIGNATURE_SERVICE);
            }
            catch (Exception ex)
            {
                vm.ErrorMessage = ex.Message;
            }

            ViewBag.id = id;
            return View(vm);
        }

        [HttpGet("{id}/[action]")]
        public async Task<IActionResult> Roles([FromRoute] string id)
        {
            var vm = new UserAuthorizations_DTO();
            var u = await _userService.GetUserProfile(id);
            vm.Roles = u.GlobalRoles;
            vm.Groups = u.Roles.Where(r => r.Id.Contains("\\")).ToList();
            ViewBag.id = id;

            return View(vm);
        }

        [HttpGet("{id}/[action]")]
        public async Task<IActionResult> Groups([FromRoute] string id)
        {
            var vm = new UserAuthorizations_DTO();
            var u = await _userService.GetUserProfile(id);
            vm.Groups = u.Groups;

            ViewBag.id = id;
            return View(vm);
        }

        [HttpGet("{id}/[action]")]
        public async Task<IActionResult> Authorization([FromRoute] string id)
        {
            var vm = new UserAuthorizations_DTO();
            vm.Permessi = await _customizeBL.GetAuthorizationsForUser(id);
            ViewBag.id = id;
            return View(vm);
        }

        [HttpGet("{id}/[action]")]
        public async Task<IActionResult> Modifica([FromRoute] string id)
        {
            var vm = new UserDetails_DTO();

            try
            {
                var u = await _userService.GetUserProfile(id);
                var User = await _userService.GetById(u.userId);

                vm.FullName = User.Contact.FullName;
                vm.SurName = User.Contact.SurName;
                vm.FirstName = User.Contact.FirstName;
                vm.FiscalCode = User.Contact.FiscalCode;
                vm.LicTradNum = User.Contact.LicTradNum;

                vm.BirthDate = await _userService.GetAttribute(u.userId, UserAttributes.CONST_BIRTHDATE);
                vm.IdentityDocumentType = await _userService.GetAttribute(u.userId, UserAttributes.CONST_IDENTITY_DOCUMENT_TYPE);
                vm.IdentityDocumentId = await _userService.GetAttribute(u.userId, UserAttributes.CONST_IDENTITY_DOCUMENT_ID);
                vm.Country = await _userService.GetAttribute(u.userId, UserAttributes.CONST_COUNTRY);
                vm.NotificationMailAddress = await _userService.GetAttribute(u.userId, UserAttributes.CONST_NOTIFICATION_MAIL);
                vm.NotificationType = await _userService.GetAttribute(u.userId, UserAttributes.CONST_NOTIFICATION_TYPE);
                
                var MailsList = (await _mailboxService.GetAll(u)).ToList();

                vm.EmailAddresses = new List<SelectListItem>();

                foreach (var Mail in MailsList)
                {
                    vm.EmailAddresses.Add(new SelectListItem { Text = Mail.MailAddress, Value = Mail.Id.ToString() });
                }


                if (!string.IsNullOrEmpty(TempData["SuccessMessage"] as string))
                {
                    vm.SuccessMessage = TempData["SuccessMessage"] as string;
                    vm.Icon = "fa fa-check";
                }

                if (!string.IsNullOrEmpty(TempData["ErrorMessage"] as string))
                {
                    vm.ErrorMessage = TempData["ErrorMessage"] as string;
                    vm.Icon = "fa fa-alert";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("UtentiController -> EditUser -> THROW an Exception : " + ex.Message);
                vm.ErrorMessage = "Errore durante il caricamento delle informazioni. Errore: " + ex.Message + " ; ";
                vm.Icon = "fa fa-alert";
            }

            ViewBag.id = id;
            return View(vm);
        }

        [HttpPost("{id}/[action]/")]
        public async Task<IActionResult> Modifica([FromRoute] string id, UserDetails_DTO vm)
        {
            var SuccessMessage = "";
            var ErrorMessage = "";

            try
            {
                if (!ModelState.IsValid)
                {
                    var FirstErrorMessage = ModelState.Root.Children.FirstOrDefault(c => c.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid).Errors.FirstOrDefault().ErrorMessage;
                    throw new Exception("Non è stato possibile salvare le informazioni. Errore: I campi non sono stati caricati correttamente. " + FirstErrorMessage);
                }
                else
                {
                    var u = await _userService.GetUserProfile(id);
                    var User = await _userService.GetById(u.userId);

                    User.Contact.SurName = vm.SurName;
                    User.Contact.FirstName = vm.FirstName;
                    User.Contact.FullName = vm.FullName;
                    User.Contact.FiscalCode = vm.FiscalCode;
                    User.Contact.LicTradNum = vm.LicTradNum;
                    //User.Contact.Sex = vm.Sex;
                    await _userService.AddOrUpdate(User);

                    await _userService.SetAttribute(u.userId, UserAttributes.CONST_IDENTITY_DOCUMENT_TYPE, vm.IdentityDocumentType);
                    await _userService.SetAttribute(u.userId, UserAttributes.CONST_IDENTITY_DOCUMENT_ID, vm.IdentityDocumentId);
                    await _userService.SetAttribute(u.userId, UserAttributes.CONST_COUNTRY, vm.Country);
                    await _userService.SetAttribute(u.userId, UserAttributes.CONST_BIRTHDATE, vm.BirthDate);
                    await _userService.SetAttribute(u.userId, UserAttributes.CONST_NOTIFICATION_MAIL, vm.NotificationMailAddress);
                    await _userService.SetAttribute(u.userId, UserAttributes.CONST_NOTIFICATION_TYPE, vm.NotificationType);

                    SuccessMessage = "Modifiche salvate con successo.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("UtentiController -> SetEditUser -> THROW an Exception : " + ex.Message);
                ErrorMessage = ex.Message;
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Dettaglio", new {id = id });
        }
    }
}