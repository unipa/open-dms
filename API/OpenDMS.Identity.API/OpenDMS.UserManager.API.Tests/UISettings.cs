using System.Text.Json.Serialization;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.Infrastructure.Repositories;
using OpenDMS.MultiTenancy.Interfaces;
using OpenDMS.MultiTenancy;
using System.Text;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.MultiTenancy.DbContexts;

namespace OpenDMS.UserManager.API.Tests
{
    public class UISettings_Tests
    {
        private UISettingRepository Controller;

        [SetUp]
        public void Setup()
        {
            var T = new Tenant() { Id = "Test", Provider = "memory", ConnectionString = "test" };
            var tenantFactory = new MasterTenantDbContextFactory(() => { return T; });
            var tenantContext = new TenantContext(tenantFactory);
            var loggerdb = new Microsoft.Extensions.Logging.Abstractions.NullLogger<MultiTenantDbContext>();
            var contextFactory = new ApplicationDbContextFactory(tenantContext,loggerdb);
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<UISetting>();
            Controller = new UISettingRepository(logger,contextFactory);
        }

        [Test]
        public async Task Insert_ShouldBeOk()
        {
            //Arrange
            var sett = new UISetting();

            sett.CompanyId = 1;
            sett.UserId = "test1";
            sett.Name = "test1_name";
            sett.Value = "test1_value_" + Guid.NewGuid();

            //Act

            await Controller.Set(sett.CompanyId,sett.UserId,sett.Name,sett.Value); // create

            //Assert

            var res = await Controller.Get(sett.CompanyId, sett.UserId, sett.Name);

            Assert.IsTrue(res.Equals(sett.Value));


        }

        [Test]
        public async Task Update_ShouldBeOk()
        {
            //Arrange
            var sett = new UISetting();

            sett.CompanyId = 1;
            sett.UserId = "test2";
            sett.Name = "test2_name";
            sett.Value = "test2_value";

            //Act

            await Controller.Set(sett.CompanyId, sett.UserId, sett.Name, sett.Value); // create

            sett.Value = "test2_value_modified_"+Guid.NewGuid();

            await Controller.Set(sett.CompanyId, sett.UserId, sett.Name, sett.Value); //update

            //Assert

            var res = await Controller.Get(sett.CompanyId, sett.UserId, sett.Name);

            Assert.IsTrue(res.Equals(sett.Value));
        }

        [Test]
        public async Task Get_usingOnlyUserIdandName_ShouldBeOk()
        {
            //Arrange
            var sett = new UISetting();

            sett.CompanyId = 0; //Get(userId,Name) cerca per CompanyId = 0 
            sett.UserId = "test3";
            sett.Name = "test3_name";
            sett.Value = "test3_value_"+Guid.NewGuid();

            //Act

            await Controller.Set(sett.CompanyId, sett.UserId, sett.Name, sett.Value); // create

            //Assert

            var res = await Controller.Get(sett.UserId, sett.Name);

            Assert.IsTrue(res.Equals(sett.Value));
        }

        [Test]
        public async Task GetAll_ShouldBeOk()
        {
            //Arrange
            var sett = new UISetting();

            sett.CompanyId = 0;
            sett.UserId = "test4";
            sett.Name = "test41_name";
            sett.Value = "test41_value_"+Guid.NewGuid();

            //Act

            await Controller.Set(sett.CompanyId, sett.UserId, sett.Name, sett.Value); // create

            //Assert

            var res = await Controller.GetAll(sett.CompanyId, sett.UserId);

            Assert.IsTrue(res.Any(x=>x.Value.Equals(sett.Value)));
        }

    }
}