using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.QueryBuilder;


namespace OpenDMS.Infrastructure.Database.Builder
{

        public record SelectField
        {
            public string Alias { get; }
            public string Field { get; }
            public AggregateType AggregateType { get; }

            public SelectField(string alias, string field, AggregateType aggregateType)
            {
                Alias = alias;
                Field = field;
                AggregateType = aggregateType;
            }
        }
}
