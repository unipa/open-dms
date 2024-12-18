using MimeKit;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.MailSpooler.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.MailSpooler.Core.Archiver
{
    public class MailParser : IMailParser
    {
        private MimeMessage msg;

        public MailParser()
        {
        }


        public MailParser Read(Stream message)
        {
            msg = MimeMessage.Load(message);
            return this;
        }

        public LookupTable GetSender()
        {
            LookupTable T = new LookupTable();
            MailboxAddress a = (MailboxAddress)msg.From[0];
            T.Id = a.Address;
            T.Description = a.Name;
            T.Annotation = a.Domain;
            return T;

        }

        public string GetMessageId()
        {
            return msg.MessageId;
        }

        public int[] GetDocuments()
        {
            int[] lista = new int[0];
            var list = msg.Headers["_dms_documents_"];
            if (!String.IsNullOrEmpty(list))
            {
                try
                {
                    lista = Newtonsoft.Json.JsonConvert.DeserializeObject<int[]>(list);
                }
                catch (Exception)
                {
                }
            }
            return lista;
        }




        public List<LookupTable> GetToList()
        {
            List<LookupTable> TList = new();
            foreach (var to in msg.To)
            {
                LookupTable T = new LookupTable();
                MailboxAddress a = (MailboxAddress)to;
                T.Id = a.Address;
                T.Description = a.Name;
                T.Annotation = a.Domain;
                TList.Add(T);
            }
            return TList;
        }
        public List<LookupTable> GetCCList()
        {
            List<LookupTable> TList = new();
            foreach (var to in msg.Cc)
            {
                LookupTable T = new LookupTable();
                MailboxAddress a = (MailboxAddress)to;
                T.Id = a.Address;
                T.Description = a.Name;
                T.Annotation = a.Domain;
                TList.Add(T);
            }
            return TList;
        }
        public List<LookupTable> GetCCrList()
        {
            List<LookupTable> TList = new();
            foreach (var to in msg.Bcc)
            {
                LookupTable T = new LookupTable();
                MailboxAddress a = (MailboxAddress)to;
                T.Id = a.Address;
                T.Description = a.Name;
                T.Annotation = a.Domain;
                TList.Add(T);
            }
            return TList;
        }


    }
}
