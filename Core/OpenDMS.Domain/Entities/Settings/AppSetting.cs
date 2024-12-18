using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Settings;

[PrimaryKey(nameof(CompanyId), nameof(Name))]
public class AppSetting
{
    public int CompanyId { get; set; } = 0;
    [Required]
    [StringLength(255)]
    public string Name { get; set; }
    public string Value { get; set; }

}

