using OpenDMS.Domain.Models;

namespace OpenDMS.Infrastructure.Services.BusinessLogic
{
    public interface IDocumentWorkflowEngine
    {
        Task HandleMessage(IEvent AppEvent);
    }
}