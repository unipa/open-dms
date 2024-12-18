using Newtonsoft.Json;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Repositories;
using OpenDMS.Preservation.Core.Interfaces;
using OpenDMS.Preservation.Core.Models;

namespace OpenDMS.Preservation.API
{

    public class PreservationHostingService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<PreservationHostingService> _logger;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _serviceProvider;
        private IDocumentRepository _docRepository = null;
        private IPreservationWorker _preservationWorker = null;


        private Timer? _timer = null;

        public PreservationHostingService
            (
            ILogger<PreservationHostingService> logger,
            IConfiguration configuration,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _config = configuration;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {

            _logger.LogInformation("Start to control Preservation Docs ");
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(120));

            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            using (var scopedService = _serviceProvider.CreateScope())
            {
                 _docRepository = scopedService.ServiceProvider.GetRequiredService<IDocumentRepository>();
                _preservationWorker = scopedService.ServiceProvider.GetRequiredService<IPreservationWorker>();

                var da_Settings = _config.GetSection("CS:DA_Settings").Get<DA_Settings>();
                _logger.LogInformation(JsonConvert.SerializeObject(da_Settings));
                if (da_Settings == null)
                {
                    _logger.LogCritical("Metadati e DA non trovati");
                    return;
                }
                foreach (var typeConf in da_Settings.TypeCons)
                {
                    //CREAZIONE PDV CORRISPONDENTE (POICHE' IL PDV  VIENE CONSIDERATO AD OGNI LOGIN)
                    var gapDoc = typeConf.Gap;
                    List<int> idDocPres = await _docRepository.GetDocumentsToPreserve(typeConf.DocType, Convert.ToInt32(gapDoc));
                    if (idDocPres == null || idDocPres.Count == 0)
                        continue;

                    var response = await _preservationWorker.Login(typeConf.UserResp);
                    if (response != null && !string.IsNullOrEmpty(response.ldSessionId))
                    {
                        if (await _preservationWorker.Preservation(response.ldSessionId, response.pdv, idDocPres, typeConf))
                            _logger.LogDebug($"Tutti i documenti della tipologia {typeConf.DocType} sono stati correttamente conservati");
                        else
                            _logger.LogError("Si sono verificati degli errori durante la conservazione. Verificare Log interni");

                        if (await _preservationWorker.Logout(response.ldSessionId, typeConf.UserResp))
                            _logger.LogDebug("Logout da provider effettuato");
                    }
                    else
                        _logger.LogError("Impossibile ottenere ID Sessione. Verificare stato rete e riprovare");
                }
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Preservation Hosted Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}