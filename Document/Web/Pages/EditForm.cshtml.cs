using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using System.Text;

namespace Web.Pages
{
    [Authorize]
    public class EditFormModel : PageModel
    {
        private readonly ILogger<EditFormModel> _logger;
        private readonly IDocumentService documentService;
        private readonly IUserTaskService userTaskService;
        private readonly ILoggedUserProfile userContext;
        private readonly IFormService formService;

        public string ErrorMessage { get; set; }
        public DocumentInfo Document { get; set; } = new() { DocumentType = new() { Name = "Error", Fields = new() } };
        public FormSchema Form { get; set; } = new FormSchema();

        public string Title { get; set; }
        public string FileName { get; set; }


        public EditFormModel(ILogger<EditFormModel> logger, 
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


        public async Task OnGetAsync(int? documentId, string? documentTypeId, ContentType? contentType )
        {
            try
            {
                var u = userContext.Get();
                var su = UserProfile.SystemUser();
                if (documentId.HasValue && documentId > 0)
                {
                    Document = await documentService.Load(documentId.Value, u);
                    FileName = Document.DocumentTypeName;
                }
                else
                {
                    Document = await documentService.DocumentSchema(documentTypeId, u, contentType.Value);
                    FileName = Document.DocumentTypeName;
                }

                if (Document.DocumentType != null)
                {
                    if (!string.IsNullOrEmpty(Document.DocumentType.CreationFormKey))
                    {
                        Form = await formService.GetByUid(Document.DocumentType.CreationFormKey, u);
                        if (Document.Image != null)
                        {
                            var images = (await documentService.Images(documentId.Value, u)).OrderByDescending (i=>i.VersionNumber).ThenByDescending(i=>i.RevisionNumber);
                            foreach (var image in images)
                            {
                                //if (Document.Image.FileName.ToLower().EndsWith(".html"))                                
                                if (image.FileName.ToLower().EndsWith(".formhtml"))
                                {
                                    // Test: x<ab xx a>
                                    //       0123456789
                                    // i = 1
                                    // j = 7
                                    // json = (1+4, 7 - 1 - 4) = xx
                                    //var jsonString = Encoding.Default.GetString(await documentService.GetContent(Document.Image.Id));
                                    var jsonString = Encoding.Default.GetString(await documentService.GetContent(image.ImageId));
                                    var StartTag = "<!-- FORM-DATA-BEGIN ";
                                    var EndTag = " FORM-DATA-END -->";
                                    var i = jsonString.IndexOf(StartTag);
                                    var j = jsonString.IndexOf(EndTag);
                                    if (i >= 0 && j > 0)
                                    {
                                        string json = jsonString.Substring(i + StartTag.Length, j - i - StartTag.Length);
                                        Form.Data = json;
                                        break;
                                    }
                                }
                                if (image.FileName.ToLower().EndsWith(".formjs"))
                                {
                                    var jsonString = Encoding.Default.GetString(await documentService.GetContent(image.ImageId));
                                    Form.Data = jsonString;
                                }
                            }
                        }
                    } 
                }

                //                if (Document.DocumentType != null)
                //                    Form = await formService.GetByUid(Document.DocumentType.CreationFormKey, u);
                //if (Document.Id > 0)
                //{
                //    var version = await documentService.GetPublished (documentId);
                //    Form = await formService.GetByImageId(Document, version, u);
                //}
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message.ToString();
            }

        }


 

    }
}
