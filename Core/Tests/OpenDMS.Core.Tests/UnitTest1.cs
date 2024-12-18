
using Elmi.Core.FileConverters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.DataTypes;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.Domain.Infrastructure.Providers;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using OpenDMS.Infrastructure.Database.Builder;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.Infrastructure.Database.Repositories;
using OpenDMS.Infrastructure.Repositories;
using OpenDMS.Infrastructure.Services;
using OpenDMS.MultiTenancy;
using OpenDMS.MultiTenancy.DbContexts;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Core.Tests
{
    public class Tests
    {

        private ApplicationDbContextFactory factory;
        private ProcessSearchService service;


        [SetUp]
        public async Task Setup()
        {
            //var T = new Tenant() { Provider = "memory", ConnectionString = "test" };
            var T = new Tenant() { Provider = "Mysql", ConnectionString = "server=localhost;Database=OpenDMS11;uid=root;pwd=SqlServer20!7;" };
            //var T = new Tenant() { Provider = "memory", ConnectionString = "test" };

            var builder = Host.CreateDefaultBuilder();
            var alogger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<AppSetting>();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<IDocumentService>();
            var logger3 = new Microsoft.Extensions.Logging.Abstractions.NullLogger<IRoleService>();
            var logger4 = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ICustomFieldService>();
            var logger5 = new Microsoft.Extensions.Logging.Abstractions.NullLogger<IDocumentTypeService>();
            var logger7 = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ILookupTableService>();
            var logger10 = new Microsoft.Extensions.Logging.Abstractions.NullLogger<IACLService>();
            var logger6 = new Microsoft.Extensions.Logging.Abstractions.NullLogger<UserSetting>();
            var loggerdb = new Microsoft.Extensions.Logging.Abstractions.NullLogger<MultiTenantDbContext>();
            var logger11 = new Microsoft.Extensions.Logging.Abstractions.NullLogger<UserService>();


            var config = new Microsoft.Extensions.Configuration.ConfigurationManager();
            var tenantFactory = new MasterTenantDbContextFactory(() => { return T; });
            var tenantContext = new TenantContext(tenantFactory);
            var contextFactory = new ApplicationDbContextFactory(tenantContext, loggerdb);

            builder.ConfigureServices(s =>
            {
                s.AddScoped<IApplicationDbContextFactory, ApplicationDbContextFactory>();
                s.AddScoped<IMasterTenantDbContextFactory>(s => new MasterTenantDbContextFactory(() => { return T; }));

                s.AddScoped<ITenantContext<Tenant>, TenantContext>();
                s.AddScoped<IVirtualFileSystem>(s => new AbstractFileSystem());
                s.AddScoped<IRoleRepository, RoleRepository>();
                s.AddScoped<IUserRepository, UserRepository>();
                s.AddScoped<IDataSourceProvider, DataSourceProvider>();
                s.AddScoped<IUserSettingsRepository, UserSettingRepository>();
                s.AddScoped<IACLRepository, ACLRepository>();
                s.AddScoped<IACLService, ACLService>();

                s.AddScoped<IOrganizationRepository, OrganizationRepository>();
                s.AddScoped<IUserGroupRepository, UserGroupRepository>();

                s.AddScoped<IRoleService, RoleService>();
                s.AddScoped<IUserService, UserService>();
                s.AddScoped<IOrganizationUnitService, OrganizationUnitService>();
                s.AddScoped<IDataTypeManager, OrganizationProfileFieldType>();
                s.AddScoped<IDataTypeManager, TextFieldType>();
                s.AddScoped<IDataTypeManager, ParagraphFieldType>();
                s.AddScoped<IDataTypeManager, DateFieldType>();
                s.AddScoped<IDataTypeManager, DatabaseFieldType>();
                s.AddScoped<IVirtualFileSystem, AbstractFileSystem>();
                s.AddScoped<IVirtualFileSystemProvider, VirtualFileSystemProvider>();
                s.AddScoped<IDocumentNotificationService, DocumentNotificationService>();
                s.AddScoped<IEventManager, EventManager>();
                s.AddScoped<INotificationService, NotificationService>();
            });
            var s = builder.Build();
            var fileConverter = new FileConvertFactory(s.Services);
            var dataFactory = new DataTypeFactory(s.Services);
            var filesystemFactory = new VirtualFileSystemProvider(s.Services);

            var companyRepo = new CompanyRepository(contextFactory);
            var docRepo = new DocumentRepository(contextFactory);
            var userGroup = new UserGroupRepository(contextFactory);
            var _metarepo = new CustomDataTypeRepository(contextFactory, dataFactory);
            var _docTypeRepo = new DocumentTypeRepository(contextFactory);
            var aclRepo = new ACLRepository(contextFactory);
            var organizationRepo = new OrganizationRepository(contextFactory);
            var roleRepo = new RoleRepository(contextFactory);
            var tableRepo = new LookupTableRepository(contextFactory);
            var appSettings = new AppSettingRepository(alogger, contextFactory);
            var orgRepo = new OrganizationRepository(contextFactory);
            var tlRepo = new TranslationRepository(contextFactory);
            var userRepo = new UserRepository(contextFactory);

            var aclservice = new ACLService(logger10, aclRepo);

            var metaRepo = new CustomFieldService(logger4, dataFactory, _metarepo);
            var lookupService = new LookupTableService(tableRepo, logger7);
            var docTypeRepo = new DocumentTypeService(logger5, _docTypeRepo, tableRepo, aclservice, tlRepo);
            var roleService = new RoleService(logger3, roleRepo);//, rolePeRepo);
            var orgService = new OrganizationUnitService(orgRepo, userRepo, userGroup, aclservice, roleService);
            var userSettings = new UserSettingRepository(logger6, contextFactory);
            var userManager = new UserService(userRepo, orgService, roleRepo, filesystemFactory,companyRepo, userSettings, logger11, config);

            var sqlbuilder = new SqlBuilder(contextFactory);
            var queryBuilder = new QueryBuilder(sqlbuilder);
            service = new ProcessSearchService(queryBuilder, lookupService, dataFactory);
        }

        [Test]
        public async Task Count()
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
            var view = await service.GetDefaultViewProperties("test", u);
            view.Columns.All(c => c.Settings.Visible = false);

            var sr = new SearchRequest()
            {
                ViewId = "test",
                PageIndex = 0,
                PageSize = int.MaxValue,
                Filters = new(),
                OrderBy = new()
            };
            var count = await service.Count(sr.Filters, u);
            var results = await service.Get(view, sr, u);
            if (results.Page.Rows[0].Columns[0].Value == count.ToString())
                Assert.Pass();
            else
                Assert.Fail("Found: " + results.Page.Rows[0].Columns[0].Value + " records");

        }

        //[Test]
        //public async Task CountProcesses()
        //{
        //    UserProfile u = UserProfile.SystemUser();
        //    var view = new ViewProperties() { ViewId="test" };
        //    view.Columns.Add(service.CreateColumn("userid"));
        //    view.Columns.Add(service.CreateColumn("claimtime"));
        //    view.Columns.Last().Settings.AggregateType = AggregateType.Average;
        //    view.Columns.Add(service.CreateColumn("claimdate"));
        //    view.Columns.Last().Settings.AggregateType = AggregateType.Max;
        //    view.Columns.Add(service.CreateColumn("claimdate"));
        //    view.Columns.Last().Settings.AggregateType = AggregateType.Min;
        //    var sr = new SearchRequest()
        //    {
        //        ViewId = "test",
        //        PageIndex = 0,
        //        PageSize = int.MaxValue,
        //        Filters = new(),
        //        OrderBy = new()
        //    };
        //    var results = await service.Get(view, sr, u);
        //    var count = await service.Count(sr.Filters, u);
        //    Assert.Pass();

        //}

    }
}