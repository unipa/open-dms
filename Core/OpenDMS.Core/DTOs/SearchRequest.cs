using OpenDMS.Domain.Models;

namespace OpenDMS.Core.DTOs;

public class SearchRequest
{
    public string ViewId { get; set; } = "";
    public int PageSize { get; set; } = 25;
    public int PageIndex { get; set; } = 0;
    public bool OpenOnNewWindow { get; set; } = false;
    public List<SortingColumn> OrderBy { get; set; } = new List<SortingColumn>();

    public List<SearchFilter> Filters { get; set; } = new List<SearchFilter>();

}
