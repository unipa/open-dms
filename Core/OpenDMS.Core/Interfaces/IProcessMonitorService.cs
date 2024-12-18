using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Models;

namespace OpenDMS.Core.Interfaces
{
    public interface IProcessMonitorService
    {
        Task<List<ProcessSummary_DTO>> GetActiveProcesses(UserProfile u, string businessProcessId = "");
        Task<List<ProcessSummary_DTO>> GetClosedProcesses(UserProfile u, DateTime FromDate, DateTime ToDate, string businessProcessId = "");


        Task<List<ProcessTaskUser>> GetActiveProcessDetails(UserProfile u, string businessProcessId, bool Expired = false);
        Task<List<ProcessTaskUser>> GetClosedProcessDetails(UserProfile u, DateTime FromDate, DateTime ToDate, string businessProcessId, bool Expired = false);
    }
}