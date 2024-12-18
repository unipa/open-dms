using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using OpenDMS.Domain.Entities.Schemas;


namespace OpenDMS.Domain.Entities.Documents;

[Index(nameof(DocumentId), nameof(FieldIndex), nameof(Chunk), IsUnique = true)]
[Index(nameof(DocumentId), nameof(FieldName), nameof(Chunk), IsUnique = true)]
[Index(nameof(FieldTypeId), nameof(FormattedValue), IsUnique = false)]
//[Index(nameof(DocumentTypeId), nameof(FieldTypeId), nameof(FormattedValue), IsUnique = false)]
public class DocumentField
{
    public int Id { get; set; }
    public int DocumentId { get; set; }
    public int FieldIndex { get; set; }

    /// <summary>
    /// Indica il segmento del campo per quei valori che superano la lunghezza di 255 caratteri
    /// Nelle ricerche e negli ordinamenti, viene preso in considerazione solo il chunk nr.1
    /// </summary>
    public int Chunk { get; set; } = 1;

    [StringLength(64)]
    public string DocumentTypeId { get; set; }


    /// <summary>
    /// Identificativo univoco del campo all'interno del DocumentType
    /// </summary>
    [StringLength(64)]
    public string FieldName { get; set; }

    /// <summary>
    /// Identificativo del metadato
    /// </summary>
    [StringLength(64)]
    public string FieldTypeId { get; set; }

    /// <summary>
    /// Valore originale del campo
    /// </summary>
    [StringLength(255)]
    public string Value { get; set; } = "";

    /// <summary>
    /// Valore formattato. 
    /// Utilizzato nelle ricerche per gestire correttamente l'ordinamento di campi numerici, data, bit, ...
    /// </summary>
    [StringLength(255)]
    public string FormattedValue { get; set; } = "";

    /// <summary>
    /// Valore di decodifica
    /// </summary>
    [StringLength(255)]
    public string LookupValue { get; set; } = "";

    public bool Encrypted { get; set; }

    /// <summary>
    /// Indica che il campo è stato inserito in modo dinamico e non è previsto nella tipologia
    /// </summary>
    public bool Customized { get; set; }

    /// <summary>
    /// Campo gestito dalla logica di business
    /// Non gestibile/personalizzabile/cancellabile dall'utente
    /// </summary>
    public bool Internal { get; set; }

    /// <summary>
    /// Indica che il campo contiene valori multipli
    /// </summary>
    public bool Tag { get; set; }

    public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

    [StringLength(64)]
    public string LastUpdateUser { get; set; } = "";

    [JsonIgnore]
    public virtual Document Document { get; set; }

    [JsonIgnore]
    public virtual DocumentType DocumentType { get; set; }

    /// <summary>
    /// Indica che il campo è stato inserito nella tabella dei Blob
    /// </summary>
    public int? BlobId { get; set; }
    public virtual DocumentBlobField Blob { get; set; }


}

[Index(nameof(DocumentId), nameof(FieldIndex), IsUnique = true)]
[Index(nameof(DocumentId), nameof(FieldName), IsUnique = true)]
public class DocumentBlobField
{
    public int Id { get; set; }
    public int DocumentId { get; set; }
    public int FieldIndex { get; set; }

    [StringLength(64)]
    public string DocumentTypeId { get; set; }


    /// <summary>
    /// Identificativo univoco del campo all'interno del DocumentType
    /// </summary>
    [StringLength(64)]
    public string FieldName { get; set; }

    /// <summary>
    /// Identificativo del metadato
    /// </summary>
    [StringLength(64)]
    public string FieldTypeId { get; set; }


    public string Value { get; set; }

    public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

    [StringLength(64)]
    public string LastUpdateUser { get; set; } = "";

    [JsonIgnore]
    public virtual Document Document { get; set; }

    [JsonIgnore]
    public virtual DocumentType DocumentType { get; set; }


}

