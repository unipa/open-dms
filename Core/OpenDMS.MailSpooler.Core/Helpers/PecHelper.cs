using Microsoft.Extensions.Logging;
using MimeKit;
using OpenDMS.MailSpooler.Core.DTOs;
using OpenDMS.MailSpooler.Core.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OpenDMS.MailSpooler.Core.Helper
{
    public static class PecHelperExtension
    {
        public static MimeMessage GetPostaCert(this MimeMessage mailMessage)
        {
            var entity = mailMessage.BodyParts.FirstOrDefault(part =>
            {
                return part.ContentDisposition != null &&
                     !String.IsNullOrEmpty(part.ContentDisposition.FileName) &&
                     part.ContentDisposition.FileName.Equals("postacert.eml", StringComparison.InvariantCultureIgnoreCase) &&
                     part.ContentType.MimeType.Equals("message/rfc822", StringComparison.InvariantCultureIgnoreCase);
            });

            if (entity != null)
            {
                var dtcert = GetDatiCert(mailMessage);
                if (dtcert != null && dtcert.MailType != PostaCertType.AVVENUTA_CONSEGNA && dtcert.MailType != PostaCertType.ACCETTAZIONE)
                {
                    if (entity is MessagePart)
                    {
                        var _mpart = (MessagePart)entity;
                        if (!String.IsNullOrEmpty(_mpart.Message.Headers[HeaderId.MessageId]))
                            mailMessage = _mpart.Message;
                    }
                }
            }
            return mailMessage;
        }
        public static DatiCert GetDatiCert(this MimeMessage mailMessage)
        {
            DatiCert dtCert = null;
            try
            {
                var entity = mailMessage.Attachments.FirstOrDefault(x => x.ContentDisposition.FileName != null && x.ContentDisposition.FileName.Equals("daticert.xml", StringComparison.InvariantCultureIgnoreCase));
                if (entity != null)
                {
                        using (var M = new MemoryStream())
                        {
                            entity.WriteTo(M);
                            var xml = XDocument.Load(M);
                            dtCert = new DatiCert(xml);
                        }
                        //Controllo sia il msg della PEC che l'header di riferimento
                        var msgidOri = mailMessage.MessageId.Replace("<", "").Replace(">", "");
                        var msgidRif = "";
                        if (!String.IsNullOrEmpty(mailMessage.Headers["X-Riferimento-Message-ID"]))
                            msgidRif = mailMessage.Headers["X-Riferimento-Message-ID"];

                        if (!dtCert.Id.Equals(msgidOri, StringComparison.InvariantCultureIgnoreCase) &&
                            (!dtCert.MessageId.Equals(msgidRif, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            dtCert = null;
                            //logger.Warn($"Il daticert non corrisponde al documento. Svuoto il daticert. DatiCert-> Msgid: {dtCert.MessageId} - Identificativo: {dtCert.Id}");
                        }
                }
            }
            catch (Exception ex)
            {
                //logger.Error(ex.Message, ex);
            }
            return dtCert;
        }

    }
}
