using MessageBus.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using OpenDMS.Infrastructure.Services.BusinessLogic;

namespace OpenDMS.Infrastructure.Services.Subscribers
{
    public class PreviewGeneratorSubscriber : IEventSubscriber
    {
        private readonly IPreviewGenerator previewGenerator;
        private readonly ILogger<PreviewGeneratorSubscriber> logger;
        private readonly IMessaging<string> messageBus;
        private readonly IConfiguration config;
        private readonly string queueName;

        public PreviewGeneratorSubscriber(
            IPreviewGenerator previewGenerator, 
            ILogger<PreviewGeneratorSubscriber> logger,
            IMessaging<string> messageBus, 
            IConfiguration config)
        {
            this.previewGenerator = previewGenerator;
            this.logger = logger;
            this.messageBus = messageBus;
            this.config = config;
            queueName = config[StaticConfiguration.CONST_PREVIEWSERVICE_QUEUE];
        }

        public async Task Invoke(IEvent ApplicationEvent)
        {
            var e = ApplicationEvent.EventName;
            if (e == EventType.AddVersion || e == EventType.AddRevision || e == EventType.Protocol)
            {
                var doc = (DocumentInfo)ApplicationEvent.Get<DocumentInfo>("Document");
                var documentId = doc.Id;
                var msg =  ApplicationEvent.Serialize(); 
                try
                {
                    if (doc.Image != null)
                    {
                        if (string.IsNullOrEmpty(queueName))
                        {
                            var contentId = doc.Image.Id;// (int)(ApplicationEvent.Variables["ContentId"]);
                            logger.LogDebug("StartPreviewGeneration<FrontEnd>:" + msg);
                            //TODO: Generare la preview PDF
                            if (doc.Image.PreviewStatus == Domain.Enumerators.JobStatus.Queued || e == EventType.Protocol)
                            {
                                _ = Task.Run(async () =>
                                {
                                    await previewGenerator.Generate(contentId);
                                });
                            }
                            return;
                        }
                        messageBus.PushMessage(msg, queueName);
                    }
                    //logger.LogDebug("StartPreviewGeneration:"+ queueName + ":" + msg);
                }
                catch (Exception Ex)
                {
                    logger.LogError(Ex, "StartPreviewGeneration<FrontEnd>:" +  queueName + ":" + msg);
                    throw;
                }
            }
            await Task.CompletedTask;

        }
    }
}