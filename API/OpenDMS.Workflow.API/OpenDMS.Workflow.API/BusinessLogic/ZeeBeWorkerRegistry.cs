using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Models;
using Zeebe.Client;
using Zeebe.Client.Api.Worker;

namespace OpenDMS.Workflow.API.BusinessLogic

{
    public class ZeeBeWorkerRegistry : IBPMWorkerRegistry
    {
        private readonly IZeebeClient client;

        private readonly ILoggerFactory loggerFactory;
        private readonly IConfiguration configuration;

        private static readonly string ZeebeDefaultUrl = "127.0.0.1:26500";
        private readonly string WorkerName = Guid.NewGuid().ToString();

        public const string CONST_CAMUNDA_ENDPOINT = "Camunda:EndPoint";

        public ZeeBeWorkerRegistry(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            //client = ZeebeClient;
            var zeebeUrl = configuration[CONST_CAMUNDA_ENDPOINT];
            if (string.IsNullOrWhiteSpace(zeebeUrl))
                zeebeUrl = ZeebeDefaultUrl;
            client = ZeebeClient.Builder()
               .UseLoggerFactory(loggerFactory)
               .UseGatewayAddress(zeebeUrl)
               .UsePlainText()
               .UseKeepAlive(TimeSpan.FromSeconds(60.0))
               .Build();
            this.loggerFactory = loggerFactory;
            this.configuration = configuration;
        }



        public async Task AddWorker(string JobType, AsyncJobHandler handler, int MaxJobs = 5, int PollingInterval = 1, int TimeOut = 10)
        {
            var job = client.NewWorker()
                  .JobType(JobType)
                  .Handler( handler)
                  .MaxJobsActive(MaxJobs)
                  .HandlerThreads((byte)(MaxJobs > 5 ? 5 : MaxJobs))
                  .Name(WorkerName)
                  //                  .AutoCompletion()
                  .PollInterval(TimeSpan.FromSeconds(PollingInterval < 1 ? 1 : PollingInterval))
                  .Timeout(TimeSpan.FromSeconds(TimeOut < 10 ? 10 : TimeOut ));
            job.Open();
        }


    }
}
