using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Models;

namespace OpenDMS.Core.DTOs
{
    public class TaskListContext
    {
        public string UserId { get; set; }
        public string Profile { get; set; }

        public string UserName { get; set; }

        public bool CanCreateTask { get; set; } = true;
        public bool CanCreateEventTask { get; set; } = true;
        public bool CanCreateMessage { get; set; } = true;
        public bool CanCreateFiltersForGroups { get; set; } = true;
        public bool CanCreateFiltersForRoles { get; set; } = true;
        public bool CanFilterByUser { get; set; } = true;


        public string SearchServiceEndPoint { get; set; }
        public string DocumentPreviewServiceEndPoint { get; set; }
        public string DocumentServiceEndPoint { get; set; }
        public string UISettingsServiceEndPoint { get; set; }
        public string UserServiceEndPoint { get; set; }

        public List<SearchFiltersGroup> SearchFilters { get; set; } = new List<SearchFiltersGroup>();
        public List<LookupTable> Groups { get; set; } = new List<LookupTable>();
        public List<LookupTable> Roles{ get; set; } = new List<LookupTable>();
        public List<LookupTable> Categories { get; set; } = new List<LookupTable>();
        public List<LookupTable> Priorities { get; set; } = new List<LookupTable>();
        public List<LookupTable> ExecutionStatus { get; set; } = new List<LookupTable>();
        public List<LookupTable> Companies { get; set; } = new List<LookupTable>();
        public List<LookupTable> Projects { get; set; } = new List<LookupTable>();

    }
}
