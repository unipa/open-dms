using OpenDMS.Domain.Events.Types;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Documents;

public class HistoryEntry
{
    public int Id { get; set; }
    [StringLength(64)]
    public string UserId { get; set; }
    [StringLength(64)]
    public string DeputyUserId { get; set; }
    [StringLength(255)]
    public string Description { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    public string Details { get; set; } = "";

    [StringLength(64)]
    public string EventType { get; set; }

    public virtual IList<HistoryRecipient> Recipients { get; set; } = new List<HistoryRecipient>();

    [StringLength(64)]
    public string WorkflowId { get; set; }
    [StringLength(64)]
    public string TaskId { get; set; }
    public virtual List<HistoryDocument> Documents { get; set; } = new List<HistoryDocument>();
}
