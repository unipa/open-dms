using OpenDMS.Domain.Models;

namespace OpenDMS.Core.DTOs
{
    public class EventMessage : IEvent
    {
        public UserProfile UserInfo { get; set; }
        public string EventName { get; set; }
        public Dictionary<string, object> Variables { get; set; }

        public EventMessage(UserProfile userInfo, string applicationEvent, Dictionary<string, object> variables)
        {
            UserInfo = userInfo;
            this.EventName = applicationEvent;
            if (variables == null) variables = new Dictionary<string, object>();
            if (!variables.ContainsKey("UserId")) variables.Add("UserId", userInfo.userId);
            if (!variables.ContainsKey("UserProfile")) variables.Add("UserProfile", userInfo);
            if (!variables.ContainsKey("EventName")) variables.Add("EventName", applicationEvent);

            this.Variables = variables;

        }

        public EventMessage()
        {
        }

        public string Serialize()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }

        public static EventMessage Deserialize(string message)
        {
            return System.Text.Json.JsonSerializer.Deserialize<EventMessage>(message);
        }

        public T Get<T>(String Name)
        {
            if (this.Variables[Name].GetType() == typeof(System.Text.Json.JsonElement))
                return System.Text.Json.JsonSerializer.Deserialize<T>(this.Variables[Name].ToString());
            else 
                return (T)this.Variables[Name];
        }
    }
}
