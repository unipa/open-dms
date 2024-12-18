using iText.Forms.Form.Element;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace OpenDMS.PdfManager
{


    /// <summary>
    /// RUN apt update -y && apt install -y WKHTMLTOPDF
    /// 
    /// </summary>
    public static class HTMLConverter
    {
        private const string LINUX_WKHTMLTOPDF_EXECUTABLE = "/usr/local/bin/wkhtmltopdf";
        private const string WINDOWS_WKHTMLTOPDF_EXECUTABLE = "Apps\\wkhtmltopdf\\bin\\wkhtmltopdf.exe";
        private const string LINUX_WKHTMLTOIMAGE_EXECUTABLE = "/usr/local/bin/wkhtmltoimage";
        private const string WINDOWS_WKHTMLTOIMAGE_EXECUTABLE = "Apps\\wkhtmltopdf\\bin\\wkhtmltoimage.exe";
        private const int WKHTMLTOPDF_TIMEOUT = 60000;
        private const int WKHTMLTOPDF_DEFAULT_WIDTH = 64;

        public static async Task<Stream> ConvertToPDF(Stream inputStream, int width = WKHTMLTOPDF_DEFAULT_WIDTH)
        {
            List<string> switches = new List<string> {
                "-q",
                "-n",
                //"--zoom 1",
                "--print-media-type",
                "--load-media-error-handling skip",
                "--load-error-handling skip skip",
                //"--footer-left ''",
                //"--footer-right 'Pagina [page] / [topage]'"
                //"--zoom 1.2", //1.22
                //"--viewport-size 1000",
                };
            //switches.AddRange(switches);

            Stream output = new MemoryStream();
            Process P = new Process();
            P.StartInfo.FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? LINUX_WKHTMLTOPDF_EXECUTABLE : Path.Combine(AppContext.BaseDirectory, WINDOWS_WKHTMLTOPDF_EXECUTABLE);
            P.StartInfo.UseShellExecute = false;
            P.StartInfo.RedirectStandardOutput = true;
            P.StartInfo.RedirectStandardError = true;
            P.StartInfo.RedirectStandardInput = true;



            string inputfile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".html");
            string outputfile = Path.ChangeExtension(inputfile, ".pdf");
            using (var inputFile = File.Create(inputfile))
            {
                await inputStream.CopyToAsync(inputFile);
            }
            // fix temporaneo alla gestione del charset
            var data = File.ReadAllText(inputfile);
            if (!data.StartsWith("<!DOCTYPE"))
            {
                data = "<!DOCTYPE html>\n<html><head>\n<meta charset='UTF-16' /></head>" + data + "</html>";
                File.WriteAllText(inputfile, data);
            }
            //            File.WriteAllBytes(inputfile, ((MemoryStream)inputStream).ToArray());
            switches.Add("\"" + inputfile + "\"");
            switches.Add("\"" + outputfile + "\"");
            P.StartInfo.WorkingDirectory = Path.GetDirectoryName(inputfile);

            P.StartInfo.Arguments = string.Join(" ", switches.ToArray());

            var started = P.Start();

            if (started)
            {
                P.WaitForExit(WKHTMLTOPDF_TIMEOUT);
                if (P.HasExited)
                {
                    output = new MemoryStream(File.ReadAllBytes(outputfile));
                    try
                    {
                        var xml = Pdf2Html.ToXml(outputfile);
                        var tags = Find("[FIRMA:", "]", xml, 100);
                        if (tags.Count > 0)
                        {
                            Hashtable tagList = new Hashtable();
                            for (int index = 0; index < tags.Count(); index++)
                            {
                                var tag = tags[index];
                                // converto i campi con nome duplicato in nome_numero
                                var name = tag.Name;
                                int i = 0;
                                while (tagList.ContainsKey(name))
                                {
                                    i++;
                                    name += tag.Name + "_" + i.ToString();
                                }
                                tagList.Add(name, tag);
                                tag.Name = name;
                                tag.Right = tag.Left + 25;
                                tag.Bottom = tag.Top + 7;
                                tags[index] = tag;
                            }
                            using (var Pdf = new PDFFile(output))
                            {
                                output = (MemoryStream)Pdf.AddBlankSignatures(tags);
                            }
                        }
                    }
                    catch { };


                    File.Delete(inputfile);
                    File.Delete(outputfile);

                    //                    var errorMessage = P.StandardError.ReadToEnd();
                    //                    if (!string.IsNullOrEmpty(errorMessage))
                    //                        throw new TimeoutException(errorMessage);
                    if (output.Length == 0)
                        throw new TimeoutException("errore in generazione preview");
                }
                else
                {
                    var errorMessage = P.StandardError.ReadToEnd();
                    if (!string.IsNullOrEmpty(errorMessage))
                        throw new TimeoutException(errorMessage);
                }
            }
            if (!started)
            {
                var errorMessage = P.StandardError.ReadToEnd();
                if (!string.IsNullOrEmpty(errorMessage))
                    throw new FileNotFoundException(errorMessage);
            }



            inputStream.Position = 0;

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(inputStream);
            var signatures = doc.DocumentNode.Descendants("div")?
                .Where(d => d.GetAttributeValue("class", "")
                    .Contains("DigitalSignature")).ToList();
            if (signatures != null && signatures.Count > 0)
            {

                foreach (var sign in signatures)
                {
                    // recupero la posizione dallo stile (es style="left:<num>px|%;top:<num>px|%")
                    // recupero il nome del campo di firma da name o id
                    // recupero il nr. pagina dall'attributo "page" (1+) 
                    var name = sign.GetAttributeValue("name", "");
                    if (string.IsNullOrEmpty(name))
                    {
                        name = sign.GetAttributeValue("id", "");
                    }
                    if (!String.IsNullOrEmpty(name))
                    {
                        var styles = sign.GetAttributeValue("style", "").ToLower().Split(";");
                        var left = styles.FirstOrDefault(n => n.StartsWith("left:")).Split(":")[1];
                        var top = styles.FirstOrDefault(n => n.StartsWith("top:")).Split(":")[1];
                        var pixel = left.EndsWith("px");
                        float x = 0;
                        float y = 0;
                        if (pixel)
                        {
                            x = (float)8.5 + ((float.Parse(left.Replace("px", ""))) * 82 / 810);
                            y = (float)3 + ((float.Parse(top.Replace("px", ""))) * 92 / 1300);
                        }
                        else
                        {
                            x = (float)8.5 + float.Parse(left.Replace("%", ""));
                            y = (float)3 + float.Parse(top.Replace("%", ""));
                        }
                        var pagestring = sign.GetAttributeValue("page", "1");
                        if (!int.TryParse(pagestring, out var page)) page = 1;

                        using (PDFFile pdf = new PDFFile(output))
                        {
                            var output2 = pdf.AddSignature(name, page, x, y, 24, 6);
                            if (output2 != null)
                            {
                                output = output2;
                                output.Position = 0;
                            }
                        }
                    }
                }

            }

            return output;
        }



        public static List<FieldPosition> Find(String StartText, String EndText, string xmlData, Int32 PageWidth = 1024, Int32 PageIndex = 0)
        {
            List<FieldPosition> sr = new List<FieldPosition>();
            if (string.IsNullOrEmpty(xmlData)) return sr;
            if (String.IsNullOrEmpty(StartText)) return sr;

            var xml = new XmlDocument();
            try
            {
                xml.LoadXml(xmlData);

                XmlNode root = xml.DocumentElement;
                if (root == null) return sr;
                XmlNode pl = root.SelectSingleNode("/pdf2xml/page" + (PageIndex > 0 ? "[@number=" + PageIndex.ToString() + "]" : ""));
                Int32 pw = Int32.Parse(pl.Attributes["width"].Value.ToString());
                Decimal ratio = (Decimal)(100) / pw;
                Int32 ph = Int32.Parse(pl.Attributes["height"].Value.ToString());
                Decimal ratioh = (Decimal)(100) / ph;
                XmlNodeList nl = root.SelectNodes("/pdf2xml/page" + (PageIndex > 0 ? "[@number=" + PageIndex.ToString() + "]" : "") + "/text");
                foreach (XmlNode x in nl)
                {
                    String row = x.InnerText.Replace("<b>", "").Replace("</b>", "");
                    //if (row.Length > 0) row = row.Substring(0, row.Length);
                    Int32 i = row.Length - StartText.Length;
                    Int32 StartI = 0;
                    while ((i >= 0) && (row.Length > 0))
                    {
                        i = row.IndexOf(StartText, StartI);
                        if ((i >= 0) && (row.Length > 0))
                        {
                            String f = x.Attributes["font"].InnerText;
                            var j = row.IndexOf(EndText, i);
                            if (j > 0)
                            {
                                var Text = row.Substring(i + StartText.Length, j - i - StartText.Length);
                                XmlNode fn = root.SelectSingleNode("/pdf2xml/page/fontspec[@id=" + f + "]");
                                Decimal l, t, b, r;
                                l = System.Convert.ToDecimal(x.Attributes["left"].InnerText) * ratio;
                                t = System.Convert.ToDecimal(x.Attributes["top"].InnerText) * ratioh;
                                r = System.Convert.ToDecimal(x.Attributes["width"].InnerText) * ratio;
                                b = System.Convert.ToDecimal(x.Attributes["height"].InnerText) * ratioh;
                                String FontName = fn.Attributes["family"].InnerText;
                                float FontSize = float.Parse(fn.Attributes["size"].InnerText);

                                //Bitmap bmp = new Bitmap(pw, (Int32)FontSize * 2);
                                //Graphics G = Graphics.FromImage(bmp);

                                Image<Rgba32> image = new(pw, (Int32)FontSize * 2);
                                FontFamily fontFamily;
                                if (!SystemFonts.TryGet(FontName, out fontFamily))
                                    if (SystemFonts.Collection.Families.FirstOrDefault() != null)
                                    {
                                        fontFamily = SystemFonts.Collection.Families.FirstOrDefault();
                                    }
                                    else
                                        return sr;

                                var font = fontFamily.CreateFont((float)(image.Height / 1.75), FontStyle.Regular);


                                var options = new TextOptions(font)
                                {
                                    //Dpi = 150,
                                    KerningMode = KerningMode.Standard,
                                    VerticalAlignment = VerticalAlignment.Center
                                };
                                var s1 = TextMeasurer.MeasureAdvance(row.Substring(0, i), options);
                                var s2 = TextMeasurer.MeasureAdvance(row.Substring(0, j + EndText.Length), options);
                                var s3 = TextMeasurer.MeasureAdvance(row, options);

                                //SizeF s1 = G.MeasureString(row.Substring(0, i),  new System.Drawing.Font(FontName, FontSize));
                                //SizeF s2 = G.MeasureString(row.Substring(0, i + Text.Length), new System.Drawing.Font(FontName, FontSize));
                                //SizeF s3 = G.MeasureString(row, new System.Drawing.Font(FontName, FontSize));

                                l = l + (r * (Decimal)(s1.Width / s3.Width));
                                r = (r * (Decimal)((s2.Width - s1.Width) / s3.Width));

                                FieldPosition rect = new FieldPosition();
                                rect.Left = (l);
                                rect.Top = (t);
                                rect.Right = (r);
                                rect.Bottom = (b);
                                rect.Name = Text;
                                rect.Description = Text;
                                Int32 pn = Int32.Parse(x.ParentNode.Attributes["number"].Value.ToString());
                                rect.Page = pn;
                                sr.Add(rect);
                                StartI = i + StartText.Length;
                            }
                        }
                    };
                }
            }
            catch { };
            return sr;
        }


        public static async Task<Stream> ConvertToImage(Stream inputStream, int width = WKHTMLTOPDF_DEFAULT_WIDTH)
        {
            List<string> switches = new List<string> {
                "-q",
                "--format PNG",
                "--width 0",
                "--load-error-handling ignore",
                "--transparent"
                };


            var output = new MemoryStream();
            Process P = new Process();
            P.StartInfo.FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? LINUX_WKHTMLTOIMAGE_EXECUTABLE : Path.Combine(AppContext.BaseDirectory, WINDOWS_WKHTMLTOIMAGE_EXECUTABLE);
            P.StartInfo.UseShellExecute = false;
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                P.StartInfo.LoadUserProfile = true;
            string inputfile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".html");
            string outputfile = Path.ChangeExtension(inputfile, ".png");
            using (var inputFile = File.Create(inputfile))
            {
                await inputStream.CopyToAsync(inputFile);
            }
            switches.Add("\"" + inputfile + "\"");
            switches.Add("\"" + outputfile + "\"");
            P.StartInfo.WorkingDirectory = Path.GetDirectoryName(inputfile);

            P.StartInfo.Arguments = string.Join(" ", switches.ToArray());
            var started = P.Start();
            try
            {
                if (started)
                {
                    P.WaitForExit(WKHTMLTOPDF_TIMEOUT);
                }
                else
                if (!started)
                {
                    var errorMessage = P.StandardError.ReadToEnd();
                    if (!string.IsNullOrEmpty(errorMessage))
                        throw new FileNotFoundException(errorMessage);
                }
                {
                    output = new MemoryStream(File.ReadAllBytes(outputfile));
                    File.Delete(inputfile);
                    File.Delete(outputfile);
                }
            }
            catch (Exception ex)
            {
                throw;
            };
            return output;
        }



        private static void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}