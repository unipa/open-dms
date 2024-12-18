
namespace OpenDMS.NotificationSpool.Core.Interfaces
{
    public interface INotificationWorker
    {
        public bool StartListenForNotifications();
        public bool StopListenForNotifications();

    }
}
