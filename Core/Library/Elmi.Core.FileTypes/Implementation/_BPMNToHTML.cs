using Elmi.Core.PreviewGenerators;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using iText.Layout.Element;
using System.Text.RegularExpressions;

namespace Elmi.Core.FileConverters.Implementation
{
    public class _BPMNToHTML : IFileConverter
    {
        public string[] FromFile => new[] { "BPMN" };
        public string ToFile => "HTML";

        public async Task<Stream> Convert(string Extension, Stream data)
        {
            Regex nonUTF8 = new Regex("[^\\x20 -\\x7e]");

            var dest = new MemoryStream();
            {
                TextWriter target = new StreamWriter(dest);
                target.Write("<!doctype html><html lang='it-IT'><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"></head><body>");

                using (StreamReader source = new StreamReader(data))
                {
                    while (!source.EndOfStream)
                        target.WriteLine("<div>" + (nonUTF8.Replace(source.ReadLine().Replace("<", "&lt").Replace(">", "&gt"), "")) + "</div>");
                    //var row = source.ReadLine() + "<br/>";
                    //target.Write(row);
                }
                target.Write("</body></html>");
                target.Flush();
                dest.Position = 0;
                //                target.Close();
                return dest; // await HTMLConverter.ConvertToPDF(dest);
            }
        }


    }
}
