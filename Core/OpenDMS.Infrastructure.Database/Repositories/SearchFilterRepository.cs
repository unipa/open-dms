using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Models;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Domain.Repositories;


/// <summary>
/// Descrizione di riepilogo per BancheDatiDAO
/// </summary>
public class SearchFilterRepository : ISearchFilterRepository
{

    private readonly ApplicationDbContext ds;
    private readonly IApplicationDbContextFactory contextFactory;

    public SearchFilterRepository(IApplicationDbContextFactory contextFactory)
    {
        this.contextFactory = contextFactory;
        this.ds = (ApplicationDbContext)contextFactory.GetDbContext();
    }

    public async Task<Entities.Tasks.TaskListCustomFilter> GetById(int filterId)
    {
        return await ds.UserFilters.AsNoTracking().FirstOrDefaultAsync(u=>u.Id == filterId);
    }
    public async Task<int> Insert(Entities.Tasks.TaskListCustomFilter bd)
    {
        ds.UserFilters.Add(bd);
        var r = await ds.SaveChangesAsync();
        ds.Entry<Entities.Tasks.TaskListCustomFilter>(bd).State = EntityState.Detached;
        return r;
    }
    public async Task<int> Update(Entities.Tasks.TaskListCustomFilter bd)
    {
        ds.UserFilters.Update(bd);
        var r = await ds.SaveChangesAsync();
        ds.Entry<Entities.Tasks.TaskListCustomFilter>(bd).State = EntityState.Detached;
        return r;
    }
    public async Task<int> Delete(int filterId)
    {
        var f = ds.UserFilters.Find(filterId);
        ds.UserFilters.Remove(f);
        var r = await ds.SaveChangesAsync();
        ds.Entry<Entities.Tasks.TaskListCustomFilter>(f).State = EntityState.Detached;
        return r;
    }
    public async Task<IList<Entities.Tasks.TaskListCustomFilter>> GetAll(UserProfile userInfo)
    {
        return await ds.UserFilters.AsNoTracking().Where(u => u.UserId == userInfo.userId || userInfo.GlobalRoles.Select(s => s.Id).Contains(u.RoleId) || userInfo.Roles.Select(s => s.Id).Contains(u.RoleId) || userInfo.Groups.Select(s => s.Id).Contains(u.GroupId)).ToListAsync();
    }

}


