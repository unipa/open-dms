using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenDMS.Domain.Repositories;
using OpenDMS.Preservation.API.APIModels;
using OpenDMS.Preservation.Core.Interfaces;
using OpenDMS.Preservation.Core.Models;

namespace OpenDMS.Preservation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConservazioneController : Controller
    {
        private ILogger<ConservazioneController> _logger;
        private readonly IConfiguration _config;
        private IPreservationWorker _preservationWorker;
        private IDocumentRepository _docRepository;
        public ConservazioneController(ILogger<ConservazioneController> logger, IConfiguration configuration, IPreservationWorker worker, IDocumentRepository document)//, ICSManager manager)
        {
            _logger = logger;
            _config = configuration;
            _preservationWorker = worker;
            _docRepository = document;
        }

        //[HttpPost("PreservationWorkerTest")]
        //public async Task<IActionResult>  PreservationWorkerTest()
        //{
        //    _preservationWorker.Test();
        //    //var DA_Settings = _config.GetSection("CS:DA_Settings").Get<DA_Settings>();
        //    //if (DA_Settings == null)
        //    //{
        //    //    _logger.LogCritical("Metadati e DA non trovati");
        //    //    return BadRequest("Metadati e DA non trovati");
        //    //}
        //    //foreach (var typeConf in DA_Settings.TypeCons)
        //    //{
        //    //    //CREAZIONE PDV CORRISPONDENTE (POICHE' IL PDV  VIENE CONSIDERATO AD OGNI LOGIN)
        //    //    var gapDoc = typeConf.Gap;
        //    //    List<int> idDocPres = await _docRepository.GetDocumentsToPreserve(typeConf.DocType, Convert.ToInt32(gapDoc));
        //    //    if (idDocPres == null || idDocPres.Count == 0)
        //    //        continue;

        //    //    // var test = _preservationWorker.Preservation("", "", idDocPres, typeConf);
        //    //    var response = await _preservationWorker.Login(typeConf.UserResp);
        //    //    if (response != null && !string.IsNullOrEmpty(response.ldSessionId))
        //    //    {
        //    //        if (await _preservationWorker.Preservation(response.ldSessionId, response.pdv, idDocPres, typeConf))
        //    //            _logger.LogDebug($"Tutti i documenti della tipologia {typeConf.DocType} sono stati correttamente conservati");
        //    //        else
        //    //            _logger.LogError("Si sono verificati degli errori durante la conservazione. Verificare Log interni");

        //    //        if (await _preservationWorker.Logout(response.ldSessionId, typeConf.UserResp))
        //    //            _logger.LogDebug("Logout da provider effettuato");
        //    //    }
        //    //    else
        //    //        _logger.LogError("Impossibile ottenere ID Sessione. Verificare stato rete e riprovare");
        //    //}
        //    return new JsonResult(true);
        //}


        [HttpPost("ConservaListaDoc")]
        public async Task<IActionResult> ConservaListaDocumenti([FromBody] ConservaRequestObj preservationDocObj)
        {
            try
            {
                //CONTROLLI PRELIMINARI
                if (String.IsNullOrEmpty(preservationDocObj.DocType) || preservationDocObj.Gap == null)
                    return BadRequest("Valorizzare correttamente i campi");

                var da_Settings = _config.GetSection("CS:DA_Settings").Get<DA_Settings>();
                if (da_Settings == null)
                {
                    _logger.LogCritical("Metadati e DA non trovati");
                    return BadRequest("Metadati e DA non trovati");
                }

                var typeConf = da_Settings.TypeCons.Where(p => p.DocType == preservationDocObj.DocType).FirstOrDefault();
                if (typeConf == null)
                    return BadRequest("Impossibile trovare la tipologia da conservare nel settaggio");


                var gapDoc = preservationDocObj.Gap;
                List<int> idDocPres =await  _docRepository.GetDocumentsToPreserve(typeConf.DocType, Convert.ToInt32(gapDoc));
                if (idDocPres == null || idDocPres.Count == 0)
                    _logger.LogWarning($"Nessun id documento trovato per tipologia {typeConf.DocType} e GAP {gapDoc}");

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
            catch (Exception ex)
            {
                _logger.LogError($"Impossibile terminare il processo di conservazione dei documenti a causa della seguente eccezione: {ex.Message}");
                return BadRequest($"Impossibile terminare il processo di conservazione dei documenti a causa della seguente eccezione: {ex.Message}");
            }
            return new JsonResult("Operazione completata");
        }
    }
}
