using OpenDMS.NotificationSpool.Core.Interfaces;

namespace OpenDMS.NotificationSpool.Web.Services
{

    public class NotificationHostingService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<NotificationHostingService> _logger;
        private readonly IConfiguration _config;
        private readonly INotificationWorker _notificationWorker;
//        private Timer? _timer = null;

        public NotificationHostingService(ILogger<NotificationHostingService> logger,
            IConfiguration configuration,
            INotificationWorker notificationWorker,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _config = configuration;
            _notificationWorker = notificationWorker;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _notificationWorker.StartListenForNotifications();

            _logger.LogInformation("Start to control queue " + _config.GetSection("RabbitMQ_Notification_Queue").Value);
            //_timer = new Timer(DoWork, null, TimeSpan.Zero,
            //    TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        //private void DoWork(object? state)
        //{
        //}

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _notificationWorker.StopListenForNotifications();
            _logger.LogInformation("Notification Hosted Service is stopping.");
            //_timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            //_timer?.Dispose();
        }
    }
}