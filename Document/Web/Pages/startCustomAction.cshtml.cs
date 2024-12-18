using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Models;
using System.Text;

namespace Web.Pages
{
    public class startCustomActionModel : PageModel
    {
        public DocumentInfo Document { get; set; } = new() { DocumentType = new() { Name = "Error", Fields = new() } };
        public string IdCifrato { get; set; } = "";

        private readonly ILoggedUserProfile userContext;
        private readonly IDocumentService documentService;

        public startCustomActionModel(ILoggedUserProfile userContext, IDocumentService documentService)
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
            IdCifrato = Convert.ToBase64String(Encoding.Unicode.GetBytes(documentId.ToString().PadLeft(8, '0')));
//            ProcessId = Request.Query["Id"].ToString();
//            ProcessInstanceKey = Request.Query["ProcessInstanceKey"].ToString();
        }
    }
}
