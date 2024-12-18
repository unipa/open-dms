using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Domain.Models
{
    public interface IEvent
    {
        public UserProfile UserInfo { get; set; }
        public string EventName { get; set; }
        public Dictionary<string, Object> Variables { get; set; }

        string Serialize();
        //static IEvent Deserialize(string jsonObject)
        //{
        //    return (IEvent)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonObject);
        //}
        T Get<T>(String Name);

    }
}
