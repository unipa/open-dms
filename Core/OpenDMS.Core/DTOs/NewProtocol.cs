using OpenDMS.Domain.Models;

namespace OpenDMS.Core.DTOs
{
    public class NewProtocol
    {
        public string Description { get; set; }
        public string ChannelId { get; set; } = "";
        public string Register { get; set; }
        public string Inventory { get; set; }
        public string Direction { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string EmergencySession { get; set; }
        public int EmergencyNumber { get; set; }
        public string SenderProtocol { get; set; }
        public string ProtocolReference { get; set; }
        public DateTime? SenderDate { get; set; }
        public string Signer { get; set; }
        public ProfileInfo Sender { get; set; }
        public List<ProfileInfo> To { get; set; } = new List<ProfileInfo>();
        public List<ProfileInfo> CC { get; set; } = new List<ProfileInfo>();
        public List<ProfileInfo> CCr { get; set; } = new List<ProfileInfo>();

        public string TitularyId { get; set; } = "";
        public int TitularyYear { get; set; }
        public string TitularyDescription { get; set; } = "";

    }
}
