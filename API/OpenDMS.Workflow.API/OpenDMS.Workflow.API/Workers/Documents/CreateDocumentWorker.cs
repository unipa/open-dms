using Newtonsoft.Json.Linq;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;

namespace OpenDMS.Workflow.API.Workers.Documents
{
    public class CreateDocumentWorker : BaseWorker
    {
        private readonly IDocumentService documentService;
        private readonly ISearchService documentSearchService;
        private readonly IVirtualFileSystemProvider virtualFileSystem;

        public override string JobType { get; set; } = "createDocumentTask";
        public override string TaskLabel { get; set; } = "Crea o modifica un Documento";
        public override string Icon { get; set; } = "fa fa-plus-square-o";
        public override string GroupName { get; set; } = "Documenti";
        public override string[] AlternativeTasks { get; set; } = { };

        public override string Inputs { get; set; } = "DocumentId,Document,Filters,MaxResults,DeleteInputFile";
        public override string Outputs { get; set; } = "Document";

        public override int MaxJobs { get; set; } = 1;
        public override int PollingInterval { get; set; } = 1;
        public override int TimeOut { get; set; } = 60;





        public CreateDocumentWorker(
            ILogger<CreateDocumentWorker> logger,
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
                    Name = "TaskDocumentId",
                    Required = false,
                    InputType = 0,
                    Label = "Id Documento",
                    Description = "Identificativo per il documento da Aggiornare.<br/>Se non indicato o lasciato a '0' sarà creato un nuovo documento",
                    Values = "",
                    DefaultValue = "0"
                },
                new InputParameter
                {
                    Name = "Document",
                    Required = true,
                    InputType = 0,
                    Label = "Document",
                    Description = "Dati del documento da Aggiornare o Creare",
                    Values = "",
                    DefaultValue = "{ DocumentTypeId: \"\", ContentType: 1, Description: \"\", DocumentNumber:\"\", Owner: \"$system$\", ExternalId: \"\" }"
                },
                new InputParameter
                {
                    Name = "Filters",
                    Required = false,
                    InputType = 0,
                    Label = "Filtro",
                    Description = "Filtro per cercare un documento da aggiornare (in sostituzione di TaskDocumentId)",
                    Values = "",
                    DefaultValue = "[ { columnName: \"Document.ExternalId\", operator: \"EqualTo\", values: [ \"\" ] } ]"
                },
                new InputParameter
                {
                    Name = "MaxResults",
                    Required = false,
                    InputType = 0,
                    Label = "Risutato Massimo",
                    Description = "Numero massimo di risultati",
                    Values = "",
                    DefaultValue = "1"
                },
                new InputParameter
                {
                    Name = "DeleteInputFile",
                    Required = false,
                    InputType = 0,
                    Label = "Input del file da cancellare",
                    Description = "Indica se il file di input è da eliminare o meno alla fine della lavorazione",
                    Values = "",
                    DefaultValue = "1"
                }

            };
            var outputs = new List<OutputParameter>
            {
                new OutputParameter
                {
                    Name = "NewDocumentId",
                    DefaultValue = "NewDocumentId",
                    Required = false,
                    Label = " Id del Documento",
                    Description = "Id del documento creato o aggiornato"
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

        public override async Task<string> Execute(IJob job)
        {
            var nid = 0;
            var docData = (CreateOrUpdateDocument?)JObject.Parse(job.Variables)["Document"]?.ToObject<CreateOrUpdateDocument>();
            if (docData != null)
            {
                //                    var userId = (string?)JObject.Parse(job.Variables)["UserId"]?.ToObject<string>();
                UserProfile u = UserProfile.SystemUser();
                // se è specificato il contenuto, ma solo il nome del file, vedo se lo trovo sul filesystem
                if (docData.Content != null && !String.IsNullOrEmpty(docData.Content.FileName) && string.IsNullOrEmpty(docData.Content.FileData))
                {
                    var fm = await virtualFileSystem.InstanceOf((await appSettings.Get("Documents.FileSystemType")) + "");
                    var basePath = (await appSettings.Get("Documents.TempFolder")) + "";
                    if (string.IsNullOrEmpty(basePath))
                    {
                        basePath = "Temp";
                    }
                    if (await fm.Exists(docData.Content.FileName))
                    {
                        docData.Content.FileData = Convert.ToBase64String(await fm.ReadAllBytes(docData.Content.FileName));
                        docData.Content.DataIsInBase64 = true;
                        int i = 0;
                        int.TryParse((string)JObject.Parse(job.Variables)["DeleteInputFile"], out i);
                        bool delete = (i > 0) || docData.Content.FileName.StartsWith(basePath);
                        if (delete) await fm.Delete(docData.Content.FileName);
                    }
                }

                var docId = (int?)JObject.Parse(job.Variables)["TaskDocumentId"] ?? 0;
                //if (docId == 0) docId = (int?)JObject.Parse(job.Variables)["DocumentId"] ?? 0;
                var docFilters = (List<SearchFilter>)JObject.Parse(job.Variables)["Filters"]?.ToObject<List<SearchFilter>>();
                var maxResults = (int?)JObject.Parse(job.Variables)["MaxResults"] ?? 1;

                DocumentInfo newdoc = new DocumentInfo();
                // Aggiorno un documento se viene specificato un ID
                if (docId > 0)
                {
                    newdoc = (await documentService.Update(docId, docData, u));
                }
                else
                {
                    // Cerco "MaxResults" documenti, se vengno indicati dei filtri, e aggiorno quelli che trovo
                    if (docFilters != null && docFilters.Count > 0)
                    {
                        if (!docFilters.Any(f=>f.ColumnName == DocumentColumn.Status))
                        {
                            docFilters.Add(new SearchFilter() { ColumnName = DocumentColumn.Status, Operator = OperatorType.EqualTo, Values = { ((int)DocumentStatus.Active).ToString() } });
                        }
                        var results = await documentSearchService.Find(docFilters, u, maxResults);
                        if (results != null && results.Count == 1)
                            newdoc = (await documentService.Update(results[0], docData, u));
                        else
                            newdoc = (await documentService.CreateAndRead(docData, u));
                    }
                    else
                    {
                        // Creo un documento 
                        newdoc = (await documentService.CreateAndRead(docData, u));
                    }
                }
                nid = newdoc.Id;
            }
            var variables = System.Text.Json.JsonSerializer.Serialize(new { NewDocumentId = nid });
            return variables;
        }
    }
}
