using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Core.DTOs;


public class DocumentRecipientInfo
{
    public int Id { get; set; } = 0;
    public int DocumentId { get; set; } = 0;
    public ProfileType ProfileType { get; set; } = 0;
    public string ProfileId { get; set; } = "";
    public string InitialProfileId { get; set; } = "";
    public string ProfileName { get; set; } = "";
    public DateTime LastUpdate { get; set; }
    public DateTime CreationDate { get; set; }
    public RecipientType RecipientType { get; set; } = RecipientType.Customized;

    public DocumentRecipientInfo()
    {
    }


    public DocumentRecipientInfo(DocumentRecipient R, string Name)
    {
        Id = R.Id;
        DocumentId = R.DocumentId;
        ProfileType = R.ProfileType;
        ProfileId = R.ProfileId;
        RecipientType = R.RecipientType;
        ProfileName = Name;
        InitialProfileId = R.InitialProfileId;
        LastUpdate = R.LastUpdate;
        CreationDate = R.CreationDate;
     }

}


