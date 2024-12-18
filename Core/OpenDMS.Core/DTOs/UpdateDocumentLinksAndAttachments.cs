namespace OpenDMS.Core.DTOs
{
    public class UpdateDocumentLinksAndAttachments
    {
        public int DocumentId { get; set; }
        public int[] Attachments { get; set; }
        public int[] LinkTo { get; set; }
    }
}
