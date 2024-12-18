using MessageBus.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using OpenDMS.Domain.Worker;
using OpenDMS.Core.DTOs;
using Microsoft.Extensions.DependencyInjection;
using MailKit;
using OpenDMS.MailSpooler.Core;

namespace OpenDMS.Infrastructure.Services.Monitor
{
    public class MailMessageQueueMonitor : IMessageBusMonitor
    {
        private readonly IMessaging<string> _messaging;
        private readonly ILogger<MailMessageQueueMonitor> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider serviceProvider;
//        private readonly IMailSenderService mailSpooler;
        private readonly string queue = "";

        public MailMessageQueueMonitor(
            IMessaging<string> messaging,
            ILogger<MailMessageQueueMonitor> logger,
            IConfiguration configuration,
            IServiceProvider serviceProvider
            //IMailSenderService mailSpooler
            )
        {
            _messaging = messaging;
            _logger = logger;
            _configuration = configuration;
            this.serviceProvider = serviceProvider;
            //this.mailSpooler = mailSpooler;
            queue = _configuration[StaticConfiguration.CONST_MAILSERVICE_QUEUE] ?? "";
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
                _logger.LogInformation("Mail Spooler Listening on Queue: " + queue);
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
                var notification = System.Text.Json.JsonSerializer.Deserialize<CreateOrUpdateMailMessage>(message);

                using (var scope = serviceProvider.CreateScope())
                {
                    var mailSpooler = (IMailSenderService)scope.ServiceProvider.GetService(typeof(IMailSenderService));
                    await mailSpooler.Save(notification, UserProfile.SystemUser());
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
                _logger.LogInformation("Mail Spooler Stopped");
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
