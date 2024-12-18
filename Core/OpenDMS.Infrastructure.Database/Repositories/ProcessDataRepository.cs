using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Infrastructure.Database.Repositories
{

    //public class ProcessDataRepository : IProcessDataRepository
    //{


    //    private readonly ApplicationDbContext ds;
    //    private readonly IApplicationDbContextFactory contextFactory;

    //    public ProcessDataRepository(IApplicationDbContextFactory contextFactory)
    //    {
    //        this.contextFactory = contextFactory;
    //        this.ds = (ApplicationDbContext)contextFactory.GetDbContext();
    //    }


    //    public async Task DeleteByProcessId(string processId)
    //    {
    //        await ds.ProcessDataVariables.AsNoTracking().Where(v => v.ProcessId == processId).ExecuteDeleteAsync();
    //    }

    //    public async Task<ProcessPerformanceIndicator?> GetIndicator(string processInstanceId, string indicatorId)
    //    {
    //        return await ds.ProcessPerformanceIndicators.AsNoTracking().FirstOrDefaultAsync(v => v.ProcessInstanceId == processInstanceId && v.IndicatorId == indicatorId);
    //    }

    //    public async Task<List<ProcessPerformanceIndicator>> GetIndicators(string processInstanceId)
    //    {
    //        return await ds.ProcessPerformanceIndicators.AsNoTracking().Where(v => v.ProcessInstanceId == processInstanceId).ToListAsync();
    //    }

    //    public async Task<List<ProcessPerformanceIndicator>> GetIndicatorValues(string processId, string indicatorId)
    //    {
    //        return await ds.ProcessPerformanceIndicators.AsNoTracking().Where(v => v.ProcessId == processId).ToListAsync();
    //    }

    //    public async Task<ProcessDataVariable> GetVariable(string processInstanceId, string variableId)
    //    {
    //        return await ds.ProcessDataVariables.AsNoTracking().FirstOrDefaultAsync(v => v.ProcessInstanceId == processInstanceId && v.VariableId == variableId);
    //    }

    //    public async Task<List<ProcessDataVariable>> GetVariables(string processInstanceId)
    //    {
    //        return await ds.ProcessDataVariables.AsNoTracking().Where(v => v.ProcessInstanceId == processInstanceId).ToListAsync();
    //    }

    //    public async Task<List<ProcessDataVariable>> GetVariableValues(string processId, string variableId)
    //    {
    //        return await ds.ProcessDataVariables.AsNoTracking().Where(v => v.ProcessId == processId).ToListAsync();
    //    }

    //    public async Task RemoveBeforeDate(DateTime Date)
    //    {
    //        await ds.ProcessDataVariables.AsNoTracking().Where(v => v.CreationDate < Date).ExecuteDeleteAsync();
    //        await ds.ProcessPerformanceIndicators.AsNoTracking().Where(v => v.CreationDate < Date).ExecuteDeleteAsync();
    //    }

    //    public async Task SavePerformanceIndicator(ProcessPerformanceIndicator indicatorValue)
    //    {
    //        var stored = await ds.ProcessPerformanceIndicators.AsNoTracking().FirstOrDefaultAsync(v => v.ProcessInstanceId == indicatorValue.ProcessInstanceId && v.IndicatorId == indicatorValue.IndicatorId);
    //        ds.Entry<ProcessPerformanceIndicator>(indicatorValue).State = stored != null ? EntityState.Modified : EntityState.Added;
    //        try
    //        {
    //            await ds.SaveChangesAsync();
    //        }
    //        finally
    //        {
    //            ds.Entry<ProcessPerformanceIndicator>(indicatorValue).State = EntityState.Unchanged;
    //        }
    //    }
    //    public async Task IncrementPerformanceIndicator(ProcessPerformanceIndicator indicatorValue)
    //    {
    //        var stored = await ds.ProcessPerformanceIndicators.AsNoTracking().FirstOrDefaultAsync(v => v.ProcessInstanceId == indicatorValue.ProcessInstanceId && v.IndicatorId == indicatorValue.IndicatorId);
    //        if (stored != null)
    //        {
    //            indicatorValue.Value += stored.Value;
    //        }
    //        ds.Entry<ProcessPerformanceIndicator>(indicatorValue).State = stored != null ? EntityState.Modified : EntityState.Added;
    //        try
    //        {
    //            await ds.SaveChangesAsync();
    //        }
    //        finally
    //        {
    //            ds.Entry<ProcessPerformanceIndicator>(indicatorValue).State = EntityState.Unchanged;
    //        }
    //    }

    //    public async Task SaveVariable(ProcessDataVariable variable)
    //    {
    //        var stored = await ds.ProcessDataVariables.AsNoTracking().FirstOrDefaultAsync(v => v.ProcessInstanceId == variable.ProcessInstanceId && v.VariableId == variable.VariableId);
    //        ds.Entry<ProcessDataVariable>(variable).State = stored != null ? EntityState.Modified : EntityState.Added;
    //        try
    //        {
    //            await ds.SaveChangesAsync();
    //        }
    //        finally
    //        {
    //            ds.Entry<ProcessDataVariable>(variable).State = EntityState.Unchanged;
    //        }
    //    }

    //    public async Task AddPermissions(ProcessPermission permission)
    //    {
    //        var stored = await ds.ProcessPermissions.AsNoTracking().FirstOrDefaultAsync(v => v.ProcessInstanceId == permission.ProcessInstanceId && v.ProfileId == permission.ProfileId && v.ProfileType == permission.ProfileType);
    //        ds.Entry<ProcessPermission>(permission).State = stored != null ? EntityState.Modified : EntityState.Added;
    //        try
    //        {
    //            await ds.SaveChangesAsync();
    //        }
    //        finally
    //        {
    //            ds.Entry<ProcessPermission>(permission).State = EntityState.Unchanged;
    //        }
    //    }
    //    public async Task RemovePermissions(ProcessPermission permission)
    //    {
    //        var stored = await ds.ProcessPermissions.FirstOrDefaultAsync(v => v.ProcessInstanceId == permission.ProcessInstanceId && v.ProfileId == permission.ProfileId && v.ProfileType == permission.ProfileType);
    //        if (stored != null)
    //        {
    //            ds.Entry<ProcessPermission>(permission).State = EntityState.Deleted;
    //            try
    //            {
    //                await ds.SaveChangesAsync();
    //            }
    //            finally
    //            {
    //                ds.Entry<ProcessPermission>(permission).State = EntityState.Unchanged;
    //            }
    //        }
    //    }
    //}


}
