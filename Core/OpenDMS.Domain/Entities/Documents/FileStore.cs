using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Documents;

[Index(nameof(FileManager), nameof(Path), nameof(FileName), IsUnique = true)]
[Index(nameof(FileHash), IsUnique = false)]
public class FileStore
{
    [Key]
    public int Id { get; set; }

    [StringLength(64)]
    public string FileManager { get; set; }

    [StringLength(255)]
    public string FileName { get; set; }

    [StringLength(64)]
    public string FileExtension { get; set; }
    [StringLength(255)]
    public string Path { get; set; }

    [StringLength(255)]
    public string FileHash { get; set; }

    [StringLength(64)]
    public string Owner { get; set; }
    public DateTime CreationDate { get; set; }
    public long FileSize { get; set; }
    public byte[] Content { get; set; }

}
