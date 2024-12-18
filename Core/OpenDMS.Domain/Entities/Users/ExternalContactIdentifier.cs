using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Users;

[PrimaryKey(nameof(SourceId), nameof(SourceCode))]
[Index(nameof(ContactId), IsUnique = false)]

public class ExternalContactIdentifier
{
    [StringLength(64)]
    public string SourceId { get; set; } = "";
    [StringLength(64)]
    public string SourceCode { get; set; } = "";

    public int ContactId { get; set; } = 0;

    public virtual Contact Contact { get; set; }
}

