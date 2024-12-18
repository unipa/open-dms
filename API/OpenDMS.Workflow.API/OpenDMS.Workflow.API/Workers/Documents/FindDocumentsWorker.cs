using Newtonsoft.Json.Linq;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;

namespace OpenDMS.Workflow.API.Workers.Documents
{
    public class FindDocumentsWorker : BaseWorker
    {
        private readonly ISearchService documentSearchService;
        private readonly IDocumentService documentService;

        public override string JobType { get; set; } = "findDocumentsTask";
        public override string TaskLabel { get; set; } = "Cerca Documenti";
        public override string Icon { get; set; } = "fa fa-search";
        public override string GroupName { get; set; } = "Documenti";
        public override string[] AlternativeTasks { get; set; } = { };

        public override string Inputs { get; set; } = "Filters,MaxResults";
        public override string Outputs { get; set; } = "Documents";

        public override int MaxJobs { get; set; } = 1;
        public override int PollingInterval { get; set; } = 1;
        public override int TimeOut { get; set; } = 60;



        public FindDocumentsWorker(
            ILogger<FindDocumentsWorker> logger,
            IWorkflowEngine engine,
            ISearchService documentSearchService,
            IDocumentService documentService,
           IAppSettingsRepository appSettings) : base(logger, engine, appSettings)
        {
            this.documentSearchService = documentSearchService;
            this.documentService = documentService;
        }



        public override async Task<TaskItem> PaletteItem()
        {

            var inputs = new List<InputParameter>
            {
                new InputParameter
                {
                    Name = "Filters",
                    Required = true,
                    InputType = 0,
                    Label = "Filtro",
                    Description = "Filtro da applicare durante la ricerca dei documenti",
                    Values = "",
                    DefaultValue = ""
                },
                new InputParameter
                {
                    Name = "MaxResults",
                    Required = false,
                    InputType = 0,
                    Label = "Risultato Massimo",
                    Description = "Numero di ricerche massimo dei risultati",
                    Values = "",
                    DefaultValue = ""
                }
            };
            var outputs = new List<OutputParameter>
            {
                new OutputParameter
                {
                    Name = "Documents",
                    DefaultValue = "",
                    Required = false,
                    Label = "Lista dei documenti",
                    Description = "Lista dei documenti che combaciano con i parametri di ricerca"
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
            var jobKey = job.Key;
            var docFilters = (List<SearchFilter>?)JObject.Parse(job.Variables)["Filters"].ToObject<List<SearchFilter>>();
            var maxResults = (int?)JObject.Parse(job.Variables)["MaxResults"] ?? 1;
            if (maxResults < 1) maxResults = 1;

            UserProfile su = UserProfile.SystemUser();
            List<DocumentInfo> newdoc = new List<DocumentInfo>();
            // Aggiorno un documento se viene specificato un ID
            var results = await documentSearchService.Find(docFilters, su, maxResults);
            logger.LogDebug(TaskLabel + ": " + job.ProcessDefinitionKey + "\\" + job.ElementId + "\\" + jobKey + " - Instance: " + job.ProcessDefinitionKey + "\\" + job.ElementInstanceKey);
            foreach (var r in results)
            {
                var doc = await documentService.Load(r, su);
                newdoc.Add(doc);
            }
            var variables = System.Text.Json.JsonSerializer.Serialize(new { Documents = newdoc });
            return variables;
        }
    }
}
