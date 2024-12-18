using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.Infrastructure.Repositories;
using OpenDMS.MultiTenancy;
using OpenDMS.MultiTenancy.DbContexts;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Infrastructure.Database.Tests
{
    public class documentSearchRepository_Tests
    {
        private ApplicationDbContextFactory factory;
        //private DocumentSearchRepository repo;

        [SetUp]
        public void Setup()
        {
            var T = new Tenant() { Provider = "memory", ConnectionString = "test" };
            MasterTenantDbContextFactory mt = new MasterTenantDbContextFactory(() => { return T; });
            var tenantGetter = new TenantContext(mt);
            var loggerdb = new Microsoft.Extensions.Logging.Abstractions.NullLogger<MultiTenantDbContext>();
            factory = new ApplicationDbContextFactory(tenantGetter, loggerdb);
        }

       // [Test]
        public async Task Search()
        {
            UserProfile u = UserProfile.SystemUser();
            List<SearchFilter> filters = new List<SearchFilter>
            {
                new SearchFilter() { ColumnName="Description", Operator=OperatorType.EqualTo, Values = new List<string> { "Documento con file 1" } }
            };
            List<string> columns = new List<string> { "Descrizione", "tipodocumento", "numerodocumento" };
            List<SortingType> orderby = new List<SortingType> {
                SortingType.Descending,
                SortingType.None,
                SortingType.None,
                SortingType.None,
                SortingType.None
            };
            //var old = await repo.GetPage(u,filters, columns, orderby, 0,1);
            //Assert.IsTrue(old != null);
        }
    }
}