using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.Infrastructure.Repositories;
using OpenDMS.MultiTenancy;
using OpenDMS.MultiTenancy.DbContexts;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Infrastructure.Database.Tests
{
    public class lookupRepositoryy_Tests
    {
        private ApplicationDbContextFactory factory;
        private LookupTableRepository repo;

        [SetUp]
        public void Setup()
        {
            var T = new Tenant() { Provider = "memory", ConnectionString = "test" };
            MasterTenantDbContextFactory mt = new MasterTenantDbContextFactory(() => { return T; });
            var tenantGetter = new TenantContext(mt);
            var loggerdb = new Microsoft.Extensions.Logging.Abstractions.NullLogger<MultiTenantDbContext>();
            factory = new ApplicationDbContextFactory(tenantGetter, loggerdb);
            repo = new LookupTableRepository(factory);
        }

        [Test]
        public async Task Add_shouldFail2()
        {
            var C = new LookupTable() { TableId="X", Id = "1", Description = "LookupTable demo 1" };
            var i = await repo.Insert(C);
            try
            {
                var C2 = new LookupTable() { TableId = "X", Id = "1", Description = "LookupTable demo 2" };
                i = await repo.Insert(C2);
            }
            catch (Exception ex)
            {
                Assert.Pass();
            }
            Assert.Fail("non ha dato errore di id duplicato");
        }


        [Test]
        public async Task Add_shouldBeOk()
        {
            var C = new LookupTable() { TableId = "X", Id = "2", Description = "LookupTable demo 1" };
            await repo.Insert(C);
            var C2 = new LookupTable() { TableId = "X", Id = "3", Description = "LookupTable demo 2" };
            Assert.IsTrue((await repo.Insert(C2)) > 0);
        }

        [Test]
        public async Task GetById_shouldBeOk()
        {
            var C = new LookupTable() { TableId = "X", Id = "4", Description = "LookupTable demo 1" };
            var i = await repo.Insert(C);
            var C2 = new LookupTable() { TableId = "X", Id = "5", Description = "LookupTable demo 2" };
            i = await repo.Insert(C2);

            var C3 = await repo.GetById("X","4");
            Assert.IsTrue(C3 != null && C3.Id == C.Id && C3.Id != C2.Id);
        }



        [Test]
        public async Task Edit_shouldBeOk()
        {
            var C = new LookupTable() { TableId = "X", Id = "6", Description = "LookupTable demo 1" };
            await repo.Insert(C);
            var C2 = new LookupTable() { TableId = "X", Id = "7", Description = "LookupTable demo 2" };
            await repo.Insert(C2);
            var C3 = await repo.GetById("X","6");
            C3.Description = "LookupTable demo 6 offline";
            Assert.IsTrue((await repo.Update(C3)) > 0);
        }

        [Test]
        public async Task GetAll_shouldreturn()
        {
            var C = new LookupTable() { TableId = "X", Id = "8", Description = "LookupTable demo 1" };
            await repo.Insert(C);
            var C2 = new LookupTable() { TableId = "X", Id = "9", Description = "LookupTable demo 2" };
            await repo.Insert(C2);
            var C3 = await repo.GetAll("X");
            Assert.IsTrue(C3.Count() >= 2);
        }
        [Test]
        public async Task Delete_shouldBeOk()
        {
            var C = new LookupTable() { TableId = "X", Id = "10", Description = "LookupTable demo 1" };
            await repo.Insert(C);
            var C2 = new LookupTable() { TableId = "X", Id = "11", Description = "LookupTable demo 2" };
            await repo.Insert(C2);
            Assert.IsTrue(await repo.Delete(C) > 0);
        }
    }
}