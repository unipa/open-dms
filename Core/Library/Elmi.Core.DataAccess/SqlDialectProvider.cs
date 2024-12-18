using Elmi.Core.DataAccess.Dialects;
using Elmi.Core.DataAccess.Interfaces.Dialects;

namespace Elmi.Core.DataAccess
{
    public class SqlDialectProvider
    {
        private static Dictionary<string, ISqlDialect> Dialects = new Dictionary<string, ISqlDialect>();

        public static void Register(string providername, ISqlDialect Dialect)
        {
            var p = providername.ToLower();
            if (!Dialects.ContainsKey(p)) Dialects[p] = Dialect;
        }

        public static ISqlDialect Get(string provider) 
        {
            if (!Dialects.ContainsKey("microsoft.data.sqlclient"))
                Register("Microsoft.Data.SqlClient", new SqlServerDialect());
            if (!Dialects.ContainsKey("system.data.sqlclient"))
                Register("System.Data.SqlClient", new SqlServerDialect());
            if (!Dialects.ContainsKey("system.data.sqllite"))
                Register("System.Data.SQLite", new SqlServerDialect());


            if (!Dialects.ContainsKey("system.data.oracleclient"))
                Register("System.Data.OracleClient", new Oracle12cDialect());
            if (!Dialects.ContainsKey("oracle.manageddataaccess.client"))
                Register("Oracle.ManagedDataAccess.Client", new Oracle12cDialect());

            if (!Dialects.ContainsKey("system.data.mysql"))
                Register("system.data.mysql", new MySqlDialect());

            if (!Dialects.ContainsKey("mysql.data.mysqlclient"))
                Register("MySql.Data.MySqlClient", new MySqlDialect());


            string lowerprovider = provider.ToLower();
            return  Dialects.ContainsKey(lowerprovider) ? Dialects[lowerprovider] : Dialects["microsoft.data.sqlclient"];
        }
    }
}
