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
    public class UserSettings_Tests
    {
        private UserSettingRepository Controller;

        [SetUp]
        public void Setup()
        {
            var T = new Tenant() { Id = "Test", Provider = "memory", ConnectionString = "test" };
            var tenantFactory = new MasterTenantDbContextFactory(() => { return T; });
            var tenantContext = new TenantContext(tenantFactory);
            var loggerdb = new Microsoft.Extensions.Logging.Abstractions.NullLogger<MultiTenantDbContext>();
            var contextFactory = new ApplicationDbContextFactory(tenantContext, loggerdb);
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<UserSetting>();
            Controller = new UserSettingRepository(logger, contextFactory);
        }

        [Test]
        public async Task Insert_ShouldBeOk()
        {
            //Arrange
            var sett = new UserSetting();

            sett.CompanyId = 1;
            sett.ContactId = "test1";
            sett.AttributeId = "test1_name";
            sett.Value = "test1_value";

            //Act

            await Controller.Set(sett.CompanyId, sett.ContactId, sett.AttributeId, sett.Value); // create

            //Assert

            var res = await Controller.Get(sett.CompanyId, sett.ContactId, sett.AttributeId);

            Assert.IsTrue(res.Equals(sett.Value));

        }

        [Test]
        public async Task Update_ShouldBeOk()
        {
            //Arrange
            var sett = new UserSetting();

            sett.CompanyId = 1;
            sett.ContactId = "test2";
            sett.AttributeId = "test2_name";
            sett.Value = "test2_value";

            //Act

            await Controller.Set(sett.CompanyId, sett.ContactId, sett.AttributeId, sett.Value); // create

            sett.Value = "test2_value_modified_"+Guid.NewGuid();

            await Controller.Set(sett.CompanyId, sett.ContactId, sett.AttributeId, sett.Value); //update

            //Assert

            var res = await Controller.Get(sett.CompanyId, sett.ContactId, sett.AttributeId);

            Assert.IsTrue(res.Equals(sett.Value));
        }

        [Test]
        public async Task Get_usingOnlyUserIdandName_ShouldBeOk()
        {
            //Arrange
            var sett = new UserSetting();

            sett.CompanyId = 0; //Get(userId,Name) cerca per CompanyId = 0 
            sett.ContactId = "test3";
            sett.AttributeId = "test3_name";
            sett.Value = "test3_value";

            //Act

            await Controller.Set(sett.CompanyId, sett.ContactId, sett.AttributeId, sett.Value); // create

            //Assert

            var res = await Controller.Get(sett.ContactId, sett.AttributeId);

            Assert.IsTrue(res.Equals(sett.Value));
        }

        [Test]
        public async Task GetAll_ShouldBeOk()
        {
            //Arrange
            var sett = new UserSetting();

            sett.CompanyId = 0;
            sett.ContactId = "test4";
            sett.AttributeId = "test41_name";
            sett.Value = "test41_value_"+Guid.NewGuid();

            //Act

            await Controller.Set(sett.CompanyId, sett.ContactId, sett.AttributeId, sett.Value); // create

            //Assert

            var res = await Controller.GetAll(sett.CompanyId, sett.ContactId);

            Assert.IsTrue(res.Any(x => x.Value.Equals(sett.Value)));
        }

    }
}