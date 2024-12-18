using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace OpenDMS.Domain.Entities.Documents;

[Index(nameof(DocumentId), nameof(AttachmentId), IsUnique = true)]
public class DocumentRelationship
{
    public int Id { get; set; }
    public int DocumentId { get; set; }

    /// <summary>
    /// Identificativo dell'immagine del documento corrente, al momento della creazione del legame
    /// </summary>
    public int ImageId { get; set; }


    public int AttachmentId { get; set; }

    /// <summary>
    /// Identificativo dell'immagine dell'allegato associata
    /// </summary>
    public int AttachmentImageId { get; set; }

    public bool IsLinked { get; set; }

    public DateTime CreationDate { get; set; } = new DateTime();

    [StringLength(64)]
    public string CreationUser { get; set; }

    [JsonIgnore]
    public virtual Document Document { get; set; }

    [JsonIgnore]
    public virtual Document Attachment { get; set; }
}
