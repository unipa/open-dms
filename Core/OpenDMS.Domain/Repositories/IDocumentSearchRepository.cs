using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;

namespace OpenDMS.Domain.Repositories;
public interface IDocumentSearchRepository
{
    Task<List<Dictionary<string, LookupTable>>> GetPage(UserProfile userInfo, List<SearchFilter> SearchFilters, List<string> columns, List<SortingType> orderby, int skip, int take);
    Task<int> Count(UserProfile userInfo, List<SearchFilter> SearchFilters);
}
