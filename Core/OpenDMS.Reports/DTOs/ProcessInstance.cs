using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Reports.DTOs
{
    public class ProcessInstance
    {
        // ID Univoco del processo
        public string ProcessId { get; set; }

        // ID Istanza del processo
        public string ProcessInstanceId { get; set; }

        // ID univoco del Nodo
        public string NodeId { get; set; }

        // ID Univoco della transazione sul nodo
        public string JobInstanceId { get; set; }

    }
}
