using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RemoteSign.BL;
using RemoteSignInfocert.Models;

namespace RemoteSignInfocert.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SignController : ControllerBase
    {
        private readonly ILogger<SignController> _logger;
        private readonly SignService _sign;

        public SignController(
            ILogger<SignController> logger, 
            SignService sign)
        {
            _logger = logger;
            _sign = sign;
        }

        [HttpGet("{username}/{signType}")]
        public string CreateSignRoom(string username, SignTypes signType)
        {
            return _sign.CreateSignRoom(username, signType);
        }

        [HttpPost("{signRoom}/{labelCampoFirma}")]
        public async Task<bool> UploadFileWithSignField(string signRoom, IFormFile file, string labelCampoFirma = "")
        {
            return await _sign.AddFile(signRoom, file, labelCampoFirma);
        }

        [HttpPost("{signRoom}")]
        public async Task<bool> UploadFile(string signRoom, IFormFile file)
        {
            return await _sign.AddFile(signRoom, file, "");
        }



        [HttpDelete]
        public bool ClearSignRoom(string SignRoom, string? esito = "")
        {
            return _sign.ClearSignRoom(SignRoom, esito);
        }


        [HttpDelete]
        public bool DeleteSignroomDirectory(string SignRoom)
        {
            return _sign.DeleteSignroomDirectory(SignRoom);
        }


        [HttpDelete]
        public bool DeleteSingleFile(string SignRoom, string FileName)
        {
            try
            {
                return _sign.DeleteSingleFile(SignRoom, FileName);
            }
            catch
            {
                return false;
            }
        }



        [HttpGet("{SignRoom}")]
        public bool CheckDelivery(string SignRoom)
        {
            return _sign.CheckDelivery(SignRoom);
        }


        [HttpGet("{username}")]
        public bool CheckUserInfo(string username)
        {
            return _sign.CheckUserInfo(username);
        }



        [HttpGet("{SignRoom}/{UserName}")]
        public bool CheckSignRoom(string SignRoom, string UserName)
        {
            return _sign.CheckSignRoom(SignRoom, UserName);
        }

        [HttpGet("{SignRoom}")]
        public string CheckDeliveryResult(string SignRoom)
        {
            return _sign.CheckDeliveryResult(SignRoom);
        }


        [HttpGet("{SignRoom}")]
        public string CheckStatus(string SignRoom)
        {
            return _sign.CheckStatus(SignRoom);
        }



        [HttpGet("{SignRoom}")]
        public SignRoomModel GetSignRoom(string SignRoom)
        {
            return _sign.GetSignRoom(SignRoom);
        }



        [HttpGet("{SignRoom}")]
        public bool CheckSigned(string SignRoom)
        {
            return _sign.CheckSigned(SignRoom);
        }


        [HttpGet("{SignRoom}")]
        public FileContentResult DownloadSignedFile(string SignRoom)
        {
            //var dirname = Path.Combine(_signRoomBasePath, SignRoom);
            var sr = GetSignRoom(SignRoom);

            var data = _sign.GetSignedFile(SignRoom);
            var flname = _sign.GetSignedFileName(SignRoom);

            if (sr.NumeroFile > 1)
                Response.Headers.Add("IsZip", "true");
            Response.Headers.Add("FileName",Path.GetFileName(flname));
            Response.Headers.Add("SignRoom", JsonConvert.SerializeObject(sr));

            if (sr.SignType == SignTypes.Pades)
            {
                Response.Headers.Add("IsPades", "true");
            }

            return File(data, "Application/zip");
        }


        [HttpPost("{SignRoom}/{UserName}")]
        public async Task<IActionResult> SendOTP(string SignRoom, string UserName)
        {
            try
            {
                var s = await _sign.SendOTP(SignRoom, UserName);
                if (string.IsNullOrEmpty(s))
                    return Ok();
                else
                    return BadRequest(s);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost("{SignRoom}/{UserName}/{OTP}/{PIN}")]
        public async Task<IActionResult> StartSign(string SignRoom, string UserName, string OTP, string PIN)
        {
            try
            {
                BackgroundJob.Enqueue(() => _sign.SignFile(SignRoom, PIN, OTP));
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"StartSign({SignRoom}, {UserName}, ..., {OTP})");
                return BadRequest(ex);
            }
        }


    }
}
