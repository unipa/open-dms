using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Documents;

[Index(nameof(FolderId), nameof(DocumentId), IsUnique = true)]
[Index(nameof(FolderId), IsUnique = false)]
[Index(nameof(DocumentId), IsUnique = false)]
public class FolderContent
{
    public int Id { get; set; }

    public int DocumentId { get; set; }
    public int FolderId { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    [StringLength(64)]
    public string CreationUser { get; set; }

    public virtual Document Document { get; set; }
    public virtual Document Folder { get; set; }
}
