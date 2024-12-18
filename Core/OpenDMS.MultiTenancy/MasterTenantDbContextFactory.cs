using OpenDMS.MultiTenancy.Interfaces;
using OpenDMS.MultiTenancy.DbContexts;
using Microsoft.Extensions.Options;

namespace OpenDMS.MultiTenancy
{
    public class MasterTenantDbContextFactory : IMasterTenantDbContextFactory
    {
        public Tenant tenant { get; internal set; } = default!;
        private TenantRegistryDbContext c = null;
        public MasterTenantDbContextFactory(Func<Tenant> T)
        {
            tenant = T();    
        }

        public TenantRegistryDbContext GetDbContext(string AssemblyName = "")
        {
            var Provider = tenant.Provider.ToLower();
            if (c != null) return c;
            switch (Provider)
            {
                case "mysql":
                    c= new MySqlTenantRegistryDbContext(this);break;
                case "sqlite":
                    c = new SqliteTenantRegistryDbContext(this); break;
                case "memory":
                    c = new TenantRegistryDbContext(this); break;
                case "oracle":
                    c = new OracleTenantRegistryDbContext(this); break;
                case "mssql":
                case "sqlserver":
                    c = new SqlServerTenantRegistryDbContext(this); break;
                default:
                    throw new KeyNotFoundException(nameof(Provider) + $" {Provider} non trovato");
            }
            return c;
        }
    }
}
