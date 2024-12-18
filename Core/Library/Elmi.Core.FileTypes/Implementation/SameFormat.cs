using Elmi.Core.PreviewGenerators;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;

namespace Elmi.Core.FileConverters.Implementation
{
    public class SameFormat : IFileConverter
    {
        public string[] FromFile => new[] { "*" };
        public string ToFile => "*";

        public async Task<Stream> Convert(string Extension, Stream data)
        {
            return data;
        }


    }
}
