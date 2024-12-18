using OpenDMS.Core.DTOs;

namespace Web.Model
{
    public class _EditMetaPanelViewModel
    {
        public int Id { get; set; } = 0;
        public string? Type { get; set; } = "";
        public string ErrorMessage { get; set; }

        public DocumentInfo Document { get; set; } = new() { DocumentType = new() { Name = "Error", Fields = new() } };
    }
}
