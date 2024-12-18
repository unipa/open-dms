using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Models;

namespace OpenDMS.Domain.Repositories;

public interface IDocumentTypeRepository
{
    Task<DocumentType> GetById(string id);
    Task<int> Insert(DocumentType docType);
    Task<int> Update(DocumentType docType);
    Task<int> Delete(string documentTypeId);
    Task<List<DocumentType>> GetAll();

    Task<int> AddField(DocumentTypeField definition);
    Task<List<DocumentType>> GetByUser(UserProfile acls);


    Task<List<DocumentTypeWorkflow>> GetAllWorkflows(string documentTypeId);
    Task<List<DocumentTypeWorkflow>> GetAllTypesWorkflows();
    Task<int> AddWorkflow(DocumentTypeWorkflow definition);
    Task<int> UpdateWorkflow(DocumentTypeWorkflow definition);
    Task<int> RemoveWorkflow(DocumentTypeWorkflow definition);
    Task<DocumentTypeWorkflow> GetWorkflow(string documentTypeId, string eventName);

}