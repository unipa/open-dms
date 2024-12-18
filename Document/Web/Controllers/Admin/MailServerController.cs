using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Filters;
using System.Diagnostics;
using Web.BL.Interface;
using Web.Model.Admin;
using Web.Utilities;

namespace Web.Controllers.Admin
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorization(OpenDMS.Domain.Constants.PermissionType.CanAdminMailServers)]
    [Route("Admin/[controller]/[action]")]
    public class MailServerController : Controller
    {
        private readonly ILogger<MailServerController> _logger;
        private readonly IMailServerBL _bl;

        public MailServerController(
            ILogger<MailServerController> logger,
            IMailServerBL bl)
        {
            _logger = logger;
            _bl = bl;
        }

        public async Task<IActionResult> Index(int Id = 0)
        {
            try
            {
                //INIZIALIZZO I DATI

                var ErrorMessage = "";
                var Icon = "";

                var vm = new MailServerViewModel();

                if (Id != 0)
                {
                    var mb = new MailServer_DTO();
                    try { mb = await _bl.GetMailServerById(Id); }
                    catch (Exception ex)
                    {
                        ErrorMessage = "Errore durante il caricamento delle informazioni. Errore: " + ex.Message + " ; ";
                        Icon = "fa fa-alert";
                    }

                    if (mb == null)
                    {
                        vm.ErrorMessage = "Il Server di posta cercato non è stato trovato in memoria.";
                        vm.Icon = "fa fa-alert";
                    }

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<MailServer_DTO, MailServerViewModel>());
                    Mapper mp = new Mapper(config);
                    vm = mp.Map<MailServerViewModel>(mb);
                }
                else
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<MailServer_DTO, MailServerViewModel>());
                    Mapper mp = new Mapper(config);
                    vm = mp.Map<MailServerViewModel>(new MailServer_DTO());
                }

                //ottengo tutte i server di posta
                try { vm.MailServers = (await _bl.GetAllMailServer()).ToList(); }
                catch (Exception ex)
                {
                    ErrorMessage = "Errore durante il caricamento delle informazioni. Errore: " + ex.Message + " ; ";
                    Icon = "fa fa-alert";
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

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError("MailServerController -> Index -> THROW an Exception : " + ex.Message);
                throw ex;
            }
        }

        public async Task<IActionResult> Memorizza(MailServerViewModel vm)
        {
            var ErrorMessage = "";
            var SuccessMessage = "";

            try
            {

                if (!ModelState.IsValid)
                    throw new Exception("Errore : " + ControllerUtility.GetErrorsString(ModelState));

                var config = new MapperConfiguration(cfg => cfg.CreateMap<MailServerViewModel, MailServer_DTO>());
                Mapper mp = new Mapper(config);
                var mb = mp.Map<MailServer_DTO>(vm);

                if (vm.Id == 0)
                {
                    try
                    {
                        await _bl.Insert(mb);
                        SuccessMessage = "Server di posta aggiunto con successo";
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = "Errore nel caricamento delle informazioni. Errore: " + ex.Message;
                    }
                }
                else
                {

                    try
                    {
                        await _bl.Update(mb);
                        SuccessMessage = "Server di posta modificato con successo";
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = "Errore durante il caricamento delle informazioni. Errore: " + ex.Message;
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("MailServerController -> Memorizza -> THROW an Exception : " + ex.Message);
                ErrorMessage = "Errore in fase di esecuzione: " + ex.Message;
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Index", new { vm.Id });

        }

        public async Task<IActionResult> Delete(int Id)
        {

            var ErrorMessage = "";
            var SuccessMessage = "";

            if (Id == 0) return RedirectToAction("Index", new { Id = 0 });

            try
            {
                await _bl.Delete(Id);
                SuccessMessage = " Server di posta eliminato con successo. ";
            }
            catch (Exception ex)
            {

                _logger.LogError("MailServerController -> Delete -> THROW an Exception : " + ex.Message);
                ErrorMessage = "Errore in fase di esecuzione: " + ex.Message;
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Index", new { Id = 0 });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}