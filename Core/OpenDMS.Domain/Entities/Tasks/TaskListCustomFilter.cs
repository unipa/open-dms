using System.ComponentModel.DataAnnotations;


namespace OpenDMS.Domain.Entities.Tasks
{
    /// <summary>
    /// Filtri custom per l'utente
    /// </summary>
    public class TaskListCustomFilter
    {
        public int Id { get; set; }
        [StringLength(64)]
        public string UserId { get; set; }
        [StringLength(64)]
        public string RoleId { get; set; }
        [StringLength(64)]
        public string GroupId { get; set; }

        public int Position { get; set; }

        [StringLength(64)]
        public string Icon { get; set; }

        /// <summary>
        /// Indica se il filtro è stato creato dal sistema e non può essere rimosso o spostato.
        /// </summary>
        public bool SystemFilter { get; set; } = false;

        [StringLength(64)]
        public string Name { get; set; }

        public string SerializedFilters { get; set; }

    }
}
