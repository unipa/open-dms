using OpenDMS.Core.DTOs;

namespace Web.DTOs
{
    public class ProcessInstanceInfo_DTO
    {
        public string Id { get; set; }
        public string Icon { get; set; }
        public string IconColor { get; set; }
        public string Description { get; set; }
        public int DefinitionId { get; set; }
        public string DefinitionKey { get; set; }
        public int DocumentId { get; set; }
        public string EventName { get; set; }
        public string Version { get; set; }
        public string StartTime { get; set; }
        public string StartUserId { get; set; }
        public string StartUserName { get; set; }
        public List<ProcessIncident> Problems { get; set; } = new();
        public Dictionary<string, string> Variables { get; set; } = new();

        public List<UserTaskInfo> ActiveTasks { get; set; } = new();
        public List<UserTaskInfo> ClosedTasks { get; set; } = new();
    }

}
