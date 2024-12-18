namespace Web.DTOs
{
    public class ProcessSummary_DTO
    {
        public int  Id { get; set; }
        public string Description { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int Count { get; set; }
        public int IntimeInstances { get; set; }
        public int DelayedInstances { get; set; }

        public string Version { get; set; }
        public int Users { get; set; }
    }
}
