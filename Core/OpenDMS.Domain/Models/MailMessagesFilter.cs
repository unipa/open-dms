using OpenDMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Domain.Models
{
    public class MailMessagesFilter
    {
        public string internalAddress { get; set; }
        public MailStatus? mailStatus { get; set; }
        public MailDirection? mailDirection { get; set; }
        public string descriptionText { get; set; } = "";
        public DateTime? dateFrom { get; set; }
        public DateTime? dateTo { get; set; }
        public string externalAddress { get; set; } = "";
        public int pageIndex { get; set; } = 0;
        public int pageSize { get; set; } = 5;
        public int[] mailboxId { get; set; } 
    }
}
