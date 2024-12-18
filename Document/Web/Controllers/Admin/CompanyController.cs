using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Filters;
using OpenDMS.Domain.Entities.Settings;
using System.Diagnostics;
using Web.BL.Interface;
using Web.Model.Admin;
using Web.Utilities;

namespace Web.Controllers.Admin
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorization(OpenDMS.Domain.Constants.PermissionType.CanAdminCompanies)]

    [Route("Admin/[controller]/[action]")]
    public class CompanyController : Controller
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly IBancheDatiBL _bl;

        public CompanyController(ILogger<CompanyController> logger, IBancheDatiBL bl)
        {
            _logger = logger;
            _bl = bl;
        }

        public async Task<IActionResult> Index(int cdb = 1)
        {
            var vm = new BancaDatiViewModel();

            vm.Icon = "fa fa-check";

            var company = new Company();

            try
            {
                try
                {
                    company = await _bl.GetById(cdb);
                    if (company == null) throw new Exception("La Company indicata non è stata trovata.");
                }
                catch (Exception ex)
                {
                    throw new Exception("La Company indicata non è stata trovata. Errore: " + ex.Message + " ; ");
                }

                if (company != null)
                {
                    var config2 = new MapperConfiguration(cfg => cfg.CreateMap<Company, BancaDatiViewModel>());
                    Mapper mp2 = new Mapper(config2);
                    vm = mp2.Map<BancaDatiViewModel>(company);
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
                _logger.LogError("BancheDati -> GetById -> THROW an Exception : " + ex.Message);
                vm.Icon = "fa fa-alert";
                vm.ErrorMessage = "Errore: " + ex.Message;
            }
            return View(vm);

        }

        [HttpPost]
        public async Task<IActionResult> Memorizza(BancaDatiViewModel vm)
        {
            var SuccessMessage = "";
            var ErrorMessage = "";

            var config = new MapperConfiguration(cfg => cfg.CreateMap<BancaDatiViewModel, Company>());
            Mapper mp = new Mapper(config);
            var new_company = mp.Map<Company>(vm);

            try
            {

                if (new_company.Id != 0)
                {
                    if (await _bl.GetById(new_company.Id) == null)
                        throw new Exception("Errore: La Company che si sta cercando di modificare non è stata trovata.");
                    else
                    {
                        if (!ModelState.IsValid)
                            throw new Exception(ControllerUtility.GetErrorsString(ModelState));
                        else
                        {
                            await _bl.Update(new_company);
                            TempData["SuccessMessage"] = "Company modificata con successo.";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("BancheDati -> GetById -> THROW an Exception : " + ex.Message);
                TempData["ErrorMessage"] = "Errore durante la modifica della company. Errore: " + ex.Message;
            }

            return RedirectToAction("Index", new { cdb = new_company.Id });


        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}