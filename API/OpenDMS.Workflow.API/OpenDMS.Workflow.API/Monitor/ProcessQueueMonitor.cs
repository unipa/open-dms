//using MessageBus.Interface;
//using OpenDMS.Domain.Constants;
//using OpenDMS.Domain.Models;
//using OpenDMS.Domain.Worker;
//using OpenDMS.Infrastructure.Services.BusinessLogic;

//namespace OpenDMS.Workflow.API.Monitor
//{
//    public class ProcessQueueMonitor : IMessageBusMonitor
//    {
//        private readonly IMessaging<string> _messaging;
//        private readonly ILogger<ProcessQueueMonitor> _logger;
//        private readonly IConfiguration _configuration;
//        private readonly IDocumentWorkflowEngine wf;
//        private readonly string queue = "";

//        public ProcessQueueMonitor(
//            IMessaging<string> messaging,
//            ILogger<ProcessQueueMonitor> logger,
//            IConfiguration configuration,
//            IDocumentWorkflowEngine wf
//            )
//        {
//            _messaging = messaging;
//            _logger = logger;
//            _configuration = configuration;
//            this.wf = wf;
//            queue = _configuration[StaticConfiguration.CONST_BPMSERVICE_QUEUE] ?? "";
//        }



//        public bool StartListenForNewMailMessages()
//        {
//            bool Success = false;
//            try
//            {
//                _messaging.Listening(queue);
//                _messaging.OnMessageReceived += _messaging_OnMessageReceived;
//                Success = true;
//                _logger.LogInformation("Process Monitor Listening on Queue: " + queue);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex.Message, ex);
//            }

//            return Success;
//        }

//        private async Task _messaging_OnMessageReceived(string message)
//        {
//            if (string.IsNullOrWhiteSpace(message)) return;

//            try
//            {

//                _logger.LogWarning("StartProcess", "<EmptyQueue>:" + message);
//                var ApplicationEvent = System.Text.Json.JsonSerializer.Deserialize<IEvent>(message);
//                await wf.HandleMessage(ApplicationEvent);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex.Message, ex);
//            }
//        }


//        public bool StopListenForNewMailMessages()
//        {
//            bool Success = false;
//            try
//            {
//                _messaging.StopListening();
//                _logger.LogInformation("Process Monitor Stopped");
//                Success = true;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex.Message, ex);
//            }

//            return Success;
//        }
//    }
//}
