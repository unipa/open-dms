using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net.Codecrete.QrCodeGenerator;

namespace Web.Controllers.Services
{
    [Authorize]
    [ApiController]
    [Route("internalapi/qrcode")]

    public class QrCodeController : ControllerBase
    {
        /// <summary>
        /// Ritorna l'immagine QRCode di un codice
        /// </summary>
        /// <param name="code">Id del documento.</param>
        /// <returns>Restituisce una lista di oggetti DocumentPermissions</returns>
        [HttpGet("{code}")]
        public async Task<ActionResult> Get(string code)
        {
            var qr = QrCode.EncodeText(code, QrCode.Ecc.Medium);
            var svg = qr.ToBmpBitmap(4,7);//.ToSvgString(4);
            return File(svg,  "image/bmp", code+".bmp");
        }

    }
}
