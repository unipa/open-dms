using Core.DigitalSignature;
using Elmi.Core.FileConverters;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.PdfManager;
using SixLabors.ImageSharp;
using Microsoft.Extensions.Logging;

namespace OpenDMS.Infrastructure.Services.BusinessLogic
{
    public class PreviewGenerator : IPreviewGenerator
    {
        private readonly IDocumentService documentService;
        private readonly IVirtualFileSystemProvider fileSystemProvider;
        private readonly IFileConvertFactory fileConverterFactory;
        private readonly ILogger<PreviewGenerator> logger;
        private readonly IUserService userService;

        public PreviewGenerator(
            IDocumentService documentService,
            IVirtualFileSystemProvider fileSystemProvider,
            IFileConvertFactory fileConverterFactory,
            ILogger<PreviewGenerator> logger,
            IUserService userService
            )
        {
            this.documentService = documentService;
            this.fileSystemProvider = fileSystemProvider;
            this.fileConverterFactory = fileConverterFactory;
            this.logger = logger;
            this.userService = userService;
        }

        public async Task<bool> HasPreview(int imageId)
        {
            var data = await documentService.GetContentInfo(imageId);
            var filesystem = await fileSystemProvider.InstanceOf(data.FileManager + "");
            var FName = data.FileName;
            var PdfName = FName;
            var ext = Path.GetExtension(PdfName).ToLower();

            while (ext == ".p7m" || ext == ".m7m" || ext == ".tsd")
            {
                // estraggo il file originale
                PdfName = Path.Combine(Path.GetDirectoryName(PdfName), Path.GetFileNameWithoutExtension(PdfName));
                ext = Path.GetExtension(PdfName).ToLower();
            }

            if (ext != ".pdf")
            {
                PdfName += ".pdf";
            }
            // Provo a generare una nuova preview
            return await filesystem.Exists(PdfName);
        }

        public async Task Generate(int imageId)
        {
            //if (! await HasPreview(imageId)) return;
            var u = await userService.GetUserProfile(SpecialUser.SystemUser);
            var data = await documentService.GetContentInfo(imageId);
            if (data == null) return;

            //if (data.PreviewStatus != Domain.Enumerators.JobStatus.Running)
            {

                var filesystem = await fileSystemProvider.InstanceOf(data.FileManager + "");
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
                // Provo a generare una nuova preview
                if (!await filesystem.Exists(PdfName) || data.PreviewStatus == Domain.Enumerators.JobStatus.Queued)
                {
                    // Provo a creare il pdf
                    await documentService.UpdatePreviewStatus(imageId, Domain.Enumerators.JobStatus.Running, u);
                    var fileConverter = await fileConverterFactory.Get(ext, ".pdf");
                    if (fileConverter != null)
                    {
                        if (await filesystem.Exists(FName))
                        {
                            using (var M = await filesystem.ReadAsStream(FName))
                            {
                                Stream M2 = (IsP7M) ? M.VerifyAndExtract() : M;
                                if (M2 != null)
                                {
                                    try
                                    {
                                        using (var pdfdata = await fileConverter.Convert(ext, M2))
                                        {
                                            if (pdfdata != null)
                                            {
                                                var newPdf = await CheckProtocol(imageId, u, pdfdata);
                                                if (newPdf == null)
                                                {
                                                    newPdf = pdfdata;
                                                    newPdf.Seek(0, SeekOrigin.Begin);
                                                }
                                                if (await filesystem.WriteFromStream(PdfName, newPdf))
                                                {
                                                    await documentService.UpdatePreviewStatus(imageId, Domain.Enumerators.JobStatus.Completed, u);
                                                }
                                            }
                                            else await documentService.UpdatePreviewStatus(imageId, Domain.Enumerators.JobStatus.NotNeeded, u);

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        logger.LogError(ex,"Preview-Generate<"+ext+">: "+ FName);
                                        await documentService.UpdatePreviewStatus(imageId, Domain.Enumerators.JobStatus.Failed, u);
                                    };
                                }
                                else await documentService.UpdatePreviewStatus(imageId, Domain.Enumerators.JobStatus.Failed, u);

                            }
                        }
                        else await documentService.UpdatePreviewStatus(imageId, Domain.Enumerators.JobStatus.Aborted, u);

                    }
                    else
                        await documentService.UpdatePreviewStatus(imageId, Domain.Enumerators.JobStatus.Aborted, u);
                }
                else
                {
                    using (var pdfdata = await filesystem.ReadAsStream(PdfName))
                    {
                        var newPdf = await CheckProtocol(imageId, u, pdfdata);
                        if (newPdf != null)
                        {
                            if (await filesystem.WriteFromStream(PdfName, newPdf))
                            {
                                await documentService.UpdatePreviewStatus(imageId, Domain.Enumerators.JobStatus.Completed, u);
                            }
                        }

                    }

                }
            }

        }

        private async Task<Stream> CheckProtocol(int imageId, UserProfile u, Stream pdfdata)
        {
            bool changed = false;
            var docs = await documentService.GetDocumentsFromContentId(imageId, u);
            foreach (var id in docs)
            {
                var doc = await documentService.Get(id);
                if (!String.IsNullOrEmpty(doc.ProtocolNumber) && (doc.DocumentType == null || doc.DocumentType.LabelPosition != Domain.Enumerators.LabelPosition.Manuale))
                    {
                    using (PdfManager.PDFFile pdf = new PdfManager.PDFFile(pdfdata))
                    {
                        int PageIndex = 1;
                        int X = 0;
                        int Y = 0;
                        if (doc.DocumentType != null)
                        {
                            switch (doc.DocumentType.LabelPosition)
                            {
                                case Domain.Enumerators.LabelPosition.InAltoADestra:
                                    X = 100;
                                    break;
                                case Domain.Enumerators.LabelPosition.InBassoASinistra:
                                    Y = 100;
                                    break;
                                case Domain.Enumerators.LabelPosition.InBassoADestra:
                                    X = 100;
                                    Y = 100;
                                    break;
                                case Domain.Enumerators.LabelPosition.Personalizzata:
                                    X = doc.DocumentType.LabelX;
                                    Y = doc.DocumentType.LabelY;
                                    break;
                                default:
                                    break;
                            }
                        }
                        SixLabors.ImageSharp.Image ProtocolStamp = await CreateProtocolStamp(doc);
                        if (ProtocolStamp != null)
                        {
                            try
                            {
                                using (var NewData = await pdf.AddImage(PageIndex, ProtocolStamp, X, Y, true, true))
                                {
                                    if (NewData != null)
                                    {
                                        NewData.Seek(0, SeekOrigin.Begin);
                                        var m = new MemoryStream();
                                        await NewData.CopyToAsync(m); // await NewData.CopyToAsync(pdfdata);
                                        return m;
                                        //changed = true;
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex, "Preview-CheckProtocol");

                            }

                        };
                    }
                    //if (NewData != null)
                    //{
                    //    NewData.Seek(0, SeekOrigin.Begin);
                    //    await NewData.CopyToAsync(pdfdata);
                    //    changed = true;
                    //}
                }
            }
            return changed ? (pdfdata) : null;
        }

        const string DefaultProtocolStamp = @"
<body style='margin:0;padding:0'>
<div style='margin:10px;border-radius:2px;border:1px solid red;background-color:#fff;padding:10px;width:960px;height:20px;'>
<h1 style='font-size:16px;margin:0;color:red'>[Copia] {ProtocolRegister} - PROT. {ProtocolNumber}-{ProtocolDate}</h1>
</div>
</body>
";
        private async Task<Image> CreateProtocolStamp(Document doc)
        {
            Image img = null;
            try
            {

            string Template = DefaultProtocolStamp;
                //TODO: caricare un template differente in base alla tipologia
                var HTMLStamp = Template
                    .Replace("{ProtocolRegister}", doc.ProtocolCustomProperties)
                    .Replace("{ProtocolNumber}", doc.ProtocolNumber)
                    .Replace("{ProtocolDate}", doc.ProtocolDate.Value.ToString("dd/MM/yyyy"))//, HH:mm:ss"))
                .Replace("{ExternalProtocolUid}", doc.ExternalProtocolUid);

            using (var M = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(HTMLStamp)))
            {
                using (var stamp = await HTMLConverter.ConvertToImage(M))
                {
                    img = Image.Load(stamp);
                }
            }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Preview-CreateProtocolStamp");

            }
            return img;
        }
    }
}
