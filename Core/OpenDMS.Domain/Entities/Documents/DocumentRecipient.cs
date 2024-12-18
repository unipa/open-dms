using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace OpenDMS.Domain.Entities.Documents;

[Index(nameof(DocumentId), nameof(RecipientType), nameof(ProfileId), nameof(ProfileType), IsUnique = true)]
public class DocumentRecipient
{
    public int Id { get; set; }

    public int DocumentId { get; set; }

    /// <summary>
    ///  Tipo di recipiente
    ///  es. Mittente, destinatario, destinatario in cc, ...
    /// </summary>
    public RecipientType RecipientType { get; set; } // es. Mittente, Destinatario

    /// <summary>
    /// Identificativo del profilo
    /// </summary>
    [StringLength(64)]
    public string ProfileId { get; set; } = "";

    /// <summary>
    /// Tipo di profilo
    /// </summary>
    public ProfileType ProfileType { get; set; } // es. Utente, Gruppo, Partner

    /// <summary>
    /// Identiticativo originale del profilo (nel caso cambiasse)
    /// </summary>
    [StringLength(64)]
    public string InitialProfileId { get; set; } = "";

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

    [StringLength(64)]
    public string LastUpdateUser { get; set; } = "";

    [JsonIgnore]
    public virtual Document Document { get; set; }

}
