using OpenDMS.Domain.Worker;
using OpenDMS.MailSpooler.Core.Interfaces;

namespace OpenDMS.MailSpooler.API.Service
{

    public class MailHostingService : IHostedService, IDisposable
    {
        private IMessageBusMonitor _mailWorker = null;
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<MailHostingService> _logger;

        public MailHostingService(IServiceProvider serviceProvider, ILogger<MailHostingService> logger)
        {
            this.serviceProvider = serviceProvider;
            this._logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            using (var scopedService = serviceProvider.CreateScope())
            {
                _mailWorker = scopedService.ServiceProvider.GetRequiredService<IMessageBusMonitor>();
                if (_mailWorker == null) throw new Exception("Impossibile creare un'istanza di MailMessageWorker");
            }
            _logger.LogInformation("Mail Spooler Starting");

            _mailWorker.StartListenForNewMailMessages();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Mail Spooler Stopping");
            if (_mailWorker != null)
                _mailWorker.StopListenForNewMailMessages();
            _mailWorker = null;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_mailWorker != null)
                _mailWorker.StopListenForNewMailMessages();
            _mailWorker = null;
        }
    }
}