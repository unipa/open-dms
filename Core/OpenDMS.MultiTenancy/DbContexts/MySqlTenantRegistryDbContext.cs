using Microsoft.EntityFrameworkCore;
using OpenDMS.MultiTenancy.Interfaces;
using System.Reflection;

namespace OpenDMS.MultiTenancy.DbContexts
{
    public class MySqlTenantRegistryDbContext : TenantRegistryDbContext
    {

        //        public MySqlTenantRegistryDbContext(ITenantGetter<Tenant> tenantGetter) : base (tenantGetter) { }

        public MySqlTenantRegistryDbContext(IMasterTenantDbContextFactory tenant) : base(tenant) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenant = tenantFactory.tenant;
            optionsBuilder
               .EnableSensitiveDataLogging()
               .UseMySql(tenant.ConnectionString, ServerVersion.AutoDetect(tenant.ConnectionString));

        }



    



    }
}
