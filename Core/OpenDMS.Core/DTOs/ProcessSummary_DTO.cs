using OpenDMS.Domain.Models;

namespace OpenDMS.Core.DTOs;

public class ProcessSummary_DTO
{
    public int ProcessKey { get; set; }
    public string BusinessProcessId { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Version { get; set; }
    public string FromDate { get; set; }
    public string ToDate { get; set; }

    public int Users { get; set; }
    public int NotExpired { get; set; }
    public int Expired { get; set; }

}
