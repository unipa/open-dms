using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Domain.QueryBuilder.FieldTypes
{
    public class QueryElement : IQueryElement
    {
        public string Id { get; internal set; }
        public QueryModel ModelId { get; internal set; }
        public ColumnDataType DataType { get; internal set; } 


        public QueryElement(QueryModel modelId, string id, ColumnDataType dataType = ColumnDataType.Text)
        {
            ModelId = modelId;
            Id = id;
            DataType = dataType;
        }

    }
}
