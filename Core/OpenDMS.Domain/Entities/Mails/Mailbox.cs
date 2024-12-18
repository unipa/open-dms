using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenDMS.Domain.Entities.Settings;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Mails
{
    [Index(nameof(MailAddress), IsUnique = true)]
    public class Mailbox
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Indirizzo Email (minuscolo, trimmato, senza nomi e virgolette)
        /// </summary>
        [StringLength(255)]
        public string MailAddress { get; set; }

        /// <summary>
        /// Nome da anteporre all'indirizzo mail
        /// </summary>
        [StringLength(128)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Firma dell'utente da inserire in calce della mail
        /// </summary>
        public string UserSignature { get; set; }

        /// <summary>
        /// Id del mailserver collegato, da recuperare attraverso il dominio dell mail
        /// </summary>
        public int MailServerId { get; set; }

        /// <summary>
        /// Account di accesso alal casella mail 
        /// </summary>
        [StringLength(128)]
        public string Account { get; set; }

        /// <summary>
        ///  Password di accesso 
        /// </summary>
        [StringLength(128)]
        public string Password { get; set; }

        /// <summary>
        /// Token di accesso alla casella per autenticazioni OAUTH.
        /// non va mostrato a video
        /// </summary>
        public string TokenId { get; set; }

        /// <summary>
        /// Refresh Token di accesso alla casella per autenticazioni OAUTH
        /// non va mostrato a video
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// true = creenziali validate. Al cambio delle credenziali va impostato a false
        /// </summary>
        public bool Validated { get; set; }


        /// <summary>
        /// Ultima data di modifica delle credenziali. 
        /// Questo campo viene impostato automaticamente al cambio delle credenziali
        /// </summary>
        public DateTime? LastCredentialUpdate { get; set; }

        /// <summary>
        /// Utente applicativo collegato alla mail
        /// </summary>
        [StringLength(64)]
        public string UserId { get; set; }

        /// <summary>
        /// Company collegata alla mail
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Ultimo errore di invio registrato
        /// </summary>
        public string LastSendingError { get; set; }
        public DateTime? LastSendingDate { get; set; }

        /// <summary>
        /// Ultimo errore di ricezione registrato
        /// </summary>
        public string LastReceivingError { get; set; }
        public DateTime? LastReceivingDate { get; set; }

        /// <summary>
        /// Elenco di profili (utenti/ruoli/gruppi), separati da virgola, abilitati alla lettura della casella
        /// </summary>
        public string ReadOnlyProfiles { get; set; }

        /// <summary>
        /// Elenco di profili (utenti/ruoli/gruppi), separati da virgola, abilitati all'invio dalla casella
        /// </summary>
        public string SendEnabledProfiles { get; set; }

        /// <summary>
        /// Elenco di profili (utenti/ruoli/gruppi) abilitati alla creazione di bozze dalla casella
        /// </summary>
        public string DraftEnabledProfiles { get; set; }

        /// <summary>
        /// true = abilita il download della posta dalla casella
        /// </summary>
        public bool EnableDownload { get; set; }

        /// <summary>
        /// true = cancella i messaggi correttamente archiviati
        /// </summary>
        public bool DeleteDownloadedMail { get; set; }

        /// <summary>
        /// Cartelle IMAP da monitorare
        /// </summary>
        public string DownloadImapFolders { get; set; }

        /// <summary>
        /// Cartella IMAP in cui salvare i messaggi scaricati
        /// </summary>
        public string SaveToImapFolder { get; set; }

        /// <summary>
        /// Data dopo la quale abilitare il download dei messaggi
        /// </summary>
        public DateTime? FirstReceivingMessageDate { get; set; }

        /// <summary>
        /// Numero di giorni da monitorare per il download
        /// </summary>
        public int DaysToRead { get; set; }

        /// <summary>
        /// Numero di giorni dopo il quale cancellare la posta archiviata
        /// </summary>
        public int GracePeriod { get; set; }


        public virtual Company Company { get; set; }
        public virtual MailServer MailServer { get; set; }


        // V2
        /// <summary>
        /// true = svuota il cestino
        /// </summary>
        public bool EmptyTrash { get; set; }
        public DateTime? LastDeletionDate { get; set; }
        [StringLength(64)]
        public string ReaderWorkerId { get; set; }
        [StringLength(64)]
        public string SenderWorkerId { get; set; }
        [StringLength(64)]
        public string EraserWorkerId { get; set; }
        public DateTime? NextReaderDate { get; set; }
        public DateTime? NextSenderDate { get; set; }
        public DateTime? NextEraserDate { get; set; }
        /// <summary>
        /// Numero di minuti di pausa tra una lettura della casella e la successiva
        /// </summary>
        public int IdleTime { get; set; }

        /// <summary>
        /// Tipo di documento da utilizzare per archiviare la posta in ingresso (es. ticket, Fatture, ...)
        /// </summary>
        public string DocumentType { get; set; }


        /// <summary>
        /// true = i messaggi vengono automaticamente archiviati come documenti
        /// </summary>
        public bool SaveAsDocument { get; set; }


        // Evoluzioni future
        //public bool EnableVirusTotal { get; set; }
        //public bool EnableAntispam { get; set; }
        //public bool IgnoreSPAM { get; set; }
        //public string DocumentTypeId { get; set; } //Tipologia da usare per l'archiviazione
        //public int ProcessId { get; set; } // Processo da avviare all'archiviazione
    }
}
