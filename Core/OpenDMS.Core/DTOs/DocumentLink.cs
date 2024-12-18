using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Core.DTOs;

public class DocumentLink
{

    public int Id { get; set; }
    public int VersionNumber { get; set; }
    public int RevisionNumber { get; set; }
    public DocumentStatus Status { get; set; }
    public string Protocol { get; set; }
    public string DocumentType { get; set; }
    public string Description { get; set; }
    public string FileName { get; set; }
    public string FileExtension { get; set; }
    public long FileSize { get; set; }
    public int ImageId { get; set; }
    public DateTime? DocumentDate { get; set; }
    public string DocumentNumber { get; set; }
//        public List<LookupTable> Fields { get; set; } = new List<LookupTable>();

}
