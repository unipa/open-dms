using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Models;

namespace OpenDMS.Domain.Services;
public interface IDocumentNotificationService
{

    public Task Notify (DocumentNotification notification, UserProfile userInfo);
//    public Task NotifyError(int DocumentId, string userId, string Message);
    public Task NotifyException( UserProfile userInfo, Exception ex, Document d = null);


}