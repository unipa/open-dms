using OpenDMS.MultiTenancy.Interfaces;
using OpenDMS.MultiTenancy;
using OpenDMS.TenantManager.API.Controllers;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.MultiTenancy.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.TenantManager.API.DTOs;
using OpenDMS.MultiTenancy.DbContexts;

namespace OpenDMS.TenantManager.API.Tests
{
    public class Tests
    {
        private NullLogger<TenantController> logger;
        private ApplicationDbContextFactory appFactory;
        private MasterTenantDbContextFactory contextFactory;
        private TenantRegistryRepository tenantRegistryRepository;
        private TenantContext tenantContext;
        private TenantController controller;

        public Tenant tenant { get; private set; }

        [SetUp]
        public void Setup()
        {
            tenant = new Tenant();
            tenant.Provider = "Memory"; // "MySql";
            tenant.ConnectionString = "test"; 
            var builder = Host.CreateDefaultBuilder();
            logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<TenantController>();
            var config = new Microsoft.Extensions.Configuration.ConfigurationManager();
            contextFactory = new MasterTenantDbContextFactory(() => { return tenant; });
            tenantContext = new TenantContext(contextFactory);
            var loggerdb = new Microsoft.Extensions.Logging.Abstractions.NullLogger<MultiTenantDbContext>();
            appFactory = new ApplicationDbContextFactory(tenantContext, loggerdb);

            var db = contextFactory.GetDbContext();
            if (!db.Database.IsInMemory()) db.Database.Migrate();
            tenantRegistryRepository = new TenantRegistryRepository(contextFactory, appFactory);
            controller = new TenantController(logger, config, tenantRegistryRepository);
        }

        [Test]
        public async Task Create_IdEmpty()
        {
            var T = new TenantCreationDTO() { Id = "" };
            var result = await controller.Create(T);
            Assert.IsTrue(result.Result is BadRequestObjectResult);
        }
        [Test]
        public async Task Create_IdOk()
        {
            var T = new TenantCreationDTO() { Id = "test1" };
            var result = await controller.Create(T);
            Assert.IsTrue(result.Result is CreatedAtActionResult);
        }

        [Test]
        public async Task GetAll_AtLeastOneTenant()
        {
            var T = new TenantCreationDTO() { Id = "test2", DatabaseConnectionStrategy = DatabaseConnectionMode.ConnectOrCreate };
            var result = await controller.Create(T);
            var l = (controller.GetAll(false)).Count();
            Assert.IsTrue(l >= 1);
        }
        [Test]
        public async Task GetAll_AtLeastOneTenantOnLine()
        {

            {
                foreach (var t in tenantRegistryRepository.GetAll(false).ToList())
                    await tenantRegistryRepository.Delete(t.Id);

                var T = new TenantCreationDTO() { Id = "test3", DatabaseConnectionStrategy = DatabaseConnectionMode.ConnectOrCreate };
                var result = await controller.Create(T);

                T = new TenantCreationDTO() { Id = "test4", Offline = true, DatabaseConnectionStrategy = DatabaseConnectionMode.ConnectOrCreate };
                result = await controller.Create(T);

                T = new TenantCreationDTO() { Id = "test5", Offline = true, DatabaseConnectionStrategy = DatabaseConnectionMode.ConnectOrCreate };
                result = await controller.Create(T);
                var l1 = (controller.GetAll(true)).ToList();
                var l2 = (controller.GetAll(false)).ToList();
                Assert.IsTrue(l1.Count() >= 1 && l2.Count()-l1.Count() == 2);
            }
        }
        [Test]
        public async Task Create_DbNotExists()
        {
            try
            {
                var T = new TenantCreationDTO() { Id = "test6", DatabaseConnectionStrategy = DatabaseConnectionMode.Connect };
                var result = await controller.Create(T);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is TenantDatabaseNotFoundException);
            }
        }
    }
}