using Newtonsoft.Json.Linq;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;

namespace OpenDMS.Workflow.API.Workers.Documents
{
    public class ChangeStatusWorker : BaseWorker
    {
        private readonly IDocumentService documentService;
        private readonly IAppSettingsRepository appSettings;

        public override string JobType { get; set; } = "changeDocumentStatusTask";
        public override string TaskLabel { get; set; } = "Modifica lo stato di un Documento";
        public override string Icon { get; set; } = "fa fa-question-circle";
        public override string GroupName { get; set; } = "Documenti";
        public override string[] AlternativeTasks { get; set; } = { };

        public override string Inputs { get; set; } = "TaskDocumentId,Status";
        public override string Outputs { get; set; } = "NewStatus";
        public override int MaxJobs { get; set; } = 1;
        public override int PollingInterval { get; set; } = 1;
        public override int TimeOut { get; set; } = 60;




        public ChangeStatusWorker(
            ILogger<ChangeStatusWorker> logger,
            IWorkflowEngine engine,
            IDocumentService documentService,
           IAppSettingsRepository appSettings) : base(logger, engine, appSettings)
        {
            this.documentService = documentService;
            this.appSettings = appSettings;
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
                    Label = "Id del documento",
                    Description = "Id del documento al quale cambiare stato",
                    Values = "",
                    DefaultValue = "DocumentId"
                },
                new InputParameter
                {
                    Name = "Status",
                    Required = true,
                    InputType = 0,
                    Label = "Stato",
                    Description = "Nuovo stato del documento da modificare<br>1=Attivo<br>2=Bozza<br>5=Cancellato",
                    Values = "",
                    DefaultValue = "1"
                }

            };
            var outputs = new List<OutputParameter>
            {
                new OutputParameter
                {
                    Name = "NewStatus",
                    DefaultValue = "NewStatus",
                    Required = false,
                    Label = "Stato",
                    Description = "Nuovo Stato del documento appena modificato"
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
        public override async Task<string> Execute(IJob job)
        {
            // business logic
            // 
            var docId = (int?)JObject.Parse(job.Variables)["TaskDocumentId"] ?? 0;
            var status = (DocumentStatus?)((int?)JObject.Parse(job.Variables)["Status"]);
            var s = 0;
            if (docId > 0 && status.HasValue && status.Value > 0)
            {
                UserProfile u = UserProfile.SystemUser();
                await documentService.ChangeStatus(docId, u, status.Value);
                s = (int)status.Value;
            } 
            var variables = "{ \"NewStatus\" : " + System.Text.Json.JsonSerializer.Serialize(s) + " }";
            return variables;

        }
    }
}



