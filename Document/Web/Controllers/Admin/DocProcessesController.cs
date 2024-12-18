using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenDMS.Core.Filters;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.Models;
using System.Diagnostics;
using System.Reflection;
using Web.BL.Interface;
using Web.Model.Admin;

namespace Web.Controllers.Admin
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorization(OpenDMS.Domain.Constants.PermissionType.CanAdminDocumentTypes)]

    [Route("Admin/[controller]/[action]")]
    public class DocProcessesController : Controller
    {
        private readonly ILogger<DocProcessesController> _logger;
        private readonly ILoggedUserProfile usercontext;
        private readonly IDocProcessesBL _bl;
        private readonly ITabelleInterneBL _tabelleinterneBl;


        public DocProcessesController(ILogger<DocProcessesController> logger, ILoggedUserProfile usercontext, IDocProcessesBL bl, ITabelleInterneBL tabelleInterneBL)
        {
            _logger = logger;
            this.usercontext = usercontext;
            _bl = bl;
            _tabelleinterneBl = tabelleInterneBL;
        }

        public async Task<IActionResult> Index(string? TypeId = "ALL")
        {
            var vm = new DocProcessesViewModel();
            UserProfile u = usercontext.Get();
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

                vm.TypeId = TypeId;
                vm.Types = (await _bl.GetAllDocTypes(u)).ToList();
                vm.TypeName = TypeId.Equals("ALL") ? "Tutte le tipologie" : vm.Types.FirstOrDefault(t => t.Id.Equals(TypeId)).Name;
                vm.ProcessList = await GetProcessList(u);
                vm.DocumentTypeWorkflow = (await _bl.GetAllDocumentTypeWorkflow(TypeId, vm.Types, vm.ProcessList)).ToList();


                vm.EventList = GetEventList();
                vm.DocumentTypeWorkflow.ForEach((v) =>
                {
                    SelectListItem? item = vm.EventList.Find(e => e.Value == v.EventName);

                    if (item != null)
                        v.EventDescription = item.Text;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("DocProcessesController -> Index -> THROW an Exception : " + ex.Message);
                vm.ErrorMessage = "Non è stato possibile ottenere le informazioni. Errore: " + ex.Message + " ; ";
                vm.Icon = "fa fa-alert";
            }
            return View(vm);
        }

        public async Task<IActionResult> Aggiungi(DocumentTypeWorkflow bd)
        {
            var SuccessMessage = "";
            var ErrorMessage = "";

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Non è stato possibile salvare le informazioni. Errore: I campi non sono stati caricati correttamente.");
                }
                else
                {
                    // aggiungi
                    await _bl.AggiungiDocumentTypeWorkflow(bd);
                    SuccessMessage = "Regola Evento-Processo aggiunta con successo.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("DocProcessesController -> Index -> THROW an Exception : " + ex.Message);
                ErrorMessage = ex.Message;
                //vm.Icon = "fa fa-alert";
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Index", new { TypeId = bd.DocumentTypeId });
        }

        public async Task<IActionResult> Modifica(string TypeId, string EventName, string ProcessKey, string Query)
        {
            var SuccessMessage = "";
            var ErrorMessage = "";

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Non è stato possibile salvare le informazioni. Errore: I campi non sono stati caricati correttamente.");
                }
                else
                {
                    // aggiungi
                    await _bl.ModificaDocumentTypeWorkflow(TypeId, EventName, ProcessKey);
                    SuccessMessage = "Regola Evento-Processo modificata con successo.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("DocProcessesController -> Index -> THROW an Exception : " + ex.Message);
                ErrorMessage = ex.Message;
                //vm.Icon = "fa fa-alert";
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Index", new { TypeId = Query });
        }

        public async Task<IActionResult> Elimina(string TypeId, string EventName, string Query)
        {

            var SuccessMessage = "";
            var ErrorMessage = "";

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Non è stato possibile eliminare le informazioni. Errore: I campi non sono stati caricati correttamente.");
                }
                else
                {
                    // aggiungi
                    await _bl.EliminaDocumentTypeWorkflow(TypeId, EventName);
                    SuccessMessage = "Regola Evento-Processo eliminata con successo.";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("DocProcessesController -> Index -> THROW an Exception : " + ex.Message);
                ErrorMessage = ex.Message;
            }

            TempData["SuccessMessage"] = SuccessMessage;
            TempData["ErrorMessage"] = ErrorMessage;

            return RedirectToAction("Index", new { TypeId = Query });
        }

        //public async Task<IActionResult> CreaProcessoAggiungi( string Name, string EventName, string DocumentTypeId, string Query)
        //{
        //    //action chiamata in fase di creazione di un processo in ambito di aggiunta
        //    var SuccessMessage = "";
        //    var ErrorMessage = "";
        //    var dtw = new DocumentTypeWorkflow();
        //    UserProfile u = usercontext.Get();

        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            throw new Exception("Non è stato possibile creare il documento. Errore: I campi non sono stati caricati correttamente.");
        //        }
        //        else
        //        {
        //            // aggiungi
        //            var processId = await _bl.CreaProcesso(Name,u);
        //            dtw = new DocumentTypeWorkflow() { DocumentTypeId = DocumentTypeId, EventName = EventName, ProcessKey = processId };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("DocProcessesController -> Index -> THROW an Exception : " + ex.Message);
        //        ErrorMessage = ex.Message;
        //    }

        //    TempData["SuccessMessage"] = SuccessMessage;
        //    TempData["ErrorMessage"] = ErrorMessage;

        //    if (!string.IsNullOrEmpty(ErrorMessage))
        //        return RedirectToAction("Index", new { TypeId = Query });
        //    else
        //        return RedirectToAction("Aggiungi", dtw);
        //}

        //public async Task<IActionResult> CreaProcessoModifica( string Name, string EventName, string DocumentTypeId, string Query)
        //{
        //    //action chiamata in fase di creazione di un processo in ambito di modifica
        //    var SuccessMessage = "";
        //    var ErrorMessage = "";
        //    var dtw = new DocumentTypeWorkflow();
        //    UserProfile u = usercontext.Get();

        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            throw new Exception("Non è stato possibile creare il documento. Errore: I campi non sono stati caricati correttamente.");
        //        }
        //        else
        //        {
        //            // aggiungi
        //            var processId = await _bl.CreaProcesso(Name, u);
        //            dtw = new DocumentTypeWorkflow() { DocumentTypeId = DocumentTypeId, EventName = EventName, ProcessKey = processId };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("DocProcessesController -> Index -> THROW an Exception : " + ex.Message);
        //        ErrorMessage = ex.Message;
        //    }

        //    TempData["SuccessMessage"] = SuccessMessage;
        //    TempData["ErrorMessage"] = ErrorMessage;

        //    if (!string.IsNullOrEmpty(ErrorMessage))
        //        return RedirectToAction("Index", new { TypeId = Query });
        //    else
        //        return RedirectToAction("Modifica", new { TypeId = dtw.DocumentTypeId, EventName, dtw.ProcessId, Query });

        //}

        private List<SelectListItem> GetEventList()
        {
            var EventList = new List<SelectListItem>();
            Type type = typeof(EventType);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                string? fieldValue = field.GetValue(null).ToString();
                if (!string.IsNullOrEmpty(fieldValue) && fieldValue.StartsWith("Document.")) //aggiungo solo gli eventi sul documento
                {
                    OpenDMS.Domain.Entities.Settings.LookupTable lookupEntry = _tabelleinterneBl.GetLookupTable("$EVENTS$", fieldValue).GetAwaiter().GetResult();
                    EventList.Add(new SelectListItem { Text = !string.IsNullOrEmpty(lookupEntry.Description) ? lookupEntry.Description : field.Name, Value = fieldValue });
                }
            }

            return EventList;
        }

        private async Task<List<SelectListItem>> GetProcessList(UserProfile u)
        {
            var ProcessIdList = await _bl.GetProcesses(u);

            var ProcessList = new List<SelectListItem>();

            ProcessList.Add(new SelectListItem { Text = "Seleziona un processo", Value = "" });
            //ProcessList.Add(new SelectListItem { Text = "Crea nuovo", Value = "new" });

            foreach (var Id in ProcessIdList)
            {
                var process = await _bl.GetProcess(Id, u);
                if (process != null)
                    ProcessList.Add(
                        new SelectListItem
                        {
                            Text = process.Description + " (" + (process.Image == null || process.Image.SendingStatus != OpenDMS.Domain.Enumerators.JobStatus.Completed ? "Bozza" : process.Image.VersionNumber + "." + process.Image.RevisionNumber.ToString("00")) + ")",
                            Value = process.ExternalId.ToString()
                        }
                    );
            }
            return ProcessList;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}