using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RemoteSignInfocert.Models
{
    [PrimaryKey("SignRoom")]
    public class SignRoomModel
    {

        [StringLength(255)]

        public string SignRoom { get; set; }
        [StringLength(64)]
        public string? UserName { get; set; }
        public int? NumeroFile { get; set; }
        public DateTime CreationDate { get; set; }
        public SignRoomStatus Status { get; set; }
        public SignTypes SignType { get; set; }
        [StringLength(255)]
        public string? StatusComment { get; set; }

        // Delivered: flag usato per indicare che la firma è stata consegnata
        // o in generale se la sessione è chiusa,
        // per vedere l'esito riferirisi a DeliveryResult
        public bool Delivered { get; set; } = false;
        public DateTime? DeliveryDate { get; set; }
        public string? DeliveryResult { get; set; }
        public String? ReturnURL { get; set; }
        public String? AbortURL { get; set; }
        public String? SignRoomURL { get; set; }

        public static string GetDescription(SignRoomStatus jobStatus)
        {
            var result = "";
            switch ((int)jobStatus)
            {
                case 0:
                    result = "Creata";
                    break;
                case 1:
                    result = "File caricato";
                    break;
                case 2:
                    result = "Pronta per la firma";
                    break;
                case 3:
                    result = "in fase di firma";
                    break;
                case 4:
                    result = "In consegna";
                    break;
                case 5:
                    result = "Fallita";
                    break;
                case 6:
                    result = "Abortita";
                    break;
                case 7:
                    result = "Ignorata";
                    break;
                case 8:
                    result = "Non necessaria";
                    break;
                case 9:
                    result = "Completata";
                    break;

                default:
                    result = "Non pervenuto";
                    break;
            }
            return result;
        }
    }

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
}
