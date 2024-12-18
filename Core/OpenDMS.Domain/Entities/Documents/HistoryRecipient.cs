using OpenDMS.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Documents;

public class HistoryRecipient
{
    public int Id { get; set; }
    public int EntryId { get; set; }
    [StringLength(64)]
    public string ProfileId { get; set; }
    public ProfileType ProfileType { get; set; }
    public bool CC { get; set; }
    public virtual HistoryEntry Entry { get; set; }
}
