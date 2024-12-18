using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.Utilities;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;

namespace Web.Pages
{
    [Authorize]
    public class NewFormModel : PageModel
    {
        private readonly ILogger<NewFormModel> _logger;
        private readonly IDocumentService documentService;
        private readonly IUserTaskService userTaskService;
        private readonly ILoggedUserProfile userContext;
        private readonly IFormService formService;

        public string ErrorMessage { get; set; }
        public DocumentInfo Document { get; set; } = new() { DocumentType = new() { Name = "Error", Fields = new() } };
        public FormSchema Form { get; set; } = new FormSchema();


        public NewFormModel(ILogger<NewFormModel> logger, 
            IDocumentService documentService,
            IUserTaskService userTaskService,
            ILoggedUserProfile userContext,
            IFormService formService)
        {
            _logger = logger;
            this.documentService = documentService;
            this.userTaskService = userTaskService;
            this.userContext = userContext;
            this.formService = formService;
        }


        public async Task OnGetAsync(string DocumentType, ContentType ContentType = ContentType.Document, int? FolderId=0, int? TaskId=0)
        {
            try
            {
                var u = userContext.Get();
                var su = UserProfile.SystemUser();
                Document = await documentService.DocumentSchema(DocumentType, su, ContentType);
                if (ContentType != ContentType.Any)
                    Document.ContentType = ContentType;
                Document.FolderId = FolderId;


                if (TaskId.HasValue && TaskId.Value > 0)
                {
                    var task = await userTaskService.GetById(TaskId.Value, u);

                    //                    Form = task.TaskItemInfo.Form;
                    if (task.TaskItemInfo.Attachments.Count > 0)
                    {
                        Document = task.TaskItemInfo.Attachments[0];
                    }
                    if (Document.DocumentType != null)
                    {
                        Form = await formService.GetByUid(Document.DocumentType.CreationFormKey, u);
                        Form.Data = task.TaskItemInfo.Form.Data;
                    }
                }
                else
                {
                    if (Document.DocumentType != null)
                        Form = await formService.GetByUid(Document.DocumentType.CreationFormKey, u);
                }
                //if (Form != null)
                //{
                //    // Se ho indicato un documento da caricare, uso lo schema di questo
                //    if (DocumentId.HasValue && DocumentId.Value > 0)
                //    {
                //    //    var d = await documentService.Get(DocumentId.Value);
                //    //    if (d.ImageId.HasValue && d.ImageId.Value > 0)
                //    //        Form.Schema = System.Text.Encoding.UTF8.GetString(await documentService.GetContent(d.ImageId.Value));
                //    }
                //    Form.Schema = Form.Schema
                //        .Parse(Document, "Document");
                // }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message.ToString();
            }

        }


 

    }
}
