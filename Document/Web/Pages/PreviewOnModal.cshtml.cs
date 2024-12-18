using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using Web.DTOs;

namespace Web.Pages
{
    [Authorize]
    public class PreviewOnModalModel : PageModel
    {
        private readonly ILogger<PreviewOnModalModel> _logger;
        private readonly ILogger<IndexModel> _logger2;
        private readonly IDocumentService documentService;
        private readonly IDocumentTypeService documentTypeService;
        private readonly ILoggedUserProfile userContext;
        private readonly IUserService userService;
        private readonly IHistoryRepository historyRepo;
        private readonly IVirtualFileSystemProvider fileSystemProvider;

        public int DocumentId { get; set; }

        // public string? token { get; set; }
        public string Viewer { get; set; }
        public string Editor { get; set; }
        public string FileContent { get; set; }

        public DocumentInfo Document { get; set; }
        public FileExplorerModel FileFilters { get; set; }

      public bool CanRemoveContent { get; set; } = false;
        public bool CanShare { get; set; } = false;
        public bool CanAuthorize { get; set; } = false;
        public bool CanViewRegistry { get; set; } = false;
        public IndexModel IndexModel { get; set; }


        public PreviewOnModalModel(ILogger<PreviewOnModalModel> logger,
            IDocumentService documentService,
            IDocumentTypeService documentTypeService,
            ILoggedUserProfile userContext,
            IHistoryRepository historyRepo,
            ILogger<IndexModel> logger2,
            IVirtualFileSystemProvider fileSystemProvider,
            IUserService userService)
        {
            _logger = logger;
            this.documentService = documentService;
            this.documentTypeService = documentTypeService;
            this.userContext = userContext;
            this.historyRepo = historyRepo;
            _logger2 = logger2;
            this.fileSystemProvider = fileSystemProvider;
            this.userService = userService;
        }

        public async Task<IActionResult> OnGet(int? id)
        {
            DocumentId = id ?? 0;
            if (DocumentId <= 0)
            {
                return RedirectToPage("Folders/Index");
            }
            //token =  await this.HttpContext.GetTokenAsync("access_token");
            var u = userContext.Get();
            Document = await documentService.Load(DocumentId, u);

            if (Document.ContentType == ContentType.Folder)
            {
                Viewer = "Viewer_Files";
                Editor = "";
            }
            else
            {
                if (Document.Image != null)
                {
                    switch (Path.GetExtension(Document.Image?.FileName).ToLower())
                    {
                        case ".txt":
                        case ".htm":
                        case ".html":
                        case ".log":
                        case ".xml":
                        case ".json":
                        case ".eml":
                        case ".msg":
                        case ".md":
                            Viewer = "Viewer_HTML";
                            Editor = "";
                            break;
                        case ".bmp":
                        case ".jpg":
                        case ".jpeg":
                        case ".png":
                        case ".tiff":
                            Viewer = "Viewer_Image";
                            Editor = "";
                            break;

                        case ".bpmn":
                            Viewer = "Viewer_BPMN";
                            Editor = "modeler";
                            break;
                        case ".form":
                            Viewer = "Viewer_BPMN";
                            Editor = "form";
                            break;
                        case ".dmn":
                            Viewer = "Viewer_BPMN";
                            Editor = "dmn";
                            break;
                        default:
                            Viewer = "Viewer_PDF";
                            Editor = "";
                            break;
                    }
                }
                else
                {
                    Viewer = "Viewer_NoFile";
                    Editor = "";
                }
            }

            var viewId = "Doc.Folder_" + Document.DocumentType.Id;
            FileFilters = new FileExplorerModel();
            FileFilters.Title = "Documenti nel fascicolo";
            FileFilters.ShowRemoveFromFolder = CanRemoveContent;
            FileFilters.ShowDelete = true;
            FileFilters.Request.PageSize = 25;
            FileFilters.Request.PageSize = 0;
            FileFilters.Request.ViewId = viewId;
            FileFilters.Request.Filters = new List<SearchFilter>();
            if (DocumentId > 0)
                FileFilters.Request.Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.Parent, Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { DocumentId.ToString() } });
            else
                FileFilters.Request.Filters.Add(new SearchFilter() { ColumnName = DocumentColumn.ContentType, Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { ((int)ContentType.Folder).ToString() } });

            //FileFilters.Filters.Add(new SearchFilter() { ColumnName = "Status", Operator = OpenDMS.Domain.Enumerators.OperatorType.EqualTo, Values = new() { ((int)DocumentStatus.Active).ToString() } });
            await documentService.View(Document, u);



            //IndexModel = new IndexModel(
            //    _logger2,
            //documentService,
            //documentTypeService,
            //userContext,
            //userService,
            //historyRepo,
            //fileSystemProvider
            //)
            //{
            //    DocumentId = DocumentId,
            //    Viewer = Viewer,
            //    Editor = Editor,
            //    FileContent = FileContent,
            //    Document = Document,
            //    FileFilters = FileFilters,
            //    Versions = Versions,
            //    FoldersCount = FoldersCount,
            //    AttachmentsCount = AttachmentsCount,
            //    LinksCount = LinksCount,
            //    CanView = CanView,
            //    CanViewContent = CanViewContent,
            //    CanEdit = CanEdit,
            //    CanDelete = CanDelete,
            //    CanAddAttachment = CanAddAttachment,
            //    CanAddContent = CanAddContent,
            //    CanCreate = CanCreate,
            //    CanCreateDocument = CanCreateDocument,
            //    CanRemoveContent = CanRemoveContent,
            //    CanShare = CanShare,
            //    CanAuthorize = CanAuthorize,
            //    CanViewRegistry = CanViewRegistry
            //};

            return Page();
        }


        public async Task<IActionResult> OnGetFolders(int documentId)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            var Folders = new List<DocumentInfo>();
            foreach (var f in await documentService.GetDocumentFolders(documentId, u))
                Folders.Add(await documentService.Load(f, u));
            return new JsonResult(Folders);
        }


        public async Task<IActionResult> OnGetHistories(int documentId, int PageIndex, string SearchText, int Events)
        {
            HistoryFilters filters = new HistoryFilters()
            {
                DocumentId = documentId,
                PageSize = 10,
                Search = SearchText,
                PageIndex = PageIndex,
                Events = new List<string>()
            };
            return new JsonResult(await historyRepo.GetByFilters(filters));
        }

        public async Task<IActionResult> OnGetAttachments(int documentId)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            var Attachments = await documentService.Links(documentId, u, true);
            return new JsonResult(Attachments);
        }
        public async Task<IActionResult> OnGetRemoveAttachment(int documentId, int attachmentId)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            if (documentId != attachmentId)
            {
                var errors = await documentService.RemoveLink(documentId, attachmentId, u, true);
                if (!errors)
                {
                    return new JsonResult("Impossibile rimuovere l'allegato");
                }
            }
            return await OnGetAttachments(documentId);
        }



        public async Task<IActionResult> OnGetLinks(int documentId)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            var Links = await documentService.Links(documentId, u, false);
            return new JsonResult(Links);
        }
        public async Task<IActionResult> OnGetRemoveLink(int documentId, int linkId)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            if (documentId != linkId)
            {
                var errors = await documentService.RemoveLink(documentId, linkId, u, false);
                if (!errors)
                {
                    return new JsonResult("Impossibile rimuovere il collegamento");
                }
            }
            return await OnGetLinks(documentId);
        }


        public async Task<IActionResult> OnGetVersions(int documentId)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            var Versions = await documentService.Images(documentId, u);
            return new JsonResult(Versions);
        }
        public async Task<IActionResult> OnGetRestoreVersion(int documentId, int imageId)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            var Document = await documentService.Load(documentId, u);
            if (Document.Image.Id != imageId)
            {
                var contentInfo = await documentService.GetContentInfo(imageId);
                FileContent content = new FileContent() { FileName = contentInfo.OriginalFileName, FileData = Convert.ToBase64String(await documentService.GetContent(imageId)), LinkToContent = false };
                await documentService.AddContent(documentId, u, content, true);
            }
            return await OnGetLinks(documentId);
        }


        public async Task<IActionResult> OnGetPermissions(int documentId)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            var P = await documentService.GetDocumentPermissions(documentId);
            return new JsonResult(P);
        }







        public string FormatFileSize(long fileSize)
        {
            if (fileSize < 1000)
            {
                return fileSize.ToString() + " B";
            }
            else
                if (fileSize < 1000_000)
            {
                return (fileSize / 1000).ToString("##0") + " K";
            }
            else
                if (fileSize < 1000_000_000)
            {
                return (fileSize / 1000_000).ToString("##0") + " M";
            }
            else
            {
                return (fileSize / 1000_000_000).ToString("##0") + " G";
            }
        }

        //TODO: Spostare in un controller

        public async Task<IActionResult> OnGetRemoveFolders(string documents, int folderId)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            List<int> docs = new List<int>();
            foreach (var d in documents.Split(","))
                if (int.TryParse(d, out int i))
                    docs.Add(i);
            {
                var errors = await documentService.RemoveFromFolder(folderId, docs, u);
                if (errors != null)
                {
                    return new JsonResult(errors);
                }
            }
            return new JsonResult("");
        }
        public async Task<IActionResult> OnGetStoricize(string documents)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            foreach (var d in documents.Split(","))
                if (int.TryParse(d, out int i))
                {
                    await documentService.ChangeStatus(i, u, DocumentStatus.Stored);
                }
            return new JsonResult("");
        }
        public async Task<IActionResult> OnGetRestore(string documents)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            foreach (var d in documents.Split(","))
                if (int.TryParse(d, out int i))
                {
                    await documentService.UnDelete(i, u);
                }
            return new JsonResult("");
        }
        public async Task<IActionResult> OnGetDelete(string documents, string justification, bool recursive)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            foreach (var d in documents.Split(","))
                if (int.TryParse(d, out int i))
                {
                    await documentService.Delete(i, u, justification);
                }
            return new JsonResult("");
        }

        public async Task<IActionResult> OnGetDownload(int documentId, bool original)
        {
            var u = userContext.Get();
            //u = UserProfile.SystemUser();
            var d = await documentService.Load(documentId, u);
            if (d.Image == null)
                return NotFound();
            byte[] data = null;
            var name = d.Image.OriginalFileName;
            if (original)
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

        public class PostFileInfo
        {
            public int documentId { get; set; }
            public string filename { get; set; }
            public string data { get; set; }
        }
        public async Task<IActionResult> OnGetPaste(string documentIds, int folderId)
        {
            var u = userContext.Get();
            try
            {
                var docs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(documentIds);
                var errors = await documentService.AddToFolder(folderId, docs, u, false);
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
        public async Task<IActionResult> OnPostUploadFile([FromBody] PostFileInfo postfile)
        {
            var u = userContext.Get();
            FileContent content = new FileContent();
            content.FileData = postfile.data;
            content.DataIsInBase64 = true;
            content.LinkToContent = false;
            content.FileName = postfile.filename;
            var i = await documentService.AddContent(postfile.documentId, u, content, true);
            if (i != null)
                return new JsonResult("");
            else
                return new JsonResult("Si è verificato un errore imprevisto durante la memorizzazione del file");
        }

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
                return new JsonResult("");
            }
            else
                return new JsonResult("Si è verificato un errore imprevisto durante la memorizzazione del file");
        }
    }
}