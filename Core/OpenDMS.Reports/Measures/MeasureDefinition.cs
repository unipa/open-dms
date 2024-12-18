using OpenDMS.Domain.Entities;
using OpenDMS.Reports.Enumerators;
using OpenDMS.Reports.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Reports.Measures
{
    public class MeasureDefinition
    {
        // Titolo del gestore
        // es. Attività Utente
        public string Title { get; set; }

        public string Icon { get; set; }

        public string Description { get; set; }

        public List<PerformanceIndicator> GetAllIndicators() { return new(); }

        public List<AggregationFunction> GetVariables() { return new(); }

        public List<AggregationFunction> GetAggregationFunctionsByIndicator(string indicatorId) { return new(); }





    }
}
