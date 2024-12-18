using Elmi.Core.PreviewGenerators;
using OpenDMS.PdfManager;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlToOpenXml;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Reflection;

namespace Elmi.Core.FileConverters.Implementation
{
    public class HTMLToDocX : IFileConverter
    {
        public string[] FromFile => new[] {"FORMHTML", "HTML" };
        public string ToFile => "DOCX";
    

        public async Task<Stream> Convert(string Extension, Stream data)
        {
            var htmlValue = "";
            using (MemoryStream text = new MemoryStream())
            {
                await data.CopyToAsync(text);
                htmlValue = System.Text.Encoding.UTF8.GetString(text.ToArray());
            }

            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return cert!.Verify();
                };

            var image =  Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "wwwroot");
            if (image.EndsWith("/")) image = image.Substring(0, image.Length - 1);
            int i = htmlValue.IndexOf("\"/images/");
            while (i >= 0)
            {
                string img = "data:image/jpeg;base64, ";
                var j = htmlValue.IndexOf("\"", i + 1);
                var url = htmlValue.Substring(i + 1, j - i - 1).Replace("/","\\");
                img += System.Convert.ToBase64String(File.ReadAllBytes(image + url));
                htmlValue = htmlValue.Substring(0, i) + "\"" + img + "\"" + htmlValue.Substring(j + 1);
                i = htmlValue.IndexOf("\"/images/");
            }
            var css = File.ReadAllText(image + "\\css\\site.css");
            if (!htmlValue.StartsWith("<!DOCTYPE html"))
            {
                htmlValue = "<!DOCTYPE html><html><head><meta charset='UTF-16'/><style>\n" + css + "</style>\n</head><body>" + htmlValue + "</body></html>";
            }
            else
            {
                var i2 = htmlValue.IndexOf("<head>");
                htmlValue = htmlValue.Insert(i2 + 6, "<style>\n" + css + "</style>\n");
            }
            using (MemoryStream stream = new MemoryStream())
            {
                using WordprocessingDocument package = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document);
                if (package != null )
                {
                    MainDocumentPart? mainPart = package!.MainDocumentPart;
                    if (mainPart == null)
                    {
                        mainPart = package.AddMainDocumentPart();
                        new Document(new Body()).Save(mainPart);
                    }

                    HtmlConverter converter = new HtmlConverter(mainPart);
                    await converter.ParseBody (htmlValue);
                    mainPart.Document.Save();
                }
                stream.Seek (0, SeekOrigin.Begin);
                return stream;
            }
        }
    }
}
