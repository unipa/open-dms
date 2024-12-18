using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenDMS.Core.DTOs
{
    [Table("ProcessMeasures")]
    public class PerformanceIndicatorValue 
    {
        public string ProcessId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        // ID Istanza del processo
        public string ProcessInstanceId { get; set; }

        public string IndicatorId { get; set; }

        public decimal Value { get; set; }
    }
}
