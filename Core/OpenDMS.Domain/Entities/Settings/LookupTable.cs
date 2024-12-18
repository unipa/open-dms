using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Settings;

[PrimaryKey(nameof(TableId), nameof(Id))]
public class LookupTable
{
    [StringLength(64)]
    public string TableId { get; set; }
    [StringLength(64)]
    public string Id { get; set; }
    [StringLength(255)]
    public string Description { get; set; }
    [StringLength(255)]
    public string Annotation { get; set; }

}
