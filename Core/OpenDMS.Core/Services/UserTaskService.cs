using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OpenDMS.Core.Builders;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Entities.Tasks;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
/// <summary>
/// Descrizione di riepilogo per BancheDatiProvider
/// </summary>
/// 
namespace OpenDMS.Core.BusinessLogic
{
    public class UserTaskService : IUserTaskService
    {
        private readonly ITaskRepository _repository;
        private readonly IDocumentService _documentService;
        private readonly IRoleService _roleService;
        private readonly IACLService aclService;
        private readonly IUserService userService;
        private readonly IMailboxService mailboxService;
        private readonly IUserFilterService _searchFilterService;
        private readonly INotificationService notificationService;
        private readonly IAppSettingsRepository appSettingsRepo;
        private readonly IMessageBuilder messageBuilder;
        private readonly ILookupTableService _lookService;
        private readonly IUserGroupRepository groupRepo;
        private readonly IViewServiceResolver serviceResolver;
        private readonly IOrganizationUnitService organizationUnitService;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IEventManager eventManager;
        private readonly ILogger<UserTaskService> logger;
        private readonly IAppSettingsRepository appSettings;
        private readonly ICompanyService companyService;
        private readonly IFormService formService;

        public UserTaskService(
            ITaskRepository Dao,
            IDocumentService docRepo,
            IRoleService roleRepo,
            IACLService aclService,
            IUserService userService,
            IMailboxService mailboxService,
            IUserFilterService filterRepo,
            INotificationService notificationService,
            IAppSettingsRepository appSettingsRepo,
            IMessageBuilder messageBuilder,
            ILookupTableService lookupRepo,
            IUserGroupRepository groupRepo,
            IViewServiceResolver serviceResolver,
            IOrganizationUnitService organizationUnitService,
            IOrganizationRepository userGroupRepo,
            IFormService formService,
            ILogger<UserTaskService> logger,
            IAppSettingsRepository appSettings,
            ICompanyService companyService,
            IEventManager eventManager = null
            )
        {
            this._repository = Dao;
            this._documentService = docRepo;
            this._roleService = roleRepo;
            this.aclService = aclService;
            this.userService = userService;
            this.mailboxService = mailboxService;
            this._searchFilterService = filterRepo;
            this.notificationService = notificationService;
            this.appSettingsRepo = appSettingsRepo;
            this.messageBuilder = messageBuilder;
            this._lookService = lookupRepo;
            this.groupRepo = groupRepo;
            this.serviceResolver = serviceResolver;
            this.organizationUnitService = organizationUnitService;
            this._organizationRepository = userGroupRepo;
            this.eventManager = eventManager;
            this.logger = logger;
            this.appSettings = appSettings;
            this.companyService = companyService;
            this.formService = formService;
        }


        public async Task<List<SearchFilters>> Filters(UserProfile userInfo)
        {
            return await _searchFilterService.GetAll(userInfo);
        }

        public async Task<SearchFilters> SaveFilter(SearchFilters filters)
        {
            var id = await _searchFilterService.Insert(filters);
            return id > 0 ? await _searchFilterService.GetById(id) : null;
        }

        public async Task<SearchFilters> RenameFilter(int filterId, string NewName, UserProfile userInfo)
        {
            var id = await _searchFilterService.Rename(filterId, NewName);
            return id > 0 ? await _searchFilterService.GetById(id) : null;
        }
        public async Task<int> RemoveFilter(int filterId, UserProfile userInfo)
        {
            return await _searchFilterService.Delete(filterId);
        }

        public async Task<List<int>> GetDocuments(List<int> TaskIdList)
        {
            List<int> docs = new();
            foreach (var tid in TaskIdList)
            {
                var TI = await _repository.GetUserTask(tid);
                var T = await _repository.GetById(TI.TaskItemId);
                if (T.Attachments != null)
                    foreach (var A in T.Attachments.Where(a => !a.IsLinked))
                    {
                        if (docs.IndexOf(A.DocumentId) < 0)
                            docs.Add(A.DocumentId);
                    }
            }
            return docs;
        }


        public async Task<List<UserTaskListItem>> Find(TaskListFilter filters, UserProfile userInfo)
        {
            List<UserTaskListItem> Items = new List<UserTaskListItem>();
            bool ok = false;
            ok = await CheckFilters(filters, userInfo, ok);
            if (!ok) return Items;
            foreach (var UT in await _repository.Find(filters, userInfo))
            {
                var TI = await _repository.GetById(UT.TaskItemId);
                UserTaskListItem Item = new UserTaskListItem();
                Item.Attachments = TI.Attachments.Select(a => a.DocumentId).ToList(); ;
                Item.Status = TI.Status;
                Item.Title = TI.Title;
                Item.CreationDate = TI.CreationDate;
                Item.Process.Id = TI.ProcessDefinitionKey;
                Item.Category.Id = TI.CategoryId;
                Item.Priority.Id = TI.PriorityId;
                Item.CompanyId = "";// TI.CompanyId;
                Item.TaskType = TI.TaskType;
                Item.Description = TI.Description;
                Item.Percentage = TI.ExecutionPercentage;
                Item.Sender = "0" + TI.FromUserId; // <-- necessario per avatar
                Item.SenderName = await userService.GetName(TI.FromUserId);
                Item.TaskItemId = TI.Id;
                var c = await _lookService.GetById(TaskConstants.CONST_TASK_GROUPS, TI.CategoryId);
                var pr = await _lookService.GetById(TaskConstants.CONST_TASK_PRIORITIES, TI.PriorityId);
                Item.Category.Description = c != null ? c.Description : TI.CategoryId;
                Item.Priority.Description = pr != null ? pr.Description : TI.PriorityId;

                Item.ToList = TI.Recipients.Where(r => !r.CC).Select(r => new LookupTable() { Id = ((Int32)r.ProfileType).ToString() + r.ProfileId, Description = GetName(r) }).ToList();
                Item.CCList = TI.Recipients.Where(r => r.CC).Select(r => new LookupTable() { Id = ((Int32)r.ProfileType).ToString() + r.ProfileId, Description = GetName(r) }).ToList();
                Item.Percentage = UT.Percentage;
                Item.IsCC = UT.CC;
                Item.ManagerId = UT.ManagerId;
                Item.RoleId = UT.RoleId;
                Item.GroupId = UT.GroupId;
                Item.UserId = UT.UserId;
                Item.ClaimDate = UT.ClaimDate;
                Item.CreationDate = UT.CreationDate;
                Item.NotificationDate = UT.NotificationDate;
                Item.ExpirationDate = UT.ExpirationDate;
                Item.ValidationDate = UT.ValidationDate;
                Item.Id = UT.Id;
                Items.Add(Item);

            }
            return Items;
        }

        public async Task<int> Count(List<SearchFilter> filters, UserProfile userInfo)
        {
            //            var view = await vewManager.Get("task", userInfo.userId);
            var service = await serviceResolver.GetSearchService("task");
            //TODO: convertire SearchFilter in TaskListFilter
            return await service.Count(filters, userInfo);
        }


        public async Task<int> Count(TaskListFilter filters, UserProfile userInfo)
        {
            bool ok = false;
            ok = await CheckFilters(filters, userInfo, ok);
            if (!ok) return 0;
            return await _repository.Count(filters, userInfo);
        }

        //TODO: Aggiungere metodo per creare un task da documento
        public async Task<TaskItemInfo> CreateTask(CreateNewTask newTask, UserProfile userInfo)//, string processJobId = "", string processId = "")
        {
            TaskItem TI = await CreateTaskItem(newTask, userInfo);
            if (!String.IsNullOrEmpty(newTask.CategoryId))
                TI.CategoryId = newTask.CategoryId;

            TI.NotifyExpiration = newTask.NotifyExpiration;
            if (!newTask.ExpirationDate.HasValue && (newTask.TaskType == TaskType.Activity || newTask.TaskType == TaskType.Event || newTask.TaskType == TaskType.Error))
            {
                // Verifico se c'è una impostazione predefinita sui settings
                var gg = int.Parse("0" + await appSettings.Get("Tasks.DefaultExpiration"));
                if (gg > 0)
                {
                    newTask.ExpirationDate = newTask.StartDate.HasValue ? newTask.StartDate.Value.AddDays(gg) : DateTime.UtcNow.AddDays(gg);
                };
            };
            TI.ExpirationDate = newTask.ExpirationDate;
            TI.NotifyExecution = newTask.NotifyExecution;
            TI.TaskType = newTask.TaskType;
            TI.EventId = newTask.EventId;

            if (newTask.IncludeParentAttachments && newTask.ParentTaskId > 0)
            {
                var PTI = await _repository.GetUserTask(newTask.ParentTaskId);
                var PI = await _repository.GetById(PTI.TaskItemId);
                foreach (var a in PI.Attachments)
                {
                    TI.Attachments.Add(new TaskAttachment() { DocumentId = a.DocumentId, IsLinked = a.IsLinked });
                }
            }


            TI.FormKey = newTask.FormKey;
            TI.Variables = newTask.FormData;
            foreach (var attachment in newTask.NewContent)
            {
                var D = new CreateOrUpdateDocument();
                D.CompanyId = newTask.CompanyId;
                D.Description = attachment.FileName;
                D.DocumentDate = DateTime.Now;
                D.ContentType = ContentType.Document;
                var DI = await _documentService.Create(D, userInfo);
                var I = await _documentService.AddContent(DI, userInfo, attachment);
                TI.Attachments.Add(new TaskAttachment() { DocumentId = DI });
            }
            List<TaskRecipient> UsersToNotify = await CreateUserTasksAndGetUsersToNotify(newTask, TI, userInfo);
            var r = await _repository.Insert(TI);
            if (r <= 0)
            {
                logger.LogError($"CreateTask: Si è verificato un errore durante la memorizzazione del task #{TI.Id}");
                return null;
            }
            var taskItemInfo = await GetByTaskId(TI.Id, userInfo);
            // Attribuisce i permessi ai recipients
            await SendNotification(taskItemInfo, userInfo);
            if (eventManager != null)
            {
                foreach (var d in TI.Attachments)
                {
                    var Doc = await _documentService.Load(d.DocumentId, userInfo);
                    await eventManager.Publish(new DocumentEventMessage(Doc, userInfo, EventType.Share, new Dictionary<string, object>() { { "Task", taskItemInfo } }));
                }
            }
            return taskItemInfo;
        }

        public async Task<TaskItemInfo> CreateMessage(CreateNewTaskMessage newTask, UserProfile userInfo)//, string processJobId = "", string processId = "")
        {
            TaskItem TI = await CreateTaskItem(newTask, userInfo);//, processJobId, processId);
            List<TaskRecipient> UsersToNotify = await CreateUserTasksAndGetUsersToNotify(newTask, TI, userInfo);

            await _repository.Insert(TI);
            // Attribuisce i permessi ai recipients
            var UT = await GetByTaskId(TI.Id, userInfo);
            await SendNotification(UT, userInfo);
            return UT;
        }
        public async Task<int> DeleteByDocument (int documentId)
        {
            return await _repository.DeleteByDocument(documentId);
        }
        public async Task<TaskItemInfo> CreateError(CreateNewTaskMessage newTask, UserProfile userInfo)//, string processJobId = "", string processId = "")
        {
            TaskItem TI = await CreateTaskItem(newTask, userInfo);//, processJobId, processId);
            TI.TaskType = TaskType.Error;
            List<TaskRecipient> UsersToNotify = await CreateUserTasksAndGetUsersToNotify(newTask, TI, userInfo);

            await _repository.Insert(TI);
            // Attribuisce i permessi ai recipients
            var UT = await GetByTaskId(TI.Id, userInfo);
            await SendNotification(UT, userInfo);
            //if (eventManager != null) await eventManager.Publish(new TaskEventMessage(UT, userInfo, EventType.TaskCreated, TI.Variables));
            return UT;
        }
        public async Task<TaskItemInfo> CreateWarning(CreateNewTaskMessage newTask, UserProfile userInfo)//, string processJobId = "", string processId = "")
        {
            TaskItem TI = await CreateTaskItem(newTask, userInfo);//, processJobId, processId);
            TI.TaskType = TaskType.Warning;
            List<TaskRecipient> UsersToNotify = await CreateUserTasksAndGetUsersToNotify(newTask, TI, userInfo);

            await _repository.Insert(TI);
            // Attribuisce i permessi ai recipients
            var UT = await GetByTaskId(TI.Id, userInfo);
            await SendNotification(UT, userInfo);
            return UT;
        }

        public async Task<int> Delete (int userTaskId, UserProfile userInfo)
        {
            var TI = await _repository.GetUserTask(userTaskId);

            var children = (await _repository.GetByParentId(userTaskId)).Count();
            if (children > 0) return -2;

            if (TI.TaskItem.FromUserId == userInfo.userId)
                return await _repository.Delete(userTaskId);
            else
                return -1;
        }
        public async Task<TaskItemInfo> GetByTaskId(int TaskItemId, UserProfile userInfo)
        {
            var TI = await _repository.GetById(TaskItemId);
            TaskItemInfo TaskItemInfo = new TaskItemInfo();
            foreach (var id in TI.Attachments?.Where(a => a.DocumentId > 0).Select(a => a.DocumentId))
            {
                var doc = await _documentService.Load(id, userInfo);
                TaskItemInfo.Attachments.Add(doc); 
            }
            TaskItemInfo.Status = TI.Status;
            TaskItemInfo.Title = TI.Title;
            TaskItemInfo.CreationDate = TI.CreationDate;
            TaskItemInfo.StartDate = TI.StartDate;
            TaskItemInfo.EndDate = TI.EndDate;
            TaskItemInfo.Duration = TI.Duration;
            TaskItemInfo.Process.Id = TI.ProcessDefinitionKey;
            TaskItemInfo.ProcessId = TI.ProcessInstanceId;
            TaskItemInfo.ProcessDataId = TI.ProcessDataId;
            if (!String.IsNullOrEmpty(TI.ProcessDefinitionKey))
            {
                var processId = TI.ProcessDefinitionId;
                var process = await _documentService.Get(processId);
                var image = await _documentService.GetContentInfo(TI.ProcessImageId);
                TaskItemInfo.Process.Description = process.Description;
                TaskItemInfo.Process.Annotation = image.VersionNumber.ToString() + "." + image.RevisionNumber.ToString("00");
                TaskItemInfo.Process.TableId = TI.ProcessInstanceId.ToLower();
            }
            TaskItemInfo.Category.Id = TI.CategoryId;
            TaskItemInfo.Priority.Id = TI.PriorityId;

            TaskItemInfo.CompanyId = TI.CompanyId;
            TaskItemInfo.Company.Id = TI.CompanyId.ToString();
            var company = await companyService.GetById(TI.CompanyId);
            if (company != null)
            {
                TaskItemInfo.Company.Description = company.Description;
                TaskItemInfo.Company.TableId = company.Logo;
            }

            TaskItemInfo.Event.Id = TI.EventId;
            TaskItemInfo.TaskType = TI.TaskType;
            TaskItemInfo.Description = TI.Description;
            TaskItemInfo.Percentage = TI.ExecutionPercentage;
            TaskItemInfo.Sender = "0" + TI.FromUserId; // <-- necessario per avatar
            TaskItemInfo.SenderName = await userService.GetName(TI.FromUserId);
            TaskItemInfo.Id = TI.Id;
            TaskItemInfo.Template = TI.MessageTemplate;
            TaskItemInfo.ParentId = TI.ParentId;
            if (TI.ParentId > 0)
            {
                var PTI = await _repository.GetUserTask(TI.ParentId);
                var PI = await _repository.GetById(PTI.TaskItemId);
                TaskItemInfo.ParentTitle = PI.Title;
                TaskItemInfo.ParentType = PI.TaskType;
            }
            var c = await _lookService.GetById(TaskConstants.CONST_TASK_GROUPS, TI.CategoryId);
            var pr = await _lookService.GetById(TaskConstants.CONST_TASK_PRIORITIES, TI.PriorityId);
            var ev = await _lookService.GetById("$EVENTS$", TI.EventId);
            TaskItemInfo.Category.Description = c != null ? c.Description : TI.CategoryId;
            TaskItemInfo.Priority.Description = pr != null ? pr.Description : TI.PriorityId;
            TaskItemInfo.Event.Description = ev != null ? ev.Description : TI.EventId;

            foreach (var r in TI.Recipients)
            {
                try
                {
                    var L = new LookupTable() { Id = ((Int32)r.ProfileType).ToString() + r.ProfileId, Description = GetName(r) };
                    if (r.CC)
                        TaskItemInfo.CCList.Add(L);
                    else
                        TaskItemInfo.ToList.Add(L);
                }
                catch (Exception)
                {
                }
            }
            foreach (var p in await _repository.GetProgress(TaskItemInfo.Id))
            {
                TaskProgressInfo TP = new();
                TP.Message = p.Message;
                TP.Id = p.Id;
                TP.UserId = "0" + p.UserId; // <-- necssario per l'avatar
                TP.UserName = await userService.GetName(p.UserId);
                TP.Percentage = p.Percentage;
                TP.UserTaskId = p.UserTaskId.Value;
                TP.TaskItemId = TaskItemInfo.Id;
                TP.CreationDate = p.CreationDate;
                TaskItemInfo.Progress.Add(TP);
            }
            try
            {
                FormSchema FormInfo = await formService.GetByTask(TI, userInfo);
                //if (FormInfo != null)
                //{
                //    FormInfo.Schema = ParseSettings(FormInfo.Schema).Parse(TI, "Task");
                //}
                //else FormInfo = new FormSchema();
                TaskItemInfo.Form = FormInfo;
            }
            catch (Exception ex)
            {
                FormSchema FormInfo = new FormSchema();
                FormInfo.Schema = "<h1>" + ex.Message + "</h1>";
                TaskItemInfo.Form = FormInfo;

            }
            TaskItemInfo.ExecutionId = TI.JobInstanceId;
            TaskItemInfo.Form.Data = TI.Variables;
            return TaskItemInfo;
        }

        private async Task<UserTaskInfo> MapUserTaskInfo(UserTask UT, TaskItemInfo TI, UserProfile userInfo)
        {
            if (UT == null) return null;
            UserTaskInfo T = new UserTaskInfo();
            T.TaskItemInfo = TI;
            if (!String.IsNullOrEmpty(UT.Variables))
                T.TaskItemInfo.Form.Data = UT.Variables;
            T.Percentage = UT.Percentage;
            T.IsCC = UT.CC;
            T.ManagerId = UT.ManagerId;
            T.Status = UT.Status;
            T.UserName = await userService.GetName(UT.UserId);
            T.RoleId = UT.RoleId;
            T.GroupId = UT.GroupId;
            T.GroupName = string.IsNullOrEmpty(UT.GroupId) ? "" : (await organizationUnitService.GetGroup(UT.GroupId)).Name;
            T.RoleName = string.IsNullOrEmpty(UT.RoleId) ? "" : (await _roleService.GetById(UT.RoleId)).RoleName;
            T.UserId = UT.UserId;
            T.ClaimDate = UT.ClaimDate;
            T.CreationDate = UT.CreationDate;
            T.NotificationDate = UT.NotificationDate;
            T.ExpirationDate = UT.ExpirationDate;
            T.ValidationDate = UT.ValidationDate;
            T.FirstExecutionDate = UT.FirstExecutionDate;
            T.LastExecutionDate = UT.LastExecutionDate;
            T.Id = UT.Id;
            T.Received = userInfo.userId.Equals(UT.UserId, StringComparison.InvariantCultureIgnoreCase)
                        || (userInfo.Roles.Any(s => s.Id.Equals(T.RoleId + "\\" + T.GroupId, StringComparison.InvariantCultureIgnoreCase)) && !String.IsNullOrEmpty(T.RoleId) && !String.IsNullOrEmpty(T.GroupId))
                        || (userInfo.GlobalRoles.Any(s => s.Id.Equals(T.RoleId, StringComparison.InvariantCultureIgnoreCase)) && !String.IsNullOrEmpty(T.RoleId) && String.IsNullOrEmpty(T.GroupId))
                        || (userInfo.Groups.Any(s => s.Id.Equals(T.GroupId, StringComparison.InvariantCultureIgnoreCase)) && !String.IsNullOrEmpty(T.GroupId) && String.IsNullOrEmpty(T.RoleId));

            return T;
        }


        public async Task<UserTaskInfo> GetById(int UserTaskId, UserProfile userInfo)
        {
            UserTask UT = await _repository.GetUserTask(UserTaskId);
            if (UT == null) return null;
            var TI = await GetByTaskId(UT.TaskItemId, userInfo); 
            var UTI = await MapUserTaskInfo(UT, TI, userInfo);

            foreach (var userTask in await _repository.GetByParentId(UserTaskId))
            {
                var PTI = await GetByTaskId(userTask.TaskItemId, userInfo);
                var PUT = await MapUserTaskInfo(userTask, PTI, userInfo);
                UTI.SubTasks.Add(PUT);
            }
            return UTI;
        }

        public async Task<UserTaskInfo> Claim(int UserTaskId, UserProfile user)
        {
            var UT = await _repository.GetUserTask(UserTaskId);
            if (UT == null) return null;
            // if (UT.ClaimDate.HasValue) return null;
            if (UT.Status != ExecutionStatus.Unassigned) return null;
            UT.TaskItem = await _repository.GetById(UT.TaskItemId);
            UT.UserId = user.userId;
            UT.ClaimDate = DateTime.UtcNow;
            UT.Status = ExecutionStatus.Assigned;
            await _repository.UpdateUserTask(UT);
            await AddProgress(UT.Id, user.userId, "Presa in carico");
            var taskItem = await GetById(UserTaskId, user);
            if (eventManager != null) await eventManager.Publish(new UserTaskEventMessage(taskItem, user, EventType.UserTaskClaimed, UT.Variables));
            return taskItem;
        }


        public async Task<UserTaskInfo> Release(int UserTaskId, UserProfile userInfo, string Justification)
        {
            var UT = await _repository.GetUserTask(UserTaskId);
            if (UT == null) return null;
            if (UT.ClaimDate is null) return null;
            if (UT.Status != ExecutionStatus.Assigned) return null;

            if (!String.IsNullOrEmpty(UT.GroupId) || !String.IsNullOrEmpty(UT.RoleId)) UT.UserId = "";
            UT.ClaimDate = null;
            UT.TaskItem = await _repository.GetById(UT.TaskItemId);
            UT.Status = ExecutionStatus.Unassigned;
            await _repository.UpdateUserTask(UT);
            await AddProgress(UT.Id, userInfo.userId, "Rilascio attività: " + Justification);
            var taskItem = await GetById(UserTaskId, userInfo);
            if (eventManager != null) await eventManager.Publish(new UserTaskEventMessage(taskItem, userInfo, EventType.UserTaskReleased, UT.Variables));
            return taskItem;
        }


        public async Task<UserTaskInfo> Execute(int UserTaskId, UserProfile user, string variables = "{}")
        {
            var UT = await _repository.GetUserTask(UserTaskId);
            if (UT == null) throw new InvalidDataException($"Task #{UserTaskId} non trovato");
            if (UT.Status == ExecutionStatus.Executed) throw new Exception("Il task è già stato completato."); ;
            if (UT.Status == ExecutionStatus.Unassigned)
            {
                if (await Claim(UT.Id, user) == null) throw new Exception("Non è stato possibile prendere in carico il task");
                UT.Status = ExecutionStatus.Assigned;
                UT.ClaimDate = DateTime.UtcNow;
            }
            if (UT.Status != ExecutionStatus.Assigned) throw new Exception("Il task non è stato preso in carico."); ;
            var TI = await _repository.GetById(UT.TaskItemId);
            UT.TaskItem = TI;
            UT.Variables = variables;

            if (UT.ClaimDate is null)
                UT.ClaimDate = DateTime.UtcNow;
            if (UT.FirstExecutionDate is null)
                UT.FirstExecutionDate = DateTime.UtcNow;
            UT.UserId = user.userId;
            UT.LastExecutionDate = DateTime.UtcNow;
            UT.Percentage = 100;
            UT.Status = ExecutionStatus.Executed;
            if (!String.IsNullOrWhiteSpace(TI.ProcessInstanceId))
            {
                UT.ManagerId = SpecialUser.SystemUser;
                UT.ValidationDate = DateTime.UtcNow;
            }
            await _repository.UpdateUserTask(UT);


            string Justification = "";
            string esito = "";

            try
            {
                var json = JObject.Parse(variables);

                int[] attachments = json.ContainsKey("attachments") ? json.GetValue("attachments")?.ToObject<int[]>() : null;
                if (attachments != null && TI.ProcessDataId > 0)
                {
                    // carico gli allegati nei documenti del tasks
                    foreach (var attachmentId in attachments)
                    {
                        await _documentService.AddLink(TI.ProcessDataId, attachmentId, user, false);
                    }
                }

//                Justification = json.ContainsKey("justification") ? json["justification"]?.ToString() : "";
                Justification = JToken.Parse(variables)["Justification"]?.ToString() ?? "";
                if (String.IsNullOrEmpty(Justification))
                    Justification = JToken.Parse(variables)["justification"]?.ToString() ?? "";

                if (json.ContainsKey("ExitCode"))
                {
                    esito = json["ExitCode"]?.ToString() ?? "";
                }
                if (json.ContainsKey("ApprovalStatus") && string.IsNullOrEmpty(esito))
                {
                    esito = json["ApprovalStatus"]?.ToString() ?? "";
                    json["ExitCode"] = esito;
                }
                if (json.ContainsKey("choise") && string.IsNullOrEmpty(esito))
                {
                    esito = json["choise"]?.ToString() ?? "";
                    json["ExitCode"] = esito;
                }
                if (json.ContainsKey("Choise") && string.IsNullOrEmpty(esito))
                {
                    esito = json["Choise"]?.ToString() ?? "";
                    json["ExitCode"] = esito;
                }

                if (string.IsNullOrEmpty(Justification))
                {
                    if (TI.EventId == EventType.Approval)
                    {
                        if (esito == "1")
                            Justification = "Autorizzato";
                        else
                            Justification = "Non Autorizzato";
                    }
                }
                json["Justification"] = Justification;
                variables = json.ToString();
            }
            catch (Exception ex)
            {
                logger.LogError("JSON Parsing", ex);
            }

            if (String.IsNullOrWhiteSpace(Justification))
                Justification = "Completato";

            await AddProgress(UT.Id, user.userId, Justification, 100);

            // Genero Evento di Esecuzione attività
            var taskItem = await GetById(UserTaskId, user);
            if (eventManager != null) await eventManager.Publish(new UserTaskEventMessage(taskItem, user, EventType.UserTaskExecuted, variables));

            // Verifico se posso chiudere il task padre
            var c = (UT.TaskItem.TaskType == TaskType.Activity || UT.TaskItem.TaskType == TaskType.Event)
                ? await _repository.CountActiveUserTasks(UT.TaskItemId)
                : 0;
            // se non ci sono altri usertask assegnati, aperti o sospesi 
            if (c == 0)
            {
                TI.Status = ExecutionStatus.Executed;
                await _repository.Update(TI);
                taskItem = await GetById(UserTaskId, user);
                if (eventManager != null) await eventManager.Publish(new TaskEventMessage(taskItem.TaskItemInfo, user, EventType.TaskExecuted, variables));
                if (TI.NotifyExecution && user.userId != TI.FromUserId)
                {
                    // Creo una notifica di tipo messaggio al richiedente
                    await SendFeedback(UT, TI, Justification, user);
                }
            }
            else
            {
                if (TI.Status != ExecutionStatus.Running)
                {
                    TI.Status = ExecutionStatus.Running;
                    await _repository.Update(TI);
                    taskItem = await GetById(UserTaskId, user);
                    //if (eventManager != null) await eventManager.Publish(new UserTaskEventMessage(taskItem, user, EventType.TaskRunning, variables));
                }

            }

            return taskItem;
        }


        private async Task SendFeedback(UserTask UT, TaskItem TI, string justification, UserProfile user)
        {
            TaskItem NTI = new TaskItem();
            NTI.CategoryId = "";
            NTI.NotifyExpiration = false;
            NTI.CompanyId = TI.CompanyId;
            NTI.ParentId = UT.Id;
            NTI.Description = justification;
            NTI.Title = "Attività completata - " + TI.Title;
            NTI.ExpirationDate = null;
            NTI.FromUserId = UT.UserId;
            NTI.NotifyExecution = false;
            NTI.NotifyExpiration = false;
            NTI.Recipients = new List<TaskRecipient>();
            NTI.UserTasks = new List<UserTask>();
            NTI.Attachments = new List<TaskAttachment>();
            NTI.TaskType = TaskType.Message;
            NTI.ProcessDataId = TI.ProcessDataId;
            NTI.ProjectId = TI.ProjectId;
            NTI.Recipients.Add(new TaskRecipient() { ProfileId = TI.FromUserId, CC = true, ProfileType = ProfileType.User });

            await _repository.Insert(NTI);
            var NUT = await GetByTaskId(NTI.Id, user);
            await SendNotification(NUT, user);

        }



        public async Task<UserTaskInfo> Validate(int UserTaskId, UserProfile manager, string Justification = "")
        {
            var UT = await _repository.GetUserTask(UserTaskId);
            if (UT == null) return null;
            if (UT.Status != ExecutionStatus.Executed) return null;
            var TI = await _repository.GetById(UT.TaskItemId);

            UT.ManagerId = manager.userId;
            UT.ValidationDate = DateTime.UtcNow;
            UT.LastExecutionDate = DateTime.UtcNow;
            UT.Status = ExecutionStatus.Validated;

            await _repository.UpdateUserTask(UT);
            if (!String.IsNullOrWhiteSpace(Justification)) await AddProgress(UT.Id, manager.userId, Justification);
            //TODO: Aggiornare il processo collegato
            // Chiudere gli altri Task
            //
            return await GetById(UserTaskId, manager);
        }
        /// <returns></returns>
        public async Task<UserTaskInfo> Reject(int UserTaskId, UserProfile user, string Justification)
        {
            var UT = await _repository.GetUserTask(UserTaskId);
            if (UT == null) return null;
            if (UT.Status != ExecutionStatus.Unassigned) return null;
            var TI = await _repository.GetById(UT.TaskItemId);

            UT.UserId = TI.FromUserId;
            UT.ClaimDate = null;


            UT.Status = ExecutionStatus.Unassigned;
            UT.LastExecutionDate = DateTime.UtcNow;
            await _repository.UpdateUserTask(UT);
            await AddProgress(UT.Id, user.userId, Justification);
            var taskItem = await GetById(UserTaskId, user);

            if (eventManager != null) await eventManager.Publish(new UserTaskEventMessage(taskItem, user, EventType.UserTaskRejected, ""));

            return taskItem;
        }


        public async Task<UserTaskInfo> Reassign(int UserTaskId, UserProfile user, string ProfileId, ProfileType ProfileType, string Justification)
        {
            var UT = await _repository.GetUserTask(UserTaskId);
            if (UT == null) return null;
            if (UT.Status != ExecutionStatus.Unassigned) return null;
            var TI = await _repository.GetById(UT.TaskItemId);
            var profiles = ProfileId.Split("\\");
            var profile1 = profiles[0];
            var profile2 = profiles.Length > 1 ? profiles[1] : "";
            UT.GroupId = ProfileType == ProfileType.Group ? profile1 :  profile2;
            UT.RoleId = ProfileType == ProfileType.Role ? profile1 : "";
            UT.UserId = ProfileType == ProfileType.User ? profile1 : "";
            UT.Status = ExecutionStatus.Unassigned;
            UT.TaskItem = TI;
            UT.ClaimDate = null;
            UT.LastExecutionDate = DateTime.UtcNow;
            var destinatario =  await userService.GetName(UT.UserId);
            await _repository.UpdateUserTask(UT);
            await _documentService.SetPermissions(TI.Attachments.Select(a => a.DocumentId).ToList(), user, ProfileId, ProfileType, new() { { PermissionType.CanView, AuthorizationType.Granted } ,  { PermissionType.CanViewContent, AuthorizationType.Granted } } );
            await AddProgress(UT.Id, user.userId, "Riassegnazione a "+ destinatario +": " + Justification.Replace("<p>", "<span>").Replace("</p>", "</span>"));
            var taskItem = await GetById(UserTaskId, user);

            if (eventManager != null) await eventManager.Publish(new UserTaskEventMessage(taskItem, user, EventType.UserTaskReassigned, ""));

            return taskItem;
        }


        public async Task<TaskItemInfo> AddMessage(int UserTaskId, UserProfile userInfo, string Message)
        {

            var UT = await _repository.GetUserTask(UserTaskId);
            if (UT == null) return null;
            var TI = await _repository.GetById(UT.TaskItemId);

            CreateNewTask M = new CreateNewTask();

            M.TaskType = TaskType.Message;
            M.Sender = "0" + userInfo.userId;
            M.ProjectId = TI.ProjectId;
            M.CompanyId = TI.CompanyId;
            M.Description = Message;
            //TODO: Legare uno usertask ad un altro user task
            //M.ParentTaskId = UT.Id; // TI.Id;
            M.ParentTaskId = TI.Id;
            M.Title = Message;
            // Notifico a tutti tranne che a me
            //M.NotifyCC.AddRange(TI.Recipients.Where(r => r.CC && !(r.ProfileId == userInfo.userId && r.ProfileType == ProfileType.User)).Select(r => ((int)r.ProfileType).ToString() + r.ProfileId).ToList());
            M.NotifyTo.Add(((int)ProfileType.User).ToString() + TI.FromUserId);

            //UserTask UT2 = new UserTask();
            //UT2.ExpirationDate = TI.ExpirationDate;

            //UT2.GroupId = "";
            //UT2.RoleId = "";
            //UT2.UserId = UT.UserId;
            //UT2.Variables = TI.Variables;
            //UT2.TaskItemId = TI.Id;
            //UT2.CC = true;
            //TI.UserTasks.Add(UT2); 
            //List<TaskRecipient> UserRecipients = new List<TaskRecipient>() { new TaskRecipient() { CC=true, ProfileId=UT.UserId, ProfileType=ProfileType.User  } }
            //await SendNotification(TI, UserRecipients, userInfo)

            return await CreateTask(M, userInfo);
        }


        //public async Task<TaskProgressInfo> AddProgress(int UserTaskId, string userId, string Message, decimal Percentage = -1, string variables = null)
        //{
        //    var UT = await _repository.GetUserTask(UserTaskId);
        //    if (UT == null) return null;
        //    if (variables != null)
        //    {
        //        UT.Variables = variables;
        //        await _repository.UpdateUserTask(UT);
        //    }
        //    TaskProgress TP = new TaskProgress();
        //    TP.Message = Message;
        //    TP.UserId = userId;
        //    TP.Percentage = Percentage;
        //    TP.UserTaskId = UserTaskId;
        //    TP.CreationDate = DateTime.UtcNow;
        //    TP.TaskItemId = UT.TaskItemId;
        //    await _repository.AddProgress(TP);

        //    TaskProgressInfo P = new TaskProgressInfo();
        //    P.Message = Message;
        //    P.UserId = userId;
        //    P.UserName = await userService.GetName(userId);
        //    P.UserTaskId = UserTaskId;
        //    P.UserTaskId = UT.TaskItemId;
        //    P.Percentage = Percentage;
        //    P.CreationDate = DateTime.UtcNow;
        //    P.Id = TP.Id;

        //    return P;
        //}


        public async Task<TaskProgressInfo> AddProgress(int UserTaskId, string userId, string Message, decimal Percentage = -1, string variables = null)
        {
            var UT = await _repository.GetUserTask(UserTaskId);
            if (UT == null) return null;
            if (variables != null)
            {
                string Justification = "";
                try
                {
                    Justification = JToken.Parse(variables)["Justification"]?.ToString() ?? "";
                    if (String.IsNullOrEmpty(Justification))
                        Justification = JToken.Parse(variables)["justification"]?.ToString() ?? "";
                }
                catch (Exception)
                {
                }
                if (String.IsNullOrWhiteSpace(Justification) && Percentage == 100)
                    Justification = "Completato";

                if (string.IsNullOrEmpty(Message))
                    Message = Justification;
                UT.Variables = variables;
                await _repository.UpdateUserTask(UT);
            }
            TaskProgress TP = new TaskProgress();
            TP.Message = Message;
            TP.UserId = userId;
            TP.UserTaskId = UserTaskId;
            TP.Percentage = Percentage;
            TP.CreationDate = DateTime.UtcNow;
            TP.TaskItemId = UT.TaskItemId;
            await _repository.AddProgress(TP);

            TaskProgressInfo P = new TaskProgressInfo();
            P.Message = Message;
            P.UserId = userId;
            P.UserName = await userService.GetName(userId);
            P.UserTaskId = UserTaskId;
            P.TaskItemId = UT.TaskItemId;
            P.Percentage = Percentage;
            P.CreationDate = DateTime.UtcNow;
            P.Id = TP.Id;

            return P;
        }


        public async Task<int> RemoveProgress(int TaskProgressId)
        {
            return await _repository.RemoveProgress(TaskProgressId);
        }


        public async Task<int> GetByJobKey(string JobInstanceId)
        {
            return await _repository.GetByJobInstanceId(JobInstanceId);
        }


        public async Task RemoveOrphanTasks(DateTime LastUpdate)
        {
            await _repository.RemoveOrphanTasks(LastUpdate);
        }
        public async Task<int> Refresh(int TaskItemId)
        {
            return await _repository.Refresh(TaskItemId);
        }
        public async Task<int> CompleteByTaskId(int TaskItemId, string Message = "")
        {
            return await _repository.CompleteByTaskId(TaskItemId, Message);
        }
        public async Task<List<UserTask>> GetByEvent(int documentId, string EventId, UserProfile userInfo)
        {
            return await _repository.GetByEvent(documentId, EventId, userInfo);
        }
        public async Task<List<UserTask>> GetByDocument(int documentId, UserProfile userInfo)
        {
            return await _repository.GetByDocument(documentId, userInfo);
        }
        public async Task<List<UserTask>> GetByProcessId(string processId, UserProfile userInfo)
        {
            return await _repository.GetByProcessId(processId, userInfo);
        }

        public async Task<List<UserTask>> GetAllDocumentTasks(int documentId, UserProfile userInfo)
        {
            return await _repository.GetAllDocumentTasks(documentId, userInfo);
        }
        public async Task<int> CaptureEvent(UserTask UT, IEvent Event)
        {
            int r = 0;
            if (UT == null) throw new InvalidDataException($"Task nullo");
            //DocumentEventMessage e = (DocumentEventMessage)Event;
            var TI = await _repository.GetById(UT.TaskItemId);
            UT.TaskItem = TI;
            if (UT.Status == ExecutionStatus.Unassigned)
            {
                await Claim(UT.Id, Event.UserInfo);
                UT.Status = ExecutionStatus.Assigned;
                UT.ClaimDate = DateTime.UtcNow;
            }
            string variables = UT.Variables;
            var json = JObject.Parse(variables);
            var eventVariables = Event.Variables;
            // aggiorno le variabili del task con quelle dell'evento
            foreach (var kv in eventVariables)
            {
                json[kv.Key] = System.Text.Json.JsonSerializer.Serialize( kv.Value);
            }
            string Justification = "";
            try
            {
                //Justification = JToken.Parse(variables)["justification"]?.ToString() ?? "";
                Justification = json["Justification"]?.ToString() ?? "";
                if (String.IsNullOrEmpty(Justification))
                    Justification = json["justification"]?.ToString() ?? "";
            }
            catch (Exception)
            {
            }
            if (String.IsNullOrWhiteSpace(Justification) && UT.TaskItem.TaskType != TaskType.Message)
                Justification = "Attività Eseguita";

            if (UT.FirstExecutionDate is null)
                UT.FirstExecutionDate = DateTime.UtcNow;
            UT.UserId = Event.UserInfo.userId;
            UT.LastExecutionDate = DateTime.UtcNow;
            UT.Status = ExecutionStatus.Executed;
            //UT.Status = !String.IsNullOrWhiteSpace(TI.ProcessId) ? ExecutionStatus.Validated : ExecutionStatus.Executed;
            UT.Percentage = 100;
            if (!String.IsNullOrWhiteSpace(TI.ProcessInstanceId))
            {
                UT.ManagerId = SpecialUser.SystemUser;
                UT.ValidationDate = DateTime.UtcNow;
            }
            r = await _repository.UpdateUserTask(UT);
            await AddProgress(UT.Id, Event.UserInfo.userId, Justification, 100);
            var taskItem = await GetById(UT.Id, Event.UserInfo);

            if (eventManager != null) await eventManager.Publish(new UserTaskEventMessage(taskItem, Event.UserInfo, EventType.UserTaskExecuted, variables, true));

            // Verifico se posso chiudere il task padre
            var c = (UT.TaskItem.TaskType == TaskType.Activity || UT.TaskItem.TaskType == TaskType.Event)
                ? await _repository.CountActiveUserTasks(UT.TaskItemId)
                : 0;
            // se non ci sono altri usertask assegnati, aperti o sospesi 
            if (c == 0)
            {
                TI.Status = ExecutionStatus.Executed;
                await _repository.Update(TI);
                taskItem = await GetById(UT.Id, Event.UserInfo);

                if (eventManager != null) await eventManager.Publish(new TaskEventMessage(taskItem.TaskItemInfo, Event.UserInfo, EventType.TaskExecuted, variables, true));
                if (TI.NotifyExecution && Event.UserInfo.userId != TI.FromUserId && TI.FromUserId != SpecialUser.SystemUser)
                {
                    // Creo una notifica di tipo messaggio al richiedente
                    await SendFeedback(UT, TI, Justification, Event.UserInfo);
                }
            }
            else
            {
                if (TI.Status != ExecutionStatus.Running)
                {
                    TI.Status = ExecutionStatus.Running;
                    await _repository.Update(TI);
                    taskItem = await GetById(UT.Id, Event.UserInfo);

                    if (eventManager != null) await eventManager.Publish(new UserTaskEventMessage(taskItem, Event.UserInfo, EventType.TaskRunning, UT.Variables, true));
                }
            }
            return r;
        }




        public async Task<int> UpdateTaskVariables (UserTask UT, string EventVariables)
        {
            int r = 0;
            if (UT == null) throw new InvalidDataException($"Task nullo");
            //DocumentEventMessage e = (DocumentEventMessage)Event;
            var TI = await _repository.GetById(UT.TaskItemId);
            UT.TaskItem = TI;
            string variables = UT.Variables;
            var json = JObject.Parse(variables);
            var eventVariables = JObject.Parse(EventVariables);
            // aggiorno le variabili del task con quelle dell'evento
            foreach (var k in eventVariables)
            {
                json[k.Key] = k.Value;
            }
            UT.Variables = json.ToString();
            r = await _repository.UpdateUserTask(UT);
            return r;
        }




        public async Task<List<SearchFiltersGroup>> TaskListFilters(UserProfile u)
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
                            Id = -2,
                            Icon = "fa fa-clock-o",//"icoTaskUnassigned",
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
                            Id = -1,
                            Icon = "fa fa-inbox",//"icoTaskInbox",
                            SystemFilter = true,
                            UserId = u.userId,
                            Name = "In Carico",
                            Filters = new List<SearchFilter> {
                                new SearchFilter { ColumnName=TaskColumn.Direction, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { "0", u.userId } },
                                new SearchFilter { ColumnName=TaskColumn.TaskType, CustomTypeId="", Operator=OperatorType.NotEqualTo, Values = new List<string> { ((int)TaskType.Message).ToString() } },
                                new SearchFilter { ColumnName=TaskColumn.Status, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { ((int)ExecutionStatus.Assigned).ToString() } }
                            }
                        },

                        new SearchFilters()
                        {
                            Id = -5,
                            Icon = "fa fa-calendar",//"icoTaskExecuted",
                            SystemFilter = true,
                            UserId = u.userId,
                            Name = "Scadute",
                            Filters = new List<SearchFilter> {
                                new SearchFilter { ColumnName=TaskColumn.Direction, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { "0", u.userId } },
                                new SearchFilter { ColumnName=TaskColumn.ExpirationDate, CustomTypeId="", Operator=OperatorType.LessThanOrEqualTo, Values = new List<string> { DateTime.UtcNow.ToString("yyyyMMdd") } },
                                new SearchFilter { ColumnName=TaskColumn.Status, CustomTypeId="", Operator=OperatorType.In, Values = new List<string> { ((int)ExecutionStatus.Unassigned).ToString(), ((int)ExecutionStatus.Assigned).ToString() } }
                            }
                        },
                        new SearchFilters()
                        {
                            Id = -3,
                            Icon = "fa fa-check",//"icoTaskExecuted",
                            SystemFilter = true,
                            UserId = u.userId,
                            Name = "Completate",
                            Filters = new List<SearchFilter> {
                                new SearchFilter { ColumnName=TaskColumn.Direction, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { "0", u.userId } },
                                new SearchFilter { ColumnName=TaskColumn.TaskType, CustomTypeId="", Operator=OperatorType.NotEqualTo, Values = new List<string> { ((int)TaskType.Message).ToString() } },
                                new SearchFilter { ColumnName=TaskColumn.Status, CustomTypeId="", Operator=OperatorType.In, Values = new List<string> { ((int)ExecutionStatus.Executed).ToString(), ((int)ExecutionStatus.Validated).ToString() } }
                            }
                        },
                        new SearchFilters()
                        {
                            Id = -4,
                            Icon = "fa fa-send",//"icoTaskOutbox",
                            SystemFilter = true,
                            UserId = u.userId,
                            Name = "Assegnate",
                            Filters = new List<SearchFilter> {
                                new SearchFilter { ColumnName=TaskColumn.Direction, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { "1", u.userId } },
                                new SearchFilter { ColumnName=TaskColumn.TaskType, CustomTypeId="", Operator=OperatorType.NotEqualTo, Values = new List<string> { ((int)TaskType.Message).ToString() } },
                                new SearchFilter { ColumnName=TaskColumn.Status, CustomTypeId="", Operator=OperatorType.In, Values = new List<string> { ((int)ExecutionStatus.Unassigned).ToString(), ((int)ExecutionStatus.Assigned).ToString() } },
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
                            Icon = "fa fa-inbox",// "icoMessageInbox",
                            SystemFilter = true,
                            UserId = u.userId,
                            Name = "Ricevuti",
                            Filters = new List<SearchFilter> {
                                new SearchFilter { ColumnName=TaskColumn.Direction, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { "0", u.userId } },
                                new SearchFilter { ColumnName=TaskColumn.Status, CustomTypeId="", Operator=OperatorType.In, Values = new List<string> { ((int)ExecutionStatus.Unassigned).ToString(), ((int)ExecutionStatus.Assigned).ToString() } },
                                new SearchFilter { ColumnName=TaskColumn.TaskType, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { ((int)TaskType.Message).ToString() } }
                            }
                        },
                        new SearchFilters()
                        {
                            Id = -11,
                            Icon = "fa fa-send",//"icoMessageOutbox",
                            SystemFilter = true,
                            UserId = u.userId,
                            Name = "Inviati",
                            Filters = new List<SearchFilter> {
                                new SearchFilter { ColumnName=TaskColumn.Direction, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { "1", u.userId } },
                                new SearchFilter { ColumnName=TaskColumn.TaskType, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { ((int)TaskType.Message).ToString() } },
                                new SearchFilter { ColumnName=TaskColumn.Status, CustomTypeId="", Operator=OperatorType.In, Values = new List<string> { ((int)ExecutionStatus.Unassigned).ToString(), ((int)ExecutionStatus.Assigned).ToString() } },
                            }
                        },
                        new SearchFilters()
                        {
                            Id = -12,
                            Icon = "fa fa-folder",//"icoMessageArchive",
                            SystemFilter = true,
                            UserId = u.userId,
                            Name = "Archiviati",
                            Filters = new List<SearchFilter> {
                                new SearchFilter { ColumnName=TaskColumn.Direction, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { "0", u.userId } },
                                new SearchFilter { ColumnName=TaskColumn.TaskType, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { ((int)TaskType.Message).ToString() } },
                                new SearchFilter { ColumnName=TaskColumn.Status, CustomTypeId="", Operator=OperatorType.In, Values = new List<string> { ((int)ExecutionStatus.Executed).ToString(), ((int)ExecutionStatus.Validated).ToString() } }
                            }
                        }
                    }
                }
            };

            var F = await this.Filters(u);
            if (F.Count > 0)
            {
                var FilterGroups = F.Select(f => (f.Name + "@").Split("@")[1]).Distinct().ToList();
                foreach (var g in FilterGroups)
                {
                    Filters.Add(
                    new SearchFiltersGroup
                    {
                        Name = string.IsNullOrEmpty(g) ? "Altri Filtri" : g,
                        Filters = F.Where(f => (f.Name + "@").Split("@")[1].Equals(g, StringComparison.InvariantCultureIgnoreCase)).Select(f => { f.Name = f.Name.Split("@")[0]; return f; }).ToList()
                    });
                }
                //Filters.Add(
                //new SearchFiltersGroup
                //{
                //    Name = "Altri Filtri",
                //    Filters = F
                //});
            };

            Filters.Add(
            new SearchFiltersGroup
            {
                Name = "Segnalazioni",
                Filters = new List<SearchFilters>
                {
                    new SearchFilters()
                    {
                        Id = -13,
                        Icon = "fa fa-warning",//"icoWarningInbox",
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
                        Id = -14,
                        Icon = "fa fa-bug",//"icoErrorInbox",
                        SystemFilter = true,
                        UserId = u.userId,
                        Name = "Errori",
                        Filters = new List<SearchFilter> {
                            new SearchFilter { ColumnName=TaskColumn.Direction, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { "0", u.userId } },
                            new SearchFilter { ColumnName=TaskColumn.TaskType, CustomTypeId="", Operator=OperatorType.EqualTo, Values = new List<string> { ((int)TaskType.Error).ToString() } }
                        }
                    }
                }
            });
            foreach (var flist in Filters.Select(f => f.Filters.Where(f => f.SystemFilter)))
                foreach (var f in flist)
                {
                    if (
                        !f.Filters.Any(f => f.ColumnName == TaskColumn.Status && f.Values.Contains(((int)ExecutionStatus.Executed).ToString()))
                        &&
                        f.Filters.Any(f => f.ColumnName == TaskColumn.Direction && f.Values.Contains("0"))
                        &&
                        !f.Filters.Any(f => f.ColumnName == TaskColumn.ExpirationDate)
                    )
                        f.Badge = await Count(f.Filters, u);
                }
            return Filters;
        }



        private async Task<TaskItem> CreateTaskItem(CreateNewTaskMessage newTask, UserProfile userInfo)//, string processJobId = "", string processId = "")
        {
            TaskItem TI = new TaskItem();
            TI.CategoryId = "";
            TI.PriorityId = newTask.Priority;
            TI.NotifyExpiration = false;
            TI.CompanyId = newTask.CompanyId;
            if (TI.CompanyId <= 0) TI.CompanyId = 1;

            TI.ParentId = newTask.ParentTaskId;
            TI.MessageTemplate = newTask.MessageTemplate;
            // Verifico se ho indicato un template
            try
            {
                if (!String.IsNullOrEmpty(newTask.MessageTemplate))
                {
                    User From = await userService.GetById(newTask.Sender);

                    var template = await GetTemplate(newTask.MessageTemplate, newTask.CompanyId);
                    template = template
                        .UpdateTags("LoggedUser", userInfo)
                        .UpdateTags("Sender", From.Contact);

                    if (newTask.Attachments.Count > 0)
                    {
                        List<DocumentInfo> documents = new List<DocumentInfo>();
                        foreach (var a in newTask.Attachments.Take(5))
                        {
                            var d = await _documentService.Load(a, userInfo);
                            if (d != null)
                                documents.Add(d);
                        }
                        template = template
                            .UpdateTags("Document", documents[0])
                            .UpdateTags("Documents", documents);
                    }
                    var t = template.Build(newTask.Title, newTask.Description);
                    newTask.Description = t.Body;
                    newTask.Title = t.Title;
                }
            } catch (Exception ex){
                logger.LogError(ex, "CreateTaskItem-> Message Template");
            } 
            //TODO: Gestire template e segnaposto
            TI.Description = newTask.Description;
            TI.Title = newTask.Title;
            TI.ExpirationDate = null;
            TI.FromUserId = string.IsNullOrEmpty(newTask.Sender) ? userInfo.userId : newTask.Sender; 
            TI.NotifyExecution = false;
            TI.NotifyExpiration = false;
            TI.ProcessInstanceId = newTask.ProcessInstanceId;
            TI.ProcessDataId = newTask.ProcessDataId;
            TI.JobInstanceId = newTask.JobInstanceId;
            TI.ProcessDefinitionKey = newTask.ProcessDefinitionKey;
            var processId = String.IsNullOrWhiteSpace(newTask.ProcessDefinitionKey) ? 0 : await _documentService.FindByUniqueId(null, newTask.ProcessDefinitionKey, ContentType.Workflow);
            if (processId > 0)
            {
                var process = await _documentService.Get(processId);
                if (process != null && process.Id > 0)
                {
                    TI.ProcessDefinitionId = processId;
                    var Published = (await _documentService.GetPublished(processId));
                    TI.ProcessImageId = Published == null ? 0 : Published.ImageId;
                    TI.CategoryId = process.DocumentType?.CategoryId;
                }
            }
            TI.StartDate = newTask.StartDate ?? DateTime.UtcNow;
            TI.EndDate = newTask.EndDate ??  DateTime.UtcNow;
            if (TI.StartDate > TI.CreationDate) TI.CreationDate = TI.StartDate;
            if (TI.EndDate < TI.StartDate ) TI.EndDate = TI.StartDate;

            TI.Recipients = new List<TaskRecipient>();
            TI.UserTasks = new List<UserTask>();
            TI.Attachments = new List<TaskAttachment>();
            TI.TaskType = TaskType.Message;
            TI.ProjectId = newTask.ProjectId;
            TI.FormKey = "";

            foreach (var attachment in newTask.Attachments)
            {
                TI.Attachments.Add(new TaskAttachment() { DocumentId = attachment });
            }
            foreach (var link in newTask.Links)
            {
                TI.Attachments.Add(new TaskAttachment() { DocumentId = link, IsLinked = true });
            }
            return TI;
        }


        private async Task<MessageBuilder> GetTemplate(string TemplateId, int CompanyId, bool CC = false)
        {
            var template = TemplateId;
            if (string.IsNullOrEmpty(template))
                template = NotificationConstants.CONST_TEMPLATE_NOTIFY;

            var t_title = await appSettingsRepo.Get(CompanyId, TemplateId + (CC ? ".CC" : "") + ".Title");
            var t_body = await appSettingsRepo.Get(CompanyId, TemplateId + (CC ? ".CC" : "") + ".Body");

            var MessageBuilder = messageBuilder
                .SetTitle(t_title)
                .SetBody(t_body);

            return MessageBuilder;
        }


        private async Task SendNotification(TaskItemInfo TI, UserProfile userProfile)
        {
            try
            {
                var FromUser = TI.Sender.Substring(1);
                var SystemUserMailBoxId = await userService.GetAttribute(SpecialUser.SystemUser, UserAttributes.CONST_NOTIFICATION_MAIL, TI.CompanyId);

                if (string.IsNullOrEmpty(SystemUserMailBoxId) || !int.TryParse(SystemUserMailBoxId, out int SenderMailBoxId) || SenderMailBoxId <= 0) 
                {
                    logger.LogWarning($"SendNotification: Impossibile notificare il task #{TI.Id} perchè l'utente {TI.SenderName} non ha una MailBox associata.");
                    return;
                }

                var SenderMailbox = await mailboxService.GetById(SenderMailBoxId);
                var FromName = TI.SenderName;
                var FromAddress = SenderMailbox.MailAddress;
                if (string.IsNullOrEmpty(FromAddress))
                {
                    logger.LogWarning($"SendNotification: Impossibile notificare il task #{TI.Id} perchè l'utente {TI.SenderName} non ha un indirizzo mail associato.");
                    return;
                }

                List<DocumentInfo> documents = TI.Attachments.Take(3).ToList();

                var template = TI.Template;
                if (string.IsNullOrEmpty(template))
                    template = NotificationConstants.CONST_TEMPLATE_NOTIFY;
                int mailSent = 0;
                User From = await userService.GetById(FromUser);

                foreach (var userTask in await _repository.GetUserTasks(TI.Id))
                {
                    var ptype = !string.IsNullOrEmpty(userTask.UserId)
                        ? ProfileType.User
                        : !string.IsNullOrEmpty(userTask.RoleId)
                            ? ProfileType.Role
                            : ProfileType.Group;
                    var profileId = !string.IsNullOrEmpty(userTask.UserId)
                        ? userTask.UserId
                        : !string.IsNullOrEmpty(userTask.RoleId)
                            ? !string.IsNullOrEmpty(userTask.GroupId)
                                ? userTask.RoleId+"\\"+ userTask.GroupId
                                : userTask.RoleId
                            : userTask.GroupId;

                    List<TaskRecipient> UserRecipients = new List<TaskRecipient>();
                    switch (ptype)
                    {
                        case ProfileType.User:
                            if (UserRecipients.Find(r => r.ProfileId == profileId && r.ProfileType == ProfileType.User) == null)
                            {
                                TaskRecipient r = new TaskRecipient() { ProfileId = profileId, ProfileType = ProfileType.User, CC = userTask.CC };
                                UserRecipients.Add(r);
                            }
                            break;
                        case ProfileType.Group:
                            // Aggiungo gli utenti dei gruppi abilitati all'inbox  
                            var users = await _organizationRepository.GetUsersInGroup(profileId);
                            var ugroup = (await _organizationRepository.GetById(0, profileId));
                            if (ugroup != null)
                            {
                                var group = ugroup.UserGroup;
                                if (group != null)
                                {
                                    var permissionid = userTask.CC ? PermissionType.CanViewInBoxCC : PermissionType.CanViewInBox;
                                    var permission = AuthorizationType.None;

                                    /// 1. Utenti abilitati al permesso "ViewInbox"
                                    /// 2. Utenti con ruolo specifico (NotificationProfile)
                                    /// 3. Tutti gli utenti del gruppo
                                    /// 4. Tutti gli utenti e sotto-strutture
                                    var strategy = userTask.CC ? group.NotificationStrategyCC : group.NotificationStrategy;
                                    switch (strategy)
                                    {
                                        case 1: permission = await aclService.GetAuthorization("", profileId, ProfileType.Group, permissionid); break;
                                        case 2: users = users.Where(u => u.RoleId == group.NotificationProfile).ToList(); permission = AuthorizationType.Granted; break;
                                        case 4: //TODO: Smistare ai sottogruppi se Strategy = 4
                                            foreach (var g in await organizationUnitService.GetOrganizationTree(0, profileId))
                                                users.AddRange(await _organizationRepository.GetUsersInGroup(g.Info.UserGroupId));
                                            break;
                                        default:
                                            permission = AuthorizationType.Granted; break;
                                    }
                                    var gfound = false;
                                    foreach (var u in users)
                                    {
                                        var upermission = permission;
                                        if (strategy == 1)
                                        {
                                            if (upermission == AuthorizationType.None)
                                                upermission = await aclService.GetAuthorization("", u.RoleId + "\\" + u.UserGroupId, ProfileType.Role, permissionid);
                                            if (upermission == AuthorizationType.None)
                                                upermission = await aclService.GetAuthorization("", u.UserId, ProfileType.User, permissionid);
                                        }
                                        if (upermission == AuthorizationType.Granted)
                                        {
                                            if (UserRecipients.FirstOrDefault(r => r.ProfileId == u.UserId && r.ProfileType == ProfileType.User) == null)
                                            {
                                                TaskRecipient tr = new TaskRecipient() { ProfileId = u.UserId, ProfileType = ProfileType.User, CC = userTask.CC };
                                                gfound = true;
                                                UserRecipients.Add(tr);
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case ProfileType.Role:
                            // Aggiungo gli utenti dei gruppi abilitati all'inbox o gli utenti dei ruoli 
                            List<string> usersr = null;
                            var RoleId = profileId;
                            var GroupId = "";
                            int i = profileId.IndexOf("\\");
                            if (i >= 0)
                            {
                                RoleId = profileId.Substring(0, i);
                                GroupId = profileId.Substring(i + 1);
                            }

                            if (String.IsNullOrEmpty(GroupId))
                                usersr = (await _organizationRepository.GetUsersInRole(RoleId));
                            else
                                usersr = (await _organizationRepository.GetUsersInGroup(GroupId)).Where(u => u.RoleId.Equals(RoleId, StringComparison.InvariantCultureIgnoreCase)).Select(u => u.UserId).ToList();
                            if (usersr != null)
                                foreach (var u in usersr.Distinct())
                                {
                                    if (UserRecipients.Find(r => r.ProfileId.Equals(u, StringComparison.InvariantCultureIgnoreCase) && r.ProfileType == ProfileType.User) == null)
                                    {
                                        TaskRecipient tr = new TaskRecipient() { ProfileId = u, ProfileType = ProfileType.User, CC = userTask.CC };
                                        UserRecipients.Add(tr);
                                    }
                                }
                            break;
                        default:
                            break;
                    }

                    foreach (var r in UserRecipients)
                    {
                        var notificationType = (await userService.GetAttribute(r.ProfileId, UserAttributes.CONST_NOTIFICATION_TYPE, TI.CompanyId))?.ToLower();
                        if (notificationType != "no")
                        {
                            if (string.IsNullOrEmpty(notificationType))
                                notificationType = (await userService.GetAttribute(SpecialUser.SystemUser, UserAttributes.CONST_NOTIFICATION_TYPE, TI.CompanyId))?.ToLower();

                            if (string.IsNullOrEmpty(notificationType))
                                notificationType = (await appSettings.Get(TI.CompanyId, UserAttributes.CONST_NOTIFICATION_TYPE));

                            if (notificationType != "no")
                            {
                                var mailboxId = await userService.GetAttribute(r.ProfileId, UserAttributes.CONST_NOTIFICATION_PREFIX + notificationType, TI.CompanyId);

                                var ToMailAddress = "";
                                if (int.TryParse(mailboxId, out int ToMailBoxId) && ToMailBoxId > 0)
                                {
                                    var ToMailBox = await mailboxService.GetById(ToMailBoxId);
                                    if (ToMailBox != null) ToMailAddress = ToMailBox.MailAddress;
                                }
                                if (string.IsNullOrEmpty(ToMailAddress))
                                {
                                    var mail = (await userService.GetById(r.ProfileId)).Email;
                                    if (!string.IsNullOrEmpty(mail))
                                    {
                                        ToMailAddress = mail;
                                    }
                                    else
                                    {
                                        // FIX per Inviare una mail a chi non si è mail loggato
                                        var contacts = await userService.GetAllContactDigitalAddress(r.ProfileId) ?? new();
                                        var address = contacts.FirstOrDefault(c => c.DigitalAddressType == DigitalAddressType.Email && !c.Deleted);
                                        if (address != null)
                                        {
                                            ToMailAddress = address.Address;
                                            try
                                            {
                                                var user = (await userService.GetById(r.ProfileId));
                                                user.Email = address.Address;
                                                await userService.AddOrUpdate(user);
                                            }
                                            catch (Exception)
                                            {
                                            }
                                        }
                                    }
                                }

                                if (!string.IsNullOrEmpty(ToMailAddress))
                                {
                                    mailSent += 1;
                                    User U = await userService.GetById(r.ProfileId);
                                    var MessageBuilder = await GetTemplate(template, TI.CompanyId, r.CC);
                                    MessageBuilder = MessageBuilder
                                        //                                .UpdateTags("Task", TI)
                                        .UpdateTags("Task", userTask)
                                        .UpdateTags("User", U.Contact)
                                        .UpdateTags("Sender", From.Contact)
                                        .UpdateTags("UserProfile", userProfile);
                                    if (documents.Count > 0)
                                    {
                                        MessageBuilder = MessageBuilder
                                            .UpdateTags("Document", documents[0])
                                            .UpdateTags("Documents", documents);
                                    }
                                    var Template = MessageBuilder.Build(TI.Title, TI.Description);

                                    CreateOrUpdateMailMessage N = new CreateOrUpdateMailMessage();
                                    N.FromUser = FromName + "<" + FromAddress + ">";
                                    N.MailboxId = SenderMailBoxId;
                                    N.Body = Template.Body;
                                    N.Subject = Template.Title;
                                    N.Attachments = TI.Attachments.Select(t => t.Id).ToList();
                                    N.LinkAttachments = true;
                                    N.Status = MailStatus.Sending;

                                    //var ToMailBox = await mailboxService.GetById(ToMailBoxId);
                                    N.To.Add(ToMailAddress);
                                    try
                                    {
                                        if (notificationService != null) await notificationService.SendMail(N);
                                    }
                                    catch (Exception ex)
                                    {
                                        logger.LogError(ex, $"SendNotification->SendMail({N.FromUser},{ToMailAddress})");
                                    }
                                }
                                else
                                    logger.LogWarning($"SendNotification: Impossibile notificare il task #{TI.Id} all'utente '{r.ProfileId}' perchè non ha un indirizzo di posta assegnato");
                            }
                            else
                                logger.LogWarning($"SendNotification: Impossibile notificare il task #{TI.Id} all'utente '{r.ProfileId}' a causa delle impostazionie globali");
                        }
                        else
                        {
                            logger.LogWarning($"SendNotification: Impossibile notificare il task #{TI.Id} all'utente '{r.ProfileId}' perchè non ha una modalità di notifica attiva");

                        }
                    }
                }
            } catch (Exception ex)
            {
                logger.LogError(ex, "SendNotification");
            }
        }

        public async Task<int> GetUsersToNotify(CreateNewTaskMessage newTask, UserProfile userInfo)
        {
            List<TaskRecipient> UserRecipients = new List<TaskRecipient>();
            var documents = new List<int>();
            if (newTask.NotifyTo.Count == 0 && newTask.NotifyCC.Count == 0)
            {
                newTask.NotifyTo.Add(((int)ProfileType.User).ToString() + newTask.Sender);
            }

            await CreateUserTasks(newTask.NotifyTo, newTask.AssignToAllUsers, null, userInfo, UserRecipients, documents, false, true);
            await CreateUserTasks(newTask.NotifyCC, newTask.AssignToAllUsers, null, userInfo, UserRecipients, documents, true, true);

            return UserRecipients.Count;
        }


        private async Task<List<TaskRecipient>> CreateUserTasksAndGetUsersToNotify(CreateNewTaskMessage newTask, TaskItem TI, UserProfile userInfo)
        {
            List<TaskRecipient> UserRecipients = new List<TaskRecipient>();
            var documents = TI.Attachments.Select(a => a.DocumentId).ToList();
            // TODO: Verificare le modalità di notifica 

            // Se non ci sono destinatari (principali o in cc) assegno il task al richiedente
            if (newTask.NotifyTo.Count == 0 && newTask.NotifyCC.Count == 0)
            {
                newTask.NotifyTo.Add(((int)ProfileType.User).ToString() + TI.FromUserId);
            }

            await CreateUserTasks(newTask.NotifyTo, newTask.AssignToAllUsers, TI, userInfo, UserRecipients, documents, false);
            await CreateUserTasks(newTask.NotifyCC, newTask.AssignToAllUsers, TI, userInfo, UserRecipients, documents, true);
            return UserRecipients;
        }


        private async Task SetDocumentPermission(TaskItem taskItem, List<int> documents, UserProfile userInfo, string profileId, ProfileType profileType)
        {
            if (documents.Count == 0) return;
            Dictionary<string, AuthorizationType> Perms = new Dictionary<string, AuthorizationType>()
            {
               { PermissionType.CanViewContent, AuthorizationType.Granted },
               { PermissionType.CanView, AuthorizationType.Granted }
            };
            switch (taskItem.EventId)
            {
                case EventType.RemoveRevision:
                case EventType.RemoveVersion:
                    Perms.Add(PermissionType.CanRemoveContent, AuthorizationType.Granted);
                    break;
                case EventType.Delete:
                    Perms.Add(PermissionType.CanDelete, AuthorizationType.Granted);
                    break;
                case EventType.ChangeStatus:
                case EventType.Classify:
                case EventType.AddAttach:
                case EventType.AddToFolder:
                case EventType.RemoveFromFolder:
                    Perms.Add(PermissionType.CanEdit, AuthorizationType.Granted);
                    break;
                case EventType.AddProtocolSign:
                case EventType.Protocol:
                    Perms.Add(PermissionType.CanProtocol, AuthorizationType.Granted);
                    Perms.Add(PermissionType.CanAddContent, AuthorizationType.Granted);
                    break;
                case EventType.AddStamp:
                case EventType.AddRevision:
                case EventType.AddVersion:
                case EventType.CheckIn:
                case EventType.CheckOut:
                    Perms.Add(PermissionType.CanAddContent, AuthorizationType.Granted);
                    break;
                case EventType.AddCheckSign:
                case EventType.AddBiometricalSignature:
                case EventType.AddDigitalSignature:
                case EventType.AddPreservationSignature:
                case EventType.AddSignatureField:
                    Perms.Add(PermissionType.CanAddContent, AuthorizationType.Granted);
                    break;
                case EventType.Publish:
                case EventType.Share:
                case EventType.Print:
                case EventType.Download:
                    Perms.Add(PermissionType.CanShare, AuthorizationType.Granted);
                    break;
                default:
                    break;
            }
            await _documentService.SetPermissions(documents, userInfo, profileId, profileType, Perms);
        }

        private async Task CreateUserTasks(List<string> Notify, bool AssignToAllUsers, TaskItem TI, UserProfile userInfo, List<TaskRecipient> UserRecipients, List<int> documents, bool CC, bool test = false)
        {
            foreach (var n in Notify)
                if (!String.IsNullOrEmpty(n) && n.Length > 1)
                {
                    string profileId = n.Substring(1);
                    string GroupId = profileId;
                    string RoleId = "";
                    ProfileType ptype = (ProfileType)int.Parse(n.Substring(0, 1));
                    int i = profileId.IndexOf("\\");
                    if (ptype == ProfileType.Role)
                    {
                        if (i >= 0)
                        {
                            RoleId = profileId.Substring(0, i);
                            GroupId = profileId.Substring(i + 1);
                        }
                        else
                        {
                            RoleId = profileId;
                            GroupId = "";
                        }
                        if (RoleId.StartsWith("@"))
                        {
                            var role = await _roleService.GetByName(RoleId.Substring(1));
                            if (role != null) RoleId = role.Id;
                        }
                        if (GroupId.StartsWith("@"))
                        {
                            var Group = await groupRepo.GetByExternalId(GroupId.Substring(1));
                            if (Group != null) GroupId = Group.Id;
                        }
                    }
                    else if (ptype == ProfileType.Group)
                    {
                        if (profileId.StartsWith("@"))
                        {
                            var Group = await groupRepo.GetByExternalId(profileId.Substring(1));
                            if (Group != null) profileId = Group.Id;
                        }

                    }
                    // Destinatari Iniziali del Task
                    TaskRecipient r = new TaskRecipient();
                    r.ProfileType = ptype;
                    r.ProfileId = profileId;
                    r.CC = CC;
                    if (!test)
                        TI.Recipients.Add(r);

                    switch (ptype)
                    {
                        case ProfileType.User:
                            if (UserRecipients.FirstOrDefault(r => r.ProfileId == profileId && r.ProfileType == ProfileType.User) == null)
                            {
                                if (!test)
                                {
                                    await SetDocumentPermission(TI, documents, userInfo, r.ProfileId, r.ProfileType);
                                    UserTask UT = new UserTask();
                                    UT.ExpirationDate = TI.ExpirationDate;
                                    UT.GroupId = "";
                                    UT.RoleId = "";
                                    UT.UserId = profileId;
                                    UT.CC = CC;
                                    UT.Variables = TI.Variables;
                                    TI.UserTasks.Add(UT);
                                }
                                UserRecipients.Add(r);
                            }
                            break;
                        case ProfileType.Group:
                            if (!test)
                            {
                                await SetDocumentPermission(TI, documents, userInfo, profileId, ptype);
                            }
                            // Aggiungo gli utenti dei gruppi abilitati all'inbox  
                            var users = await _organizationRepository.GetUsersInGroup(profileId);
                            var group = (await _organizationRepository.GetById(0, profileId))?.UserGroup;
                            if (group != null)
                            {
                                var permissionid = CC ? PermissionType.CanViewInBoxCC : PermissionType.CanViewInBox;
                                var permission = AuthorizationType.None;

                                /// 1. Utenti abilitati al permesso "ViewInbox"
                                /// 2. Utenti con ruolo specifico (NotificationProfile)
                                /// 3. Tutti gli utenti del gruppo
                                /// 4. Tutti gli utenti e sotto-strutture
                                var strategy = CC ? group?.NotificationStrategyCC : group?.NotificationStrategy;
                                if (AssignToAllUsers) strategy = 3;
                                switch (strategy)
                                {
                                    case 1: permission = await aclService.GetAuthorization("", profileId, ProfileType.Group, permissionid); break;
                                    case 2: users = users.Where(u => u.RoleId == group.NotificationProfile).ToList(); permission = AuthorizationType.Granted; break;
                                    case 4: //TODO: Smistare ai sottogruppi se Strategy = 4
                                        foreach (var g in await organizationUnitService.GetOrganizationTree(0, GroupId))
                                        {
                                            users.AddRange(await _organizationRepository.GetUsersInGroup(g.Info.UserGroupId));
                                        }
                                        break;
                                    default:
                                        permission = AuthorizationType.Granted; break;
                                }
                                var gfound = false;
                                foreach (var u in users)
                                {
                                    var upermission = permission;
                                    if (strategy == 1)
                                    {
                                        if (upermission == AuthorizationType.None)
                                            upermission = await aclService.GetAuthorization("", u.RoleId + "\\" + u.UserGroupId, ProfileType.Role, permissionid);
                                        if (upermission == AuthorizationType.None)
                                            upermission = await aclService.GetAuthorization("", u.UserId, ProfileType.User, permissionid);
                                    }
                                    if (upermission == AuthorizationType.Granted)
                                    {
                                        if (UserRecipients.FirstOrDefault(r => r.ProfileId == u.UserId && r.ProfileType == ProfileType.User) == null)
                                        {
                                            TaskRecipient tr = new TaskRecipient() { ProfileId = u.UserId, ProfileType = ProfileType.User, CC = CC };
                                            if (!test)
                                            {
                                                await SetDocumentPermission(TI, documents, userInfo, tr.ProfileId, tr.ProfileType);
                                                //Assegno il task all'utente solo se:
                                                //1. strategia è diverso da 1
                                                //2. Ho scelto AssignToAllUsers (in questo caso strategia = 3)
                                                // Creo uno UserTask per ogni utente se il task viene assegnato a tutti o se è in CC
                                                if (strategy != 1)
                                                {
                                                    UserTask UT1 = new UserTask();
                                                    UT1.ExpirationDate = TI.ExpirationDate;
                                                    UT1.GroupId = u.UserGroupId;
                                                    UT1.RoleId = RoleId;
                                                    UT1.UserId = u.UserId;
                                                    UT1.Variables = TI.Variables;
                                                    UT1.CC = CC;
                                                    TI.UserTasks.Add(UT1);
                                                    gfound = true;
                                                }
                                            }
                                            UserRecipients.Add(tr);
                                        }
                                    }
                                }
                                if (strategy == 1 && !gfound && !test)
                                {
                                    UserTask UT2 = new UserTask();
                                    UT2.ExpirationDate = TI.ExpirationDate;
                                    UT2.GroupId = GroupId;
                                    UT2.RoleId = "";
                                    UT2.UserId = "";
                                    UT2.Variables = TI.Variables;
                                    UT2.CC = CC;
                                    TI.UserTasks.Add(UT2);
                                }
                            }
                            break;
                        case ProfileType.Role:
                            if (!test)
                            {
                                await SetDocumentPermission(TI, documents, userInfo, profileId, ptype);
                            }

                            // Aggiungo gli utenti dei gruppi abilitati all'inbox o gli utenti dei ruoli 
                            List<string> usersr = null;
                            if (String.IsNullOrEmpty(GroupId))
                                usersr = (await _organizationRepository.GetUsersInRole(RoleId)).Distinct().ToList();
                            else
                                usersr = (await _organizationRepository.GetUsersInGroup(GroupId)).Where(u => u.RoleId == RoleId).Select(u => u.UserId).Distinct().ToList();
                            bool rfound = false;
                            if (usersr != null)
                                foreach (var u in usersr)
                                {
                                    if (UserRecipients.FirstOrDefault(r => r.ProfileId == u && r.ProfileType == ProfileType.User) == null)
                                    {
                                        TaskRecipient tr = new TaskRecipient() { ProfileId = u, ProfileType = ProfileType.User, CC = CC };
                                        //if (!test)
                                        //{
                                        //    await SetDocumentPermission(TI, documents, userInfo, tr.ProfileId, tr.ProfileType);
                                        //}
                                        if ((AssignToAllUsers || CC) && !test)
                                        {

                                            UserTask UT2 = new UserTask();
                                            UT2.ExpirationDate = TI.ExpirationDate;
                                            UT2.GroupId = GroupId;
                                            UT2.RoleId = RoleId;
                                            UT2.Variables = "";
                                            UT2.UserId = u;
                                            UT2.Variables = TI.Variables;
                                            UT2.CC = CC;
                                            TI.UserTasks.Add(UT2);
                                            rfound = true;
                                        }
                                        UserRecipients.Add(tr);
                                    }
                                }
                            // Se ho trovato il ruolo ma non ho assegnato a tutti... assegno al ruolo
                            if (!AssignToAllUsers && !rfound && !test)
                            {

                                UserTask UT2 = new UserTask();
                                UT2.ExpirationDate = TI.ExpirationDate;
                                UT2.GroupId = GroupId;
                                UT2.RoleId = RoleId;
                                UT2.UserId = "";
                                UT2.Variables = TI.Variables;
                                UT2.CC = CC;
                                TI.UserTasks.Add(UT2);
                            }

                            break;
                        default:
                            break;
                    }
                }
        }

        private async Task<bool> CheckFilters(TaskListFilter filters, UserProfile userInfo, bool ok)
        {
            if (!String.IsNullOrEmpty(filters.Recipient) && filters.Recipient.StartsWith(((int)ProfileType.User).ToString()))
            {
                ok = userInfo.userId == SpecialUser.SystemUser;
                if (!ok)
                {
                    // Verifica che l'utente loggato sia collega dell'utente richiesto
                    var p = filters.Recipient.Substring(1);
                    foreach (var g in userInfo.Groups)
                    {
                        var users = await _organizationRepository.GetUsersInGroup(g.Id);
                        var MyRoles = users.Where(u => u.UserId == userInfo.userId).Select(u => u.RoleId).ToList();
                        if (MyRoles != null)
                        {
                            bool IsManager = false;
                            foreach (var r in MyRoles)
                            {
                                //IsManager = (await _aclService.GetAuthorization("*", userInfo.userId, r, g, PermissionType.CanViewUsers)) == AuthorizationType.Granted;
                                IsManager = (await aclService.GetAuthorization("", r, ProfileType.Role, PermissionType.Task_CanViewUsers)) == AuthorizationType.Granted;
                                if (IsManager) break;
                            }
                            ok = IsManager && users.Any(u => u.UserId == p);
                        }
                    }
                }
            }
            //if (!String.IsNullOrEmpty(filters.Recipient) && filters.Recipient.StartsWith(((int)ProfileType.Group).ToString()))
            //{
            //    // Verifica che l'utente loggato sia in uno dei gruppi richiesti
            //    var p = filters.Recipient.Substring(1);
            //    var i = (p.Contains("\\")) ? userInfo.RolesInGroups.FindIndex(g => g == p) : userInfo.Groups.FindIndex(g => g == p);
            //    ok = (i >= 0);
            //}
            if (!String.IsNullOrEmpty(filters.Recipient) && filters.Recipient.StartsWith(((int)ProfileType.Role).ToString()))
            {
                // Verifica che l'utente possieda il ruolo richiesto
                var p = filters.Recipient.Substring(1);
                var i = userInfo.Roles.FindIndex(r => r.Id == p);
                ok = (i >= 0);
            }
            if (filters.ParentId > 0) ok = true;
            if (filters.TaskItemId > 0) ok = true;
            if (filters.DocumentId > 0) ok = true;
            if (!String.IsNullOrEmpty(filters.ProcessDefinitionKey)) ok = true;
            if (!String.IsNullOrEmpty(filters.ProcessInstanceId)) ok = true;
            if (filters.ProjectId > 0) ok = true;
            return ok;
        }

        private string GetName(TaskRecipient r)
        {
            switch (r.ProfileType)
            {
                case ProfileType.User:
                    return userService.GetName(r.ProfileId).GetAwaiter().GetResult() ?? "";
                case ProfileType.Group:
                    var GroupId = r.ProfileId;
                    var g = groupRepo.GetById(GroupId).GetAwaiter().GetResult();
                    var Name = g == null ? "#"+GroupId : g.ShortName;
                    return Name;
                case ProfileType.Role:
                    var RoleId = r.ProfileId;
                    var RGroupId = "";
                    var i = RoleId.IndexOf("\\");
                    if (i >= 0)
                    {
                        RGroupId = RoleId.Substring(i + 1);
                        RoleId = RoleId.Substring(0, i);
                    }
                    var Rr = _roleService.GetById(RoleId).GetAwaiter().GetResult();
                    var RName = Rr == null ? "#"+RoleId : Rr.RoleName;
                    if (!String.IsNullOrEmpty(RGroupId))
                    {
                        var rg = groupRepo.GetById(RGroupId).GetAwaiter().GetResult();
                        RName += " (" + (rg == null ? "#"+RGroupId : rg.ShortName) + ")";
                    }
                    return RName;

                case ProfileType.MailAddress:
                    break;
                default:
                    break;
            }
            return "";
        }


        public async Task Rebase ()
        {
            // 1. Recupero gli utenti dismessi (senza ruoli attivi) e distribuisco le sue attività a chi ?
            // 2. Recupero gli uffici dismessi (senza nodi attivi) e distribuisco le sue attività secondo configurazione
            // 3. Contrassegno definitiamente i nodi dismessi

            var users = await _organizationRepository.GetInactiveUsers();
            foreach (var user in users)
            {
                await _repository.Reassign(user);
            }

            var nodes = await _organizationRepository.GetInactiveNodes();
            foreach (var node in nodes)
            {
                await _repository.Reassign(node);
            }
        }
    }
}