using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using OpenDMS.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Documents;



[Index(nameof(FileName), nameof(FileNameHash), IsUnique = false)]
[Index(nameof(Hash), IsUnique = false)]
public class DocumentImage
{


    public int Id { get; set; }

    public int VersionNumber { get; set; }
    public int RevisionNumber { get; set; }


    [Required(ErrorMessage = "Non è stato indicato il nome del file")]
    [StringLength(255), MinLength(1)]
    public string FileName { get; set; } = "";

    public bool IsLinked { get; set; }

    [Required(ErrorMessage = "Non è stato indicato il percorso del file")]
    [StringLength(255), MinLength(1)]
    public string OriginalPath { get; set; } = "";

    [StringLength(64)]
    public string FileManager { get; set; } = "";


    [StringLength(255)]
    public string FileNameHash { get; set; } = "";


    [StringLength(64)]
    public string FileExtension { get; set; } = "";

    [StringLength(255)]
    public string Hash { get; set; } = "";


    [StringLength(64)]
    public string Owner { get; set; } = "";
    public bool Deleted { get; set; }
    public long FileSize { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime? DeletionDate { get; set; }
    [StringLength(255)]
    public string OriginalFileName { get; set; } = "";

    [StringLength(64)]
    public string CheckOutUser { get; set; } = "";

    /// <summary>
    /// Dati relativi alla conservazione
    /// </summary>
    [StringLength(64)]
    public string PreservationUser { get; set; } = "";
    [StringLength(64)]
    public string PreservationPDV { get; set; } = "";
    public DateTime? PreservationDate { get; set; }


    /// <summary>
    /// ID della sessione di firma
    /// </summary>
    [StringLength(64)]
    public string SignatureSession { get; set; }

    /// <summary>
    /// Id dell'utente che ha firmato/firmerà il documento
    /// </summary>
    [StringLength(64)]
    public string SignatureUser { get; set; }
    public DateTime? SignatureDate { get; set; }
    public int Signatures { get; set; } = 0;



    [StringLength(64)]
    public string SendingUser { get; set; } = "";
    public DateTime? SendingDate { get; set; }

    // Identificativo Univoco della spedizione
    [StringLength(128)]
    public string SendingIdentifier { get; set; } = "";

    [StringLength(64)]
    public string DeletionUser { get; set; } = "";

    [JsonIgnore]
    public virtual List<ImageVersion> Versions { get; set; } = new List<ImageVersion>();


    public JobStatus PreservationStatus { get; set; }
    public JobStatus SignatureStatus { get; set; }
    public JobStatus SendingStatus { get; set; }

    public JobStatus StoringStatus { get; set; }
    public JobStatus IndexingStatus { get; set; }
    public JobStatus PreviewStatus { get; set; }


}
