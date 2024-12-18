using Elmi.Core.FileConverters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenDMS.Core;
using OpenDMS.Core.Builders;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.DataTypes;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.Managers;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.Domain.Infrastructure.Providers;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using OpenDMS.Infrastructure.Database;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.Infrastructure.Database.Repositories;
using OpenDMS.Infrastructure.Repositories;
using OpenDMS.Infrastructure.Services;
using OpenDMS.MultiTenancy;
using OpenDMS.MultiTenancy.DbContexts;
using OpenDMS.MultiTenancy.Interfaces;


namespace OpenDMS.Tasklist.API.Tests
{
    public class Tasklist
    {

        private UserTaskService service;
        private CompanyRepository companyRepo;
        private UserProfile u;
        private UserService UserServ;

        [SetUp]
        public async Task Setup()
        {
            var T = new Tenant() { Id = "Test", Provider = "memory", ConnectionString = "test" };

            var builder = Host.CreateDefaultBuilder();
            var alogger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<AppSetting>();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<IDocumentService>();
            var logger3 = new Microsoft.Extensions.Logging.Abstractions.NullLogger<IRoleService>();
            var logger4 = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ILookupTableService>();
            var logger5 = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ICustomFieldService>();
            var logger6 = new Microsoft.Extensions.Logging.Abstractions.NullLogger<UserSetting>();
            var logger7 = new Microsoft.Extensions.Logging.Abstractions.NullLogger<UserTaskService>();
            var logger8 = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ICustomFieldService>();
            var logger9 = new Microsoft.Extensions.Logging.Abstractions.NullLogger<IDocumentTypeService>();
            var logger10 = new Microsoft.Extensions.Logging.Abstractions.NullLogger<IACLService>();
            var logger11 = new Microsoft.Extensions.Logging.Abstractions.NullLogger<UserService>();
            var loggerdb = new Microsoft.Extensions.Logging.Abstractions.NullLogger<MultiTenantDbContext>();

            var config = new Microsoft.Extensions.Configuration.ConfigurationManager();
            var tenantFactory = new MasterTenantDbContextFactory(() => { return T; });
            var tenantContext = new TenantContext(tenantFactory);
            var contextFactory = new ApplicationDbContextFactory(tenantContext, loggerdb);

            builder.ConfigureServices(s =>
            {
                s.AddRepositories();
                s.AddScoped<IApplicationDbContextFactory, ApplicationDbContextFactory>();
                s.AddScoped<IMasterTenantDbContextFactory>(s => new MasterTenantDbContextFactory(() => { return T; }));

                s.AddScoped<ITenantContext<Tenant>, TenantContext>();
                s.AddScoped<IVirtualFileSystem>(s => new AbstractFileSystem());
              //  s.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
                s.AddScoped<IRoleRepository, RoleRepository>();
                s.AddScoped<IUserRepository, UserRepository>();

                s.AddScoped<IOrganizationRepository, OrganizationRepository>();
                //s.AddScoped<IRoleService, RoleService>();
                s.AddScoped<IUserService, UserService>();
                s.AddScoped<IUserGroupRepository, UserGroupRepository>();
                s.AddScoped<IOrganizationUnitService, OrganizationUnitService>();
                s.AddScoped<IVirtualFileSystem, AbstractFileSystem>();
                s.AddScoped<IVirtualFileSystemProvider, VirtualFileSystemProvider>();
                s.AddScoped<INotificationService, NotificationService>();
                s.AddScoped<IEventManager, EventManager>();
                s.AddCoreServices();

            });

            var s = builder.Build();

            var dataFactory = new DataTypeFactory(s.Services);
            var filesystemFactory = new VirtualFileSystemProvider(s.Services);
            var viewServiceResolver = new ViewServiceResolver(s.Services);
            var fileConverter = new FileConvertFactory(s.Services);

            companyRepo = new CompanyRepository(contextFactory);
            var companyService = new CompanyService(companyRepo);
            var docRepo = new DocumentRepository(contextFactory);
            var userGroup = new UserGroupRepository(contextFactory);
            var metaRepo = new CustomDataTypeRepository(contextFactory, dataFactory);
            var docTypeRepo = new DocumentTypeRepository(contextFactory);
            var aclRepo = new ACLRepository(contextFactory);
            var organizationRepo = new OrganizationRepository(contextFactory);
            var roleRepo = new RoleRepository(contextFactory);
            //var rolePermissionRepo = new RolePermissionRepository(contextFactory);
            var tableRepo = new LookupTableRepository(contextFactory);
            var appSettings = new AppSettingRepository(alogger, contextFactory);
            var trrepo = new TranslationRepository(contextFactory);

            //var rolePeRepo = new RolePermissionRepository(contextFactory);
            var OrgRepo = new OrganizationRepository(contextFactory);
            var userRepo = new UserRepository(contextFactory);
            var Dao = new TaskRepository(contextFactory);
            var filterRepo = new SearchFilterRepository(contextFactory);
            var UserGroupRepo = new UserGroupRepository(contextFactory);
            var CfRepo = new CustomDataTypeRepository(contextFactory, dataFactory);


            var aclservice = new ACLService(logger10, aclRepo);
            var RoleServ = new RoleService(logger3, roleRepo);//, rolePeRepo);
            var orgService = new OrganizationUnitService(OrgRepo, userRepo, userGroup, aclservice, RoleServ);
            var userSettings = new UserSettingRepository(logger6, contextFactory);
            UserServ = new UserService(userRepo, orgService, roleRepo, filesystemFactory,companyRepo, userSettings, logger11, config);
            var UserFilterServ = new UserFiltersService(filterRepo);
            var LtServ = new LookupTableService(tableRepo, logger4);
            var metaservice = new CustomFieldService(logger8, dataFactory, metaRepo);
            var doctypeservice = new DocumentTypeService(logger9, docTypeRepo, tableRepo, aclservice, trrepo);


            u = await UserServ.GetUserProfile("admin");

            IEventManager eventDispatcher = null;
            IDocumentNotificationService notificationService = null;
            INotificationService mailService = null;
            IMailboxService mailbox = null;
            IMessageBuilder messageBuilder = new MessageBuilder();
            var CfServ = new CustomFieldService(logger5, dataFactory, CfRepo);

            var historyRepository = new HistoryRepository (contextFactory);
            var DocServ = new DocumentService(
                docRepo,
                metaservice,
                doctypeservice,
                aclservice, 
                userGroup,
                roleRepo,
                //rolePermissionRepo,
                companyRepo,
                tableRepo,
                fileConverter,
                UserServ,
                filesystemFactory, 
                
                notificationService,
                historyRepository,
                eventDispatcher,
                dataFactory,
                config,
                logger);

            var FormServ = new FormService(DocServ, appSettings, null, null,null);

            service = new UserTaskService(
                Dao,
                DocServ,
                RoleServ,
                aclservice,
                UserServ,
                mailbox,
                UserFilterServ,
                mailService,
                appSettings,
                messageBuilder,
                LtServ,
                
                UserGroupRepo,
                viewServiceResolver,
                orgService,
                OrgRepo,
                FormServ, 
                logger7,
                appSettings,
                                companyService
);
        }

        [Test]
        public async Task SaveFilter_ShouldBeOk()
        {
            //Arrange 

            var newFilter = new SearchFilters();

            newFilter.UserId = "admin";
            newFilter.Name = "test1";

            //Act 

            await service.SaveFilter(newFilter);

            var getFilters = await service.Filters(u);

            //Assert

            Assert.IsTrue(getFilters.Any(f => f.Name.Equals(newFilter.Name)));

        }

        [Test]
        public async Task RenameFilter_ShouldBeOk()
        {
            //Arrange 

            var newFilter = new SearchFilters();

            newFilter.Id = 1;
            newFilter.UserId = "admin";
            newFilter.Name = "test2";

            //Act 

            var addedFilter = await service.SaveFilter(newFilter);

            var newName = "test_modified";

            await service.RenameFilter(addedFilter.Id, newName, u);

            var getFilters = await service.Filters(u);

            //Assert

            Assert.IsTrue(getFilters.Any(f => f.Name.Equals(newName)));

        }

        [Test]
        public async Task RenameFilter_ShouldFail()
        {
            //Arrange 

            var NotExistingFilterId = new Random().Next();

            //Act 

            var newName = "test_modified";
            try
            {
                var res = await service.RenameFilter(NotExistingFilterId, newName, u);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.Pass();
            }
        }

        [Test]
        public async Task RemoveFilter_ShouldBeOk()
        {
            //Arrange 

            var newFilter = new SearchFilters();

            newFilter.Id = 1;
            newFilter.UserId = "admin";
            newFilter.Name = "test3";

            //Act 

            var addedFilter = await service.SaveFilter(newFilter);

            var res = await service.RemoveFilter(addedFilter.Id, u);

            var getFilters = await service.Filters(u);

            //Assert

            Assert.IsTrue(!getFilters.Any(f => f.Name.Equals(newFilter.Name)));

        }

        [Test]
        public async Task CreateTask_ShouldBeOk()
        {
            //Arrange 

            var filters = new Domain.Models.TaskListFilter();
            filters.ParentId = 1;

            var newTask = new CreateNewTask();

            newTask.ParentTaskId = 1;
            newTask.Title = "test_task1";

            //Act 

            var addedTask = await service.CreateTask(newTask, u);


            var getTasks = await service.Find(filters, u);

            //Assert

            Assert.IsTrue(getTasks.Any(t => t.Title.Equals(newTask.Title)));

        }

        [Test]
        public async Task GetById_ShouldBeOk()
        {
            //Arrange 

            var filters = new Domain.Models.TaskListFilter();
            var newTask = new CreateNewTask();

            newTask.ParentTaskId = 0;
            newTask.Title = "test_task2_" + Guid.NewGuid();

            //Act 

            var addedTask = await service.CreateTask(newTask, u);


            var UserTaskInfo = await service.GetById(addedTask.Id, u);

            //Assert

            Assert.IsTrue(UserTaskInfo.TaskItemInfo.Title.Equals(newTask.Title));

        }

        [Test]
        public async Task Claim_ShouldBeOk()
        {
            //Arrange 

            var filters = new Domain.Models.TaskListFilter();
            var newTask = new CreateNewTask();

            newTask.ParentTaskId = 0;
            newTask.Title = "test_task3";

            //Act 

            var addedTask = await service.CreateTask(newTask, u);

            var UserTaskInfo = await service.Claim(addedTask.Id, u);

            //Assert

            Assert.IsTrue(UserTaskInfo.Status == ExecutionStatus.Assigned);

        }

        [Test]
        public async Task Claim_ShouldBeFail()
        {
            //Arrange 

            var filters = new Domain.Models.TaskListFilter();
            var newTask = new CreateNewTask();

            newTask.ParentTaskId = 0;
            newTask.Title = "test_task3";

            //Act 

            var addedTask = await service.CreateTask(newTask, u);

            await service.Claim(addedTask.Id, u); // first claim 

            var UserTaskInfo = await service.Claim(addedTask.Id, u); // secondo claim should return null

            //Assert

            Assert.IsTrue(UserTaskInfo == null);

        }

        [Test]
        public async Task Release_ShouldBeOk()
        {
            //Arrange 

            var filters = new Domain.Models.TaskListFilter();
            var newTask = new CreateNewTask();

            newTask.ParentTaskId = 0;
            newTask.Title = "test_task4";

            //Act 

            var addedTask = await service.CreateTask(newTask, u);

            var UserTaskInfo_Claimed = await service.Claim(addedTask.Id, u);

            if (UserTaskInfo_Claimed.Status != ExecutionStatus.Assigned) Assert.Fail();

            var UserTaskInfo_Released = await service.Release(addedTask.Id, u, "test realease");

            //Assert

            Assert.IsTrue(UserTaskInfo_Released.Status == ExecutionStatus.Unassigned);

        }

        [Test]
        public async Task Release_ShouldFail()
        {
            //Arrange 

            var filters = new Domain.Models.TaskListFilter();
            var newTask = new CreateNewTask();

            newTask.ParentTaskId = 0;
            newTask.Title = "test_task4";

            //Act 

            var addedTask = await service.CreateTask(newTask, u);

            var UserTaskInfo_Claimed = await service.Claim(addedTask.Id, u);

            if (UserTaskInfo_Claimed.Status != ExecutionStatus.Assigned) Assert.Fail();

            await service.Release(addedTask.Id, u, "test realease");
            try
            {
                var UserTaskInfo_Released_Again = await service.Release(addedTask.Id, u, "test realease");
                Assert.Fail();
            }
            catch
            {
                Assert.Pass();

            }

        }

        [Test]
        public async Task Execute_ShouldBeOk()
        {
            //Arrange 

            var filters = new Domain.Models.TaskListFilter();
            var newTask = new CreateNewTask();

            newTask.ParentTaskId = 0;
            newTask.Title = "test_task5";

            //Act 

            var addedTask = await service.CreateTask(newTask, u);
            await service.Claim(addedTask.Id, u);

            var UserTaskInfo = await service.Execute(addedTask.Id, u, "test execute");

            //Assert

            Assert.IsTrue(UserTaskInfo.Status == ExecutionStatus.Executed);

        }

        [Test]
        public async Task Execute_ShouldFail()
        {
            //Arrange 

            var filters = new Domain.Models.TaskListFilter();
            var newTask = new CreateNewTask();

            newTask.ParentTaskId = 0;
            newTask.Title = "test_task5";


            //Act 

            var addedTask = await service.CreateTask(newTask, u);
            await service.Claim(addedTask.Id, u);
            await service.Execute(addedTask.Id, u, "test execute"); // first change status to execute
            try
            {
                var UserTaskInfo = await service.Execute(addedTask.Id, u, "test execute"); // Try to re-execute task
                Assert.Fail();
            }
            catch {
                Assert.Pass();
            };
            //Assert


        }

        [Test]
        public async Task Validate_ShouldBeOk()
        {
            //Arrange 

            var filters = new Domain.Models.TaskListFilter();
            var newTask = new CreateNewTask();

            newTask.ParentTaskId = 0;
            newTask.Title = "test_task6";

            //Act 

            var addedTask = await service.CreateTask(newTask, u);
            await service.Claim(addedTask.Id, u);

            await service.Execute(addedTask.Id, u, "test execute");

            var UserTaskInfo = await service.Validate(addedTask.Id, u, "test validate");

            //Assert

            Assert.IsTrue(UserTaskInfo.Status == ExecutionStatus.Validated);

        }

        [Test]
        public async Task Validate_ShouldFail()
        {
            //Arrange 

            var filters = new Domain.Models.TaskListFilter();
            var newTask = new CreateNewTask();

            newTask.ParentTaskId = 0;
            newTask.Title = "test_task6";

            //Act 

            var addedTask = await service.CreateTask(newTask, u);
            await service.Claim(addedTask.Id, u);

            // Execution fase missing to cause fail in validation

            var UserTaskInfo = await service.Validate(addedTask.Id, u, "test validate");

            //Assert

            Assert.IsTrue(UserTaskInfo == null);

        }

        [Test]
        public async Task Reject_ShouldBeOk()
        {
            //Arrange 

            var filters = new Domain.Models.TaskListFilter();
            var newTask = new CreateNewTask();

            newTask.ParentTaskId = 0;
            newTask.Title = "test_task7";

            //Act 

            var addedTask = await service.CreateTask(newTask, u);

            //ottengo un nuovo utente loggato

            var u2 = await UserServ.GetUserProfile("admin2");

//            var UserTaskInfo_Claimed = await service.Claim(addedTask.Id, u2);

//            if (UserTaskInfo_Claimed.Status != ExecutionStatus.Assigned) Assert.Fail();

            var UserTaskInfo_Rejected = await service.Reject(addedTask.Id, u2, "test reject");

            //Assert

            Assert.IsTrue(UserTaskInfo_Rejected.Status == ExecutionStatus.Unassigned);

        }

        [Test]
        public async Task Reject_ShouldFail()
        {
            //Arrange 

            var filters = new Domain.Models.TaskListFilter();
            var newTask = new CreateNewTask();

            newTask.ParentTaskId = 0;
            newTask.Title = "test_task7";

            //Act 

            var addedTask = await service.CreateTask(newTask, u);

            //ottengo un nuovo utente loggato

            var u2 = await UserServ.GetUserProfile("admin2");

            var UserTaskInfo_Claimed = await service.Claim(addedTask.Id, u2);

            if (UserTaskInfo_Claimed.Status != ExecutionStatus.Assigned) Assert.Fail();

            var UserTaskInfo_Executed = await service.Execute(addedTask.Id, u2);

            if (UserTaskInfo_Executed.Status != ExecutionStatus.Executed) Assert.Fail();

            var UserTaskInfo_Rejected = await service.Reject(addedTask.Id, u2, "test reject");

            //Assert

            Assert.IsTrue(UserTaskInfo_Rejected == null);

        }

        [Test]
        public async Task Reassign_ShouldBeOk()
        {
            //Arrange 

            var filters = new Domain.Models.TaskListFilter();
            var newTask = new CreateNewTask();

            newTask.ParentTaskId = 0;
            newTask.Title = "test_task8";

            //Act 

            var addedTask = await service.CreateTask(newTask, u);

            var ReassignetionUser = "test user reassing";

            var UserTaskInfo = await service.Reassign(addedTask.Id, u, ReassignetionUser, ProfileType.User, "test reassing");

            //Assert

            Assert.IsTrue(UserTaskInfo.UserId.Equals(ReassignetionUser));

        }

        [Test]
        public async Task Reassign_ShouldFail()
        {
            //Arrange 

            var filters = new Domain.Models.TaskListFilter();
            var newTask = new CreateNewTask();

            newTask.ParentTaskId = 0;
            newTask.Title = "test_task8";

            //Act 

            var addedTask = await service.CreateTask(newTask, u);

            var UserTaskInfo_Claimed = await service.Claim(addedTask.Id, u);

            if (UserTaskInfo_Claimed.Status != ExecutionStatus.Assigned) Assert.Fail();

            var UserTaskInfo_Executed = await service.Execute(addedTask.Id, u);

            if (UserTaskInfo_Executed.Status != ExecutionStatus.Executed) Assert.Fail();

            var ReassignetionUser = "test user reassing";

            var UserTaskInfo = await service.Reassign(addedTask.Id, u, ReassignetionUser, ProfileType.User, "test reassing");

            //Assert

            Assert.IsTrue(UserTaskInfo == null);

        }

        [Test]
        public async Task AddMessage_ShouldBeOk()
        {
            //Arrange 

            var filters = new Domain.Models.TaskListFilter();
            var newTask = new CreateNewTask();

            newTask.ParentTaskId = 0;
            newTask.Title = "test_task9";

            //Act 

            var addedTask = await service.CreateTask(newTask, u);

            var MessageTaskInfo = await service.AddMessage(addedTask.Id, u, "test message");

            //Assert

            Assert.IsTrue(MessageTaskInfo != null);

        }

        [Test]
        public async Task AddMessage_ShouldFail()
        {
            //Arrange 

            var NotExistingId = new Random().Next();

            //Act 

            var MessageTaskInfo = await service.AddMessage(NotExistingId, u, "test message");

            //Assert

            Assert.IsTrue(MessageTaskInfo == null);

        }

        [Test]
        public async Task AddProgress_ShouldBeOk()
        {
            //Arrange 

            var filters = new Domain.Models.TaskListFilter();
            var newTask = new CreateNewTask();

            newTask.ParentTaskId = 0;
            newTask.Title = "test_task10";
            var progressText = "test add progress_" + Guid.NewGuid();

            //Act 

            var addedTask = (await service.CreateTask(newTask, u));

            await service.AddProgress(addedTask.Id, u.userId, progressText, 50);

            var UserTaskInfo = await service.GetById(addedTask.Id, u);

            //Assert

            Assert.IsTrue(UserTaskInfo.TaskItemInfo.Progress.Any(p => p.Message.Equals(progressText)));

        }

        [Test]
        public async Task RemoveProgress_ShouldFail()
        {
            //Arrange 

            var filters = new Domain.Models.TaskListFilter();
            var newTask = new CreateNewTask();

            newTask.ParentTaskId = 0;
            newTask.Title = "test_task11";

            var progressText = "test remove progress_" + Guid.NewGuid();

            //Act 

            var addedTask = await service.CreateTask(newTask, u);

            var taskProgressInfo = await service.AddProgress(addedTask.Id, u.userId, progressText, 50);

            await service.RemoveProgress(taskProgressInfo.Id);

            var UserTaskInfo = await service.GetById(addedTask.Id, u);

            //Assert

            Assert.IsTrue(!UserTaskInfo.TaskItemInfo.Progress.Any(p => p.Message.Equals(progressText)));

        }

        [Test]
        public async Task Count_UsingTaskListFilter_ShouldBeOk()
        {
            //Arrange 

            var filters = new Domain.Models.TaskListFilter();
            filters.ParentId = 1;

            var newTask = new CreateNewTask();

            newTask.ParentTaskId = 1;
            newTask.Title = "test_task12";

            //Act 

            var addedTask = await service.CreateTask(newTask, u);


            var TasksCount = await service.Count(filters, u);

            //Assert

            Assert.IsTrue(TasksCount >= 1);

        }

        //[Test]
        //public async Task Count_UsingListSearchFilter_ShouldBeOk()
        //{
        //    //Arrange 

        //    var newTask = new CreateNewTask();

        //    newTask.ParentTaskId = 1;
        //    newTask.Title = "test_task12";

        //    //Act 

        //    var addedTask = await service.CreateTask(newTask, u);

        //    var ListSearchFilters = await service.Filters(u);

        //    var TasksCount = await service.Count(ListSearchFilters[0].Filters, u);

        //    //Assert

        //    Assert.IsTrue(TasksCount >= 1);
        //    throw new NotImplementedException(); //metodo count ancora da finire

        //}


    }
}