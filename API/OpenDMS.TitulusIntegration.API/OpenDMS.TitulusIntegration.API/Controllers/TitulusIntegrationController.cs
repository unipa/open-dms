using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.TitulusIntegration.API.BL.Interfacce;
using OpenDMS.TitulusIntegration.API.Models;

namespace OpenDMS.TitulusIntegration.API.Controllers
{
  //  [Authorize]
    [ApiController]
    [Route("api/titulus/[Action]")]
    public class TitulusIntegrationController : ControllerBase
    {
        private readonly ILogger<TitulusIntegrationController> _logger;
        private readonly ITitulusBL _bl;
        private readonly IConfiguration _config;

        public TitulusIntegrationController(ILogger<TitulusIntegrationController> logger, ITitulusBL bl, IConfiguration config)
        {
            _logger = logger;
            _bl = bl;
            _config = config;
        }

        [HttpGet(Name = "GetDocumentFromProtocol")]
        public IActionResult GetDocumentFromProtocol(string numero_protocollo)
        {
            try
            {
                string stringResponse = _bl.GetDocumentFromProtocol(numero_protocollo);

                var contentResult = new ContentResult();
                contentResult.Content = stringResponse;
                contentResult.ContentType = "application/json";

                return contentResult;
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore nel controller, nel metodo GetDocumentFromProtocol(), errore " + ex.Message);
                throw new Exception("Errore nel controller, nel metodo GetDocumentFromProtocol(), errore " + ex.Message);
            }
        }

        [HttpPost(Name = "GetFileBase64")]
        public IActionResult GetFileBase64(string fileID)
        {
            try
            {
                string stringResponse = _bl.GetFileBase64(fileID);

                var contentResult = new ContentResult();
                contentResult.Content = stringResponse;
                contentResult.ContentType = "application/text";

                return contentResult;
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore nel controller, nel metodo GetFileBase64(), errore " + ex.Message);
                throw new Exception("Errore nel controller, nel metodo GetFileBase64(), errore " + ex.Message);
            }
        }


        [HttpPost(Name = "CreateNewDocument")]
        public IActionResult CreateNewDocument(NewDocument doc, bool draft)
        {
            try
            {
                string stringResponse = _bl.CreateNewDocument(doc,draft);

                var contentResult = new ContentResult();
                contentResult.Content = stringResponse;
                contentResult.ContentType = "application/json";

                return contentResult;
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore nel controller, nel metodo ArchiveDocumentInDraft(), errore " + ex.Message);
                throw new Exception("Errore nel controller, nel metodo ArchiveDocumentInDraft(), errore " + ex.Message);
            }
        }
    }
}