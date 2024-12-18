using OpenDMS.Core.DTOs;

namespace Web.Model
{
    public class DragAndDropViewModel
    {
        public string? Type { get; set; } = "";
        public string ErrorMessage { get; set; } = "";
        public int DocId { get; set; }
        public bool ModalView { get; set; } = false;
    }
}
