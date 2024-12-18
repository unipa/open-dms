using Elmi.Core.DataAccess.Dialects;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Net.Sockets;

namespace Elmi.Core.DataAccess.Tests
{
    public class DatabaseSchemaBuilderTests
    {

        /// 
        /// docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=SqlServer20!7" -p 1433:1433 -d -e MSSQL_PID="Developer" -e MSSQL_AGENT_ENABLED="true"  --name mssql mcr.microsoft.com/mssql/server
        /// 
        /// docker run -e "MYSQL_ROOT_PASSWORD=SqlServer20!7" -p:3306:3306 -d  --name mysql mysql
        /// 
        /// docker run -d -p 1521:1521 -e ORACLE_PASSWORD=SqlServer20!7 gvenzl/oracle-xe
        /// 



        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Creare un database con una tabella, un indice primario, un indice univoco, un indice non univoco
        /// </summary>
        [Test]
        public void CreateDatabaseWithAllFieldTypesSqlServer()
        {
            DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);
            var builder = new SchemaBuilder("data Source=.;Database=test_001;user id=sa;password=SqlServer20!7;User Instance=false;Max Pool Size=600;TrustServerCertificate=True", "Microsoft.Data.SqlClient");
            builder
                .Drop()
                .Create()
                .CreateTable("prova", "")
                    .AddIdentity("id", "")
                    .AddString("stringa", "", 10, false, "x")
                    .AddString("stringa_vuota", "", 20, true, "")
                    .AddInteger("intero", "", false, 1)
                    .AddDecimal("decimale", "", 18, 7, false, (decimal)99.9)
                    .AddDate("data", "", true)
                    .AddBoolean("bit", "", true)
                    .AddText("testo", "", true)
                    .AddPrimaryKey("id","stringa")
                    .AddUniqueIndex("uidx2", "id", "stringa")
                    .AddUniqueIndex("uidx", "intero", "stringa")
                    .AddIndex("idx", "intero")
                    .View("vwprova", "SELECT intero, stringa,data,bit from prova")
                .Build();
            Assert.Pass();
        }
        [Test]
        public void CreateDatabaseWithAllFieldTypesOracle()
        {
            Oracle12cDialect.ORACLE_SYSTEM_PASSWORD = "SqlServer20!7";
            DbProviderFactories.RegisterFactory("Oracle.ManagedDataAccess.Client", Oracle.ManagedDataAccess.Client.OracleClientFactory.Instance);
            var builder = new SchemaBuilder("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=XEPDB1)));user id=test_001;password=SqlServer20!7", "Oracle.ManagedDataAccess.Client");
            builder
                 .Drop()
                .Create()
                .CreateTable("prova", "")
                    .AddIdentity("id", "")
                    .AddString("stringa", "", 10, false, "x")
                    .AddString("stringa_vuota", "", 20, true, "")
                    .AddInteger("intero", "", false, 1)
                    .AddDecimal("decimale", "", 18, 7, false, (decimal)99.9)
                    .AddDate("data", "", true)
                    .AddBoolean("bit", "", true)
                    .AddText("testo", "", true)
                    .AddPrimaryKey("id", "stringa")
                    .AddUniqueIndex("uidx2", "id", "stringa")
                    .AddUniqueIndex("uidx", "intero", "stringa")
                    .AddIndex("idx", "intero")
                    .View("vwprova", "SELECT intero, stringa,data,bit from prova")
                .Build();
            Assert.Pass();
        }
        [Test]
        public void CreateDatabaseWithAllFieldTypesMySql()
        {
            DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySql.Data.MySqlClient.MySqlClientFactory.Instance);
            var builder = new SchemaBuilder("server=localhost;Database=test_001;uid=root;pwd=SqlServer20!7;", "MySql.Data.MySqlClient");
            builder
                .Drop()
                .Create()
                .CreateTable("prova", "")
                    .AddIdentity("id", "")
                    .AddString("stringa", "", 10, false, "x")
                    .AddString("stringa_vuota", "", 20, true, "")
                    .AddInteger("intero", "", false, 1)
                    .AddDecimal("decimale", "", 18, 7, false, (decimal)99.9)
                    .AddDate("data", "", true)
                    .AddBoolean("bit", "", true)
                    .AddText("testo", "", true)
                    .AddPrimaryKey("id", "stringa")
                    .AddUniqueIndex("uidx2", "id", "stringa")
                    .AddUniqueIndex("uidx", "intero", "stringa")
                    .AddIndex("idx", "intero")
                    .View("vwprova", "SELECT intero, stringa,data,bit from prova")
                .Build();
            Assert.Pass();
        }

        [Test]
        public void UpdateDatabaseWithAllFieldTypesSqlServer()
        {
            DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);
            var builder = new SchemaBuilder("data Source=.;Database=test_001;user id=sa;password=SqlServer20!7;User Instance=false;Max Pool Size=600;TrustServerCertificate=True", "Microsoft.Data.SqlClient");
            var sql = builder
                .Create()
                .CreateTable("prova", "")
                    .AddIdentity("id", "")
                    .AddString("stringa", "", 10, false, "x")
                    .AddString("stringa_vuota", "", 20, true, "")
                    .AddInteger("intero", "", false, 1)
                    .AddDecimal("decimale", "", 18, 7, false, (decimal)99.9)
                    .AddDate("data", "", true)
                    .AddBoolean("bit", "", true)
                    .AddText("testo", "", true)
                    .AddPrimaryKey("id", "stringa")
                    .AddUniqueIndex("uidx2", "id", "stringa")
                    .AddUniqueIndex("uidx", "intero", "stringa")
                    .AddIndex("idx", "intero")
                    .View("vwprova", "SELECT intero, stringa,data,bit from prova")
                .ToString();
            Assert.IsEmpty(sql.Trim());
        }
        [Test]
        public void UpdateDatabaseWithAllFieldTypesOracle()
        {
            Oracle12cDialect.ORACLE_SYSTEM_PASSWORD = "SqlServer20!7";
            DbProviderFactories.RegisterFactory("Oracle.ManagedDataAccess.Client", Oracle.ManagedDataAccess.Client.OracleClientFactory.Instance);
            var builder = new SchemaBuilder("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=XEPDB1)));user id=test_001;password=SqlServer20!7", "Oracle.ManagedDataAccess.Client");
            var sql = builder
                .Create()
                .CreateTable("prova", "")
                    .AddIdentity("id", "")
                    .AddString("stringa", "", 10, false, "x")
                    .AddString("stringa_vuota", "", 20, true, "")
                    .AddInteger("intero", "", false, 1)
                    .AddDecimal("decimale", "", 18, 7, false, (decimal)99.9)
                    .AddDate("data", "", true)
                    .AddBoolean("bit", "", true)
                    .AddText("testo", "", true)
                    .AddPrimaryKey("id", "stringa")
                    .AddUniqueIndex("uidx2", "id", "stringa")
                    .AddUniqueIndex("uidx", "intero", "stringa")
                    .AddIndex("idx", "intero")
                    .View("vwprova", "SELECT intero, stringa,data,bit from prova")
                .ToString();
            Assert.IsEmpty(sql.Trim());
        }
        [Test]
        public void UpdateDatabaseWithAllFieldTypesMySql()
        {
            DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySql.Data.MySqlClient.MySqlClientFactory.Instance);
            var builder = new SchemaBuilder("server=localhost;Database=test_001;uid=root;pwd=SqlServer20!7;", "MySql.Data.MySqlClient");
            var sql = builder
                .Create()
                .CreateTable("prova", "")
                    .AddIdentity("id", "")
                    .AddString("stringa", "", 10, false, "x")
                    .AddString("stringa_vuota", "", 20, true, "")
                    .AddInteger("intero", "", false, 1)
                    .AddDecimal("decimale", "", 18, 7, false, (decimal)99.9)
                    .AddDate("data", "", true)
                    .AddBoolean("bit", "", true)
                    .AddText("testo", "", true)
                    .AddPrimaryKey("id", "stringa")
                    .AddUniqueIndex("uidx2", "id", "stringa")
                    .AddUniqueIndex("uidx", "intero", "stringa")
                    .AddIndex("idx", "intero")
                    .View("vwprova", "SELECT intero, stringa,data,bit from prova")
                .ToString();
            Assert.IsEmpty(sql.Trim());
        }



    }
}