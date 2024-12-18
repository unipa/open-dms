using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;

namespace OpenDMS.Core.DTOs;

public partial class CreateOrUpdateDocument
{
    public int CompanyId { get; set; } = 1;

    public string ExternalId { get; set; } = "";
    public ContentType ContentType { get; set; } = ContentType.Document;
    public string? DocumentTypeId { get; set; }
    public string? Owner { get; set; }
    public string? ACLId { get; set; }
    public int? MasterDocumentId { get; set; } = 0;
    public DocumentStatus Status { get; set; } = 0;

    public string IconColor { get; set; } = "";
    public string Icon { get; set; } = "";
    public string Description { get; set; } = "";
    public string DocumentNumber { get; set; } = "";
    public DateTime? DocumentDate { get; set; } = null;
    public DateTime? ExpirationDate { get; set; }
    public int? FolderId { get; set; } = 0;

    /// <summary>
    /// Elenco di profili nel formato <ProfileType><ProfileId>
    /// </summary>
    public string ReferentList { get; set; } = "";
    /// <summary>
    /// Elenco di profili nel formato <ProfileType><ProfileId>
    /// </summary>
    public string ReferentListCC { get; set; } = "";

    public bool Reserved { get; set; } = false;
    public bool PersonalData { get; set; } = false;

    public List<AddOrUpdateDocumentField> FieldList { get; set; } = new List<AddOrUpdateDocumentField>();

    /// <summary>
    /// Elenco di profili nel formato <ProfileType><ProfileId>
    /// </summary>
    public string NotifyTo { get; set; } = "";
    /// <summary>
    /// Elenco di profili nel formato <ProfileType><ProfileId>
    /// </summary>
    public string NotifyCC { get; set; } = "";


    /// <summary>
    /// Elenco di profili nel formato <ProfileType><ProfileId>
    /// es. 0admin,2admin,1user
    /// </summary>
    public string Authorize { get; set; } = "";

    /// <summary>
    /// Elenco di id documenti
    /// </summary>
    public int[] AttachTo { get; set; } = null;

    /// <summary>
    /// Elenco di id documenti
    /// </summary>
    public int[] Attachments { get; set; } = null;

    /// <summary>
    /// Elenco di id documenti
    /// </summary>
    public int[] LinkTo { get; set; } = null;


    public FileContent Content { get; set; } = null;

    public Dictionary<string, object> ProcessVariables { get; set; } = null;

    public bool FailIfExists { get; set; } = false;


    public string NotificationTemplate { get; set; } = "";

}



