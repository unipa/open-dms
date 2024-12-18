using OpenDMS.Domain.Enumerators;


namespace OpenDMS.Core.DTOs;

public class DocumentVersion
{

    public int Id { get; set; }
    public int ImageId { get; set; }
    public int VersionNumber { get; set; }
    public int RevisionNumber { get; set; }
    public DateTime CreationDate { get; set; }
    public string Owner { get; set; }

    public string FileName { get; set; }
    public string FileExtension { get; set; }
    public long FileSize { get; set; }
//        public bool Protected { get; set; }
    public JobStatus PreservationStatus { get; set; }
    public JobStatus SignatureStatus { get; set; }
    public JobStatus SendingStatus { get; set; }

}
