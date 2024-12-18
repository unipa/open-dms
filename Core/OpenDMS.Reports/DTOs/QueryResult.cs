using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Reports.DTOs
{
    public class QueryResult
    {
        public int Records { get; set; }

        public string[] Columns { get; set; }
        public string[][] Rows { get; set; }


    }
}
