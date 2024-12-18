
using Newtonsoft.Json;

namespace OpenDMS.Domain.Entities.Documents;

public class HistoryDocument
{
    public int Id { get; set; }

    public int EntryId { get; set; }
    public int DocumentId { get; set; }
    public int ImageId { get; set; }

    [JsonIgnore]
    public virtual HistoryEntry Entry { get; set; }
}
