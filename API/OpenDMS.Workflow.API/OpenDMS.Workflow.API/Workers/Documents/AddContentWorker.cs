using Newtonsoft.Json.Linq;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;

namespace OpenDMS.Workflow.API.Workers.Documents
{
    public class AddContentWorker : BaseWorker
    {
        private readonly IDocumentService documentService;
        private readonly ISearchService documentSearchService;
        private readonly IVirtualFileSystemProvider virtualFileSystem;

        public override string JobType { get; set; } = "addContentTask";
        public override string TaskLabel { get; set; } = "Aggiunge un contenuto ad un Documento";
        public override string Icon { get; set; } = "fa fa-camera";
        public override string GroupName { get; set; } = "Documenti";
        public override string[] AlternativeTasks { get; set; } = { };

        public override string Inputs { get; set; } = "TargetDocumentId,InputFile,DeleteInputFile";
        public override string Outputs { get; set; } = "Content,errorMessage";
        public override int MaxJobs { get; set; } = 1;
        public override int PollingInterval { get; set; } = 1;
        public override int TimeOut { get; set; } = 60;





        public AddContentWorker(
            ILogger<AddContentWorker> logger,
            IWorkflowEngine engine,
            IDocumentService documentService,
            ISearchService documentSearchRepository,
            IVirtualFileSystemProvider virtualFileSystem,
            IAppSettingsRepository appSettings) : base(logger, engine, appSettings)
        {
            this.documentService = documentService;
            this.documentSearchService = documentSearchRepository;
            this.virtualFileSystem = virtualFileSystem;
        }



        public override async Task<TaskItem> PaletteItem()
        {
            var inputs = new List<InputParameter>
            {
                new InputParameter
                {
                    Name = "TargetDocumentId",
                    Required = true,
                    InputType = 0,
                    Label = "Id Documento Scelto",
                    Description = "Id per il Documento Scelto",
                    Values = "",
                    DefaultValue = "DocumentId"
                },
                new InputParameter
                {
                    Name = "InputFile",
                    Required = true,
                    InputType = 0,
                    Label = "Input File",
                    Description = "Percorso al file di Input",
                    Values = "",
                    DefaultValue = "\"\""
                },
                new InputParameter
                {
                    Name = "DeleteInputFile",
                    Required = false,
                    InputType = 0,
                    Label = "Cancellare File di Input",
                    Description = "Specifica se eliminare (=1) il file Input dopo l'elaborazione o lasciarlo sul file system (=0)",
                    Values = "",
                    DefaultValue = "0"
                }
            };
            var outputs = new List<OutputParameter>
            {
                new OutputParameter
                {
                    Name = "Content",
                    DefaultValue = "",
                    Required = false,
                    Label = "Contenuto",
                    Description = "Informazioni del Contenuto"
                },
                new OutputParameter
                {
                    Name = "errorMessage",
                    DefaultValue = "",
                    Required = false,
                    Label = "Messaggio di Errore",
                    Description = "Messaggio di errore in caso di fallimento"
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
            var jobKey = job.Key;
            string variables = "";
            var docId = (int?)JObject.Parse(job.Variables)["TaskDocumentId"] ?? 0;
            //if (docId == 0) docId = (int?)JObject.Parse(job.Variables)["DocumentId"] ?? 0;
            if (docId > 0)
            {

                string InputFile = (string?)JObject.Parse(job.Variables)["InputFile"] ?? "";
                var fm = await virtualFileSystem.InstanceOf((await appSettings.Get("Documents.FileSystemType")) + "");
                //var basePath = (await appSettings.Get("Documents.TempFolder")) + "";
                //if (string.IsNullOrEmpty(basePath))
                //{
                //    basePath = "Temp";
                //}
                if (!String.IsNullOrEmpty(InputFile) && await fm.Exists(InputFile))
                {
                    var Data = await fm.ReadAllBytes(InputFile);
                    int i = 0;
                    int.TryParse((string)JObject.Parse(job.Variables)["DeleteInputFile"], out i);
                    bool delete = i > 0;
                    if (delete) await fm.Delete(InputFile);
                    UserProfile u = UserProfile.SystemUser();
                    var content = new FileContent()
                    {
                        DataIsInBase64 = false,
                        FileData = System.Text.Encoding.UTF8.GetString(Data),
                        FileName = InputFile,
                        LinkToContent = false
                    };
                    var image = await documentService.AddContent(docId, u, content, true);
                    variables = System.Text.Json.JsonSerializer.Serialize(new { Content = image });
                }
                else
                    variables = System.Text.Json.JsonSerializer.Serialize(new { errorMessage = $"File '{InputFile}' non trovato" });

            }
            else
                variables = System.Text.Json.JsonSerializer.Serialize(new { errorMessage = $"Documento non trovato" });
            return variables;
        }
    }
}
