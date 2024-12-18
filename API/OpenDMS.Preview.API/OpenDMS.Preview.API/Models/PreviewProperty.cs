namespace OpenDMS.Preview.API.Models;

public class PageSize
{
    public int Width { get; set; }
    public int Height { get; set; }

    public PageSize()
    {

    }
    public PageSize(int width, int height)
    {
        Width = width;
        Height = height;
    }
}
public enum PageType
{
    Image,
    Html
}
public class PreviewProperty
{
    public bool Protected { get; set; }
    public bool Obscured { get; set; }
    public string ErrorMessage { get; set; }

    public string FileName { get; set; }
    public long FileSize { get; set; }
    public string FileExtension { get; set; }
    public DateTime CreationDate { get; set; }
    public string MediaType { get; set; }
    public string FileIcon { get; set; }
    public int Pages { get; set; }
    public PageType PageType { get; set; }
    public PageSize DefaultPageSize { get; set; } = new PageSize();
    //public Dictionary<int, PageSize> PagesWithDifferentSize { get; set; }
}
