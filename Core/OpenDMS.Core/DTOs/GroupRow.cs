namespace OpenDMS.Core.DTOs
{
    public class GroupRow
    {
        public List<SearchResultRow> Rows { get; set; } = new List<SearchResultRow>();
        public int ColumnIndex { get; set; } = 0;
        public string Value { get; set; } = "";
        public string Description { get; set; } = "";
        public int Count { get; set; }

        public List<string> Totals { get; set; } = new List<string>();
    }
}