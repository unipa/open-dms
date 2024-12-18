using Elmi.Core.PreviewGenerators;
using MimeKit;
using OpenDMS.PdfManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elmi.Core.FileConverters.Implementation
{
    public class ImageToPDF : IFileConverter
    {
        public string[] FromFile => new[] { "BMP", "JPG", "JPEG", "PNG", "TIFF" };
        public string ToFile => "PDF";
    

        public async Task<Stream> Convert(string Extension, Stream data)
        {
            byte[] dataArray = null;
            using (var M = new MemoryStream())
            {
                await data.CopyToAsync(M);
                M.Position = 0;
                dataArray = M.ToArray();
            }
            if (dataArray != null)
            {
                PDFFile pdf = new PDFFile();
                var newpdf = await pdf.AddRasterPage(1, dataArray);
                return newpdf;
            };
            return new MemoryStream();
        }
    }
}
