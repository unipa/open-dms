namespace OpenDMS.Core.DTOs
{
    public class FileContent
    {
        public string FileName { get; set; }
        //public byte[] FileData { get; set; } = null;
        public string FileData { get; set; } = null;
        /// <summary>
        /// Indica che il file non va copiato all'interno del sistema, ma va mantenuto un link al file originale
        /// </summary>
        public bool LinkToContent { get; set; }
        public bool DataIsInBase64 { get; set; } = true;
        public bool ExtractAttachment { get; set; } = false;
    }
}
