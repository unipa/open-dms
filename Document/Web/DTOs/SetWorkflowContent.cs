namespace Web.DTOs
{
    public class CheckInContent
    {
        /// <summary>
        /// Testo formattato (json, xml, ...) del contenuto del documento workflow
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// testo in formato base64 della preview del documento
        /// </summary>
        public string PreviewInBase64 { get; set; }

        /// <summary>
        /// estensione della preview (es. png, jpg, pdf)
        /// </summary>
        public string PreviewExtension { get; set; }

        /// <summary>
        /// true = documento da pubblicare
        /// </summary>
        public bool ToBePublished { get; set; }
    }
}
