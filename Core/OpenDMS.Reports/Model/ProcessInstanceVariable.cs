using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenDMS.Reports.DTOs;

namespace OpenDMS.Reports.Model
{
    public class ProcessInstanceVariable
    {
        [StringLength(64)]
        public string ProcessId { get; set; }

        // ID Istanza del processo
        [StringLength(12)]
        public string ProcessInstanceId { get; set; }

        // VariableId fa riferimento ad un metadato globale
        [StringLength(64)]
        public string? VariableId { get; set; }

        [StringLength(64)]
        public string VariableName { get; set; }

        [StringLength(255)]
        public string Value { get; set; }

        [StringLength(255)]
        public string LookupValue { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
