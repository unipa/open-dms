using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Tasks;
using OpenDMS.Domain.Models;

namespace OpenDMS.Domain.Repositories;


public class UserFiltersService : IUserFilterService
{
    private readonly ISearchFilterRepository _repository;

    public UserFiltersService(ISearchFilterRepository filterRepo)
    {
        this._repository = filterRepo;
    }


    public async Task<SearchFilters> GetById(int filterId)
    {
        return new SearchFilters(await _repository.GetById(filterId));
    }
    public async Task<int> Insert(SearchFilters bd)
    {
        TaskListCustomFilter uf = new TaskListCustomFilter();
        uf.SystemFilter = false;
        uf.Icon = bd.Icon;
        uf.Name = bd.Name;
        uf.UserId = bd.UserId;
        uf.RoleId = bd.RoleId;
        uf.GroupId = bd.GroupId;
        uf.SerializedFilters = System.Text.Json.JsonSerializer.Serialize(bd.Filters);
        await _repository.Insert(uf);
        return uf.Id;
    }

    public async Task<int> Rename(int filterId, string NewName)
    {
        var uf = await _repository.GetById(filterId);
        uf.Name = NewName;
        return await _repository.Update(uf) > 0 ? uf.Id : 0;
    }
    public async Task<int> Delete(int filterId)
    {
        return await _repository.Delete(filterId);
    }

    public async Task<List<SearchFilters>> GetAll(UserProfile userInfo)
    {
        List<SearchFilters> Items = new List<SearchFilters>();
        foreach (var i in await _repository.GetAll(userInfo))
        {
            SearchFilters S = new SearchFilters(i);
            Items.Add(S);
        }
        return Items;
    }

}


