using Newtonsoft.Json.Linq;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;

namespace OpenDMS.Workflow.API.Workers.UserTasks
{
    public class UserMessageWorker : BaseWorker
    {
        private readonly ILogger<UserMessageWorker> logger;
        private readonly IUserTaskService taskService;

        public override string JobType { get; set; } = "userMessageTask";
        public override string TaskLabel { get; set; } = "Messaggio Utente";
        public override string Icon { get; set; } = "fa fa-comment";
        public override string GroupName { get; set; } = "Task Utente";
        public override string[] AlternativeTasks { get; set; } = new[] { "userTask", "requestForEventTask", "formTask", "abortTask" };
        public override string Inputs { get; set; } = "Title,Description,CompanyId,ProjectId,ParentTaskId,AssignToAllUsers,NotifyTo,NotifyCC,Sender,Attachments";
        public override string Outputs { get; set; } = "TaskId";
        public override int MaxJobs { get; set; } = 1;
        public override int PollingInterval { get; set; } = 1;
        public override int TimeOut { get; set; } = 86400;


        public UserMessageWorker(
            ILogger<UserMessageWorker> logger,
            IWorkflowEngine engine,
            IUserTaskService taskService,
            IAppSettingsRepository appSettings) : base(logger, engine, appSettings)
        {
            this.logger = logger;
            this.taskService = taskService;
        }


        public override async Task<bool> Complete(IJob job)
        {
            return true;
        }

        public override async Task<string> Execute(IJob job)
        {
            // business logic
            // 
            var jobKey = job.Key;
            var processId = job.ProcessInstanceKey.ToString();
            var processkey = job.BpmnProcessId.ToString();
            var TaskId = await taskService.GetByJobKey(jobKey.ToString());
            if (TaskId <= 0)
            {
                logger.LogDebug(TaskLabel + ": " + job.ProcessDefinitionKey + "\\" + job.ElementId + "\\" + job.Key + " - Instance: " + job.ProcessDefinitionKey + "\\" + job.ElementInstanceKey);

                CreateNewTask TaskInputVariable = new CreateNewTask();
                TaskInputVariable.Title = "" + (string?)JObject.Parse(job.Variables)["Title"] ?? "";
                if (String.IsNullOrEmpty(TaskInputVariable.Title))
                    TaskInputVariable.Title = "Nuovo Messaggio da Processo";
                TaskInputVariable.ProcessInstanceId = processId;
                TaskInputVariable.JobInstanceId = jobKey.ToString();
                TaskInputVariable.ProcessDefinitionKey = processkey;
                TaskInputVariable.ProcessDataId = (int?)JObject.Parse(job.Variables)["ProcessId"] ?? 0; ;



                TaskInputVariable.StartDate = ParseDate((string?)JObject.Parse(job.Variables)["NotificationDate"], DateTime.UtcNow);
                TaskInputVariable.Description = "" +  (string?)JObject.Parse(job.Variables)["Description"] ?? "";
                TaskInputVariable.CompanyId = (int?)JObject.Parse(job.Variables)["CompanyId"] ?? 0;
                TaskInputVariable.ProjectId = (int?)JObject.Parse(job.Variables)["ProjectId"] ?? TaskInputVariable.ProcessDataId;
                TaskInputVariable.ParentTaskId = (int?)JObject.Parse(job.Variables)["ParentTaskId"] ?? 0;
                TaskInputVariable.AssignToAllUsers = (bool?)JObject.Parse(job.Variables)["AssignToAllUsers"] ?? false;
                TaskInputVariable.NotifyCC = ((string?)JObject.Parse(job.Variables)["NotifyCC"] + "").Split(',').ToList();
                TaskInputVariable.NotifyTo = ((string?)JObject.Parse(job.Variables)["NotifyTo"] + "").Split(',').ToList();

                UserProfile lastUserObj = JObject.Parse(job.Variables)["TaskUser"]?.ToObject<UserProfile>();
                string lastUser = lastUserObj != null ? lastUserObj.userId : SpecialUser.SystemUser;
                var sender = (string?)JObject.Parse(job.Variables)["Sender"] ?? lastUser;
                TaskInputVariable.Sender = sender;

                TaskInputVariable.TaskType = Domain.Enumerators.TaskType.Message;
                var json = JObject.Parse(job.Variables);
                if (json.ContainsKey("Justification"))
                {
                    json.Remove("Justification");
                };
                if (json.ContainsKey("justification"))
                {
                    json.Remove("justification");
                }

                TaskInputVariable.Links = new List<int>();
                TaskInputVariable.Attachments = new List<int>();

                var att = JObject.Parse(job.Variables)["Attachments"];
                if (att != null)
                    try
                    {
                        foreach (var s in att.ToObject<int[]>())
                            if (s > 0)
                                TaskInputVariable.Attachments.Add(s);

                    }
                    catch (Exception)
                    {
                        var s = att.ToObject<int>();
                        if (s > 0)
                            TaskInputVariable.Attachments.Add(s);
                    }

                var links = JObject.Parse(job.Variables)["Links"];
                if (links != null)
                    try
                    {
                        foreach (var s in links.ToObject<int[]>())
                            if (s > 0)
                                TaskInputVariable.Links.Add(s);
                    }
                    catch (Exception)
                    {
                        var s = links.ToObject<int>();
                        if (s > 0)
                            TaskInputVariable.Links.Add(s);
                    }

                json.Remove("Title");
                json.Remove("Description");
                json.Remove("FormKey");
                json.Remove("EventId");
                json.Remove("EventType");
                TaskInputVariable.FormData = json.ToString();
                TaskInputVariable.FormKey = "";
                TaskInputVariable.EventId = "";
                UserProfile u = UserProfile.SystemUser();
                var userTaskInfo = await taskService.CreateTask(TaskInputVariable, u);//, processId, processkey);
                var variables = "{ \"TaskId\" : " + userTaskInfo.Id + " }";
                return variables;
            }
            return "";
        }


        public override async Task<TaskItem> PaletteItem()
        {
            var inputs = new List<InputParameter>
            {
                 new InputParameter
                {
                    Name = "Title",
                    Required = true,
                    InputType = 0,
                    Label = "Title",
                    Description = "Titolo della richiesta",
                    Values = "",
                    DefaultValue = "\"Richiesta di Autorizzazione\""
                },
                new InputParameter
                {
                    Name = "Description",
                    Required = true,
                    InputType = 0,
                    Label = "Descrizione",
                    Description = "Descrizione dettagliata dell'attività richiesta",
                    Values = "",
                    DefaultValue = "\"\""
                },
                new InputParameter
                {
                    Name = "NotifyTo",
                    Required = true,
                    InputType = 0,
                    Label = "NotifyTo",
                    Description = "Elenco di destinatari nel formato [<tipo><id>,<tipo><id>] dove <tipo> indica un utente (0), ruolo(2) o una struttura(1)",
                    Values = "",
                    DefaultValue = "[]"
                },
                new InputParameter
                {
                    Name = "NotifyCC",
                    Required = true,
                    InputType = 0,
                    Label = "NotifyCC",
                    Description = "Elenco di destinatari in conoscenza nel formato [<tipo><id>,<tipo><id>] dove <tipo> indica un utente (0), ruolo(2) o una struttura(1)",
                    Values = "",
                    DefaultValue = "[]"
                },
                new InputParameter
                {
                    Name = "Sender",
                    Required = true,
                    InputType = 0,
                    Label = "Sender",
                    Description = "Identifiativo dell'utente mittente (senza '0' iniziale)",
                    Values = "",
                    DefaultValue = "\"$system$\""
                },
                new InputParameter
                {
                    Name = "Attachments",
                    Required = false,
                    InputType = 0,
                    Label = "Allegati",
                    Description = "Elenco di Id di Allegati",
                    Values = "",
                    DefaultValue = "[]"
                },
                new InputParameter
                {
                    Name = "Links",
                    Required = false,
                    InputType = 0,
                    Label = "Links",
                    Description = "Elenco di Id di Links",
                    Values = "",
                    DefaultValue = "[]"
                }
            };
            var outputs = new List<OutputParameter>
            {
                new OutputParameter
                {
                    Name = "ExitCode",
                    DefaultValue = "ExitCode",
                    Required = false,
                    Label = "ExitCode",
                    Description = "Esito del Task (1=Ok)"
                },
                new OutputParameter
                {
                    Name = "Justification",
                    DefaultValue = "Justification",
                    Required = false,
                    Label = "Commento",
                    Description = "Commento dell'esito del task"
                },
                new OutputParameter
                {
                    Name = "TaskUser",
                    DefaultValue = "TaskUser",
                    Required = false,
                    Label = "TaskUser",
                    Description = "Attività Utente"
                }
            };


            var taskItem = new TaskItem
            {
                Id = this.JobType,
                TaskServiceId = this.JobType,
                Group = this.GroupName,
                Name = this.JobType,
                AuthenticationType = 0,
                JobWorker = this.JobType,
                Label = this.TaskLabel,
                Description = this.TaskLabel,
                Icon = this.Icon,
                ColorStroke = "",
                ColorFill = "",
                Inputs = inputs,
                Outputs = outputs
            };

            return taskItem;
        }


    }
}
