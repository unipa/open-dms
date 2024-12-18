using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Core.DTOs
{
    public class CreateNewTaskMessage
    {
        /// <summary>
        /// Identificativo del task padre. 0=Nessun padre.
        /// </summary>
        public int ParentTaskId { get; set; }

        /// <summary>
        /// Utente richiedente del task nel formato "<profileType><ProfileId>". 
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Titolo dell'attività richiesta (obbligatorio)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Descrizione aggiuntiva del task
        /// </summary>
        public string Description { get; set; }
        public int CompanyId { get; set; }

        /// <summary>
        /// Indice di priorità del ll'attività da decodificare nella tabella "$TASK.PRIORITIES$"
        /// </summary>
        public string Priority { get; set; } = "";

        /// <summary>
        /// Elenco di utenti/gruppi/ruoli destinatari del task, nel formato  "<profileType><ProfileId>".
        /// </summary>
        public List<string> NotifyTo { get; set; } = new List<string>();

        /// <summary>
        /// Elenco di utenti/gruppi/ruoli destinatari per conoscenza del task, nel formato  "<profileType><ProfileId>".
        /// </summary>
        public List<string> NotifyCC { get; set; } = new List<string>();

        /// <summary>
        /// Elenco di allegati archiviati da collegare al task
        /// </summary>
        public List<int> Attachments { get; set; } = new List<int>();

        /// <summary>
        /// Elenco di correlati archiviati da collegare al task
        /// </summary>
        public List<int> Links { get; set; } = new List<int>();


        // EVOLUZIONI FUTURE
        /// <summary>
        /// Identificativo del fascicolo (o cartella) che rappresenta un progetto o un obiettivo al quale il task fa riferimento.
        /// </summary>
        public int ProjectId { get; set; }

        //// EVOLUZIONI FUTURE
        ///// <summary>
        ///// Indica se il Task deve essere assegnato a tutti gli utenti
        ///// </summary>
        public bool AssignToAllUsers { get; set; } = false;


        /// <summary>
        /// Identificativo del documento/fascicolo che contiene i dati di processo
        /// </summary>
        public int ProcessDataId { get; set; } = 0;
        public string ProcessInstanceId { get; set; } = "";
        public string JobInstanceId { get; set; } = "";

        public string ProcessDefinitionKey { get; set; } = "";

        public string MessageTemplate { get; set; }

        public int Duration { get; set; } = 0;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


    }
}
