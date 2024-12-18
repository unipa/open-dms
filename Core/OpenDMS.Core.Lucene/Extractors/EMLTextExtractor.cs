using MimeKit;
using OpenDMS.Core.Interfaces;
using System.Text;

namespace OpenDMS.Core.LuceneIndexer.Extractors
{
    public class EMLTextExtractor : IContentTextExtractor
    {
        public bool FileTypeSupported(string Extension)
        {
            return Extension.ToLower().StartsWith(".eml");
        }

        public async Task<string> GetText(Stream stream)
        {
            StringBuilder sb = new StringBuilder();
            var msg = await MimeMessage.LoadAsync(stream);
            sb.Append(msg.Subject + " ");
            sb.Append(msg.TextBody);
            return sb.ToString().Trim();
        }
    }
}
