using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Models;

namespace OpenDMS.Core.Interfaces
{
    public interface ICompanyService
    {
        Task CheckDemo();
        Task Create(Company bd);
        Task Delete(Company bd);
        Task<IList<Company>> GetAll();
        Task<Company> GetById(int cbd);
        Task<IList<Company>> GetByUser(UserProfile userInfo);
        Task Update(Company bd);
    }
}