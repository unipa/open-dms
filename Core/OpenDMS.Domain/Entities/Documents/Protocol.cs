using OpenDMS.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Documents;


public partial class Document
{

//    public int ProtocolNumber { get; set; }

    [StringLength(128)]
    public string ProtocolNumber { get; set; }

    public int ProtocolImageId { get; set; }

    [StringLength(64)]
    public string ExternalProtocolUid { get; set; } = "";

    public string ExternalProtocolURL { get; set; } = "";

    public DateTime? ProtocolDate { get; set; }

    public string ProtocolCustomProperties { get; set; } = "";
    public string ProtocolUser { get; set; } = "";

    public JobStatus ProtocolStatus { get; set; }


}


