namespace OpenDMS.DocumentManager.API.DTOs
{
    public class CheckOutContent
    {
        /// <summary>
        /// Nome del file archiviato
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Estensione (in minuscolo) del file archiviato
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// contenuto del documento (testuale o base64)
        /// </summary>
        public string Content { get; set; }
        public int VersionNumber { get; set; }
        public int RevisionNumber { get; set; }
        public bool Published { get; set; }
        public DateTime CreationDate { get; set; }
        public string LastUpdateUser { get; set; }

    }
}
