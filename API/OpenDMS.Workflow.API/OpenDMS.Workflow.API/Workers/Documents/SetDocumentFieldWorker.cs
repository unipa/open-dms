using Newtonsoft.Json.Linq;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;

namespace OpenDMS.Workflow.API.Workers.Documents
{
    public class SetDocumentFieldWorker : BaseWorker
    {
        private readonly IDocumentService documentService;
        private readonly IDocumentTypeService documentTypeService;

        public override string JobType { get; set; } = "setDocumentFieldTask";
        public override string TaskLabel { get; set; } = "Imposta uno o più metadati di un Documento";
        public override string Icon { get; set; } = "fa fa-tag";
        public  override string GroupName { get; set; } = "Documenti";
        public override string[] AlternativeTasks { get; set; } = { };

        public override string Inputs { get; set; } = "TaskDocumentId,Fields";
        public override string Outputs { get; set; } = "";

        public override int MaxJobs { get; set; } = 1;
        public override int PollingInterval { get; set; } = 1;
        public override int TimeOut { get; set; } = 60;





        public SetDocumentFieldWorker(
            ILogger<SetDocumentFieldWorker> logger,
            IWorkflowEngine engine,
            IDocumentService documentService,
            IDocumentTypeService documentTypeService,
        IAppSettingsRepository appSettings) : base(logger, engine, appSettings)
        {
            this.documentService = documentService;
            this.documentTypeService = documentTypeService;
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
                    Label = "Id Documento Attività",
                    Description = "Id del Documento",
                    Values = "",
                    DefaultValue = "DocumentId"
                },
                new InputParameter
                {
                    Name = "Fields",
                    Required = true,
                    InputType = 0,
                    Label = "Campi",
                    Description = "Lista di campi da impostare",
                    Values = "",
                    DefaultValue = "[ { FieldName: \"\", Value: \"\"} ]"
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
            var IsTemp = false;
            var docData = (List<AddOrUpdateDocumentField>?)JObject.Parse(job.Variables)["Fields"]?.ToObject<List<AddOrUpdateDocumentField>>();
            if (docData != null)
            {
                // se è specificato il contenuto, ma solo il nome del file, vedo se lo trovo sul filesystem
                var docId = (int?)JObject.Parse(job.Variables)["TaskDocumentId"] ?? 0;
                // if (docId == 0) docId = (int?)JObject.Parse(job.Variables)["DocumentId"] ?? 0;
                UserProfile u = UserProfile.SystemUser();
                var doc = await documentService.Load(docId, u);
                bool ok = (!String.IsNullOrEmpty(doc.DocumentType.Id));
                if (ok)
                {
                    foreach (var f in docData)
                    {
                        DocumentTypeField docTypeField = doc.DocumentType.Fields == null ? null : doc.DocumentType.Fields.FirstOrDefault(t => t.FieldName != null &&  t.FieldName.Equals(f.FieldName, StringComparison.InvariantCultureIgnoreCase));
                        if (docTypeField == null)
                        {
                            docTypeField = new DocumentTypeField()
                            {
                                DocumentTypeId = doc.DocumentType.Id,
                                FieldName = f.FieldName,
                                FieldTypeId = f.FieldTypeId ?? "$$t",
                                FieldIndex = (doc.DocumentType.Fields == null ? 0 : doc.DocumentType.Fields.Select(ft => ft.FieldIndex).DefaultIfEmpty().Max()) + 1,
                                Editable = true,
                                Mandatory = false,
                                Title = f.FieldName,
                                Width = "100px",
                            };
                            doc.DocumentType.Fields.Add(docTypeField);
                            await documentTypeService.Update(doc.DocumentType);
                            await documentService.AddField(doc, docTypeField, f.Value);
                        }
                        else
                        {
                            await documentService.UpdateField(doc, docTypeField, f.Value);
                        }
                    }
                }
            }
            return "";
        }
    }
}
