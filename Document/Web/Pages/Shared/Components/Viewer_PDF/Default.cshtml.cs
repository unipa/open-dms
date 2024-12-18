using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Models;
using Web.BL.Interface;

namespace Web.Pages.Shared.Components.Viewer_PDF
{
    public class Viewer_PDF : ViewComponent
    {
        private readonly IDocumentService documentService;
        private readonly ILoggedUserProfile userContext;
        private readonly IViewManager viewManager;
        private readonly IViewServiceResolver viewMapper;

        public IndexModel Data { get; set; }
        public string BPM_Url { get; set; }

        public Viewer_PDF(IDocumentService documentService,
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
            return View(sr);
        }


    }
}
