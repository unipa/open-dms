using Microsoft.EntityFrameworkCore;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;
using OpenDMS.MultiTenancy.DbContexts;
using Microsoft.Extensions.Logging;


namespace OpenDMS.Infrastructure.Database.DbContext
{
    public class ApplicationDbContextFactory : IApplicationDbContextFactory //, IDisposable
    {
        private readonly ITenantContext<Tenant> tenantContext;
        private readonly ILogger<MultiTenantDbContext> logger;
        private MultiTenantDbContext context = null;

        public ApplicationDbContextFactory(ITenantContext<Tenant> tenantContext, ILogger<MultiTenantDbContext> logger)
        {
            this.tenantContext = tenantContext;
            this.logger = logger;
            InternalGetContext(tenantContext);
        }

        private void InternalGetContext(ITenantContext<Tenant> tenantContext)
        {
            var Provider = tenantContext.Tenant.Provider.ToLower();
            switch (Provider)
            {
                case "mysql":
                    context = new MySqlDbContext(tenantContext,logger);
                    break;
                case "sqlite":
                    context = new SqliteDbContext(tenantContext, logger);
                    break;
                case "memory":
                    context = new ApplicationDbContext(tenantContext, logger);
                    break;
                case "oracle":
                    context = new OracleDbContext(tenantContext, logger);
                    break;
                case "mssql":
                case "sqlserver":
                    context = new SqlServerDbContext(tenantContext, logger);
                    break;
                default:
                    throw new KeyNotFoundException(nameof(Provider) + $" {Provider} non trovato");
            }
        }

        //public void Dispose()
        //{
        //    if (context != null)
        //        context.Dispose();
        //}

        //public ApplicationDbContextFactory(Tenant tenant)
        //{
        //    this.tenant = tenant;
        //}

        public MultiTenantDbContext GetDbContext(string AssemblyName = "")
        {
            //if (context != null) return context; // context.Dispose();
  
            return context;
        }

        public MultiTenantDbContext GetDbContext(Tenant tenant, string AssemblyName = "")
        {
            if (context != null) context.Dispose();
            tenantContext.SetTenant(tenant);
            InternalGetContext(tenantContext);
            return context;
        }
    }
}
