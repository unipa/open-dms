using OpenDMS.Domain.Entities.Settings;

namespace OpenDMS.Domain.Repositories;


public interface ILookupTableRepository
{
    Task<LookupTable> GetById(string tableId, string id, bool ReturnDefault = true);
    Task<List<LookupTable>> GetAll(string tableId);
    Task<int> Insert(LookupTable table);
    Task<int> Update(LookupTable table);
    Task<int> Delete(LookupTable table);

}