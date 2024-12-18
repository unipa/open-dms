using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Core.DTOs;


public class DocumentInfo
{
    public int Id { get; set; } = 0;
    public int MasterDocumentId { get; set; } = 0;
    public int? FolderId { get; set; } = 0;

    public string ExternalId { get; set; } = "";

    public DocumentStatus DocumentStatus { get; set; }
    public ContentType ContentType { get; set; }

    public string IconColor { get; set; } = "";
    public string Icon { get; set; } = "";
    public string Description { get; set; } = "";
    public string DocumentTypeName { get; set; } = "";
    public string DocumentTypeCategory { get; set; } = "";

    public string DocumentNumber { get; set; } = "";
    public string DocumentNumberFormattedValue { get; set; } = "";
    public string DocumentNumberLookupValue { get; set; } = "";
    public FieldType DocumentNumberFieldType { get; set; } = null;

    public string Owner { get; set; } = "";
    public DateTime? DocumentDate { get; set; }
    public string DocumentISODate { 
        get {
            return DocumentDate?.ToString("yyyy-MM-dd") ?? "";
        } 
        set {
            DocumentDate =String.IsNullOrEmpty(value) ? DocumentDate = null : DateTime.ParseExact(value, "yyyy-MM-dd", null);
        } 
    }
    public DateTime? ExpirationDate { get; set; }
    public string ExpirationISODate
    {
        get
        {
            return ExpirationDate?.ToString("yyyy-MM-dd") ?? "";
        }
        set
        {
            try
            {
                ExpirationDate = String.IsNullOrEmpty(value) ? ExpirationDate = null :  DateTime.ParseExact(value, "yyyy-MM-dd", null);
            }
            catch {
                ExpirationDate = null;
            }
        }
    }

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public string CreationISODate
    {
        get
        {
            return CreationDate.ToString("yyyy-MM-ddTHH:mm:ss") ?? "";
        }
        set
        {
            CreationDate = DateTime.ParseExact(value, "yyyy-MM-ddTHH:mm:ss", null);
        }
    }

    public DateTime? DeletionDate { get; set; }
    public string DeletionISODate
    {
        get
        {
            return DeletionDate?.ToString("yyyy-MM-ddTHH:mm:ss") ?? "";
        }
        set
        {
            if (!String.IsNullOrEmpty(value)) DeletionDate = DateTime.ParseExact(value, "yyyy-MM-ddTHH:mm:ss", null);
            else
                DeletionDate = null;
        }
    }

    public DateTime? LastUpdate { get; set; }
    public string LastUpdateUser { get; set; } = "";
    public DateTime? ConsolidationDate { get; set; }
    public string ConsolidationISODate
    {
        get
        {
            return ConsolidationDate?.ToString("yyyy-MM-ddTHH:mm:ss") ?? "";
        }
        set
        {
            if (!String.IsNullOrEmpty(value)) ConsolidationDate = DateTime.ParseExact(value, "yyyy-MM-ddTHH:mm:ss", null);
            else ConsolidationDate = null;
        }
    }

    public string? ACLId { get; set; } = "";
    public bool Reserved { get; set; }
    public bool PersonalData { get; set; }

    public int ExpirationTolerance { get; set; }

    public Company Company { get; set; } = new Company();
    public List<LookupTable> Path { get; set; } = new List<LookupTable>();
    public DocumentType DocumentType { get; set; } = new DocumentType();
    public List<DocumentFieldInfo> FieldList { get; set; } = new List<DocumentFieldInfo>();
    public DocumentImage Image { get; set; } = new DocumentImage();
    public List<LookupTable> ReferentList { get; set; } = new List<LookupTable>();
    public List<LookupTable> ReferentListCC { get; set; } = new List<LookupTable>();
    public List<DocumentRecipientInfo> RecipientList { get; set; } = new List<DocumentRecipientInfo>();
    public List<DocumentFieldInfo> ContactList { get; set; } = new List<DocumentFieldInfo>();

    public ProtocolInfo Protocol { get; set; } = new ProtocolInfo();

}
