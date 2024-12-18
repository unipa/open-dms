using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.Infrastructure.Database.Repositories;
using OpenDMS.MultiTenancy;
using OpenDMS.MultiTenancy.DbContexts;
using OpenDMS.MultiTenancy.Interfaces;
using System.Text;

namespace OpenDMS.Document.API.Tests
{
    public class History_Tests
    {
        private IHistoryRepository Controller;

        [SetUp]
        public void Setup()
        {
            var T = new Tenant() { Id = "Test", Provider = "memory", ConnectionString = "test" };
            var tenantFactory = new MasterTenantDbContextFactory(() => { return T; });
            var tenantContext = new TenantContext(tenantFactory);
            var loggerdb = new Microsoft.Extensions.Logging.Abstractions.NullLogger<MultiTenantDbContext>();
            var contextFactory = new ApplicationDbContextFactory(tenantContext, loggerdb);
            Controller = new HistoryRepository(contextFactory);
        }

        [Test]
        public async Task Insert_ShouldBeOk()
        {
            //Arrange
            var sett = new HistoryEntry();

            sett.UserId        = "TEST1" ;
            sett.DeputyUserId  = "TEST1_DeputyUserId";
            sett.Description   = "TEST1_Description";
            sett.EventType     = EventType.Comment;
            sett.Documents = new List<HistoryDocument>() {
                new HistoryDocument() { DocumentId=1, ImageId=0 },
                new HistoryDocument() { DocumentId=2, ImageId=3 }
            };
            sett.WorkflowId    = "";
            sett.TaskId        = "";
            var i = await Controller.Insert(sett);
            Assert.IsTrue(sett.Id > 0);
            Assert.IsTrue(i > 0);
        }

        [Test]
        public async Task GetById_ShouldBeOk()
        {
            //Arrange

            var sett = new HistoryEntry();

            sett.UserId = "TEST2";
            sett.DeputyUserId = "TEST2_DeputyUserId";
            sett.Description = "TEST2_Description_"+Guid.NewGuid();
            sett.EventType = EventType.Comment;
            sett.Documents = new List<HistoryDocument>() {
                new HistoryDocument() { DocumentId=1, ImageId=0 },
                new HistoryDocument() { DocumentId=2, ImageId=3 }
            };
            sett.WorkflowId = "";
            sett.TaskId = "";

            //var filter = new HistoryFilters();
            //filter.UserId = sett.UserId;

            //Act

            var i = await Controller.Insert(sett);

            var res = await Controller.GetById(sett.Id);
            
            //Assert

            Assert.IsTrue(res.Description.Equals(sett.Description));

        }

        [Test]
        public async Task GetByFilter_ShouldBeOk()
        {
            //Arrange

            var sett = new HistoryEntry();

            sett.UserId = "TEST2";
            sett.DeputyUserId = "TEST2_DeputyUserId";
            sett.Description = "TEST2_Description_" + Guid.NewGuid();
            sett.EventType = EventType.Comment;
            sett.Documents = new List<HistoryDocument>() {
                new HistoryDocument() { DocumentId=1, ImageId=0 },
                new HistoryDocument() { DocumentId=2, ImageId=3 }
            };
            sett.WorkflowId = "";
            sett.TaskId = "";

            var filter = new HistoryFilters();
            filter.UserId = sett.UserId;

            //Act

            var i = await Controller.Insert(sett);

            var res = await Controller.GetByFilters(filter);

            //Assert

            Assert.IsTrue(res.Any(x=> x.Description.Equals(sett.Description)));

        }

        [Test]
        public async Task Count_ShouldBeOk()
        {
            //Arrange
            var sett = new HistoryEntry();

            sett.UserId = "TEST3_" + Guid.NewGuid();
            sett.DeputyUserId = "TEST3_DeputyUserId";
            sett.Description = "TEST3_Description";
            sett.EventType = EventType.Comment;
            sett.WorkflowId = "TEST3_WorkflowId";
            sett.TaskId = "TEST3_TaskId";

            //Act

            var i = await Controller.Insert(sett);

            //Assert

            var count = await Controller.Count();

            Assert.IsTrue(count >= 1);

        }

        [Test]
        public async Task Count_withFilter_ShouldBeOk()
        {
            //Arrange
            var sett = new HistoryEntry();

            sett.UserId = "TEST3_" + Guid.NewGuid();
            sett.DeputyUserId = "TEST3_DeputyUserId";
            sett.Description = "TEST3_Description";
            sett.EventType = EventType.Comment;
            sett.WorkflowId = "TEST3_WorkflowId";
            sett.TaskId = "TEST3_TaskId";

            var filter = new HistoryFilters();
            filter.UserId = sett.UserId;

            //Act

            var i = await Controller.Insert(sett);

            //Assert

            var count = await Controller.Count(filter);

            Assert.IsTrue(count >= 1);

        }
    }
}