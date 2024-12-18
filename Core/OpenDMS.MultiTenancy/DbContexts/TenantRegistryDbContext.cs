using Microsoft.EntityFrameworkCore;
using OpenDMS.MultiTenancy.Interfaces;
namespace OpenDMS.MultiTenancy.DbContexts
{
    public class TenantRegistryDbContext : DbContext
    {
        private readonly ITenantContext<Tenant> tenantContext;

        //protected readonly ITenantGetter<Tenant> tenantGetter;
        public IMasterTenantDbContextFactory tenantFactory { get;  }

        public DbSet<Tenant> tenants { get; set; }

        //public TenantRegistryDbContext(ITenantContext<Tenant> tenantContext)
        //{
        //    this.tenantGetter = tenantGetter;
        //    this.tenant = tenantGetter.Tenant;
        //    this.tenantContext = tenantContext;
        //}

        public TenantRegistryDbContext(IMasterTenantDbContextFactory tenantContextFactory) {
            this.tenantFactory = tenantContextFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase("tenants");
        }


        public async Task<bool> TestConnection()
        {
            try
            {
                return Database.IsInMemory() ? true : await Database.CanConnectAsync();
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> CreateDatabase()
        {
            try
            {
                await Database.MigrateAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }




    }
}
