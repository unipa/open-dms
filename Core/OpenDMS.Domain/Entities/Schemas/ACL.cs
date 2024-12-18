using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Schemas;

public class ACL
{
    [StringLength(64)]
    public string Id { get; set; } = "";

    [StringLength(128)]
    public string Name { get; set; } = "";

    [StringLength(255)]
    public string Description { get; set; } = "";

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    public virtual List<ACLPermission> Permissions { get; set; } = new List<ACLPermission>();

    public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
}