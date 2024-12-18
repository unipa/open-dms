using Newtonsoft.Json.Linq;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;

namespace OpenDMS.Workflow.API.Workers.Documents
{
    public class AddToFolderWorker : BaseWorker
    {
        private readonly IDocumentService documentService;

        public override string JobType { get; set; } = "addToFolderTask";
        public override string TaskLabel { get; set; } = "Aggiunge un documento ad un fascicolo";
        public override string Icon { get; set; } = "fa fa-folder";
        public override string GroupName { get; set; } = "Documenti";
        public override string[] AlternativeTasks { get; set; } = { };

        public override string Inputs { get; set; } = "TaskDocumentId,TaskFolderId,MoveToFolder";
        public override string Outputs { get; set; } = "Errors";

        public override int MaxJobs { get; set; } = 1;
        public override int PollingInterval { get; set; } = 1;
        public override int TimeOut { get; set; } = 60;





        public AddToFolderWorker(
            ILogger<AddToFolderWorker> logger,
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
                    Label = "Id documento",
                    Description = "Id del documento",
                    Values = "",
                    DefaultValue = "DocumentId"
                },
                new InputParameter
                {
                    Name = "TaskFolderId",
                    Required = true,
                    InputType = 0,
                    Label = "Id Fascicolo",
                    Description = "Id del Fascicolo",
                    Values = "",
                    DefaultValue = "0"
                },
                new InputParameter
                {
                    Name = "MoveToFolder",
                    Required = false,
                    InputType = 0,
                    Label = "Sposta alla cartella",
                    Description = "Specifica in quale cartella va spostato il documento",
                    Values = "",
                    DefaultValue = "0"
                }
            };
            var outputs = new List<OutputParameter>
            {
 
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
            // 
            var jobKey = job.Key;
            //        public string Inputs { get; set; } = "DocumentId,FileName,Document,Filters,MaxResults";
            var docId = (int?)JObject.Parse(job.Variables)["TaskDocumentId"] ?? 0;
            //if (docId == 0) docId = (int?)JObject.Parse(job.Variables)["DocumentId"] ?? 0;
            var errors = new List<BatchErrorResult>();
            if (docId > 0)
            {
                var folderId = (int?)JObject.Parse(job.Variables)["TaskFolderId"] ?? 0;
                if (folderId == 0) folderId = (int?)JObject.Parse(job.Variables)["TargetFolderId"] ?? 0;
                var move = ((bool?)JObject.Parse(job.Variables)["MoveToFolder"] ?? false);
                //var folderUID = (string?)JObject.Parse(job.Variables)["FolderUID"] ?? "";
                if (docId > 0 && folderId > 0)
                {
                    UserProfile u = UserProfile.SystemUser();
                    errors = await documentService.AddToFolder(folderId, new List<int>() { docId }, u, move);
                }
            }
            else
            {
                errors.Add(new BatchErrorResult(0, "Id Documento non indicato"));
            }
            var variables = System.Text.Json.JsonSerializer.Serialize(new { Errors = errors });
            return variables;
        }
    }
}
