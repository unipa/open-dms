using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Models;
using System.Text;

namespace Web.Pages
{
    public class PostSaveModel : PageModel
    {
        public DocumentInfo Document { get; set; } = new() { DocumentType = new() { Name = "Error", Fields = new() } };
        public string IdCifrato { get; set; } = "";

        private readonly ILoggedUserProfile userContext;
        private readonly IDocumentService documentService;

        public PostSaveModel(ILoggedUserProfile userContext, IDocumentService documentService)
        {
            this.userContext = userContext;
            this.documentService = documentService;
        }

        public async Task OnGet()
        {
            if (!int.TryParse(Request.Query["Id"].ToString(), out int documentId) || documentId <= 0)
                RedirectToAction("/ErrorPage?ErrorMessage=Parametro non valido");

            var u = userContext.Get();
            var su = UserProfile.SystemUser();
            Document = await documentService.Load(documentId, su);
            var Folders = await documentService.GetDocumentFolders(Document.Id, u);
            if (Folders.Count > 0)
            {
                var f = await documentService.Load(Folders[Folders.Count - 1], u);
                Document.Path = f.Path;
                Document.Path.Add(new LookupTable() { Id = f.Id.ToString(), Description = f.Description });
            };

            IdCifrato = documentId.ToString("X") + (Document.Image != null ? "-" + Document.Image.Id.ToString("X").PadLeft(10, '0') : "");
//            ProcessId = Request.Query["Id"].ToString();
//            ProcessInstanceKey = Request.Query["ProcessInstanceKey"].ToString();
        }
    }
}
