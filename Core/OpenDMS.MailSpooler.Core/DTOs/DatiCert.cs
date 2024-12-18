using OpenDMS.MailSpooler.Core.Enums;
using OpenDMS.MailSpooler.Core.Helpers;
using System.Xml.Linq;

namespace OpenDMS.MailSpooler.Core.DTOs
{
    public class DatiCert
        {

            // PostaCert
            public PostaCertType MailType { get;  set; }
            public PostaCertError Error { get; private set; }

            // PostaCert > Intestazione
            public string From { get; private set; }
            public IList<KeyValuePair<PostaCertDeliveryType, string>> To { get; private set; }
            public string Replies { get; private set; }
            public string Subject { get; private set; }

            // PostaCert > Dati
            public string Provider { get; private set; }

            // PostaCert > Dati > Data
            public string TimeZone { get; private set; }
            public string DayTime { get; private set; }
            public string HourTime { get; private set; }

            // PostaCert > Dati
            public string Id { get; private set; }
            public string MessageId { get; private set; }
            public PostaCertEnvelopeType ReceiveType { get; private set; }
            public string DeliveryType { get; private set; }
            public IList<string> Receives { get; private set; }
            public string ExternalError { get; private set; }


        public DatiCert(XDocument datiCertXML)
        {

            try
            {
                // Parsing PostaCert
                MailType = EnumHelper<PostaCertType>.GetDescriptionEnum(datiCertXML.Root.Attribute("tipo").Value);
                Error = EnumHelper<PostaCertError>.GetDescriptionEnum(datiCertXML.Root.Attribute("errore").Value);

                // Parsing PostaCert > Intestazione
                From = datiCertXML.Descendants("intestazione").Elements("mittente").First().Value;
                datiCertXML.Descendants("intestazione").Elements("destinatari").ToList().ForEach(delegate (XElement item)
                {
                    To.Add(
                        new KeyValuePair<PostaCertDeliveryType, string>(EnumHelper<PostaCertDeliveryType>.GetDescriptionEnum(item.Attribute("tipo").Value), item.Value)
                    );
                });

                Replies = datiCertXML.Descendants("intestazione").Elements("risposte").First().Value;
                Subject = datiCertXML.Descendants("intestazione").Elements("oggetto").DefaultIfEmpty(new XElement("oggetto")).First().Value;
                if (Subject.Length > 250)
                    Subject = Subject.Substring(0, 245) + "...";

                // Parsing PostaCert > Dati
                Provider = datiCertXML.Descendants("dati").Elements("gestore-emittente").First().Value;

                // Parsing PostaCert > Dati > Data
                TimeZone = datiCertXML.Descendants("dati").Elements("data").First().Attribute("zona").Value;
                DayTime = datiCertXML.Descendants("dati").Descendants("data").Elements("giorno").First().Value;
                HourTime = datiCertXML.Descendants("dati").Descendants("data").Elements("ora").First().Value;

                // Parsing PostaCert > Dati
                Id = datiCertXML.Descendants("dati").Elements("identificativo").First().Value;
                MessageId = datiCertXML.Descendants("dati").Elements("msgid").DefaultIfEmpty(new XElement("msgid")).First().Value;
                ReceiveType = EnumHelper<PostaCertEnvelopeType>.GetDescriptionEnum(datiCertXML.Descendants("dati").Elements("ricevuta").DefaultIfEmpty(new XElement("ricevuta", "")).First().Value);
                DeliveryType = datiCertXML.Descendants("dati").Elements("consegna").DefaultIfEmpty(new XElement("consegna", "")).First().Value;

                Receives = datiCertXML.Descendants("dati").Elements("consegna").ToList().ConvertAll<string>(delegate (XElement item)
                {
                    return item.Value;
                });

                ExternalError = datiCertXML.Descendants("dati").Elements("errore-esteso").DefaultIfEmpty(new XElement("errore-esteso", "")).First().Value;

            }
            catch (System.Exception ex)
            {
                MailType = PostaCertType.UNKNOW;
                Error = PostaCertError.ALTRO;
                DayTime = DateTime.Now.ToString("dd/MM/yyyy");
                Subject = "ERRORE IN LETTURA DAL FILE DATICERT. Controllare il contenuto della mail";
            }

        }
    }
}
