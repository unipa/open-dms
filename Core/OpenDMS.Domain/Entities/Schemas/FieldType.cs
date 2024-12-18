using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenDMS.Domain.Entities.Schemas;

public class FieldType
{
    [StringLength(64)]
    public string Id { get; set; }

    /// <summary>
    /// Nome descrittivo del metadato
    /// </summary>
    [StringLength(128)]
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Codice raggruppamento del metadato
    /// </summary>
    [StringLength(64)]
    public string CategoryId { get; set; }

    [StringLength(255)]
    public string Description { get; set; }

    /// <summary>
    /// Nome visualizzato del metadato 
    /// </summary>
    [StringLength(128)]
    [Required]
    public string Title { get; set; }

    /// <summary>
    /// Identificativo della DataTypeFactory che gestisce il metadato
    /// </summary>
    [StringLength(64)]
    [Required]
    public string DataType { get; set; }

    [StringLength(255)]
    public string DefaultValue { get; set; }

    /// <summary>
    /// Accetta valori multipli
    /// </summary>
    public bool Tag { get; set; }

    /// <summary>
    /// Personalizzabile dall'utente
    /// </summary>
    public bool Customized { get; set; } = true;

    [StringLength(64)]
    public string LastUpdateUser { get; set; }

    public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    [StringLength(64)]
    public string CreationUser { get; set; }

    public string CustomProperties { get; set; }

    [StringLength(10)]
    public string ColumnWidth { get; set; } = "160px";

    /// <summary>
    /// Utilizzabile nelle ricerche
    /// </summary>
    public bool Searchable { get; set; }
    /// <summary>
    /// Dato cifrato sul database
    /// </summary>
    public bool Encrypted { get; set; }

    /// <summary>
    /// Visibile
    /// </summary>
    //public bool Visible { get; set; }

    /// <summary>
    /// Tipo di controllo web 
    /// </summary>
    [StringLength(10)]
    public string ControlType { get; set; } = "textbox";


    public FieldType()
    {
        Id = Guid.NewGuid().ToString();
        Name = "";
        CategoryId = "";
        CreationDate = DateTime.UtcNow;
        LastUpdate = DateTime.UtcNow;
    }


}