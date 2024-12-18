using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Entities.Settings;
using System.ComponentModel.DataAnnotations;
using Web.DTOs;

namespace Web.Model.Customize
{
    public class UserContactDigitalAddressViewModel
    {
        public List<ContactDigitalAddress_DTO> DigitalAddresses { get; set; } = null;
        public List<MailServer> MailServers { get; set; } = null;
        public List<Company> Companies { get; set; } = null;
        public string Utente { get; set; }
        public string Host { get; set; }
        public string Token { get; set; }
        public string MailboxAddress { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public string Icon { get; set; }
    }

    public class Mailbox_DTO
    {

        public int ContactDigitalAddressId { get; set; } = 0;
        public int Id { get; set; } = 0;

        /// <summary>
        /// Indirizzo Email (minuscolo, trimmato, senza nomi e virgolette)
        /// </summary>
        //[Required, RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$"), StringLength(255)]
        public string MailAddress { get; set; }

        /// <summary>
        /// Nome da anteporre all'indirizzo mail
        /// </summary>
        [Required, StringLength(128)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Firma dell'utente da inserire in calce della mail
        /// </summary>
        [ValidateNever]
        public string UserSignature { get; set; } = string.Empty;

        /// <summary>
        /// Id del mailserver collegato, da recuperare attraverso il dominio dell mail
        /// </summary>
        [Required]
        public int MailServerId { get; set; }

        /// <summary>
        /// Account di accesso alal casella mail 
        /// </summary>
        [ValidateNever]
        public string Account { get; set; }

        /// <summary>
        ///  Password di accesso 
        /// </summary>
        [ValidateNever]
        public string Password { get; set; }

        /// <summary>
        /// Token di accesso alla casella per autenticazioni OAUTH
        /// </summary>
        [ValidateNever]
        public string TokenId { get; set; }

        /// <summary>
        /// Refresh Token di accesso alla casella per autenticazioni OAUTH
        /// </summary>
        [ValidateNever]
        public string RefreshToken { get; set; }

        /// <summary>
        /// true = abilita il download della posta dalla casella
        /// </summary>
        [ValidateNever]
        public bool Validated { get; set; }


        /// <summary>
        /// Ultima data di modifica delle credenziali
        /// </summary>
        [ValidateNever]
        public DateTime? LastCredentialUpdate { get; set; }

        /// <summary>
        /// Utente applicativo collegato alla mail
        /// </summary>
        [ValidateNever] //perché lo carico sul action nel controller
        public string UserId { get; set; }

        /// <summary>
        /// Company collegata alla mail
        /// </summary>
        [Required]
        public int CompanyId { get; set; }

        /// <summary>
        /// Elenco di profili (utenti/ruoli/gruppi) abilitati alla lettura della casella
        /// </summary>
        [ValidateNever]
        public string ReadOnlyProfiles { get; set; }

        /// <summary>
        /// Elenco di profili (utenti/ruoli/gruppi) abilitati all'invio dalla casella
        /// </summary>
        [ValidateNever]
        public string SendEnabledProfiles { get; set; }

        /// <summary>
        /// Elenco di profili (utenti/ruoli/gruppi) abilitati alla creazione di bozze dalla casella
        /// </summary>
        [ValidateNever]
        public string DraftEnabledProfiles { get; set; }

        /// <summary>
        /// true = abilita il download della posta dalla casella
        /// </summary>
        [Required]
        public bool EnableDownload { get; set; } = false;

        /// <summary>
        /// true = cancella i messaggi correttamente archiviati
        /// </summary>
        [Required]
        public bool DeleteDownloadedMail { get; set; } = false;

        /// <summary>
        /// Cartelle IMAP da monitorare
        /// </summary>
        [ValidateNever]
        public string? DownloadImapFolders { get; set; } = string.Empty;

        /// <summary>
        /// Cartella IMAP in cui salvare i messaggi scaricati
        /// </summary>
        [ValidateNever]
        public string? SaveToImapFolder { get; set; } = string.Empty;

        /// <summary>
        /// Data dopo la quale abilitare il download dei messaggi
        /// </summary>
        [ValidateNever]
        public DateTime? FirstReceivingMessageDate { get; set; }

        /// <summary>
        /// Numero di giorni da monitorare per il download
        /// </summary>
        [ValidateNever]
        public int? DaysToRead { get; set; } = 0;

        /// <summary>
        /// Numero di giorni dopo il quale cancellare la posta archiviata
        /// </summary>
        [ValidateNever]
        public int? GracePeriod { get; set; } = 0;
    }

}
