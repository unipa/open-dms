using System.Text.Json.Serialization;

namespace OpenDMS.Domain.Models
{
    public class OrganizationNodeTree 
    {
        public OrganizationNodeInfo Info { get; set; }
        public List<UserInGroup> Users { get; set; } = new List<UserInGroup>();

//        [JsonIgnore]
//        public OrganizationNodeTree Parent { get; set; } = null;
        public List<OrganizationNodeTree> Nodes { get; set; } = new List<OrganizationNodeTree>();
    }
}