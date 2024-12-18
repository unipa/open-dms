using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Interfaces;
using Web.Model;

namespace Web.Pages.Shared.Components.DragAndDrop
{

    public class DragAndDrop : ViewComponent
    {
        private readonly IConfiguration _config;
        private readonly IDocumentService _documentService;
        private readonly ILoggedUserProfile _userContext;

        public DragAndDrop(IConfiguration config, IDocumentService documentService)
        {
            _config = config;
            _documentService = documentService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int DocId, bool modalView)
        {
            var VM = new DragAndDropViewModel();
            //if (Request.Query.ContainsKey("modalView"))
                VM.ModalView = modalView; // Request.Query["modalView"] == "S";
            try
            {
                if (DocId == null) throw new NullReferenceException("Almeno uno dei due parametri DocId o TypeId deve contenere un Id valido.");
                VM.DocId = DocId;

            }
            catch (Exception ex) { VM.ErrorMessage = ex.Message.ToString(); }

            return View(VM);
        }

    }

}
