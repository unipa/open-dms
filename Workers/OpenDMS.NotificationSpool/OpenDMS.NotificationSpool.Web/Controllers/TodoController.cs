using MessageBus.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Services;

namespace OpenDMS.NotificationSpool.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class TodoController : ControllerBase
    {
        private readonly ILogger<TodoController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMessaging<string> _messaging;
        private readonly ILoggedUserProfile userContext;
        private readonly IDocumentNotificationService _notification;

        public TodoController(ILogger<TodoController> logger, IConfiguration configuration, 
            IMessaging<string> messaging,
            ILoggedUserProfile userContext,
            IDocumentNotificationService notification)
        {
            _logger = logger;
            _configuration = configuration;
            _messaging = messaging;
            this.userContext = userContext;
            _notification = notification;
        }


        [HttpGet(Name = "ReadOnQueue")]
        public string Get()
        {
            string result_test = null;
            _messaging.GetSingleMessage("opendms_notifications", result =>
            {
                result_test = result;
            });
            return result_test;
        }

        /*[HttpPost(Name = "Post")]
        public bool Post([FromBody] NotificationHistory todoTest)
        {
            //_messaging.PushMessage(todoTest.MessageString, "opendms_notifications");
            //Notification notification_test = new ;
            //_notification.Notify()
            
            
            return true;
        }*/

        [HttpPost(Name = "WriteOnQueue")]
        public bool WriteOnQueue()
        {
            var u = userContext.Get();
            //CREO NOTIFICA E LA MANDO TRAMITE NOTIFICATION SERVICE
            OpenDMS.Domain.Models.DocumentNotification notifica = new Domain.Models.DocumentNotification();
            notifica.Sender = "Giuseppe";
            //notifica.Priority = 1;
            notifica.NotificationDate = DateTime.Now;
            notifica.Message = "Ciao, "+ Guid.NewGuid();
            notifica.CC =  new List<Domain.Models.ProfileInfo>() { new Domain.Models.ProfileInfo() { ProfileId = "1243", ProfileType = Domain.Enumerators.ProfileType.User  } };
            notifica.Documents = new List<int> { 1,2,3 };
            //notifica.FeedbackRequired = true;
            //notifica.Template = "123";

            //NOTIFICA CLASSICA
            _notification.Notify(notifica, u);
            //NOTIFICA ERRORE
            //_notification.NotifyError(1, "1234", "Errore nell'inserimento del documento");
            ////NOTIFICA ECCEZIONE
            //_notification.NotifyException(1, "5678", new Exception("Errore generico"));

            return true;
        }
    }
}
