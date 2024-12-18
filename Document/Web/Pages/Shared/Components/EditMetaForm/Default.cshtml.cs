using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.DTOs;

namespace Web.Pages.Shared.Components.EditMetaForm
{
    public class EditMetaForm : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync(DocumentInfo document)
        {
            return View(document);
        }
    }

}
