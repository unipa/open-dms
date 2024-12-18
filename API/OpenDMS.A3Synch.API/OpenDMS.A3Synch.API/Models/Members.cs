using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace A3Synch.Models
{
    public class Members
    {
        public string username { get; set; }
        public string nome { get; set; }
        public string id_ruolo { get; set; }
        public string cognome { get; set; }

        public string sesso { get; set; }

        public string codice_fiscale { get; set; }

        public DateTime inizio_validita { get; set; }

        public DateTime fine_validita { get; set; }

        [JsonProperty("role")]
        public JObject Role { get; set; }
        //public string id_ruolo => Role?["id_ruolo"]?.ToString();
        public string denominazione => Role?["denominazione"]?.ToString();
        public string? UserGroupId { get; set; }
    }

}