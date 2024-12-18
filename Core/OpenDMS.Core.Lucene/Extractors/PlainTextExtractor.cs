using OpenDMS.Core.Interfaces;
using System.Text;

namespace OpenDMS.Core.LuceneIndexer.Extractors
{
    public class PlainTextExtractor : IContentTextExtractor
    {
        public bool FileTypeSupported(string Extension)
        {
            return Extension.ToLower().StartsWith(".txt")
                || Extension.ToLower().StartsWith(".log")
                || Extension.ToLower().StartsWith(".html")
                || Extension.ToLower().StartsWith(".htm")
                || Extension.ToLower().StartsWith(".xml")
                || Extension.ToLower().StartsWith(".json")
                || Extension.ToLower().StartsWith(".bpmn")
                || Extension.ToLower().StartsWith(".form")
                || Extension.ToLower().StartsWith(".dmn")
                || Extension.ToLower().StartsWith(".md");
        }
        public async Task<string> GetText(Stream stream)
        {
                return await (new StreamReader(stream, Encoding.UTF8)).ReadToEndAsync();
        }

    }
}
