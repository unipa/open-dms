using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using Web.DTOs;

namespace OpenDMS.TaskList.API.Controllers
{

    [Authorize]
    [ApiController]
    [Route("internalapi/tasklist")]
    public class TaskListController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IUserTaskService service;
        private readonly ICompanyService company;
        private readonly ILookupTableRepository lookup;
        private readonly IACLService aclService;
        private readonly ILoggedUserProfile userProfile;
        private readonly IAppSettingsRepository appSettings;
        private readonly IUserService userContext;
        

        public TaskListController(
            IConfiguration configuration,
            IUserTaskService service,
            ICompanyService company,
            ILookupTableRepository lookup,
            IACLService aclService,
            ILoggedUserProfile userProfile,
            IAppSettingsRepository appSettings,
            IUserService userContext)
        {
            this.configuration = configuration;
            this.service = service;
            this.company = company;
            this.lookup = lookup;
            this.aclService = aclService;
            this.userProfile = userProfile;
            this.appSettings = appSettings;
            this.userContext = userContext;
        }
        //[HttpGet("ciao")]
        //public async Task<ActionResult> ciao()
        //{
        //    return Ok("ciao");
        //}

        /// <summary>
        /// Ritorna tutte le informazioni nencessarie per comporre la pagina di gestione dei task
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(TaskListContext))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // CONTEXT
        [HttpGet("AppContext")]
        public async Task<ActionResult<TaskListContext>> AppContext()
        {
            try
            {

                var u = userProfile.Get();
                //u = UserProfile.SystemUser();
                TaskListContext TC = new TaskListContext();
                TC.UserId = u.userId;
                TC.Profile = ((int)(ProfileType.User)).ToString() + u.userId;
                TC.UserName = await userContext.GetName(u.userId);
                TC.SearchFilters = await _UserFilters(u);
                TC.Groups = u.Groups; // await userContext.GetGroups(u.userId);
                TC.Roles = u.GlobalRoles;// await userContext.GetRoles(u.userId);
                TC.Categories = await lookup.GetAll(TaskConstants.CONST_TASK_GROUPS);
                TC.Priorities = await lookup.GetAll(TaskConstants.CONST_TASK_PRIORITIES);
                TC.Companies = u.Companies.Where(c => !c.OffLine).Select(c => new LookupTable() { Id = c.Id.ToString(), Description = c.Description, Annotation = c.Logo }).ToList(); // (await company.GetByUser(u)).Where(c => !c.OffLine).Select(c => new LookupTable() { Id = c.Id.ToString(), Description = c.Description, Annotation = c.Logo }).ToList();

                TC.CanFilterByUser = await aclService.GetAuthorization("", u, PermissionType.Task_CanViewUsers) == AuthorizationType.Granted;
                TC.CanCreateFiltersForGroups = true;
                TC.CanCreateFiltersForRoles = true;
                TC.CanCreateMessage = await aclService.GetAuthorization("", u, PermissionType.Task_CanCreateMessage) == AuthorizationType.Granted;
                TC.CanCreateTask = await aclService.GetAuthorization("", u, PermissionType.Task_CanCreateTask) == AuthorizationType.Granted;
                TC.CanCreateEventTask = false;// await authorizationService.HasPermission("Task:Create:Event", u);

                TC.SearchServiceEndPoint = configuration["Endpoint:SearchService"];
                TC.DocumentPreviewServiceEndPoint = configuration["Endpoint:DocumentPreviewService"];
                TC.DocumentServiceEndPoint = configuration["Endpoint:DocumentService"];
                TC.UISettingsServiceEndPoint = configuration["Endpoint:UISettingsService"];
                TC.UserServiceEndPoint = configuration["Endpoint:UserService"];

                TC.ExecutionStatus.Add(new LookupTable() { Id = ((int)ExecutionStatus.Unassigned).ToString(), Description = "Non Gestite" });
                TC.ExecutionStatus.Add(new LookupTable() { Id = ((int)ExecutionStatus.Assigned).ToString(), Description = "In Carico" });
                TC.ExecutionStatus.Add(new LookupTable() { Id = ((int)ExecutionStatus.Executed).ToString(), Description = "Archiviate" });

                //foreach (var e in Enum.GetValues(typeof(ExecutionStatus)))
                //{
                //    var status = new LookupTable() { Id = ((int)e).ToString(), Description = e.ToString() };
                //    TC.ExecutionStatus.Add(status);
                //}
                return Ok(TC);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("badge")]
        public async Task<int> Badge()
        {
            var u = userProfile.Get();
            int tot = 0;
            if (u != null)
                foreach (var f in (await _UserFilters(u)))
                {
                    tot += f.Filters.Sum(f => f.Badge);
                }
            return tot;
        }

        [HttpPatch("Attachments")]
        public async Task<ActionResult<string>> GetDocuments (List<int> TaskIdList)
        {
            var list = await service.GetDocuments(TaskIdList);
            return Ok(string.Join(",", list.Select(i=>i.ToString())));
        }



        // FILTRI

        /// <summary>
        /// Ritorna un elenco di filtri preimpostati per l'utente loggato
        /// </summary>
        /// <returns>Elenco di filtri per l'utente</returns>
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<SearchFiltersGroup>))]
        [HttpGet("Filters")]
        public async Task<ActionResult<List<SearchFiltersGroup>>> UserFilters()
        {
            try
            {
                var u = userProfile.Get();
                return Ok(await _UserFilters(u));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<List<SearchFiltersGroup>> _UserFilters(UserProfile u)
        {
            return await service.TaskListFilters(u);
        }


        /// <summary>
        /// Memorizza un filtro per l'utente loggato
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(SearchFilters))]
        [HttpPost("Filters")]
        public async Task<ActionResult<SearchFilters>> SaveFilter(SearchFilters filters)
        {
            var u = userProfile.Get();
            try
            {
                return Ok(await service.SaveFilter(filters));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Elimina un filtro preimpostato per l'utente loggato
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(int))]
        [HttpDelete("Filters/{filterId:int}")]
        public async Task<ActionResult<int>> RemoveFilter(int filterId)
        {
            try
            {
                var u = userProfile.Get();
                return Ok(await service.RemoveFilter(filterId, u));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // RICERCA

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<UserTaskListItem>))]
        [HttpPost("Find")]
        public async Task<ActionResult<List<UserTaskListItem>>> Find(Domain.Models.TaskListFilter filters)
        {
            try
            {
                var u = userProfile.Get();
                return Ok(await service.Find(filters, u));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    


        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(int))]
        [HttpGet("Count")]
        public async Task<ActionResult<int>> Count(Domain.Models.TaskListFilter filters)
        {
            try
            {
                var u = userProfile.Get();
                return Ok(await service.Count(filters, u));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GESTIONE TASK

        /// <summary>
        /// Crea un nuovo TaskItem. Se il Task è di tipo "Activity"
        /// </summary>
        /// <param name="newTask"></param>
        /// <returns>Task Principale</returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(TaskItemInfo))]
        [RequestSizeLimit(2_000_000_000)]
        [HttpPost]
        public async Task<ActionResult<TaskItemInfo>> CreateTask(CreateNewTask newTask)
        {
            if (newTask == null) return BadRequest("Parametro nullo o mal formattato");
            try
            {
                var u = userProfile.Get();
                return Ok(await service.CreateTask(newTask, u));
                 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Recupera le informazioni del TaskItem e dello UserTask relativo all'utente loggato
        /// </summary>
        /// <param name="UserTaskId"></param>
        /// <returns></returns>
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(UserTaskInfo))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{UserTaskId:int}")]
        public async Task<ActionResult<UserTaskInfo>> GetById(int UserTaskId)
        {
            try
            {
                var u = userProfile.Get();
                var T = await service.GetById(UserTaskId, u);
                //if (!T.Received)
                //{
                //    T.TaskItemInfo.Form.Schema = "";
                //    T.TaskItemInfo.Form.Data = "";
                //}
                return Ok(T);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// L'utente loggato prende in carico il task identificato da TaskItemId
        /// </summary>
        /// <param name="UserTaskId"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(UserTaskInfo))]
        [HttpPatch("Claim/{UserTaskId}")]
        public async Task<ActionResult<UserTaskInfo>> Take(int UserTaskId)
        {
            try
            {
                var u = userProfile.Get();
                return Ok(await service.Claim(UserTaskId, u));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// L'utente loggato rilascia il task precedentemente preso in carico
        /// </summary>
        /// <param name="UserTaskId"></param>
        /// <param name="Justification">Giustificazione del rilascio del task</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(UserTaskInfo))]
        [HttpPatch("Release/{UserTaskId}/{Justification}")]
        public async Task<ActionResult<UserTaskInfo>> Release(int UserTaskId, string Justification)
        {
            try
            {
                var u = userProfile.Get();
                return Ok(await service.Release(UserTaskId, u, Justification));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Elimina il task utente di un taskItem
        /// </summary>
        /// <param name="UserTaskId"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(int))]
        [HttpDelete("Delete/{UserTaskId}")]
        public async Task<ActionResult<int>> Delete(int UserTaskId)
        {
            try
            {
                var u = userProfile.Get();
                return Ok(await service.Delete(UserTaskId, u));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// L'utente loggato completa il task
        /// </summary>
        /// <param name="UserTaskId"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(UserTaskInfo))]
        [HttpPatch("MassiveExecute")]
        public async Task<ActionResult<UserTaskInfo>> MassiveExecute(int[] UserTaskIds)
        {
            try
            {
                var u = userProfile.Get();
                foreach (var t in UserTaskIds)
                {
                    await service.Execute(t, u, "{}");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// L'utente loggato completa il task
        /// </summary>
        /// <param name="UserTaskId"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(UserTaskInfo))]
        [HttpPatch("Execute/{UserTaskId}")]
        public async Task<ActionResult<UserTaskInfo>> Execute(int UserTaskId, [FromBody] string Variables = "{}")
        {
            try
            {
                var u = userProfile.Get();
                return Ok(await service.Execute(UserTaskId, u, Variables));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// L'utente loggato rifiuta il task riassegnandolo al "sender"
        /// </summary>
        /// <param name="UserTaskId"></param>
        /// <param name="Justification">Giustificazione del rifiuto</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(UserTaskInfo))]
        [HttpPatch("Reject/{UserTaskId}/{Justification}")]
        public async Task<ActionResult<UserTaskInfo>> Reject(int UserTaskId, string Justification)
        {
            try
            {
                var u = userProfile.Get();
                return Ok(await service.Reject(UserTaskId, u, Justification));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        /// <summary>
        /// L'utente loggato riassegna il task ad un nuovo profilo
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(UserTaskInfo))]
        [HttpPatch("Reassign")]
        public async Task<ActionResult<UserTaskInfo>> Reassign(TaskReassign_DTO taskInfo)
        {
            try
            {
                var u = userProfile.Get();
                string ProfileId = taskInfo.Profile.Substring(1);
                ProfileType ProfileType = (ProfileType)int.Parse(taskInfo.Profile.Substring(0, 1));
                return Ok(await service.Reassign(taskInfo.UserTaskId, u, ProfileId, ProfileType, taskInfo.Justification));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GESTIONE MESSAGGI

        /// <summary>
        /// Crea un nuovo task di tipo messaggio
        /// </summary>
        /// <param name="newTask">Oggetto task da creare</param>
        /// <returns>Task principale</returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(TaskItemInfo))]
        [HttpPost("Message")]
        public async Task<ActionResult<TaskItemInfo>> CreateMessage(CreateNewTaskMessage newTask)
        {
            if (newTask == null) return BadRequest("Parametro nullo o mal formattato");
            try
            {
                var u = userProfile.Get();
                return Ok(await service.CreateMessage(newTask, u));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(TaskItemInfo))]
        [HttpPost("checkRecipients")]
        public async Task<ActionResult<int>> CheckRecipients(CreateNewTaskMessage newTask)
        {
            if (newTask == null) return BadRequest("Parametro nullo o mal formattato");
            try
            {
                var u = userProfile.Get();
                var liv1 = int.Parse("0" + await appSettings.Get("Task.Recipients.Level1"));
                if (liv1 == 0)
                    liv1 = 20;
                int max = await service.GetUsersToNotify(newTask,u);
                if (await aclService.GetAuthorization("", u, "Task.CanAssignToAll.NoLimits") != AuthorizationType.Granted)
                {
                    var liv2 = int.Parse("0" + await appSettings.Get("Task.Recipients.Level2"));
                    if (liv2 == 0)
                        liv2 = 100;
                    // entro il primo livello è possibile assegnare
                    if (max <= liv1)
                        return 0;
                    // entro il secondo livello è possibile assegnare con un avviso
                    if (max <= liv2)
                        return max;
                    // qui non è possibile assegnare
                    return -1;
                }
                else {
                    // chiedo un avviso se supero il secondo livello 
                    if (max > liv1)
                        return max;
                    return 0;
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// L'utente loggato invia un commento a tutti i destinatari del task (se stesso escluso)
        /// </summary>
        /// <param name="UserTaskId"></param>
        /// <param name="Message"></param>
        /// <returns>Task Principale</returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(TaskItemInfo))]
        [HttpPost("Message/{UserTaskId}/{Message}")]
        public async Task<ActionResult<TaskItemInfo>> AddMessage(int UserTaskId, string Message)
        {
            try
            {
                var u = userProfile.Get();
                return Ok(await service.AddMessage(UserTaskId, u, Message));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GESTIONE PROGRESSI

        ///// <summary>
        ///// L'utente loggato inserisce un progresso nel task. 
        ///// La percentuale indicata va a modificare la percentuale dello UserTask e anche la percentuale del TaskItem.
        ///// La percentuale del TaskItem è la media delle percentuali di tutti gli UserTask
        ///// </summary>
        ///// <param name="UserTaskId"></param>
        ///// <param name="Message"></param>
        ///// <param name="Percentage"></param>
        ///// <returns></returns>
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(TaskProgressInfo))]
        //[HttpPost("Progress/{UserTaskId:int}/{Message}/{Percentage}")]
        //public async Task<ActionResult<TaskProgressInfo>> AddProgress(int UserTaskId, [FromQuery] string Message, [FromQuery] decimal Percentage)
        //{
        //    try
        //    {
        //        var u = userProfile.Get();
        //        return Ok(await service.AddProgress(UserTaskId, u.userId, Message, Percentage));

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}


        /// <summary>
        /// L'utente loggato inserisce un progresso nel task. 
        /// La percentuale indicata va a modificare la percentuale dello UserTask e anche la percentuale del TaskItem.
        /// La percentuale del TaskItem è la media delle percentuali di tutti gli UserTask
        /// </summary>
        /// <param name="UserTaskId"></param>
        /// <param name="ProgressData"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(TaskProgressInfo))]
        [HttpPost("Progress/{UserTaskId:int}")]
        public async Task<ActionResult<TaskProgressInfo>> AddProgressWithVariables(int UserTaskId,  AddProgress_DTO ProgressData)
        {
            try
            {
                var u = userProfile.Get();
                return Ok(await service.AddProgress(UserTaskId, u.userId, ProgressData.message, ProgressData.percentage, ProgressData.variables));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// L'utente loggato elimina un progresso (solo se è proprio o se l'utente è il sender del task o dei task padre.
        /// </summary>
        /// <param name="TaskProgressId"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(int))]
        [HttpDelete("Progress/{TaskProgressId:int}")]
        public async Task<ActionResult<int>> RemoveProgress(int TaskProgressId)
        {
            try
            {
                var u = userProfile.Get();
                return Ok(await service.RemoveProgress(TaskProgressId));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //TODO: pubblicare metodo
        //public async Task<List<TaskStatusSummary>> Status()
        //{
        //    List<TaskStatusSummary> items = new List<TaskStatusSummary>();
        //    return items;
        //}
    }
}
