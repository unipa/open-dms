using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenDMS.Core.Filters;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Enumerators;
using System.Diagnostics;
using Web.BL.Interface;
using Web.DTOs;
using Web.Model.Admin;
using Web.Utilities;

namespace Web.Controllers.Admin
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorization(OpenDMS.Domain.Constants.PermissionType.CanAdminACL)]
    [Route("Admin/[controller]/[action]")]
    public class PermessiGlobaliController : Controller
    {
        private readonly ILogger<PermessiGlobaliController> _logger;
        private readonly IPermessiBL _bl;
        private readonly ILookupTableService _lookupTableService;

        public PermessiGlobaliController(ILogger<PermessiGlobaliController> logger, IPermessiBL bl,  ILookupTableService lookupTableService)
        {
            _logger = logger;
            _bl = bl;
            _lookupTableService = lookupTableService;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new PermessiGlobaliViewModel();

            try
            {
                //INIZIALIZZO I DATI

                //ottengo l'acl dei permessi globali e lo inserisco nel VM
                var mb = new ACL();
                try
                {
                    mb = await _bl.GetACL(PermessiGlobaliViewModel.Id);
                }
                catch (Exception ex)
                {
                    TempData["SuccessMessage"] = "I dati relativi ai permessi Globali non sono stati trovati in memoria. Errore: " + ex.Message + " ; ";
                }

                var config = new MapperConfiguration(cfg => cfg.CreateMap<ACL, PermessiGlobaliViewModel>());
                Mapper mp = new Mapper(config);
                vm = mp.Map<PermessiGlobaliViewModel>(mb);

                //ottengo tutti le authorization (ACLPermission) per l'acl permessi globali
                try
                {
                    //ottengo le ACLPermissions convertendole in ACLPermission_DTO
                    var permsList = (await _bl.GetAuthorizations(PermessiGlobaliViewModel.Id)).ToList();

                    foreach (var perm in permsList)
                    {
                        var pname = (await _lookupTableService.GetById("$PERMISSIONS$", perm.PermissionId)).Description;
                        vm.Authorizations.Add(new ACLPermission_DTO()
                        {
                            ACLId = perm.ACLId,
                            ProfileId = perm.ProfileId,
                            ProfileName = perm.ProfileId,
                            ProfileType = perm.ProfileType,
                            PermissionId = perm.PermissionId,
                            PermissionName = pname,
                            Authorization = perm.Authorization
                        });
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Non sono state trovate autorizazioni. Errore: " + ex.Message + " ; ";
                }

                //ottengo la lista dei permessi dalla lookuptable $PERMISSIONS$
                var ltPermessi = (await _lookupTableService.GetAll("$PERMISSIONS$")).ToList();

                ViewBag.PermessiList = new List<SelectListItem>();

                foreach (var lt in ltPermessi)
                    ViewBag.PermessiList.Add(new SelectListItem { Text = lt.Description, Value = lt.Id });

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
                _logger.LogError("PermessiGlobaliController -> Index -> THROW an Exception : " + ex.Message);
                throw ex;
            }
            return View(vm);
        }

        public async Task<IActionResult> AggiungiAutorizzazione(ACLPermission_DTO aCLPermission)
        {
            var SuccessMessage = "";
            var ErrorMessage = "";

            try
            {

                aCLPermission.ACLId = PermessiGlobaliViewModel.Id;

                if (!ModelState.IsValid)
                {
                    ErrorMessage = "Errore: " + ControllerUtility.GetErrorsString(ModelState);
                }
                else
                {
                    aCLPermission.ProfileType = (ProfileType)int.Parse(aCLPermission.ProfileId.Substring(0, 1));
                    aCLPermission.ProfileId = aCLPermission.ProfileId.Substring(1);

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<ACLPermission_DTO, ACLPermission>());
                    Mapper mp = new Mapper(config);
                    var perm = mp.Map<ACLPermission>(aCLPermission);

                    await _bl.AddAuthorization(perm);
                    SuccessMessage = "Autorizzazione aggiunta con successo.";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("PermessiGlobaliController -> AggiungiAutorizzazione -> THROW an Exception : " + ex.Message);
                ErrorMessage = "Errore in fase di aggiunzione del permesso: " + ex.Message;
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> ModificaAutorizzazione(ACLPermission aCLPermission)
        {
            var SuccessMessage = "";
            var ErrorMessage = "";

            try
            {
                aCLPermission.ACLId = PermessiGlobaliViewModel.Id;

                if (!ModelState.IsValid)
                {
                    var message = ControllerUtility.GetErrorsString(ModelState);
                    ErrorMessage = "I dati non sono stati caricati correttamente. Errore: " + message;
                }
                else
                {
                    await _bl.UpdateAuthorization(aCLPermission);
                    SuccessMessage = "Autorizzazione aggiornata con successo.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("PermessiGlobaliController -> ModificaAutorizzazione -> THROW an Exception : " + ex.Message);
                ErrorMessage = "Errore in fase di modifica del permesso: " + ex.Message;
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> EliminaAutorizzazione(string ACLId, string ProfileId, ProfileType profileType, string PermissionId)
        {
            var SuccessMessage = "";
            var ErrorMessage = "";

            try
            {
                ACLId = PermessiGlobaliViewModel.Id;

                if (!ModelState.IsValid)
                {
                    ErrorMessage = "I dati non sono stati caricati correttamente";
                }
                else
                {
                    var permission = new ACLPermission() { ACLId = ACLId, ProfileId = ProfileId, ProfileType = profileType, PermissionId = PermissionId };
                    await _bl.DeleteAuthorization(permission/*ACLId, ProfileId, profileType, PermissionId*/);
                    SuccessMessage = "Autorizzazione eliminata con successo.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("PermessiGlobaliController -> EliminaAutorizzazione -> THROW an Exception : " + ex.Message);
                ErrorMessage = "Errore in fase di eliminazione del permesso: " + ex.Message;
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Index");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}