namespace OpenDMS.Core.DTOs
{
    public class FolderExportModel
    {
        public DocumentInfo DocumentInfo { get; set; }
        public FileContent? Content { get; set; }
        public List<ProfilePermissions> Permissions { get; set; } = new List<ProfilePermissions>();
        public List<DocumentLink> Links { get; set; } = new List<DocumentLink>();
        public List<DocumentLink> Attachments { get; set; } = new List<DocumentLink>();
        public List<FolderExportModel> SubDocuments { get; set; } = new List<FolderExportModel>();
    }
}
