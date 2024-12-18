using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Enumerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Core.DTOs
{
    public class AddOrUpdateContactDigitalAddress
    {
        public string ContactId { get; set; }

        public DigitalAddressType DigitalAddressType { get; set; }

        /// <summary>
        /// Nome del contatto
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Nome di ricerca (normalizzato) del contatto
        /// </summary>
        public string SearchName { get; set; }

        /// <summary>
        /// Indirizzo (es. mail)
        /// </summary>
        public string Address { get; set; }

    }
}
