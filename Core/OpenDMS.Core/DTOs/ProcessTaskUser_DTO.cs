using OpenDMS.Domain.Models;

namespace OpenDMS.Core.DTOs;

public class ProcessTaskUser_DTO
{
    public string BusinessProcessId { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public int Tasks { get; set; }

    public DateTime FromDate  { get; set; }
    public DateTime ToDate { get; set; }

    public int MinHours { get; set; }
    public int MaxHours { get; set; }
    public int AvgHours { get; set; }

}
