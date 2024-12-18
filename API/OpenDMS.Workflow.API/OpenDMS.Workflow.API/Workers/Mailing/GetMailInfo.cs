using javax.security.auth;
using Newtonsoft.Json.Linq;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using OpenDMS.MailSpooler.Core.Archiver;
using OpenDMS.MailSpooler.Core.Interfaces;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;

namespace OpenDMS.Workflow.API.Workers.Documents
{
    public class GetMailInfo : BaseWorker
    {

        public override string JobType { get; set; } = "sendMailTask";
        public override string TaskLabel { get; set; } = "Invia una Mail/PEC";
        public override string Icon { get; set; } = "fa fa-envelope-o";
        public override string GroupName { get; set; } = "Comunicazioni";
        public override string[] AlternativeTasks { get; set; } = { };

        public override string Inputs { get; set; } = "Sender,ToList,CCList,CCrList,Subject,Body,Attachments,IncludePDFPreview";
        public override string Outputs { get; set; } = "MailEntry";
        public override int MaxJobs { get; set; } = 1;
        public override int PollingInterval { get; set; } = 1;
        public override int TimeOut { get; set; } = 60;
        public IUserService UserService { get; }
        public IDocumentService documentService { get; }
        public IMailParser MailParser { get; }

        public GetMailInfo(
            ILogger<GetMailInfo> logger,
            IWorkflowEngine engine,
            IUserService userService,
            IDocumentService documentService,
            IMailParser mailParser,
           IAppSettingsRepository appSettings) : base(logger, engine, appSettings)
        {
            UserService = userService;
            this.documentService = documentService;
            MailParser = mailParser;
        }




        public override async Task<TaskItem> PaletteItem()
        {
            var inputs = new List<InputParameter>
            {
                new InputParameter
                {
                    Name = "Sender",
                    Required = false,
                    InputType = 0,
                    Label = "Id Mittente",
                    Description = "Id dell'utente Mittente",
                    Values = "",
                    DefaultValue = "'$system$'"
                },
                new InputParameter
                {
                    Name = "SenderMail",
                    Required = false,
                    InputType = 0,
                    Label = "Email Mittente",
                    Description = "Indirizzo Email dell'utente Mittente",
                    Values = "",
                    DefaultValue = "''"
                },
                new InputParameter
                {
                    Name = "ToList",
                    Required = false,
                    InputType = 0,
                    Label = "Destinatari",
                    Description = "Elenco di mail destinatarie",
                    Values = "",
                    DefaultValue = "''"
                },
                new InputParameter
                {
                    Name = "CCList",
                    Required = false,
                    InputType = 0,
                    Label = "Destinatari CC",
                    Description = "Elenco di mail destinatarie in CC",
                    Values = "",
                    DefaultValue = "''"
                },
                new InputParameter
                {
                    Name = "CCrList",
                    Required = false,
                    InputType = 0,
                    Label = "Destinatari CCr",
                    Description = "Elenco di mail destinatarie CCr",
                    Values = "",
                    DefaultValue = "''"
                },
                new InputParameter
                {
                    Name = "Subject",
                    Required = true,
                    InputType = 0,
                    Label = "Oggetto",
                    Description = "Oggetto della mail",
                    Values = "",
                    DefaultValue = ""
                },
                new InputParameter
                {
                    Name = "Body",
                    Required = true,
                    InputType = 0,
                    Label = "Corpo del messaggio",
                    Description = "Corpo del messaggio in formato HTML",
                    Values = "",
                    DefaultValue = "''"
                },
                new InputParameter
                {
                    Name = "Attachments",
                    Required = false,
                    InputType = 0,
                    Label = "Allegati",
                    Description = "Elenco di ID di allegati",
                    Values = "",
                    DefaultValue = "''"
                },
                new InputParameter
                {
                    Name = "MailBoxId",
                    Required = false,
                    InputType = 0,
                    Label = "ID MailBox",
                    Description = "ID della MailBox del mittente",
                    Values = "",
                    DefaultValue = "0"
                },
                new InputParameter
                {
                    Name = "IncludePDFPreview",
                    Required = false,
                    InputType = 0,
                    Label = "Preview PDF",
                    Description = "Include la preview PDF degli alelgati",
                    Values = "",
                    DefaultValue = "0"
                }
            };
            var outputs = new List<OutputParameter>()
            {
                new OutputParameter
                {
                    Name = "From",
                    Required = false,
                    Label = "Mittente",
                    Description = "Mittente (Id,Description,Annotation)",
                    DefaultValue = "Mittente"
                },
                new OutputParameter
                {
                    Name = "To",
                    Required = false,
                    Label = "Destinatari",
                    Description = "Elenco Destinatari (Id,Description,Annotation)",
                    DefaultValue = "Destinatari"
                },
                new OutputParameter
                {
                    Name = "CC",
                    Required = false,
                    Label = "Destinatari CC",
                    Description = "Elenco Destinatari CC (Id,Description,Annotation)",
                    DefaultValue = "DestinatariCC"
                },
                new OutputParameter
                {
                    Name = "CCr",
                    Required = false,
                    Label = "Destinatari CCr",
                    Description = "Elenco Destinatari CCr (Id,Description,Annotation)",
                    DefaultValue = "DestinatariCCr"
                },
                new OutputParameter
                {
                    Name = "Subject",
                    Required = false,
                    Label = "Oggetto",
                    Description = "Oggetto del messaggio",
                    DefaultValue = "Oggetto"
                },
                new OutputParameter
                {
                    Name = "Body",
                    Required = false,
                    Label = "Messaggio",
                    Description = "Corpo del messaggio in HTML",
                    DefaultValue = "Messaggio"
                },
                new OutputParameter
                {
                    Name = "BodyText",
                    Required = false,
                    Label = "Messaggio (Testo)",
                    Description = "Corpo del Messaggio in testo libero",
                    DefaultValue = "MessaggioTesto"
                },
                new OutputParameter
                {
                    Name = "MessageId",
                    Required = false,
                    Label = "Id Univoco",
                    Description = "ID Univoco del messaggio",
                    DefaultValue = "MessageId"
                }
            };

            var taskItem = new TaskItem
            {
                Id = this.JobType,
                TaskServiceId = this.JobType,
                Group = this.GroupName,
                Name = this.JobType,
                AuthenticationType = 0,
                JobWorker =this.JobType,
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
            var docId = (int?)JObject.Parse(job.Variables)["TaskDocumentId"] ?? 0;

            UserProfile u = UserProfile.SystemUser();

            var doc = await documentService.Get (docId);
            if (doc.ImageId.HasValue)
            {
                var data = await documentService.GetContent(doc.ImageId.Value);
                using (var M = new MemoryStream(data)) {
                    MailParser.Read(M);
                }
                logger.LogDebug(TaskLabel + ": " + job.ProcessDefinitionKey + "\\" + job.ElementId + "\\" + jobKey + " - Instance: " + job.ProcessDefinitionKey + "\\" + job.ElementInstanceKey);

                var variables = System.Text.Json.JsonSerializer.Serialize(
                    new { 
                        To = MailParser.GetToList(),
                        CC = MailParser.GetCCList(),
                        CCr = MailParser.GetCCrList(),
                        From = MailParser.GetSender(),
//                        Subject = MailParser.GetSubject(),
//                        Body = MailParser.GetBody(),
                        MessageId = MailParser.GetMessageId(),
                        Documents = MailParser.GetDocuments()
                    });
                return variables;
            }
            return "";
        }
    }
}
