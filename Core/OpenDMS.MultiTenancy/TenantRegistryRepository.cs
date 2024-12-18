using Microsoft.EntityFrameworkCore;
using OpenDMS.MultiTenancy.Exceptions;
using System.Data;
using OpenDMS.MultiTenancy.Interfaces;
using OpenDMS.MultiTenancy.DbContexts;
using System.Runtime.CompilerServices;

namespace OpenDMS.MultiTenancy
{
    public class TenantRegistryRepository: ITenantRegistryRepository<Tenant>
    {
        private readonly TenantRegistryDbContext context;
        private readonly IMasterTenantDbContextFactory tenantDbContextFactory;
        private readonly IApplicationDbContextFactory contextFactory;

        public TenantRegistryRepository(IMasterTenantDbContextFactory tenantDbContextFactory, IApplicationDbContextFactory contextFactory)
        {
            this.tenantDbContextFactory = tenantDbContextFactory;
            this.contextFactory = contextFactory;
            this.context = tenantDbContextFactory.GetDbContext();
        }
        public Tenant GetDefault()
        {
            return context.tenantFactory.tenant;
        }

        public Tenant? GetById(string name)
        {
            return context.tenants.FirstOrDefault(t => t.Id == (name));
        }
        public async Task<Tenant?> GetByIdAsync(string name)
        {
            return await context.tenants.FirstOrDefaultAsync(t => t.Id == name);
        }
        public IList<Tenant> GetAll(bool Online = false)
        {
            var query = context.tenants.AsQueryable();
            if (Online) query = query.Where(t => t.Offline == false);
            return query.AsNoTracking().ToList();
        }

        public async Task<int> Create(Tenant tenant)
        {
            var T = await GetByIdAsync(tenant.Id);
            if (T != null)
                throw new DuplicateNameException($"Il tenant {T.Id} è già stato registrato");
            if (string.IsNullOrEmpty(tenant.Description)) tenant.Description = tenant.Id;
            context.tenants.Add(tenant);
            if (!(await CreateDatabase(tenant)))
                throw new TenantDatabaseCreationException(tenant);
            return await context.SaveChangesAsync();
        }
        public async Task<int> ConnectOrCreate(Tenant tenant)
        {
            var T = await GetByIdAsync(tenant.Id);
            if (T != null)
                throw new DuplicateNameException($"Il tenant {T.Id} è già stato registrato");
            try
            {
                context.tenants.Add(tenant);
                if (!(await TestConnection(tenant)))
                    if (!(await CreateDatabase(tenant)))
                        throw new TenantDatabaseCreationException(tenant);
                return await context.SaveChangesAsync();
            } catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<int> TryConnect(Tenant tenant)
        {
            var T = await GetByIdAsync(tenant.Id);
            if (T != null)
                throw new DuplicateNameException($"Il tenant {T.Id} è già stato registrato");
            if (!await TestConnection(tenant))
                throw new TenantDatabaseNotFoundException(tenant);

            context.tenants.Add(tenant);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Update(Tenant tenant)
        {
            context.tenants.Update(tenant);
            return await context.SaveChangesAsync();
        }
        public async Task<int> Delete(Tenant tenant)
        {
            context.Entry<Tenant>(tenant).State = EntityState.Deleted; // .Remove(tenant);
            return await context.SaveChangesAsync();
        }
        public async Task<int> Delete(string name)
        {
            var tenant = GetById(name);
            context.Entry<Tenant>(tenant).State = EntityState.Deleted; // .Remove(tenant);
            return await context.SaveChangesAsync();
        }

        public async Task<bool> TestConnection(Tenant T)
        {
            using (var db = contextFactory.GetDbContext(T))
                try
                {
                    return await db.Database.CanConnectAsync();
                }
                catch
                {
                    return false;
                }
        }

        public async Task<bool> CreateOrUpdateDatabase(Tenant T)
        {
            using (var db = contextFactory.GetDbContext(T))
                    await db.Database.MigrateAsync();
            return await TestConnection(T);
        }

        protected async Task<bool> CreateDatabase(Tenant T)
        {
            //TenantContext tenantContext = new MultiTenancy.TenantContext(tenantDbContextFactory);
            //tenantContext.SetTenant(T);
            using (var db = contextFactory.GetDbContext(T))
            {
                await db.CreateDatabase(T);
            }
            return await TestConnection(T);
        }
    }
}
