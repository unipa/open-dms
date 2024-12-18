using Newtonsoft.Json.Linq;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Services.BusinessLogic;
using OpenDMS.Workflow.API.BusinessLogic;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;

namespace OpenDMS.Workflow.API.Workers.UserTasks
{
    public class CompleteTaskWorker : ICustomTask
    {
        private readonly ILogger<CompleteTaskWorker> logger;
        private readonly IWorkflowEngine client;
        private readonly IUserTaskService taskService;
        private readonly IAppSettingsRepository appSettings;

        public string JobType { get; set; } = "abortTask";
        public string TaskLabel { get; set; } = "Annullamento Task";
        public string Icon { get; set; } = "fa fa-outdent";
        public string GroupName { get; set; } = "Task Utente";
        public string[] AlternativeTasks { get; set; } = new[] { "requestForEventTask", "userMessageTask", "userTask", };

        public string Inputs { get; set; } = "TaskId,JobKey,FeedbackMessage";
        public string Outputs { get; set; } = "";

        public int MaxJobs { get; set; } = 1;
        public int PollingInterval { get; set; } = 1;
        public int TimeOut { get; set; } = 60;


        public CompleteTaskWorker(
            ILogger<CompleteTaskWorker> logger,
            IWorkflowEngine engine,
            IUserTaskService taskService,
            IAppSettingsRepository appSettings)
        {
            this.logger = logger;
            client = engine;
            this.taskService = taskService;
            this.appSettings = appSettings;
        }
        public async Task Initialize()
        {
            if (!int.TryParse(await appSettings.Get(Constants.CONST_USERTASK_WORKER_MAXJOBS), out int i))
                i = 1;
            MaxJobs = i;

            if (!int.TryParse(await appSettings.Get(Constants.CONST_USERTASK_WORKER_POLLING), out int i2))
                i2 = 1;
            PollingInterval = i2;

            if (!int.TryParse(await appSettings.Get(Constants.CONST_USERTASK_WORKER_TIMEOUT), out int i3))
                i3 = 10;
            TimeOut = i3;

            // await client.AddWorker(JobType, HandleJob, MaxJobs, PollingInterval, TimeOut);
        }
        public virtual async Task<TaskItem> PaletteItem()
        {
            var inputs = new List<InputParameter>
            {
                new InputParameter
                {
                    Name = "TaskId",
                    Required = true,
                    InputType = 0,
                    Label = "Id di Attività",
                    Description = "ID di Attività",
                    Values = "",
                    DefaultValue = ""
                },
                new InputParameter
                {
                    Name = "JobKey",
                    Required = true,
                    InputType = 0,
                    Label = "ChiavediLavoro",
                    Description = "Chiave di Lavoro",
                    Values = "",
                    DefaultValue = ""
                },
                new InputParameter
                {
                    Name = "FeedbackMessage",
                    Required = true,
                    InputType = 0,
                    Label = "MessaggioDiRiscontro",
                    Description = "Messaggio di Riscontro",
                    Values = "",
                    DefaultValue = ""
                }
            };
            var outputs = new List<OutputParameter>();

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
                Inputs = inputs,
                Outputs = outputs
            };

            return taskItem;
        }

    
        //internal class TaskVariables
        //{
        //    public int TaskId { get; set; }
        //    public string Sender { get; set; } = "";
        //    public string Recipients { get; set; } = "";
        //    public string Event { get; set; } = "";
        //    public string Attachments { get; set; } = "";

        //    public string FeedbackMessage { get; set; }

        //}


        public async Task HandleJob(IJobClient jobClient, IJob job)
        {
            // business logic
            // 
            var jobKey = job.Key;
            var processkey = job.BpmnProcessId;//.Key;

            var taskID = (int?)JObject.Parse(job.Variables)["TaskId"] ?? 0;
            var lastJobKey = (int?)JObject.Parse(job.Variables)["JobKey"] ?? 0;
            var Message = (string?)JObject.Parse(job.Variables)["FeedbackMessage"] ?? "";
            var variables = "{ \"TaskID\" : 0 }";
            if (taskID > 0)
            {
                await taskService.CompleteByTaskId (taskID, Message);
                //TODO: cercare i task tramite altri parametri
                //var TaskId = await taskService.GetByProcessId(lastJobKey.ToString());
                //if (TaskId > 0)
            }
            await jobClient.NewCompleteJobCommand(jobKey)
                .Variables(variables)
                .Send();
        }
    }
}
