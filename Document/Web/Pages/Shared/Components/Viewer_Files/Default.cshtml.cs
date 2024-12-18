using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Models;

namespace Web.Pages.Shared.Components.Viewer_Files
{
    public class Viewer_Files : ViewComponent
    {
        private readonly IDocumentService documentService;
        private readonly ILoggedUserProfile userContext;
        private readonly IViewManager viewManager;
        private readonly IViewServiceResolver viewMapper;


        public IndexModel Data { get; set; }

        public Viewer_Files(IDocumentService documentService,
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
//            sr.FileFilters.Request.OrderBy
            return View(sr);
        }


    }
}
