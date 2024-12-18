using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.MultiTenancy.Interfaces;
using OpenDMS.MultiTenancy;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Infrastructure.Repositories;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Infrastructure;
using Microsoft.Extensions.Hosting;
using OpenDMS.Domain.Services;
using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using OpenDMS.Domain.Infrastructure.Providers;
using OpenDMS.Infrastructure.Database.Repositories;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.DataTypes;
using OpenDMS.Infrastructure.Services;
using OpenDMS.Domain.Constants;
using Elmi.Core.FileConverters;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.MultiTenancy.DbContexts;

namespace OpenDMS.Document.API.Tests
{
    public class Document_Tests
    {
        private DocumentService Controller;
        private CompanyRepository companyRepo;
        private UserProfile u;

        [SetUp]
        public async Task Setup()
        {
            // non è possibile usare il provider inmemory perchè non supporta le transazioni
            var T = new Tenant() { Id = "Test", Provider = "memory", ConnectionString = "test" };


            var builder = Host.CreateDefaultBuilder();
            var alogger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<AppSetting>();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<IDocumentService>();
            var logger3 = new Microsoft.Extensions.Logging.Abstractions.NullLogger<IRoleService>();
            var logger4 = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ICustomFieldService>();
            var logger5 = new Microsoft.Extensions.Logging.Abstractions.NullLogger<IDocumentTypeService>();
            var logger6 = new Microsoft.Extensions.Logging.Abstractions.NullLogger<UserSetting>();
            var logger10 = new Microsoft.Extensions.Logging.Abstractions.NullLogger<IACLService>();
            var logger11 = new Microsoft.Extensions.Logging.Abstractions.NullLogger<UserService>();
            var loggerdb = new Microsoft.Extensions.Logging.Abstractions.NullLogger<MultiTenantDbContext>();


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
                //s.AddScoped<IVirtualFileSystem>(s => new OSFileSystem(".\filecontent"));
                //s.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
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

            companyRepo = new CompanyRepository(contextFactory);
            var docRepo = new DocumentRepository(contextFactory);
            var userGroup = new UserGroupRepository(contextFactory);
            var _metarepo = new CustomDataTypeRepository (contextFactory, dataFactory);
            var _docTypeRepo = new DocumentTypeRepository(contextFactory);
            var aclRepo = new ACLRepository(contextFactory);
            var organizationRepo = new OrganizationRepository(contextFactory);
            var roleRepo = new RoleRepository(contextFactory);
            //var rolePermissionRepo = new RolePermissionRepository(contextFactory);
            var tableRepo = new LookupTableRepository(contextFactory);
            var appSettings = new AppSettingRepository(alogger, contextFactory);

            //var rolePeRepo = new RolePermissionRepository(contextFactory);
            var orgRepo = new OrganizationRepository(contextFactory);
            var tlRepo = new TranslationRepository(contextFactory);
            var userRepo = new UserRepository(contextFactory);

            var aclservice = new ACLService(logger10, aclRepo);
            var historyRepository = new HistoryRepository (contextFactory);


            var metaRepo = new CustomFieldService(logger4, dataFactory, _metarepo);
            var docTypeRepo = new DocumentTypeService(logger5, _docTypeRepo, tableRepo, aclservice, tlRepo);
            var roleService = new RoleService(logger3, roleRepo);//, rolePeRepo);
            var orgService = new OrganizationUnitService(orgRepo, userRepo, userGroup, aclservice, roleService);
            var userSettings = new UserSettingRepository(logger6, contextFactory);
            var userManager = new UserService(userRepo, orgService, roleRepo, filesystemFactory, companyRepo, userSettings, logger11, config);

            u = await userManager.GetUserProfile(SpecialUser.SystemUser);

            IEventManager eventDispatcher = null;
            IDocumentNotificationService notificationService = null;

            Controller = new DocumentService(
                docRepo, 
                metaRepo,
                docTypeRepo,
                aclservice,
                userGroup, 
                roleRepo,
                companyRepo,
                tableRepo,
                fileConverter,
                userManager,
                filesystemFactory,
                notificationService,
                historyRepository,
                eventDispatcher,
                dataFactory,
                config,
                logger);
            if (await companyRepo.GetById(1) == null)
            {
                Company c = new Company();
                c.Id = 1;
                c.Description = "test";
                await companyRepo.Insert(c);
            }
        }

        [Test]
        public async Task Insert_EmptyDocument_ShouldBeOk()
        {
            //Arrange
            var doc = new CreateOrUpdateDocument();
            doc.CompanyId = 1;
            doc.Description = "Description";

            var di = await Controller.CreateAndRead(doc, u);
            Assert.IsTrue(di.Id > 0);
        }
        [Test]
        public async Task Insert_Folder_ShouldBeOk()
        {
            //Arrange
            var doc = new CreateOrUpdateDocument();
            doc.CompanyId = 1;
            doc.Description = "Fascicolo Personale";
            doc.ContentType = Domain.Enumerators.ContentType.Folder;
            var di = await Controller.Create(doc, u);
            Assert.IsTrue(di > 0);
        }

        [Test]
        public async Task Insert_Document_With_Referents_ShouldBeOk()
        {
            //Arrange
            var doc = new CreateOrUpdateDocument();
            doc.CompanyId = 1;
            doc.Description = "Description";
            doc.DocumentDate = DateTime.Now.AddDays(-2);
            doc.ReferentList = "0admin";
            doc.ReferentListCC= "0admin,0user";
            var di = await Controller.CreateAndRead(doc, u);
            Assert.IsTrue(di.Id > 0);
            Assert.IsTrue(di.ReferentList.Count == 1);
            Assert.IsTrue(di.ReferentListCC.Count == 1);
        }

        [Test]
        public async Task Insert_Document_With_Notify_ShouldBeOk()
        {
            //Arrange
            var doc = new CreateOrUpdateDocument();
            doc.CompanyId = 1;
            doc.Description = "Description";
            doc.DocumentDate = DateTime.Now.AddDays(-2);
            doc.ReferentList = "0admin";
            doc.ReferentListCC = "0admin,0user";

            doc.NotifyTo = "0admin,0user";
            doc.NotifyCC = "1gruppo 1,1gruppo 2,0admin,2admin";
            var di = await Controller.CreateAndRead(doc, u);
            Assert.IsTrue(di.Id > 0);
            Assert.IsTrue(di.ReferentList.Count == 1);
            Assert.IsTrue(di.ReferentListCC.Count == 1);
        }

        [Test]
        public async Task Insert_Document_With_Content_ShouldBeOk()
        {

            //Arrange
            var doc = new CreateOrUpdateDocument();
            doc.CompanyId = 1;
            doc.Description = "Documento con file";

            var di = await Controller.Create(doc, u);
            Assert.IsTrue(di > 0);
            var fc = new FileContent()
            {
                FileName = "texzt.pdf",
                FileData = Convert.ToBase64String(new byte[] { 0, 1, 2, 3, 4 , 5 , 6 , 7 }),
                LinkToContent = false
            };
            var im = await Controller.AddContent(di, u, fc, true);
            Assert.IsTrue(im.Id > 0);
            var d = await Controller.Load(di, u);
            Assert.IsTrue(d.Image.Id == im.Id);


        }

        [Test]
        public async Task Insert_Document_With_Fields_ShouldBeOk()
        {
            //Arrange
            var doc = new CreateOrUpdateDocument();
            doc.CompanyId = 1;
            doc.Description = "Documento con campi";
            doc.FieldList.Add(new AddOrUpdateDocumentField() { FieldTypeId = "$$t", Value = "testo" });
            doc.FieldList.Add(new AddOrUpdateDocumentField() { FieldTypeId = "$$p", Value = "paragrafo" });
            doc.FieldList.Add(new AddOrUpdateDocumentField() { FieldTypeId = "$$d", Value = "01/01/2022,31/12/2022" });
            doc.FieldList.Add(new AddOrUpdateDocumentField() { FieldTypeId = "$us", Value = "admin" });
            doc.FieldList.Add(new AddOrUpdateDocumentField() { FieldTypeId = "$gr", Value = "protocollo" });
            var di = await Controller.Create(doc, u);
            Assert.IsTrue(di > 0);
            var fc = new FileContent()
            {
                FileName = "texzt.pdf",
                FileData = Convert.ToBase64String(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                LinkToContent = false
            };
            var im = await Controller.AddContent(di, u, fc, true);
            Assert.IsTrue(im.Id > 0);
            var d = await Controller.Load(di, u);
            Assert.IsTrue(d.Image.Id == im.Id);


        }


        [Test]
        public async Task Insert_MultipleDocuments_ShouldBeOk()
        {
            //Arrange
            for (int i = 0; i < 1000; i++)
            {
                var doc = new CreateOrUpdateDocument();
                doc.CompanyId = 1;
                doc.Description = "Documento con file #" + i + "";

                var di = await Controller.Create(doc, u);
                Assert.IsTrue(di > 0);
                var fc = new FileContent()
                {
                    FileName = "texzt.pdf",
                    FileData = Convert.ToBase64String(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    LinkToContent = false
                };
                var im = await Controller.AddContent(di, u, fc, true);
                Assert.IsTrue(im.Id > 0);
                var d = await Controller.Load(di, u);
                Assert.IsTrue(d.Image.Id == im.Id);
            }
        }
    }
}