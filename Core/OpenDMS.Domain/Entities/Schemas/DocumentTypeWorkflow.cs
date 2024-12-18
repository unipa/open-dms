using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;


namespace OpenDMS.Domain.Entities.Schemas
{
    [PrimaryKey(nameof(DocumentTypeId), nameof(EventName))]
    public class DocumentTypeWorkflow
    {
        /// <summary>
        /// Tipo documentale a cui associare un processo
        /// </summary>
        public string DocumentTypeId { get; set; }

        /// <summary>
        /// Evento che innesca il processo
        /// </summary>
        public string EventName { get; set; }

        ///// <summary>
        ///// Id del documento che contiene il modello di processo da avviare
        ///// </summary>
        //public int ProcessId { get; set; }


        /// <summary>
        /// Id Univoco del documento che contiene il modello di processo da avviare
        /// </summary>
        public string ProcessKey { get; set; }

        //[JsonIgnore]
        //public virtual DocumentType DocumentType { get; set; }
    }
}
