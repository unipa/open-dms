
namespace OpenDMS.Domain.Models;
public class ProcessSummary
{
    public string BusinessProcessId { get; set; }
    public string BusinessProcessName { get; set; }

    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int Count { get; set; }
    public int Users{ get; set; }
    public int AvgHours { get; set; }
    public int MinHours { get; set; }
    public int MaxHours { get; set; }

}
