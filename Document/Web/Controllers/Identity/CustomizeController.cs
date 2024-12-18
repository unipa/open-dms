using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Distributed;
using OpenDMS.Core.Interfaces;
using OpenDMS.CustomPages;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using System.Diagnostics;
using Web.BL.Interface;
using Web.DTOs;
using Web.Model.Admin;
using Web.Model.Customize;

namespace Web.Controllers.Identity
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    [Route("[controller]/[action]")]
    public class CustomizeController : Controller
    {
        private readonly ILogger<CustomizeController> _logger;
        private readonly ILoggedUserProfile loggedUserProfile;
        private readonly IUserService userService;
        private readonly IUISettingsRepository uiSettingsRepository;
        private readonly IACLService aclService;
        private readonly IMailboxService mailboxService;
        private readonly IMailServerRepository mailServerRepository;
        private readonly ICustomPageProvider customPageProvider;
        private readonly IDistributedCache cache;
        private readonly ICustomizeBL _bl;

        public CustomizeController(ILogger<CustomizeController> logger,
            ILoggedUserProfile loggedUserProfile,
            IUserService userService,
            IUISettingsRepository uiSettingsRepository,
            IACLService aclService,
            IMailboxService mailboxService,
            IMailServerRepository mailServerRepository,
            ICustomPageProvider customPageProvider,
            IDistributedCache cache,
            ICustomizeBL bl)
        {
            _logger = logger;
            this.loggedUserProfile = loggedUserProfile;
            this.userService = userService;
            this.uiSettingsRepository = uiSettingsRepository;
            this.aclService = aclService;
            this.mailboxService = mailboxService;
            this.mailServerRepository = mailServerRepository;
            this.customPageProvider = customPageProvider;
            this.cache = cache;
            _bl = bl;
        }

        #region Profilo utente e Autorizzazioni
        public async Task<IActionResult> Index()
        {
            var u = loggedUserProfile.Get();
            var vm = new UserDetails_DTO();

            try
            {

                var User = await userService.GetById(u.userId);

                vm.FullName = User.Contact.FullName;
                vm.SurName = User.Contact.SurName;
                vm.FirstName = User.Contact.FirstName;
                vm.FiscalCode = User.Contact.FiscalCode;
                vm.LicTradNum = User.Contact.LicTradNum;
                vm.BirthDate = await userService.GetAttribute(u.userId, UserAttributes.CONST_BIRTHDATE);

          
                vm.Country = await userService.GetAttribute(u.userId,UserAttributes.CONST_COUNTRY);

                vm.NotificationMailAddress = "Nessuno";

                var mboxId = await userService.GetAttribute(u.userId, UserAttributes.CONST_NOTIFICATION_MAIL);
                if (int.TryParse(mboxId, out int mid))
                {
                    var mailbox = await mailboxService.GetById(mid);
                    vm.NotificationMailAddress = mailbox.MailAddress;
                }
                vm.NotificationType = await userService.GetAttribute(u.userId,UserAttributes.CONST_NOTIFICATION_TYPE);
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

            return View(vm);
        }
        public async Task<IActionResult> Info()
        {
            var u = loggedUserProfile.Get();
            var vm = new UserDetails_DTO();

            try
            {

                var User = await userService.GetById(u.userId);

                vm.FullName = User.Contact.FullName;
                vm.SurName = User.Contact.SurName;
                vm.FirstName = User.Contact.FirstName;
                vm.FiscalCode = User.Contact.FiscalCode;
                vm.LicTradNum = User.Contact.LicTradNum;
                vm.BirthDate = await userService.GetAttribute(u.userId, UserAttributes.CONST_BIRTHDATE);


                vm.Country = await userService.GetAttribute(u.userId, UserAttributes.CONST_COUNTRY);

                vm.NotificationMailAddress = "Nessuno";

                var mboxId = await userService.GetAttribute(u.userId, UserAttributes.CONST_NOTIFICATION_MAIL);
                if (int.TryParse(mboxId, out int mid))
                {
                    var mailbox = await mailboxService.GetById(mid);
                    vm.NotificationMailAddress = mailbox.MailAddress;
                }
                vm.NotificationType = await userService.GetAttribute(u.userId, UserAttributes.CONST_NOTIFICATION_TYPE);
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

            return View(vm);
        }

        public async Task<IActionResult> Authorization()
        {
            var vm = new UserAuthorizations_DTO();
            var u = loggedUserProfile.Get();
            vm.Permessi = await _bl.GetAuthorizations();
            return View(vm);
        }

        public async Task<IActionResult> HomePage()
        {
            var vm = new Homepages_DTO();
            var u = loggedUserProfile.Get();
            vm.Pages = await customPageProvider.GetPages(u);
            vm.HomePage = await uiSettingsRepository.Get(u.userId, "HomePage");
            if (string.IsNullOrEmpty(vm.HomePage)) vm.HomePage = "/";
            return View(vm);
        }

        public async Task<IActionResult> Roles()
        {
            var vm = new UserAuthorizations_DTO();
            var u = loggedUserProfile.Get();
            vm.Roles = u.GlobalRoles;
            vm.Groups = u.Roles.Where(r=>r.Id.Contains("\\")).ToList();

            return View(vm);
        }
        public async Task<IActionResult> Groups()
        {
            var vm = new UserAuthorizations_DTO();
            var u = loggedUserProfile.Get();
            vm.Groups = u.Groups;

            return View(vm);
        }

        public async Task<IActionResult> AddOn()
        {
            var vm = new UserDetails_DTO();

            return View(vm);
        }

        public async Task<IActionResult> Email()
        {
            var u = loggedUserProfile.Get();
            var vm = new UserMailBoxes_DTO();
            vm.Mailboxes = (await mailboxService.GetAll(u)).ToList();
            vm.Companies = u.Companies;// (await companyService.GetAll()).ToList();
            vm.MailServers = (await mailServerRepository.GetAll()).ToList();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Email(Mailbox_DTO bd)
        {
            //INIZIALIZZO I DATI
            var u = loggedUserProfile.Get();
            var SuccessMessage = "";
            var ErrorMessage = "";
            var MailboxAddress = "";

            try
            {

                if (!ModelState.IsValid)
                {
                    MailboxAddress = "";
                    throw new Exception("Non è stato possibile salvare le informazioni. Errore: I campi non sono stati caricati correttamente.");
                }
                else
                {

                        var user = u.userId;
                        // verifico che nesusn altro abbia questa mail in carico
                        var mailbox = await mailboxService.GetByAddress(bd.MailAddress);
                        if (mailbox != null && mailbox.UserId != null && !mailbox.UserId.Equals(user, StringComparison.InvariantCultureIgnoreCase))
                            throw new Exception("Questa casella è già intestata ad un altro utente");

                        if (bd.MailServerId <= 0)
                        {
                            throw new Exception("non hai indicato un dominio valido");
                        };
                        var mailServer = await mailServerRepository.GetById(bd.MailServerId);
                        if (bd.Id > 0)
                        {
                            var oldmailbox = await mailboxService.GetById(bd.Id);
                            if (oldmailbox.MailAddress != bd.MailAddress)
                            {
                                var OldAddress = (await userService.GetAllContactDigitalAddress(u.userId)).FirstOrDefault(c => c.Address == oldmailbox.MailAddress);
                                if (OldAddress != null)
                                {
                                    // Elimino il vecchio indirizzo
                                    OldAddress.Deleted = true;
                                    await userService.AddOrUpdateAddress(OldAddress, u.userId);
                                };
                            }
                        }


                        // verifico se avevo già creato e cancellato in precedenza il nuovo indirizzo email
                        var deletedAddress = (await userService.GetAllDeletedContactDigitalAddress(u.userId)).FirstOrDefault(c => c.Address == bd.MailAddress);
                        if (deletedAddress != null)
                        {
                            // Elimino il vecchio indirizzo
                            deletedAddress.Name = bd.DisplayName;
                            deletedAddress.SearchName = bd.DisplayName;
                            deletedAddress.DigitalAddressType = mailServer.MailType == MailType.Mail ? DigitalAddressType.Email : DigitalAddressType.Pec;
                            deletedAddress.Deleted = false;
                            await userService.AddOrUpdateAddress(deletedAddress, u.userId);
                        } else
                        {
                            ContactDigitalAddress ContactDigitalAddress = new ContactDigitalAddress();
                            var c = await userService.GetById (u.userId);
                            ContactDigitalAddress.ContactId = c.ContactId;
                            ContactDigitalAddress.DigitalAddressType = mailServer.MailType == MailType.Mail ? DigitalAddressType.Email : DigitalAddressType.Pec;
                            ContactDigitalAddress.Name = bd.DisplayName;
                            ContactDigitalAddress.SearchName = bd.DisplayName;
                            ContactDigitalAddress.Address = bd.MailAddress;
                            ContactDigitalAddress.Deleted = false;
                            await userService.AddOrUpdateAddress(ContactDigitalAddress, u.userId);

                        }
                        var config = new MapperConfiguration(cfg => cfg.CreateMap<Mailbox_DTO, Mailbox>());
                        Mapper mp = new Mapper(config);
                        var mbd = mp.Map<Mailbox>(bd);
                        mbd.UserId = u.userId;

                        if (bd.Id <= 0)
                        {
                            mbd.Id = 0;
                            await mailboxService.Create(mbd);
                        }
                        else
                        {
                            await mailboxService.Update(mbd);
                        }
                        SuccessMessage = "Informazioni aggiornate correttamente.";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("UserContactDigitalAddressController -> SetRecapito -> THROW an Exception : " + ex.Message);
                ErrorMessage = ex.Message;
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Email");

        }

        public async Task<IActionResult> DeleteEmail(int DigitalAddressId)
        {
            //INIZIALIZZO I DATI

            var u = loggedUserProfile.Get();
            var SuccessMessage = "";
            var ErrorMessage = "";

            try
            {

                    var oldmailbox = await mailboxService.GetById(DigitalAddressId);
                    var OldAddress = (await userService.GetAllContactDigitalAddress(u.userId)).FirstOrDefault(c => c.Address == oldmailbox.MailAddress);
                    if (OldAddress != null)
                    {
                        // Elimino il vecchio indirizzo
                        OldAddress.Deleted = true;
                        await userService.AddOrUpdateAddress(OldAddress, u.userId);
                    };
                    await mailboxService.Delete(DigitalAddressId);
                    SuccessMessage = "Recapito eliminato correttamente.";

            }
            catch (Exception ex)
            {
                _logger.LogError("UserContactDigitalAddressController -> DeleteRecapito -> THROW an Exception : " + ex.Message);
                ErrorMessage = ex.Message;
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Email");

        }



        public async Task<IActionResult> EditUser()
        {
            var vm = new UserDetails_DTO();

            try
            {
                var u = loggedUserProfile.Get();
                var User = await userService.GetById(u.userId);

                vm.FullName = User.Contact.FullName;
                vm.SurName = User.Contact.SurName;
                vm.FirstName = User.Contact.FirstName;
                vm.FiscalCode = User.Contact.FiscalCode;
                vm.LicTradNum = User.Contact.LicTradNum;

                vm.BirthDate = await userService.GetAttribute(u.userId,UserAttributes.CONST_BIRTHDATE);
                vm.IdentityDocumentType = await userService.GetAttribute(u.userId, UserAttributes.CONST_IDENTITY_DOCUMENT_TYPE);
                vm.IdentityDocumentId = await userService.GetAttribute(u.userId,UserAttributes.CONST_IDENTITY_DOCUMENT_ID);
                vm.Country = await userService.GetAttribute(u.userId,UserAttributes.CONST_COUNTRY);
                vm.NotificationMailAddress = await userService.GetAttribute(u.userId, UserAttributes.CONST_NOTIFICATION_MAIL);
                vm.NotificationType = await userService.GetAttribute(u.userId, UserAttributes.CONST_NOTIFICATION_TYPE);
//                if (string.IsNullOrEmpty(vm.NotificationType))
//                    vm.NotificationType = "*" + await userService.GetAttribute(SpecialUser.SystemUser, UserAttributes.CONST_NOTIFICATION_TYPE);

                var MailsList = (await mailboxService.GetAll(u)).ToList();

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
                _logger.LogError("CustomizeController -> EditUser -> THROW an Exception : " + ex.Message);
                vm.ErrorMessage = "Errore durante il caricamento delle informazioni. Errore: " + ex.Message + " ; ";
                vm.Icon = "fa fa-alert";
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserDetails_DTO vm)
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
                    var u = loggedUserProfile.Get();
                    User User = await userService.GetById(u.userId);

                    User.Contact.SurName = vm.SurName;
                    User.Contact.FirstName = vm.FirstName;
                    User.Contact.FullName = vm.FullName;
                    User.Contact.FiscalCode = vm.FiscalCode;
                    User.Contact.LicTradNum = vm.LicTradNum;
                    //User.Contact.Sex = vm.Sex;
                    await userService.AddOrUpdate(User);
                    await cache.RemoveAsync ("userProfile-" + u.userId);

                    await userService.SetAttribute(u.userId, UserAttributes.CONST_IDENTITY_DOCUMENT_TYPE, vm.IdentityDocumentType);
                    await userService.SetAttribute(u.userId, UserAttributes.CONST_IDENTITY_DOCUMENT_ID, vm.IdentityDocumentId);
                    await userService.SetAttribute(u.userId, UserAttributes.CONST_COUNTRY, vm.Country);
                    await userService.SetAttribute(u.userId, UserAttributes.CONST_BIRTHDATE, vm.BirthDate);
                    await userService.SetAttribute(u.userId, UserAttributes.CONST_NOTIFICATION_MAIL, vm.NotificationMailAddress);
                    await userService.SetAttribute(u.userId, UserAttributes.CONST_NOTIFICATION_TYPE, vm.NotificationType);

                    SuccessMessage = "Modifiche salvate con successo.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("CustomizeController -> SetEditUser -> THROW an Exception : " + ex.Message);
                ErrorMessage = ex.Message;
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Index");
        }

        #endregion

        #region Firme

        public async Task<IActionResult> Firme()
        {
            var u = _bl.GetUserProfile();
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
                vm.CanHaveHandwrittenSignature = (await aclService.GetAuthorization("$GLOBAL$", u, PermissionType.Profile_CanHaveSignature)) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;
                
                vm.CanHaveRemoteSignature = (await aclService.GetAuthorization("$GLOBAL$", u, PermissionType.Profile_CanHaveRemoteSignature)) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;

                vm.RemoteSignService = await userService.GetAttribute(u.userId,UserAttributes.CONST_REMOTESIGNATURE_SERVICE);
            }
            catch (Exception ex)
            {
                vm.ErrorMessage = ex.Message;
            }

            return View(vm);
        }

        /// <summary>
        /// Usato per settare negli UserAttribute il provider di firma remota
        /// </summary>
        /// <param name="Service">identificatico del provider di firma remota , usato poi per reperire gli url dall'appsettings</param>
        /// <returns></returns>
        public async Task SetFirme(string? Service)
        {
            try
            {
                //if (string.IsNullOrEmpty(Service)) throw new ArgumentNullException(nameof(Service));

                await _bl.SetUserAttribute(UserAttributes.CONST_REMOTESIGNATURE_SERVICE, Service+"");
            }
            catch (Exception ex)
            {
                _logger.LogError("CustomizeController -> SetFirme -> THROW an Exception : " + ex.Message);
            }
        }

        public async Task<IActionResult> SetHandWrittenSign([FromBody]string sign)
        {
            try
            {
                await _bl.SetHandWrittenSign(sign);
                return Ok("Firma caricata con successo");
            }
            catch (Exception ex)
            {
                _logger.LogError("CustomizeController -> SetHandWrittenSign -> THROW an Exception : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        
        public async Task<IActionResult> SetVisto([FromBody]string visto)
        {
            try
            {
                await _bl.SetVisto(visto);
                return Ok("Visto caricato con successo");
            }
            catch (Exception ex)
            {
                _logger.LogError("CustomizeController -> SetVisto -> THROW an Exception : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteHandWrittenSign()
        {
            try
            {
                await _bl.DeleteHandWrittenSign();
                return Ok("Firma eliminata con successo");
            }
            catch (Exception ex)
            {
                _logger.LogError("CustomizeController -> DeleteHandWrittenSign -> THROW an Exception : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteVisto()
        {
            try
            {
                await _bl.DeleteVisto();
                return Ok("Visto eliminato con successo");
            }
            catch (Exception ex)
            {
                _logger.LogError("CustomizeController -> DeleteVisto -> THROW an Exception : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Avatar(Immagine del profilo)
        public async Task<IActionResult> Avatar()
        {

            var vm = new AvatarViewModel();
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

                vm.UploadedAvatars = await _bl.GetUploadedAvatars();
            }
            catch (Exception ex)
            {
                vm.ErrorMessage = ex.Message;
            }

            return View(vm);
        }

        public async Task<IActionResult> SetAvatar(string imagePath)
        {
            var SuccessMessage = "";
            var ErrorMessage = "";

            try
            {
                await _bl.SetAvatar(imagePath);
                SuccessMessage = "Immagine profilo salvata con successo";
            }
            catch (Exception ex)
            {
                _logger.LogError("CustomizeController -> SetAvatar -> THROW an Exception : " + ex.Message);
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Avatar");
        }

        public async Task<IActionResult> DeleteAvatar(string guid)
        {
            var SuccessMessage = "";
            var ErrorMessage = "";

            try
            {
                await _bl.DeleteAvatar(guid);
                SuccessMessage = "Immagine profilo salvata con successo";
            }
            catch (Exception ex)
            {
                _logger.LogError("CustomizeController -> SetAvatar -> THROW an Exception : " + ex.Message);
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Avatar");
        }

        [HttpPost]
        public async Task<IActionResult> UploadAvatar(IFormFile imageFile)
        {
            var SuccessMessage = "";
            var ErrorMessage = "";

            try
            {
                await _bl.UploadAvatar(imageFile);
                SuccessMessage = "Immagine caricata con successo";
            }
            catch (Exception ex)
            {
                _logger.LogError("CustomizeController -> UploadAvatar -> THROW an Exception : " + ex.Message);
                ErrorMessage = ex.Message;
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Avatar");
        }

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}