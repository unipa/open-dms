using Elmi.Core.PreviewGenerators;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;

namespace Elmi.Core.FileConverters.Implementation
{
    public class ZIPToHTML : IFileConverter
    {
        public string[] FromFile => new[] { "ZIP", "RAR", "7Z", "TAR" };
        public string ToFile => "HTML";

        public async Task<Stream> Convert(string Extension, Stream data)
        {
            var ext = Extension;
            StringBuilder sb = new StringBuilder();
            sb.Append("<!doctype html><html lang='it-IT'><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"><style>container > *{ white-space:true; font-family: monospace; }; .pagebreak { page-break-before: always; }</style></head><body>");
            try
            {
                sb.Append("<style>");
                sb.Append(@" table, th, td { border: 1px solid lightgray; border-collapse: collapse; padding: 2px }");
                sb.Append("</style>");
                sb.AppendFormat("<h1>Tipo File:{0}</h1>", ext);
                sb.AppendFormat("<h2>Dimensione File: {0} KB</h2>", data.Length.ToString("#,##"));
                sb.AppendLine("<hr/>");
                sb.Append("<div>");
                try
                {
                        sb.Append("<h2>Contenuto archivio</h2>");
                        sb.Append("<table style='width:100%;margin: 0 auto; '>");
                        sb.Append("<thead>");
                        sb.Append("<tr style='background-color: gray; color: white;' > ");
                        sb.Append("     <th style='text-align:left'>Nome File</th>");
                        sb.Append("     <th style='text-align:center'>Dimensione File (KB)</th>");
                        sb.Append("     <th style='text-align:center'>Dimensione File Compr. (KB)</th>");
                        sb.Append("</tr>");
                        sb.Append("</thead>");
                        sb.Append("<tbody>");

                    using (ZipFile zip = new ZipFile (data))
                    {
                        for (int i = 0; i < zip.Count; i++)
                        {
                            sb.AppendFormat("<tr>");
                            sb.AppendFormat("   <td style='text-align:left'>{0}</td>", zip[i].Name);
                            sb.AppendFormat("   <td style='text-align:right'>{0}</td>", zip[i].Size);
                            sb.AppendFormat("   <td style='text-align:right'>{0}</td>", zip[i].CompressedSize);
                            sb.AppendFormat("</tr>");
                        }
                    }
                        sb.Append("</tbody>");
                        sb.Append("</table>");
                }
                catch (Exception)
                {
                    sb.Append("<h2>Anteprima del file non disponibile</h2>");
                }

                sb.Append("</div>");
                sb.Append("</body></html>");
            }
            catch (Exception ex)
            {
            }
            return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(sb.ToString()));
        }


    }
}
