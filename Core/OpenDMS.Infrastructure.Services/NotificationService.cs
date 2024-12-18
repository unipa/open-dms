using MessageBus.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using OpenDMS.MailSpooler.Core.Interfaces;

namespace OpenDMS.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IMessaging<string> _messaging;
        private readonly ILogger<NotificationService> logger;
        private readonly IServiceProvider serviceProvider;
        private readonly IConfiguration _config;
        private readonly string queueName;

        public NotificationService(IMessaging<string> messaging,
            ILogger<NotificationService> logger,
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            _messaging = messaging;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            _config = configuration;

            queueName = _config[StaticConfiguration.CONST_MAILSERVICE_QUEUE];
        }
        public async Task SendMail(CreateOrUpdateMailMessage MailMessage)
        {

            string msg = System.Text.Json.JsonSerializer.Serialize(MailMessage);
            if (String.IsNullOrEmpty(queueName))
            {
                logger.LogWarning("SendMail", "<EmptyQueue>:" + msg);
                try
                {
                    IMailSenderService MailSender = serviceProvider.GetRequiredService<IMailSenderService>();
                    var Entry = await MailSender.Save (MailMessage, UserProfile.SystemUser());
//                    await MailSender.SendMail(Entry, UserProfile.SystemUser())
                }
                catch (Exception Ex)
                {
                    logger.LogError(Ex, "SendMail", queueName + ":" + msg);
                    throw;
                }
                return;
            }
            try
            {
                _messaging.PushMessage(msg, queueName);
                logger.LogDebug("SendMail", queueName + ":" + msg);
            }
            catch (Exception Ex)
            {
                logger.LogError(Ex, "SendMail", queueName + ":" + msg);
                throw;
            }


            await Task.CompletedTask;
        }
    }
}
