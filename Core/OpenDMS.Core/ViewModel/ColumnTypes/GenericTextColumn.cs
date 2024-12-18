using OpenDMS.Core.DTOs;


namespace OpenDMS.Core.ViewModel.ColumnTypes
{
    public class GenericTextColumn : ViewColumn
    {
        public GenericTextColumn(
             string id,
             string title,
             string description,
             string category,
             int size = 0)
            : base(id, Domain.Enumerators.ColumnDataType.Text, title, description, category, size, true, true, null )
        {
        }
        public GenericTextColumn(
             string id,
             string title,
             string category,
             int size = 0)
            : base(id, Domain.Enumerators.ColumnDataType.Text, title, title, category, size, true, true, null)
        {
        }
    }
}
