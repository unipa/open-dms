using OpenDMS.Domain.Entities.Settings;

namespace OpenDMS.Domain.QueryBuilder;

public interface IQueryFieldRenderer
{
    LookupTable Render(QueryRow queryResult);
}
