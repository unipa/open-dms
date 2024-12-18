using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Domain.Entities.Settings
{

    [Index(nameof(ObjectId), nameof(RecordId), IsUnique = true)]
    public class DistributedLock
    {
        public int Id { get; set; }

        /// <summary>
        /// Identificativo dell'oggetto/tabella da gestire
        /// </summary>
        public string ObjectId { get; set; }

        /// <summary>
        /// Identificativo del Record da elaborare
        /// </summary>
        public string RecordId { get; set; }

        /// <summary>
        /// Identificativo del servizio che prende in carico il lavoro
        /// </summary>
        public string ServiceId { get; set; }

        /// <summary>
        /// Data di avvio elaborazione
        /// </summary>
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Data di scadenza oltre la quale il lavoro è considerato bloccato
        /// </summary>
        public DateTime ExpirationDate { get; set; }

    }
}
