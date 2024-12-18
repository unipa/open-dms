

using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Core.DTOs;

public class SearchResultRow
{
    public List<string> Keys { get; set; } = new List<string>();
    public bool Selectable { get; set; }
    public RowState RowState { get; set; }

    public List<SearchResultColumn> Columns { get; set; } = new List<SearchResultColumn>();

}
