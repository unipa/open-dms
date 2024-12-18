using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenDMS.Domain.Entities.Settings;

public class Company
{
    public int Id { get; set; } = 0;

    [Required]
    [StringLength(128), MinLength(1)]
    public string Description { get; set; }
    [StringLength(64)]
    public string Theme { get; set; } = "";
    [StringLength(128)]
    public string Logo { get; set; } = "";
    [StringLength(128)]
    public string ERP { get; set; } = "";
    [StringLength(64)]
    public string AOO { get; set; } = "";
    public bool OffLine { get; set; }

    //    [NotMapped]
    //    public string Codice { get { return Id.ToString().PadLeft(3, '0'); }  }

    /// <summary>
    /// Nome identificativo esterno
    /// Utiler per identificare un sistema informativo attraverso un nome/codice esterno
    /// </summary>
    [StringLength(128)]
    public string ExternalReference { get; set; } = "";

    public string RootOrganizationNode { get; set; } = "";

}

