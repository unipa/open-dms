using OpenDMS.Domain.Models;

namespace OpenDMS.Core.Interfaces
{
    public interface IUserFilterService
    {
        Task<int> Delete(int filterId);
        Task<List<SearchFilters>> GetAll(UserProfile userInfo);
        Task<SearchFilters> GetById(int filterId);
        Task<int> Insert(SearchFilters bd);
        Task<int> Rename(int filterId, string NewName);
    }
}