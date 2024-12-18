using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;

namespace Web.Pages
{
    [Authorize]
    public class NewDocumentModel : PageModel
    {
        private readonly ILogger<NewDocumentModel> _logger;
        private readonly IDocumentService documentService;
        private readonly ILoggedUserProfile userContext;
        private readonly IConfiguration _config;

        public string ErrorMessage { get; set; }
        public DocumentInfo Document { get; set; } = new() { DocumentType = new() { Name = "Error", Fields = new() } };



        public NewDocumentModel(ILogger<NewDocumentModel> logger, 
            IDocumentService documentService,
            ILoggedUserProfile userContext,
            IConfiguration config)
        {
            _logger = logger;
            this.documentService = documentService;
            this.userContext = userContext;
            _config = config;
        }
        public async Task OnGetAsync(string DocumentType, ContentType ContentType = ContentType.Document, int? FolderId=0)
        {
            try
            {
                var u = userContext.Get();
                 Document = await documentService.DocumentSchema(DocumentType, u, ContentType);
                Document.ContentType = ContentType;
                Document.FolderId = FolderId;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message.ToString();
            }

        }

    }
}
