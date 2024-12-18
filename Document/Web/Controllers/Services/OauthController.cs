using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Repositories;
using OpenDMS.MailSpooler.Core;
using OpenDMS.MailSpooler.Core.Interfaces;

namespace Web.Controllers.Services
{

    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Route("Services/Oauth")]
    public class OauthController : ControllerBase
    {

        private readonly ILogger<OauthController> _logger;
        private readonly IAuthenticatorFactory authenticatorFactory;
        private readonly IConfiguration _config;
        private readonly IMailServerRepository _mailServer;
        private readonly IMailboxRepository _mailBox;


        public OauthController(
            ILogger<OauthController> logger,
            IAuthenticatorFactory authenticatorFactory,
            IMailServerRepository mailServerController,
            IMailboxRepository mailboxController,
            IConfiguration config = null)
        {
            _logger = logger;
            this.authenticatorFactory = authenticatorFactory;
            _config = config;
            _mailServer = mailServerController;
            _mailBox = mailboxController;
        }


        [AllowAnonymous]
        [HttpGet("{Index}")]
        public async Task<IActionResult> _Index(string? code, string? state)
        {
            return await Index(code, state);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index(string? code, string? state)
        {
            //RICEVO ELEMENTI
            try
            {
                //                string code = getParamsFromRequest(Request, "code");
                //                string state = getParamsFromRequest(Request, "state");
                byte[] data = Convert.FromBase64String(state);
                string stateString = System.Text.Encoding.UTF8.GetString(data);

                //                Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(System.Text.Json.JsonSerializer.Serialize(stato)));

                //JObject oggetto = JObject.Parse(state);

                if (!String.IsNullOrEmpty(code) && !String.IsNullOrEmpty(stateString))
                {
                    //                    OAUTHState state = Newtonsoft.Json.JsonConvert.DeserializeObject<OAUTHState>(stateString);

                    var jsonState = JsonConvert.DeserializeObject<OAUTHState>(stateString);

                    var mailBox = await _mailBox.GetById(jsonState.MailboxID);
                    if (mailBox != null)
                    {
                        //GESTIONE TOKEN
                        //var mailServerRes = await _mailServer.get(Convert.ToInt32(data["MailServerId"].ToString()));
                        //var mailServer = (MailServer)((OkObjectResult)mailServerRes.Result).Value;
                        var tokenObj = await GetOAuthAccessData(mailBox, code);

                        //AGGIUNGO A DATA I TOKEN OTTENUTI
                        if (tokenObj != null && tokenObj.Token != null && tokenObj.eMail.Equals(jsonState.eMail, StringComparison.InvariantCultureIgnoreCase))
                        {
                            //var mailboxRes = await _mailBox.GetByAddress(jsonState.eMail);
                            //var mailBox = (Mailbox)((OkObjectResult)mailboxRes.Result).Value;

                            //var mailBox = (Mailbox) _mailBox.GetByAddress(jsonState.eMail).Result;
                            mailBox.TokenId = tokenObj.Token;
                            if (tokenObj.RefreshToken != null)
                                mailBox.RefreshToken = tokenObj.RefreshToken;
                            mailBox.Validated = true;
                            mailBox.LastCredentialUpdate = DateTime.UtcNow;
                            mailBox.UserId = jsonState.UserName;
                            await _mailBox.Update(mailBox);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return RedirectToAction("Email", "Customize");
        }


        [HttpGet("GetURL/{Address}")]
        public async Task<ActionResult> GetURL(int Address)
        {
            try
            {
                var mailbox = await _mailBox.GetById(Address);
                var mailServer = mailbox.MailServer;
                if (mailServer.AuthenticationType == AuthenticationType.None || mailServer.AuthenticationType == AuthenticationType.UserCredentials)
                    return Ok("");

                //var mailbox = new Mailbox()
                //{
                //    MailServer = mailServer,
                //    MailAddress = Address,
                //    UserId = HttpContext.User.Identity.Name
                //    // RedirectURI = "https://localhost:7001/Services/Oauth/Index"
                //};

                var _auth = authenticatorFactory.GetAuthenticator(mailServer.AuthenticationType);
                return Ok(_auth.GetOAUTH_URL(
                new OAUTHState()
                {
                    MailboxID = Address,
                    Mailtype = mailServer.MailType,
                    OauthType = mailServer.AuthenticationType,
                    eMail = mailbox.MailAddress,
                    UserName = HttpContext.User.Identity.Name
                }
                , mailbox));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }






        private string getParamsFromRequest(HttpRequest request, string property)
        {
            string value = "";
            try
            {
                value = Request.Query[property].ToString();
                if (String.IsNullOrEmpty(value))
                    value = Request.Form[property].ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return value;
        }



        private async Task<OAUTHToken> GetOAuthAccessData(Mailbox mailBox, string code)
        {
            var _auth = authenticatorFactory.GetAuthenticator(mailBox.MailServer.AuthenticationType);
            var token = _auth.AcquireCode(code, mailBox);
            if (token != null)
                return (token);
            else
                return null;
        }




    }
}
