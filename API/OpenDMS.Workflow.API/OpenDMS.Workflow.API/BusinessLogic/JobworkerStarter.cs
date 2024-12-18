using OpenDMS.Core.Interfaces;
using OpenDMS.Infrastructure.Services.BusinessLogic;
using OpenDMS.Workflow.API.DTOs;
using Org.BouncyCastle.Utilities;
using System.Reflection;

namespace OpenDMS.Workflow.API.BusinessLogic
{
    public class JobworkerStarter : IJobworkerStarter
    {
        private readonly IBPMWorkerRegistry workflowEngine;
        private readonly IServiceProvider service;

        public JobworkerStarter(IBPMWorkerRegistry  workflowEngine, IServiceProvider service)
        {
            this.workflowEngine = workflowEngine;
            this.service = service;
        }

        public async Task LoadTasks()
        {
            var _customTasks = service.GetServices<ICustomTask>().ToList();
            foreach (var task in _customTasks)
            {
                Console.WriteLine("STARTING " + task.JobType);
                await task.Initialize();
                await workflowEngine.AddWorker(task.JobType, task.HandleJob, task.MaxJobs, task.PollingInterval, task.TimeOut);

            }
        }


    }
}
