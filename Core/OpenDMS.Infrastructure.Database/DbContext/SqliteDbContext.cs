using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenDMS.MultiTenancy.DbContexts;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Infrastructure.Database.DbContext;

public class SqliteDbContext : ApplicationDbContext
{



    //    public SqliteDbContext(Tenant tenant) : base(tenant) { }
    public SqliteDbContext(ITenantContext<Tenant> tenantContext,ILogger<MultiTenantDbContext> logger) : base(tenantContext,logger) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var tenant = tenantContext.Tenant;
        optionsBuilder
             .EnableSensitiveDataLogging()
           .UseSqlite(tenant.ConnectionString);

    }



}
