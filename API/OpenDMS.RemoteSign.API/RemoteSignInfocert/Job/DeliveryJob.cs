using RemoteSign.BL;

namespace RemoteSignInfocert.Job
{
    public class DeliveryJob
    {
        private readonly ILogger<DeliveryJob> _logger;
        private readonly SignService _signService;

 
        public DeliveryJob(ILogger<DeliveryJob> logger, SignService SignService)
        {
            _logger = logger;
            _signService = SignService;
        }

        public async Task CheckForExpiredSignRoom()
        {
                await _signService.CheckForExpiredSignRoom();
        }

        public async Task CheckForDelivery()
        {
            await _signService.CheckForDelivery();
        }

    }
}
