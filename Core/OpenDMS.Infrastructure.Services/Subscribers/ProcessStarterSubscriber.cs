using MessageBus.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using OpenDMS.Infrastructure.Services.BusinessLogic;

namespace OpenDMS.Infrastructure.Services.Subscribers
{
    public class ProcessStarterSubscriber : IEventSubscriber
    {
        private readonly ILogger<ProcessStarterSubscriber> logger;
        private readonly IServiceProvider serviceProvider;
        private readonly IMessaging<string> messageBus;
        private readonly IConfiguration config;

        private string queueName;

        public ProcessStarterSubscriber(
            ILogger<ProcessStarterSubscriber> logger,
            IServiceProvider serviceProvider,
            IMessaging<string> messageBus, 
            IConfiguration config)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            this.messageBus = messageBus;
            this.config = config;
            queueName = config[StaticConfiguration.CONST_BPMSERVICE_QUEUE];
        }

        public async Task Invoke(IEvent ApplicationEvent)
        {
            var msg =  ApplicationEvent.Serialize();
            if (string.IsNullOrEmpty(queueName))
            {  
                var e = EventMessage.Deserialize(msg);
                logger.LogWarning("StartProcess", "<EmptyQueue>:" + msg);
                IDocumentWorkflowEngine WF = serviceProvider.GetRequiredService<IDocumentWorkflowEngine>();
                if (WF != null)
                    await WF.HandleMessage(e);
                return;
            }
            try
            {
                messageBus.PushMessage(msg, queueName);
                logger.LogDebug("StartProcess", queueName + ":" + msg);
            }
            catch (Exception Ex)
            {
                logger.LogError(Ex, "StartProcess", queueName + ":" + msg);
                throw;
            }
            await Task.CompletedTask;
        }
    }
}
