using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Enumerators;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace OpenDMS.Domain.Entities.Tasks;

public class UserTask
{
    public int Id { get; set; }

    public int TaskItemId { get; set; }

    /// <summary>
    /// Id della struttura di riferimento 
    /// </summary>
    [StringLength(64)]
    public string GroupId { get; set; }
    /// <summary>
    /// Id del ruolo di riferimento 
    /// </summary>
    [StringLength(64)]
    public string RoleId { get; set; }

    /// <summary>
    /// Id dell'utente assegnatario
    /// </summary>
    [StringLength(64)]
    public string UserId { get; set; }

    /// <summary>
    /// Indica che il Task è stato letto
    /// </summary>
    public bool Read { get; set; }

    /// <summary>
    /// True se il task è stato inviato per conoscenza
    /// </summary>
    public bool CC { get; set; }
    public ExecutionStatus Status { get; set; }

    /// <summary>
    /// Id del manager che ha validato l'attività
    /// </summary>
    [StringLength(64)]
    public string ManagerId { get; set; }

    /// <summary>
    /// Percentuale di completamento del task
    /// </summary>
    [Precision(6, 2)]
    public decimal Percentage { get; set; }
    public NotificationType NotificationType { get; set; }
    public DateTime? NotificationDate { get; set; }



    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime? ClaimDate { get; set; }
    public DateTime? FirstExecutionDate { get; set; }
    public DateTime? LastExecutionDate { get; set; }
    public DateTime? ValidationDate { get; set; }
    public DateTime? ExpirationDate { get; set; }

    /// <summary>
    /// Variabili di input/output dell'eventuale form associato al task
    /// Le variabili sono 
    /// </summary>
    public virtual string Variables { get; set; }

    [JsonIgnore]
    public virtual TaskItem TaskItem { get; set; }

 //   public virtual List<TaskItem> SubTaskItems { get; set; }

}
