using Zeebe.Client.Api.Worker;

namespace OpenDMS.Workflow.API.BusinessLogic

{
    public interface IBPMWorkerRegistry
    {
        Task AddWorker(string JobType, AsyncJobHandler handler, int MaxJobs = 5, int PollingInterval = 1, int TimeOut = 5);

    }
}