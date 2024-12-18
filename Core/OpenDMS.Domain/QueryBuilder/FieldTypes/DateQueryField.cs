using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Domain.QueryBuilder.FieldTypes
{
    public class DateQueryField : QueryField
    {

        public DateQueryField(QueryModel modelId, string id, string title, string description, bool sortable = true)
            : base(modelId, id, ColumnDataType.Number, title, description, modelId.ToString(), 100, false, sortable)
        {
            AggregateTypes = new[] { AggregateType.Count, AggregateType.Min, AggregateType.Max, AggregateType.Average };
        }


    }
}
