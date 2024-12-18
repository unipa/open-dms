using OpenDMS.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Core.DTOs
{
    public class HistoryItem
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Sender { get; set; }
        public List<string> To { get; set; } = new List<string>();
        public List<string> CC { get; set; } = new List<string>();
        public List<string> CCr { get; set; } = new List<string>();

        public string EventDateTime { get; set; }
        public string Description { get; set; }
        public string EventType { get; set; }
        public string TaskId { get; set; }
        public string DeputyUser { get; set; }

        public string ProcessName { get; set; }
        public List<HistoryDetail> Details { get; set; } = new List<HistoryDetail>();

    }
}
