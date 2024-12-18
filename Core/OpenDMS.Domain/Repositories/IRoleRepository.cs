using OpenDMS.Domain.Entities.OrganizationUnits;
using OpenDMS.Domain.Models;

namespace OpenDMS.Domain.Repositories
{
    public interface IRoleRepository 
    {
        Task<Role> Insert(Role role);
        Task<Role> Rename(Role role);
        Task<int> Delete(Role role);
        Task<int> Delete(string roleId);
        Task<int> Restore(Role role);
        Task<int> Restore(string roleId);


        Task<List<Role>> GetAll(bool IncludeDeleted = false);
        Task<Role> GetById(string roleId);
        Task<Role> GetByName(string roleId);
        Task<List<Role>> Find(string SearchText, int MaxResults = 0);

    }

}
