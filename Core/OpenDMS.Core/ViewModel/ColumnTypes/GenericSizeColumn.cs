using OpenDMS.Core.DTOs;

namespace OpenDMS.Core.ViewModel.ColumnTypes;

public class GenericSizeColumn : GenericNumberColumn
{
    public const int defaultSize = 100;

    public GenericSizeColumn(
        string id, 
        string title, 
        string description, 
        string category 
        ) : base(id, title, description, category, defaultSize, NumberFormat.Money, false, true)
    {
    }


    public override async Task<SearchResultColumn> Render(string[] fields)
    {
        var a = fields[0];
        string b = a;
        if (!string.IsNullOrEmpty(a))
        {
            decimal d = decimal.Parse(a);
            if (d > 1000_000_000) //mega
                b = (d / 1000_000_000).ToString("#,##0") + " GB";
            else
            if (d > 1000_000) //mega
                b = (d / 1000_000).ToString("#,##0") + " MB";
            else
            if (d > 1000) //kilo 
                b = (d / 1000).ToString("#,##0") + " KB";
            else
                b = d.ToString() + "  B";

        }
        return new SearchResultColumn() { Value = a, Description = b };
    }
}
