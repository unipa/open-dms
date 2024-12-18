using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.MailSpooler.Core.DTOs
{
    public class MailDetails
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int DocumentId { get; set; }
        public string ProtocolNumber { get; set; }
        public LookupTable From { get; set; } = new();
        public List<LookupTable> To { get; set; } = new();
        public List<LookupTable> CC { get; set; } = new();
        public List<LookupTable> CCr { get; set; } = new();
        public string Title { get; set; }
        public string Body { get; set; }
        public string Date { get; set; }
        public string ClaimUser { get; set; }
        public string ClaimDate { get; set; }

        public MailStatus Status { get; set; }
        public MailSubType Subtype { get; set; }
        public string MailType { get; set; }
        public string MailColor { get; set; }
        public string CheckType { get; set; }
        public string CheckColor { get; set; }
        public List<LookupTable> Attachments { get; set; } = new();

        public List<MailDetails> Previous { get; set; } = new();
        public List<MailDetails> Next { get; set; } = new();
    }

}
