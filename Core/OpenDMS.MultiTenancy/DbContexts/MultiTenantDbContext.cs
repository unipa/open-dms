
using Microsoft.EntityFrameworkCore;
using OpenDMS.MultiTenancy.Interfaces;
namespace OpenDMS.MultiTenancy.DbContexts
{
    public class MultiTenantDbContext : DbContext
    {
        protected readonly ITenantContext<Tenant> tenantContext;

        //public Tenant tenant { get; }
        //public MultiTenantDbContext(Tenant tenant)
        //{
        //    this.tenant = tenant;
        //}
        public MultiTenantDbContext(ITenantContext<Tenant> tenantContext)
        {
            this.tenantContext = tenantContext;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableSensitiveDataLogging();

            var tenant = tenantContext.Tenant;
            var Provider = tenant.Provider.ToLower();
            //Console.WriteLine($"provider: {Provider}, Connection: {tenant.ConnectionString}");
            switch (Provider)
            {
                case "mysql":
                    optionsBuilder.UseMySql(tenant.ConnectionString, ServerVersion.AutoDetect(tenant.ConnectionString));
                    break;
                case "sqlite":
                    optionsBuilder.UseSqlite(tenant.ConnectionString);
                    break;
                case "memory":
                    optionsBuilder.UseInMemoryDatabase(tenant.ConnectionString);
                    break;
                case "oracle":
                    optionsBuilder.UseOracle(tenant.ConnectionString);
                    break;
                case "mssql":
                case "sqlserver":
                    optionsBuilder.UseSqlServer(tenant.ConnectionString);
                    break;
                default:
                    throw new KeyNotFoundException(nameof(Provider) + $" {Provider} non trovato");
            }
        }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder
        //        .EnableSensitiveDataLogging()
        //        .UseInMemoryDatabase(tenant.ConnectionString);

        //}

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
        public virtual async Task<bool> CreateDatabase(Tenant T)
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
