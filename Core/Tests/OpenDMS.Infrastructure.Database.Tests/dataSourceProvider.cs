using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.Infrastructure.Repositories;
using OpenDMS.MultiTenancy;
using OpenDMS.MultiTenancy.DbContexts;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Infrastructure.Database.Tests
{
    public class dataSourceProvider_Tests
    {
        private ApplicationDbContextFactory factory;
        private DataSourceProvider repo;

        [SetUp]
        public void Setup()
        {
            var T = new Tenant() { Provider = "memory", ConnectionString = "test" };
            MasterTenantDbContextFactory mt = new MasterTenantDbContextFactory(() => { return T; });
            var tenantGetter = new TenantContext(mt);
            var loggerdb = new Microsoft.Extensions.Logging.Abstractions.NullLogger<MultiTenantDbContext>();
            factory = new ApplicationDbContextFactory(tenantGetter, loggerdb);
            repo = new DataSourceProvider(factory);
        }

        [Test]
        public async Task GetAll_ReturnAtLeastOne()
        {
            //Arrange
            var extDb = new ExternalDatasource();
            extDb.Name = "test";
            extDb.Driver = "Microsoft.Data.SqlClient";
            extDb.Connection = "test";
            extDb.UserName = "prova";
            extDb.Password = "prova!";
            //Act

            await repo.Set(null,extDb);

            var result = await repo.GetAll();

            //Assert

            Assert.IsTrue(result.Count>0);

        }

       [Test]
        public async Task Get_Empty_ReturnVoidAsync()
        {
            //Arrange

            //Act

            var result = await repo.Get("id_inesistente");

            //Assert

            Assert.IsNull(result);

        }

          [Test]
          public async Task Set_Insert_ReturnInserted()
          {
              //Arrange

              ExternalDatasource extDb = new ExternalDatasource();
              extDb.Name = "test";
              extDb.Driver = "Microsoft.Data.SqlClient";
              extDb.Connection = "data Source=.\\SQLEXPRESS;Database=Test;User Instance=false;Max Pool Size=600;TrustServerCertificate=True";
              extDb.UserName = "prova";
              extDb.Password = "prova!";

              ExternalDatasource ReadExtDb = null;

              //Act

              await repo.Set(null, extDb);

              ReadExtDb = await repo.Get(extDb.Id);

              //Assert

              Assert.AreEqual(ReadExtDb.Id, extDb.Id);

          }

            [Test]
            public async Task Set_Update_ReturnUpdatedAsync()
            {
                //Arrange

                ExternalDatasource extDb = new ExternalDatasource();
                extDb.Name = "test";
                extDb.Driver = "Microsoft.Data.SqlClient";
                extDb.Connection = "data Source=.\\SQLEXPRESS;Database=Test;User Instance=false;Max Pool Size=600;TrustServerCertificate=True";
                extDb.UserName = "prova";
                extDb.Password = "prova!";

                string New_Name = "test_updated";

                //Act

                await repo.Set(null, extDb);

                var ReadExtDb = await repo.Get( extDb.Id);

                ReadExtDb.Name = New_Name;

                await repo.Set(ReadExtDb.Id, ReadExtDb);

                var Updated_ExtDb = await repo.Get(ReadExtDb.Id);

                //Assert

                Assert.AreEqual(Updated_ExtDb.Name, New_Name);

            }

            [Test]
            public async Task Delete_ReturnNullAsync()
            {
                //Arrange

                ExternalDatasource extDb = new ExternalDatasource();
                extDb.Name = "test";
                extDb.Driver = "Microsoft.Data.SqlClient";
                extDb.Connection = "data Source=.\\SQLEXPRESS;Database=Test;User Instance=false;Max Pool Size=600;TrustServerCertificate=True";
                extDb.UserName = "prova";
                extDb.Password = "prova!";

                //Act

                await repo.Set(null, extDb);

                await repo.Delete(extDb.Id);

                var ReadExtDb = await repo.Get(extDb.Id);

                //Assert

                Assert.IsNull(ReadExtDb);

            }
           
    }
}