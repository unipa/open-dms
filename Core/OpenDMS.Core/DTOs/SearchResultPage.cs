

namespace OpenDMS.Core.DTOs;

public class SearchResultPage
{
    public string ViewId { get; set; } = "";
    public SearchRequest Request { get; set; } = new();
    public int PageIndex { get; set; }

    public List<SearchResultRow> Rows { get; set; } = new List<SearchResultRow>();


//    public List<GroupRow> Groups { get; set; } = new List<GroupRow>();
    public SearchResultPage()
    {
    }

    public SearchResultPage(SearchRequest sr)
    {
        ViewId = sr.ViewId;
        PageIndex = sr.PageIndex;
        Request = sr;   
    }

}
