using MessageBus.Interface;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using OpenDMS.Domain.Worker;

//namespace OpenDMS.MailSpooler.API.Worker
//{
//    public class MessageQueueMonitor : IMessageBusMonitor
//    {
//        private readonly IMessaging<string> _messaging;
//        private readonly ILogger<MessageQueueMonitor> _logger;
//        private readonly IConfiguration _configuration;
//        private readonly IMailSenderService mailSpooler;
//        private readonly string queue = "";

//        public MessageQueueMonitor(
//            IMessaging<string> messaging,
//            ILogger<MessageQueueMonitor> logger,
//            IConfiguration configuration,
//            IMailSenderService mailSpooler
//            )
//        {
//            _messaging = messaging;
//            _logger = logger;
//            _configuration = configuration;
//            this.mailSpooler = mailSpooler;
//            queue = _configuration[StaticConfiguration.CONST_MAILSERVICE_QUEUE] ?? "";
//        }



//        public bool StartListenForNewMailMessages()
//        {
//            bool Success = false;
//            try
//            {
//                _messaging.Listening(queue);
//                _messaging.OnMessageReceived += _messaging_OnMessageReceived;
//                Success = true;
//                _logger.LogInformation("Mail Spooler Listening on Queue: " + queue);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex.Message, ex);
//            }

//            return Success;
//        }

//        private async Task _messaging_OnMessageReceived(string message)
//        {
//            if (string.IsNullOrWhiteSpace(message)) return;

//            try
//            {
//                var notification = System.Text.Json.JsonSerializer.Deserialize<CreateOrUpdateMailMessage>(message);
//                await mailSpooler.Save(notification, UserProfile.SystemUser());
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex.Message, ex);
//            }
//        }


//        public bool StopListenForNewMailMessages()
//        {
//            bool Success = false;
//            try
//            {
//                _messaging.StopListening();
//                _logger.LogInformation("Mail Spooler Stopped");
//                Success = true;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex.Message, ex);
//            }

//            return Success;
//        }
//    }
//}
