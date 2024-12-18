
using OpenDMS.Domain.Enumerators;
namespace OpenDMS.Core.DTOs
{
    public class ViewProperties
    {
        public string ViewId { get; set; } = "";
        public RowId KeyFields { get; set; } = new RowId();

        //  NOTA: E' la pagina che decide se le righe sono selezionabili, le colonne personalizzabili e come apparirà il titolo 
        //        public bool Selectable { get; set; }
        //        public bool Customizable { get; set; }
        //        public string Title { get; set; } = "";
        //        public string CSSIcon { get; set; } = "";
        public string DoubleClickAction { get; set; } = "";
        public string RightClickAction { get; set; } = "";


        // Informazioni personalizzabili
        public List<ViewColumn> Columns { get; set; } = new List<ViewColumn>();
        public ViewStyle ViewStyle { get; set; }

        // VNEXT
        public string GroupByColumn { get; set; } = "";

    }
}
