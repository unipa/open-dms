using OpenDMS.Domain.QueryBuilder;


namespace OpenDMS.Infrastructure.Database.Builder
{

        public record SortField
        {
            public string Field { get; }
            public bool Ascending { get; }

            public SortField(string field, bool ascending)
            {
                Field = field;
                Ascending = ascending;
            }
        }
}
