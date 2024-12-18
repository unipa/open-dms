namespace OpenDMS.Domain.Enumerators;

public enum ExecutionStatus
{
    /// <summary>
    /// Task non assegnato ad un utente
    /// </summary>
    Unassigned = 0,
    /// <summary>
    /// Preso in carico da un utente
    /// </summary>
    Assigned = 1,
    /// <summary>
    /// In lavorazione
    /// </summary>
    Running = 2,
    /// <summary>
    /// Sospeso dall'utente o da un processo
    /// </summary>
    Suspended = 3,
    /// <summary>
    /// Task eseguito e completato dall'utente
    /// </summary>
    Executed = 250,
    /// <summary>
    /// Task verificato da un supervisore
    /// </summary>
    Validated = 251,
    /// <summary>
    /// Task annullato da un processo
    /// </summary>
    Aborted = 254,
    /// <summary>
    /// Task eliminato da un supervisore o da un processo
    /// </summary>
    Deleted = 255
}
