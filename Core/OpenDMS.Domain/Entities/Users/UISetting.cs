using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Users;

[PrimaryKey(nameof(CompanyId), nameof(UserId), nameof(Name))]
public class UISetting
{
    public int CompanyId { get; set; }
    [StringLength(64)]
    [Required]
    public string UserId { get; set; }
    [StringLength(64)]
    [Required]
    public string Name { get; set; }

    public string Value { get; set; }

}

