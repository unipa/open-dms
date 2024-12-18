using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenDMS.MultiTenancy.DbContexts;
using OpenDMS.MultiTenancy.Interfaces;
using System.Data;

namespace OpenDMS.Infrastructure.Database.DbContext;

public class OracleDbContext : ApplicationDbContext
{



    //   public OracleDbContext(Tenant tenant) : base(tenant) { }
    public OracleDbContext(ITenantContext<Tenant> tenantContext, ILogger<MultiTenantDbContext> logger) : base(tenantContext, logger) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var tenant = tenantContext.Tenant;
        optionsBuilder
           .EnableSensitiveDataLogging()
           .UseOracle(tenant.ConnectionString);

    }

    public virtual string FormatDate(DateTime Date)
    {
        return "TO_DATE('"+ Date.ToString("yyyy-MM-dd")+ "','YYYY-MM-DD')";
    }
    public override string GetSql(string Fields, string Tables, string Where, String GroupBy, String Having, String OrderBy, int Skip, int Take)
    {
        string Sql = "SELECT " + Fields + " FROM " + Tables;
        if (!String.IsNullOrEmpty(Where)) Sql += " WHERE " + Where;
        if (!String.IsNullOrEmpty(GroupBy)) Sql += " GROUP BY " + GroupBy;
        if (!String.IsNullOrEmpty(Having)) Sql += " HAVING " + Having;
        if (!String.IsNullOrEmpty(OrderBy)) Sql += " ORDER BY " + OrderBy;
        if (Take > 0)
            Sql += $" OFFSET {Skip} ROWS FETCH NEXT {Take} ROWS ONLY";
        return Sql;
    }
 
}
