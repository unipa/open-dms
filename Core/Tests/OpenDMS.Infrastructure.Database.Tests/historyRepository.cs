using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.Infrastructure.Database.Repositories;
using OpenDMS.MultiTenancy;
using OpenDMS.MultiTenancy.DbContexts;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Infrastructure.Database.Tests
{
    public class historyRepository_Tests
    {
        private ApplicationDbContextFactory factory;
        private HistoryRepository repo;

        [SetUp]
        public void Setup()
        {
            var T = new Tenant() { Provider = "memory", ConnectionString = "test" };
            MasterTenantDbContextFactory mt = new MasterTenantDbContextFactory(() => { return T; });
            var tenantGetter = new TenantContext(mt);
            var loggerdb = new Microsoft.Extensions.Logging.Abstractions.NullLogger<MultiTenantDbContext>();
            factory = new ApplicationDbContextFactory(tenantGetter, loggerdb);
            repo = new HistoryRepository(factory);
        }

        [Test]
        public async Task Add_shouldBeOk()
        {
            var C = new HistoryEntry() { 
                Description="Test Evento ", 
                DeputyUserId="user 2", 
                UserId="user 1", 
                EventType= EventType.Creation, 
                Recipients= new List<HistoryRecipient>()
                {
                    new HistoryRecipient() { ProfileId="user 2", ProfileType=ProfileType.User, CC=false } ,
                    new HistoryRecipient() {ProfileId="group 1", ProfileType=ProfileType.Group, CC=true } 
                },
                Documents = new List<HistoryDocument>()
                {
                    new HistoryDocument() { DocumentId=1, ImageId=2 },
                    new HistoryDocument() { DocumentId=3, ImageId=4 }
                }
            };
            var i = await repo.Insert(C);
            Assert.IsTrue( i > 0 );
        }


    }
}