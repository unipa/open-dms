using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Core.DTOs
{
    public class CreateNewTask : CreateNewTaskMessage
    {


        /// <summary>
        /// Identificativo della catgoria di appartenenza dell'attività 
        /// Le categorie di appartenenza sono gestire nelle tabelle interne, con in codice tabella "$TASKS.GROUPS$";
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// Tipo di task (notifica, attività generica, attività basata su evento (solo per workflow)
        /// </summary>
        public TaskType TaskType { get; set; } = TaskType.Activity;

        /// <summary>
        /// Data Scadenza
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Indica che il richiedente riceverà una notifica alla conclusione del task
        /// </summary>
        public bool NotifyExecution { get; set; }

        /// <summary>
        /// Indica che il richiedente riceverà una notifica alla scadenza del task
        /// </summary>
        public bool NotifyExpiration { get; set; }

        /// <summary>
        /// Elenco di allegati estemporanei da archiviare e collegare al task
        /// </summary>
        public List<FileContent> NewContent { get; set; } = new List<FileContent>();

        public string FormKey { get; set; } = "";
        //public string? FormSchema { get; set; }
        public string? FormData { get; set; }

        ///// <summary>
        ///// Nome dell'evento che completa il task
        ///// </summary>
        public string? EventId { get; set; }
        ////public string WorkerId { get; set; }


        // public bool AssignToAllUsers { get; set; } = false;
        /// <summary>
        /// Indica se rendere visibili gli allegati del task di provenienza
        /// </summary>
        public bool IncludeParentAttachments { get; set; } = false;

    }
}
