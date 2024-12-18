using Elmi.Core.PreviewGenerators;
using OpenDMS.PdfManager;

namespace Elmi.Core.FileConverters.Implementation
{
    public class OfficeToPDF : IFileConverter
    {
        public string[] FromFile => new[] { "DOCX", "XLSX", "PPTX" };
        public string ToFile => "PDF";
    
        
        public async Task<Stream> Convert(string Extension, Stream data)
        {
            return await OpenOfficeConverter.ConvertToPDF(data);
        }
    }
}
