using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;

namespace OpenDMS.Domain.Services;

public interface IEventManager
{

    Task<bool> Publish(IEvent ApplicationEvent);
    //Task<bool> Subscribe(Action<IEvent> service);
    //Task<bool> Unsubscribe(Action<IEvent> service);

}