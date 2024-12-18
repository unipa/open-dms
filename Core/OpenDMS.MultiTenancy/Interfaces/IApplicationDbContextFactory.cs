using Microsoft.EntityFrameworkCore;
using OpenDMS.MultiTenancy.DbContexts;

namespace OpenDMS.MultiTenancy.Interfaces
{
    public interface IApplicationDbContextFactory
    {
        MultiTenantDbContext GetDbContext(string AssemblyName = "");
        MultiTenantDbContext GetDbContext(Tenant tenant, string AssemblyName = "");
    }
}