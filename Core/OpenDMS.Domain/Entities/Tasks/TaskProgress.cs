using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenDMS.Domain.Entities.Tasks;

public class TaskProgress
{
    public int Id { get; set; }

    public int? UserTaskId { get; set; }
    public int TaskItemId { get; set; }

    [StringLength(64)]
    public string UserId { get; set; }

    public string Message { get; set; }


    [Precision(6, 2)]
    public decimal Percentage { get; set; }


    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public virtual TaskItem TaskItem { get; set; }
    [JsonIgnore]
    public virtual UserTask UserTask { get; set; }
}
