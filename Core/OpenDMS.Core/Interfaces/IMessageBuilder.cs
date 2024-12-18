using OpenDMS.Core.Builders;
using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Entities;

namespace OpenDMS.Core.Interfaces
{
    public interface IMessageBuilder
    {
        NotificationTemplate Build(string title, string body);
        MessageBuilder SetBody(string body);

        MessageBuilder SetTitle(string title);
        MessageBuilder UpdateTags(string tagPrefix, object tagProperties);
    }
}