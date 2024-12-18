using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Infrastructure.Database.DbContext;
using OpenDMS.Infrastructure.Repositories;
using OpenDMS.MultiTenancy;
using OpenDMS.MultiTenancy.DbContexts;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Infrastructure.Database.Tests
{
    public class documentRepository_Tests
    {
        private ApplicationDbContextFactory factory;
        private DocumentRepository repo;

        [SetUp]
        public void Setup()
        {
            var T = new Tenant() { Provider = "memory", ConnectionString = "test" };
            MasterTenantDbContextFactory mt = new MasterTenantDbContextFactory(() => { return T; });
            var tenantGetter = new TenantContext(mt);
            var loggerdb = new Microsoft.Extensions.Logging.Abstractions.NullLogger<MultiTenantDbContext>();
            factory = new ApplicationDbContextFactory(tenantGetter, loggerdb);
            repo = new DocumentRepository(factory);
        }

        [Test]
        public async Task Add_shouldFail2()
        {
            var old = await repo.FindByUniqueId("", "1", ContentType.Document);
            if (old != null) await repo.Delete(old, "", "", false);
            var C = new Document() { Description = "documento demo 1", ExternalId="1" };
            await repo.Create(C);
            try
            {
                var C2 = new Document() { Description = "documento demo 2", ExternalId = "1" };
                await repo.Create(C2);
            }
            catch (Exception ex)
            {
                Assert.Pass();
            }
            Assert.Fail("non ha dato errore di id duplicato");
        }


        [Test]
        public async Task CreateUnClassified_shouldBeOk()
        {
            var C = new Document() { Description = "add document", };
            await repo.Create(C);
            Assert.IsTrue(C.Id > 0);
        }

        [Test]
        public async Task AddImage_shouldBeOk()
        {
            var C = new Document() { Description = "add document with image", };
            var img = new DocumentImage() { FileName = "test.pdf", OriginalFileName = "test.pdf", OriginalPath = "test.pdf" };
            await repo.Create(C);
            var i = await repo.AddImage(C.Id, img, "test user");
            Assert.IsTrue(i > 0 && C.Id > 0 && img.Id > 0);
        }

        [Test]
        public async Task AddSecondImageAndAttachments_shouldBeOk()
        {
            var Att = new Document() { Description = "Attachment" };
            await repo.Create(Att);
            var C = new Document() { Description = "add document with 2 images and Attachments" };
            var img = new DocumentImage() { FileName = "test.pdf", OriginalFileName = "test.pdf", OriginalPath = "test.pdf" };
            var img2 = new DocumentImage() { FileName = "test2.pdf", OriginalFileName = "test2.pdf", OriginalPath = "test2.pdf" };
            await repo.Create(C);

            var i = await repo.SaveChanges();
            await repo.AddLink(C.Id, Att.Id, "utente attahment", true);
            i = await repo.AddImage(C.Id, img, "test user");
            i = await repo.AddImage(C.Id, img2, "test user 2");

            var d = await repo.GetById(C.Id);
            var atts = await repo.GetLinks(d.Id, true);
            var links = await repo.GetLinks(d.Id, false);
            //var attedtos = await repo.GetLinkedToDocuments(d.Id, true);
            //var linkedtos = await repo.GetLinkedToDocuments(d.Id, false);

            Assert.IsTrue(i > 0 && d.Id > 0 && img2.Id > 0);
        }
   
      



        [Test]
        public async Task Edit_shouldBeOk()
        {
            var C = new Document() { Description = "add document to edit", };
            await repo.Create(C);
            var i = await repo.SaveChanges();
            var C3 = await repo.GetById(C.Id);
            C3.Description = "->Edited";
            C3.DocumentDate = DateTime.Now;
            await repo.Update(C3);
            C3 = await repo.GetById(C.Id);
            Assert.IsTrue(C3.Description == "->Edited");
        }

        [Test]
        public async Task Delete_shouldBeOk()
        {
            var C = new Document() { Description = "add document to delete", };
            await repo.Create(C);
            var i = await repo.SaveChanges();
            await repo.Delete(C.Id, "utente test", "test", true);
            var C3 = await repo.GetById(C.Id);
            if (C3.DocumentStatus != DocumentStatus.Deleted) Assert.Fail("Documento non cancellato");
            await repo.Delete(C.Id, "utente test", "test", false);
            var C4 = await repo.GetById(C.Id);
            Assert.IsTrue(C4==null);
        }
    }
}