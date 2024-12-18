using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;

namespace OpenDMS.Domain.Services;

public interface IEventSubscriber
{
    Task Invoke(IEvent ApplicationEvent);

}