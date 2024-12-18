using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Domain.QueryBuilder.FieldTypes
{
    public class IconQueryField : QueryField
    {

        public IconQueryField(QueryModel modelId, string id, string description, string category, string images, string tooltips = "")
            : base(modelId, id, ColumnDataType.Icon, "", description, category, 24, false, false)
        {
            AggregateTypes = new[] { AggregateType.Count };
            LookupValues = images.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            ToolTips = tooltips.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        }


        public IconQueryField(QueryModel modelId, string id, string description, string images, string tooltips = "")
            : this(modelId, id, description, modelId.ToString(), images, tooltips)
        {
        }



    }
}
