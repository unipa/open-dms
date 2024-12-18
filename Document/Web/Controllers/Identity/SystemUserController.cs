using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Services;
using System.Diagnostics;
using Web.BL.Interface;
using Web.DTOs;
using Web.Model.Admin;
using Web.Model.Customize;
using AutoMapper;
using OpenDMS.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Domain.Models;
using OpenDMS.Core.Filters;

namespace Web.Controllers.Identity
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    [Authorization(":admin")]
    [Route("Admin/[controller]/[action]")]
    public class SystemUserController : Controller
    {
        private readonly ILogger<SystemUserController> _logger;
        private readonly ILoggedUserProfile loggedUserProfile;
        private readonly IUserService userService;
        private readonly IACLService aclService;
        private readonly IMailboxService mailboxService;
        private readonly IMailServerRepository mailServerRepository;
        private readonly ICompanyService companyService;
        private readonly ICustomizeBL _bl;

        public SystemUserController(ILogger<SystemUserController> logger,
            ILoggedUserProfile loggedUserProfile,
            IUserService userService,
            IACLService aclService,
            IMailboxService mailboxService,
            IMailServerRepository mailServerRepository,
            ICompanyService companyService,
            ICustomizeBL bl)
        {
            _logger = logger;
            this.loggedUserProfile = loggedUserProfile;
            this.userService = userService;
            this.aclService = aclService;
            this.mailboxService = mailboxService;
            this.mailServerRepository = mailServerRepository;
            this.companyService = companyService;
            _bl = bl;
        }

        #region Profilo utente e Autorizzazioni
        public async Task<IActionResult> Index()
        {
            var u = await userService.GetUserProfile(SpecialUser.SystemUser);
            var vm = new UserDetails_DTO();
            try
            {
                var User = await userService.GetById(u.userId);

                vm.FullName = User.Contact.FullName;
                vm.SurName = User.Contact.SurName;
                vm.FirstName = User.Contact.FirstName;
                vm.IdentityDocumentType = await userService.GetAttribute(u.userId,UserAttributes.CONST_IDENTITY_DOCUMENT_TYPE);

                switch (vm.IdentityDocumentType)
                {
                    case "TIN":
                        vm.IdentityDocument = "Codice Fiscale"; break;
                    case "PAS":
                        vm.IdentityDocument = "Passaporto"; break;
                    case "IDC":
                        vm.IdentityDocument = "Documento di Identità"; break;
                    case "VAT":
                        vm.IdentityDocument = "Partita IVA"; break;
                    case "NTR":
                        vm.IdentityDocument = "Registrazione Camera di Commercio"; break;
                    default:
                        vm.IdentityDocument = "Nessuno";
                        break;
                }
                vm.IdentityDocumentId = await userService.GetAttribute(u.userId,UserAttributes.CONST_IDENTITY_DOCUMENT_ID);
                vm.Country = await userService.GetAttribute(u.userId,UserAttributes.CONST_COUNTRY);

                vm.NotificationMailAddress = "Nessuno";

                var mboxId = await userService.GetAttribute(u.userId, UserAttributes.CONST_NOTIFICATION_MAIL);
                if (int.TryParse(mboxId, out int mid))
                {
                    var mailbox = await mailboxService.GetById(mid);
                    if (mailbox != null)
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

        public async Task<IActionResult> Email()
        {
            var u = await userService.GetUserProfile(SpecialUser.SystemUser);
            var vm = new UserMailBoxes_DTO();
            vm.Mailboxes = (await mailboxService.GetAll(u)).ToList();
            vm.Companies = (await companyService.GetAll()).ToList();
            vm.MailServers = (await mailServerRepository.GetAll()).ToList();
            if (vm.Mailboxes.Count == 0 && vm.MailServers.Count > 0) {
                Mailbox M = new Mailbox();
                M.UserId = u.userId;
                M.CompanyId = 1;
                M.MailServerId = vm.MailServers[0].Id;
                await mailboxService.Create(M);
                M.MailServer = vm.MailServers[0];
                vm.Mailboxes.Add(M);    
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Email(Mailbox_DTO bd)
        {
            //INIZIALIZZO I DATI
            var u = await userService.GetUserProfile(SpecialUser.SystemUser);
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
                        if (mailServer.AuthenticationType == AuthenticationType.None)
                        {
                            bd.Validated = true;
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

            var u = await userService.GetUserProfile(SpecialUser.SystemUser);
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
                var u = await userService.GetUserProfile(SpecialUser.SystemUser);
                var User = await userService.GetById(u.userId);

                vm.FullName = User.Contact.FriendlyName;
                vm.SurName = User.Contact.SurName;
                vm.FirstName = User.Contact.FirstName;

                vm.IdentityDocumentType = await userService.GetAttribute(u.userId,UserAttributes.CONST_IDENTITY_DOCUMENT_TYPE);
                vm.IdentityDocumentId = await userService.GetAttribute(u.userId,UserAttributes.CONST_IDENTITY_DOCUMENT_ID);
                vm.Country = await userService.GetAttribute(u.userId,UserAttributes.CONST_COUNTRY);
                var MailsList = (await mailboxService.GetAll(u)).ToList(); // await userService.GetAllContactDigitalAddress(u.userId);// await _bl.GetUserMails();
                vm.EmailAddresses = new List<SelectListItem>();

                foreach (var Mail in MailsList)
                {
                    vm.EmailAddresses.Add(new SelectListItem { Text = Mail.MailAddress, Value = Mail.Id.ToString() });
                }

                vm.NotificationMailAddress = await userService.GetAttribute(u.userId,UserAttributes.CONST_NOTIFICATION_MAIL);
                vm.NotificationType = await userService.GetAttribute(u.userId,UserAttributes.CONST_NOTIFICATION_TYPE);

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
                    var u = await userService.GetUserProfile(SpecialUser.SystemUser);
                    var User = await userService.GetById(u.userId);

                    User.Contact.SurName = vm.SurName;
                    User.Contact.FirstName = vm.FirstName;
                    await userService.AddOrUpdate(User);

                    await userService.SetAttribute(u.userId, UserAttributes.CONST_IDENTITY_DOCUMENT_TYPE, vm.IdentityDocumentType);
                    await userService.SetAttribute(u.userId, UserAttributes.CONST_IDENTITY_DOCUMENT_ID, vm.IdentityDocumentId);
                    await userService.SetAttribute(u.userId, UserAttributes.CONST_COUNTRY, vm.Country);
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