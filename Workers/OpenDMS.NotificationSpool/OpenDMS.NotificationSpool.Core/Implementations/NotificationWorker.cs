using MessageBus.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenDMS.NotificationSpool.Core.Interfaces;
using OpenDMS.Domain.Services;
using OpenDMS.Domain.Models;

namespace OpenDMS.NotificationSpool.Core.Implementations
{
    public class NotificationWorker : INotificationWorker
    {
        private readonly IMessaging<string> _messaging;
        private readonly ILogger<NotificationWorker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMailSenderService mailSenderService;
        private readonly string queueName = "";

        public NotificationWorker(
            IMessaging<string> messaging,
            ILogger<NotificationWorker> logger,
            IConfiguration configuration,
            IMailSenderService mailSenderService)
        {
            _messaging = messaging;
            _logger = logger;
            _configuration = configuration;
            this.mailSenderService = mailSenderService;
            //_notificationRepository = notificationRepository;
            queueName = _configuration.GetSection("RabbitMQ_Notification_Queue").Value;

        }

        //public string GetSingleNotification()
        //{
        //    string result_test = null;
        //    try
        //    {
        //        string queue = _configuration.GetSection("RabbitMQ_Notification_Queue").Value;
        //        _messaging.GetSingleMessage(queue, result =>
        //        {
        //            result_test = result;
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message, ex);
        //    }
        //    return result_test;
        //}
        //public bool AddMessageToHistory(NotificationHistory notification)
        //{

        //    return (_notificationRepository.saveData(notification) > 0);
        //}

        //public bool PushNotificationMessage(NotificationHistory notification)
        //{
        //    bool messaggioInoltrato = false;
        //    try
        //    {
        //        string KCToken = _apiManager.getTokenFromKC();
        //        if (String.IsNullOrEmpty(KCToken))
        //        {
        //            _logger.LogError("Impossible to get KC Token");
        //            return false;
        //        }
        //        messaggioInoltrato =  _apiManager.sendToTaskListAsync(KCToken, notification).Result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message, ex);
        //    }
        //    return messaggioInoltrato;
        //}
        public bool StartListenForNotifications()
        {
            _messaging.Listening(queueName);
            _messaging.OnMessageReceived += _messaging_OnMessageReceived;
            return true;
        }
        public bool StopListenForNotifications()
        {
            _messaging.StopListening();
            return true;
        }
        private async Task _messaging_OnMessageReceived(string message)
        {

            //SALVO FILE NEL DB LOCALE
            //            NotificationHistory newNotificationHistory = new NotificationHistory();
            //            newNotificationHistory.MessageString = message;
            //            newNotificationHistory.DataRicezione = DateTime.Now;
            CreateOrUpdateMailMessage MailMessage = System.Text.Json.JsonSerializer.Deserialize< CreateOrUpdateMailMessage>(message);
            try
            {
                await mailSenderService.Save(MailMessage, UserProfile.SystemUser());
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex, "SendMail", queueName + ":" + message);
                throw;
            }



            //_notificationRepository.saveData(newNotificationHistory);
            //if (!PushNotificationMessage(newNotificationHistory))
            //    _logger.LogError("Impossible to Send Task with ID -->" + newNotificationHistory.Id);
            //else
                //_notificationRepository.deleteData(newNotificationHistory);
        }
        //public bool DeleteMessageHistory(NotificationHistory notification)
        //{
        //    return _notificationRepository.deleteData(notification);
        //}
    }
}
