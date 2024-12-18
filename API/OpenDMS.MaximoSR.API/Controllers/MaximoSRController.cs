using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.MaximoSR.API.BL.Interfacce;
using OpenDMS.MaximoSR.API.Models;

namespace OpenDMS.MaximoSR.API.Controllers
{
    [ApiController]
    [Route("/api/maximosr/")]
    public class MaximoSRController : ControllerBase
    {
        

        private readonly ILogger<MaximoSRController> _logger;

        private readonly IMaximoSRBL _bl;

        public MaximoSRController(ILogger<MaximoSRController> logger, IMaximoSRBL bl)
        {
            _logger = logger;
            _bl = bl;
        }

        /// <summary>
        /// Questa API inizia avvia il processo bpmn per aprire un ticket su Maximo
        /// L'API per essere lanciata necessita che l'utente sia autenticato e che abbia il ruolo student.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAssetListFromMaximo")]
        public async Task<IActionResult> GetAssetListFromMaximo()
        { 
            
            try
            {
                List<ASSET> asset = await _bl.GetAssetListFromMaximo();
                return Ok(asset);
            }

            catch (Exception ex)
            {
                _logger.LogError("Errore durante l'ottenimento degli asset da Maximo, erorre: " + ex.Message);
                return BadRequest("Si è verificato un errore durante l'ottenimento degli asset da Maximo");
            }
        }
    }
}
