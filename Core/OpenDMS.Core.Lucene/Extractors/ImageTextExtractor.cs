using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using Microsoft.Extensions.Logging;
using MimeKit;
using OpenDMS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TikaOnDotNet.TextExtraction;
using Microsoft.Extensions.Configuration;
using OpenDMS.Domain.Repositories;

namespace OpenDMS.Core.LuceneIndexer.Extractors
{
    public class ImageTextExtractor : IContentTextExtractor
    {
        private string TesseractDirectory = "";
        private string TesseractLang = "";


        public ImageTextExtractor(IConfiguration config)
            {
                var TesseractDirectory = config["OCR:RootFolder"]?.ToString();
                if (string.IsNullOrEmpty(TesseractDirectory)) { TesseractDirectory = AppContext.BaseDirectory; };
                var TesseractLang = config["OCR:Language"]?.ToString();
                if (string.IsNullOrEmpty(TesseractLang)) { TesseractLang = "it-IT"; };
        }

        public bool FileTypeSupported(string Extension)
        {
            return Extension.ToLower().StartsWith(".jpg")
                || Extension.ToLower().StartsWith(".bmp")
                || Extension.ToLower().StartsWith(".tif")
                || Extension.ToLower().StartsWith(".tiff")
                || Extension.ToLower().StartsWith(".gif")
                || Extension.ToLower().StartsWith(".png");
        }

        public async Task<string> GetText(Stream stream)
        {
            if (string.IsNullOrEmpty(TesseractDirectory) || string.IsNullOrEmpty(TesseractLang)) return "";
            StringBuilder res = new StringBuilder();
            try
            {
                using (TesseractOCR.Engine engine = new TesseractOCR.Engine(TesseractDirectory, TesseractLang))
                {
                    TesseractOCR.Pix.Image pix = TesseractOCR.Pix.Image.LoadFromMemory((MemoryStream)stream);
                    try
                    {
                        pix = pix.ConvertRGBToGray();
                    }
                    catch { };
                    using (TesseractOCR.Page page = engine.Process(pix))
                    {
                        res.Append(page.Text.Trim());
                    }
                    pix.Dispose();
                }
            }
            catch (Exception ex1)
            {

            }
            return res.ToString().Trim();
        }

    }
}
