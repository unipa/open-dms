using OpenDMS.Domain.Entities.Documents;
using System.Text.Json.Serialization;


namespace OpenDMS.Domain.Entities.Tasks;

public class TaskAttachment
{
    public int Id { get; set; }
    public int TaskItemId { get; set; }

    //[StringLength(255)]
    //public string TenantId { get; set; }

    public int DocumentId { get; set; }

    // Id di un documento collegato al task, ma non coinvolto nell'attività
    public bool IsLinked { get; set; } = false;

    [JsonIgnore]
    public virtual TaskItem TaskItem { get; set; }
    [JsonIgnore]
    public virtual Document Document { get; set; }
}
