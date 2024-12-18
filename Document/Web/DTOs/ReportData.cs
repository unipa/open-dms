
using OpenDMS.Core.DTOs;

namespace Web.DTOs
{
    public class ReportData
    {
        public ReportFile Definition { get; set; }
        public List<FileExplorerModel> Data { get; set; }
    }
}
