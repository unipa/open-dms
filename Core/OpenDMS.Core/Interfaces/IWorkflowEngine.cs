

using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Models;


namespace OpenDMS.Core.Interfaces;

public interface IWorkflowEngine
    {
        Task<Topology> Status();
        Task<long> DeployNewProcess(string processDiagram, string processName);
        Task SendMessage(string messageName, string correlationKey, string variables);
        Task SetVariables(string elementInstanceKey, string variables, bool Locale = false);
        Task CompleteJob(string jobKey, string variables);
        Task FailJob(string jobKey, int remainingRetries);
        Task ResolveIncident(string incidentKey, string incidentJob, int remainingRetries);
        Task<string> ReactivateElement(long processKey, string elementId, string variables = null);


        Task<long> StartProcess(long processKey, string variables);
        Task<long> StartProcess(string bpmnId, DocumentInfo Document, UserProfile User, string EventId, string variables);
        Task<long> UpdateProcessVariables(DocumentInfo Document, UserProfile User, string EventId, string variables);

        Task<string> StartProcessAndWait(long processKey, string variables);
        Task<string> StartProcessAndWait(string bpmnId, string variables);
        Task CancelProcessInstance(int processInstanceKey);
    //       Task AddWorker(string JobType AsyncJobHandler handler, int MaxJobs = 5, int PollingInterval = 1, int TimeOut = 5);

        Task<List<ProcessInstance_DTO>> GetAllInstancesByKey(string processDefinitionKey);
        Task<ProcessInstance_DTO> GetInstanceById(string processInstanceId);



}
