namespace OpenDMS.Core.DTOs
{
    public class TaskProgressInfo
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int UserTaskId { get; set; }
        public int TaskItemId { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public decimal Percentage { get; set; }
        public DateTime CreationDate { get; set; }

    }
}
