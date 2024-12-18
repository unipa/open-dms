using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Core.DTOs
{
    public class ProcessInstance_DTO
    {
        public string processInstanceId { get; set; } = "";
        public string processDefinitionId { get; set; } = "";
        public string processDefinitionKey { get; set; } = "";

        public DateTime startDate { get; set; }
        public Dictionary<string, string> variables { get; set; } = new();
        public List<ProcessIncident> incidents { get; set; } = new();
    }

    public class ProcessIncident
    {
        public string Id { get; set; } = "";
        public string jobKey { get; set; } = "";
        public string elementId { get; set; } = "";
        public string elementKey { get; set; } = "";
        public string errorType { get; set; } = "";
        public string errorMessage { get; set; } = "";
        public DateTime creationDate { get; set; }
        public DateTime lastUpdate { get; set; }
        public string status { get; set; }
    }


}
