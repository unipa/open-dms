using OpenDMS.Domain.Worker;

namespace OpenDMS.Workflow.API.Service;

public class ProcessHostingService : IHostedService, IDisposable
{
    private IMessageBusMonitor _mailWorker = null;
    private readonly IServiceProvider serviceProvider;
    private readonly ILogger<ProcessHostingService> _logger;

    public ProcessHostingService(IServiceProvider serviceProvider, ILogger<ProcessHostingService> logger)
    {
        this.serviceProvider = serviceProvider;
        this._logger = logger;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        using (var scopedService = serviceProvider.CreateScope())
        {
            _mailWorker = scopedService.ServiceProvider.GetRequiredService<IMessageBusMonitor>();
            if (_mailWorker == null) throw new Exception("Impossibile creare un'istanza di ProcessHostingService");
        }
        _logger.LogInformation("ProcessHostingService Starting");

        _mailWorker.StartListenForNewMailMessages();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ProcessHostingService Stopping");
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