using Elmi.Core.PreviewGenerators;
using MimeKit;
using MsgReader;
using OpenDMS.PdfManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Elmi.Core.FileConverters.Implementation
{
    public class TextToHTML : IFileConverter
    {
        public string[] FromFile => new[] { "TXT", "LOG", "CSV", "DAT" };
        public string ToFile => "HTML";

        public async Task<Stream> Convert(string Extension, Stream data)
        {
            Regex nonUTF8 = new Regex("[^\\x20 -\\x7e]");
            var dest = new MemoryStream();
            {
                TextWriter target = new StreamWriter(dest);
                target.Write("<!doctype html><html lang='it-IT'><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"><style>container > * { white-space:true; font-family: monospace; }; .pagebreak { page-break-before: always; }</style></head><body>");

                using (StreamReader source = new StreamReader(data))
                {
                    while (!source.EndOfStream)
                    {
                        var inputRow = source.ReadLine();
                        var outputrow = "<div>" + nonUTF8.Replace(inputRow, "").Replace("<", "&lt;").Replace(">", "&gt;") + "</div>";
                        target.WriteLine(outputrow);
                    }
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
