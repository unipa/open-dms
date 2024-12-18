using Authenticator;
using MessageBus.Interface;
using Microsoft.Extensions.DependencyInjection;
using OpenDMS.Domain.Services;
using OpenDMS.Infrastructure.Services;
using OpenDMS.Infrastructure.Services.BusinessLogic;
using OpenDMS.Infrastructure.Services.Subscribers;
using OpenDMS.MailSpooler.Core;
using MessageBus.RabbitMQ;
using OpenDMS.MailSpooler.Core.Archiver;
using OpenDMS.Core.Interfaces;
using Elmi.Core.FileConverters.Implementation;
using Elmi.Core.FileConverters;
using Elmi.Core.PreviewGenerators;
using OpenDMS.MailSpooler.Core.Interfaces;
using OpenDMS.Infrastructure.Services.Workers;
using OpenDMS.Domain.Worker;
using OpenDMS.Infrastructure.Services.Monitor;
using Microsoft.Extensions.Configuration;
using OpenDMS.Domain.Constants;

namespace OpenDMS.Core
{
    public static class RepositoryExtensions
    {

        public static IServiceCollection AddServiceWorkers(this IServiceCollection Services, IConfiguration config)
        {
            if (string.IsNullOrEmpty(config[StaticConfiguration.CONST_MAILSERVICE_QUEUE]))
            {
                Services.AddHostedService<MailSenderWorker>();
                Services.AddHostedService<MailReceiverWorker>();
            }
            if (string.IsNullOrEmpty(config[StaticConfiguration.CONST_PREVIEWSERVICE_QUEUE]))
                Services.AddHostedService<PreviewGeneratorWorker>();

            if (string.IsNullOrEmpty(config[StaticConfiguration.CONST_INDEXSERVICE_QUEUE]))
                Services.AddHostedService<DocumentIndexerWorker>();
            return Services;
        }

        public static IServiceCollection AddMessageBusMonitors(this IServiceCollection Services)
        {
            Services.AddSingleton<IMessageBusMonitor, MailMessageQueueMonitor>();
            Services.AddSingleton<IMessageBusMonitor, PreviewQueueMonitor>();
            Services.AddSingleton<IMessageBusMonitor, ProcessQueueMonitor>();
            return Services;
        }

        public static IServiceCollection AddExternalServices(this IServiceCollection Services, IConfiguration config)
        {
            // Services.AddScoped<IFileConverter, _BPMNToHTML>();
            Services.AddTransient<IFileConverter, EMLToHTML>();
            Services.AddTransient<IFileConverter, HTMLToPDF>();
            Services.AddTransient<IFileConverter, ImageToPDF>();
            Services.AddTransient<IFileConverter, MSGToHTML>();
            Services.AddTransient<IFileConverter, OfficeToPDF>();
            Services.AddTransient<IFileConverter, OpenOfficeToPDF>();
            Services.AddTransient<IFileConverter, SameFormat>();
            Services.AddTransient<IFileConverter, TextToHTML>();
            Services.AddTransient<IFileConverter, XMLToHTML>();
            Services.AddTransient<IFileConverter, ZIPToHTML>();
            Services.AddTransient<IFileConverter, HTMLToDocX>();
            Services.AddTransient<IFileConverter, MP4ToImage>();

            Services.AddTransient<IFileConvertFactory, FileConvertFactory>();

            if (!String.IsNullOrEmpty(config["Camunda:EndPoint"]))
            {
                //TODO: Spostare questi servizi su un Microservizio dedicato
                // Services.AddScoped<IWorkflowInterface, ZeeBeEngine>();
                Services.AddTransient<IWorkflowEngine, ZeeBeWorkflowEngine>();
                Services.AddTransient<IDocumentWorkflowEngine, DocumentWorkflowEngine>();
                Services.AddTransient<IEventSubscriber, ProcessStarterSubscriber>();
            }

            Services.AddTransient<IPreviewGenerator, PreviewGenerator>();

            //Sottoscrittori dell'Event Manager
            Services.AddTransient<IEventSubscriber, EventTaskManagerSubscriber>();
            Services.AddTransient<IEventSubscriber, DefaultContentCreatorSubscriber>();
            Services.AddTransient<IEventSubscriber, HistoryPersisterSubscriber>();

            Services.AddTransient<IEventSubscriber, IndexEngineSubscriber>();
            Services.AddTransient<IEventSubscriber, PreviewGeneratorSubscriber>();
            Services.AddTransient<IEventSubscriber, MailParsingSubscriber>();

            //-- i Repo e i Service necessari al MailSpooler e MailSender sono in AddRepositories() e AddCoreServices().

            Services.AddTransient<IAuthenticator, MicrosoftAuth>();
            Services.AddTransient<IAuthenticator, GoogleAuth>();
            Services.AddTransient<IAuthenticator, PlainTextAuth>();

            Services.AddTransient<IAuthenticatorFactory, AuthenticatorFactory>();
            Services.AddTransient<IMailParser, MailParser>();
            Services.AddTransient<IMailEntryService, MailEntryService>();
            Services.AddTransient<IMessageRetrieverService, MessageRetrieverService>();
            Services.AddTransient<IMailReaderService, MailReaderService>();
            Services.AddTransient<IMailSenderService, MailSenderService>();

            Services.AddTransient<IDocumentNotificationService, DocumentNotificationService>();
            Services.AddTransient<INotificationService, NotificationService>();

            Services.AddTransient<IEventManager, EventManager>();

            //DIPENDENZA DA NOTIFICATION SERVICE
            Services.AddTransient<IMessaging<string>, RabbitMQMessageBus<string>>();

            return Services;
        }
        public static IServiceCollection AddExternalCoreServices(this IServiceCollection Services, IConfiguration config)
        {
            // Services.AddScoped<IFileConverter, _BPMNToHTML>();
            Services.AddTransient<IFileConverter, EMLToHTML>();
            Services.AddTransient<IFileConverter, HTMLToPDF>();
            Services.AddTransient<IFileConverter, ImageToPDF>();
            Services.AddTransient<IFileConverter, MSGToHTML>();
            Services.AddTransient<IFileConverter, OfficeToPDF>();
            Services.AddTransient<IFileConverter, OpenOfficeToPDF>();
            Services.AddTransient<IFileConverter, SameFormat>();
            Services.AddTransient<IFileConverter, TextToHTML>();
            Services.AddTransient<IFileConverter, XMLToHTML>();
            Services.AddTransient<IFileConverter, ZIPToHTML>();
            Services.AddTransient<IFileConverter, MP4ToImage>();

            Services.AddTransient<IFileConvertFactory, FileConvertFactory>();

            if (!String.IsNullOrEmpty(config["Camunda:EndPoint"]))
            {
                //TODO: Spostare questi servizi su un Microservizio dedicato
                // Services.AddScoped<IWorkflowInterface, ZeeBeEngine>();
                Services.AddTransient<IWorkflowEngine, ZeeBeWorkflowEngine>();
                Services.AddTransient<IDocumentWorkflowEngine, DocumentWorkflowEngine>();
                Services.AddTransient<IEventSubscriber, ProcessStarterSubscriber>();
            }

            Services.AddTransient<IPreviewGenerator, PreviewGenerator>();

            //Sottoscrittori dell'Event Manager
            Services.AddTransient<IEventSubscriber, EventTaskManagerSubscriber>();
            Services.AddTransient<IEventSubscriber, DefaultContentCreatorSubscriber>();
            Services.AddTransient<IEventSubscriber, HistoryPersisterSubscriber>();

            //Services.AddTransient<IEventSubscriber, IndexEngineSubscriber>();
            //Services.AddTransient<IEventSubscriber, PreviewGeneratorSubscriber>();
            //Services.AddTransient<IEventSubscriber, MailParsingSubscriber>();

            //Services.AddTransient<IAuthenticator, MicrosoftAuth>();
            //Services.AddTransient<IAuthenticator, GoogleAuth>();
            //Services.AddTransient<IAuthenticator, PlainTextAuth>();

            //Services.AddTransient<IAuthenticatorFactory, AuthenticatorFactory>();
            //Services.AddTransient<IMailParser, MailParser>();
            //Services.AddTransient<IMailEntryService, MailEntryService>();
            //Services.AddTransient<IMessageRetrieverService, MessageRetrieverService>();
            //Services.AddTransient<IMailReaderService, MailReaderService>();
            //Services.AddTransient<IMailSenderService, MailSenderService>();

            Services.AddTransient<IDocumentNotificationService, DocumentNotificationService>();
            Services.AddTransient<INotificationService, NotificationService>();

            Services.AddTransient<IEventManager, EventManager>();

            //DIPENDENZA DA NOTIFICATION SERVICE
            Services.AddTransient<IMessaging<string>, RabbitMQMessageBus<string>>();

            return Services;
        }
    }
}
