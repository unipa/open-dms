using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using OpenDMS.TaskList.API.DTOs;

namespace OpenDMS.TaskList.API.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/tasklist/")]
    public class TaskListController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IUserTaskService service;
        private readonly ICompanyService company;
        private readonly IACLService aclService;
        private readonly ILookupTableRepository lookup;
        private readonly ILoggedUserProfile userProfile;
        private readonly IUserService userContext;

        public TaskListController(
            IConfiguration configuration,
            IUserTaskService service,
            ICompanyService company,
            IACLService aclService,
            ILookupTableRepository lookup,
            ILoggedUserProfile userProfile,
            IUserService userContext)
        {
            this.configuration = configuration;
            this.service = service;
            this.company = company;
            this.aclService = aclService;
            this.lookup = lookup;
            this.userProfile = userProfile;
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
                TC.Groups = await userContext.GetGroups(u.userId);
                TC.Roles = await userContext.GetRoles(u.userId);
                TC.Categories = await lookup.GetAll(TaskConstants.CONST_TASK_GROUPS);
                TC.Priorities = await lookup.GetAll(TaskConstants.CONST_TASK_PRIORITIES);
                TC.Companies = (await company.GetByUser(u)).Where(c => !c.OffLine).Select(c => new LookupTable() { Id = c.Id.ToString(), Description = c.Description, Annotation = c.Logo }).ToList();

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
                return BadRequest(ex);
            }
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
                return BadRequest(ex);
            }
        }

        private async Task<List<SearchFiltersGroup>> _UserFilters(UserProfile u)
        {
            List<SearchFiltersGroup> Filters = new List<SearchFiltersGroup>()
            {
                new SearchFiltersGroup
                {
                    Name = "Attività",
                    Filters = new List<SearchFilters>
                    {
                        new SearchFilters()
                        {
                            Id = -1,
                            Icon = "icoTaskInbox",
                            SystemFilter = true,
                            UserId = u.userId,
                            Name = "In Carico",
                            Filters = new List<SearchFilter> {
                                new SearchFilter { ColumnName = TaskColumn.Direction, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { "0", u.userId } },
                                new SearchFilter { ColumnName = TaskColumn.TaskType, CustomTypeId="", Operator=OperatorType.NotEqualTo, Values = new List<string> { ((int)TaskType.Message).ToString() } },
                                new SearchFilter { ColumnName = TaskColumn.Status, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { ((int)ExecutionStatus.Assigned).ToString() } }
                            }
                        },
                        new SearchFilters()
                        {
                            Id = -2,
                            Icon = "icoTaskUnassigned",
                            SystemFilter = true,
                            UserId = u.userId,
                            Name = "Non Gestite",
                            Filters = new List<SearchFilter> {
                                new SearchFilter { ColumnName=TaskColumn.Direction, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { "0", u.userId } },
                                new SearchFilter { ColumnName=TaskColumn.TaskType, CustomTypeId="", Operator=OperatorType.NotEqualTo, Values = new List<string> { ((int)TaskType.Message).ToString() } },
                                new SearchFilter { ColumnName=TaskColumn.Status, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { ((int)ExecutionStatus.Unassigned).ToString() } }
                            }
                        },
                        new SearchFilters()
                        {
                            Id = -3,
                            Icon = "icoTaskExecuted",
                            SystemFilter = true,
                            UserId = u.userId,
                            Name = "Archiviate",
                            Filters = new List<SearchFilter> {
                                new SearchFilter { ColumnName=TaskColumn.Direction, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { "0", u.userId } },
                                new SearchFilter { ColumnName=TaskColumn.TaskType, CustomTypeId="", Operator=OperatorType.NotEqualTo, Values = new List<string> { ((int)TaskType.Message).ToString() } },
                                new SearchFilter { ColumnName=TaskColumn.Status, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { ((int)ExecutionStatus.Executed).ToString() } }
                            }
                        },
                        new SearchFilters()
                        {
                            Id = -4,
                            Icon = "icoTaskOutbox",
                            SystemFilter = true,
                            UserId = u.userId,
                            Name = "Assegnate",
                            Filters = new List<SearchFilter> {
                                new SearchFilter { ColumnName=TaskColumn.Direction, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { "1", u.userId } },
                                new SearchFilter { ColumnName=TaskColumn.TaskType, CustomTypeId="", Operator=OperatorType.NotEqualTo, Values = new List<string> { ((int)TaskType.Message).ToString() } }
                            }
                        }
                        //new SearchFilters()
                        //{
                        //    Id = -5,
                        //    Icon = "",
                        //    SystemFilter = true,
                        //    UserId = u.userId,
                        //    Name = "Attività Validate",
                        //    Filters = new List<SearchFilter> { }
                        //}
                        //new SearchFilters()
                        //{
                        //    Id = -6,
                        //    Icon = "icoTaskAll",
                        //    SystemFilter = true,
                        //    UserId = u.userId,
                        //    Name = "Tutte",
                        //    Filters = new List<SearchFilter> {
                        //        new SearchFilter { ColumnName="TaskType", CustomTypeId="", Operator=OperatorType.NotEqualTo, Values = new List<string> { ((int)TaskType.Message).ToString() } }
                        //    }
                        //}
                    }
                },
                new SearchFiltersGroup
                {
                    Name = "Messaggi",
                    Filters = new List<SearchFilters>
                    {
                        new SearchFilters()
                        {
                            Id = -10,
                            Icon = "icoMessageInbox",
                            SystemFilter = true,
                            UserId = u.userId,
                            Name = "Ricevuti",
                            Filters = new List<SearchFilter> {
                                new SearchFilter { ColumnName=TaskColumn.Direction, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { "0", u.userId } },
                                new SearchFilter { ColumnName=TaskColumn.TaskType, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { ((int)TaskType.Message).ToString() } }
                            }
                        },
                        new SearchFilters()
                        {
                            Id = -11,
                            Icon = "icoMessageOutbox",
                            SystemFilter = true,
                            UserId = u.userId,
                            Name = "Inviati",
                            Filters = new List<SearchFilter> {
                                new SearchFilter { ColumnName=TaskColumn.Direction, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { "1", u.userId } },
                                new SearchFilter { ColumnName=TaskColumn.TaskType, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { ((int)TaskType.Message).ToString() } }
                            }
                        },
                        new SearchFilters()
                        {
                            Id = -12,
                            Icon = "icoMessageArchive",
                            SystemFilter = true,
                            UserId = u.userId,
                            Name = "Archiviati",
                            Filters = new List<SearchFilter> {
                                new SearchFilter { ColumnName=TaskColumn.Direction, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { "0", u.userId } },
                                new SearchFilter { ColumnName=TaskColumn.TaskType, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { ((int)TaskType.Message).ToString() } },
                                new SearchFilter { ColumnName=TaskColumn.Status, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { ((int)ExecutionStatus.Executed).ToString() } }
                            }
                        }
                    }
                },
                new SearchFiltersGroup
                {
                    Name = "Segnalazioni",
                    Filters = new List<SearchFilters>
                    {
                        new SearchFilters()
                        {
                            Id = -10,
                            Icon = "icoWarningInbox",
                            SystemFilter = true,
                            UserId = u.userId,
                            Name = "Warning",
                            Filters = new List<SearchFilter> {
                                new SearchFilter { ColumnName=TaskColumn.Direction, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { "0", u.userId } },
                                new SearchFilter { ColumnName=TaskColumn.TaskType, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { ((int)TaskType.Warning).ToString() } }
                            }
                        },
                        new SearchFilters()
                        {
                            Id = -11,
                            Icon = "icoErrorInbox",
                            SystemFilter = true,
                            UserId = u.userId,
                            Name = "Errori",
                            Filters = new List<SearchFilter> {
                                new SearchFilter { ColumnName=TaskColumn.Direction, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { "0", u.userId } },
                                new SearchFilter { ColumnName=TaskColumn.TaskType, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { ((int)TaskType.Error).ToString() } }
                            }
                        }
                    }
                }
            };
            var F = await service.Filters(u);
            if (F.Count > 0)
                Filters.Add(
                new SearchFiltersGroup
                {
                    Name = "Altri Filtri",
                    Filters = F
                });
            foreach (var flist in Filters.Select(f => f.Filters.Where(f => f.SystemFilter)))
                foreach (var f in flist)
                    f.Badge = await service.Count(f.Filters, u);
            return Filters;
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
                return BadRequest(ex);
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
                return BadRequest(ex);
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
                return BadRequest(ex);
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
                return BadRequest(ex);
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
                return BadRequest(ex);
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
                return Ok(await service.GetById(UserTaskId, u));

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
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
                return BadRequest(ex);
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
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// L'utente loggato completa il task
        /// </summary>
        /// <param name="UserTaskId"></param>
        /// <param name="Variables">Elenco di variabili in formato json</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(UserTaskInfo))]
        [HttpPatch("Execute/{UserTaskId}/{Variables}")]
        public async Task<ActionResult<UserTaskInfo>> Execute(int UserTaskId, string Variables = "")
        {
            try
            {
                var u = userProfile.Get();
                return Ok(await service.Execute(UserTaskId, u, Variables));

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
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
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// L'utente loggato riassegna il task ad un nuovo profilo
        /// </summary>
        /// <param name="UserTaskId"></param>
        /// <param name="Profile"></param>
        /// <param name="Justification"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(UserTaskInfo))]
        [HttpPatch("Reassign/{UserTaskId}/{Profile}/{Justification}")]
        public async Task<ActionResult<UserTaskInfo>> Reassign(int UserTaskId, string Profile, string Justification)
        {
            try
            {
                string ProfileId = Profile.Substring(1);
                ProfileType ProfileType = (ProfileType)int.Parse(Profile.Substring(0, 1));
                var u = userProfile.Get();
                return Ok(await service.Reassign(UserTaskId, u, ProfileId, ProfileType, Justification));

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
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
                return BadRequest(ex);
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
                return BadRequest(ex);
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
        [HttpPost("Progress/{UserTaskId:int}/{Message}/{Percentage}")]
        public async Task<ActionResult<TaskProgressInfo>> AddProgressWithVariables(int UserTaskId, [FromBody] AddProgress_DTO ProgressData)
        {
            try
            {
                var u = userProfile.Get();
                return Ok(await service.AddProgress(UserTaskId, u.userId, ProgressData.message, ProgressData.percentage, ProgressData.variables));

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
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
                return BadRequest(ex);
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
