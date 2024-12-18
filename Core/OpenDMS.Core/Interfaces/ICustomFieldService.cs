using OpenDMS.Domain.Entities.Schemas;

namespace OpenDMS.Core.Interfaces
{
    public interface ICustomFieldService
    {
        Task<int> Delete(string Id);
        Task<IEnumerable<FieldType>> GetAll();
        Task<IEnumerable<FieldType>> GetAllTypes();
        Task<FieldType> GetById(string id);
        Task<int> Insert(FieldType bd);
        Task<int> Update(FieldType bd);
    }
}