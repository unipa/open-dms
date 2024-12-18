using OpenDMS.Domain.Enumerators;

namespace OpenDMS.TaskList.API.DTOs
{
    public class TaskStatusSummary
    {
        public ExecutionStatus Status { get; set; }
        public string StatusName { get; set; }

        public int Value { get; set; }

    }
}
