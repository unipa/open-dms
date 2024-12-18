using DocumentFormat.OpenXml.Wordprocessing;
using java.nio.channels;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Models;
using org.apache.poi.ss.formula.functions;

namespace Web.Pages.Shared.Components.BPMN_Incident
{
    public class BPMN_Incident : ViewComponent
    {
        private readonly IDocumentService documentService;
        private readonly ILoggedUserProfile userContext;
        private readonly IConfiguration config;


        public class ProcessVariable
        {
            public string VarName { get; set; }
            public string VarValue { get; set; }
        }


        public class ProcessIncident
        {
            public long ElementInstanceKey { get; set; }
            public string jobKey { get; set; }
            public string TaskName { get; set; }
            public string ErrorMessage { get; set; }
            public string ErrorType { get; set; }

        }
        public class ProcessTask
        {
            public long ElementInstanceKey { get; set; }
            public string TaskName { get; set; }
            public string TaskStatus { get; set; }
            public string TimeStamp { get; set; }

        }

        public class ProcessInstanceInfo
        {
            public int ProcessSchemaId { get; set; }
            public long ProcessInstanceId { get; set; }
            public string BPMNProcessId { get; set; }
            public string ProcessName { get; set; }
            public string Version { get; set; }
            //public string StartDate { get; set; }
            //public string EndDate { get; set; }
            //public string ExitCode { get; set; }
            //public string ErrorMessage { get; set; }

            public List<ProcessIncident> Incidents { get; set; }
            public List<ProcessTask> Tasks { get; set; }
            public List<ProcessVariable> Variables { get; set; }
        }

        protected class Process_Instance_Response_Value
        {
            public string bpmnProcessId { get; set; }
            public long processDefinitionKey { get; set; }
            public long processInstanceKey { get; set; }
            public string version { get; set; }
        }
        protected class Process_Instance_Response
        {
            public Process_Instance_Response_Value value { get; set; }

        }



        protected class Process_Response_Value
        {
            public string bpmnProcessId { get; set; }
            public long processDefinitionKey { get; set; }
            public string resourceName { get; set; }
            public string version { get; set; }
            public string resource { get; set; }
        }
        protected class Process_Response
        {
            public Process_Response_Value value { get; set; }
        }

        protected class Incident_Response_Value
        {
            public string errorResponse { get; set; }
            public string elementId { get; set; }
            public long elementInstanceKey { get; set; }
            public string errorType { get; set; }
            public long processDefinitionKey { get; set; }
        }
        protected class Incident_Response
        {
            public Incident_Response_Value value { get; set; }
        }

        protected class Task_Response_Value
        {
            public string bpmnElementType { get; set; }
            public string bpmnEventType { get; set; }
        }
        protected class Task_Response
        {
            public Task_Response_Value value { get; set; }
            public string intent { get; set; }
            public long timestamp { get; set; }
        }


        protected class Variable_Response_Value
        {
            public string name { get; set; }
            public string value { get; set; }
        }
        protected class Variable_Response
        {
            public Variable_Response_Value value { get; set; }
        }


        public BPMN_Incident(
            IDocumentService documentService,
            ILoggedUserProfile userContext,
            IConfiguration config)
        {
            this.documentService = documentService;
            this.userContext = userContext;
            this.config = config;
        }

        public async Task<List<ProcessInstanceInfo>> GetInstances(string bpmnProcessId, bool completed=false)
        {
            List<ProcessInstanceInfo> instances = new List<ProcessInstanceInfo>();
            var elasticsearchUrl = config["Endpoint:ElasticSearch"];
            if (string.IsNullOrEmpty(elasticsearchUrl)) elasticsearchUrl = "http://localhost:9200";
            HttpClient client = new HttpClient();

            var content = new StringContent(@"
{
  ""query"": {
    ""bool"": {
      ""must"": [
        {
          ""match"": {
            ""value.bpmnProcessId"": """ + bpmnProcessId + @"""
          }
        },
        {
          ""match"": {
            ""value.bpmnProcessId"": ""PROCESS""
          }
        },
        {
          ""match"": {
            ""intent"": """+ (!completed ? "ELEMENT_ACTIVATED" : "ELEMENT_TERMINATED") +@"""
          }
        }
      ]
    }
  },
  ""size"": 255,
  ""from"": 0,
  ""sort"": []
}
");
            var response = await client.PostAsync(elasticsearchUrl + "/zeebe-record-process-instance/_search", content);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                String jsonResult = await response.Content.ReadAsStringAsync();
                var result = System.Text.Json.JsonSerializer.Deserialize<List<Process_Instance_Response>>(jsonResult, new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                foreach (var r in result)
                {
                    ProcessInstanceInfo pi = new ProcessInstanceInfo();
                    pi.Version = r.value.version;
                    pi.BPMNProcessId = r.value.bpmnProcessId;
                    pi.ProcessInstanceId = r.value.processDefinitionKey;
                    instances.Add(pi);
                }
            };
            return instances;
        }
        public async Task<ProcessInstanceInfo> GetInstance(long processInstance)
        {

            var elasticsearchUrl = config["Endpoint:ElasticSearch"];
            if (string.IsNullOrEmpty(elasticsearchUrl)) elasticsearchUrl = "http://localhost:9200";
            HttpClient client = new HttpClient();

            var content = new StringContent(@"
{
  ""query"": {
    ""bool"": {
      ""must"": [
        {
          ""match"": {
            ""value.processInstanceKey"": " + processInstance + @"
          }
        }
      ]
    }
  },
  ""size"": 255,
  ""from"": 0,
  ""sort"": []
}
");
            ProcessInstanceInfo pi = new ProcessInstanceInfo();
            var response = await client.PostAsync(elasticsearchUrl + "/zeebe-record-process/_search", content);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                String jsonResult = await response.Content.ReadAsStringAsync();
                var result = System.Text.Json.JsonSerializer.Deserialize<List<Process_Response>>(jsonResult, new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                foreach (var r in result)
                {
                    pi.Version = r.value.version;
                    pi.BPMNProcessId = r.value.bpmnProcessId;
                    pi.ProcessName = r.value.resourceName;
                    pi.ProcessInstanceId = r.value.processDefinitionKey;
                }
            };
            return pi;
        }
        public async Task<List<ProcessVariable>> GetVariables (long processInstance)
        {

            var elasticsearchUrl = config["Endpoint:ElasticSearch"];
            if (string.IsNullOrEmpty(elasticsearchUrl)) elasticsearchUrl = "http://localhost:9200";
            HttpClient client = new HttpClient();

            var content = new StringContent(@"
{
  ""query"": {
    ""bool"": {
      ""must"": [
        {
          ""match"": {
            ""value.processInstanceKey"": " + processInstance + @"
          }
        }
      ]
    }
  },
  ""size"": 255,
  ""from"": 0,
  ""sort"": []
}
");
            List<ProcessVariable> variables = new List<ProcessVariable>();
            var response = await client.PostAsync(elasticsearchUrl + "/zeebe-record-variable/_search", content);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                String jsonResult = await response.Content.ReadAsStringAsync();
                var result = System.Text.Json.JsonSerializer.Deserialize<List<Variable_Response>>(jsonResult, new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                foreach (var r in result)
                {
                    ProcessVariable pt = new ProcessVariable();
                    pt.VarValue = r.value.name;
                    pt.VarValue = r.value.value;
                    variables.Add(pt);
                }
            };
            return variables;
        }
        public async Task<List<ProcessIncident>> GetIncident(long processInstance)
        {

            var elasticsearchUrl = config["Endpoint:ElasticSearch"];
            if (string.IsNullOrEmpty(elasticsearchUrl)) elasticsearchUrl = "http://localhost:9200";
            HttpClient client = new HttpClient();

            var content = new StringContent(@"
{
  ""query"": {
    ""bool"": {
      ""must"": [
        {
          ""match"": {
            ""value.processInstanceKey"": " + processInstance + @"
          }
        }
      ]
    }
  },
  ""size"": 255,
  ""from"": 0,
  ""sort"": []
}
");
            List<ProcessIncident> tasks = new List<ProcessIncident>();
            var response = await client.PostAsync(elasticsearchUrl + "/zeebe-record-incident/_search", content);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                String jsonResult = await response.Content.ReadAsStringAsync();
                var result = System.Text.Json.JsonSerializer.Deserialize<List<Incident_Response>>(jsonResult, new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                foreach (var r in result)
                {
                    ProcessIncident pt = new ProcessIncident();

                    pt.ElementInstanceKey = r.value.elementInstanceKey;
                    pt.TaskName = r.value.elementId;
                    pt.ErrorMessage = r.value.errorResponse;
                    pt.ErrorType = r.value.errorType;
                    tasks.Add(pt);
                }
            };
            return tasks;
        }
        public async Task<List<ProcessTask>> GetTasks(long processInstance)
        {

            var elasticsearchUrl = config["Endpoint:ElasticSearch"];
            if (string.IsNullOrEmpty(elasticsearchUrl)) elasticsearchUrl = "http://localhost:9200";
            HttpClient client = new HttpClient();

            var content = new StringContent(@"
{
  ""query"": {
    ""bool"": {
      ""must"": [
        {
          ""match"": {
            ""value.processInstanceKey"": " + processInstance + @"
          }
        }
      ]
    }
  },
  ""size"": 255,
  ""from"": 0,
  ""sort"": []
}
");
            List<ProcessTask> tasks = new List<ProcessTask>();
            var response = await client.PostAsync(elasticsearchUrl + "/zeebe-record-process-instance/_search", content);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                String jsonResult = await response.Content.ReadAsStringAsync();
                var result = System.Text.Json.JsonSerializer.Deserialize<List<Task_Response>>(jsonResult, new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                foreach (var r in result)
                {
                    ProcessTask pt = new ProcessTask();

                    pt.TaskName = r.value.bpmnElementType;
                    pt.TaskStatus = r.intent;
                    pt.TimeStamp = TimeSpan.FromMilliseconds((double) r.timestamp).ToString("dd/MM/yyyy HH:mm:ss");
                    tasks.Add(pt);
                }
            };
            return tasks;
        }

        public async Task<IViewComponentResult> InvokeAsync(long processInstance)
        {
            ProcessInstanceInfo pi = await GetInstance(processInstance);
            pi.Incidents = await GetIncident(processInstance);
            pi.Tasks= await GetTasks(processInstance);
            pi.Variables = await GetVariables(processInstance);
            return View(pi);
        }


    }
}
