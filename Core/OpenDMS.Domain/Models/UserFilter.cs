namespace OpenDMS.Domain.Models
{
    public class UserFilter
    {
        public bool IncludeDeletes { get; set; } = false;
        public string userGroupId { get; set; } = "*";
        public string roleId { get; set; } = "*";
        public string filter { get; set; } = "*";
    }
}
