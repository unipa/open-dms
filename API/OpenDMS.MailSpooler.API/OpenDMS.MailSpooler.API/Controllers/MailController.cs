using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using OpenDMS.MailSpooler.API.DTOs;
using OpenDMS.MailSpooler.Core.Interfaces;
using System.Reflection;

namespace OpenDMS.MailSpooler.Controllers
{
    [Route("api/mailspooler/[controller]")]
    [ApiController]
    [Authorize(Roles="user")]
    public class MailController : ControllerBase
    {
        private readonly ILoggedUserProfile loggedUserProfile;
        private readonly IHostApplicationLifetime hostApplication;
        private readonly ILogger<MailController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMailSenderService mailSender;
        private readonly IMailEntryService _mailSpooler;
        public MailController(
            IMailSenderService mailSender,
            IMailEntryService mailService,
            ILoggedUserProfile loggedUserProfile,
            IHostApplicationLifetime hostApplication,
            ILogger<MailController> logger, 
            IConfiguration configuration)
        {
            this.loggedUserProfile = loggedUserProfile;
            this.hostApplication = hostApplication;
            _logger = logger;
            _configuration = configuration;
            this.mailSender = mailSender;
            _mailSpooler = mailService;
        }

        [HttpPost("Send", Name = "Send")]
        public async Task<IActionResult> Send(CreateOrUpdateMailMessage mailMessage)
        {
            //TODO: Verificare se l'utente loggato può inviare la mail
            var u = loggedUserProfile.Get();
            mailMessage.FromUser = "";
            //      con l'indirizzo mittente indicato
            var mailEntry = await mailSender.Save(mailMessage, u);
            return Ok(mailEntry);

        }

        [HttpGet("SendProgress", Name = "SendProgress")]
        public async Task<SendProgressModel> Progress()
        {
            SendProgressModel P = new();
            P.Queued = await _mailSpooler.Count(MailStatus.Sending, MailDirection.Outbound);
            P.Sent = await _mailSpooler.Count(MailStatus.Sent, MailDirection.Outbound);
            P.Failed = await _mailSpooler.Count(MailStatus.Failed, MailDirection.Outbound);
            return P;
        }

        [HttpGet("Log")]
        public async Task<IActionResult> Log()
        {
            var fileName = Path.Combine(Environment.CurrentDirectory, "Log", $"OpenDMS.MailSpooler.API" + DateTime.Now.ToString("yyyyMMdd") + ".log");
            using (var m = new MemoryStream())
            {
                using (var reader = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    await reader.CopyToAsync(m);
                }
                var fileBytes = m.ToArray();
                return File(fileBytes, "application/octet-stream", fileName);
            }
        }

        [HttpGet("Configuration")]
        public async Task<string> Configuration()
        {
            string log = "";

            FieldInfo[] Fields = typeof(OpenDMS.Domain.Constants.StaticConfiguration).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var F in Fields)
            {
                var Value = (string)F.GetRawConstantValue();
                if (Value != null)
                {
                    if (_configuration[Value] == null)
                        log += ("## - Setting: " + Value);
                    else
                        log += ("OK - Setting: " + Value + " = " + _configuration[Value].ToString());
                    log += Environment.NewLine;
                }
            }
            return log;
        }

        [HttpGet("shutdown")]
        public void Shutdown(string pwd)
        {
            if (pwd == DateTime.Now.ToString("yyyyMMdd"))
            {
                hostApplication.StopApplication();
            }
        }


        [HttpPost("GetMailMessageList", Name = "GetMailMessageList")]
        public async Task<IActionResult> GetMailMessageList([FromBody] MailMessagesFilter mailMessageFilter)
        {
            var u = loggedUserProfile.Get();
            var listaMailMessages = new List<MailEntry>();
            try
            {
                if (mailMessageFilter == null || mailMessageFilter.mailDirection == null)
                    return BadRequest("Inserire almeno filtro per Direction Mail");
                listaMailMessages = await _mailSpooler.GetEntries(mailMessageFilter, u);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(listaMailMessages);
        }
    }
}
