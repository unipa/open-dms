namespace OpenDMS.CustomPages.Models;

public class CustomPageDTO
{
    public List<CustomPageDTO> SubItems { get; set; } = new();

    public string PageId { get; set; } = "";
    public string ParentPageId { get; set; } = "";
    public int Alignment { get; set; } = 0;

    public string Title { get; set; } = "";
    public string URL { get; set; } = "";

    public string Tooltip { get; set; } = "";


    public string Icon { get; set; } = "";
    public string Target { get; set; } = "";

    public string BadgeCss { get; set; } = "";
    public string BadgeURL { get; set; } = "";
    public string BadgeMessage { get; set; } = "";


}
