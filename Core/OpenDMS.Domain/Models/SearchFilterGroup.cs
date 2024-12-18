using OpenDMS.Domain.Entities;

namespace OpenDMS.Domain.Models
{
    public class SearchFiltersGroup
    {
        public string Name { get; set; }

        public List<SearchFilters> Filters { get; set; } = new List<SearchFilters>();

    }
}
