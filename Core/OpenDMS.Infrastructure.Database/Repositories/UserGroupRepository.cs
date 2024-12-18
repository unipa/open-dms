using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Entities.OrganizationUnits;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;
using System.Data;

namespace OpenDMS.Infrastructure.Repositories;

public class UserGroupRepository : IUserGroupRepository
{
    private IApplicationDbContextFactory contextFactory;
    private readonly ApplicationDbContext DS;


    public UserGroupRepository(IApplicationDbContextFactory contextFactory)
    {
        this.contextFactory = contextFactory;
        this.DS = (ApplicationDbContext)contextFactory.GetDbContext();
    }




    public async Task<UserGroup> GetById(string Id)
    {
        return await DS.UserGroups.AsNoTracking().FirstOrDefaultAsync(u => u.Id == Id);
    }
    public async Task<UserGroup> GetByExternalId(string Id)
    {
        return await DS.UserGroups.AsNoTracking().FirstOrDefaultAsync(u => u.ExternalId == Id);
    }

    public async Task<List<UserGroup>> GetAll(bool OnlyActiveUserGoups = true)
    {
        var groups = DS.UserGroups.AsQueryable();
        if (OnlyActiveUserGoups) groups = groups.Where(u => u.ClosingDate >= DateTime.UtcNow);
        return await groups.AsNoTracking().ToListAsync();
    }
    public async Task<List<UserGroup>> Find(string SearchText, int MaxResults = 0)
    {
        var groups = DS.UserGroups.Where(u => (!u.Closed || u.ClosingDate >= DateTime.UtcNow) && u.Name.Contains(SearchText));
        if (MaxResults > 0 ) groups = groups.Take(MaxResults);
        return await groups.AsNoTracking().OrderBy(o=>o.Name).ToListAsync();
    }


    public async Task<int> Insert(UserGroup bd)
    {
        if (String.IsNullOrEmpty(bd.Id))
            bd.Id = Guid.NewGuid().ToString(); 
        if (bd.ShortName.Length > 64) bd.ShortName = bd.ShortName.Substring(64);
        if (bd.Name.Length > 128) bd.Name = bd.Name.Substring(128);

        DS.UserGroups.Add(bd);
        return await DS.SaveChangesAsync();

    }
    public async Task<int> Update(UserGroup bd)
    {
        if (bd.ShortName.Length > 64) bd.ShortName = bd.ShortName.Substring(64);
        if (bd.Name.Length > 128) bd.Name = bd.Name.Substring(128);

        DS.Entry<UserGroup>(bd).State = EntityState.Modified;
        return await DS.SaveChangesAsync();
    }

    public async Task<int> Delete(UserGroup bd)
    {
        DS.Entry<UserGroup>(bd).State = EntityState.Deleted;
        return await DS.SaveChangesAsync();
    }




}

