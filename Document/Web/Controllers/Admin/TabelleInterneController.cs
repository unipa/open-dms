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
    [Authorization(OpenDMS.Domain.Constants.PermissionType.CanAdminTables)]
    [Route("Admin/[controller]/[action]")]
    public class TabelleInterneController : Controller
    {
        private readonly ILogger<TabelleInterneController> _logger;
        private readonly ITabelleInterneBL _bl;

        public TabelleInterneController(ILogger<TabelleInterneController> logger, ITabelleInterneBL bl)
        {
            _logger = logger;
            _bl = bl;
        }

        public async Task<IActionResult> Index(string TableId = "$TABLES$")
        {

            var vm = new TabelleInterneViewModel();
            try
            {
                //INIZIALIZZO I DATI
                vm.TableId = TableId;

                //ottengo tutte le tabelle
                try
                {
                    vm.Tables = await _bl.GetTables();
                    var lt_padre = await _bl.GetLookupTable("$TABLES$", TableId);
                    if(lt_padre.Description.Equals("#"+ TableId))
                    {
                        TableId = "$TABLES$";
                        TempData["ErrorMessage"] = "Tabella non trovata";
                        lt_padre = await _bl.GetLookupTable("$TABLES$", TableId);
                    }
                    vm.Description = lt_padre.Description;
                    //ottengo tutti gli oggetti Lookuptable associati a un tableId
                    vm.LookupTables = await _bl.GetLookupTables(TableId);
                }
                catch (Exception ex)
                {
                    vm.ErrorMessage = ex.Message;
                    vm.Icon = "fa fa-alert";
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
                _logger.LogError("TabelleInterne -> Index -> THROW an Exception : " + ex.Message);
                throw ex;
            }

        }

        public async Task<IActionResult> Aggiorna(TabelleInterneViewModel new_vm)
        {
            var ErrorMessage = "";
            var SuccessMessage = "";

            try
            {


                if (new_vm.Id == null)
                    throw new Exception("Errore: Il campo Id risulta vuoto.");

                if (!ModelState.IsValid)
                    throw new Exception("Errore: " + ControllerUtility.GetErrorsString(ModelState));
                else
                {
                    new_vm.Tables = new List<LookupTable>();
                    new_vm.LookupTables = new List<LookupTable>();

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<TabelleInterneViewModel, LookupTable>());
                    Mapper mp = new Mapper(config);
                    var new_lt = mp.Map<LookupTable>(new_vm);

                    await _bl.Update(new_lt);
                    SuccessMessage = "Tabella Interna aggiornata con successo!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("TabelleInterne -> Aggiorna -> THROW an Exception : " + ex.Message);
                ErrorMessage = "Errore durante l'aggiornamento della tabella interna : " + ex.Message;
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Index", new { new_vm.TableId });

        }

        public async Task<IActionResult> Memorizza(TabelleInterneViewModel new_vm)
        {
            var ErrorMessage = "";
            var SuccessMessage = "";

            try
            {

                if (!ModelState.IsValid)
                    throw new Exception("Errore: " + ControllerUtility.GetErrorsString(ModelState));

                if (new_vm.Id == null) new_vm.Id = Guid.NewGuid().ToString();

                var config = new MapperConfiguration(cfg => cfg.CreateMap<TabelleInterneViewModel, LookupTable>());
                Mapper mp = new Mapper(config);
                var new_lt = mp.Map<LookupTable>(new_vm);

                new_lt.Id = new_vm.Id.ToUpper();

                await _bl.Insert(new_lt);
                SuccessMessage = "Tabella Interna aggiunta con successo!";
            }
            catch (Exception ex)
            {

                _logger.LogError("TabelleInterne -> Memorizza -> THROW an Exception : " + ex.Message);
                ErrorMessage = "Errore durante la creazione della tabella interna : " + ex.Message;
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Index", new { new_vm.TableId });

        }

        public async Task<IActionResult> Delete(string TableId, string Id)
        {
            var ErrorMessage = "";
            var SuccessMessage = "";

            try
            {

                //var config = new MapperConfiguration(cfg => cfg.CreateMap<TabelleInterneViewModel, LookupTable>());
                //Mapper mp = new Mapper(config);
                //var new_lt = mp.Map<LookupTable>(new_vm);

                //new_lt.Id = new_vm.Id.ToUpper();

                //await _bl.Delete(new_lt);

                var lt = await _bl.GetLookupTable(TableId, Id);

                await _bl.Delete(lt);
                SuccessMessage = "Tabella Interna eliminata con successo!";

            }
            catch (Exception ex)
            {

                _logger.LogError("TabelleInterne -> Delete -> THROW an Exception : " + ex.Message);
                ErrorMessage = "Errore durant la creazione della tabella interna : " + ex.Message;
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Index", new { TableId });

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}