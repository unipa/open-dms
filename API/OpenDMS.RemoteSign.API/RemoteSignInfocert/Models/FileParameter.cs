using FirmeRemoteLib.Models;

namespace RemoteSignInfocert.Models
{
    public class FileParameter
    {
        public byte[]? Data { get; set; }
        public string? OriginalFileName { get; set; }
        //public string? TempFileName { get; set; }
        //public string? FullTempFileName { get; set; }
        public long? Size { get; set; }

        public FirmaPades? CampoFirma { get; set; }
        public string? UserName { get; set; }
        public SignTypes signType { get; set; }
    }
}
