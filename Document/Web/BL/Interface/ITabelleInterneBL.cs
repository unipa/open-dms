using OpenDMS.Domain.Entities.Settings;

namespace Web.BL.Interface
{
    public interface ITabelleInterneBL
    {
        Task Delete(LookupTable bd);
        Task<LookupTable> GetLookupTable(string TableId, string Id);
        Task<List<LookupTable>> GetLookupTables(string TableId);
        Task<List<LookupTable>> GetTables();
        Task Insert(LookupTable bd);
        Task Update(LookupTable bd);
    }
}