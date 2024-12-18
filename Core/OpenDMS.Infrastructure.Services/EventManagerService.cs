using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using OpenDMS.Infrastructure.Services;

namespace OpenDMS.Infrastructure.Services
{
    public class EventManager : IEventManager
    {

        private readonly ILogger<EventManager> logger;
        private readonly IServiceProvider serviceProvider;

        public EventManager(ILogger<EventManager> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        public async Task<bool> Publish(IEvent ApplicationEvent)
        {
            int errors = 0;
            int subscribers = 0;
            foreach (var service in serviceProvider.GetServices<IEventSubscriber>())
            {
                try
                {
                    subscribers++;
                    logger.LogDebug("Publish:" + ApplicationEvent.EventName, ApplicationEvent.Variables);
                    await service.Invoke(ApplicationEvent);
                }
                catch (Exception ex)
                {
                        errors++;
                        logger.LogError(ex, "Publish:" + ApplicationEvent.EventName, ApplicationEvent.Variables);
                }
            }
            if (subscribers == 0)
            {
                logger.LogError("Publish: <No Subscribers> - Ignored Event: " + ApplicationEvent.EventName, ApplicationEvent.Variables);
            }
            return subscribers > 0;
        }
    }
}