using Core.DigitalSignature;
using Elmi.Core.FileConverters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.Domain.Services;
using OpenDMS.PdfManager;
using SixLabors.ImageSharp;
using System.Text;
using System.Text.RegularExpressions;

namespace Web.Controllers.Documents;

[Authorize]
[Route("internalapi/preview/")]
[ApiController]
public class PreviewController : ControllerBase
{
    const int image_BigSize = 1024;

    private readonly IDocumentService documentService;
    private readonly IDocumentTypeService documentTypeService;
    private readonly IUserService userService;
    private readonly ILoggedUserProfile userContext;
    private readonly IFileConvertFactory fileConverterFactory;
    private readonly IVirtualFileSystemProvider fileSystemProvider;
    private readonly ILogger<PreviewController> _logger;
    private readonly IConfiguration config;


    public PreviewController(
        ILogger<PreviewController> logger,
        IConfiguration config,
        IDocumentService documentService,
        IDocumentTypeService documentTypeService,
        IUserService userService,
        ILoggedUserProfile userContext,
        IFileConvertFactory fileConverterFactory,
        IVirtualFileSystemProvider fileSystemProvider
        )
    {

        this.documentService = documentService;
        this.documentTypeService = documentTypeService;
        this.userService = userService;
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
        if (!ext.EndsWith("html"))
            htmlName += ".html";

        if (await filesystem.Exists(htmlName))
        {
            FileContent = await filesystem.ReadAllText(htmlName);
        }
        else
        {
            try
            {
                var content = await documentService.GetContent(imageId);
                if (content != null && content.Length > 0)
                {
                    MemoryStream src = new MemoryStream(content);
                    MemoryStream M = (MemoryStream)src.VerifyAndExtract();
                    M.Position = 0;
                    string name = FName;
                    while (ext.Length > 0 && (ext.EndsWith(".m7m") || ext.EndsWith(".p7m") || ext.EndsWith(".tsa") || ext.EndsWith(".tsd")))
                    {
                        name = Path.GetFileNameWithoutExtension(name);
                        ext = Path.GetExtension(name).ToLower();
                    }
                    // se non ho estensione, caso tipico dei file p7m, provo a ricavare il tipo di file dal contenuto
                    if (string.IsNullOrEmpty(ext))
                    {
                        var Signatures = src.GetSignatureInfo();
                        if (Signatures.Length > 0)
                            ext = Signatures[Signatures.Length - 1].FileExtension;
                        else
                            ext = src.FindFileType();
                    }


                    // Provo a creare il pdf
                    var fileConverter = await fileConverterFactory.Get(ext, ".html");
                    //using (var M = new MemoryStream(content))
                    {
                        using (var htmldata = await fileConverter.Convert(ext, M))
                        {
                            using (var M2 = new MemoryStream())
                            {
                                htmldata.CopyTo(M2);
                                M2.Position = 0;
                                FileContent = Regex.Replace(
                                    Encoding.UTF8.GetString(M2.ToArray()),
                                    @"</?(?i:script|embed|object|frameset|frame|iframe|link)(.|\n|\s)*?>",
                                    string.Empty,
                                    RegexOptions.Singleline | RegexOptions.IgnoreCase
                                );
                            }
                            //await filesystem.WriteAllText (htmlName, FileContent);
                        }
                    }
                }
                else
                if (content == null) throw new Exception("Non è stato possibile leggere il file");
            } catch (Exception ex) {
                return ex.Message;
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
        try
        {
            var content = await documentService.GetContent(imageId);
            if (content == null) throw new Exception("Non è stato possibile leggere il file");

            MemoryStream src = new MemoryStream(content);
            MemoryStream M = (MemoryStream)src.VerifyAndExtract();

            return Convert.ToBase64String(M.ToArray());
        } catch (Exception ex)
        {
            return "";
        }
    }


    [HttpGet("Get")]
    public async Task<PreviewProperty> Get(int documentId, int imageId, string? Password, bool small = false)
    {
        PreviewProperty P = new PreviewProperty();

        var u = userContext.Get();
        var d = documentId; 
        var doc = await documentService.Get(d);
        if (imageId == 0 && doc.Image != null) imageId = doc.Image.Id;

        P.ErrorMessage = imageId > 0 ? "" : "Identificativo documento non valido";
        P.Pages = 0;


        if (imageId > 0)
        {
            var data = await documentService.GetContentInfo(imageId);
            var filesystem = await fileSystemProvider.InstanceOf(data.FileManager + "");
            var FName = data.FileName;
            var PdfName = FName;
            var ext = Path.GetExtension(PdfName).ToLower();
            if (ext != ".pdf")
            {
                PdfName += ".pdf";
            }

            //if (!await filesystem.Exists(PdfName))
            //{

            //    MemoryStream src = (MemoryStream)await filesystem.ReadAsStream(FName);
            //    MemoryStream M = (MemoryStream)src.VerifyAndExtract();

            //    string name = FName;
            //    while (ext.Length > 0 && (ext.EndsWith(".m7m") || ext.EndsWith(".p7m") || ext.EndsWith(".tsa") || ext.EndsWith(".tsd")))
            //    {
            //        name = Path.GetFileNameWithoutExtension(name);
            //        ext = Path.GetExtension(name).ToLower();
            //    }
                
            //    // se non ho estensione, caso tipico dei file p7m, provo a ricavare il tipo di file dal contenuto
            //    if (string.IsNullOrEmpty(ext))
            //    {
            //        var Signatures = src.GetSignatureInfo();
            //        if (Signatures.Length > 0)
            //            ext = Signatures[Signatures.Length - 1].FileExtension;
            //        else
            //            ext = src.FindFileType();
            //    }

            //    // Provo a creare il pdf
            //    var fileConverter = await fileConverterFactory.Get(ext, ".pdf");
            //    if (fileConverter != null)
            //    {
            //        try
            //        {
            //            //using (var M = await filesystem.ReadAsStream(FName))
            //            {
            //                //Stream M2 = (IsP7M) ? M.VerifyAndExtract() : M;
            //                using (var pdfdata = await fileConverter.Convert(ext, M))
            //                {
            //                    if (pdfdata != null)
            //                    {
            //                        pdfdata.Position = 0;
            //                        if (await filesystem.WriteFromStream(PdfName, pdfdata))
            //                        {
            //                            P.FileSize = pdfdata.Length;
            //                            P.FileExtension = ext;
            //                            P.FileName = imageId.ToString() + ".pdf";
            //                            P.PageType = PageType.Image;
            //                            P.FileIcon = "icoPdf";
            //                            P.CreationDate = DateTime.UtcNow;
            //                            P.MediaType = "application/pdf";
            //                            P.DefaultPageSize = new PageSize() { Width = 1050, Height = 1395 }; // ,675
            //                            return P;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        catch (Exception ex) {
            //            Console.Write(ex.Message);
            //        };
            //    }
            //}

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
                    "Non è stato possibile produrre l'anteprima di stampa del documento";
                P.FileExtension = ext;
                P.FileName = imageId.ToString() + ext;
                P.PageType = PageType.Image;
            }
            else
            {
                // Recupera l'anteprima PDF del documento
                try
                {
                    if (!small)

                    {
                        using (var M = await filesystem.ReadAsStream(PdfName))
                        {
                            using (var pdf = new PDFFile(M, Password))
                            {
                                P.Protected = string.IsNullOrEmpty(Password) && pdf.IsProtected();
                                P.Pages = P.Protected ? 1 : pdf.GetNumberOfPages();
                            }
                        }
                    }
                    else
                    {
                        P.Pages = 1;
                    }
                    P.FileSize = data.FileSize;
                    P.ErrorMessage = P.Protected ? "Contenuto protetto. Per visualizzarlo è necessario fornire una password" : "";
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
            P.DefaultPageSize = new PageSize() { Width = image_BigSize, Height = (int)Math.Round( image_BigSize / 0.7525) }; // ,675
        }
        P.FileIcon = "icoPdf";
        P.CreationDate = DateTime.UtcNow;
        P.MediaType = "application/pdf";
        return P;
    }


    public static string GetThumbnail(string FileName, ContentType ContentType, int Direction , JobStatus PreviewStatus = JobStatus.NotNeeded)
    {
        var img = ContentType == OpenDMS.Domain.Enumerators.ContentType.Folder
            ? "FolderFile.png"
            : ContentType == OpenDMS.Domain.Enumerators.ContentType.Form
            ? "FORMFile.png"
            : ContentType == OpenDMS.Domain.Enumerators.ContentType.DMN
            ? "DMNFile.png"
            : ContentType == OpenDMS.Domain.Enumerators.ContentType.Workflow
            ? "BPMNFile.png"
            : ContentType == OpenDMS.Domain.Enumerators.ContentType.Report
            ? "ReportFile.png"
            : ContentType == OpenDMS.Domain.Enumerators.ContentType.Template
            ? "TemplateFile.png"
            : "";
        if (string.IsNullOrEmpty(img))
        {
            var ext = Path.GetExtension(FileName);
            switch (ext)
            {
                case ".ifc": img = "BimFile.png"; break;
                case ".zip": img = "ZipFile.png"; break;
                case ".form": img = "FORMFile.png"; break;
                case ".formhtml": img = "FORMFile.png"; break;
                case ".formio": img = "FORMFile.png"; break;
                case ".bpmn": img = "BPMNFile.png"; break;
                case ".dmn": img = "BMNFile.png"; break;
                case ".eml": img = "MailFile.png"; break;
                case ".msg": img = "MailFile.png"; break;
                case ".mp4": img = "VideoFile.png"; break;
                case ".wav": img = "VideoFile.png"; break;
                case ".ogg": img = "VideoFile.png"; break;
                case ".report": img = "ReportFile.png"; break;
                case ".bmp":
                case ".png":
                case ".jpg":
                case ".jpeg": img = "ImageFile.png"; break;
                default:

                    if (PreviewStatus == JobStatus.Queued || PreviewStatus == JobStatus.Running)
                        img = "GeneratingPreview.png";
                    else
                        if (PreviewStatus == JobStatus.Completed || PreviewStatus == JobStatus.NotNeeded)
                    {
                        img = Direction == 0
                            ? "InternalFile.png"
                            :  Direction == 1
                            ? "InboxFile.png"
                            : "OutboxFile.png";
                    }
                    else
                        img = "NoPreview.png";
                    break;
            }


        }
        return img;
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
        var doc = await documentService.Get(d);
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

        if ((doc.PersonalData && small) || (await documentService.GetPermission(d, u, PermissionType.CanViewContent)).Authorization != AuthorizationType.Granted)
        {
            var b = System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "images", "previews", "Protected.png"));
            return File(b, "image/png", "Protected.png");
        };

        var data = await documentService.GetContentInfo(imageId);
        var filesystem = await fileSystemProvider.InstanceOf(data.FileManager);
        var FName = data.FileName;
        var PdfName = FName;
        var ext = Path.GetExtension(PdfName).ToLower();
        if (ext != ".pdf")
        {
            PdfName += ".pdf";
        }

        byte[] byteArray = null;
        string previewName = data.Id.ToString();
        if (!await filesystem.Exists(PdfName))
        {
            string img = PreviewController.GetThumbnail(ext, doc.ContentType, doc.DocumentType?.Direction ?? 2, doc.Image?.PreviewStatus ?? JobStatus.NotNeeded);
         
            if (!String.IsNullOrEmpty(img))
                byteArray = System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "images", "previews", img));
        }
        else
        {
            using (var src = await filesystem.ReadAsStream(PdfName))
            {
                using (var pdf = new PDFFile(src, Password))
                {
                    try
                    {
                        byteArray = await pdf.GetPageImage(PageIndex, small ? (int)(image_BigSize / 5) : image_BigSize);
                    }
                    catch (Exception)
                    {
                        byteArray = System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "images", "previews", "NoImage.png"));
                        previewName = "ImageNotFound";
                    }
                }
            }
        }
        //if (String.IsNullOrEmpty(Password)) return null;
        return File(byteArray, "image/png", previewName + "_" + Guid.NewGuid().ToString() + ".png");

    }


    /// <summary>
    /// Ritorna l'immagine in formato PNG di una pagina del documento
    /// </summary>
    /// <param name="imageId">Identificativo del contenuto del documento</param>
    /// <param name="PageIndex">Numero della pagina (base 1)</param>
    /// <param name="Password"></param>
    /// <returns></returns>
    [HttpGet("GetDocumentType")]
    public async Task<FileResult> GetDocumentType(string? documentTypeId, bool IsFolder = false)
    {
        if (documentTypeId == null) documentTypeId = "";
        var tipo = await documentTypeService.GetById(documentTypeId);
        if (IsFolder) tipo.ContentType = ContentType.Folder;
        byte[] byteArray = null;
        JobStatus status = JobStatus.NotNeeded;
        if (!String.IsNullOrEmpty(tipo.CreationFormKey) && tipo.ContentType == ContentType.Document)
        {
            var docId = await documentService.FindByUniqueId(null, tipo.CreationFormKey, ContentType.Form);
            if (docId > 0)
            {
                var doc = await documentService.Get(docId);
                if (doc.ImageId.HasValue)
                {
                    //status = JobStatus.Queued;
                    var pdfdata = await documentService.GetPreview(doc.Image);
                    if (pdfdata != null)
                    {
                        using (var src = new MemoryStream(pdfdata))
                        {
                            using (var pdf = new PDFFile(src, ""))
                            {
                                try
                                {
                                    byteArray = await pdf.GetPageImage(1, (int)(image_BigSize / 5) );
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }
                }
            }
        }
        string imageData = "";
        if (byteArray == null || byteArray.Length == 0)
        {
            byteArray = System.IO.File.ReadAllBytes (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "images", "previews", PreviewController.GetThumbnail("", tipo.ContentType, tipo.Direction, status)));
        }

        //if (String.IsNullOrEmpty(Password)) return null;
        return File(byteArray, "image/png", "doctype-"+tipo.Id + (IsFolder ? "1" : "0") + ".png");

    }





    [HttpGet("AddStamp")]
    public async Task<FileResult> AddStamp(int documentId, string stampId, int pageIndex, float xPercentage, float yPercentage)
    {
        if (documentId <= 0) throw new InvalidDataException("L'identificativo del documento non può essere 0");
        var u = userContext.Get();
        var doc = await documentService.Load(documentId, u);
        if (doc.ContentType != ContentType.Document) throw new InvalidDataException("Non è possibile aggiungere un'immagine ad un fascicolo");
        if (doc.Image == null || doc.Image.Id == 0) throw new InvalidDataException("Il documento non ha contenuto");

        var imageId = doc.Image.Id;
        var data = await documentService.GetContentInfo(imageId);
        var filesystem = await fileSystemProvider.InstanceOf(data.FileManager);
        var FName = data.FileName;
        var PdfName = FName;
        var ext = Path.GetExtension(PdfName).ToLower();
        if (ext != ".pdf")
        {
            PdfName += ".pdf";
        }
        if (await filesystem.Exists(PdfName))
        {
            MemoryStream stamp = new MemoryStream(await userService.GetUserStamp(u.userId, ProfileType.User, stampId));
            SixLabors.ImageSharp.Image Stamp = await Image.LoadAsync(stamp);
            if (Stamp != null)
            {
                //                try
                //                {
                using (MemoryStream src = new MemoryStream(await filesystem.ReadAllBytes(PdfName)))
                {
                    MemoryStream M = (MemoryStream)src.VerifyAndExtract();

                    using (OpenDMS.PdfManager.PDFFile pdf = new OpenDMS.PdfManager.PDFFile(M))
                    {
                        using (var NewData = await pdf.AddImage(pageIndex, Stamp, xPercentage, yPercentage, true, true))
                        {
                            if (NewData != null)
                            {
                                NewData.Seek(0, SeekOrigin.Begin);
                                FileContent FC = new FileContent();
                                FC.FileData = Convert.ToBase64String(((MemoryStream)NewData).ToArray());
                                FC.DataIsInBase64 = true;
                                FC.FileName = Path.GetFileName( PdfName);
                                var i = await documentService.AddContent(documentId, u, FC, false);
                                imageId = i.Id;
                            }

                        }
                    }
                }
                //                }
                //                catch (Exception ex)
                //                {
                //                }

            }
            else { 
                // Loggare la mancata applicazione dell'etichetta
            }

        }
        else throw new InvalidDataException("L'anteprima PDF non è disponibile pe rquesto documento");
        return await Get(documentId, imageId, pageIndex, "", false);
    }
    
    [HttpGet("AddSignField")]
    public async Task AddSignField(int documentId, int pageIndex, float xPercentage, float yPercentage, string nomeCampoFirma)
    {
        var u = userContext.Get();
        var doc = await documentService.Load(documentId, u);
        if (doc == null) throw new InvalidDataException("Nessun documento trovato per l'id: " + documentId);
        await documentService.AddBlankSignField(doc, userContext.Get(), pageIndex, xPercentage, yPercentage, nomeCampoFirma);
    }

    [HttpGet("AddStamps/{documentId}/{stampId}/{coordinates}")]
    public async Task AddStamps(int documentId, string stampId, string coordinates)
    {
        try
        {

            _logger.LogInformation($"AddStamps inputs : docId = {documentId} , stampId = {stampId} , coordinates = {coordinates}");

            if (coordinates == null) throw new Exception("Non è stato possibile leggere le coordinate.");
            var coords = coordinates.Split(',');

            coords = coords.SkipLast(1).ToArray();

            if (coords.Length <= 0) throw new Exception("La stringa delle coordinate passate non contiene nessuna coordinata.");

            foreach (var coord in coords)
            {
                var part = coord.Split('_');

                //'sign_1_VistoMark_39.24050632911393_15.40755467196819_23.90998593530239_7.057654075546719_dW5kZWZpbmVk_undefined_undefined'
                if (part.Length != 10)
                    throw new Exception("Una delle coordinate passate non è nel formato corretto.");

                int pageIndex = int.Parse(part[1]);
                float x = (float)(Double.Parse(part[3].Replace(".", ",")));
                float y = (float)(Double.Parse(part[4].Replace(".", ",")));
                //float dx = (float)(Double.Parse(part[5].Replace(".", ",")));
                //float dy = (float)(Double.Parse(part[6].Replace(".", ",")));

                await AddStamp(documentId, stampId, pageIndex, x, y);

            }

        }
        catch (Exception ex)
        {
            _logger.LogError($"AddStamps exception : {ex}");
        }
    }




}