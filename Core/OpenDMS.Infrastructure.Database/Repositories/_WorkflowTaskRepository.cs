using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Workflow;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Infrastructure.Repositories
{
    //public class WorkflowTaskRepository 
    //{
    //    private readonly ApplicationDbContext ds;
    //    private readonly IApplicationDbContextFactory contextFactory;

    //    public WorkflowTaskRepository(IApplicationDbContextFactory contextFactory)
    //    {
    //        this.contextFactory = contextFactory;
    //        this.ds = (ApplicationDbContext)contextFactory.GetDbContext();
    //    }

    //    public async Task<IList<CustomTaskGroup>> GetAll()
    //    {
    //        return await ds.CustomTaskGroups.Where(t=>!t.Deleted).Include(i=>i.CustomTaskItems).AsNoTracking().ToListAsync();
    //    }

    //    public async Task<CustomTaskItem> GetById(int taskId)
    //    {
    //        return await ds.CustomTaskItems.AsNoTracking().FirstOrDefaultAsync(c=>c.Id == taskId);
    //    }


    //    public async Task<int> Insert(CustomTaskGroup CustomTaskGroup)
    //    {
    //        if (String.IsNullOrEmpty(CustomTaskGroup.Name)) throw new ArgumentNullException(nameof(CustomTaskGroup.Name));
    //        ds.CustomTaskGroups.Add(CustomTaskGroup);
    //        var r = await ds.SaveChangesAsync();
    //        ds.Entry<CustomTaskGroup>(CustomTaskGroup).State = EntityState.Detached;
    //        return r;
    //    }



    //    public async Task<int> Update(CustomTaskGroup CustomTaskGroup)
    //    {
    //        ds.CustomTaskGroups.Update(CustomTaskGroup);
    //        var r = await ds.SaveChangesAsync();
    //        ds.Entry<CustomTaskGroup>(CustomTaskGroup).State = EntityState.Detached;
    //        return r;
    //    }

    //}

}
