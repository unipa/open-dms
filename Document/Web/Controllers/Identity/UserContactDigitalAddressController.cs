using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.BL.Interface;
using Web.Model.Admin;
using Web.Model.Customize;

namespace Web.Controllers.Identity
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("Customize/[controller]/[action]")]
    public class UserContactDigitalAddressController : Controller
    {
        private readonly ILogger<UserContactDigitalAddressController> _logger;
        private readonly IUserContactDigitalAddressBL _bl;
        private readonly IConfiguration _config;

        public UserContactDigitalAddressController(ILogger<UserContactDigitalAddressController> logger, IUserContactDigitalAddressBL bl, IConfiguration config = null)
        {
            _logger = logger;
            _bl = bl;
            _config = config;
        }

        public async Task<IActionResult> Index(string MailboxAddress = "")
        {
            //INIZIALIZZO I DATI

            var vm = new UserContactDigitalAddressViewModel();

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

                vm.Host = _config["Endpoint:AdminService"];
                vm.MailboxAddress = MailboxAddress;
                vm.Token = await HttpContext.GetTokenAsync("access_token");
                vm.Utente = User.Identity.Name;

                vm.DigitalAddresses = await _bl.GetDigitalAddresses();
                vm.MailServers = (await _bl.GetAllMailServer()).ToList();
                vm.Companies = await _bl.GetCompanies();
            }
            catch (Exception ex)
            {
                _logger.LogError("UserContactDigitalAddressController -> Index -> THROW an Exception : " + ex.Message);
                vm.ErrorMessage = "Non è stato possibile ottenere le informazione dell'utente. Errore: " + ex.Message + " ; ";
                vm.Icon = "fa fa-alert";
            }
            return View(vm);
        }

        public async Task<IActionResult> SetRecapito(Mailbox_DTO bd)
        {
            //INIZIALIZZO I DATI
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
                    try
                    {
                        MailboxAddress = (await _bl.SetRecapito(bd, bd.ContactDigitalAddressId)).MailAddress.ToString();
                        SuccessMessage = "Informazioni aggiornate correttamente.";
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Non è stato possibile salvare le informazioni. Errore: " + ex.Message + " ; ");
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("UserContactDigitalAddressController -> SetRecapito -> THROW an Exception : " + ex.Message);
                ErrorMessage = ex.Message;
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Index", new { MailboxAddress });

        }

        public async Task<IActionResult> DeleteRecapito(int DigitalAddressId, string MailboxAddress)
        {
            //INIZIALIZZO I DATI

            var SuccessMessage = "";
            var ErrorMessage = "";

            try
            {


                try
                {
                    await _bl.DeleteRecapito(DigitalAddressId, MailboxAddress);
                    MailboxAddress = "";
                    SuccessMessage = "Recapito eliminato correttamente.";
                }
                catch (Exception ex)
                {
                    throw new Exception("Non è stato possibile salvare le informazione dell'utente. Errore: " + ex.Message + " ; ");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("UserContactDigitalAddressController -> DeleteRecapito -> THROW an Exception : " + ex.Message);
                ErrorMessage = ex.Message;
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Index", new { MailboxAddress });

        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}