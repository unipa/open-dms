using Elmi.Core.PreviewGenerators;
using MimeKit;
using OpenDMS.PdfManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elmi.Core.FileConverters.Implementation
{
    public class OpenOfficeToPDF : IFileConverter
    {
        public string[] FromFile => new[] { "ODT", "ODS", "ODP" };
        public string ToFile => "PDF";
    
        public async Task<Stream> Convert(string Extension, Stream data)
        {
            return await OpenOfficeConverter.ConvertToPDF(data);
        }
    }
}
