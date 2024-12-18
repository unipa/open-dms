using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Domain.Entities.Settings
{
    public class ApplicationRegistry
    {
        /// <summary>
        /// Identificativo univoco dell'applicazione
        /// </summary>
        [StringLength(64)]
        public string Id { get; set; }

        /// <summary>
        /// Nome dell'applicativo
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Icona dell'applicativo
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Elenco, separato da virgole, di permessi richiesti dall'applicazione
        /// </summary>
        public string Permissions { get; set; }

        /// <summary>
        /// Client Secret per l'accesso al registro delle applicazioni
        /// </summary>
        [StringLength(255)]
        public string ClientSecret { get; set; }

        public bool Disabled { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
