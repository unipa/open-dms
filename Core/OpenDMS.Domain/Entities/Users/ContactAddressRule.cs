using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Users;

/// <summary>
/// Regole per l'auto-collegamento di mail e pec ai comtatti
/// </summary>
[PrimaryKey(nameof(AddresType), nameof(Address))]
public class ContactAddressRule
{
    public DigitalAddressType AddresType { get; set; }

    [StringLength(255)]
    public string Address { get; set; }

    [StringLength(64)]
    public string ContactId { get; set; }



    [StringLength(64)]
    public string CreationUser { get; set; }

    [StringLength(64)]
    public string LastUpdateUser { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

    public virtual Contact Contact { get; set; }


}
