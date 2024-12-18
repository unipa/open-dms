using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Reports.Model
{
    public class UserTaskIndicator
    {
        public string ProcessId { get; set; }
        
        public string ProcessInstanceId { get; set; }

        public string TaskItemId { get; set; }

        public string TaskInstanceId { get; set; }

        public string UserId { get; set; }

        public string RoleId { get; set; }

        public string GroupId { get; set; }

        public string CompanyId { get; set; }

        public string EventId { get; set; }

        public int Status { get; set; }

        public decimal Percentage { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? ClaimDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime? ExecutionDate { get; set; }

        public decimal ClaimTime { get; set; }
        public decimal JobTime { get; set; }
        public decimal ExecutionTime { get; set; }
        public decimal DelayTime { get; set; }
        public decimal ExpirationTime { get; set; }

    }
}
