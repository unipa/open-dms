using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Domain.Entities.Workflow
{
    public class CustomTaskItem
    {
        [StringLength(64)]
        public string Id { get; set; }

        [StringLength(64)]
        public string GroupId { get; set; }

        [StringLength(64)]
        public string EndpointId { get; set; }



        /// <summary>
        /// Nome visualizzato del task
        /// </summary>
        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(128)]
        public string Label { get; set; }

        /// <summary>
        /// Descrizione del task
        /// </summary>
        public string Description { get; set; }

        [StringLength(64)]
        public string Icon { get; set; }
        [StringLength(64)]
        public string Color { get; set; }

        public int AuthenticationType { get; set; }


        /// <summary>
        ///  Identificativo del jobWorker che gestirà questo task
        /// </summary>
        [StringLength(255)]
        public string JobWorker { get; set; }
        /// <summary>
        /// Elenco di variabili di input in formato json
        /// </summary>
        public string InputVariables { get; set; }

        /// <summary>
        /// Elenco di variabili di output in formato json
        /// </summary>
        public string OutputVariables { get; set; }


        public DateTime CreationDate { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletionDate { get; set; }

        [JsonIgnore]
        public virtual CustomTaskGroup TaskGroup { get; set; }

    }
}
