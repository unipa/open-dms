using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenDMS.Domain.Entities.Schemas;

namespace Web.Model.Admin
{
    public class DocProcessesViewModel
    {
        public string TypeId { get; set; }
        public string TypeName { get; set; }

        public List<DocumentType> Types { get; set; } = new List<DocumentType>();
        public List<DocumentTypeWorkflow_DTO> DocumentTypeWorkflow { get; set; } = new List<DocumentTypeWorkflow_DTO>();
        public List<SelectListItem> EventList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ProcessList { get; set; } = new List<SelectListItem>();

        [ValidateNever]
        public string ErrorMessage { get; set; }
        [ValidateNever]
        public string SuccessMessage { get; set; }
        [ValidateNever]
        public string Icon { get; set; }

    }

    public class DocumentTypeWorkflow_DTO
    {
        public DocumentTypeWorkflow_DTO(string? documentTypeId, string? documentTypeName, string? eventName, string? eventDescription, string processId, List<SelectListItem> processList)
        {
            DocumentTypeId = documentTypeId;
            DocumentTypeName = documentTypeName;
            EventName = eventName;
            ProcessKey = processId;
            ProcessList = processList;
            EventDescription = eventDescription;
        }

        public DocumentTypeWorkflow_DTO(string? documentTypeId, string? documentTypeName, string? eventName, string processId, List<SelectListItem> processList)
        {
            DocumentTypeId = documentTypeId;
            DocumentTypeName = documentTypeName;
            EventName = eventName;
            ProcessKey = processId;
            ProcessList = processList;
        }

        public List<SelectListItem> ProcessList { get; set; } = new List<SelectListItem>();
        public string? DocumentTypeId { get; set; } = "";
        public string? DocumentTypeName { get; set; } = "";
        public string? EventName { get; set; } = "";
        public string? EventDescription { get; set; } = "";
        public string ProcessKey { get; set; }
    }

    public class CreateProcessDocumentTypeWorkflow_DTO
    {
        public string DocumentTypeId { get; set; }
        public string Query { get; set; }
        public string EventName { get; set; }
        public string ProcessName { get; set; }
    }

}
