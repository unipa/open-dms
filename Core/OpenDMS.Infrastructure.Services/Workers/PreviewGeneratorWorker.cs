using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using OpenDMS.Infrastructure.Services.BusinessLogic;

namespace OpenDMS.Infrastructure.Services.Workers;

public class PreviewGeneratorWorker : IHostedService, IDisposable
{
    //        private IMailSenderService _mailSpooler = null;
    //        private IMailEntryService _mailEntryRepository = null;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PreviewGeneratorWorker> _logger;
    private readonly IConfiguration _configuration;

    private System.Timers.Timer _timer = null;

    private List<System.Timers.Timer> _timers = new List<System.Timers.Timer>();
    private int threads = 0;
    private bool IsActive = false;


    public PreviewGeneratorWorker(
        IServiceProvider serviceProvider, 
        ILogger<PreviewGeneratorWorker> logger,
        IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configuration = configuration;
        threads = 0;
        if (!int.TryParse(_configuration.GetSection("PreviewGenerator:Threads").Value, out threads))
            threads = 1;

    }
    object LockObject = new object();
    private void StartThreads()
    {
        if (!IsActive) return;
        lock (LockObject)
        {
            int time = 0;
            if (!int.TryParse(_configuration.GetSection("PreviewGenerator:Frequency").Value, out time))
                time = 5;

            for (int i = _timers.Count - 1; i >= 0; i--)
            {
                var _timer = _timers[i];
                if (!_timer.Enabled)
                {
                    _timers.RemoveAt(i);
                    _timer.Dispose();
                    _timer = null;
                }
            }
            _logger.LogInformation("Preview Generator: Starting " + (threads - _timers.Count()) + " new threads");
            while (_timers.Count() < threads)
            {
                var _timer = new System.Timers.Timer(time * 1000);
                _timers.Add(_timer);
                _timer.Elapsed += _timer_Elapsed;
                _timer.AutoReset = false;
                _timer.Start();
            }
        }

    }
    private void StopThreads()
    {
        lock (LockObject)
        {
            while (_timers.Count > 0)
            {
                var _timer = _timers[_timers.Count - 1];
                _timers.RemoveAt(_timers.Count - 1);
                _timer.Stop();
                _timer = null;
            }
        }
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
            _logger.LogInformation("Starting Preview Generator");
            IsActive = true;

            StartThreads();
        return Task.CompletedTask;
    }

    private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        if (IsActive)
            DoWork(e);

    }


    private async void DoWork(object state)
    {
        var timeOut = 5;
        var maxTosent = 50;
        try
        {
            string WorkerId = Environment.MachineName + "_" + Guid.NewGuid().ToString();
            if (WorkerId.Length > 64) WorkerId = WorkerId.Substring(0, 64);

            string MaxToSentString = _configuration.GetSection("PreviewGenerator:MaxPreviews").Value;
            string TimeOutString = _configuration.GetSection("PreviewGenerator:TimeOut").Value;
            if (!int.TryParse(MaxToSentString, out maxTosent)) maxTosent = 50;
            if (!int.TryParse(TimeOutString, out timeOut)) timeOut = 5;
            using (var scopedService = _serviceProvider.CreateScope())
            {

                try
                {
                    IDistributedLocker _locker = scopedService.ServiceProvider.GetRequiredService<IDistributedLocker>();
                    if (_locker == null) throw new Exception("Impossibile creare un'istanza di IDistributedLocker");

                    IDocumentService _documentService = scopedService.ServiceProvider.GetRequiredService<IDocumentService>();
                    if (_documentService == null) throw new Exception("Impossibile creare un'istanza di IDocumentService");

                    IPreviewGenerator _previewGenerator = scopedService.ServiceProvider.GetRequiredService<IPreviewGenerator>();
                    if (_previewGenerator == null) throw new Exception("Impossibile creare un'istanza di IPreviewGenerator");
                    
                    int sent = 0;

                    foreach (var ImageId in _documentService.GetContentsToPreview().ToList())
                    {
                        if (_locker.Acquire("Preview", ImageId.ToString(), WorkerId, DateTime.UtcNow.AddMinutes(timeOut)))
                        {
                            await _documentService.UpdatePreviewStatus(ImageId, Domain.Enumerators.JobStatus.Running, UserProfile.SystemUser());
                            _logger.LogDebug($"Elaborazione Immagine #{ImageId}");
                            try
                            {
                                await _previewGenerator.Generate(ImageId);
                                sent++;
                                //await _documentService.UpdatePreviewStatus(ImageId, Domain.Enumerators.JobStatus.Completed, UserProfile.SystemUser());
                                _locker.Release("Preview", ImageId.ToString(), WorkerId);
                            }
                            catch (Exception ex)
                            {
                                await _documentService.UpdatePreviewStatus(ImageId, Domain.Enumerators.JobStatus.Failed, UserProfile.SystemUser());
                                _locker.Update("Preview", ImageId.ToString(), WorkerId, DateTime.UtcNow.AddMinutes(timeOut*2));
                                _logger.LogError(ex.Message, ex);
                            }
                            if (sent > maxTosent) break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // errore nella lettura/scrittura del database
                    _logger.LogError("Preview Generator (DoWork): EXCEPTION:" + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Preview Generator (DoWork:Services): EXCEPTION:" + ex.Message);
            //Loggare l'errore
        }
        StartThreads();

    }


    public Task StopAsync(CancellationToken stoppingToken)
    {
        IsActive = false;
        _logger.LogInformation("Stop Preview Generator");
        StopThreads();
        return Task.CompletedTask;
    }

    bool disposed = false;

    public void Dispose()
    {
        if (!disposed)
        {
            disposed = true;
            StopThreads();
        }
    }
}