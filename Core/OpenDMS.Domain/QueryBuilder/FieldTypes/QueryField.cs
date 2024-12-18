using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Domain.QueryBuilder.FieldTypes
{
    public class QueryField : IQueryElement, IQueryField
    {
        public string Id { get; internal set; }
        public QueryModel ModelId { get; internal set; }
        public ColumnDataType DataType { get; internal set; } = ColumnDataType.Text;

        /// <summary>
        /// Titolo della colonna da inserire come intestazione di tabella
        /// </summary>
        public string Title { get; internal set; }

        /// <summary>
        /// Titolo/descrizione estesa della colonna
        /// </summary>
        public string Description { get; internal set; }

        /// <summary>
        /// Raggruppamento della colonna
        /// </summary>
        public string Category { get; internal set; }

        /// <summary>
        /// Dimensione Minima
        /// </summary>
        public int Size { get; internal set; }

        /// <summary>
        /// Indica se il campo è ridimensionabile
        /// </summary>
        public bool Resizable { get; internal set; }

        /// <summary>
        /// Indica se il campo è ordinabile
        /// </summary>
        public bool Sortable { get; internal set; }

        /// <summary>
        /// Elenco posizionale di valori di decodifica per le colonne numeriche o binarie
        /// </summary>
        public string[] LookupValues { get; internal set; }

        /// <summary>
        /// Elenco posizionale di valori di tooltip per le colonne numeriche o binarie
        /// </summary>
        public string[] ToolTips { get; internal set; }

        /// <summary>
        /// Elenco delle funzioni di aggregazione disponibili
        /// </summary>
        public AggregateType[] AggregateTypes { get; internal set; }


        public QueryField(QueryModel modelId, string id, ColumnDataType dataType,  string title, string description, string category, int size = 0, bool resizable = true, bool sortable = true)
        {
            ModelId = modelId;
            Id = id;
            Title = title;
            Description = description;
            Category = category;
            Size = size;
            Resizable = resizable;
            Sortable = sortable;
            DataType = dataType;

            LookupValues = null;
            ToolTips = null;
            AggregateTypes = new[] { AggregateType.Count, AggregateType.Min, AggregateType.Max };
        }

        public QueryField(QueryModel modelId, string id, ColumnDataType dataType, string title, string description, int size = 0, bool sortable = true)
            : this(modelId, id, dataType, title, description, modelId.ToString(), size, true, sortable)
        {
        }



    }
}
