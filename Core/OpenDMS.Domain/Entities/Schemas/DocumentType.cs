using OpenDMS.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OpenDMS.Domain.Entities.Schemas;

public class DocumentType
{
    [StringLength(64)]
    public string Id { get; set; } = "";

    public int CompanyId { get; set; } = 0;

    public ContentType ContentType { get; set; } = ContentType.Document;

    [StringLength(255)]
    [Required]
    public string Name { get; set; } = "";

    [StringLength(64)]
    public string CategoryId { get; set; } = "";

    [StringLength(64)]
    public string Owner { get; set; } = "";

    public string Description { get; set; } = "";

    [StringLength(64)]
    public string Icon { get; set; } = "";

    [StringLength(8)]
    public string IconColor { get; set; } = "";

    public DocumentStatus InitialStatus { get; set; }


    /// <summary>
    /// Indica che la tipologia è interna e non può essere cancellata
    /// </summary>
    public bool Internal { get; set; }

    //public bool BatchScan { get; set; }

    public bool ToBeSigned { get; set; }

    public bool ToBePublished { get; set; }

    public bool ToBePreserved { get; set; }

    public bool ToBeIndexed { get; set; }

    /// <summary>
    /// Elenco di estensioni accettate per la tipologia, separate da virgola
    /// </summary>
    [StringLength(128)]
    public string AcceptedExtensions { get; set; } = "";

    public int MaxVersions { get; set; }

    [StringLength(64)]
    public string FileNamingTemplate { get; set; } = "";



    /* SICUREZZA */
    [StringLength(64)]
    public string ACLId { get; set; }

    [StringLength(64)]
    public string FileManager { get; set; } = "";
    public bool PersonalData { get; set; }
    public bool Reserved { get; set; }


    /* DATI PROTOCOLLO */
    //[StringLength(1)]
    //public string ProtocolDirection { get; set; } = "";

    //[StringLength(64)]
    //public string ProtocolRegister { get; set; } = "";

    //[StringLength(64)]
    //public string ProtocolInventory { get; set; } = "";


    public LabelPosition LabelPosition { get; set; } = LabelPosition.Manuale;
    public int LabelX { get; set; }
    public int LabelY { get; set; }



    /* SCADENZA */
    public ExpirationStrategy ExpirationStrategy { get; set; }

    //[StringLength(64)]
    //public string ExpirationOwner { get; set; } = "";
    public int ExpirationDays { get; set; }
    public int ExpirationTolerance { get; set; }



    //[StringLength(64)]
    //public string ProtocolChannel { get; set; } = "";


    //public bool IncludeAttachments { get; set; }
    //public bool SendOnlySignedFiles { get; set; }
    //public bool IncludePreview { get; set; }


    /* METADATI */
    [StringLength(128)]
    public string DescriptionLabel { get; set; } = "";
    [StringLength(128)]
    public string DocumentDateLabel { get; set; } = "";
    [StringLength(128)]
    public string DocumentNumberLabel { get; set; } = "";


    [StringLength(64)]
    public string DocumentNumberDataType { get; set; }


    [NotMapped]
    public int FieldCount { get { return Fields?.Count ?? 0; } }

    public virtual List<DocumentTypeField> Fields { get; set; } = new();

    [JsonIgnore]
    public virtual ACL ACL { get; set; }


    //Migrazione #2

    /// <summary>
    /// Pagina di dettaglio del tipo di documento
    /// Se vuoto viene presa /
    /// </summary>
    [StringLength(64)]
    public string DetailPage { get; set; } = "";



    public bool DocumentNumberMandatory { get; set; }
    public bool DescriptionMandatory { get; set; }
    public bool ConvertToPDF { get; set; }


    /// <summary>
    /// 0=Document Interno, 1=Documento Esterno in Ingresso, 2 = Documento Interno in Uscita
    /// </summary>
    public int Direction { get; set; } = 0;

    /// <summary>
    /// Pagina da utilizzare per la creazione del documento
    /// Se vuoto viene presa quella di default
    /// </summary>
    [StringLength(64)]
    public string CreationFormKey { get; set; } = "";

}

