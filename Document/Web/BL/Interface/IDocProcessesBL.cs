using Microsoft.AspNetCore.Mvc.Rendering;
using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Models;
using Web.Model.Admin;

namespace Web.BL.Interface
{
    public interface IDocProcessesBL
    {
        Task<DocumentTypeWorkflow> AggiungiDocumentTypeWorkflow(DocumentTypeWorkflow bd);
        //Task<int> CreaProcesso(string processName, UserProfile u);
        Task<int> EliminaDocumentTypeWorkflow(string TypeId, string EventName);
        Task<IEnumerable<DocumentType>> GetAllDocTypes( UserProfile u);
        Task<List<DocumentTypeWorkflow_DTO>> GetAllDocumentTypeWorkflow(string TypeId, List<DocumentType> types, List<SelectListItem> ProcessList);
        Task<DocumentType> GetDocType(string Id);
        Task<DocumentTypeWorkflow> GetDocumentTypeWorkflow(string TypeId, string EventName);
        Task<DocumentInfo> GetProcess(int Id, UserProfile u);
        Task<List<int>?> GetProcesses(UserProfile u);
        Task<int> ModificaDocumentTypeWorkflow(string TypeId, string EventName, string ProcessId);
    }
}