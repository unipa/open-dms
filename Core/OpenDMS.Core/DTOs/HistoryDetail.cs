namespace OpenDMS.Core.DTOs
{
    public class HistoryDetail
    {
        public string Label { get; set; }
        public string[] Values { get; set; }

        public List<HistoryDetail> Details { get; set; } = new List<HistoryDetail>();
    }
}
