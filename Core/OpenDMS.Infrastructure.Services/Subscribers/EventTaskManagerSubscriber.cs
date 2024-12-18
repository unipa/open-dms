using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;

namespace OpenDMS.Infrastructure.Services.Subscribers
{

    public class EventTaskManagerSubscriber : IEventSubscriber
    {



        private readonly ILogger<EventTaskManagerSubscriber> logger;
        private readonly IConfiguration config;
        private readonly IUserTaskService taskService;

        public EventTaskManagerSubscriber(ILogger<EventTaskManagerSubscriber> logger,
            IConfiguration config,
            IUserTaskService taskService)
        {
            this.logger = logger;
            this.config = config;
            this.taskService = taskService;
        }

        public async Task Invoke(IEvent ApplicationEvent)
        {
            if (ApplicationEvent.Variables.ContainsKey("Document"))
            {
                var V = ApplicationEvent.Variables;
                var eventId = ApplicationEvent.EventName;
                if (eventId == EventType.AddBiometricalSignature
                    || eventId == EventType.AddCheckSign
                    || eventId == EventType.AddDigitalSignature
                    || eventId == EventType.AddPreservationSignature
                    || eventId == EventType.AddRemoteDigitalSignature
                    || eventId == EventType.AddToFolder
                    || eventId == EventType.AddAttach
                    || eventId == EventType.AddLink
                    || eventId == EventType.AddUserSignature
                    || eventId == EventType.AddVersion
                    || eventId == EventType.AddRevision
                    || eventId == EventType.View
                    || eventId == EventType.Request
                    || eventId == EventType.Approval
                    || eventId == EventType.CheckIn
                    || eventId == EventType.CheckOut
                    || eventId == EventType.Preservation
                    || eventId == EventType.Print
                    || eventId == EventType.Protocol
                    || eventId == EventType.Publish
                    || eventId == EventType.Refuse
                    || eventId == EventType.RunProcess
                    )
                {
                    var doc = (DocumentInfo)ApplicationEvent.Get<DocumentInfo>("Document");

                    // recupero i task di tipo evento per l'evento e l'utente corrente
                    var tasks = await taskService.GetByEvent(doc.Id, eventId, ApplicationEvent.UserInfo);
                    foreach (var t in tasks)
                    {
                        await taskService.CaptureEvent(t, ApplicationEvent);
                    }
                }
            }
        }
    }
}
