using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using OpenDMS.MailSpooler.Core.Interfaces;

namespace OpenDMS.Infrastructure.Services.Workers;

public class MailSenderWorker : IHostedService, IDisposable
{
    //        private IMailSenderService _mailSpooler = null;
    //        private IMailEntryService _mailEntryRepository = null;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MailSenderWorker> _logger;
    private readonly IConfiguration _configuration;

    private System.Timers.Timer _timer = null;

    private List<System.Timers.Timer> _timers = new List<System.Timers.Timer>();
    private int threads = 0;
    private bool IsActive = false;


    public MailSenderWorker(IServiceProvider serviceProvider, ILogger<MailSenderWorker> logger, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configuration = configuration;
        threads = 0;
        if (!int.TryParse(_configuration.GetSection("Mail:Sender:Threads").Value, out threads))
            threads = 1;

    }
    object LockObject = new object();
    private void StartThreads()
    {
        if (!IsActive) return;
        lock (LockObject)
        {
            int time = 0;
            if (!int.TryParse(_configuration.GetSection("Mail:Sender:Frequency").Value, out time))
                time = 60;

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
            _logger.LogInformation($"Mail Sender: Starting {(threads - _timers.Count())} new threads in {time}secs");
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
        _logger.LogInformation("Start Mail Sender");
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
        var maxTosent = 100;
        try
        {
            string WorkerId = Environment.MachineName + "_" + Guid.NewGuid().ToString();
            if (WorkerId.Length > 64) WorkerId = WorkerId.Substring(0, 64);

            string MaxToSentString = _configuration.GetSection("Mail:Sender:MaxSent").Value;
            string TimeOutString = _configuration.GetSection("Mail:Sender:TimeOut").Value;
            if (!int.TryParse(MaxToSentString, out maxTosent)) maxTosent = 100;
            if (!int.TryParse(TimeOutString, out timeOut)) timeOut = 5;
            using (var scopedService = _serviceProvider.CreateScope())
            {

                try
                {
                    IMailEntryService _mailEntryRepository = scopedService.ServiceProvider.GetRequiredService<IMailEntryService>();
                    if (_mailEntryRepository == null) throw new Exception("Impossibile creare un'istanza di MailRepository");
                    IMailSenderService _mailSpooler = scopedService.ServiceProvider.GetRequiredService<IMailSenderService>();
                    if (_mailSpooler == null) throw new Exception("Impossibile creare un'istanza di MailSender");
                    int sent = 0;
                    var lastQueueMail = await _mailEntryRepository.TakeNext(WorkerId, DateTime.UtcNow.AddMinutes(timeOut));
                    while (lastQueueMail != null)
                    {
                        _logger.LogDebug($"Invio #{lastQueueMail.Id} - Da: {lastQueueMail.InternalMailAddress} - A: {lastQueueMail.ExternalMailAddress} - Programmato per: {lastQueueMail.MessageDate?.ToString("dd/MM/yyyy HH:mm:ss")}");
                        try
                        {
                            var entry = await _mailSpooler.SendMail(lastQueueMail, UserProfile.SystemUser(), WorkerId);
                            sent++;

                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.Message, ex);
                        }
                        if (sent > maxTosent) break;
                        lastQueueMail = await _mailEntryRepository.TakeNext(WorkerId, DateTime.UtcNow.AddMinutes(timeOut));
                    }
                }
                catch (Exception ex)
                {
                    // errore nella lettura/scrittura del database
                    _logger.LogError("Mail Sender (DoWork): EXCEPTION:" + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Mail Sender (DoWork:Services): EXCEPTION:" + ex.Message);
            //Loggare l'errore
        }
        StartThreads();

    }


    public Task StopAsync(CancellationToken stoppingToken)
    {
        IsActive = false;
        _logger.LogInformation("Stop Mail Sender");
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