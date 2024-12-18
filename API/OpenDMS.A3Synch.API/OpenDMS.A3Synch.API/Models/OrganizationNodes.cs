using System.ComponentModel.DataAnnotations;

namespace A3Synch.Models
{
    public class OrganizationNodes
    {
        [Key]
        [StringLength(64)]
        public string Id { get; set; }
        public int LeftBound { get; set; }
        public int RightBound { get; set; }

        [StringLength(64)]
        public string? UserGroupId { get; set; } = "";

        [StringLength(64)]
        public string? ParentUserGroupId { get; set; }

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
        public int? TaskReallocationStrategy { get; set; } = 0;

        [StringLength(64)]
        public string? TaskReallocationProfile { get; set; } = null;
        /// <summary>
        /// Note relative alla chiusura del nodo (es. accorpamento con... )
        /// </summary>
        public string? ClosingNote { get; set; } = "";
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.Now;
    }
}