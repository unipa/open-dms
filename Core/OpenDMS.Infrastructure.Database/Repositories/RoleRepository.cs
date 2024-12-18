using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Entities.OrganizationUnits;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;


namespace OpenDMS.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext DS;
    private readonly IApplicationDbContextFactory contextFactory;

    public RoleRepository(IApplicationDbContextFactory contextFactory)
    {
        this.contextFactory = contextFactory;
        this.DS = (ApplicationDbContext)contextFactory.GetDbContext();
    }



    public async Task<Role> Insert(Role role)
    {
        DS.Roles.Add(role);
        var r = await DS.SaveChangesAsync();
        return r > 0 ? role : null;
    }

    public async Task<Role> Rename(Role role)
    {
        DS.Entry<Role>(role).State = EntityState.Modified;
        var r = await DS.SaveChangesAsync();
        return r > 0 ? role : null;
    }

    public async Task<int> Delete(Role role)
    {
        DS.Entry<Role>(role).State = EntityState.Modified;
        role.Deleted = true;
        var r = await DS.SaveChangesAsync();
        return r;
    }
    public async Task<int> Delete(string roleId)
    {
        var role = await DS.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
        if (role == null) return 0;
        DS.Entry<Role>(role).State = EntityState.Modified;
        role.Deleted = true;
        var r = await DS.SaveChangesAsync();
        return r;
    }
    public async Task<int> Restore(Role role)
    {
        DS.Entry<Role>(role).State = EntityState.Modified;
        role.Deleted = false;
        var r = await DS.SaveChangesAsync();
        return r;
    }
    public async Task<int> Restore(string roleId)
    {
        var role = await DS.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
        if (role == null) return 0;
        DS.Entry<Role>(role).State = EntityState.Modified;
        role.Deleted = false;
        var r = await DS.SaveChangesAsync();
        return r;
    }

    public async Task<Role> GetById(string roleId)
    {
        return await DS.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == roleId);
    }



    public async Task<Role> GetByName(string roleName)
    {
        return await DS.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.RoleName == roleName);
    }




    public async Task<List<Role>> GetAll(bool IncludeDeleted = false)
    {
        return await DS.Roles.AsNoTracking().Where(r => IncludeDeleted || !r.Deleted).ToListAsync();
    }

    public async Task<List<Role>> Find(string SearchText, int MaxResults = 0)
    {
        var items = DS.Roles.AsNoTracking().Where(r => r.RoleName.Contains(SearchText));
        if (MaxResults > 0) items = items.Take(MaxResults);
        return await items.OrderBy(u => u.RoleName).ToListAsync();
    }

}