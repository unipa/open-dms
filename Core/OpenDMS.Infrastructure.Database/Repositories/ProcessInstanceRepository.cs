using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Infrastructure.Database.Repositories
{

    public class ProcessInstanceRepository : IProcessInstanceRepository
    {

        private readonly ApplicationDbContext ds;
        private readonly IApplicationDbContextFactory contextFactory;

        public ProcessInstanceRepository(IApplicationDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
            this.ds = (ApplicationDbContext)contextFactory.GetDbContext();
        }


        public async Task Delete(int documentId)
        {
            await ds.ProcessInstances.AsNoTracking().Where(v => v.DocumentId== documentId).ExecuteDeleteAsync();
        }


        public async Task Start (ProcessInstance processInstance)
        {
            ds.Entry<ProcessInstance>(processInstance).State = EntityState.Added;
            try
            {
                await ds.SaveChangesAsync();
            }
            finally
            {
                ds.Entry<ProcessInstance>(processInstance).State = EntityState.Unchanged;
            }
        }
        public async Task Stop(ProcessInstance processInstance)
        {
            ds.Entry<ProcessInstance>(processInstance).State = EntityState.Added;
            try
            {
                await ds.SaveChangesAsync();
            }
            finally
            {
                ds.Entry<ProcessInstance>(processInstance).State = EntityState.Unchanged;
            }
        }
        public async Task<ProcessInstance> GetByProcessInstanceId(string processInstanceId)
        {
            return await ds.ProcessInstances.AsNoTracking().FirstOrDefaultAsync(t=>t.ProcessInstanceId == processInstanceId);
        }
        public async Task<List<ProcessInstance>> GetByProcessDefinitionId(int processDefinitionId)
        {
            return await ds.ProcessInstances.AsNoTracking().Where(t => t.ProcessDefinitionId == processDefinitionId).OrderBy(o => o.StartDate).ToListAsync();
        }
        public async Task<List<ProcessInstance>> GetByProcessKey(string processKey)
        {
            return await ds.ProcessInstances.AsNoTracking().Where(t => t.ProcessKey == processKey).OrderBy(o => o.StartDate).ToListAsync();
        }
        public async Task<List<ProcessInstance>> GetByDocumentId(int documentId)
        {
            return await ds.ProcessInstances.AsNoTracking().Where(t => t.DocumentId == documentId).OrderBy(o=>o.StartDate).ToListAsync();
        }

    }


}
