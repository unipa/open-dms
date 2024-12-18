using Elmi.Core.PreviewGenerators;
using MimeKit;
using OpenDMS.PdfManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elmi.Core.FileConverters
{
    public class CompositeFileConverter : IFileConverter
    {
        private readonly IFileConverter fromConverter;
        private readonly string intermediateExtension;
        private readonly IFileConverter toConverter;

        public string[] FromFile => null;
        public string ToFile => "";


        public CompositeFileConverter(IFileConverter FromConverter, string IntermediateExtension, IFileConverter ToConverter)
        {
            fromConverter = FromConverter;
            intermediateExtension = IntermediateExtension;
            toConverter = ToConverter;
        }
        public async Task<Stream> Convert(string Extension, Stream data)
        {
            var intermediate = await fromConverter.Convert(intermediateExtension, data);
            if (intermediate != null)
            {
                var odata = await toConverter.Convert(Extension, intermediate);
                return odata;
            }
            else
                return null;
        }
    }
}
