using Elmi.Core.FileConverters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.DigitalSignature;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.PdfManager;
using System.Text;
using System.Text.RegularExpressions;
using com.sun.jdi.@event;
using OpenDMS.Domain.Constants;

namespace Web.Controllers.Documents;

[Authorize]
[Route("/api/preview/")]
[ApiController]
public class PreviewController : ControllerBase
{

    private readonly IDocumentService documentService;
    private readonly ILoggedUserProfile userContext;
    private readonly IFileConvertFactory fileConverterFactory;
    private readonly IVirtualFileSystemProvider fileSystemProvider;
    private readonly ILogger<PreviewController> _logger;
    private readonly IConfiguration config;


    public PreviewController(
        ILogger<PreviewController> logger,
        IConfiguration config,
        IDocumentService documentService,
        ILoggedUserProfile userContext,
        IFileConvertFactory fileConverterFactory,
        IVirtualFileSystemProvider fileSystemProvider
        )
    {

        this.documentService = documentService;
        this.userContext = userContext;
        this.fileConverterFactory = fileConverterFactory;
        this.fileSystemProvider = fileSystemProvider;
        _logger = logger;
        this.config = config;
    }

    [HttpGet("GetText/{imageId}")]
    public async Task<string> GetText(int imageId)
    {
        string FileContent = "";
        var info = await documentService.GetContentInfo(imageId);
        var filesystem = await fileSystemProvider.InstanceOf(info.FileManager);
        var FName = info.FileName;
        var htmlName = FName;
        var ext = Path.GetExtension(FName).ToLower();
        if (ext != ".html")
            htmlName += ".html";

        if (await filesystem.Exists(htmlName))
        {
            FileContent = await filesystem.ReadAllText(htmlName);
        }
        else
        {
            var content = await documentService.GetContent(imageId);
            if (content != null && content.Length > 0)
            {
                // Provo a creare il pdf
                var fileConverter = await fileConverterFactory.Get(ext, ".html");
                using (var M = new MemoryStream(content))
                {
                    using (var htmldata = await fileConverter.Convert(ext, M))
                    {
                        using (var M2 = new MemoryStream())
                        {
                            htmldata.CopyTo(M2);
                            M2.Position = 0;
                            FileContent = Regex.Replace(
                                Encoding.UTF8.GetString(M2.ToArray()),
                                @"</?(?i:script|embed|object|frameset|frame|iframe|meta|link|style)(.|\n|\s)*?>",
                                string.Empty,
                                RegexOptions.Singleline | RegexOptions.IgnoreCase
                            );
                        }
                        //await filesystem.WriteAllText (htmlName, FileContent);
                    }
                }
            }
        }


        return FileContent;
    }
    [HttpGet("GetBase64/{imageId}")]
    public async Task<string> GetBase64(int imageId)
    {
        string FileContent = "";
        var info = await documentService.GetContentInfo(imageId);
        var filesystem = await fileSystemProvider.InstanceOf(info.FileManager);
        var FName = info.FileName;
        var htmlName = FName;
        var ext = Path.GetExtension(FName).ToLower();
        var content = await documentService.GetContent(imageId);
        return Convert.ToBase64String(content);
    }


    [HttpGet("Get")]
    public async Task<PreviewProperty> Get(int documentId, int imageId, string? Password, bool small=false)
    {
        PreviewProperty P = new PreviewProperty();

        var u = userContext.Get();
        var d = documentId; // await documentService.GetDocumentsFromContentId(imageId, u);
        var doc = await documentService.Load(d, u);
        if (imageId == 0 && doc.Image != null) imageId = doc.Image.Id;
        //if (d.Count <= 0)
        //{
        //    P.ErrorMessage = "Non sei autorizzato ad accedere a questo contenuto";
        //    //Loggare l'evento
        //    P.Pages = 0;
        //}
        //else
        {
            P.ErrorMessage = imageId > 0 ? "" : "Identificativo documento non valido";

            var data = await documentService.GetContentInfo(imageId);
            var filesystem = await fileSystemProvider.InstanceOf(data.FileManager + "");
            var FName = data.FileName;
            var PdfName = FName;
            var IsP7M = false;
            var ext = Path.GetExtension(PdfName).ToLower();

            while (ext== ".p7m" || ext == ".m7m" || ext == ".tsd")
            {
                // estraggo il file originale
                IsP7M = true;
                PdfName = Path.Combine(Path.GetDirectoryName(PdfName), Path.GetFileNameWithoutExtension(PdfName));
                ext = Path.GetExtension(PdfName).ToLower();
            }

            if (ext != ".pdf")
            {
                PdfName += ".pdf";
            }
            //else
            //if (ext != ".pdf")
            //{
            //    PdfName += "preview.pdf";
            //}
            // Provo a generare una nuova preview
            if (!await filesystem.Exists(PdfName))
            {
                // Provo a creare il pdf
                var fileConverter = await fileConverterFactory.Get(ext, ".pdf");
                if (fileConverter != null)
                {
                    using (var M = await filesystem.ReadAsStream(FName))
                    {
                        Stream M2 = (IsP7M) ? M.VerifyAndExtract() : M;
                        try
                        {
                            using (var pdfdata = await fileConverter.Convert(ext, M2))
                            {
                                if (await filesystem.WriteFromStream(PdfName, pdfdata))
                                {
                                    P.FileSize = pdfdata.Length;
                                    P.FileExtension = ext;
                                    P.FileName = imageId.ToString() + ".pdf";
                                    P.PageType = PageType.Image;
                                    P.FileIcon = "icoPdf";
                                    P.CreationDate = DateTime.UtcNow;
                                    P.MediaType = "application/pdf";
                                    P.DefaultPageSize = new PageSize() { Width = 1050, Height = 1395 }; // ,675
                                    return P;
                                }
                            }
                        }
                        catch { };
                    }
                }
            }

            if (!await filesystem.Exists(PdfName))
            {
                P.FileSize = data.FileSize;
                P.Pages = 1;
                P.ErrorMessage =
                    data.IndexingStatus == JobStatus.Queued ? "Anteprima di stampa in corso di generazione"
                    :
                    data.IndexingStatus == JobStatus.Running ? "Anteprima di stampa non corso di generaizone"
                    :
                    data.IndexingStatus == JobStatus.Completed ? "Anteprima di stampa non disponibile"
                    :
                    data.IndexingStatus == JobStatus.Aborted ? "Anteprima di stampa non disponibile"
                    :
                    data.IndexingStatus == JobStatus.Ignored ? "Anteprima di stampa non disponibile"
                    :
                    "Non � stato possibile produrre l'anteprima di stampa del documento";
                P.FileExtension = ext;
                P.FileName = imageId.ToString() + ext;
                P.PageType = PageType.Image;
            }
            else
            {
                // Recupera l'anteprima PDF del documento
                try
                {
                    using (var M = await filesystem.ReadAsStream(PdfName))
                    {
                        using (var pdf = new PDFFile(M, Password))
                        {
                            P.Protected = string.IsNullOrEmpty(Password) && pdf.IsProtected();
                            P.Pages = P.Protected ? 1 : pdf.GetNumberOfPages();
                        }
                    }
                    P.FileSize = data.FileSize;
                    P.ErrorMessage = P.Protected ? "Contenuto protetto. Per visualizzarlo � necessario fornire una password" : "";
                    P.FileExtension = ext;
                    P.FileName = imageId.ToString() + ".pdf";
                    P.PageType = PageType.Image;
                }
                catch (Exception ex)
                {
                    P.Protected = false;
                    P.ErrorMessage = ex.Message;
                    P.Pages = 0;
                }

            }
            P.DefaultPageSize = new PageSize() { Width = 1000, Height = 675 }; // ,675
        }
        P.FileIcon = "icoPdf";
        P.CreationDate = DateTime.UtcNow;
        P.MediaType = "application/pdf";
        return P;
    }

    /// <summary>
    /// Ritorna l'immagine in formato PNG di una pagina del documento
    /// </summary>
    /// <param name="imageId">Identificativo del contenuto del documento</param>
    /// <param name="PageIndex">Numero della pagina (base 1)</param>
    /// <param name="Password"></param>
    /// <returns></returns>
    [HttpGet("GetPage")]
    public async Task<FileResult> Get(int documentId, int imageId, int PageIndex, string? Password, bool small = false)
    {
        var u = userContext.Get();
        var d = documentId; // await documentService.GetDocumentsFromContentId(imageId, u);
        var doc = await documentService.Load(d, u);
        //if (d.Count <= 0) BadRequest("Non sei autorizzato ad accedere a questo contenuto");
        if (imageId == 0 && doc.Image != null) imageId = doc.Image.Id;
        if (imageId <= 0)
        {
            if (doc.ContentType == ContentType.Folder)
            {
                var b1 = System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "images", "previews", "FolderFile.png"));
                return File(b1, "image/png", "FolderFile.png");
            }
            else
            {
                var b = System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "images", "previews", "NoImage.png"));
                return File(b, "image/png", "NoImage.png");
            }
        };

        if (doc.PersonalData || (await documentService.GetPermission(d, u, PermissionType.CanViewContent)).Authorization != AuthorizationType.Granted)
        {
            var b = System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "images", "previews", "Protected.png"));
            return File(b, "image/png", "Protected.png");
        };

        var data = await documentService.GetContentInfo(imageId);
        var filesystem = await fileSystemProvider.InstanceOf(data.FileManager);
        var FName = data.FileName;
        var PdfName = FName;
        var IsP7M = false;
        var ext = Path.GetExtension(PdfName).ToLower();
        while (ext == ".p7m" || ext == ".m7m" || ext == ".tsd")
        {
            // estraggo il file originale
            IsP7M = true;
            PdfName = Path.Combine(Path.GetDirectoryName(PdfName), Path.GetFileNameWithoutExtension(PdfName));
            ext = Path.GetExtension(PdfName).ToLower();
        }
        if (ext != ".pdf")
        {
            PdfName += ".pdf";
        }

        byte[] byteArray = null;
        if (!await filesystem.Exists(PdfName))
        {
            string img = "";
            //TODO: Riproduco una immagine specifica a seconda del tipo di file
            switch (ext)
            {
                case ".ifc": img = "BimFile"; break;
                case ".zip": img = "ZipFile"; break;
                case ".form": img = "FORMFile"; break;
                case ".bpmn": img = "BPMNFile"; break;
                case ".dmn": img = "BMNFile"; break;
                default:
                    img = "NoPreview"; break;
                    break;
            }
            byteArray = System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "images", "previews", img + ".png"));
        }
        else
        {
            // Recupera l'anteprima PDF del documento
            using (var M = await filesystem.ReadAsStream(PdfName))
            {
                Stream M2 = (IsP7M) ? M.VerifyAndExtract() : M;
                using (var pdf = new PDFFile(M2, Password))
                {
                    byteArray = await pdf.GetPageImage(PageIndex, small ? 210 : 1050);
                }
            }
        }
        //if (String.IsNullOrEmpty(Password)) return null;
        return File(byteArray, "image/png", data.Id + ".png");

    }




}