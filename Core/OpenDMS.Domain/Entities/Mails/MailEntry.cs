using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenDMS.Domain.Entities.Mails
{
    public class MailEntry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Identificativo del messaggio comprensivo della cartella IMAP su cui è inserito
        /// </summary>
        [StringLength(255)]
        public string UIDL { get; set; }

        /// <summary>
        /// Identificativo Univoco del Messaggio
        /// </summary>
        [StringLength(255)]
        public string MessageId { get; set; }

        /// <summary>
        /// Identificativo del Client che ha generator il messaggio
        /// </summary>
        [StringLength(255)]
        public string MessageUID { get; set; }

        /// <summary>
        /// Impronta del messaggio
        /// </summary>
        [StringLength(255)]
        public string FileHash { get; set; }

        /// <summary>
        /// Codice del VirtualFileManagerProvider
        /// </summary>
        [StringLength(64)]
        public string FileManager { get; set; }

        /// <summary>
        /// Percorso di memorizzazione del source.eml
        /// </summary>
        [StringLength(255)]
        public string FilePath { get; set; }

        /// <summary>
        /// Tipo di messaggio (Mail/PEC)
        /// </summary>
        public MailType MailType { get; set; }

        public MailSubType SubType { get; set; }

        public MailStatus Status { get; set; }

        /// <summary>
        /// Identificativo del mailserver associato
        /// </summary>
        public int MailServerId { get; set; }

        public MailDirection Direction { get; set; }

        /// <summary>
        /// Indirizzo del mittente (in uscita) o del destinatario (casella monitorata)
        /// </summary>
        public string InternalMailAddress { get; set; }

        /// <summary>
        /// Data del messaggio
        /// </summary>
        public DateTime? MessageDate { get; set; }

        /// <summary>
        /// Indirizzo del mittente (in ingresso) o del destinatario (in uscita)
        /// </summary>
        public string ExternalMailAddress { get; set; }

        [StringLength(255)]
        public string MessageTitle { get; set; }

        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Identificativo del messaggio padre
        /// </summary>
        public int? ParentId { get; set; }

        public int NumberOfAttachments { get; set; }

        /// <summary>
        /// Data trasmissione (ricezione o invio)
        /// </summary>
        public DateTime? TransmissionDate { get; set; }

        /// <summary>
        /// Nr.tentativi di invio
        /// </summary>
        public int RetryValue { get; set; }

        public string LastException { get; set; }

        [StringLength(255)]
        public string IMAPFolder { get; set; }
        public bool IsSPAM { get; set; }
        public bool IsInfected { get; set; }

        [StringLength(64)]
        public string WorkerId { get; set; }

        /// <summary>
        /// Data dopo la quale è possibile svuotare WorkerId e riportare lo stato a Queued
        /// </summary>
        public DateTime? LastRunningUpdate { get; set; }

        //public Nullable<DateTime> ClaimDate { get; set; }

        ///// <summary>
        ///// Utente che prende in carico il messaggio
        ///// </summary>
        //[StringLength(64)]
        //public string ClaimUserId { get; set; }

        /// <summary>
        /// Data archiviazione documento
        /// </summary>
        public DateTime? ArchivingDate { get; set; }

        /// <summary>
        /// Data in cui il messaggio viene marcato come da cancellare
        /// </summary>
        public DateTime? DeletionDate { get; set; }

        /// <summary>
        /// Data in cui il messaggio viene realmente cancellato
        /// </summary>
        public DateTime? PurgedDate { get; set; }

        /// <summary>
        /// Identificativo del documento archiviato
        /// </summary>
        public int DocumentId { get; set; }

        public virtual MailEntry Parent { get; set; }

        // V2
        [StringLength(64)]
        public string ProtocolNumber { get; set; }
        [StringLength(255)]
        public string ProtocolURL { get; set; }

        [StringLength(64)]
        public string ClaimUser { get; set; }
        public DateTime? ClaimDate { get; set; }

        // V3
        [StringLength(64)]
        public string DeletionUser { get; set; }
        [StringLength(64)]
        public string ArchivingUser { get; set; }

        public int MailboxId { get; set; }

        /// <summary>
        /// Indirizzo dei destinatari CC
        /// </summary>
        public string ExternalMailAddressCC { get; set; }

        /// <summary>
        /// Indirizzo dei destinatari CCr
        /// </summary>
        public string ExternalMailAddressCCr { get; set; }

        public bool LinkAttachments { get; set; }
        public bool IncludePDFPreview { get; set; }

        public int ImageId { get; set; }

    }
}
