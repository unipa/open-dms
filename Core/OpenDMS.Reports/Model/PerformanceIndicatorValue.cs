using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenDMS.Reports.DTOs;

namespace OpenDMS.Reports.Model
{
    [Table("ProcessMeasures")]
    public class PerformanceIndicatorValue 
    {
        /// <summary>
        /// Id del processo associato
        /// </summary>
        [StringLength(64)]
        public string ProcessId { get; set; }

        // ID Istanza del processo
        [StringLength(12)]
        public string ProcessInstanceId { get; set; }

        [StringLength(64)]
        public string IndicatorId { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public decimal Value { get; set; }
    }
}
