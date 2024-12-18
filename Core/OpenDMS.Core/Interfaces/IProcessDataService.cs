using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Entities;

namespace OpenDMS.Core.Interfaces
{
    public interface IProcessDataService
    {
//        Task AddPermissions(ProcessPermission permission);
        Task DeleteByProcessId(string processId);
        Task<PerformanceIndicatorValue> GetIndicator(string processInstanceId, string indicatorId);
        Task<List<PerformanceIndicatorValue>> GetIndicators(string processInstanceId);
        Task<List<PerformanceIndicatorValue>> GetIndicatorValues(string processId, string indicatorId);
        Task<ProcessInstanceVariable> GetVariable(string processInstanceId, string variableId);
        Task<List<ProcessInstanceVariable>> GetVariables(string processInstanceId);
        Task<List<ProcessInstanceVariable>> GetVariableValues(string processId, string variableId);
        Task RemoveBeforeDate(DateTime Date);
//        Task RemovePermissions(ProcessPermission permission);
        Task SavePerformanceIndicator(PerformanceIndicatorValue indicatorValue);
        Task SaveVariable(ProcessInstanceVariable variable);
    }
}