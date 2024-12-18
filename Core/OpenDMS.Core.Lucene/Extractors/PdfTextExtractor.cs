using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.Extensions.Configuration;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Repositories;
using OpenDMS.PdfManager;
using System.Text;
using static org.apache.poi.hssf.util.HSSFColor;

namespace OpenDMS.Core.LuceneIndexer.Extractors
{
    public class PDFTextExtractor : IContentTextExtractor
    {
        private int TesseractMaxPageIndex = 1;
        private string TesseractDirectory = "";
        private string TesseractLang = "";
        private readonly IAppSettingsRepository appSettings;

        public PDFTextExtractor(IConfiguration config, IAppSettingsRepository appSettings)
        {
            var TesseractDirectory = config["OCR:RootFolder"]?.ToString();
            if (string.IsNullOrEmpty(TesseractDirectory)) { TesseractDirectory = AppContext.BaseDirectory; };
            var TesseractLang = config["OCR:Language"]?.ToString();
            if (string.IsNullOrEmpty(TesseractLang)) { TesseractLang = "it-IT"; };
            this.appSettings = appSettings;
            if (int.TryParse(config["OCR:MaxPages"], out int nr))
                TesseractMaxPageIndex = nr;
            else
                TesseractMaxPageIndex = 1;
        }

        public bool FileTypeSupported(string Extension)
        {
            return Extension.ToLower().StartsWith(".pdf");
        }

        public async Task<string> GetText(Stream stream)
        {
            string testo = "";
            testo = await GetPdfContent(stream);
            if (string.IsNullOrEmpty(testo))
                testo = await GetContentPdfImg(stream);
            return testo;
        }


        private async Task<string> GetPdfContent(Stream stream)
        {
            StringBuilder text = new StringBuilder();

            using (PdfReader pdfReader = new PdfReader(stream))
            {
                using (PdfDocument doc = new PdfDocument(pdfReader))
                {
                    //TODO: Limitare il numero di pagine
                    var PageCount = doc.GetNumberOfPages(); 
                    if (PageCount > TesseractMaxPageIndex) PageCount = TesseractMaxPageIndex;

                    for (int page = 1; page <= PageCount; page++)
                    {
                        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                        string currentText = PdfTextExtractor.GetTextFromPage(doc.GetPage(page), strategy);
                        //currentText = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                        text.Append(currentText + " ");
                    }
                }
                pdfReader.Close();
            }
            return text.ToString().Trim();
        }

        private async Task<string> GetContentPdfImg(Stream stream)
        {
            StringBuilder res = new StringBuilder();
            try
            {
                var f = new PDFFile(stream);
                int PageCount = f.GetNumberOfPages();
                if (PageCount > TesseractMaxPageIndex) PageCount = TesseractMaxPageIndex;
                PageCount += 1;
                for (int i = 1; i < PageCount; i++)
                {
                    try
                    {
                        MemoryStream temp = (MemoryStream)await f.GetPageImageAsStream(i, 1024);
                        res.Append(GetContentImg(temp));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            { }
            return res.ToString().Trim();
        }

        private string GetContentImg(MemoryStream stream)
        {
            StringBuilder res = new StringBuilder();
            if (string.IsNullOrEmpty(TesseractDirectory)) return "";
            if (string.IsNullOrEmpty(TesseractLang)) TesseractLang = "it-IT";
            try
            {
                using (TesseractOCR.Engine engine = new TesseractOCR.Engine(TesseractDirectory, TesseractLang))
                {
                    TesseractOCR.Pix.Image pix = TesseractOCR.Pix.Image.LoadFromMemory(stream);
                    //  if (pix.Colormap.Depth >= 32)
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
