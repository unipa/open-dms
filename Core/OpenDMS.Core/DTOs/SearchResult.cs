using OpenDMS.Domain.Entities.Settings;

namespace OpenDMS.Core.DTOs
{

    public class GroupedResult
    {
        public string Value { get; set; }
        public string Description { get; set; }
        public int Total { get; set; }
    }
    public class FilterGroupResult
    {
        public string Label { get; set; }
        public string ColumnId { get; set; }
        public List<GroupedResult> Values { get; set; } = new();
    }

    public class SearchResult
    {
        public ViewProperties View { get; set; } = new ViewProperties();
        public SearchResultPage Page { get; set; } = new SearchResultPage();
        public int Pages { get; set; }
        public int Count { get; set; }
        public List<SearchResultColumn> Totals { get; set; } = new List<SearchResultColumn>();
        public string Title { get; set; } = "";

        public bool HideSelection { get; set; }

        public List<FilterGroupResult> Filters { get; set; } = new List<FilterGroupResult>();

    }
}
