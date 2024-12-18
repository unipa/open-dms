using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Enumerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Core.DTOs
{
    public class DigitalAddress
    {
        public int Id { get; set; }

        /// <summary>
        /// Codice utente proprietario dell'indirizzo
        /// </summary>
        public string OwnerId { get; set; }
        public string? ContactId { get; set; }

        public DigitalAddressType AddressType { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

        public bool Deleted { get; set; }
        public LookupTable Company { get; set; }

    }
}