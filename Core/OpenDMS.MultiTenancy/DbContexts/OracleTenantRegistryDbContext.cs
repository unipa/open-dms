using Microsoft.EntityFrameworkCore;
using OpenDMS.MultiTenancy.Interfaces;
using System.Reflection;

namespace OpenDMS.MultiTenancy.DbContexts
{
    public class OracleTenantRegistryDbContext : TenantRegistryDbContext
    {

 //       public OracleTenantRegistryDbContext(ITenantGetter<Tenant> tenantGetter) : base (tenantGetter) { }
        public OracleTenantRegistryDbContext(IMasterTenantDbContextFactory tenant) : base(tenant) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenant = tenantFactory.tenant;
            optionsBuilder
                 .EnableSensitiveDataLogging()
                .UseOracle(tenant.ConnectionString);

        }



 

    }
}
