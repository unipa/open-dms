

using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.QueryBuilder;
using RtfPipe.Tokens;

namespace OpenDMS.Core.DTOs
{
    public class ViewColumn
    {

        public static List<AggregateType> textfunctions = new()
        {
            AggregateType.Count
        };
        public static List<AggregateType> allfunctions = new()
        {
            AggregateType.Count,
            AggregateType.Sum,
            AggregateType.Min,
            AggregateType.Max,
            AggregateType.Average
        };
        public static List<AggregateType> datefunctions = new()
        {
            AggregateType.Count
        };

        // Id della colonna
        public string Id { get; set; } = "";
        public string Category { get; set; } = ""; // es. documento, icona, pulsante ...
        public string Description { get; set; } = ""; // es. documento, icona, pulsante ...
        public bool IsSortable { get; set; } = true;
        public bool IsResizable { get; set; } = true;
        public ColumnDataType DataType { get; set; } = ColumnDataType.Text;

        /// <summary>
        /// Valori da mostrare quando il campo è di tipo icona o azione
        /// </summary>
        public List<string> LookupValues { get; set; } = null;

        public List<string> Tooltips { get; set; } = null;
        public string Tooltip
        {
            get {
                return (Tooltips != null && Tooltips.Count > 0) ? Tooltips[0] : string.Empty;
            }
        }
 
        public List<AggregateType> AggregateTypes { get; set; } = textfunctions;

        // Elementi personalizzabili
        public ViewColumnSettings Settings { get; set; } = new ViewColumnSettings();

        public bool IsGroupable { get; set; }

        public List<string> Fields { get; set; } = new();
        public ViewColumn()
        {
        }

        public ViewColumn(
            string id,
            ColumnDataType dataType,
            List<string> fields,
            string title,
            string description,
            string category,
            int size = 0,
            bool resizable = true,
            bool sortable = true,
            List<AggregateType> aggregateTypes = null,
            List<string> tooltips = null,
            List<string> lookupvalues = null
            )
        {
            Id = id;
            DataType = dataType;
            Fields = fields is null ? new List<string> { id } : fields;
            Settings.Title = title;
            Description = description;
            Category = category;
            IsResizable = resizable;
            IsSortable = sortable;
            AggregateTypes = aggregateTypes;
            Tooltips = tooltips;
            LookupValues = lookupvalues;
            Settings.Width =  size < 0 ? "24px" : size == 0 ? "250px" : size.ToString()+"px";
            Settings.Visible = true;
        }

        public ViewColumn(
            string id,
            ColumnDataType dataType,
            string title,
            string description,
            string category,
            int size = 0,
            bool resizable = true,
            bool sortable = true,
            List<AggregateType> aggregateTypes = null,
            List<string> tooltips = null,
            List<string> lookupvalues = null) : this(id, dataType, new List<string>() { id }, title, description, category, size, resizable, sortable, aggregateTypes, tooltips, lookupvalues)
        { }
        public ViewColumn(
            string id,
            ColumnDataType dataType,
            string title,
            string category,
            int size = 0,
            bool resizable = true,
            bool sortable = true,
            List<AggregateType> aggregateTypes = null,
            List<string> tooltips = null,
            List<string> lookupvalues = null) : this(id, dataType, new List<string>() { id }, title, title, category, size, resizable, sortable, aggregateTypes, tooltips, lookupvalues)
        { }

        public virtual async Task<SearchResultColumn> Render(string[] fields)
        {
            SearchResultColumn Column = new SearchResultColumn()
            {
                Value = fields[0],
                Description = fields[fields.Length-1]
            };
            return Column;           
        }
    }
}