using MessageBus.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Worker;
using OpenDMS.Infrastructure.Services.BusinessLogic;

namespace OpenDMS.Infrastructure.Services.Monitor
{
    public class PreviewQueueMonitor : IMessageBusMonitor
    {
        private readonly IMessaging<string> _messaging;
        private readonly ILogger<PreviewQueueMonitor> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider serviceProvider;
        private readonly string queue = "";

        public PreviewQueueMonitor(
            IMessaging<string> messaging,
            ILogger<PreviewQueueMonitor> logger,
            IConfiguration configuration,
            IServiceProvider serviceProvider
            )
        {
            _messaging = messaging;
            _logger = logger;
            _configuration = configuration;
            this.serviceProvider = serviceProvider;
            queue = _configuration[StaticConfiguration.CONST_PREVIEWSERVICE_QUEUE] ?? "";
        }



        public bool StartListenForNewMailMessages()
        {
            bool Success = false;
            if (string.IsNullOrEmpty(queue)) return Success;
            try
            {
                _messaging.Listening(queue);
                _messaging.OnMessageReceived += _messaging_OnMessageReceived;
                Success = true;
                _logger.LogInformation("Preview Monitor Listening on Queue: " + queue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return Success;
        }

        private async Task _messaging_OnMessageReceived(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            try
            {

                var ApplicationEvent = (IEvent)EventMessage.Deserialize (message);
                var doc = (DocumentInfo)ApplicationEvent.Get<DocumentInfo>("Document");
                var contentId = doc.Image.Id;
                using (var scope = serviceProvider.CreateScope())
                {
                    IPreviewGenerator previewGenerator = (IPreviewGenerator)scope.ServiceProvider.GetService(typeof(IPreviewGenerator));
                    await previewGenerator.Generate(contentId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }


        public bool StopListenForNewMailMessages()
        {
            bool Success = false;
            try
            {
                _messaging.StopListening();
                _logger.LogInformation("Preview Monitor Stopped");
                Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return Success;
        }
    }
}
