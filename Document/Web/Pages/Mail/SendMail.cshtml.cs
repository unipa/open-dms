using com.microsoft.schemas.office.office;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;
using OpenDMS.Core.Filters;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.Utilities;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using OpenDMS.MailSpooler.Core.Interfaces;
using Web.DTOs;

namespace Web.Pages
{

    [Authorization(OpenDMS.Domain.Constants.PermissionType.Profile_CanSendMail)]
    public class SendMailModel : PageModel
    {
        private readonly IMailboxService mailboxService;
        private readonly IMailEntryService mailRepo;
        private readonly IDocumentService documentService;
        private readonly ILoggedUserProfile userContext;
        private readonly IAppSettingsRepository appSettingsRepository;
        private readonly IUserService userService;


        public SendMailModel(
            IMailboxService mailboxService,
            IMailEntryService mailRepo,
            IDocumentService documentService,
            ILoggedUserProfile userContext,
            IAppSettingsRepository appSettingsRepository,
            IUserService userService)
        {
            this.mailboxService = mailboxService;
            this.mailRepo = mailRepo;
            this.documentService = documentService;
            this.userContext = userContext;
            this.appSettingsRepository = appSettingsRepository;
            this.userService = userService;
        }

        public string Title { get; set; } = "Nuovo Messaggio...";
        public string? DocumentApi { get; set; } = "";
        public string? UserServiceApi { get; set; } = "";
        public string? MailSpoolerApi { get; set; } = "";
        public string Selected { get; set; }
        public string UserId { get; set; }
        public string EndPoint { get; set; }
        public string MailAddress { get; set; }
        public IEnumerable<FieldTypeValue> UserAddresses { get; set; }
        public bool NewMessage { get; set; } = true;
        public MailMessage_DTO Message { get; set; } = new();
        

        public async Task OnGetAsync(string? Id, string cmd="send", string Mail = "")
        {
            if (cmd is null) cmd = "";
            int n = 0;
            if (!String.IsNullOrEmpty(Id))
            {
                n = Id.Split(",", StringSplitOptions.RemoveEmptyEntries).Count();
                Selected = n.ToString() + " document" + (n > 1 ? "i" : "o");
            }
            else
                Selected = "Nuovo messaggio di posta elettronica";
            UserId = User.Identity.Name;
            var u = userContext.Get();
            Message.LinkAttachments = false;
            Message.Body = await appSettingsRepository.Get(n>1 ? NotificationConstants.CONST_TEMPLATE_MULTIPLEDOCUMENTS_MAIL : NotificationConstants.CONST_TEMPLATE_SINGLEDOCUMENT_MAIL);

            UserAddresses = (await mailboxService.GetAll(u)).Select(x =>
                    new FieldTypeValue
                    {
                        LookupValue = x.DisplayName + (x.MailServer.MailType == OpenDMS.Domain.Entities.MailType.PEC ? " (PEC)" : "" ) +"<"+x.MailAddress+">",
                        Icon = x.MailServer.MailType == OpenDMS.Domain.Entities.MailType.PEC ? "fa fa-envelope" : "fa fa-envelope-o",
                        Value = x.Id.ToString(),
                    }).ToList();
            if (UserAddresses.Count() > 0)
            {
                MailAddress = Mail;
                if (String.IsNullOrEmpty(MailAddress))
                    MailAddress = UserAddresses.ToList()[0].Value;
            }
            if (!String.IsNullOrEmpty(Id))
            {
                int nr = 0;
                int[] docs  = Newtonsoft.Json.JsonConvert.DeserializeObject<int[]>(Id);
                foreach (var d in docs)
                {
                    nr++;
                    if (d > 0)
                    {
                        switch (cmd.ToLower())
                        {
                            case "send":
                                var doc = await documentService.Load(d, u);
                                Message.Attachments.Add(doc);
                                Message.Body.Parse(doc, "Document[" + nr + "]");
//                                Message = await GetMessage(d);
                                break;
                            case "reply":
                                Title = "Rispondi";
                                Message = await ReplyToMessage(d, false);
                                break;
                            case "replytoall":
                                Title = "Rispondi a tutti";
                                Message = await ReplyToMessage(d, true);
                                break;
                            case "forward":
                                Title = "Inoltra";
                                Message = await ForwardMessage(d, false);
                                break;
                            case "forwardwithattachment":
                                Title = "Inoltra con allegati";
                                Message = await ForwardMessage(d, true);
                                break;
                            default:
                                NewMessage = false;
                                Message = await GetMessage(d);
                                break;
                        }
                    }
                }
                if (Message.Attachments.Count > 0)
                {
                    Message.IncludePDFPreview = true;
                }
            }
        }

        public async Task<MailMessage_DTO> GetMessage(int mailMessageId)
        {
            var mail = await mailRepo.GetById(mailMessageId);
            MimeMessage msg = await mailRepo.GetContent(mail);
            MailMessage_DTO MailMessage = new MailMessage_DTO();
            MailMessage.Id = mailMessageId;
            MailMessage.FromAddress = mail.InternalMailAddress;
            MailMessage.To = msg.To.Select(t => t.ToString()).ToList();
            MailMessage.CC = msg.Cc.Select(t => t.ToString()).ToList();
            MailMessage.CCr = msg.Bcc.Select(t => t.ToString()).ToList();
            MailMessage.Subject = msg.Subject;
            MailMessage.Body = msg.HtmlBody;
            MailMessage.SavedAttachments = msg.Attachments.Where(a=>a.IsAttachment).ToList();
            MailMessage.Attachments = new();
            MailMessage.IncludePDFPreview = true;
            MailAddress = mail.MailboxId.ToString();
            return MailMessage;
        }
        public async Task<MailMessage_DTO> ReplyToMessage(int mailMessageId, bool replyToAll=false)
        {
            var mail = await mailRepo.GetByDocumentId(mailMessageId);
            MimeMessage msg = await mailRepo.GetContent (mail);
            MailMessage_DTO MailMessage = new MailMessage_DTO();
            MailMessage.Id = mailMessageId;
            MailMessage.FromAddress = "";
            MailMessage.To = msg.From.Select(t => t.ToString()).ToList();
            if (replyToAll) { MailMessage.To.AddRange(msg.To.Select(t => t.ToString())); };
            MailMessage.CC = msg.Cc.Select(t => t.ToString()).ToList();
            MailMessage.CCr = msg.Bcc.Select(t => t.ToString()).ToList();

            MailMessage.Subject = "Re:" + msg.Subject;
            //TODO: includere i mittenti
            MailMessage.Body = "<br/><br/><h6>Messaggio ricevuto</h6><container>" + mailRepo.GetMailHtml(msg, true)+"</container>";
            MailMessage.SavedAttachments = new();
            MailMessage.Attachments = new();
            MailMessage.IncludePDFPreview = true;
            return MailMessage;
        }
        public async Task<MailMessage_DTO> ForwardMessage(int mailMessageId, bool IncludeAttachments)
        {
            var mail = await mailRepo.GetByDocumentId(mailMessageId);
            MimeMessage msg = await mailRepo.GetContent(mail);
            MailMessage_DTO MailMessage = new MailMessage_DTO();
            MailMessage.Id = mailMessageId;
            MailMessage.FromAddress = "";
            MailMessage.Subject = "I:" + msg.Subject;
            //TODO: includere i mittenti
            MailMessage.Body = "<br/><h6>Messaggio ricevuto</h6><br/><container>" + mailRepo.GetMailHtml(msg, true) + "</container>";
            if (IncludeAttachments)
                MailMessage.SavedAttachments = msg.Attachments.Where(a => a.IsAttachment).ToList();
            MailMessage.Attachments = new();
            MailMessage.IncludePDFPreview = true;
            return MailMessage;
        }
    }
}
