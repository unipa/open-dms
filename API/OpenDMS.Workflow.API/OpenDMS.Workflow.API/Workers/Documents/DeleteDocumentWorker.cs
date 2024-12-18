using Newtonsoft.Json.Linq;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;

namespace OpenDMS.Workflow.API.Workers.Documents
{
    public class DeleteDocumentWorker : BaseWorker
    {
        private readonly IDocumentService documentService;

        public override string JobType { get; set; } = "deleteDocumentTask";
        public override string TaskLabel { get; set; } = "Cancella un Documento";
        public override string Icon { get; set; } = "fa fa-trash-o";
        public override string GroupName { get; set; } = "Documenti";
        public override string[] AlternativeTasks { get; set; } = { };

        public override string Inputs { get; set; } = "TaskDocumentId,Justification";
        public override string Outputs { get; set; } = "";
        public override int MaxJobs { get; set; } = 1;
        public override int PollingInterval { get; set; } = 1;
        public override int TimeOut { get; set; } = 60;




        public DeleteDocumentWorker(
            ILogger<DeleteDocumentWorker> logger,
            IWorkflowEngine engine,
            IDocumentService documentService,
           IAppSettingsRepository appSettings) : base(logger, engine, appSettings)
        {
            this.documentService = documentService;
        }




        public override async Task<TaskItem> PaletteItem()
        {
            var inputs = new List<InputParameter>
            {
                new InputParameter
                {
                    Name = "TaskDocumentId",
                    Required = true,
                    InputType = 0,
                    Label = "Id del documento in attività",
                    Description = "Id del documento da cancellare",
                    Values = "",
                    DefaultValue = "DocumentId"
                },
                new InputParameter
                {
                    Name = "Justification",
                    Required = false,
                    InputType = 0,
                    Label = "Giustificativo",
                    Description = "Ragione per la cancellazione del documento",
                    Values = "",
                    DefaultValue = "\"\""
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
                JobWorker =this.JobType,
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
        public override async Task<string> Execute(IJob job)
        {
            // business logic
            // 
            var jobKey = job.Key;
                var docId = (int?)JObject.Parse(job.Variables)["TaskDocumentId"] ?? 0;
               // if (docId == 0) docId = (int?)JObject.Parse(job.Variables)["DocumentId"] ?? 0;
                var status= ((string?)JObject.Parse(job.Variables)["Justification"]);
                if (docId > 0)
                {
                    UserProfile u = UserProfile.SystemUser();
                    await documentService.Delete (docId, u, status);
                }
            return "";
        }
    }
}
