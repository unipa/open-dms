using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Reports.Model
{
    public class UnitOfMeasure
    {
        [StringLength(64)]
        public string Id { get; set; }
        [StringLength(128)]
        public string Name { get; set; }
        [StringLength(4)]
        public string Prefix { get; set; }
        
        [StringLength(4)]
        public string Suffix { get; set; }

        // Divisore rispetto all'unità base 
        // Esempio
        // Unita Base = secondi
        // Name = "Ore"
        // Suffix = "H";
        // Divider = 3600
        public decimal Divider { get; set; }
        public decimal Decimal { get; set; }
    }
}
