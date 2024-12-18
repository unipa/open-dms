using Newtonsoft.Json.Linq;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;

namespace OpenDMS.Workflow.API.Workers.UserTasks
{
    public class RequestForEventTaskWorker : BaseWorker
    {
        private readonly IUserTaskService taskService;

        public override string JobType { get; set; } = "requestForEventTask";
        public override string TaskLabel { get; set; } = "Attesa Evento su Documento";
        public override string Icon { get; set; } = "fa fa-bolt";
        public override string GroupName { get; set; } = "Task Utente";
        public override string[] AlternativeTasks { get; set; } = new[] { "userTask", "userMessageTask", "formTask", "completeTask" };
        public override string Inputs { get; set; } = "EventId,Title,Description,CompanyId,ExpirationDate,CategoryId,ProjectId,ParentTaskId,FormKey,FormSchema,AssignToAllUsers,NotifyTo,NotifyCC,NotifyExecution,NotifyExpiration,PriorityId,Sender,Attachments";
        public override string Outputs { get; set; } = "TaskId,TaskUser,EventRaised";
        public override int MaxJobs { get; set; } = 1;
        public override int PollingInterval { get; set; } = 1;
        public override int TimeOut { get; set; } = 86400;


        public RequestForEventTaskWorker(
            ILogger<RequestForEventTaskWorker> logger,
            IWorkflowEngine engine,
            IUserTaskService taskService,
          IAppSettingsRepository appSettings) : base(logger, engine, appSettings)
        {
            this.taskService = taskService;
        }


        public override async Task<bool> Complete(IJob job)
        {
            return false;
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
                TaskInputVariable.ProcessInstanceId = processId;
                TaskInputVariable.JobInstanceId = jobKey.ToString();
                TaskInputVariable.ProcessDefinitionKey = processkey;
                TaskInputVariable.ProcessDataId = (int?)JObject.Parse(job.Variables)["ProcessId"] ?? 0; ;
                TaskInputVariable.Title = (string?)JObject.Parse(job.Variables)["Title"] ?? "";
                if (String.IsNullOrEmpty(TaskInputVariable.Title))
                    TaskInputVariable.Title = "Nuova Attività su Documento";

                TaskInputVariable.Description = (string?)JObject.Parse(job.Variables)["Description"] ?? "";
                TaskInputVariable.ExpirationDate = ParseDate((string?)JObject.Parse(job.Variables)["ExpirationDate"], DateTime.UtcNow);

                TaskInputVariable.CategoryId = (string?)JObject.Parse(job.Variables)["CategoryId"] ?? "";
                TaskInputVariable.CompanyId = (int?)JObject.Parse(job.Variables)["CompanyId"] ?? 1;
                TaskInputVariable.ProjectId = (int?)JObject.Parse(job.Variables)["ProjectId"] ?? TaskInputVariable.ProcessDataId;
                TaskInputVariable.ParentTaskId = (int?)JObject.Parse(job.Variables)["ParentTaskId"] ?? 0;
                //                   TaskInputVariable.FormSchema = (string?)JObject.Parse(job.Variables)["FormSchema"] ?? "";
                TaskInputVariable.AssignToAllUsers = (bool?)JObject.Parse(job.Variables)["AssignToAllUsers"] ?? false;
                TaskInputVariable.NotifyTo = ((string?)JObject.Parse(job.Variables)["NotifyTo"] + "").Split(',').ToList();
                TaskInputVariable.NotifyExecution = (bool?)JObject.Parse(job.Variables)["NotifyExecution"] ?? false;
                TaskInputVariable.StartDate = ParseDate((string?)JObject.Parse(job.Variables)["NotificationDate"], DateTime.UtcNow);
                TaskInputVariable.NotifyExpiration = (bool?)JObject.Parse(job.Variables)["NotifyExpiration"] ?? false;
                TaskInputVariable.Priority = (string?)JObject.Parse(job.Variables)["PriorityId"] ?? "";

                var SignatureField = (string?)JObject.Parse(job.Variables)["SignatureField"] ?? "";
                if (string.IsNullOrEmpty(SignatureField)) SignatureField = (string?)JObject.Parse(job.Variables)["SignField"] ?? "";

                UserProfile lastUserObj = JObject.Parse(job.Variables)["TaskUser"]?.ToObject<UserProfile>();
                string lastUser = lastUserObj != null ? lastUserObj.userId : SpecialUser.SystemUser;
                var sender = (string?)JObject.Parse(job.Variables)["Sender"] ?? lastUser;
                TaskInputVariable.Sender = sender;


                TaskInputVariable.EventId = (string?)JObject.Parse(job.Variables)["EventId"] ?? "";
                if (string.IsNullOrEmpty(TaskInputVariable.EventId))
                    TaskInputVariable.EventId = (string?)JObject.Parse(job.Variables)["EventType"] ?? "";
                if (string.IsNullOrEmpty(TaskInputVariable.EventId))
                    TaskInputVariable.EventId = EventType.Approval;

                TaskInputVariable.TaskType = string.IsNullOrEmpty(TaskInputVariable.EventId) ? Domain.Enumerators.TaskType.Activity : Domain.Enumerators.TaskType.Event;
                var json = JObject.Parse(job.Variables);
                if (json.ContainsKey("Justification"))
                {
                    json.Remove("Justification");
                };
                if (json.ContainsKey("justification"))
                {
                    json.Remove("justification");
                }
                if (!String.IsNullOrEmpty( SignatureField))
                {
                    json["SignatureField"] = (SignatureField);
                }
                var formKey = (string?)JObject.Parse(job.Variables)["FormKey"];
                TaskInputVariable.FormKey = formKey;

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


                UserProfile u = UserProfile.SystemUser();
                var userTaskInfo = await taskService.CreateTask(TaskInputVariable, u); //, processId, processkey);//, ApprovalValues, RejectValues, OtherValues, MinimalApproval);
                var variables = "{ \"TaskId\" : " + userTaskInfo.Id + " }";
                return variables;
            }
            else
            {
                // Aggiorno lo stato del task sulla tasklist
                await taskService.Refresh(TaskId);
            }
            return "";
        }

        public override async Task<TaskItem> PaletteItem()
        {
            var inputs = new List<InputParameter>
            {
                new InputParameter
                {
                    Name = "EventId",
                    Required = true,
                    InputType = 0,
                    Label = "EventId",
                    Description = "Nome dell'evento di cui attendere l'esecuzione",
                    Values = "",
                    DefaultValue = "\"Document.AddDigitalSignature\""
                },

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
                //new InputParameter
                //{
                //    Name = "CompanyId",
                //    Required = false,
                //    InputType = 0,
                //    Label = "IdCompagnia",
                //    Description = "Id Compagnia",
                //    Values = "",
                //    DefaultValue = "1"
                //},
                new InputParameter
                {
                    Name = "ExpirationDate",
                    Required = false,
                    InputType = 0,
                    Label = "DatadiScadenza",
                    Description = "Data di Scadenza nel formato 'yyyy/mm/dd'<br/>E' possibile utilizzare il carattere '*' come sostituto del giorno/mese/anno corrente oppure la sintassi +n per sommare 'n' quantità ad un giorno/mese/anno",
                    Values = "",
                    DefaultValue = "\"\""
                },
                //new InputParameter
                //{
                //    Name = "CategoryId",
                //    Required = false,
                //    InputType = 0,
                //    Label = "IdCategoria",
                //    Description = "Id di Categoria",
                //    Values = "",
                //    DefaultValue = "\"\""
                //},
                //new InputParameter
                //{
                //    Name = "ProjectId",
                //    Required = false,
                //    InputType = 0,
                //    Label = "ProjectId",
                //    Description = "Id di Progetto",
                //    Values = "",
                //    DefaultValue = "\"\""
                //},
                //new InputParameter
                //{
                //    Name = "ParentTaskId",
                //    Required = false,
                //    InputType = 0,
                //    Label = "ParentTaskId",
                //    Description = "Id attività principale",
                //    Values = "",
                //    DefaultValue = "0"
                //},
                //new InputParameter
                //{
                //    Name = "AssignToAllUsers",
                //    Required = false,
                //    InputType = 0,
                //    Label = "AssignToAllUsers",
                //    Description = "Assegna a tutti gli utenti",
                //    Values = "",
                //    DefaultValue = "0"
                //},
                new InputParameter
                {
                    Name = "FormKey",
                    Required = true,
                    InputType = 0,
                    Label = "FormKey",
                    Description = "Identificativo del modulo",
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
                //new InputParameter
                //{
                //    Name = "NotifyExecution",
                //    Required = false,
                //    InputType = 0,
                //    Label = "NotifyExecution",
                //    Description = "Notifica di Esecuzione",
                //    Values = "",
                //    DefaultValue = ""
                //},
                //new InputParameter
                //{
                //    Name = "NotifyExpiration",
                //    Required = false,
                //    InputType = 0,
                //    Label = "NotifyExpiration",
                //    Description = "Notifica di Scadenza",
                //    Values = "",
                //    DefaultValue = ""
                //},
                //new InputParameter
                //{
                //    Name = "PriorityId",
                //    Required = false,
                //    InputType = 0,
                //    Label = "PriorityId",
                //    Description = "Id di Priorità",
                //    Values = "",
                //    DefaultValue = ""
                //},
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
                ColorFill = "",
                ColorStroke = "",
                Inputs = inputs,
                Outputs = outputs
            };

            return taskItem;
        }


    }
}
