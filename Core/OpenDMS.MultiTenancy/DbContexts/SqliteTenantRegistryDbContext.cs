using Microsoft.EntityFrameworkCore;
using OpenDMS.MultiTenancy.Interfaces;
using System.Reflection;

namespace OpenDMS.MultiTenancy.DbContexts
{
    public class SqliteTenantRegistryDbContext : TenantRegistryDbContext
    {

        //public SqliteTenantRegistryDbContext(ITenantGetter<Tenant> tenantGetter) : base(tenantGetter) { }
        public SqliteTenantRegistryDbContext(IMasterTenantDbContextFactory tenant) : base(tenant) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenant = tenantFactory.tenant;
            optionsBuilder
                .EnableSensitiveDataLogging()
               .UseSqlite(tenant.ConnectionString);

        }



    


    }
}
