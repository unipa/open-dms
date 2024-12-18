using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Domain.Models
{
    public class ProcessFilter
    {
        public bool Closed{ get; set; }
        public bool Expired { get; set; }
        public string BusinessProcessId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
