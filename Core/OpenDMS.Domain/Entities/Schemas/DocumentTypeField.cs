using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenDMS.Domain.Entities.Schemas;


[Index(nameof(DocumentTypeId), nameof(FieldName), IsUnique = true)]
public class DocumentTypeField
{
    [StringLength(64)]
    public string Id { get; set; }

    [StringLength(64)]
    public string DocumentTypeId { get; set; }

    [StringLength(64)]
    public string FieldName { get; set; } = Guid.NewGuid().ToString();

    public int FieldIndex { get; set; }

    [StringLength(64)]
    public string FieldTypeId { get; set; }

    [StringLength(255)]
    public string DefaultValue { get; set; }

    [StringLength(128)]
    public string Title { get; set; }

    [StringLength(6)]
    public string Width { get; set; }

    public bool Mandatory { get; set; }

    public bool Editable { get; set; }

    public bool Tag { get; set; }

    public bool Preservable { get; set; }
    public bool Deleted { get; set; }

    [StringLength(64)]
    public string LastUpdateUser { get; set; }

    public virtual FieldType FieldType { get; set; }

    [JsonIgnore]
    public virtual DocumentType DocumentType { get; set; }



    /* OPENDMS */
    public bool Encrypted { get; set; }

    //public bool Inherited { get; set; }

    //public bool FilterInMailing { get; set; }

    //public bool KeyField { get; set; }

    public int Visibility { get; set; } = 0; // 0=publica, 1=privata/interna al documento, 2=Workflow



}

