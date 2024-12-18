using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.OrganizationUnits
{

    /// <summary>
    /// Operazioni sull'organigramma:
    /// 1. Creazione nuovo nodo. Non comporta alcun problema
    /// 1.1. Inserimento nuova risorsa. non comporta alcun problema
    /// 2. Spostamento gruppo. Sarà creato un nuovo nodo, con una nuova relazione tra userGroup e ParentUserGroup. Non compoerta alcun problema
    /// 3. Rimozione/spostamento risorsa da gruppo. I task attivi della risorsa, provenienti dal gruppo vengono rilasciati quando entra in vigore la data di decorrenza
    /// 4. Modifica di ruolo di un utente. I task in carico relativi al ruolo vengono riassegnati al ruolo.
    /// 5. Cessazione nodo. 
    ///     1. I task in carico al gruppo vengono riassegnati al nodo padre/nodo specifico/cessati/nessuna azione
    ///     2. I task in carico alle risorse vengono rilasciati
    ///     
    /// 
    /// Ogni giorno, un batch deve:
    /// 1. verificare la data decorrenza dei ruoli degli utenti
    ///    rilasciare i task attivi in carico, o riassegnarli al gruppo.
    ///    marcare la risorsa come elaborata.
    ///    avviare processo dismissione utente
    /// 2. Verificare la data decorrenza del gruppo
    ///    rilasciare i task attivi delle risorse 
    ///    riassegnare il tutto secondo le policy del gruppo
    ///    marcare il gruppo come elaborato
    ///    avviare processo dismissione nodo
    /// 3. verificare l'apertura di un nuovo nodo
    ///    avviare processo apertura nuovo nodo
    /// 4. verificare apertura nuovo ruolo in nodo
    ///    avviare processo apertura nuova risorsa
    /// </summary>

    [PrimaryKey(nameof(Id))]
    [Index(nameof(LeftBound), IsUnique = true)]
    [Index(nameof(RightBound), IsUnique = true)]
    [Index(nameof(StartISODate), nameof(UserGroupId), IsUnique = true)]
    public class OrganizationNode
    {

        [StringLength(64)]
        public string Id { get; set; } = Guid.NewGuid().ToString();


        public int LeftBound { get; set; }
        public int RightBound { get; set; }

        [StringLength(64)]
        public string UserGroupId { get; set; } = "";

        [StringLength(64)]
        public string ParentUserGroupId { get; set; }

        /// <summary>
        /// Data Inizio Validità dello UserGroup in questo nodo (YYYYMMDD)
        /// </summary>
        public int StartISODate { get; set; } = 0;

        /// <summary>
        /// Data Fine Validità dello UserGroup in questo nodo  (YYYYMMDD) (0 oppure 99999999 per non avere fine)
        /// </summary>
        public int EndISODate { get; set; } = 0;

        /// <summary>
        /// Modalità di chiusura del nodo
        /// 1. Riassegnazione Tasks al nodo padre
        /// 2. Riassegnazione Tasks ad un nodo specifico (ClosingProfile)
        /// 3. Riassegnazione Tasks ad un ruolo specifico (ClosingProfile)
        /// 4. Completamento Automatico dei Tasks
        /// </summary>
        public int TaskReallocationStrategy { get; set; }

        [StringLength(64)]
        public string TaskReallocationProfile { get; set; }

        /// <summary>
        /// Note relative alla chiusura del nodo (es. accorpamento con... )
        /// </summary>
        public string ClosingNote { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

        public virtual UserGroup UserGroup { get; set; }
        [JsonIgnore]
        public virtual UserGroup ParentUserGroup { get; set; }
    }
}