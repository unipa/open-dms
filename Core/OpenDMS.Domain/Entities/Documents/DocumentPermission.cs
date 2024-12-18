using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenDMS.Domain.Entities.Documents;

[PrimaryKey(nameof(DocumentId), nameof(ProfileId), nameof(ProfileType), nameof(PermissionId))]
public class DocumentPermission
{
    public int DocumentId { get; set; }

    /// <summary>
    /// Può essere un profilo (64 char) o l'unione di 2 o più profili (es. gruppo\ruolo)
    /// </summary>
    [StringLength(129)]
    public string ProfileId { get; set; }
    public ProfileType ProfileType { get; set; }


    [StringLength(64)]
    public string PermissionId { get; set; }

    public AuthorizationType Authorization { get; set; }

    public DocumentPermission() { }

    public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public virtual Document Document { get; set; }


    public DocumentPermission(int documentId, string profileId, ProfileType userType, string permission)
    {
        DocumentId = documentId;
        ProfileId = profileId;
        ProfileType = userType;
        PermissionId = permission;
        Authorization = AuthorizationType.None;
    }



}

