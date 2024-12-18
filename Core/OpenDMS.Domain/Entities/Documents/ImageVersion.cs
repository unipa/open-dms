using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Documents;

[Index(nameof(DocumentId), nameof(ImageId), IsUnique = true)]
public class ImageVersion
{
    public int Id { get; set; }

    public int DocumentId { get; set; }
    public int ImageId { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    [StringLength(64)]
    public string CreationUser { get; set; }

    [JsonIgnore]
    public virtual Document Document { get; set; }
    [JsonIgnore]
    public virtual DocumentImage Image { get; set; }

}
