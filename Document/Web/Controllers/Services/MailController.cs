using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using OpenDMS.MailSpooler.Core.Interfaces;

namespace OpenDMS.Web.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("internalapi/mailentry")]
    [ApiController]
    public class MailEntryController : ControllerBase
    {
        private readonly ILoggedUserProfile loggedUserProfile;
        private readonly IMailboxService mailboxService;
        private readonly IMailEntryService mailEntryService;
        private readonly IMailSenderService mailSender;
        private readonly ILogger<MailEntryController> _logger;
        private readonly IConfiguration _configuration;
        public MailEntryController(
            ILoggedUserProfile loggedUserProfile,
            IMailboxService mailboxService,
            IMailEntryService mailEntryService,
            IMailSenderService mailSender,
            ILogger<MailEntryController> logger, IConfiguration configuration)
        {
            this.loggedUserProfile = loggedUserProfile;
            this.mailboxService = mailboxService;
            this.mailEntryService = mailEntryService;
            this.mailSender = mailSender;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("Send", Name = "Send")]
        public async Task<IActionResult> Send(CreateOrUpdateMailMessage mailMessage)
        {
            //TODO: Verificare se l'utente loggato può inviare la mail
            //      con l'indirizzo mittente indicato
            var u = loggedUserProfile.Get();
            var Entry = await mailSender.Save(mailMessage, u);
            if (Entry != null && mailMessage.Status == MailStatus.Sending)
            {

                await mailSender.SendMail(Entry, u);
            }
            return Ok(Entry);

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
                listaMailMessages = await mailEntryService.GetEntries(mailMessageFilter, u);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(listaMailMessages);
        }

        [HttpGet("badge", Name = "badge")]
        public async Task<IActionResult> GetBadge()
        {
            var u = loggedUserProfile.Get();
            if (u == null) return Ok(0);
            MailMessagesFilter F = new MailMessagesFilter();
            F.mailStatus = MailStatus.Received;
            F.mailDirection = MailDirection.Inbound;
            var allMailboxes = (await mailboxService.GetAll(u));
            if (allMailboxes != null)
                F.mailboxId = allMailboxes.Select(m=>m.Id).ToArray();

            try
            {
                var count = await mailEntryService.Count(F, u);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok (0);
        }
    }
}
