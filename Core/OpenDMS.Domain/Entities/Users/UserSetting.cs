using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Users;

[PrimaryKey(nameof(CompanyId), nameof(ContactId), nameof(AttributeId))]
public class UserSetting
{
    public int CompanyId { get; set; } = 0;
    [StringLength(64)]
    [Required]
    public string ContactId { get; set; }
    [Required]
    [StringLength(64)]
    public string AttributeId { get; set; }
    public string Value { get; set; }

    //public virtual Contact Contact { get; set; }
    public UserSetting()
    {

    }
}

