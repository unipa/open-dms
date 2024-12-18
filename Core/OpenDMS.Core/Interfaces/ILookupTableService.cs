using OpenDMS.Domain.Entities.Settings;

namespace OpenDMS.Core.Interfaces
{
    public interface ILookupTableService
    {
        Task<int> Delete(LookupTable bd);
        Task<List<LookupTable>> GetAll();
        Task<List<LookupTable>> GetAll(string id);
        Task<LookupTable> GetById(string tabella, string codice, bool ReturnDefault = true);
        Task<int> Insert(LookupTable bd);
        Task<int> Update(LookupTable bd);
    }
}