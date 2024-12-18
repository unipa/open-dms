using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Core.ViewModel
{
    public class IViewColumn : IQueryField
    {
        public QueryModel ModelId { get; }

        public string Id { get; }

        public ColumnDataType DataType { get; }


        public string Title { get; }
        public string Description { get; }
        public string Category { get; }
        public int Size { get; }
        public bool Sortable { get; }
        public bool Resizable { get; }
        public string[] LookupValues { get; }
        public string[] ToolTips { get; }

    }
}
