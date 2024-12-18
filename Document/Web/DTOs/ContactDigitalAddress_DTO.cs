using OpenDMS.Domain.Enumerators;
using System;
using System.ComponentModel.DataAnnotations;

namespace Web.DTOs
{
    public class ContactDigitalAddress_DTO
    {
        public int Id { get; set; }

        [StringLength(64)]
        public string ContactId { get; set; }

        public DigitalAddressType DigitalAddressType { get; set; }

        /// <summary>
        /// Nome del contatto
        /// </summary>
        [StringLength(128)]
        public string Name { get; set; }

        /// <summary>
        /// Nome di ricerca (normalizzato) del contatto
        /// </summary>
        [StringLength(128)]
        public string SearchName { get; set; }

        /// <summary>
        /// Indirizzo (es. mail)
        /// </summary>
        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(64)]
        public string CreationUser { get; set; }

        [StringLength(64)]
        public string LastUpdateUser { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

        public bool Deleted { get; set; }

        public bool Validated { get; set; } = false;

        public bool EnableDownload { get; set; } = false;

    }
}