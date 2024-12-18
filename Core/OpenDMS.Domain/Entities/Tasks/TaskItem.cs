using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Tasks;



/// <summary>
/// Definisce il task principale dal quale derivano sotto-task assegnati a singoli utenti/ruoli/gruppi
/// </summary>
public class TaskItem
{
    public int Id { get; set; }

    /// <summary>
    /// Identificativo dello UserTask padre (task utente)
    /// </summary>
    public int ParentId { get; set; }

    public int CompanyId { get; set; } = 0;

    /// <summary>
    /// Id dell'utente richiedente
    /// </summary>
    [StringLength(64)]
    public string FromUserId { get; set; } = "";

    /// <summary>
    /// Indica se si tratta di un messaggio, di una attività legata ad un form o ad un evento
    /// </summary>
    public TaskType TaskType { get; set; } = TaskType.Message;

    /// <summary>
    /// Associa un categoria al task affinchè l'utente possa raggrupparli
    /// </summary>
    [StringLength(64)]
    public string CategoryId { get; set; } = "";

    [StringLength(64)]
    public string PriorityId { get; set; } = "";

    /// <summary>
    /// Identificativo univoco del form da utilizzare sul task (anche per le approvazioni semplici)
    /// </summary>
    public string FormKey { get; set; } = "";

    /// <summary>
    /// Variabili del form
    /// </summary>
    //public string FormSchema { get; set; } = "";
    public string Variables { get; set; } = "";

    /// <summary>
    /// Identificativo degli eventi che completano l'attività (es. firma digitale, firma biometrica, ...)
    /// </summary>
    [StringLength(64)]
    public string EventId { get; set; } = "";


    /// <summary>
    /// Notifica al mittente il completamento di un task
    /// </summary>
    public bool NotifyExecution { get; set; } = false;

    /// <summary>
    /// Notifica al mittente la scadenza di un task
    /// </summary>
    public bool NotifyExpiration { get; set; } = false;


    [StringLength(255)]
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";

    /// <summary>
    /// Nome del template utilizzato per inviare il messaggio di posta o la notifica sull'app
    /// </summary>
    [StringLength(255)]
    public string MessageTemplate { get; set; } = "";

    [Precision(6, 2)]
    public decimal ExecutionPercentage { get; set; } = 0;
    public ExecutionStatus Status { get; set; } = ExecutionStatus.Unassigned;



    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime? ExecutionDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public DateTime? ValidationDate { get; set; }
    public DateTime LastUpdate { get; set; } = DateTime.UtcNow;


    /// <summary>
    /// ID dell'istanza del diagramma associato al processo
    /// </summary>
    public int ProcessDefinitionId { get; set; } = 0;

    public int ProcessImageId { get; set; } = 0;



    /// <summary>
    /// ID della definizione di processo.
    /// Da non confondere con l'id del documento che definisce il processo
    /// </summary>
    [StringLength(255)]
    public string ProcessDefinitionKey { get; set; } = "";

    /// <summary>
    /// ID dell'istanza del job che ha gestito il task
    /// </summary>
    [StringLength(64)]
    public string JobInstanceId { get; set; } = "";

    /// <summary>
    /// Id dell'istanza di processo in cui insiste il task
    /// </summary>
    [StringLength(64)]
    public string ProcessInstanceId { get; set; } = "";

    /// <summary>
    /// Identificativo del documento/fascicolo che contiene i dati di processo
    /// </summary>
    public int ProcessDataId { get; set; } = 0;

    //    public virtual List<TaskVariable> Variables { get; set; }
    public virtual List<TaskRecipient> Recipients { get; set; }
    public virtual List<TaskAttachment> Attachments { get; set; }
    public virtual List<UserTask> UserTasks { get; set; }
    public virtual List<TaskProgress> Progress { get; set; }

    /// <summary>
    /// Identificativo del fascicolo/cartella di progetto a cui fa riferimento il task
    /// </summary>
    public int ProjectId { get; set; }

    // durata dell'evento
    [Precision(6, 2)]
    public decimal Duration { get; set; } = 0;

    // Data di inizio e fine evento
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; } = DateTime.UtcNow;




}
