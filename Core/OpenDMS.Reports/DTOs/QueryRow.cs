using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Reports.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Reports.DTOs
{
    public class QueryRow
    {
        // Elenco di colonne che rappresentano variabili
        public List<LookupTable> Variables { get; set; } = new();
        public List<IndicatorValue> Indicators { get; set; } = new();

    }
}
