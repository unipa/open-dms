using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Interfaces;
using System.Text;
using System.Text.RegularExpressions;

namespace Web.Pages.Shared.Components.Viewer_EML
{
    public class Viewer_EML : ViewComponent
    {
        private readonly IDocumentService documentService;
        private readonly ILoggedUserProfile userContext;
        private readonly IViewManager viewManager;
        private readonly IViewServiceResolver viewMapper;


        public IndexModel Data { get; set; }


        public Viewer_EML(IDocumentService documentService,
            ILoggedUserProfile userContext,
            IViewManager viewManager,
            IViewServiceResolver viewMapper)
        {
            this.documentService = documentService;
            this.userContext = userContext;
            this.viewManager = viewManager;
            this.viewMapper = viewMapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(IndexModel sr)
        {
            sr.FileContent = Regex.Replace(
                Encoding.UTF8.GetString(await documentService.GetContent(sr.Document.Image.Id)),
                @"</?(?i:script|embed|object|frameset|frame|iframe|meta|link|style)(.|\n|\s)*?>",
                string.Empty,
                RegexOptions.Singleline | RegexOptions.IgnoreCase
            );
            return View(sr);
        }


    }
}
