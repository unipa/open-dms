using Newtonsoft.Json.Linq;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;

namespace OpenDMS.Workflow.API.Workers.Documents
{
    public class SendMail : BaseWorker
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
        public IMailboxService MailboxService { get; }
        public IMailSenderService MailSenderService { get; }

        public SendMail(
            ILogger<SendMail> logger,
            IWorkflowEngine engine,
            IUserService userService,
            IMailboxService mailboxService,
            IMailSenderService mailSenderService,
           IAppSettingsRepository appSettings) : base(logger, engine, appSettings)
        {
            UserService = userService;
            MailboxService = mailboxService;
            MailSenderService = mailSenderService;
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
                    DefaultValue = "\"$system$\""
                },
                new InputParameter
                {
                    Name = "SenderMail",
                    Required = false,
                    InputType = 0,
                    Label = "Email Mittente",
                    Description = "Indirizzo Email dell'utente Mittente",
                    Values = "",
                    DefaultValue = "\"\""
                },
                new InputParameter
                {
                    Name = "ToList",
                    Required = false,
                    InputType = 0,
                    Label = "Destinatari",
                    Description = "Elenco di mail destinatarie",
                    Values = "",
                    DefaultValue = "\"\""
                },
                new InputParameter
                {
                    Name = "CCList",
                    Required = false,
                    InputType = 0,
                    Label = "Destinatari CC",
                    Description = "Elenco di mail destinatarie in CC",
                    Values = "",
                    DefaultValue = "\"\""
                },
                new InputParameter
                {
                    Name = "CCrList",
                    Required = false,
                    InputType = 0,
                    Label = "Destinatari CCr",
                    Description = "Elenco di mail destinatarie CCr",
                    Values = "",
                    DefaultValue = "\"\""
                },
                new InputParameter
                {
                    Name = "Subject",
                    Required = true,
                    InputType = 0,
                    Label = "Oggetto",
                    Description = "Oggetto della mail",
                    Values = "",
                    DefaultValue = "\"\""
                },
                new InputParameter
                {
                    Name = "Body",
                    Required = true,
                    InputType = 0,
                    Label = "Corpo del messaggio",
                    Description = "Corpo del messaggio in formato HTML",
                    Values = "",
                    DefaultValue = "\"\""
                },
                new InputParameter
                {
                    Name = "Attachments",
                    Required = false,
                    InputType = 0,
                    Label = "Allegati",
                    Description = "Elenco di ID di allegati",
                    Values = "",
                    DefaultValue = "\"\""
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
            var outputs = new List<OutputParameter>();

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
               // if (docId == 0) docId = (int?)JObject.Parse(job.Variables)["DocumentId"] ?? 0;
                var status= ((string?)JObject.Parse(job.Variables)["Justification"]);

            var subject = ((string?)JObject.Parse(job.Variables)["Subject"]) ?? "";
            var body = ((string?)JObject.Parse(job.Variables)["Body"]) ?? "";
            var pdf = (((int?)JObject.Parse(job.Variables)["IncludePDFPreview"]) ?? 0) != 0;
            var send = (((int?)JObject.Parse(job.Variables)["Send"]) ?? 1) != 1;
            var interactive = (((int?)JObject.Parse(job.Variables)["Interactive"]) ?? 1) != 1;

            var sender = (string)JObject.Parse(job.Variables)["Sender"] ?? SpecialUser.SystemUser;
            var senderMail = (string)JObject.Parse(job.Variables)["SenderMail"] ?? "";
            var ToList = ((string)JObject.Parse(job.Variables)["ToList"] ?? "").Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
            var CCList = ((string)JObject.Parse(job.Variables)["CCList"] ?? "").Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
            var CCrList = ((string)JObject.Parse(job.Variables)["CCrList"] ?? "").Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
            var attachments  = ((string)JObject.Parse(job.Variables)["Attachments"] ?? "").Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(a=>int.Parse(a)).ToList();

            UserProfile senderProfile = await UserService.GetUserProfile (sender);

            var mailboxes = await MailboxService.GetAll(senderProfile);
            var mailboxid = (int?)JObject.Parse(job.Variables)["MailboxId"] ?? 0;
            if (string.IsNullOrEmpty(sender))
            {
                var umailbox = mailboxes.FirstOrDefault(m => string.Compare(m.MailAddress, senderMail, true) == 0);
                if (umailbox != null)
                {
                    sender = umailbox.UserId;
                    mailboxid = umailbox.Id;
                }
            }
            if (mailboxid == 0) {
                var _mbox = mailboxes.FirstOrDefault(m => m.UserId == sender || m.SendEnabledProfiles.Split(",").Contains(sender));
                if (_mbox == null) {
                    throw new Exception("Mailbox non valida");
                }
                mailboxid = _mbox.Id;
                if (string.IsNullOrEmpty(sender)) sender = _mbox.UserId;
            };
            var mbox = await MailboxService.GetById(mailboxid);
            var mailAddress = mbox.MailAddress;

            UserProfile u = UserProfile.SystemUser();

            var Mailbox = new CreateOrUpdateMailMessage();

            Mailbox.MailboxId = mailboxid;
            Mailbox.FromUser = sender;
            //Mailbox.FromAddress = mailAddress;
            //Mailbox.FromName: UserId,
            Mailbox.To = ToList;
            Mailbox.CC = CCList;
            Mailbox.CCr = CCrList;
            Mailbox.Subject = subject;
            Mailbox.Body = body;
            Mailbox.Attachments = attachments;
            Mailbox.LinkAttachments = false;
            Mailbox.IncludePDFPreview = pdf;
            Mailbox.SendDate = new DateTime();
            Mailbox.Interactive = interactive;
            Mailbox.AbortOnError = true;
            Mailbox.EntryId = 0;
            Mailbox.Status = send ? Domain.Entities.MailStatus.Sending : Domain.Entities.MailStatus.Draft;
            var entry = await MailSenderService.Save (Mailbox, u);
            string exitcode = "";
            if (entry == null)
                exitcode = "Errore in salvataggio mail";
            if (interactive && send && string.IsNullOrEmpty(exitcode))
            {
                var e = await MailSenderService.SendMail(entry, u);
                if (e != null && e.Status != Domain.Entities.MailStatus.Sent)
                    if (!string.IsNullOrEmpty(e.LastException))
                        exitcode = e.LastException;
                    else
                        exitcode = "Messaggio non inviato. Nessun errore disponibile";
            }

            return System.Text.Json.JsonSerializer.Serialize(new { Entry = entry, ErrorMessage = exitcode });
        }
    }
}
