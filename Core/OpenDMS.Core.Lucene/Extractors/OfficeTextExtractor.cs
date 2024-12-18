using OpenDMS.Core.Interfaces;
using System.Xml.Linq;
using System.Xml;
using TikaOnDotNet.TextExtraction;
using System.IO.Compression;
using System.Xml.XPath;
using System.Text;

namespace OpenDMS.Core.LuceneIndexer.Extractors
{
    public class OfficeTextExtractor : IContentTextExtractor
    {
        public bool FileTypeSupported(string Extension)
        {
            return Extension.ToLower().StartsWith(".doc")
                || Extension.ToLower().StartsWith(".docx")
                || Extension.ToLower().StartsWith(".xls")
                || Extension.ToLower().StartsWith(".xlsx")
                || Extension.ToLower().StartsWith(".ods")
                || Extension.ToLower().StartsWith(".odt")
                || Extension.ToLower().StartsWith(".odp")
                || Extension.ToLower().StartsWith(".odf")
                || Extension.ToLower().StartsWith(".odg")
                || Extension.ToLower().StartsWith(".ppt")
                || Extension.ToLower().StartsWith(".pptx");
        }

        private string Extract(Stream data)
        {
            XmlNamespaceManager NsMgr = new XmlNamespaceManager(new NameTable());
            NsMgr.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

            using (var archive = new ZipArchive(data))
            {
                var entry = archive.GetEntry(@"word/document.xml");
                if (entry == null)
                {
                    entry = archive.GetEntry(@"xl/workbook.xml");
                }
                if (entry == null)
                {
                    entry = archive.GetEntry(@"ppt/presentation.xml");
                }
                var stream = entry.Open();
                return XDocument
                    .Load(stream)
                    .XPathSelectElements("//w:p", NsMgr)
                    .Aggregate(new StringBuilder(), (sb, p) => p
                        .XPathSelectElements(".//w:t|.//w:tab|.//w:br", NsMgr)
                        .Select(e => { switch (e.Name.LocalName) { case "br": return "\v"; case "tab": return "\t"; } return e.Value; })
                        .Aggregate(sb, (sb1, v) => sb1.Append(v)))
                    .ToString();
            }
        }

        public async Task<string> GetText(Stream stream)
        {
            string res = "";
            try
            {
                res = Extract(stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return res.Trim();
        }

    }
}
