using OpenDMS.Domain.Models;

namespace OpenDMS.Domain.Services
{
    public interface INotificationService
    {
        public Task SendMail (CreateOrUpdateMailMessage message);
    }
}
