using Newtonsoft.Json.Linq;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;

namespace OpenDMS.Workflow.API.Workers.Database
{
    public class StopTimerWorker : BaseWorker
    {
        private readonly IDocumentService documentService;
        private readonly IDocumentTypeService documentTypeService;
    
        public override string JobType { get; set; } = "StopTimer";
        public override string TaskLabel { get; set; } = "Ferma Timer";
        public override string Icon { get; set; } = "fa fa-clock-o";
        public override string GroupName { get; set; } = "Analisi";
        public override string[] AlternativeTasks { get; set; } = new[] { "" };

        public override string Inputs { get; set; } = "TimerId,FieldName,ProcessId,Reset";
        public override string Outputs { get; set; } = "Duration,Timer";

        public override int MaxJobs { get; set; } = 1;
        public override int PollingInterval { get; set; } = 1;
        public override int TimeOut { get; set; } = 30;



        public StopTimerWorker(
            ILogger<StopTimerWorker> logger,
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
                    Name = "StartTime",
                    Required = true,
                    InputType = 0,
                    Label = "Ora di Avvio",
                    Description = "Ora di Avvio",
                    Values = "",
                    DefaultValue = "CurrentTime"
                },
           new InputParameter
                {
                    Name = "StopTime",
                    Required = true,
                    InputType = 0,
                    Label = "Ora di Stop",
                    Description = "Ora di Stop",
                    Values = "",
                    DefaultValue = "date and time()"
                },
                new InputParameter
                {
                    Name = "FieldName",
                    Required = true,
                    InputType = 0,
                    Label = "Campo del Nome",
                    Description = "Campo nome da aggiornare",
                    Values = "",
                    DefaultValue = "\"durata\""
                },
                new InputParameter
                {
                    Name = "ProcessId",
                    Required = false,
                    InputType = 0,
                    Label = "Id del processo",
                    Description = " Id del processo",
                    Values = "",
                    DefaultValue = "processId"
                },
                new InputParameter
                {
                    Name = "Unit",
                    Required = false,
                    InputType = 0,
                    Label = "Unit",
                    Description = "Unità di misura del tempo",
                    Values = "",
                    DefaultValue = "\"days\""
                },
                new InputParameter
                {
                    Name = "Reset",
                    Required = false,
                    InputType = 0,
                    Label = "Reset",
                    Description = "Flag che indica se il Timer va resettato",
                    Values = "",
                    DefaultValue = "1"
                }
            };

            var outputs = new List<OutputParameter>
            {
                new OutputParameter
                {
                    Name = "Duration",
                    DefaultValue = "",
                    Required = false,
                    Label = "Durata",
                    Description = "Durata del Timer"
                },
                new OutputParameter
                {
                    Name = "Timer",
                    DefaultValue = "",
                    Required = false,
                    Label = "Timer",
                    Description = "Timer"
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

        private double Unit (TimeSpan data, string unita, bool align = false)
        {
            double d = 0;
            var u = (unita ?? "").ToLower();
            switch (u)
            {
                case "s":
                case "sec":
                case "secs":
                case "second":
                case "seconds":
                    d = data.TotalSeconds;
                    break;
                case "h":
                case "hour":
                case "hours":
                    d = data.TotalHours;
                    break;
                case "m":
                case "min":
                case "mins":
                case "minute":
                case "minutes":
                    d = data.TotalMinutes;
                    break;
                case "mill":
                case "mills":
                case "millisecond":
                case "milliseconds":
                    d = data.TotalMilliseconds;
                    break;

                default:
                    d = data.TotalDays;
                    break;
            }
            if (align) { d = Math.Round(d); };
            return d;
        }

        public override async Task<string> Execute(IJob job)
        {
            // business logic
            // 
            var jobKey = job.Key;
            var processId = job.Key.ToString();
            var processkey = job.ProcessInstanceKey.ToString();//.BpmnProcessId;//.Key;            

            var StartTime = JObject.Parse(job.Variables)["StartTime"]?.ToObject<DateTime>();
            var StopTime = JObject.Parse(job.Variables)["StopTime"]?.ToObject<DateTime>();
            if (StopTime == null) StopTime = DateTime.UtcNow;// else StopTime = StopTime.Value.ToUniversalTime();
            var misura = JObject.Parse(job.Variables)["Unit"]?.ToObject<string>() ?? "";
            var round  = JObject.Parse(job.Variables)["Align"]?.ToObject<bool>() ?? true;
            var variable = "";
            if (StartTime != null)
            {
                var reset = (JObject.Parse(job.Variables)["Reset"]?.ToObject<int>() ?? 0) != 0;
                var tempo = (StopTime.Value - StartTime.Value);

                var durata = Unit (tempo, misura, round);

                var newValue = reset ? DateTime.UtcNow : StartTime.Value;
                variable = Newtonsoft.Json.JsonConvert.SerializeObject(
                    new { Duration = durata, 
                        CurrentTime = newValue 
                    });
                var IndicatorId = JObject.Parse(job.Variables)["FieldName"]?.ToObject<string>();
                if (IndicatorId != null && !String.IsNullOrWhiteSpace(IndicatorId)) { 
                    var data = new PerformanceIndicatorValue()
                    {
                        ProcessId = job.BpmnProcessId,
                        ProcessInstanceId = processkey,
                        IndicatorId = IndicatorId,
                        Value = (decimal)durata,
                    };

                    var docId = (int?)JObject.Parse(job.Variables)["ProcessId"] ?? 0;
                    UserProfile u = UserProfile.SystemUser();
                    var doc = await documentService.Load(docId, u);
                    bool ok = (!String.IsNullOrEmpty(doc.DocumentType.Id));
                    if (ok)
                    {
                        {
                            DocumentTypeField docTypeField = doc.DocumentType.Fields == null ? null : doc.DocumentType.Fields.FirstOrDefault(t => t.FieldName != null && t.FieldName.Equals(IndicatorId, StringComparison.InvariantCultureIgnoreCase));
                            if (docTypeField == null)
                            {
                                docTypeField = new DocumentTypeField()
                                {
                                    DocumentTypeId = doc.DocumentType.Id,
                                    FieldName = IndicatorId,
                                    FieldTypeId = "$$i",
                                    FieldIndex = (doc.DocumentType.Fields == null ? 0: doc.DocumentType.Fields.Select(ft => ft.FieldIndex).DefaultIfEmpty().Max()) + 1,
                                    Editable = false,
                                    Mandatory = false,
                                    Title = IndicatorId,
                                    Width = "100px",
                                    DefaultValue = "0",
                                    Deleted = false,
                                    Tag = false,
                                    Preservable = false,
                                };
                                if (doc.DocumentType.Fields == null)
                                    doc.DocumentType.Fields = new();
                                doc.DocumentType.Fields.Add(docTypeField);
                                await documentTypeService.Update(doc.DocumentType);
                                await documentService.AddField(doc, docTypeField, durata.ToString());
                            }
                            else
                            {
                                await documentService.UpdateField(doc, docTypeField, durata.ToString());
                            }
                        }
                    }
                }
            } else
            {
                variable = Newtonsoft.Json.JsonConvert.SerializeObject(
                    new
                    {
                        Duration = 0,
                        CurrentTime = DateTime.UtcNow
                    });
            }
            return variable;
        }
    }
}
