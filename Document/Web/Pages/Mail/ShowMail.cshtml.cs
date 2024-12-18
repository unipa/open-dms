using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.MailSpooler.Core.DTOs;
using OpenDMS.MailSpooler.Core.Interfaces;

namespace Web.Pages
{

    [Authorize]
    public class ShowMailModel : PageModel
    {
        private readonly IMailEntryService mailEntryService;
        private readonly IMailReaderService mailReader;
        private readonly IDocumentService documentService;
        private readonly ILoggedUserProfile userContext;
        private readonly IUserTaskService userTaskService;


 
        public MailDetails Message { get; set; }
        public List<UserTaskListItem> Tasks { get; set; } = new();
        public List<DocumentInfo> Folders { get; set; } = new();


        public ShowMailModel(
            IMailEntryService mailEntryService,
            IMailReaderService mailReader,
            IDocumentService documentService,
            ILoggedUserProfile userContext,
            IUserTaskService userTaskService)
        {
            this.mailEntryService = mailEntryService;
            this.mailReader = mailReader;
            this.documentService = documentService;
            this.userContext = userContext;
            this.userTaskService = userTaskService;
        }

   

        public async Task OnGet(int Id)
        {
            var u = userContext.Get();
            var entry = await mailEntryService.GetById(Id);
            var mailbox = await mailEntryService.GetMailBox(entry.MailboxId);
            if (mailbox.MailServer == null)
                mailbox = await mailEntryService.GetMailBox(entry.MailboxId);

            Message = await mailReader.GetMessage(mailbox, Id);
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml (Message.Body);
            Message.Body = doc.DocumentNode.OuterHtml;
            //if (entry.ImageId == 0) {
            //    var eml = await mailReader.GetMessage(mailbox,entry);

            //}
            //Message = await mailEntryService.GetMessage(Id);
            //Message = await mailReader.GetMessage(mailbox,entry);

            if (Message.DocumentId > 0)
            {
                OpenDMS.Domain.Models.TaskListFilter filters = new() { DocumentId = Message.DocumentId, Status = new() { ExecutionStatus.Assigned, ExecutionStatus.Unassigned, ExecutionStatus.Running, ExecutionStatus.Suspended }  };
                Tasks = await userTaskService.Find(filters, u);

                Folders = new List<DocumentInfo>();
                foreach (var f in await documentService.GetDocumentFolders(Message.DocumentId, u))
                    Folders.Add(await documentService.Load(f, u));

            }


        }
        public async Task<IActionResult> OnGetSetStatus(int Id, int Status)
        {
            var u = userContext.Get();
            var mail = await mailEntryService.ChangeStatus (Id, u.userId, (MailStatus)Status);
            return new JsonResult (mail);
        }

        public async Task<IActionResult> OnGetAcquire(int Id)
        {
            var u = userContext.Get();
            var entry = await mailEntryService.GetById(Id);
            var mailbox = await mailEntryService.GetMailBox(entry.MailboxId);
            MimeMessage eml = await mailReader.GetMessage(mailbox, entry);
            var mail = await mailEntryService.Update (entry, mailbox, u.userId, eml, entry.MessageDate.Value, MailStatus.Claimed);

            return new JsonResult(mail);
        }




     

    }
}
