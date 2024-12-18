using OpenDMS.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;

namespace Web.DTOs
{

    public enum SignTypes
    {
        Cades = 0,
        Pades = 1,
        Xades = 2
    }

    public enum SignRoomStatus
    {

        Created = 0,
        FileUploaded = 1,
        ReadyToSign = 2,
        Signing = 3,
        Signed = 4,
        Failed = 5,
        Aborted = 6,
        Ignored = 7,
        NotNeeded = 8,
        Completed = 9
    }

    public class SignRoom_DTO
    {
        public string SignRoom { get; set; }
        [StringLength(64)]
        public string? UserName { get; set; }
        public int? NumeroFile { get; set; }
        public DateTime CreationDate { get; set; }
        public SignRoomStatus Status { get; set; }
        public SignTypes SignType { get; set; }
        [StringLength(255)]
        public bool Delivered { get; set; } = false;
        public DateTime? DeliveryDate { get; set; }
        public string? DeliveryResult { get; set; }



    }
}
