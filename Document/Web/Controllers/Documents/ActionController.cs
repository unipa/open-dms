using Core.DigitalSignature;
using DocumentFormat.OpenXml.Wordprocessing;
using java.nio.file.spi;
using java.time;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;

namespace Web.Controllers.Documents
{

    [Authorize]
    [ApiController]
    [Route("internalapi/action")]

    public class ActionController : ControllerBase
    {
        private readonly ILoggedUserProfile userContext;
        private readonly IUserService userService;
        private readonly IUserSettingsRepository userSettingsRepository;
        private readonly IVirtualFileSystemProvider fileSystemProvider;
        private readonly IDocumentService documentService;

        public ActionController(
                    ILoggedUserProfile userContext,
                    IUserService userService,
                    IUserSettingsRepository userSettingsRepository,
                    IVirtualFileSystemProvider fileSystemProvider,
                    IDocumentService documentService

            )
        {
            this.userContext = userContext;
            this.userService = userService;
            this.userSettingsRepository = userSettingsRepository;
            this.fileSystemProvider = fileSystemProvider;
            this.documentService = documentService;
        }


        [HttpGet("ClientSecret/{appName}/{renew}")]
        public async Task<string> OnGetClientSecret(string appName, bool renew = false)
        {
            var u = userContext.Get();
            var user = await userService.GetById(u.userId);
            var clientSecret = await userSettingsRepository.Get(user.ContactId, "ClientSecret." + appName);
            if (string.IsNullOrEmpty(clientSecret) || renew)
            {
                clientSecret = Guid.NewGuid().ToString();
                await userSettingsRepository.Set(user.ContactId, "ClientSecret." + appName, clientSecret);
            }
            return clientSecret + "-" + user.Id;
        }



        [HttpGet("RemoveFolders")]
        public async Task<IActionResult> OnGetRemoveFolders(string documents, int folderId)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            var docs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(documents);
            if (docs != null)
                    {
                        var errors = await documentService.RemoveFromFolder(folderId, docs, u);
                        if (errors != null)
                        {
                            return new JsonResult(errors);
                        }
                    }
            return new JsonResult("");
        }

        [HttpGet("Storicize")]
        public async Task<IActionResult> OnGetStoricize(string documents)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            var docs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(documents);
            if (docs != null)
                foreach (var i in docs)
                    if (i > 0)
                {
                    await documentService.ChangeStatus(i, u, DocumentStatus.Stored);
                }
            return new JsonResult("");
        }

        [HttpGet("Restore")]
        public async Task<IActionResult> OnGetRestore(string documents)
        {
            var u = userContext.Get();
            var docs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(documents);
            if (docs != null)
                foreach (var i in docs)
                    if (i > 0)
                    {
                        await documentService.UnDelete(i, u);
                }
            return new JsonResult("");
        }

        [HttpGet("Activate")]
        public async Task<IActionResult> OnGetActive(string documents)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            var docs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(documents);
            if (docs != null)
                foreach (var i in docs)
                    if (i > 0)
                    {
                        await documentService.ChangeStatus(i, u, DocumentStatus.Active);
                    }
            return new JsonResult("");
        }
        [HttpGet("Draft")]
        public async Task<IActionResult> OnGetDraft(string documents)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            var docs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(documents);
            if (docs != null)
                foreach (var i in docs)
                    if (i > 0)
                    {
                        await documentService.ChangeStatus(i, u, DocumentStatus.Draft);
                    }
            return new JsonResult("");
        }



        [HttpGet("Delete")]
        public async Task<IActionResult> OnGetDelete(string documents, string justification, int recursive)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            var docs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(documents);
            var NotDeleted = 0;
            if (docs != null)
                foreach (var i in docs)
                    if (i > 0)
                    {
                        try
                        {
                            await documentService.Delete(i, u, justification, recursive > 0);
                        }
                        catch 
                        {
                            NotDeleted++; ;
                        }
                    }
            return new JsonResult(NotDeleted);
        }

        [HttpGet("Download")]
        public async Task<IActionResult> OnGetDownload(int documentId, string? extension)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            var d = await documentService.Load(documentId, u);
            if (d.Image == null)
                return NotFound();
            byte[] data = null;
            var name = d.Image.OriginalFileName;
            if (string.IsNullOrEmpty(extension))
            {
                data = await documentService.GetContent(d.Image.Id);
            }
            else
            {

                var filesystem = await fileSystemProvider.InstanceOf(d.Image.FileManager);
                var FName = d.Image.FileName;
                var PdfName = FName;
                var ext = Path.GetExtension(FName).ToLower();
                if (ext != ".pdf")
                {
                    PdfName += ".pdf";
                }
                if (!(await filesystem.Exists(PdfName)))
                {

                    NotFound();
                }
                else
                {
                    data = await filesystem.ReadAllBytes(PdfName);
                }
                name = PdfName;
            }
            //TODO: rinominare il file secondo le specifiche della tipologia
            return File(data, "application/octet-stream", Path.GetFileName(name));
        }



        [HttpGet("Convert")]
        public async Task<IActionResult> OnGetConvert(int documentId, string? extension)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            var d = await documentService.Load(documentId, u);
            if (d.Image == null)
                return NotFound();
            int imageId = 0;
            var name = d.Image.OriginalFileName;
            if (!string.IsNullOrEmpty(extension))
            {
                var image = await documentService.ConvertTo(documentId, extension);
                if (image != null)
                {
                    imageId = image.Id; 
                }
            }
            return Ok (imageId);
        }



        [HttpPost("UploadSigned")]
        public async Task<IActionResult> OnUploadSigned([FromBody] PostFileInfo postfile)
        {
            var u = userContext.Get();
            var su = UserProfile.SystemUser();
            FileContent content = new FileContent();
            content.FileData = postfile.data;
            content.DataIsInBase64 = true;
            content.LinkToContent = false;
            content.FileName = postfile.filename;

            var fileData = Convert.FromBase64String(postfile.data);
            using (var M = new MemoryStream(fileData))
            {
                var Info = M.GetSignatureInfo();
                if (Info == null || Info.Count() <= 0)
                {
                    return new JsonResult(new { Error = "Il file fornito non è firmato o non è possibile leggere i dati di firma" });
                }
            }
            string hash = "";
            if (postfile.documentId == 0)
            {
                CreateOrUpdateDocument doc = new CreateOrUpdateDocument();
                if (postfile.document != null)
                    doc = postfile.document;
                else
                {
                    doc.Description = Path.GetFileName(content.FileName);
                    doc.DocumentDate = DateTime.UtcNow.Date;
                }
                if (string.IsNullOrEmpty(doc.Description)) doc.Description = Path.GetFileName(content.FileName);
                if (doc.DocumentDate is null) doc.DocumentDate = DateTime.UtcNow.Date;
                if (doc.FolderId == 0) doc.FolderId = postfile.folderId;
                doc.ContentType = ContentType.Document;
                doc.Owner = u.userId;

                var folder = await documentService.Create(doc, UserProfile.SystemUser());
                postfile.documentId = folder;
            }

            var i = await documentService.AddContent(postfile.documentId, su, content, true);
            if (i != null)
            {
                await documentService.UpdateSignatureStatus(i.Id, JobStatus.Completed, u, "Upload:"+DateTime.UtcNow.ToString("s"));
                return new JsonResult(new { DocumentId = postfile.documentId, Image = i });
            }
            else
                return new JsonResult(new { Error = "Si è verificato un errore imprevisto durante la memorizzazione del file" });
        }


        public class PostFileInfo
        {
            public int documentId { get; set; }
            public int folderId { get; set; }
            public string filename { get; set; }
            public string data { get; set; }
            public CreateOrUpdateDocument? document { get; set; }

        }

        [HttpGet("Paste")]
        public async Task<IActionResult> OnGetPaste(string documentIds, int folderId, bool cut)
        {
            var u = userContext.Get();
            try
            {
                var docs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(documentIds);
                var errors = await documentService.AddToFolder(folderId, docs, u, cut);
                if (errors != null)
                {
                    return new JsonResult(errors);
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);

            }
            return new JsonResult("");

        }


        [HttpGet("PasteAsLink")]
        public async Task<IActionResult> OnGetPasteAsLink(string documentIds, int masterId, bool isAttachment)
        {
            int ok = 0;
            var u = userContext.Get();
            try
            {
                var docs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(documentIds);
                foreach (var d in docs)
                {
                    var errors = await documentService.AddLink(masterId, d, u, isAttachment);
                    if (!errors)
                    {
                        ok++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return new JsonResult(ok);

        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> OnPostUploadFile([FromBody] PostFileInfo postfile)
        {
            var u = userContext.Get();
            FileContent content = new FileContent();
            content.FileData = postfile.data;
            content.DataIsInBase64 = true;
            content.LinkToContent = false;
            content.FileName = postfile.filename;
            string hash = "";
            if (postfile.documentId == 0)
            {
                CreateOrUpdateDocument doc = new CreateOrUpdateDocument();
                if (postfile.document != null)
                    doc = postfile.document;
                else
                {
                    doc.Description = Path.GetFileName(content.FileName);
                    doc.DocumentDate = DateTime.UtcNow.Date;
                }
                if (string.IsNullOrEmpty(doc.Description)) doc.Description = Path.GetFileName(content.FileName);
                if (doc.DocumentDate is null) doc.DocumentDate = DateTime.UtcNow.Date;
                if (doc.FolderId == 0) doc.FolderId = postfile.folderId;
                doc.ContentType = ContentType.Document;
                doc.Owner = u.userId;

                var folder = await documentService.Create (doc, UserProfile.SystemUser());
                postfile.documentId = folder;
            }

            var i = await documentService.AddContent(postfile.documentId, UserProfile.SystemUser(), content, true);
            if (i != null)
                return new JsonResult(new { DocumentId= postfile.documentId, Image =i });
            else
                return new JsonResult( new { Error = "Si è verificato un errore imprevisto durante la memorizzazione del file" });
        }

        [HttpPost("UploadAttachment")]
        public async Task<IActionResult> OnPostUploadAttachment([FromBody] PostFileInfo postfile)
        {
            var u = userContext.Get();

            CreateOrUpdateDocument doc = new CreateOrUpdateDocument();
            doc.Description = Path.GetFileName(postfile.filename);
            doc.ContentType = ContentType.Document;
            var newDoc = await documentService.Create(doc, u);
            //var document = await _repository.GetById(folder.Id);
            //documentId = folder.Id;


            FileContent content = new FileContent();
            content.FileData = postfile.data;
            content.DataIsInBase64 = true;
            content.LinkToContent = false;
            content.FileName = postfile.filename;

            var i = await documentService.AddContent(newDoc, u, content, true);
            if (i != null)
            {
                await documentService.AddLink(postfile.documentId, newDoc, u, true);
                return new JsonResult(new { DocumentId = postfile.documentId, ImageId = i, AttachmentId=newDoc });
            }
            else
                return new JsonResult(new { Error = "Si è verificato un errore imprevisto durante la memorizzazione del file" });
        }


    }
}
