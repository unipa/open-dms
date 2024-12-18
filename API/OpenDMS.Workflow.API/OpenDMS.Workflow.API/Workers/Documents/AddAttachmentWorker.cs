using Newtonsoft.Json.Linq;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;

namespace OpenDMS.Workflow.API.Workers.Documents
{
    public class AddAttachmentWorker : BaseWorker
    {
        private readonly IDocumentService documentService;

        public override string JobType { get; set; } = "addAttachmentTask";
        public override string TaskLabel { get; set; } = "Aggiunge un allegato/collegato ad un Documento";
        public override string Icon { get; set; } = "fa fa-paperclip";
        public override string GroupName { get; set; } = "Documenti";
        public override string[] AlternativeTasks { get; set; } = { };

        public override string Inputs { get; set; } = "TargetDocumentId,AttachmentId,IsLink";
        public override string Outputs { get; set; } = "";

        public override int MaxJobs { get; set; } = 1;
        public override int PollingInterval { get; set; } = 1;
        public override int TimeOut { get; set; } = 60;





        public AddAttachmentWorker(
            ILogger<AddAttachmentWorker> logger,
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
                    Name = "TargetDocumentId",
                    Required = true,
                    InputType = 0,
                    Label = "Id Documento scelto",
                    Description = "Id per il documento scelto",
                    Values = "",
                    DefaultValue = "DocumentId"
                },
                new InputParameter
                {
                    Name = "AttachmentID",
                    Required = true,
                    InputType = 0,
                    Label = "Allegato da Aggiungere",
                    Description = "Id dell'allegato da identificare",
                    Values = "",
                    DefaultValue = "0"
                },
                new InputParameter
                {
                    Name = "IsLink",
                    Required = false,
                    InputType = 0,
                    Label = "Collegato",
                    Description = "Specifica se l'allegato è collegato o meno",
                    Values = "",
                    DefaultValue = "false"
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
            //        public string Inputs { get; set; } = "DocumentId,FileName,Document,Filters,MaxResults";
            var docId = (int?)JObject.Parse(job.Variables)["TaskDocumentId"] ?? 0;
            //if (docId == 0) docId = (int?)JObject.Parse(job.Variables)["DocumentId"] ?? 0;
            var IsLink = ((bool?)JObject.Parse(job.Variables)["IsLink"] ?? false);
            string errors = "";
            if (docId > 0)
            {
                UserProfile u = UserProfile.SystemUser();
                JObject jsonObject = JObject.Parse(job.Variables);
                List<int> Lista = new List<int>();
                try
                {
                    Lista = jsonObject["Attachments"].ToObject<int[]>().ToList();
                }
                catch (Exception)
                {
                    try
                    {
                        Lista = (jsonObject["Attachments"].ToObject<string>()).Split(",").ToList().Select<string, int>(s => int.Parse(s)).ToList();
                    }
                    catch (Exception ex)
                    {
                        errors += ex.Message + "\n";
                    }
                }
                if (Lista.Count == 0)
                {
                    var attachmentId = (int?)JObject.Parse(job.Variables)["TaskAttachmentId"] ?? 0;
                    if (attachmentId == 0) attachmentId = (int?)JObject.Parse(job.Variables)["AttachmentId"] ?? 0;
                    Lista.Add(attachmentId);
                }

                for (int i = 0; i < Lista.Count; i++)
                {
                    var ok = (await documentService.Links(docId, u, !IsLink)).FirstOrDefault(f => f.Id == Lista[i]) != null;
                    if (!ok)
                    {
                        try
                        {
                            ok = await documentService.AddLink(docId, Lista[i], u, !IsLink);
                        }
                        catch
                        {

                        }
                    }
                }
            }
            return "";
        }
    }
}
