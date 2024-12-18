using OpenDMS.Domain.Entities.OrganizationUnits;

namespace OpenDMS.Domain.Repositories
{
    public interface IUserGroupRepository
    {
        Task<List<UserGroup>> GetAll(bool OnlyActiveUserGoups = true);
        Task<UserGroup> GetById(string Id);
        Task<int> Insert(UserGroup bd);
        Task<int> Update(UserGroup bd);
        Task<int> Delete(UserGroup bd);
        Task<List<UserGroup>> Find(string SearchText, int MaxResults = 0);
        Task<UserGroup> GetByExternalId(string Id);
    }
}