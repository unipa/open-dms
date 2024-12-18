using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.Utilities;
using OpenDMS.Domain.Entities;

namespace OpenDMS.Core.Builders
{
    public class MessageBuilder : IMessageBuilder
    {
        private string _title = "";
        private string _body = "";
        private string _templateId = "";
        private string _templateTitle = "{{title}}";
        private string _templateBody = "{{body}}";
        public MessageBuilder()
        {

        }


        public MessageBuilder UpdateTags(string tagPrefix, object tagProperties)
        {
            _templateTitle = _templateTitle.Parse(tagProperties, tagPrefix);
            _templateBody = _templateBody.Parse(tagProperties, tagPrefix);
            return this;
        }


        public MessageBuilder SetTitle(string title)
        {
            if (!String.IsNullOrEmpty(title))
                _templateTitle = title;
            return this;
        }
        public MessageBuilder SetBody(string body)
        {
            if (!String.IsNullOrEmpty(body))
                _templateBody = body;
            return this;
        }

        public NotificationTemplate Build(string title, string body)
        {
            NotificationTemplate N = new NotificationTemplate();
            N.Title = _templateTitle.Replace("{{title}}", title);
            N.Body = _templateBody.Replace("{{body}}", body).Replace("{{description}}", body);
            return N;
        }
    }
}
