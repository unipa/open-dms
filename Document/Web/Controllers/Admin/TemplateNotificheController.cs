using Lucene.Net.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Filters;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Repositories;
using System.Diagnostics;
using Web.BL.Interface;
using Web.Model.Admin;

namespace Web.Controllers.Admin
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorization(OpenDMS.Domain.Constants.PermissionType.CanAdminMessageTemplates)]
    [Route("Admin/[controller]/[action]")]
    public class TemplateNotificheController : Controller
    {
        private readonly ILogger<TemplateNotificheController> _logger;
        private readonly IAppSettingsRepository appSettingsRepository;
        private readonly ITemplateNotificheBL _bl;

        public TemplateNotificheController(ILogger<TemplateNotificheController> logger, 
            IAppSettingsRepository appSettingsRepository,
            ITemplateNotificheBL bl)
        {
            _logger = logger;
            this.appSettingsRepository = appSettingsRepository;
            _bl = bl;
        }

        public async Task<IActionResult> Index(int CompanyId = 0, string Template = "")
        {
            try
            {
                //INIZIALIZZO I DATI

                var ErrorMessage = "";
                var Icon = "";

                var vm = new TemplateNotificheViewModel();

                vm.Template = Template;

                vm.CompanyId = CompanyId;
                vm.Expiration = await appSettingsRepository.Get(vm.CompanyId, "Tasks.DefaultExpiration");
                vm.MinWarning = await appSettingsRepository.Get(vm.CompanyId, "Task.Recipients.Level1");
                vm.MinError = await appSettingsRepository.Get(vm.CompanyId, "Task.Recipients.Level2");

                if (String.IsNullOrEmpty(vm.MinWarning)) vm.MinWarning = "20";
                if (String.IsNullOrEmpty(vm.MinError)) vm.MinWarning = "100";
                //ottengo Title e Body per il template e company indicati
                try
                {
                    //ottengo le ACLPermissions convertendole in ACLPermission_DTO
                    vm.Title = await _bl.Get(vm.CompanyId, Template + ".Title");
                    vm.Body = await _bl.Get(vm.CompanyId, Template + ".Body");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] += "Non sono stati trovati i valori richiesti di Title e Body. Errore: " + ex.Message + " ; ";
                }

                //ottengo tutte le companies
                try { vm.Companies = (await _bl.GetCompanies()).ToList(); }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] += "Non sono stati trovate companies. Errore: " + ex.Message + " ; ";
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
                _logger.LogError("TemplateNotificheController -> Index -> THROW an Exception : " + ex.Message);
                throw ex;
            }
        }

        public async Task<IActionResult> Memorizza(TemplateNotificheViewModel vm)
        {
            var ErrorMessage = "";
            var SuccessMessage = "";

            try
            {

                if (!ModelState.IsValid)
                {
                    vm.CompanyId = 0; //tutte
                    vm.Template = string.Empty;
                }
                else
                {
                    if (string.IsNullOrEmpty(vm.Template))
                    {
                        if (String.IsNullOrEmpty(vm.MinWarning)) vm.MinWarning = "20";
                        if (String.IsNullOrEmpty(vm.MinError)) vm.MinWarning = "100";

                        await appSettingsRepository.Set(vm.CompanyId, "Tasks.DefaultExpiration", vm.Expiration);
                        await appSettingsRepository.Set(vm.CompanyId, "Task.Recipients.Level1", vm.MinWarning);
                        await appSettingsRepository.Set(vm.CompanyId, "Task.Recipients.Level2", vm.MinError);

                    }
                    else
                    {
                        await _bl.Set(vm.CompanyId, vm.Template + ".Title", vm.Title);
                        await _bl.Set(vm.CompanyId, vm.Template + ".Body", vm.Body);
                    }
                    SuccessMessage = "Template aggiornato con successo";

                }

            }
            catch (Exception ex)
            {
                _logger.LogError("TemplateNotificheController -> Memorizza -> THROW an Exception : " + ex.Message);
                ErrorMessage = "Errore in fase di esecuzione: " + ex.Message;
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Index", new { vm.CompanyId, vm.Template });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}