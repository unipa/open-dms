using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Settings
{

    [PrimaryKey(nameof(PageId))]
    [Index(nameof(ParentPageId), nameof(Alignment), nameof(Position))]
    public class CustomPage
    {

        /// <summary>
        /// Identificativo Univoco del menu dell'applicazione
        /// </summary>
        [StringLength(64)]
        public string PageId { get; set; } = "";

        /// <summary>
        /// Identificativo Univoco del sotto-menu dell'applicazione
        /// </summary>
        [StringLength(64)]
        public string ParentPageId { get; set; } = "";


        /// <summary>
        /// Identificativo della prima voce di menu che raggruppa questa voce
        /// </summary>
        [StringLength(64)]
        public string HeaderPageId { get; set; } = "";


        /// <summary>
        /// Icona della voce di menu
        /// </summary>
        [StringLength(128)]
        public string Icon { get; set; } = "";

        /// <summary>
        /// Titolo della voce di menu. 
        /// Se Title è URL sono vuoti viene renderizzato un separatore
        /// </summary>
        [StringLength(64)]
        public string Title { get; set; } = "";

        [StringLength(255)]
        public string ToolTip { get; set; } = "";

        /// <summary>
        /// Url dell'applicazione da richiamare. Se vuoto  "Title" viene renderizzato come Intestazione di Gruppo
        /// </summary>
        [StringLength(255)]
        public string URL { get; set; } = "";

        /// <summary>
        /// Modalità di apertura della applicazione (vuoto = stessa finestra)
        /// </summary>
        [StringLength(64)]
        public string Target { get; set; } = "";


        public string Permissions { get; set; } = "";

        /// <summary>
        /// 0 = Top - Sticky
        /// 1 = Bottom - Sticky
        /// </summary>
        public int Alignment { get; set; } = 0;

        /// <summary>
        /// Indice di posizionamento 
        /// </summary>
        public int Position { get; set; } = 0;

        /// <summary>
        /// Indica se includere le voci di sotto-menu
        /// </summary>
        public bool IncludeSubMenus { get; set; } = false;

        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// URL per il calcolo del badge (vuoto se non necessario)
        /// </summary>
        [StringLength(255)]
        public string BadgeURL { get; set; } = "";


        ///// <summary>
        ///// Identificativo Univoco dell'applicazione di riferimento
        ///// </summary>
        //[StringLength(64)]
        //public string? ApplicationRegistryId { get; set; }

        //public virtual ApplicationRegistry ApplicationRegistry { get; set; }


    }
}
