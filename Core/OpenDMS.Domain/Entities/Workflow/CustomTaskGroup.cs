using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenDMS.Domain.Entities.Workflow
{
    public class CustomTaskGroup
    {
        /// <summary>
        /// Identificativo del gruppo
        /// </summary>
        [StringLength(64)]
        public string Id { get; set; }

        /// <summary>
        /// Identificativo dell'EndPoint
        /// </summary>
        [StringLength(64)]
        public string EndpointId { get; set; }

        [StringLength(128)]
        public string Name { get; set; }
        public string Description { get; set; }


        /// <summary>
        /// True = task custom non cancellabile o modificabile dall'esterno
        /// </summary>
        public bool IsCustom { get; set; }

        public DateTime LastUpdate { get; set; }

        public bool Deleted { get; set; }

        public virtual IList<CustomTaskItem> TaskItems { get; set; }

        [JsonIgnore]
        public CustomTaskEndpoint CustomTaskEndpoint { get; set; }
    }
}
