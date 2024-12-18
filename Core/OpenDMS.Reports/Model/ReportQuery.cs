using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Reports.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Reports.Model
{
    public class ReportQueryColumn
    {
        public string ColumnId { get; set; }
        public string Label { get; set; }
        public AggregateType  AggregateType { get; set; } = AggregateType.None;

        /// <summary>
        /// Id Unità di misura 
        /// Per le dimensioni è vuoto
        /// </summary>
        public string UnitId { get; set; }
    }


    public class ReportQuery
    {
        public string Title { get; set; } = "";
        public string SubTitle { get; set; } = "";

        /// <summary>
        /// Indice della query padre che ha creato la corrente
        /// </summary>
        public int ParentQueryIndex { get; set; } = -1;

        /// <summary>
        /// TODO Creare una tabella "REPORT STYLE" per le tipologie di report
        /// </summary>
        public string ReportType { get; set; }

        public List<ReportQueryColumn> Dimensions { get; set; } = new();
        public List<ReportQueryColumn> Measures { get; set; } = new();

        public SearchFilters Filters { get; set; } = new();

    }
}
