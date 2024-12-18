using Newtonsoft.Json.Linq;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;

namespace OpenDMS.Workflow.API.Workers.Documents
{
    public class GetDocumentContentWorker : BaseWorker
    {
        private readonly IDocumentService documentService;
        private readonly IVirtualFileSystemProvider virtualFileSystem;

        public override string JobType { get; set; } = "getDocumentContentTask";
        public override string TaskLabel { get; set; } = "Recupero Contenuto";
        public override string Icon { get; set; } = "fa fa-image";
        public override string GroupName { get; set; } = "Documenti";
        public override string[] AlternativeTasks { get; set; } = { };

        public override string Inputs { get; set; } = "TaskDocumentId";
        public override string Outputs { get; set; } = "OutputFile";

        public override int MaxJobs { get; set; } = 1;
        public override int PollingInterval { get; set; } = 1;
        public override int TimeOut { get; set; } = 60;



        public GetDocumentContentWorker(
            ILogger<GetDocumentContentWorker> logger,
            IWorkflowEngine engine,
            IDocumentService documentService,
            IVirtualFileSystemProvider virtualFileSystem,
          IAppSettingsRepository appSettings) : base(logger, engine, appSettings)
        {
            this.documentService = documentService;
            this.virtualFileSystem = virtualFileSystem;
        }


        public override async Task<string> Execute(IJob job)
        {
            // business logic
            // 
            var jobKey = job.Key;
            string OutputFile = "";
            var docId = (int?)JObject.Parse(job.Variables)["TaskDocumentId"] ?? 0;
            if (docId > 0)
            {
                UserProfile u = UserProfile.SystemUser();
                var doc = await documentService.Load(docId, u);
                if (doc != null && doc.Image != null)
                {
                    var content = await documentService.GetContent(doc.Image.Id);


                    var fm = await virtualFileSystem.InstanceOf((await appSettings.Get("Documents.FileSystemType")) + "");
                    var basePath = (await appSettings.Get("Documents.TempFolder")) + "";
                    if (string.IsNullOrEmpty(basePath))
                    {
                        basePath = "Temp";
                    }
                    OutputFile = Path.Combine(basePath, doc.Image.FileName + "_" + Guid.NewGuid().ToString() + Path.GetFileName(doc.Image.FileName));
                    await fm.WriteAllBytes(OutputFile, content);

                }
            }
            var variables = System.Text.Json.JsonSerializer.Serialize(new { OutputFile = OutputFile });
            return variables;
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
                    Label = "Id Documento attività",
                    Description = "Id del documento per cui recuperare il contenuto",
                    Values = "",
                    DefaultValue = "DocumentId"
                }
            };
            var outputs = new List<OutputParameter>
            {
                new OutputParameter
                {
                    Name = "OutputFile",
                    DefaultValue = "",
                    Required = false,
                    Label = "File di Output",
                    Description = "Percorso del file per il contenuto da recuperare"
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
