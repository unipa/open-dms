using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Domain.QueryBuilder.FieldTypes
{
    public class NumberQueryField : QueryField
    {

        public NumberQueryField(QueryModel modelId, string id, string title, string description, string category, int size = 100, bool resizable = true, bool sortable = true)
            : base(modelId, id, ColumnDataType.Number, title, description, category, size, resizable, sortable)
        {
            AggregateTypes = new[] { AggregateType.Count, AggregateType.Sum, AggregateType.Min, AggregateType.Max, AggregateType.Average };
        }


        public NumberQueryField(QueryModel modelId, string id, string title, string description, int size = 100, bool sortable = true)
            : this(modelId, id, title, description, modelId.ToString(), size, true, sortable)
        {
        }



    }
}
