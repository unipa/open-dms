using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Documents;


[Index(nameof(ExternalId), nameof(DocumentStatus), IsUnique=false)]
[Index(nameof(DocumentTypeId), nameof(DocumentStatus), IsUnique = false)]
[Index(nameof(FolderId), IsUnique = false)]
[Index(nameof(MasterDocumentId), IsUnique = false)]

public partial class Document
{
    public int Id { get; set; } = 0;
    public int CompanyId { get; set; } = 0;

    /// <summary>
    ///  Identificativo esterno, univoco, del documento 
    /// </summary>
    [StringLength(255)]
    public string ExternalId { get; set; } = "";
    public ContentType ContentType { get; set; }

    [StringLength(64)]
    public string? DocumentTypeId { get; set; }

    /// <summary>
    /// Identificativo del documento originale dal quale questo deriva
    /// </summary>
    public int? MasterDocumentId { get; set; } = 0;
    public DocumentStatus DocumentStatus { get; set; } = DocumentStatus.Active;

    [StringLength(8)]
    public string IconColor { get; set; } = "";

    [StringLength(64)]
    public string Icon { get; set; } = "";

    public string Description { get; set; } = "";

    [StringLength(255)]
    public string DocumentNumber { get; set; } = "";

    [StringLength(255)]
    public string DocumentFormattedNumber { get; set; } = "";

    [StringLength(255)]
    public string? DocumentNumberDataType { get; set; } = null;

    public DateTime? DocumentDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public ExpirationStrategy ExpirationStrategy { get; set; }

    public int ExpirationTolerance { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    public DateTime? DeletionDate { get; set; }

    public DateTime? LastUpdate { get; set; }

    [StringLength(64)]
    public string LastUpdateUser { get; set; } = "";

    public int? ImageId { get; set; } 

    public int? FolderId { get; set; } = 0;

    public string Referents { get; set; } = "";

    public string ReferentsCC { get; set; } = "";

    public DateTime? ConsolidationDate { get; set; }



    [StringLength(64)]
    public string? ACLId { get; set; } = "";
    public bool Reserved { get; set; }
    public bool PersonalData { get; set; }

    [StringLength(64)]
    public string Owner { get; set; } = "";


    public virtual List<DocumentField> Fields { get; set; }
    public virtual DocumentImage Image { get; set; }
    public virtual DocumentType DocumentType { get; set; }
    //public virtual Company Company { get; set; }

}


