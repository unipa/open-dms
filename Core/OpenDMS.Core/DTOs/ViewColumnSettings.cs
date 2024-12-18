using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Domain.Entities
{

    public class ViewColumnSettings
    {
        // Elementi personalizzabili
        public string Title { get; set; } = "";
        public SortingType SortType { get; set; }
        public bool Visible { get; set; } = false;
        public string Width { get; set; } = "";

        /* V2 */
        public AggregateType AggregateType { get; set; }
        //public string CellColorScripting { get; set; }

    }
}