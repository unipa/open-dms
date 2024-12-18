using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.Infrastructure.Repositories;
using OpenDMS.MultiTenancy;
using OpenDMS.MultiTenancy.DbContexts;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Infrastructure.Database.Tests
{
    public class aclRepository_Tests
    {
        private ApplicationDbContextFactory factory;
        private ACLRepository repo;

        [SetUp]
        public void Setup()
        {
            var T = new Tenant() { Provider = "memory", ConnectionString = "test" };
            MasterTenantDbContextFactory mt = new MasterTenantDbContextFactory(()=> { return T;  });
            var tenantGetter = new TenantContext(mt);
            var loggerdb = new Microsoft.Extensions.Logging.Abstractions.NullLogger<MultiTenantDbContext>();
            factory = new ApplicationDbContextFactory(tenantGetter, loggerdb);
            repo = new ACLRepository(factory);
        }

        [Test]
        public async Task Add_shouldFail2()
        {
            var C = new ACL() { Id = "001", Name = "acl 1" };
            var i = await repo.Insert(C);
            try
            {
                var C2 = new ACL() { Id = "001", Name = "acl 1" };
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
            var C = new ACL() { Id = "002", Name = "acl 2" };
            await repo.Insert(C);
            var C2 = new ACL() { Id = "003", Name = "acl 3" };
            Assert.IsTrue((await repo.Insert(C2)) > 0);
        }
        [Test]
        public async Task GetById_shouldBeOk()
        {
            var C = new ACL() { Id = "004", Name = "acl 4" };
            var i = await repo.Insert(C);
            var C2 = new ACL() { Id = "005", Name = "acl 5" };
            i = await repo.Insert(C2);

            var C3 = await repo.GetById("004");
            Assert.IsTrue(C3 != null && C3.Id == C.Id && C3.Id != C2.Id);
        }

        [Test]
        public async Task Edit_shouldBeOk()
        {
            var C = new ACL() { Id = "006", Name = "acl 6" };
            C.Permissions.Add(new ACLPermission()
            {
                ACLId = "006",
                ProfileId = "user1",
                ProfileType =ProfileType.User,
                PermissionId = PermissionType.CanView,
                Authorization = AuthorizationType.None
            });
            await repo.Insert(C);

            var C2 = new ACL() { Id = "007", Name = "acl 7" };
            await repo.Insert(C2);

            var C3 = await repo.GetById("006");
            var P3 = await repo.GetPermission("006", "user1", ProfileType.User, PermissionType.CanView);
            C3.Name = "acl 6";
            P3.Authorization = AuthorizationType.Granted;
            await repo.ChangePermission(P3);
            C3.Permissions.Add(new ACLPermission()
            {
                ACLId = "006",
                ProfileId = "user2",
                ProfileType = ProfileType.User,
                PermissionId = PermissionType.CanView,
                Authorization = AuthorizationType.Denied
            });
            Assert.IsTrue((await repo.Update(C3)) > 0);
        }

        [Test]
        public async Task GetAll_shouldreturn()
        {
            var C = new ACL() { Id = "008", Name = "acl 8" };
            await repo.Insert(C);
            var C2 = new ACL() { Id = "009", Name = "acl 9" };
            await repo.Insert(C2);
            var C3 = await repo.GetAll();
            Assert.IsTrue(C3.Count() >= 2);
        }
        [Test]
        public async Task Delete_shouldBeOk()
        {
            var C = new ACL() { Id = "010", Name = "acl 10" };
            await repo.Insert(C);
            var C2 = new ACL() { Id = "011", Name = "acl 11" };
            await repo.Insert(C2);
            var acl = await repo.GetById("010");
            Assert.IsTrue(await repo.Delete(acl) > 0);
        }
        [Test]
        public async Task AddAuthorization_shouldBeOk()
        {
            var C = new ACL() { Id = "012", Name = "acl 12" };
            C.Permissions.Add(new ACLPermission()
            {
                ACLId = "012",
                ProfileId = "user1",
                ProfileType = ProfileType.User,
                PermissionId = PermissionType.CanView,
                Authorization = AuthorizationType.Granted
            });
            C.Permissions.Add(new ACLPermission()
            {
                ACLId = "012",
                ProfileId = "user1",
                ProfileType = ProfileType.User,
                PermissionId = PermissionType.CanDelete,
                Authorization = AuthorizationType.Denied
            });
            await repo.Insert(C);
            var acl = await repo.GetById("012");
            var p = await repo.GetAllPermissions("012");
            var hasacl = p != null && p.Count > 1;
            if (!hasacl) Assert.Fail("Non ha caricato le autorizzazioni");
            var canred = p[0].Authorization == AuthorizationType.Granted;
            var candel = p[1].Authorization == AuthorizationType.Denied;
            Assert.IsTrue(canred && candel);
        }



        [Test]
        public async Task AddAuthorization2_shouldBeOk()
        {
            var C = new ACL() { Id = "013", Name = "acl 13" };
            C.Permissions.Add(new ACLPermission()
            {
                ACLId = "013",
                ProfileId = "user1",
                ProfileType = ProfileType.User,
                PermissionId = PermissionType.CanView,
                Authorization = AuthorizationType.Granted
            });
            C.Permissions.Add(new ACLPermission()
            {
                ACLId = "013",
                ProfileId = "user1",
                ProfileType = ProfileType.User,
                PermissionId = PermissionType.CanDelete,
                Authorization = AuthorizationType.Denied
            });
            await repo.Insert(C);
            var acl = await repo.GetById("013");
            ACLPermission au = new ACLPermission();
            au.ACLId = "013";
            au.PermissionId = PermissionType.CanAddContent;
            au.ProfileId = "user3";
            au.ProfileType = ProfileType.User;
            var i = await repo.AddPermission(au);

            acl = await repo.GetById("013");
            var p = await repo.GetAllPermissions("013");
            var hasacl = p != null && p.Count > 0;
            if (!hasacl) Assert.Fail("Non ha caricato le autorizzazioni");
            var haspemissions = p.FirstOrDefault(a => a.ProfileId == "user3") != null;
            Assert.IsTrue(haspemissions);
        }


        [Test]
        public async Task GetAuthorization_shouldBeOk()
        {
            var C = new ACL() { Id = "014", Name = "acl 14" };
            C.Permissions.Add(new ACLPermission()
            {
                ACLId = "014",
                ProfileId = "user1",
                ProfileType = ProfileType.User,
                PermissionId = PermissionType.CanView,
                Authorization = AuthorizationType.Granted
            });
            C.Permissions.Add(new ACLPermission()
            {
                ACLId = "014",
                ProfileId = "user1",
                ProfileType = ProfileType.User,
                PermissionId = PermissionType.CanDelete,
                Authorization = AuthorizationType.Denied
            });
            await repo.Insert(C);
            var acl = await repo.GetById("014");
            ACLPermission au = await repo.GetPermission("014","user1", ProfileType.User, PermissionType.CanDelete);
            var hasacl = au != null && au.Authorization == AuthorizationType.Denied;
            if (!hasacl) Assert.Fail("Non ha caricato le autorizzazioni");
            var haspemissions = au != null;
            Assert.IsTrue(haspemissions);
        }

        [Test]
        public async Task UpdateAuthorization_shouldBeOk()
        {
            var C = new ACL() { Id = "015", Name = "acl 15" };
            C.Permissions.Add(new ACLPermission()
            {
                ACLId = "015",
                ProfileId = "user1",
                ProfileType = ProfileType.User,
                PermissionId = PermissionType.CanView,
                Authorization = AuthorizationType.Granted
            });
            C.Permissions.Add(new ACLPermission()
            {
                ACLId = "015",
                ProfileId = "user1",
                ProfileType = ProfileType.User,
                PermissionId = PermissionType.CanDelete,
                Authorization = AuthorizationType.Denied
            });
            await repo.Insert(C);
            var acl = await repo.GetById("015");
            ACLPermission au = await repo.GetPermission("015", "user1", ProfileType.User, PermissionType.CanView);
            au.Authorization = AuthorizationType.Denied;
            var i = await repo.ChangePermission(au);
            if (i == 0) Assert.Fail("Non ha aggiornato le autorizzazioni");
            ACLPermission au2 = await repo.GetPermission("015", "user1", ProfileType.User, PermissionType.CanView);
            Assert.IsTrue(au2.Authorization == AuthorizationType.Denied);
        }
    }
}