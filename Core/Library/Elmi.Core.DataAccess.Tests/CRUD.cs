using Elmi.Core.DataAccess.Dialects;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Elmi.Core.DataAccess.Tests
{
    public class CRUDTests
    {
        public string Provider { get; private set; }
        public string ConnectionString { get; private set; }

        private SchemaBuilder builder;

        /// <summary>
        ///  Creare un database di test su SQLLite in memoria
        ///  e caricare 10 righe
        /// </summary>
        [SetUp]
        public void Setup()
        {
            DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);
            Provider = "Microsoft.Data.SqlClient";
            ConnectionString = "data Source=.;Database=test_001;user id=sa;password=SqlServer20!7;User Instance=false;Max Pool Size=600;TrustServerCertificate=True";

            //DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySql.Data.MySqlClient.MySqlClientFactory.Instance);
            //Provider = "MySql.Data.MySqlClient";
            //ConnectionString = "server=localhost;Database=test_001;uid=root;pwd=SqlServer20!7;";

            //Oracle12cDialect.ORACLE_SYSTEM_PASSWORD = "SqlServer20!7";
            //DbProviderFactories.RegisterFactory("Oracle.ManagedDataAccess.Client", Oracle.ManagedDataAccess.Client.OracleClientFactory.Instance);
            //Provider = "Oracle.ManagedDataAccess.Client";
            //ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=XEPDB1)));user id=test_001;password=SqlServer20!7";
            builder = new SchemaBuilder(ConnectionString, Provider);
            builder
           //     .Drop()
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
                   .AddIndex("idx", "intero")
                   .View("vwprova", "SELECT intero, stringa,data,bit from prova")
               .Build();
        }
        public class riga { public string id { get; set; } public string payload { get; set; } public string ProcessStart { get; set; } };

        [Test]
        public void Insert()
        {

            int r = 0;
            try
            {
                DataSource DS = new DataSource(ConnectionString, Provider);
                DS.Delete("prova");
                r = DS.InsertWithOutput("prova", "Id"
                    , DS.Parameter("stringa", "uno")
                    , DS.Parameter("stringa_vuota", "uno vuoto")
                    , DS.Parameter("intero", DbType.Int32, 999)
                    , DS.Parameter("decimale", DbType.Decimal, 99.9)
                    , DS.Parameter("data", DbType.DateTime, DateTime.UtcNow)
                    , DS.Parameter("bit", DbType.Int32, 1)
                    , DS.Parameter("testo", "b4ddc423e046: Pull complete\r\nb338d8e4ffd1: Pull complete\r\nb2b1b06949ab: Pull complete\r\ndaf393284da9: Pull complete\r\n1cb8337ae65d: Pull complete\r\nf6c2cc79221c: Pull complete\r\n4cec461351e0: Pull complete\r\nab6bf0cba08e: Pull complete\r\n8df43cafbd11: Pull complete\r\nc6d0aac53df5: Pull complete\r\nb24148c7c251: Pull complete")
                    );
                Console.WriteLine($"r={r}");
                r = DS.InsertWithOutput("prova", "Id"
                    , DS.Parameter("stringa", "due")
                    , DS.Parameter("stringa_vuota", "")
                    , DS.Parameter("intero", DbType.Int32, 999)
                    , DS.Parameter("decimale", DbType.Decimal, 99.9)
                    , DS.Parameter("data", DbType.DateTime, DateTime.UtcNow)
                    , DS.Parameter("bit", DbType.Int32, 0)
                    , DS.Parameter("testo", "b4ddc423e046: Pull complete\r\nb338d8e4ffd1: Pull complete\r\nb2b1b06949ab: Pull complete\r\ndaf393284da9: Pull complete\r\n1cb8337ae65d: Pull complete\r\nf6c2cc79221c: Pull complete\r\n4cec461351e0: Pull complete\r\nab6bf0cba08e: Pull complete\r\n8df43cafbd11: Pull complete\r\nc6d0aac53df5: Pull complete\r\nb24148c7c251: Pull complete")
                    );
                Console.WriteLine($"r={r}");
                DS.SaveChanges();
            }
            catch (Exception ex)
            {
                Assert.Fail();
                r = 0;
            }
            Assert.AreEqual(r, 1);
        }

        [Test]
        public void InsertError()
        {
            try
            {
                DataSource DS = new DataSource(ConnectionString, Provider);
                int r = DS.Insert("prova"
                    , DS.Parameter("stringa", "due")
                    , DS.Parameter("stringa_vuota", "due vuoto")
                    , DS.Parameter("decimale", DbType.Decimal, 99.9)
                    , DS.Parameter("data", DbType.DateTime, DateTime.UtcNow)
                    , DS.Parameter("bit", DbType.Int32, 1)
                    , DS.Parameter("testo", "b4ddc423e046: Pull complete\r\nb338d8e4ffd1: Pull complete\r\nb2b1b06949ab: Pull complete\r\ndaf393284da9: Pull complete\r\n1cb8337ae65d: Pull complete\r\nf6c2cc79221c: Pull complete\r\n4cec461351e0: Pull complete\r\nab6bf0cba08e: Pull complete\r\n8df43cafbd11: Pull complete\r\nc6d0aac53df5: Pull complete\r\nb24148c7c251: Pull complete")
                    );
                DS.SaveChanges();
                Assert.Fail();
            }
            catch {
                Assert.Pass();
            }

            Assert.Pass();
        }


        [Test]
        public void SelectEmptyRow()
        {
            int r = 0;
            try
            {
                DataSource DS = new DataSource(ConnectionString, Provider);
                r = DS.ReadInteger("SELECT COUNT(*) FROM PROVA WHERE stringa_vuota<>''");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.IsTrue(r>=1);
        }


        [Test]
        public void SelectTop()
        {
            int r = 0;
            try
            {
                DataSource DS = new DataSource(ConnectionString, Provider);
                using (var dr = DS.Select("SELECT * FROM PROVA WHERE stringa_vuota<>''", 1))
                {
                    while (dr.Read())
                        r++;
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.IsTrue(r == 1);
        }


        [Test]
        public void Update()
        {
            try
            {
                DataSource DS = new DataSource(ConnectionString, Provider);
                int r = DS.Update ("prova",2
                    , DS.Parameter("stringa_vuota", "tre nuovo vuoto")
                    , DS.Parameter("decimale", DbType.Decimal, 99.9)
                    , DS.Parameter("data", DbType.Date, DateTime.UtcNow)
                    , DS.Parameter("bit", DbType.Int32, 1)
                    , DS.Parameter("testo", "b4ddc423e046: Pull complete\r\nb338d8e4ffd1: Pull complete\r\nb2b1b06949ab: Pull complete\r\ndaf393284da9: Pull complete\r\n1cb8337ae65d: Pull complete\r\nf6c2cc79221c: Pull complete\r\n4cec461351e0: Pull complete\r\nab6bf0cba08e: Pull complete\r\n8df43cafbd11: Pull complete\r\nc6d0aac53df5: Pull complete\r\nb24148c7c251: Pull complete")
                    , DS.Parameter("intero", DbType.Decimal, 1)
                    , DS.Parameter("stringa", "due")
                    );
                DS.SaveChanges();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            Assert.Pass();
        }

        /// <summary>
        /// Testare una transazione a buon fine 
        /// </summary>
        [Test]
        public void GoodTransaction()
        {
            Assert.Pass();
        }

        [Test]
        public void Delete()
        {
            DataSource DS = new DataSource(ConnectionString, Provider);
            DS.Delete("PROVA");
            DS.SaveChanges();
            Assert.Pass();
        }
        /// <summary>
        /// Provare leggere la seconda pagina di 5, sapendo quali record dovrà restituire come primo e ultimo
        /// </summary>
        [Test]
        public void Page()
        {
            Assert.Pass();
        }
    }
}