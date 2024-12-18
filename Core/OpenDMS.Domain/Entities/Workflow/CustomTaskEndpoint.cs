using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Workflow
{
    public  class CustomTaskEndpoint
    {
        /// <summary>
        /// ID del servizio interfacciato
        /// </summary>
        [StringLength(64)]
        public string Id { get; set; } = "";

        /// <summary>
        /// URL originale del servizio interfacciato
        /// </summary>
        [Key]
        [StringLength(255)]
        public string Endpoint { get; set; } = "";

        /// <summary>
        /// Nome del servizio
        /// </summary>
        [StringLength(64)]
        public string Name { get; set; } = "";

        /// <summary>
        /// Descrizione del servizio
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// Elenco di task associati
        /// </summary>
        public string Tasks { get; set; } = "";

        /// <summary>
        /// Contentuo del file swagger associato
        /// </summary>
        public string EndPointDescriptor { get; set; } = "";

        public CustomTaskEndpointType CustomTaskEndpointType { get; set; }

        public DateTime CreationDate { get; set; }
        public bool Deleted { get; set; }
        public DateTime DeletionDate { get; set; }
        public DateTime LastUpdate { get; set; }


    }
}
