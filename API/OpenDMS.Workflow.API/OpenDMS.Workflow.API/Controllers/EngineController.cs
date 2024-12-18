using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using OpenDMS.Workflow.API.BusinessLogic.CustomTask;
using OpenDMS.Workflow.API.DTOs;
using OpenDMS.Workflow.API.DTOs.Palette;
using System.Reflection;

namespace OpenDMS.Workflow.API.Controllers
{
    //    [Authorize]
    [ApiController]
    [Route("/api/workflow/[controller]")]
    public class EngineController : ControllerBase
    {
        private readonly IWorkflowEngine engine;
        private readonly IConfiguration configuration;
        private readonly IHostApplicationLifetime hostApplication;
        private readonly IACLService aclService;
        private readonly IUserTaskService userTaskService;
        private readonly ILoggedUserProfile userProfile;
        private readonly ICustomTaskProvider customTaskProvider;
        private readonly IDocumentService documentService;
        private readonly IDocumentTypeService documentTypeService;
        private readonly IUserService userService;

        public EngineController(IWorkflowEngine engine,
            IConfiguration configuration,
            IHostApplicationLifetime hostApplication,
            IACLService aclService,
            IUserTaskService userTaskService,
            ILoggedUserProfile userProfile,
            ICustomTaskProvider customTaskProvider,
            IDocumentService documentService,
            IDocumentTypeService documentTypeService,
            IUserService userService) 
        {
            this.engine = engine;
            this.configuration = configuration;
            this.hostApplication = hostApplication;
            this.aclService = aclService;
            this.userTaskService = userTaskService;
            this.userProfile = userProfile;
            this.customTaskProvider = customTaskProvider;
            this.documentService = documentService;
            this.documentTypeService = documentTypeService;
            this.userService = userService;
        }


        /// <summary>
        /// Ritorna la configurazione dell'engine
        /// </summary>
        /// <returns>Versione dell'engine e elenco dei brokers attivi</returns>
        /// 
        [HttpGet("Status")]
        public async Task<Topology> Status()
        {
            //TODO: Verificare se l'utente ha il permesso di deploy
            return await engine.Status();
        }

        [HttpGet("Log")]
        public async Task<IActionResult> Log()
        {
            var fileName = Path.Combine(Environment.CurrentDirectory, "Log", $"OpenDMS.Workflow.API" + DateTime.Now.ToString("yyyyMMdd") + ".log");
            using (var m = new MemoryStream())
            {
                using (var reader = System.IO.File.Open (fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    await reader.CopyToAsync(m);
                }
                var fileBytes = m.ToArray();
                return File(fileBytes, "application/octet-stream", fileName);
            }
        }

        [HttpGet("Configuration")]
        public async Task<string> Configuration()
        {
            string log = "";

            FieldInfo[] Fields = typeof(OpenDMS.Domain.Constants.StaticConfiguration).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var F in Fields)
            {
                var Value = (string)F.GetRawConstantValue();
                if (Value != null)
                {
                    if (configuration[Value] == null)
                        log += ("## - Setting: " + Value);
                    else
                        log += ("OK - Setting: " + Value + " = " + configuration[Value].ToString());
                    log += Environment.NewLine;
                }
            }
            return log;
        }

        [HttpGet("shutdown")]
        public void Shutdown(string pwd)
        {
            if (pwd==DateTime.Now.ToString("yyyyMMdd"))
            {
                hostApplication.StopApplication();
            }
        }

        /// <summary>
        ///  Pubblica una nuova risorsa o aggiorna una risorsa precedentemente pubblicata
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="resourseContent"></param>
        /// <returns>Identificativo del deploy del processo. Utile per avviare nuove istanze processi</returns>
        /// 
        [HttpPost("Deploy/{processName}")]
        public async Task<long> DeployNewResource (string resourceId, [FromBody] string resourseContent)
        {
            //TODO: Verificare se l'utente ha il permesso di deploy
            return await engine.DeployNewProcess(resourseContent, resourceId);
        }

        /// <summary>
        /// Invia un messaggio ad un processo
        /// </summary>
        /// <param name="messageName"></param>
        /// <param name="correlationKey"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        [HttpPost("SendMessage/{messageName}/{correlationKey}")]
        public async Task SendMessage(string messageName, string correlationKey, [FromBody] string variables)
        {
            //TODO: Verificare sel'utente ha il permesso di inviare un messaggio
            await engine.SendMessage(messageName, correlationKey, variables);
        }


        /// <summary>
        /// Avvia un processo da un modello precedentemente pubblicato
        /// </summary>
        /// <param name="businessProcessId"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        [HttpPost("Start/{businessProcessId}")]
        public async Task<long> StartProcess(string businessProcessId, [FromBody] string variables)
        {
            var u = userProfile.Get();
            var doctypeId = "$EXTERNAL_PROCESS_" + businessProcessId + "$";
            var doctype = await documentTypeService.GetById(doctypeId);
            if (doctype == null)
            {
                doctype = new Domain.Entities.Schemas.DocumentType()
                {
                    Id = doctypeId,
                    Description = "Processo Esterno " + businessProcessId,
                    Direction = 1,
                    ACLId = "$WORKFLOW$",
                    CategoryId = "$WORKFLOW$",
                    ContentType = ContentType.Folder
                };
                await documentTypeService.Create(doctype);
            };

            var diagramId = await documentService.FindByUniqueId(null, businessProcessId, ContentType.Workflow);
            var diagram = await documentService.Get(diagramId);
            CreateOrUpdateDocument doc = new CreateOrUpdateDocument() { 
                DocumentDate = DateTime.UtcNow,
                Description = "Processo: "+diagram.Description,
                DocumentNumber = "",
                ContentType = ContentType.Document,
                DocumentTypeId = doctypeId,
                Owner = u.userId
            };
            var process = await documentService.CreateAndRead(doc, u);
            if (process != null) {
                return await engine.StartProcess(businessProcessId, process, u, EventType.Creation, variables);
            }
            //TODO: Verificare se l'utente ha il permesso di esecuzione
            return 0;
        }

        /// <summary>
        /// Avvia un processo da un modello precedentemente pubblicato ed ne attende il completamento
        /// </summary>
        /// <param name="businessProcessId"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        [HttpPost("StartAndWait/{businessProcessId}")]
        public async Task<string> StartProcessAndWait(string businessProcessId, [FromBody] string variables)
        {
            //TODO: Verificare se l'utente ha il permesso di esecuzione
            return await engine.StartProcessAndWait(businessProcessId, variables);
        }


        /// <summary>
        /// Definisce il contesto del Modeler, Form Builder e DMN Builder
        /// </summary>
        /// <returns></returns>
        [HttpGet("AppContext")]
        public async Task<ActionResult<WorkflowEditorAppContext>> AppContext()
        {
            //TODO: Verificare se l'utente ha il permesso di gestire i task
            try
            {
                var u = userProfile.Get();
                u = UserProfile.SystemUser();
                WorkflowEditorAppContext TC = new WorkflowEditorAppContext();
                TC.UserId = u.userId;
                TC.Profile = ((int)(ProfileType.User)).ToString() + u.userId;

                TC.UserName = await userService.GetName(u.userId);

                TC.CanDeploy = await aclService.GetAuthorization("", u, PermissionType.WorkflowCanDeploy) == AuthorizationType.Granted;
                TC.CanCustomize = await aclService.GetAuthorization("", u, PermissionType.WorkflowCanCustomize) == AuthorizationType.Granted;

                //                TC.AdminServiceEndPoint = configuration["Endpoint:AdminService"];
                //                TC.SearchServiceEndPoint = configuration["Endpoint:SearchService"];
                //                TC.DocumentPreviewServiceEndPoint = configuration["Endpoint:DocumentPreviewService"];
                //                TC.DocumentServiceEndPoint = configuration["Endpoint:DocumentService"];
                //                TC.UISettingsServiceEndPoint = configuration["Endpoint:UISettingsService"];
                //                TC.UserServiceEndPoint = configuration["Endpoint:UserService"];
                //
                //                TC.CustomChangeItems = customTaskProvider.GetAlternativeElements();
                //                TC.CustomElementItems = customTaskProvider.GetElements(); ;
                TC.TaskGroups = customTaskProvider.GetPalette();

                // Associa ogni custom Task ad un elenco di suoi "sostituibili"
                //TC.CustomChangeItems = "";
                // Elenco di task custom selezionabili dal menu a tendina sul diagramma
                //TC.CustomElementItems = "";
                // Elenco di task custom inseribili nella palette
                //TC.CustomPaletteItems = "";

                return Ok(TC);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        /// <summary>
        /// Aggiunge un gruppo di task esterni attraverso l'interfacciamento di uno swagger
        /// </summary>
        /// <param name="swaggerURL"></param>
        /// <param name="swaggerContent"></param>
        /// <returns>La nuova palette di task</returns>
        [HttpPost("Palette/AddSwagger/{swaggerURL}")]
        public async Task<ActionResult<List<TaskGroup>>> AddSwagger(string swaggerURL, [FromBody] byte[] swaggerContent)
        {
            var u = userProfile.Get();
            if (await aclService.GetAuthorization("", u, PermissionType.WorkflowCanCustomize) != AuthorizationType.Granted)
                return BadRequest("non sei autorizzato a personalizzare la toolbar dei tasks");

            try
            {
                if (!customTaskProvider.ImportSwagger(swaggerURL, swaggerContent))
                    return BadRequest("Servizio già importato");

                List<TaskGroup> Palette = customTaskProvider.GetPalette() ;
                return Ok (Palette);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Rimuove un servizio dalla Palette dei Tasks
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns>La nuova palette di task</returns>
        [HttpDelete("Palette/{serviceId}")]
        public async Task<ActionResult<List<TaskGroup>>> DeleteTaskGroup(string serviceId)
        {
            var u = userProfile.Get();
            if (await aclService.GetAuthorization("", u, PermissionType.WorkflowCanCustomize) != AuthorizationType.Granted)
                return BadRequest("non sei autorizzato a personalizzare la toolbar dei tasks");

            try
            {
                customTaskProvider.Delete(serviceId);
                List<TaskGroup> Palette = customTaskProvider.GetPalette();
                return Ok(Palette);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Ripristina un servizio dalla Palette dei Tasks
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns>La nuova palette di task</returns>
        [HttpPut("Palette/{serviceId}/restore")]
        public async Task<ActionResult<List<TaskGroup>>> RestoreTaskGroup(string serviceId)
        {
            var u = userProfile.Get();
            if (await aclService.GetAuthorization("", u, PermissionType.WorkflowCanCustomize) != AuthorizationType.Granted)
                return BadRequest("Non sei autorizzato a personalizzare la toolbar dei tasks");

            try
            {
                customTaskProvider.Restore(serviceId);
                List<TaskGroup> Palette = customTaskProvider.GetPalette();
                return Ok(Palette);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        /// <summary>
        /// Aggiorna la definizione dei tasks di un servizio nella Palette dei Tasks
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns>La nuova palette di task</returns>
        [HttpPut("Palette/{serviceId}")]
        public async Task<ActionResult<List<TaskGroup>>> UpdateTaskGroup(string serviceId, [FromBody] string tasks)
        {
            var u = userProfile.Get();
            if (await aclService.GetAuthorization("", u, PermissionType.WorkflowCanCustomize) != AuthorizationType.Granted)
                return BadRequest("Non sei autorizzato a personalizzare la toolbar dei tasks");
            try
            {
                customTaskProvider.Update(serviceId, tasks);
                List<TaskGroup> Palette = customTaskProvider.GetPalette();
                return Ok(Palette);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        ///// <summary>
        ///// Riavvia il completamento di Task rimasti interrotti a partire da una data indicata
        ///// </summary>
        ///// <param name="FromDate">Data a partire dalla quale riavviare i processi</param>
        ///// <param name="UserTaskIds">Elenco (opzionale) di ID di UserTasks da riavviare</param>
        ///// <returns>La nuova palette di task</returns>
        //[HttpPut("RestartBrokenTasks/{FromDate}/{UserTaskIds}")]
        //public async Task RestartBrokenTasks (DateTime FromDate, string? UserTaskIds)
        //{
        //    var u = await userService.GetUserProfile(SpecialUser.SystemUser);
        //    List<UserTaskInfo> tlist = new();
        //    if (UserTaskIds == null) 
        //    {
        //        // riavvia in base all'elenco
        //        TaskListFilter filters = new TaskListFilter()
        //        {
        //            FromCreationDate = FromDate,
        //            Status = new List<ExecutionStatus>() { ExecutionStatus.Executed },
        //            PageSize = 0
        //        };
        //        var list = await userTaskService.Find(filters, u);
        //        foreach (var l in list)
        //        {
        //            var t = await userTaskService.GetById(l.Id, u);
        //            tlist.Add(t);
        //        }
        //    }
        //    else
        //    {
        //        foreach (var id in UserTaskIds.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        //        {
        //            int tid = 0;
        //            if (int.TryParse(id, out tid))
        //            {
        //                var t = await userTaskService.GetById(tid, u);
        //                tlist.Add(t);
        //            }
        //        }
        //    }
        //    foreach (var t in tlist)
        //    {
        //        var ProcessId = t.TaskItemInfo.ProcessId;
        //        var jobKey = t.TaskItemInfo.ExecutionId;
        //        var v = t.TaskItemInfo.Form.Data;
        //        var user = await userService.GetUserProfile(t.UserId);

        //        if (!string.IsNullOrEmpty(v) && v != "[]" && v != "{}")
        //        {
        //            var e = new TaskEventMessage(t.TaskItemInfo, user, EventType.TaskExecuted, v, t.TaskItemInfo.TaskType == TaskType.Event);
        //            try
        //            {
        //                await engine.SetVariables(ProcessId, v);
        //            }
        //            catch { };
        //            try
        //            {
        //                await engine.CompleteJob(jobKey, System.Text.Json.JsonSerializer.Serialize(e.Variables));
        //            }
        //            catch { };
        //        }
        //    }
        //}
    }



    // select Id, FromUserId, TaskType, Status, CreationDate, FormKey, ProcessDefinitionId, ProcessDefinitionKey, JobInstanceId, ProcessInstanceId from TaskItems  where CreationDate >= 20241024  and Status = 250 order by id desc
    //select Id, TaskItemId, Status, CreationDate from UserTasks where CreationDate >= 20241023  order by id desc
    //select u.Id, u.TaskItemId, u.Status, u.CreationDate, u.RoleId, u.GroupId, u.UserId, t.FormKey, t.EventId, t.ProcessDefinitionId, t.ProcessDefinitionKey, t.JobInstanceId, t.ProcessInstanceId from UserTasks u join TaskItems t on (u.taskitemid = t.id) where u.CreationDate >= 20241024  and u.status=250 order by id desc
    // select u.Id, u.TaskItemId, t.TaskType, u.Status, u.CreationDate, u.RoleId, u.GroupId, u.UserId, t.FormKey, t.EventId from UserTasks u join TaskItems t on (u.taskitemid = t.id) where u.CreationDate >= 20241024  and u.status=250 order by id desc

}
// update UserTasks set Status=0 Where Id in (select u.Id from UserTasks u join TaskItems t on (u.taskitemid = t.id) where t.TaskType=1 and u.CreationDate >= 20241024  and u.status=250 order by id desc