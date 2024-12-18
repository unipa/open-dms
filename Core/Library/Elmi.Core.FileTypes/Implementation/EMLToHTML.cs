using Elmi.Core.PreviewGenerators;
using iText.Layout.Element;
using iText.Layout;
using MimeKit;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Elmi.Core.FileConverters.Implementation
{
    public class EMLToHTML : IFileConverter
    {
        public string[] FromFile => new[] { "EML" };
        public string ToFile => "HTML";

        public async Task<Stream> Convert(string Extension, Stream data)
        {
            data.Seek(0, SeekOrigin.Begin);
            MimeMessage msg = MimeMessage.Load(data);
            StringBuilder sb = new StringBuilder();
            string dt = "";
            string Mittente = "";
            string Destinatari = "";
            string CC = "";
            string CCr = "";

            try
            {
                dt = msg.Date.ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch (Exception)
            {
            }
            try
            {
                if (msg.From != null && msg.From.Count > 0)
                {
                    MailboxAddress frm = (MailboxAddress)msg.From[0];
                    Mittente = string.IsNullOrEmpty(frm.Name) ? frm.Address : "\"" + frm.Name + "\" &lt" + frm.Address + "&gt";
                }
            }
            catch (Exception)
            {
            }

            if (msg.To != null)
            {
                foreach (var _to in msg.To)
                {
                    if (_to is GroupAddress)
                    {
                        foreach (MailboxAddress t in ((GroupAddress)_to).Members)
                        {
                            if (!string.IsNullOrEmpty(Destinatari))
                                Destinatari += ";";
                            Destinatari += string.IsNullOrEmpty(t.Name) ? t.Address : "\"" + t.Name + "\" &lt" + t.Address + "&gt";
                        }
                    }
                    else
                    {
                        try
                        {
                            MailboxAddress _t = (MailboxAddress)_to;
                            if (!string.IsNullOrEmpty(Destinatari))
                                Destinatari += ";";
                            Destinatari += string.IsNullOrEmpty(_t.Name) ? _t.Address : "\"" + _t.Name + "\" &lt" + _t.Address + "&gt";
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            if (msg.Cc != null)
            {
                foreach (var _to in msg.Cc)
                {
                    if (_to is GroupAddress)
                    {
                        foreach (MailboxAddress t in ((GroupAddress)_to).Members)
                        {
                            if (!string.IsNullOrEmpty(CC))
                                CC += ";";
                            CC += string.IsNullOrEmpty(t.Name) ? t.Address : "\"" + t.Name + "\" &lt" + t.Address + "&gt";
                        }
                    }
                    else
                    {
                        try
                        {
                            MailboxAddress _t = (MailboxAddress)_to;
                            if (!string.IsNullOrEmpty(CC))
                                CC += ";";
                            CC += string.IsNullOrEmpty(_t.Name) ? _t.Address : "\"" + _t.Name + "\" &lt" + _t.Address + "&gt";
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }

            if (msg.Bcc != null)
            {
                foreach (var _to in msg.Bcc)
                {
                    if (_to is GroupAddress)
                    {
                        foreach (MailboxAddress t in ((GroupAddress)_to).Members)
                        {
                            if (!string.IsNullOrEmpty(CCr))
                                CCr += ";";
                            CCr += string.IsNullOrEmpty(t.Name) ? t.Address : "\"" + t.Name + "\" &lt" + t.Address + "&gt";
                        }
                    }
                    else
                    {
                        try
                        {
                            MailboxAddress _t = (MailboxAddress)_to;
                            if (!string.IsNullOrEmpty(CCr))
                                CCr += ";";
                            CCr += string.IsNullOrEmpty(_t.Name) ? _t.Address : "\"" + _t.Name + "\" &lt" + _t.Address + "&gt";
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            //            if (FullMessage)
            sb.AppendLine("<!DOCTYPE html><html lang=\"en\"><head>");
            sb.AppendLine("<meta charset=\"UTF-8\">");
            sb.AppendLine("<title></title>");
            sb.AppendLine("</head><body>");
            {
                sb.Append("<div>");
                sb.AppendFormat("<div><strong>Oggetto:</strong> {0}</div>", msg.Subject);
                sb.AppendFormat("<div><strong>Data:</strong> {0}</div>", dt);
                sb.AppendFormat("<div style='margin-bottom:10px'><strong>Mittente:</strong> {0}</div>", Mittente);
                sb.AppendFormat("<div><strong>Destinatari:</strong> {0}</div>", Destinatari);
                if (!string.IsNullOrEmpty(CC))
                    sb.AppendFormat("<div><strong>CC:</strong> {0}</div>", CC);

                if (!string.IsNullOrEmpty(CCr))
                    sb.AppendFormat("<div><strong>CCr:</strong> {0}</div>", CCr);

                sb.Append("</div>");
                sb.AppendLine("<div style='height:1px;border-top:1px dashed #ddd'></div>");
            }
            sb.Append(getContent(msg));
            MimeMessage postacert = null;
            foreach (var attachment in msg.BodyParts)
            {
                if (attachment.ContentId == null && attachment.ContentDisposition != null && (attachment.ContentDisposition.FileName + "").ToLower() == "postacert.eml")
                {
                    try
                    {
                        postacert = MimeMessage.Load(((MimePart)attachment).Content.Stream);
                        break;
                    } catch { }
                }
            }

            if (postacert != null)
            {
                sb.AppendLine("<div style='height:4px;border-top:1px dashed #ddd;border-bottom:1px dashed #ddd'></div>");
                //sb.AppendLine(@"<h5>Postacert.eml</h5>");
                sb.AppendLine("<div style='height:4px;border-top:1px dashed #ddd;border-bottom:1px dashed #ddd'></div>");
                sb.Append(getContent(postacert));
            }
            sb.AppendLine("</body></html>");
            return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(sb.ToString()));
        }

        protected string _getContent(MimeMessage msg, string part)
        {
            if (!string.IsNullOrEmpty(part))
            {
                string regex = @"[\'\""]cid:[w\d\S]+[\'\""]"; // "cid:[\w\d]+\S+\d+|cid:_\\d_\\w+";
                RegexOptions options = RegexOptions.IgnorePatternWhitespace
                        | RegexOptions.Singleline
                        | RegexOptions.Multiline
                        | RegexOptions.IgnoreCase;

                //string cidValue = "";
                Regex reg = new Regex(regex, options);

                string ret = reg.Replace(part, delegate (Match match)
                {
                    string cid = match.Value.Replace("cid:", "");
                    string cidValue = cid.Substring(1, cid.Length - 2);
                    return cid.Substring(0, 1) + CidViewer(msg, cidValue) + cid.Substring(cid.Length - 1, 1);
                });
                return ret;

            }
            return "";
        }
        protected string GetAttachmentInfo(MimeMessage msg, MimePart part)
        {
            if (part.IsAttachment)
            {
                string Size = "-";
                if (part.ContentDisposition.Size != null)
                    Size = ((part.ContentDisposition.Size ?? 0)/1024).ToString("#.000")+"K"; 
                return "<tr><td>"+ part.ContentDisposition.FileName+ "</td><td style='min-width:150px;text-align:right'>" + Size+"</td></tr>";
            }
            return "";
        }
        protected string getContent(MimeMessage msg)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sa = new StringBuilder();
            try
            {
                var ret = _getContent(msg, msg.HtmlBody);
                if (!string.IsNullOrEmpty(ret))
                    sb.Append(ret);
                else
                {
                    ret = _getContent(msg, msg.TextBody);
                    if (!string.IsNullOrEmpty(ret))
                        sb.Append(ret);
                    else
                    {
                        foreach (MimeEntity part in msg.BodyParts)
                        {
                            if (part is TextPart)
                            {
                                var ret1 = _getContent(msg, ((TextPart)part).Text);
                                if (!String.IsNullOrEmpty(ret1))
                                    sb.Append(ret1);
                            }
                        }

                    }
                }
                foreach (MimeEntity part in msg.BodyParts)
                {
                    if (part is MimePart)
                    {
                        var ret2 = GetAttachmentInfo(msg, ((MimePart)part));
                        if (!String.IsNullOrEmpty(ret2))
                            sa.Append(ret2);
                    }
                }
                //}
                //else if (!string.IsNullOrEmpty(msg.TextBody))
                //{

                //    string regex = "[\r\n]+";
                //    RegexOptions options = RegexOptions.IgnorePatternWhitespace
                //            | RegexOptions.Singleline
                //            | RegexOptions.Multiline
                //            | RegexOptions.IgnoreCase;

                //    Regex reg = new Regex(regex, options);
                //    string ret = reg.Replace(msg.TextBody, m =>
                //    {
                //        return "<br/>";
                //    });

                //    sb.Append(ret);



                if (sa.Length > 0)
                {
                    sb.Append("<h5>Allegati</h5>");
                    sb.Append("<table>");
                    sb.Append("<tr><th>Nome File</th><th style='text-align:right'>Dimensione</th></tr>");
                    sb.Append(sa);
                    sb.Append("</table>");
                }
                if (sb.Length == 0)
                {
                    sb.Append("<div>Nessuna preview</div>");
                }
            }
            catch (Exception ex)
            {
                sb.Append("<div><h1>Impossibile visualizzare la preview</h1><p>"+ex.Message+"</p></div>");
            };
            return sb.ToString();
        }

        protected string CidViewer(MimeMessage msg, string cid)
        {
            try
            {
                MimePart entity = (MimePart)msg.BodyParts.FirstOrDefault(b => b.ContentId != null && b.ContentType.MediaType == "image" && b.ContentId.Equals(cid, StringComparison.InvariantCultureIgnoreCase));

                //                MIME_Entity entity = msg..GetEntityByCID(string.Format("<{0}>", cid));

                if (entity != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        entity.Content.DecodeTo(ms);
                        ms.Position = 0;
                        return "data:" + entity.ContentType.MimeType + ";base64," + System.Convert.ToBase64String(ms.ToArray(), Base64FormattingOptions.None);
                    }
                }
            }
            catch { };
            return "";
        }

    }
}
