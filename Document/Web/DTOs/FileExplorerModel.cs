

using OpenDMS.Core.DTOs;

namespace Web.DTOs
{
    public class FileExplorerModel
    {
        public string Title { get; set; }
        public bool ShowDelete { get; set; }
        public bool ShowRemoveFromFolder { get; set; }
        public bool HideSelection { get; set; }
        public bool DisableSorting { get; set; }
        public int Filters { get; set; } = 0;

        public SearchRequest Request { get; set; } = new();

    }
}
