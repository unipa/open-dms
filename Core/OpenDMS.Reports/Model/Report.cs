using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Reports.Model
{
    public class Report
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public List<ReportQuery> Queries { get; set; }

    }
}
