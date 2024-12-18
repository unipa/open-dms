using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Documents
{

    [Index(nameof(DocumentId), IsUnique = false)]
    [Index(nameof(ProcessKey), IsUnique = false)]
    [Index(nameof(ProcessDefinitionId), IsUnique = false)]
    [Index(nameof(ProcessInstanceId), IsUnique = false)]
    public class ProcessInstance
    {
        public int Id { get; set; }

        /// <summary>
        /// ID del documento che ha avviato il processo o che è stato creato a seguito di un processo manuale
        /// </summary>
        public int DocumentId { get; set; }

        /// <summary>
        ///  Id del documento che contiene lo schema di definizione del processo
        /// </summary>
        public int ProcessDefinitionId { get; set; }
        /// <summary>
        /// Id della versione di processo avviata
        /// </summary>
        public int ProcessImageId { get; set; }

        /// <summary>
        /// ID dell'istanza di processo gestita dal motore di workflow
        /// </summary>
        [StringLength(255)]
        public string ProcessInstanceId { get; set; }

        /// <summary>
        /// ID dell'engine che gestisce il processo (nel caso di future implementazioni di engine multipli)
        /// </summary>
        [StringLength(64)]
        public string ProcessEngineId { get; set; }

        /// <summary>
        /// Id univoco del processo avviato (ExternalId)
        /// </summary>
        [StringLength(255)]
        public string ProcessKey { get; set; }
        /// <summary>
        /// Id dell'evento che ha avviato il processo
        /// </summary>
        [StringLength(64)]
        public string EventName { get; set; }
        [StringLength(64)]
        public string StartUser { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? StopDate { get; set; }

    }
}
