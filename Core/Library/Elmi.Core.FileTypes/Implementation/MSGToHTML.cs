using Elmi.Core.PreviewGenerators;
using System.Text;

namespace Elmi.Core.FileConverters.Implementation
{
    public class MSGToHTML : IFileConverter
    {
        public string[] FromFile => new[] { "MSG" };
        public string ToFile => "HTML";

        public async Task<Stream> Convert(string Extension, Stream data)
        {
            MsgReader.Outlook.Storage.Message msg = new MsgReader.Outlook.Storage.Message(data);
            StringBuilder sb = new StringBuilder();
            string dt = "";
            string Mittente = "";
            string Destinatari = "";
            string CC = "";
            string CCr = "";

            try
            {
                if (msg.ReceivedOn.HasValue)
                    dt = msg.ReceivedOn.Value.ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch (Exception)
            {
            }
            try
            {
                if (msg.Sender != null)
                {
                    var frm = msg.Sender;
                    Mittente = String.IsNullOrEmpty(frm.DisplayName) ? frm.Email : "\"" + frm.DisplayName + "\" &lt" + frm.Email + "&gt";
                }
            }
            catch (Exception)
            {
            }
            try
            {
                if (msg.Recipients != null)
                    foreach (var _to in msg.Recipients)
                    {
                        if (_to.Type == MsgReader.Outlook.RecipientType.To)
                        {
                            if (!String.IsNullOrEmpty(Destinatari))
                                Destinatari += "; ";
                            Destinatari += String.IsNullOrEmpty(_to.DisplayName) ? _to.Email : "\"" + _to.DisplayName + "\" &lt" + _to.Email + "&gt";
                        }
                        else
                        if (_to.Type == MsgReader.Outlook.RecipientType.Cc)
                        {
                            if (!String.IsNullOrEmpty(CC))
                                CC += "; ";
                            CC += String.IsNullOrEmpty(_to.DisplayName) ? _to.Email : "\"" + _to.DisplayName + "\" &lt" + _to.Email + "&gt";
                        }
                        else
                        if (_to.Type == MsgReader.Outlook.RecipientType.Bcc)
                        {
                            if (!String.IsNullOrEmpty(CCr))
                                CCr += "; ";
                            CCr += String.IsNullOrEmpty(_to.DisplayName) ? _to.Email : "\"" + _to.DisplayName + "\" &lt" + _to.Email + "&gt";
                        }


                    }
            }
            catch (Exception)
            {
            }
//            if (FullMessage)
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
                sb.AppendLine("<hr style='height:1px'/>");
            }
            sb.Append(msg.BodyHtml);

            return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(sb.ToString()));
        }



    }
}
