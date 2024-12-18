using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Models;
using OpenDMS.MailSpooler.Core.Interfaces;

namespace OpenDMS.Infrastructure.Services.Workers
{
    public class MailReceiverWorker : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MailReceiverWorker> _logger;
        private readonly IConfiguration _configuration;
        private List<System.Timers.Timer> _timers = new List<System.Timers.Timer>();
        private int threads = 0;
        private bool IsActive = false;

        public MailReceiverWorker(IServiceProvider serviceProvider, ILogger<MailReceiverWorker> logger, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _configuration = configuration;
            threads = 0;
            if (!int.TryParse(_configuration.GetSection("Mail:Reader:Threads").Value, out threads))
                threads = 1;
        }

        object LockObject = new object();

        private void StartThreads()
        {
            if (!IsActive) return;
            lock (LockObject)
            {
                int time = 0;
                if (!int.TryParse(_configuration.GetSection("Mail:Reader:Frequency").Value, out time))
                    time = 5*60;

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
                _logger.LogInformation($"Mail Reader: Starting {(threads - _timers.Count())} new threads in {time}secs");
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
            _logger.LogInformation("Start Mail Reader");
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
            var maxToRead = 100;
            try
            {
                string WorkerId = Environment.MachineName + "_" + Guid.NewGuid().ToString();
                if (WorkerId.Length > 64) WorkerId = WorkerId.Substring(0, 64);
                string MaxToSentString = _configuration.GetSection("Mail:Reader:MaxRead").Value;
                string TimeOutString = _configuration.GetSection("Mail:Reader:TimeOut").Value;

                if (!int.TryParse(MaxToSentString, out maxToRead)) maxToRead = 100;
                if (!int.TryParse(TimeOutString, out timeOut)) timeOut = 5;

                using (var scopedService = _serviceProvider.CreateScope())
                {
                    try
                    {
                        IMailboxService _mailboxService = scopedService.ServiceProvider.GetRequiredService<IMailboxService>();
                        if (_mailboxService == null) throw new Exception("Impossibile creare un'istanza di MailboxService");
                        IMailEntryService _mailEntryRepository = scopedService.ServiceProvider.GetRequiredService<IMailEntryService>();
                        if (_mailEntryRepository == null) throw new Exception("Impossibile creare un'istanza di MailRepository");
                        IMailReaderService _mailReader = scopedService.ServiceProvider.GetRequiredService<IMailReaderService>();
                        if (_mailReader == null) throw new Exception("Impossibile creare un'istanza di MailReader");

                        int read = 0;
                        try
                        {
                            var mailbox = IsActive ? await _mailboxService.TakeNext(WorkerId, DateTime.UtcNow.AddMinutes(timeOut)) : null;
                            while (mailbox != null)
                            {
                                _logger.LogDebug($"Lettura casella #{mailbox.Id} - {mailbox.MailAddress}");
                                try
                                {
                                    var entry = await _mailReader.Read(mailbox, UserProfile.SystemUser());
                                    read++;
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex.Message, ex);
                                }
                                finally
                                {
                                    await _mailboxService.Release(mailbox);
                                    if (read < maxToRead && IsActive)
                                        mailbox = await _mailboxService.TakeNext(WorkerId, DateTime.UtcNow.AddMinutes(timeOut));
                                    else
                                        mailbox = null;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // errore nella lettura/scrittura del database
                            _logger.LogError("Mail Reader (DoWork): EXCEPTION:" + ex.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Mail Reader (DoWork:Services): EXCEPTION:" + ex.Message);
                        //Loggare l'errore
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            StartThreads();

        }


        public Task StopAsync(CancellationToken stoppingToken)
        {
            IsActive = false;
            _logger.LogInformation("Stop Mail Reader");
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
}