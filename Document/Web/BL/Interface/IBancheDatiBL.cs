using OpenDMS.Domain.Entities.Settings;

namespace Web.BL.Interface
{
    public interface IBancheDatiBL
    {
        Task Delete(Company bd);
        Task<IEnumerable<Company>> GetAll();
        Task<Company> GetById(int cdb);
        Task Insert(Company bd);
        Task Update(Company bd);
    }
}