using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Models;

namespace OpenDMS.Core.Interfaces
{
    public interface IDocumentTypeService
    {
        Task<int> Create(DocumentType docType);
        Task<int> Delete(DocumentType docType);
        Task<DocumentType> GetById(string codice);
        Task<List<DocumentType>> GetByPermission(UserProfile userInfo, string permissionId, string parent = ".", string filter = "");
        Task<DocumentType> GetByPermission(string Id, UserProfile userInfo, string permissionId);
        Task<List<DocumentType>> Select();
        Task<List<LookupTable>> SelectClasses(List<DocumentType> Tipi);
        Task<int> Update(DocumentType docType);

        Task<bool> CanCreateRootFolder(UserProfile userInfo);


        Task<List<DocumentTypeWorkflow>> GetAllWorkflows(string documentTypeId);
        Task<List<DocumentTypeWorkflow>> GetAllTypesWorkflows();
        Task<int> AddWorkflow(DocumentTypeWorkflow definition);
        Task<int> UpdateWorkflow(DocumentTypeWorkflow definition);
        Task<int> RemoveWorkflow(DocumentTypeWorkflow definition);
        Task<DocumentTypeWorkflow> GetWorkflow(string documentTypeId, string eventName);
    }
}