namespace OpenDMS.Domain.Models
{
    public class SearchFilters
    {
        public int Id { get; set; }
        public string Icon { get; set; }


        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string GroupId { get; set; }

        public bool SystemFilter { get; set; }
        public int Badge { get; set; }

        public string Name { get; set; }

        public List<SearchFilter> Filters { get; set; } = new List<SearchFilter>();

        public SearchFilters()
        {
            
        }
        public SearchFilters(Entities.Tasks.TaskListCustomFilter f)
        {
            this.Id = f.Id; 
            this.Icon = f.Icon;
            this.SystemFilter = f.SystemFilter;
            this.Name = f.Name;
            this.Filters = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SearchFilter>>(f.SerializedFilters);
        }
    }
}
