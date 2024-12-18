using OpenDMS.MultiTenancy.DbContexts;

namespace OpenDMS.MultiTenancy.Interfaces
{
    public interface IMasterTenantDbContextFactory
    {
        Tenant tenant { get; }

        TenantRegistryDbContext GetDbContext(string AssemblyName = "");
    }
}