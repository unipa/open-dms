using Newtonsoft.Json.Linq;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;

namespace OpenDMS.Workflow.API.Workers.Documents
{
    public class GetDocumentWorker : BaseWorker
    {
        private readonly ILogger<GetDocumentWorker> logger;
        private readonly IDocumentService documentService;

        public override string JobType { get; set; } = "getDocumentTask";
        public override string TaskLabel { get; set; } = "Recupero Documento";
        public override string Icon { get; set; } = "fa fa-file-text-o";
        public override string GroupName { get; set; } = "Documenti";
        public override string[] AlternativeTasks { get; set; } = { };

        public override string Inputs { get; set; } = "TaskDocumentId";
        public override string Outputs { get; set; } = "Document";

        public override int MaxJobs { get; set; } = 1;
        public override int PollingInterval { get; set; } = 1;
        public override int TimeOut { get; set; } = 60;



        public GetDocumentWorker(
            ILogger<GetDocumentWorker> logger,
            IWorkflowEngine engine,
            IDocumentService documentService,
             IAppSettingsRepository appSettings) : base(logger, engine, appSettings)
        {
            this.logger = logger;
            this.documentService = documentService;
        }


        public override async Task<string> Execute(IJob job)
        {
            // business logic
            // 
            var jobKey = job.Key;
            var docId = (int?)JObject.Parse(job.Variables)["TaskDocumentId"] ?? 0;
            //if (docId == 0) docId = (int?)JObject.Parse(job.Variables)["DocumentId"] ?? 0;
            if (docId > 0)
            {
                UserProfile u = UserProfile.SystemUser();
                var doc = await documentService.Load(docId, u);
                logger.LogDebug(TaskLabel + ": " + job.ProcessDefinitionKey + "\\" + job.ElementId + "\\" + jobKey + " - Instance: " + job.ProcessDefinitionKey + "\\" + job.ElementInstanceKey);

                var variables = System.Text.Json.JsonSerializer.Serialize(new { Document = doc });
                return variables;
            }
            return "";
        }



        public override  async Task<TaskItem> PaletteItem()
        {
            var inputs = new List<InputParameter>
            {
                new InputParameter
                {
                    Name = "TaskDocumentId",
                    Required = true,
                    InputType = 0,
                    Label = "Id del documento Attività",
                    Description = "Id del documento da recuperare",
                    Values = "",
                    DefaultValue = "DocumentId"
                }
            };
            var outputs = new List<OutputParameter>
            {
                new OutputParameter
                {
                    Name = "Document",
                    DefaultValue = "",
                    Required = false,
                    Label = "Documento Recuperato",
                    Description = "Documento Recuperato"
                }
            };

            var taskItem = new TaskItem
            {
                Id = this.JobType,
                TaskServiceId= this.JobType,
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
