using OpenDMS.Domain.Entities.Schemas;

namespace OpenDMS.Domain.Repositories;


public interface ICustomFieldTypeRepository
{
    Task<FieldType> GetById(string id);

    Task<int> Insert(FieldType meta);
    Task<int> Update(FieldType meta);
    Task<int> Delete(FieldType meta);
    Task<int> Delete(string id);

    Task<List<FieldType>> GetAll();



}