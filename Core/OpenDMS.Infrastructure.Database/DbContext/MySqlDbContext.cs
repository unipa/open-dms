using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenDMS.Domain;
using OpenDMS.Domain.Enumerators;
using OpenDMS.MultiTenancy.DbContexts;
using OpenDMS.MultiTenancy.Interfaces;
using System.Data;

namespace OpenDMS.Infrastructure.Database.DbContext;

public class MySqlDbContext : ApplicationDbContext
{




    //    public MySqlDbContext(Tenant tenant) : base(tenant) { }

    public MySqlDbContext(ITenantContext<Tenant> tenantContext, ILogger<MultiTenantDbContext> logger) : base(tenantContext, logger) { }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var tenant = tenantContext.Tenant;
         optionsBuilder
           .EnableSensitiveDataLogging()
           .UseMySql(tenant.ConnectionString, ServerVersion.AutoDetect(tenant.ConnectionString)); //, options => options.EnableRetryOnFailure()) ;

    }

    public override int Count(string Sql)
    {
        IDbConnection connection = Database.GetDbConnection();
        try
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandTimeout = 60000;
            command.CommandType = CommandType.Text;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            command.CommandText = Sql;//.Replace(@"\", @"\\"); ;
            var o = command.ExecuteScalar();
            int r = (int)Convert.ChangeType(o, typeof(int));
            connection.Close();
            return r;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "SELECT");
            throw;
        }
        finally {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }
    }

    public override string GetSql(string Fields, string Tables, string Where, String GroupBy, String Having, String OrderBy, int Skip, int Take)
    {
        string Sql = "SELECT " + Fields + " FROM " + Tables;
        if (!String.IsNullOrEmpty(Where)) Sql += " WHERE " + Where;
        if (!String.IsNullOrEmpty(GroupBy)) Sql += " GROUP BY " + GroupBy;
        if (!String.IsNullOrEmpty(Having)) Sql += " HAVING " + Having;
        if (!String.IsNullOrEmpty(OrderBy)) Sql += " ORDER BY " + OrderBy;
        if (Take > 0)
            Sql += $" LIMIT {Skip}, {Take}";
        return Sql.Replace(@"\", @"\\");
    }

 
}
