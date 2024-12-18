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
using System.Xml;
using System.Xml.Xsl;

namespace Elmi.Core.FileConverters.Implementation
{
    public class XMLToHTML : IFileConverter
    {
        public string[] FromFile => new[] { "XML" };
        public string ToFile => "HTML";

        public async Task<Stream> Convert(string Extension, Stream data)
        {
            Regex nonUTF8 = new Regex("[^\\x20 -\\x7e]");

            var dest = new MemoryStream();
            {

                //var nuovoXML = ConvertToUTF8(Fname);
                var doc = new XmlDocument();
                try
                {
                    doc.Load(data);
                    MemoryStream mStream = new MemoryStream();
                    XmlTextWriter writer = new XmlTextWriter(mStream, Encoding.UTF8);// Encoding.Unicode);
                    writer.Formatting = Formatting.Indented;
                    doc.WriteContentTo(writer);
                    writer.Flush();
                    mStream.Flush();
                    mStream.Position = 0;
                    // Read MemoryStream contents into a StreamReader.
                    StreamReader sReader = new StreamReader(mStream);
                    string[] lines = sReader.ReadToEnd().Split("\n");
                    // Extract the text from the StreamReader.
                    mStream.Close();
                    writer.Close();

                    int maxw = lines.Max(X => X.Length);
                    string wrappable = "nowrap";
                    if (maxw > 350)
                    {
                        maxw = 128;
                        wrappable = "";
                    }
                    int w = (int)Math.Round((double)1420 / maxw);
                    //w = 8;
                    String txt = "<!doctype html><html lang='it-IT'><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"><style>container > *{ white-space:" + wrappable + "; font-family: monospace; font-size: " + w.ToString() + "pt }; .pagebreak { page-break-before: always; }</style></head>" + string.Join("\n", lines).Replace(" ", "&nbsp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("\n", "<br/>") + "</body></html>";
                    dest.Write(System.Text.Encoding.UTF8.GetBytes(txt));

                }
                catch (Exception ex)
                {
                }









                //TextWriter target = new StreamWriter(dest);
                //target.Write("<!doctype html><html lang='it-IT'><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"><style>container > * { white-space:true; font-family: monospace; }; .pagebreak { page-break-before: always; }</style></head><body>");

                //using (StreamReader source = new StreamReader(data))
                //{
                //    while (!source.EndOfStream)
                //        target.WriteLine ("<div>"+(nonUTF8.Replace(source.ReadLine().Replace("<", "&lt").Replace(">", "&gt"), ""))+ "</div>");
                //    //var row = source.ReadLine() + "<br/>";
                //    //target.Write(row);
                //}
                //target.Write("</body></html>");
                //target.Flush();
                dest.Position = 0;
                return dest; // await HTMLConverter.ConvertToPDF(dest);
            }
        }



        public static string TransformXMLToHTML(XmlDocument inputXml, string xsltString)
        {
            StringWriter results = new StringWriter();
            using (MemoryStream input = new MemoryStream())
            {
                inputXml.Save(input);
                input.Seek(0, SeekOrigin.Begin);
                XslCompiledTransform transform = GetAndCacheTransform(xsltString);
                using (XmlReader reader = XmlReader.Create(input))
                {
                    transform.Transform(reader, null, results);
                }
            }
            return results.ToString();
        }

        private static Dictionary<String, XslCompiledTransform> cachedTransforms = new Dictionary<string, XslCompiledTransform>();
        private static XslCompiledTransform GetAndCacheTransform(String xslt)
        {
            XslCompiledTransform transform;
            if (!cachedTransforms.TryGetValue(xslt, out transform))
            {
                transform = new XslCompiledTransform();
                //                transform.OutputSettings.Indent = true;
                transform.Load(xslt);
                //using (XmlReader reader = XmlReader.Create(new StringReader(xslt)))
                //{
                //    transform.Load(reader);
                //}
                cachedTransforms.Add(xslt, transform);
            }
            return transform;
        }


    }
}
