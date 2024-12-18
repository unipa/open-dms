using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Core.DTOs
{
    public class ProtocolInfo
    {
        public string Number { get; set; }
        //public string Description { get; set; }
        //public LookupTable Channel { get; set; } = new LookupTable();
        //public LookupTable Register { get; set; } = new LookupTable();
        //public LookupTable Inventory { get; set; } = new LookupTable();
        //public string Direction { get; set; }
        //public int InventoryId { get; set; }
        //public int ImageId { get; set; }
        public DateTime Date { get; set; }
        public string Register { get; set; } = "";
        //public LookupTable EmergencySession { get; set; } = new LookupTable();
        //public string EmergencyNumber { get; set; }
        //public string SenderProtocol { get; set; }
        //public string ProtocolReference { get; set; }
        //public DateTime? SenderDate { get; set; }
        public string ProtocolUser { get; set; }
        //public string Signer { get; set; }
        public string ExternalProtocolURL { get; set; }
        //public ProfileInfo Sender { get; set; }

        //public List<ProfileInfo> To { get; set; } = new List<ProfileInfo>();
        //public List<ProfileInfo> CC { get; set; } = new List<ProfileInfo>();
        //public List<ProfileInfo> CCr { get; set; } = new List<ProfileInfo>();
       
        //public string TitularyId { get; set; } = "";
        //public int TitularyYear { get; set; }
        //public string TitularyDescription { get; set; } = "";

    }
}
