using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Reports.Model
{
    public class PerformanceIndicator
    {
        /// <summary>
        /// Id della dimensione
        /// </summary>
        [StringLength(64)]
        public string Id { get; set; }

        /// <summary>
        /// Id della categoria associata alla dimensione
        /// </summary>
        [StringLength(64)]
        public string CategoryId{ get; set; }

        /// <summary>
        /// Nome descrittivo della dimensione
        /// </summary>
        [StringLength(64)]
        public string Name { get; set; }

        /// <summary>
        /// Id dell'unita di misura di default
        /// </summary>
        [StringLength(64)]
        public string UnitOfMeasureId { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }


    }
}
