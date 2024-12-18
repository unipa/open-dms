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
    public class PermessiController : Controller
    {
        private readonly ILogger<PermessiController> _logger;
        private readonly IPermessiBL _bl;
        private readonly ILookupTableService lookupService;
        private readonly ILookupTableService _lookupTableService;


        public PermessiController(ILogger<PermessiController> logger, IPermessiBL bl, ILookupTableService lookupService, ILookupTableService lookupTableService = null)
        {
            _logger = logger;
            _bl = bl;
            this.lookupService = lookupService;
            _lookupTableService = lookupTableService;
        }

        public async Task<IActionResult> Index(string? Id = "")
        {
            var vm = new PermessiViewModel();

            try
            {
                //INIZIALIZZO I DATI

                if (!string.IsNullOrEmpty(Id))
                {
                    var mb = new ACL();
                    try
                    {
                        mb = await _bl.GetACL(Id);
                    }
                    catch (Exception ex)
                    {
                        TempData["SuccessMessage"] = "L'ACL cercata non è stata trovata in memoria. Errore: " + ex.Message + " ; ";
                    }
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<ACL, PermessiViewModel>());
                    Mapper mp = new Mapper(config);
                    vm = mp.Map<PermessiViewModel>(mb);
                    //ottengo tutti le authorization (ACLPermission) per l'acl esistenti
                    try
                    {
                        //ottengo le ACLPermissions convertendole in ACLPermission_DTO
                        var permsList = (await _bl.GetAuthorizations(Id)).ToList();

                        foreach (var perm in permsList)
                        {
                            var pname = (await lookupService.GetById("$PERMISSIONS$", perm.PermissionId)).Description;
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
                        TempData["SuccessMessage"] = "Non sono state trovate autorizazioni. Errore: " + ex.Message + " ; ";
                    }
                }

                //ottengo tutti gli acl esistenti
                try
                {
                    //rimuovo l'acl dei permessi globali (che ha una pagina dedicata)
                    vm.ACLs = (await _bl.GetAllACL()).Where(acl => !acl.Id.Equals("$GLOBAL$")).ToList();
                }
                catch (Exception ex)
                {
                    TempData["SuccessMessage"] = "Non sono state trovate ACL. Errore: " + ex.Message + " ; ";
                    vm.ACLs = new List<ACL>();
                }
                if (vm.ACLs == null) vm.ACLs = new List<ACL>();

                //ottengo la lista dei permessi dalla lookuptable $PERMISSIONS$ filtrando solo per quelli relativi ai documenti
                var ltPermessi = (await _lookupTableService.GetAll("$PERMISSIONS$")).Where(p => p.Id.StartsWith("Document.") || p.Id.StartsWith("DOCUMENT.")).ToList(); 

                ViewBag.PermessiList = new List<SelectListItem>();
                
                foreach (var lt in ltPermessi)
                    ViewBag.PermessiList.Add(new SelectListItem { Text = lt.Description.Replace("Documento - ", ""), Value = lt.Id });

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
                _logger.LogError("PermessiController -> Index -> THROW an Exception : " + ex.Message);
                throw ex;
            }
            return View(vm);
        }

        public async Task<IActionResult> Memorizza(PermessiViewModel vm)
        {
            var SuccessMessage = "";
            var ErrorMessage = "";

            try
            {

                vm.Id = Guid.NewGuid().ToString();
                var config = new MapperConfiguration(cfg => cfg.CreateMap<PermessiViewModel, ACL>());
                Mapper mp = new Mapper(config);
                var newAcl = mp.Map<ACL>(vm);

                if (!ModelState.IsValid)
                {
                    var message = ControllerUtility.GetErrorsString(ModelState);
                    ErrorMessage = "I dati non sono stati caricati correttamente. Errore: " + message;
                }
                else
                {
                    try
                    {
                        vm.Id = await _bl.Insert(newAcl);
                        SuccessMessage = "ACL aggiunta con successo.";
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = "Errore: " + ex.Message;
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("PermessiController -> Memorizza -> THROW an Exception : " + ex.Message);
                ErrorMessage = ex.Message;
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Index", new { vm.Id });
        }

        public async Task<IActionResult> Elimina(string ACLId)
        {
            var SuccessMessage = "";
            var ErrorMessage = "";

            try
            {
                if (!await _bl.ACLHasBind(ACLId))
                {
                    await _bl.Delete(ACLId);
                    SuccessMessage = "ACL eliminata con successo.";
                }
                else
                {
                    ErrorMessage = "Errore in fase di eliminazione della ACL: la ACL che si sta cercando di eliminare è usata in una tipologia documentale.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("PermessiController -> Elimina -> THROW an Exception : " + ex.Message);

                ErrorMessage = "Errore in fase di eliminazione della ACL: " + ex.Message;
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Index", new { Id = "" });

        }

        public async Task<IActionResult> AggiungiAutorizzazione(ACLPermission_DTO aCLPermission)
        {
            var SuccessMessage = "";
            var ErrorMessage = "";
            var Id = aCLPermission.ACLId;

            try
            {

                if (!ModelState.IsValid)
                {
                    var message = ControllerUtility.GetErrorsString(ModelState);
                    ErrorMessage = "I dati non sono stati caricati correttamente. Errore: " + message;
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
                _logger.LogError("PermessiController -> AggiungiAutorizzazione -> THROW an Exception : " + ex.Message);
                ErrorMessage = "Errore in fase di salvataggio del permesso: " + ex.Message;
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Index", new { Id });

        }

        public async Task<IActionResult> ModificaAutorizzazione(ACLPermission aCLPermission)
        {
            var SuccessMessage = "";
            var ErrorMessage = "";
            var Id = aCLPermission.ACLId;

            try
            {
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
                _logger.LogError("PermessiController -> ModificaAutorizzazione -> THROW an Exception : " + ex.Message);
                ErrorMessage = "Errore in fase di modifica del permesso: " + ex.Message;
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Index", new { Id });

        }

        public async Task<IActionResult> EliminaAutorizzazione(string ACLId, string ProfileId, ProfileType profileType, string permissionId)
        {
            var SuccessMessage = "";
            var ErrorMessage = "";
            var Id = ACLId;

            try
            {
                if (!ModelState.IsValid)
                {
                    Id = "";
                    ErrorMessage = "I dati non sono stati caricati correttamente";
                }
                else
                {
                    var permission = new ACLPermission() { ACLId = ACLId, ProfileId = ProfileId, ProfileType = profileType, PermissionId = permissionId };
                    await _bl.DeleteAuthorization(permission/*ACLId, ProfileId, profileType, PermissionId*/);
                    SuccessMessage = "Autorizzazione eliminata con successo.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("PermessiController -> EliminaAutorizzazione -> THROW an Exception : " + ex.Message);
                ErrorMessage = "Errore in fase di eliminazione del permesso: " + ex.Message;
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Index", new { Id });

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}