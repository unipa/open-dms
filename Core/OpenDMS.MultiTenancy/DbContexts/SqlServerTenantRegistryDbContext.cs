using Microsoft.EntityFrameworkCore;
using OpenDMS.MultiTenancy.Interfaces;
using System.Reflection;

namespace OpenDMS.MultiTenancy.DbContexts
{
    public class SqlServerTenantRegistryDbContext : TenantRegistryDbContext
    {
 //       public SqlServerTenantRegistryDbContext(ITenantGetter<Tenant> tenantGetter) : base(tenantGetter) { }
        public SqlServerTenantRegistryDbContext(IMasterTenantDbContextFactory tenant) : base(tenant) { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenant = tenantFactory.tenant;
            optionsBuilder
                //.EnableSensitiveDataLogging()
                .UseSqlServer(tenant.ConnectionString);

        }



    

    }
}
