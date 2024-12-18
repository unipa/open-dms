//using Newtonsoft.Json.Linq;
//using OpenDMS.Core.DTOs;
//using OpenDMS.Core.Interfaces;
//using OpenDMS.Domain.Entities.Schemas;
//using OpenDMS.Domain.Models;
//using OpenDMS.Domain.Repositories;
//using OpenDMS.Workflow.API.BusinessLogic;
//using Zeebe.Client.Api.Responses;
//using Zeebe.Client.Api.Worker;

//namespace OpenDMS.Workflow.API.Workers.Database
//{
//    public class SaveProcessIndicatorWorker : BaseWorker
//    {
//        private readonly ILogger<SaveProcessIndicatorWorker> logger;
//        private readonly IWorkflowEngine client;
//        private readonly IDocumentService processDataService;
//        private readonly IDocumentTypeService documentTypeService;
//        private readonly IAppSettingsRepository appSettings;

//        public string JobType { get; set; } = "SaveProcessIndicatorTask";
//        public string TaskLabel { get; set; } = "Indicatore di Processo";
//        public string Icon { get; set; } = "fa fa-sort-ascending";
//        public string GroupName { get; set; } = "Analisi";
//        public string[] AlternativeTasks { get; set; } = new[] { "" };

//        public string Inputs { get; set; } = "FieldName,Value";
//        public string Outputs { get; set; } = "";

//        public int MaxJobs { get; set; } = 1;
//        public int PollingInterval { get; set; } = 1;
//        public int TimeOut { get; set; } = 86400;



//        public SaveProcessIndicatorWorker(
//            ILogger<SaveProcessIndicatorWorker> logger,
//            IWorkflowEngine engine,
//            IDocumentService  documentService,
//            IDocumentTypeService documentTypeService,
//            IAppSettingsRepository appSettings) : base (appSettings)
//        {
//            this.logger = logger;
//            client = engine;
//            this.processDataService = documentService;
//            this.documentTypeService = documentTypeService;
//            this.appSettings = appSettings;
//        }


  
//        public override async Task HandleJob(IJobClient jobClient, IJob job)
//        {
//            // business logic
//            // 
//            var jobKey = job.Key;
//            var processId = job.ProcessDefinitionKey.ToString();
//            var processkey = job.BpmnProcessId.ToString();

//            var docId = (int?)JObject.Parse(job.Variables)["DocumentId"] ?? 0;


//            logger.LogDebug(TaskLabel + ": " + job.ProcessDefinitionKey + "\\" + job.ElementId + "\\" + job.Key + " - Instance: " + job.ProcessDefinitionKey + "\\" + job.ElementInstanceKey);
            
//            var processInstanceId = (int)(JObject.Parse(job.Variables)["ProcessId"]?.ToObject<int>());
            
//            var IndicatorId = JObject.Parse(job.Variables)["FieldName"]?.ToObject<string>();
//            var IndicatorValue = JObject.Parse(job.Variables)["Value"]?.ToObject<string>();
//            if (IndicatorId != null && !string.IsNullOrWhiteSpace(IndicatorId) && decimal.TryParse(IndicatorValue, out decimal value))
//            {
//                try
//                {
//                    var doc = await processDataService.Load(processInstanceId, UserProfile.SystemUser());
//                    if (doc.DocumentType != null)
//                    {
//                        var Field = doc.DocumentType?.Fields.FirstOrDefault(f => f.FieldName.Equals(IndicatorValue));
//                        if (Field == null)
//                        {
//                            //TODO: Creo il campo nella tipologia
//                            Field = new DocumentTypeField()
//                            {
//                                DefaultValue = "0",
//                                FieldName = IndicatorId,
//                                DocumentTypeId = doc.DocumentType?.Id,
//                                Editable = false,
//                                Deleted = false,
//                                FieldTypeId = "$$f",
//                                Mandatory = false,
//                                Tag = false,
//                                Title = IndicatorId,
//                                Width = "120px",
//                                Preservable = false,
//                                FieldIndex = doc.DocumentType.Fields.Select(ft => ft.FieldIndex).DefaultIfEmpty().Max() + 1,
//                            };
//                            doc.DocumentType?.Fields.Add(Field);
//                            await documentTypeService.Update (doc.DocumentType);
//                            await processDataService.AddField (doc, Field,  IndicatorValue);
//                        }
//                        else
//                        {
//                            await processDataService.UpdateField(doc, Field, IndicatorValue);
//                        }
//                    }
//                }
//                    catch (Exception ex)
//                    {
//                        logger.LogError(ex, $"SaveIndicatorWorker (PID:{job.BpmnProcessId}, PIK:{job.ProcessInstanceKey})");
//                    }
//            }

//            try
//            {
//                await jobClient.NewCompleteJobCommand(job).Send();
//            }
//            catch (Exception ex)
//            {
//                await jobClient.NewThrowErrorCommand(jobKey)
//                    .ErrorCode(ex.Message)
//                    .Send();
//            }

//        }
//    }
//}
