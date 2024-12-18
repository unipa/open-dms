namespace OpenDMS.Domain.Enumerators;

public enum TaskType
{
    /// <summary>
    /// Messaggio o notifica per conoscenza
    /// </summary>
    Message = 0,

    /// <summary>
    /// Attività generica che può prevedere la compilazione di un form e che può essere effettuata in più step
    /// </summary>
    Activity = 1,

    /// <summary>
    /// Attività che prevede il completamento attraverso un evento applicativo (es. sottoscrizione di un documento)
    /// </summary>
    Event = 2,

    /// <summary>
    /// Avviso da attenzionare (Da usare per segnalare una situazione anomala o un imminente problema (es. spazio disco in esaurimento,  reminder scadenze, ...)
    /// </summary>
    Warning = 254,

    /// <summary>
    /// Errore di sistema. Questo tipo di attività viene utilizzata per segnalare un problema (es. impossibile accedere alla posta, errore su wf, ...)
    /// </summary>
    Error = 255
}
