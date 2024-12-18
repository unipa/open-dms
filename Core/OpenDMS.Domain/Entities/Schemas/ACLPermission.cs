using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OpenDMS.Domain.Entities.Schemas;

/// <summary>
/// Permessi sull'ACL.
/// Una ACL viene associata ad una o piu tipologie di documenti e definisce chi puo utilizzarle
/// ACLPermission associa ad un utente/ruolo o gruppo un permesso specifico (es. creazione) sulle tipologie in cui viene inserita l'ACL
/// </summary>
[PrimaryKey(nameof(ACLId), nameof(ProfileId), nameof(ProfileType), nameof(PermissionId))]
public class ACLPermission
{
    [StringLength(64)]
    public string ACLId { get; set; } = "";

    [StringLength(255)]
    public string ProfileId { get; set; }

    public ProfileType ProfileType { get; set; }

    [StringLength(64)]
    public string PermissionId { get; set; }

    // [NotMapped]
    // public string PermissionName { get { return PermissionType.Name[PermissionId]; } }

    public AuthorizationType Authorization { get; set; }

    //[JsonIgnore]
    //public virtual ACL ACL { get; set; }


    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
}