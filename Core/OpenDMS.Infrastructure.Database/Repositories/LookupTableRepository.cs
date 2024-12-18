using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;
using System.Data;


namespace OpenDMS.Infrastructure.Repositories;

public class LookupTableRepository : ILookupTableRepository
{
    private readonly ApplicationDbContext DS;
    private IApplicationDbContextFactory contextFactory;

    public LookupTableRepository(IApplicationDbContextFactory contextFactory)
    {
        this.contextFactory = contextFactory;
        this.DS = (ApplicationDbContext)contextFactory.GetDbContext();
    }



    public async Task<LookupTable> GetById(string TableId, string Id, bool ReturnDefault=true)
    {
        var l = await DS.LookupTables.AsNoTracking().FirstOrDefaultAsync(t=>t.TableId == TableId && t.Id == Id);    
        return ReturnDefault ? (l == null ? new LookupTable() { Id=Id, TableId=TableId, Description="#"+Id } : l) : l;
    }

    public async Task<List<LookupTable>> GetAll(string id)
    {
        return await DS.LookupTables.AsNoTracking().Where(t => t.TableId == id).OrderBy(o=>o.Description).ToListAsync();
    }

    public async Task<int> Insert(LookupTable bd)
    {
        DS.LookupTables.Add(bd);
        var r = await DS.SaveChangesAsync();
        DS.Entry<LookupTable>(bd).State = EntityState.Detached;
        return r;
    }
    public async Task<int> Update(LookupTable bd)
    {
        DS.Entry<LookupTable>(bd).State = EntityState.Modified;
        var r = await DS.SaveChangesAsync();
        DS.Entry<LookupTable>(bd).State = EntityState.Detached;
        return r;
    }
    public async Task<int> Delete(LookupTable bd)
    {
        DS.Entry<LookupTable>(bd).State = EntityState.Deleted;
        var r = await DS.SaveChangesAsync();
        DS.Entry<LookupTable>(bd).State = EntityState.Detached;
        return r;
    }

}