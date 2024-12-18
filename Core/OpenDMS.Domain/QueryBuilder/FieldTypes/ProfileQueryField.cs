using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Domain.QueryBuilder.FieldTypes
{
    public class ProfileQueryField : QueryField
    {

        public ProfileQueryField(QueryModel modelId, string id, string title, string description, bool sortable = true)
            : base(modelId, id, ColumnDataType.Avatar, title, description, modelId.ToString(), 150, true, sortable)
        {
            AggregateTypes = new[] { AggregateType.Count };
        }


    }
}
