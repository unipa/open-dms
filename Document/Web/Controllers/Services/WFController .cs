using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using Web.DTOs;

namespace Web.Controllers.Services;

[Authorize]
[ApiController]
[Route("internalapi/wf")]
public class WFController : ControllerBase
{
    private readonly IWorkflowEngine workflowEngine;
    private readonly IDocumentService documentService;
    private readonly IDocumentTypeService docTypeService;
    private readonly IUserTaskService userTaskService;
    private readonly IProcessMonitorService processMonitorService;
    private readonly IUserService userService;
    private readonly IProcessInstanceRepository processInstanceRepository;
    private readonly ILoggedUserProfile userContext;

    public WFController(
        IWorkflowEngine workflowEngine,
        IDocumentService documentService,
        IDocumentTypeService docTypeService,
        IUserTaskService userTaskService,
        IProcessMonitorService processMonitorService,
        IUserService userService,
        IProcessInstanceRepository processInstanceRepository,
        ILoggedUserProfile userContext
        )
    {
        this.workflowEngine = workflowEngine;
        this.documentService = documentService;
        this.docTypeService = docTypeService;
        this.userTaskService = userTaskService;
        this.processMonitorService = processMonitorService;
        this.userService = userService;
        this.processInstanceRepository = processInstanceRepository;
        this.userContext = userContext;
    }



    /// <summary>
    /// Restituisce i dati relativi ai permessi su un documento tramite Id documento. 
    /// </summary>
    /// <param name="documentId">Id del documento.</param>
    /// <returns>Restituisce una lista di oggetti DocumentPermissions</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<DocumentPermission>))]
    [HttpGet("{documentId}")]
    public async Task<ActionResult<List<ProfilePermissions>>> GetByDocument(int documentId)
    {
        //TODO: Passare DTO con nome permesso
        try
        {
            return Ok((await documentService.GetDocumentPermissions(documentId)).Where(p => p.Permissions.Any(a => a.PermissionId == PermissionType.CanExecute)));
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary> 
    /// Salva un nuovo permesso per un documento.
    /// </summary>
    /// <param name="documentId">Id del documento.</param>
    /// <param name="profile">Id profilo nella forma <profileType><profileId></param>
    /// <param name="permissionId"> Tipo di permesso. </param>
    /// <param name="authorization"> Tipo di autorizzazione </param>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(DocumentPermission))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost("{documentId}/{profile}")]
    public async Task<ActionResult> Set(int documentId, string profile)
    {
        if (documentId <= 0) return BadRequest();
        ProfileType ProfileType = (ProfileType)int.Parse(profile.Substring(0, 1));
        string ProfileId = profile.Substring(1);
        var u = userContext.Get();
        var P = await documentService.GetPermission(documentId, u, PermissionType.CanEdit);
        if (P == null || P.Authorization != AuthorizationType.Granted) return BadRequest("Non sei autorizzato a impostare questo permesso");
        try
        {
            await documentService.SetPermission(documentId, u,  ProfileId, ProfileType, PermissionType.CanExecute, AuthorizationType.Granted);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /// <summary> 
    /// Salva un nuovo permesso per un documento.
    /// </summary>
    /// <param name="documentId">Id del documento.</param>
    /// <param name="profile">Id profilo nella forma <profileType><profileId></param>
    /// <param name="permissionId"> Tipo di permesso. </param>
    /// <param name="authorization"> Tipo di autorizzazione </param>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(DocumentPermission))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpDelete("{documentId}/{profile}")]
    public async Task<ActionResult> Remove(int documentId, string profile)
    {
        if (documentId <= 0) return BadRequest();
        ProfileType ProfileType = (ProfileType)int.Parse(profile.Substring(0, 1));
        string ProfileId = profile.Substring(1);
        var u = userContext.Get();
        var P = await documentService.GetPermission(documentId, u, PermissionType.CanEdit);
        if (P == null || P.Authorization != AuthorizationType.Granted) return BadRequest("Non sei autorizzato a impostare questo permesso");
        try
        {
            await documentService.SetPermission(documentId, u, ProfileId, ProfileType, PermissionType.CanExecute, AuthorizationType.None);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    ///  Pubblica un nuovo processo o aggiorna un processo precedentemente pubblicato
    /// </summary>
    /// <param name="documentId"></param>
    /// <returns></returns>
    /// 
    [HttpGet("deploy/{documentId}")]
    public async Task<IActionResult> DeployNewProcess(int documentId)
    {
        if (documentId <= 0) return BadRequest();
        var u = userContext.Get();
        var P = await documentService.GetPermission(documentId, u, PermissionType.CanPublish);
        if (P == null || P.Authorization != AuthorizationType.Granted) return BadRequest("Non sei autorizzato a pubblicare questo processo");
        try
        {
            var doc = await documentService.Load(documentId, u);
            var diagram = System.Text.Encoding.UTF8.GetString(await documentService.GetContent(doc.Image.Id));
            //var key = doc.DocumentNumber;
            var key = doc.DocumentNumber;
            if (int.TryParse(key, out int numero))
            {
                key = "ID" + key;
            }
            try
            {

                var status = await workflowEngine.DeployNewProcess(diagram, key + Path.GetExtension(doc.Image.FileName));
                if (status > 0)
                {
                    await documentService.UpdateSendingStatus(doc.Image.Id, JobStatus.Completed, u);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                await documentService.UpdateSendingStatus(doc.Image.Id, JobStatus.Failed, u);
                return BadRequest(ex.Message);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return NotFound();
    }

    public class CustomAction
    {
        public string Icon { get; set; }
        
        public string Label { get; set; }

        /// <summary>
        /// Contiene il messaggio (message:correlationKey) o la businesskey di un processo (order-process) da richiamare
        /// </summary>
        public string Action { get; set; }

        public string Tooltip { get; set; }

    }


    /// <summary>
    ///  Pubblica un nuovo processo o aggiorna un processo precedentemente pubblicato
    /// </summary>
    /// <param name="documentId"></param>
    /// <returns></returns>
    /// 
    [HttpGet("getCustomActions/{documentId}")]
    public async Task<IActionResult> GetCustomActions(int documentId)
    {
        List<CustomAction> OutputVariables = new List<CustomAction>();
        if (documentId <= 0) return BadRequest();
        var u = userContext.Get();
        var P = await documentService.GetPermission(documentId, u, PermissionType.CanView);
        if (P == null || P.Authorization != AuthorizationType.Granted) return BadRequest("Non sei autorizzato ad accedere a questo documento");
        try
        {
            var doc = await documentService.Load(documentId, u);
            // verifico se esiste un workflow per l'evento di visualizzazione di un documento
            if (doc != null)
            {
                await documentService.View(doc, u);
                var workflows = await docTypeService.GetWorkflow(doc.DocumentType.Id, EventType.View);
                if (workflows != null)
                {
                    
//                    var processDefinition = await docRepo.Load(workflows.ProcessId, u);
//                    if (processDefinition != null)
//                    {
                    var FakeEvent = new DocumentEventMessage(doc, u, EventType.View, null);

                    var key = workflows.ProcessKey;// processDefinition.DocumentNumber;
                    if (int.TryParse(key, out int numero))
                    {
                        key = "ID" + key;
                    }

                    string variables = Newtonsoft.Json.JsonConvert.SerializeObject(FakeEvent.Variables);
                    var process = await workflowEngine.StartProcessAndWait(key, variables);
                    OutputVariables = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CustomAction>>(JObject.Parse(process)["actions"].ToString());
//                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        return Ok(new JsonResult( OutputVariables));
    }


    /// <summary>
    ///  Pubblica un nuovo processo o aggiorna un processo precedentemente pubblicato
    /// </summary>
    /// <param name="documentId"></param>
    /// <returns></returns>
    /// 
    [HttpGet("startCustomAction")]
    public async Task<IActionResult> StartCustomAction(int documentId, string action)
    {
        Dictionary<string, string> OutputVariables = new();
        if (documentId <= 0) return BadRequest();
        var u = userContext.Get();
        var P = await documentService.GetPermission(documentId, u, PermissionType.CanView);
        if (P == null || P.Authorization != AuthorizationType.Granted) return BadRequest("Non sei autorizzato ad accedere a questo documento");
        try
        {
            var doc = await documentService.Load(documentId, u);
            // verifico se esiste un workflow per l'evento di visualizzazione di un documento
            //var workflows = await docTypeService.GetWorkflow(doc.DocumentTypeId, EventType.View);
            //if (workflows != null)
            {
                //var processDefinition = await docRepo.Load(workflows.ProcessId, u);
                //if (processDefinition != null)
                {
                    //OutputVariables["processId"] = workflows.ProcessId.ToString();
                    var FakeEvent = new DocumentEventMessage(doc, u, EventType.View, null);

                    string variables = Newtonsoft.Json.JsonConvert.SerializeObject(FakeEvent.Variables);
                    if (action.Contains(":"))
                    {
                        var i = action.IndexOf(":");
                        var message = action.Substring(0, i);
                        var key = action.Substring(i+1);
                        await workflowEngine.SendMessage (message, key, variables);
                    }
                    else
                    {
                        if (int.TryParse(action, out int numero))
                        {
                            action = "ID" + action;
                        }

                        //var process = await workflowEngine.StartProcessAndWait(processDefinition.Id.ToString(), variables);
                        //OutputVariables["processInstanceKey"] = process.ProcessInstanceKey.ToString();
                        //OutputVariables["BpmnProcessId"] = process.BpmnProcessId;
                        //OutputVariables["URL"] = (string?)JObject.Parse(process.Variables)["URL"] ?? "";
                        //OutputVariables["ExternalURL"] = (string?)JObject.Parse(process.Variables)["ExternalURL"] ?? "";
                        //OutputVariables["NewProcess"] = (string?)JObject.Parse(process.Variables)["NewProcess"] ?? "";
                        //OutputVariables["Error"] = (string?)JObject.Parse(process.Variables)["Error"] ?? "";

                        var diagramId = await documentService.FindByUniqueId(null, action, ContentType.Workflow);
                        var diagram = await documentService.Get(diagramId);
                        var process = await workflowEngine.StartProcess (action,doc, u, EventType.View, variables);
                        OutputVariables["processInstanceKey"] = process.ToString();
                        OutputVariables["BpmnProcessId"] = action;

                    }
                }
            }
        }
        catch (Exception ex)
        {
            OutputVariables["Error"] = ex.Message;
        }
        return Ok(OutputVariables);
    }



    /// <summary>
    ///  Pubblica un nuovo processo o aggiorna un processo precedentemente pubblicato
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    /// 
    [HttpGet("getCustomTaskActions/{taskId}")]
    public async Task<IActionResult> GetCustomTaskActions(int taskId)
    {
        List<CustomAction> OutputVariables = new List<CustomAction>();
        if (taskId <= 0) return BadRequest();
        var u = userContext.Get();
        try
        {
            var task = await userTaskService.GetById(taskId, u);
            // verifico se esiste un workflow per l'evento di visualizzazione di un documento
            if (task != null)
            {
                var doc = task.TaskItemInfo.Attachments.FirstOrDefault();
                if (doc != null)
                {
                    var workflows = await docTypeService.GetWorkflow(doc.DocumentType.Id, EventType.View);
                    if (workflows != null)
                    {
                        var FakeEvent = new UserTaskEventMessage (task, u, EventType.UserTaskViewed, task.TaskItemInfo.Form.Data);
                        var key = workflows.ProcessKey;// processDefinition.DocumentNumber;
                        if (int.TryParse(key, out int numero))
                        {
                            key = "ID" + key;
                        }

                        string variables = Newtonsoft.Json.JsonConvert.SerializeObject(FakeEvent.Variables);
                        var process = await workflowEngine.StartProcessAndWait(key, variables);
                        OutputVariables = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CustomAction>>(JObject.Parse(process)["actions"].ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        return Ok(new JsonResult(OutputVariables));
    }


    /// <summary>
    ///  Pubblica un nuovo processo o aggiorna un processo precedentemente pubblicato
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    /// 
    [HttpGet("startCustomTaskAction")]
    public async Task<IActionResult> StartCustomTaskAction(int taskId, string action)
    {
        Dictionary<string, string> OutputVariables = new();
        if (taskId <= 0) return BadRequest();
        var u = userContext.Get();
        try
        {
            var task = await userTaskService.GetById(taskId, u);
            {
                var doc = task.TaskItemInfo.Attachments.FirstOrDefault();
                if (doc != null)
                {
                    var FakeEvent = new UserTaskEventMessage(task, u, EventType.UserTaskViewed, task.TaskItemInfo.Form.Data);
                    string variables = Newtonsoft.Json.JsonConvert.SerializeObject(FakeEvent.Variables);
                    if (action.Contains(":"))
                    {
                        var i = action.IndexOf(":");
                        var message = action.Substring(0, i);
                        var key = action.Substring(i + 1);
                        await workflowEngine.SendMessage(message, key, variables);
                    }
                    else
                    {
                        if (int.TryParse(action, out int numero))
                        {
                            action = "ID" + action;
                        }
                        var diagramId = await documentService.FindByUniqueId(null, action, ContentType.Workflow);
                        var diagram = await documentService.Get(diagramId);
                        var process = await workflowEngine.StartProcess(action, doc, u, EventType.View, variables);
                        OutputVariables["processInstanceKey"] = process.ToString();
                        OutputVariables["BpmnProcessId"] = action;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            OutputVariables["Error"] = ex.Message;
        }
        return Ok(OutputVariables);
    }



    /// <summary>
    /// Ritorna i task utente disponibili per il processo indicato
    /// </summary>
    /// <param name="processInstanceKey"></param>
    /// <returns></returns>
    /// 
    [HttpGet("getProcessTasks/{projectId}")]
    public async Task<IActionResult> getTasks(int projectId)
    {
        var u = userContext.Get();
        var userTaskItems = new List<UserTaskListItem>();
        //var processo = await docRepo.Get(processInstanceKey);
        try
        {
            OpenDMS.Domain.Models.TaskListFilter filters = new()
            {
                Received = true,
                Status = new List<ExecutionStatus>() { ExecutionStatus.Unassigned, ExecutionStatus.Assigned },
                ProjectId = projectId
            };
            userTaskItems = await userTaskService.Find(filters, u);
        }
        catch (Exception ex)
        {
        }
        return Ok(userTaskItems);
    }

    
    [HttpGet("restartJob/{incidentKey}/{incidentJob}")]
    public async Task<IActionResult> retryJob(string incidentKey, string incidentJob)
    {
        try
        {
            await workflowEngine.ResolveIncident(incidentKey, incidentJob, 3);
        }
        catch (Exception ex){
            return BadRequest(ex.Message);
        }
        return Ok();
    }


    [HttpGet("setVariable/{processId}/{elementKey}/{incidentKey}/{incidentJob}")]
    public async Task<IActionResult> setVariable(
        string processId,
        string elementKey,
        string incidentKey, 
        string incidentJob,
        string variableName, 
        string Value)
    {
        try
        {
            if (elementKey == "-1" || elementKey == "0") elementKey = processId;
            await workflowEngine.SetVariables(elementKey, ($"{{ \"{variableName}\" : {Value} }}"), true);
            if (incidentKey != "0" && incidentJob != "0" && incidentKey != "-1" && incidentJob != "-1")
                await workflowEngine.ResolveIncident(incidentKey, incidentJob, 3);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok();
    }



    [HttpGet("getProcessesByDocumentId/{documentId}")]
    public async Task<List<ProcessInstanceInfo_DTO>> GetProcesses (int documentId)
    {
        List<ProcessInstanceInfo_DTO> plist = new();
        var su = await userService.GetUserProfile(SpecialUser.SystemUser);
        var userProfile = await userService.GetUserProfile(this.User.Identity.Name);
        bool IsWorkflowArchitect = userProfile.GlobalRoles.Any(r => r.Id == SpecialUser.WorkflowArchitect || r.Id == SpecialUser.AdminRole);
        //var tasks = await userTaskService.GetAllDocumentTasks(documentId, su);
        var processes = await processInstanceRepository.GetByDocumentId(documentId);
        foreach (var pi in processes)
        {
            ProcessInstanceInfo_DTO p = new();
            p.Id = pi.ProcessInstanceId;
            p.DocumentId = documentId;
            p.EventName = pi.EventName;
            p.DefinitionKey = pi.ProcessKey;
            p.DefinitionId = pi.ProcessDefinitionId;
            var doc = await documentService.Get(pi.ProcessDefinitionId);
            p.Version = doc.Image.VersionNumber + "." + doc.Image.RevisionNumber;
            p.Description = doc.Description;
            p.Icon = doc.Icon;
            p.IconColor = doc.IconColor;
            p.StartTime = pi.StartDate.ToString("dd/MM/yyyy HH:mm");
            p.StartUserId = pi.StartUser;
            p.StartUserName = await userService.GetName(pi.StartUser);
            var pinfo = await workflowEngine.GetInstanceById(p.Id);
            if (pinfo != null)
            {
                p.Problems = pinfo.incidents;
                if (IsWorkflowArchitect)
                {
                    if (pinfo.variables.ContainsKey("Document"))
                        pinfo.variables.Remove("Document");
                    if (pinfo.variables.ContainsKey("UserProfile"))
                        pinfo.variables.Remove("UserProfile");
                }
                p.Variables = IsWorkflowArchitect ? pinfo.variables : new();
            }
            var ptasks = await userTaskService.GetByProcessId(pi.ProcessInstanceId, su);
            foreach (var i in ptasks)
            {
                var ut = await userTaskService.GetById(i.Id, su);
                if (ut != null)// && ut.TaskItemInfo.ProcessId == pi.ProcessInstanceId)
                {
                    ut.TaskItemInfo.Form.Schema = "";
                    ut.TaskItemInfo.Form.Data = null;
                    ut.TaskItemInfo.Attachments = null;
                    if (i.Status != ExecutionStatus.Executed && i.Status != ExecutionStatus.Validated)
                        p.ActiveTasks.Add(ut);
                    else
                        p.ClosedTasks.Add(ut);
                }
            }
            plist.Add(p);
        }
        var altriTask = (await userTaskService.GetByDocument(documentId, su)).Where(t => string.IsNullOrEmpty(t.TaskItem.ProcessInstanceId));
        if (altriTask.Count() > 0)
        {
            ProcessInstanceInfo_DTO p = new();
            p.Id = "";
            p.DocumentId = documentId;
            p.EventName = "";
            p.DefinitionKey = "";
            p.DefinitionId = 0;
            p.Version = "";
            p.Description = "Richieste di altri utenti";
            p.Icon = "";
            p.IconColor = "";
            p.StartTime = "";
            p.StartUserId = "";
            p.StartUserName = "";
            foreach (var i in altriTask)
            {
                var ut = await userTaskService.GetById(i.Id, su);
                if (ut != null)
                {
                    ut.TaskItemInfo.Form.Schema = "";
                    ut.TaskItemInfo.Form.Data = null;
                    ut.TaskItemInfo.Attachments = null;
                    if (i.Status != ExecutionStatus.Executed && i.Status != ExecutionStatus.Validated)
                        p.ActiveTasks.Add(ut);
                    else
                        p.ClosedTasks.Add(ut);
                }
            }
            plist.Add(p);
        }
        return plist;
    }


    //[HttpGet("ActiveProcess")]
    //public async Task<IActionResult> ActiveProcess(string? pid)
    //{
    //    var u = userContext.Get();
    //    var Procs = new List<ProcessSummary_DTO>();
    //    try
    //    {
    //        return Ok(await processMonitorService.GetActiveProcesses(u, pid));
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}

    //[HttpGet("ClosedProcess")]
    //public async Task<IActionResult> ClosedProcess(int period, string? pid)
    //{
    //    var u = userContext.Get();
    //    DateTime FromDate = DateTime.Now.AddDays(-1*DateTime.Now.Day+1);
    //    DateTime ToDate = DateTime.Now;
    //    switch (period)
    //    {
    //        case 0: // Mese corrente
    //            break;
    //        case 1: // Mese precedente
    //            FromDate = DateTime.Now.AddDays(-1 * DateTime.Now.Day +1).AddMonths(-1);
    //            ToDate = DateTime.Now.AddDays(-1 * DateTime.Now.Day);
    //            break;
    //        case 10: // Anno corrente
    //            FromDate = DateTime.Now.AddDays(-1 * DateTime.Now.Day + 1).AddMonths(-1 * DateTime.Now.Month + 1);
    //            break;
    //        case 11: // Anno Precedente
    //            FromDate = new DateTime(DateTime.Now.Year - 1, 1, 1);
    //            ToDate = new DateTime(DateTime.Now.Year - 1, 31, 12);
    //            break;
    //        case 12: // Mese corrente
    //            FromDate = new DateTime(DateTime.Now.Year - 2, 1, 1);
    //            ToDate = new DateTime(DateTime.Now.Year - 2, 31, 12);
    //            break;
    //        case 20: // Mese corrente
    //            FromDate = new DateTime(DateTime.Now.Year - 2, 1, 1);
    //            break;
    //        case 21: // Mese corrente
    //            FromDate = new DateTime(DateTime.Now.Year - 3, 1, 1);
    //            break;
    //        default:
    //            FromDate = DateTime.MinValue;
    //            ToDate = DateTime.MinValue;
    //            break;
    //    }
    //    var Procs = new List<ProcessSummary_DTO>();
    //    try
    //    {
    //        return Ok(await processMonitorService.GetClosedProcesses(u, FromDate, ToDate, pid));
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}

    [HttpGet("ActiveProcessDetails")]
    public async Task<IActionResult> ActiveProcessDetails(string pid)
    {
        var u = userContext.Get();
        try
        {
            return new JsonResult ( new
                {
                    expired = await processMonitorService.GetActiveProcessDetails(u, pid, true),
                    active = await processMonitorService.GetActiveProcessDetails(u, pid, false)
            }
            );
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet("ClosedProcessDetails")]
    public async Task<IActionResult> ClosedProcessDetails (int period, string pid)
    {
        var u = userContext.Get();
        DateTime FromDate = DateTime.Now.AddDays(-1 * DateTime.Now.Day + 1);
        DateTime ToDate = DateTime.Now;
        switch (period)
        {
            case 0: // Mese corrente
                break;
            case 1: // Mese precedente
                FromDate = DateTime.Now.AddDays(-1 * DateTime.Now.Day + 1).AddMonths(-1);
                ToDate = DateTime.Now.AddDays(-1 * DateTime.Now.Day);
                break;
            case 10: // Anno corrente
                FromDate = DateTime.Now.AddDays(-1 * DateTime.Now.Day + 1).AddMonths(-1 * DateTime.Now.Month + 1);
                break;
            case 11: // Anno Precedente
                FromDate = new DateTime(DateTime.Now.Year - 1, 1, 1);
                ToDate = new DateTime(DateTime.Now.Year - 1, 31, 12);
                break;
            case 12: // Mese corrente
                FromDate = new DateTime(DateTime.Now.Year - 2, 1, 1);
                ToDate = new DateTime(DateTime.Now.Year - 2, 31, 12);
                break;
            case 20: // Mese corrente
                FromDate = new DateTime(DateTime.Now.Year - 2, 1, 1);
                break;
            case 21: // Mese corrente
                FromDate = new DateTime(DateTime.Now.Year - 3, 1, 1);
                break;
            default:
                FromDate = DateTime.MinValue;
                ToDate = DateTime.MinValue;
                break;
        }
        try
        {
            return new JsonResult(new
            {
                expired = await processMonitorService.GetClosedProcessDetails(u, FromDate, ToDate, pid, true),
                active = await processMonitorService.GetClosedProcessDetails(u, FromDate, ToDate, pid, false)
            }
            );
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("RestartJob/{TaskId}")]
    public async Task<IActionResult> RestartJob (int TaskId)
    {
        var u = userContext.Get();

        var t = await userTaskService.GetById(TaskId, u);
        if (t.Status == ExecutionStatus.Executed)
        {

            var ProcessId = t.TaskItemInfo.ProcessId;
            var jobKey = t.TaskItemInfo.ExecutionId;
            var v = t.TaskItemInfo.Form.Data;
            var user = await userService.GetUserProfile(t.UserId);

            if (!string.IsNullOrEmpty(v) && v != "[]" && v != "{}")
            {
                var e = new TaskEventMessage(t.TaskItemInfo, user, EventType.TaskExecuted, v, t.TaskItemInfo.TaskType == TaskType.Event);
                try
                {
                    await workflowEngine.SetVariables(ProcessId, v);
                }
                catch { };
                try
                {
                    await workflowEngine.CompleteJob(jobKey, System.Text.Json.JsonSerializer.Serialize(e.Variables));
                }
                catch (Exception ex) {
                    return BadRequest(ex.Message);
                };
            }
        }
        return Ok();
    }


 
}