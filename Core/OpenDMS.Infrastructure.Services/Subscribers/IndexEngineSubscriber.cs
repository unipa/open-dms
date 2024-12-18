using MessageBus.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using OpenDMS.Infrastructure.Services.BusinessLogic;

namespace OpenDMS.Infrastructure.Services.Subscribers
{
    public class IndexEngineSubscriber : IEventSubscriber
    {
        private readonly ILogger<IndexEngineSubscriber> logger;
        private readonly IServiceProvider serviceProvider;
        private readonly IMessaging<string> messageBus;
        private readonly IConfiguration config;
        private readonly string queueName;

        public IndexEngineSubscriber(ILogger<IndexEngineSubscriber> logger,
                        IServiceProvider serviceProvider,
                        IMessaging<string> messageBus, 
                        IConfiguration config)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            this.messageBus = messageBus;
            this.config = config;
            queueName = config[StaticConfiguration.CONST_INDEXSERVICE_QUEUE];
        }

        public async Task Invoke(IEvent ApplicationEvent)
        {
            var e = ApplicationEvent.EventName;
            if (e == EventType.AddVersion || e == EventType.AddRevision || e == EventType.RemoveRevision || e == EventType.RemoveVersion)
            {
                var doc = (DocumentInfo)ApplicationEvent.Get<DocumentInfo>("Document");
                var documentId = doc.Id;
                if ((doc.DocumentType == null || String.IsNullOrEmpty(doc.DocumentType.Id) || doc.DocumentType.ToBeIndexed) && (doc.Image != null))
                {
                    var ImageId = doc.Image.Id;
                    var msg = "ADD:" + ImageId.ToString();
                    //if (string.IsNullOrEmpty(queueName))
                    {
                        logger.LogDebug("StartIndexing<FrontEnd>", msg);
                        //TODO: Gestire indicizzatore locale
                        foreach (ISearchEngine Indexer in serviceProvider.GetServices<ISearchEngine>())
                        {
                            _ = Task.Run(async () =>
                            {
                                await Indexer.Add(ImageId);
                            });
                        }
                    }
                    //try
                    //{
                    //    messageBus.PushMessage(msg, queueName);
                    //    logger.LogDebug("StartIndexing", queueName + ":" + msg);
                    //}
                    //catch (Exception Ex)
                    //{
                    //    logger.LogError(Ex, "StartIndexing", queueName + ":" + msg);
                    //    throw;
                    //}
                }
            }
            if (e == EventType.Delete)
            {
                var doc = (DocumentInfo)ApplicationEvent.Get<DocumentInfo>("Document");
                var documentId = doc.Id;// ApplicationEvent.Variables["ObjectId"];
                var msg = "REMOVE:" +documentId.ToString();
                //if (string.IsNullOrEmpty(queueName))
                {
                    logger.LogDebug("StartIndexing<FrontEnd>", msg);
                    //TODO: Gestire indicizzatore locale
                    foreach (ISearchEngine Indexer in serviceProvider.GetServices<ISearchEngine>())
                    {
                        await Indexer.Remove((int)documentId);
                    }
                    return;
                }
                //try
                //{
                //    messageBus.PushMessage(msg, queueName);
                //    logger.LogDebug("StartIndexing", queueName + ":" + msg);
                //}
                //catch (Exception Ex)
                //{
                //    logger.LogError(Ex, "StartIndexing", queueName + ":" + msg);
                //    throw;
                //}
            }
            await Task.CompletedTask;

        }
    }
}