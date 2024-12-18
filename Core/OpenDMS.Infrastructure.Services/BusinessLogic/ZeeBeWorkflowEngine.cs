using AngleSharp.Io;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using Zeebe.Client;

namespace OpenDMS.Infrastructure.Services.BusinessLogic
{
    public class ZeeBeWorkflowEngine : IWorkflowEngine
    {
        private readonly IZeebeClient client;

        private readonly ILoggerFactory loggerFactory;
        private readonly IConfiguration configuration;
        private readonly IDocumentService documentService;
        private readonly IProcessInstanceRepository processInstanceRepository;
        private readonly IUserTaskService userTaskService;
        private static readonly string ZeebeDefaultUrl = "127.0.0.1:26500";
        private static readonly string ElasticSearchDefaultUrl = "http://localhost:9200";
        private readonly string WorkerName = Guid.NewGuid().ToString();

        public const string CONST_CAMUNDA_ENDPOINT = "Camunda:EndPoint";
        public const string CONST_ELASTICSEARCH_ENDPOINT = "ElasticSearch:EndPoint";

        //public ZeeBeEngine(IZeebeClient ZeebeClient, ILoggerFactory loggerFactory, IConfiguration configuration)
        public ZeeBeWorkflowEngine(ILoggerFactory loggerFactory, 
            IConfiguration configuration,
            IDocumentService documentService,
            IProcessInstanceRepository processInstanceRepository,
            IUserTaskService userTaskService)
        {
            //client = ZeebeClient;
            var zeebeUrl = configuration[CONST_CAMUNDA_ENDPOINT];
            if (string.IsNullOrWhiteSpace(zeebeUrl))
                zeebeUrl = ZeebeDefaultUrl;
            client = ZeebeClient.Builder()
               .UseLoggerFactory(loggerFactory)
               .UseGatewayAddress(zeebeUrl)
               .UsePlainText()
               .Build();
            this.loggerFactory = loggerFactory;
            this.configuration = configuration;
            this.documentService = documentService;
            this.processInstanceRepository = processInstanceRepository;
            this.userTaskService = userTaskService;
        }


        public async Task<Topology> Status()
        {
            Topology T = new Topology();

            var ret =  (await client.TopologyRequest().Send());
            T.GatewayVersion = ret.GatewayVersion;
            foreach (var b in ret.Brokers)
            {
                var b1 = new BrokerInfo() { Address = b.Address, Host = b.Host, NodeId = b.NodeId, Port = b.Port };
                foreach (var p in b.Partitions)
                    b1.Partitions.Add(new PartitionInfo() { IsLeader = p.IsLeader, PartitionId = p.PartitionId });
                T.Brokers.Add(b1);
            }
            return T;
        }

        public async Task<long> DeployNewProcess(string processDiagram, string processName)
        {
            var deployResponse = await client.NewDeployCommand()
                           .AddResourceStringUtf8(processDiagram, processName)
                           .Send();
            return deployResponse.Key;
        }

        public async Task SendMessage(string messageName, string correlationKey, string variables)
        {
            await client.NewPublishMessageCommand()
                .MessageName(messageName)
                .CorrelationKey(correlationKey)
                .TimeToLive(TimeSpan.Zero)
                .Variables(variables)
                .Send();
        }

        public async Task<long> StartProcess(long processKey, string variables)
        {
            var processInstance = await client
               .NewCreateProcessInstanceCommand()
               .ProcessDefinitionKey(processKey)
               .Variables(variables)
               .Send();
            return processInstance.ProcessInstanceKey;
        }
        public async Task<string> StartProcessAndWait(long processKey, string variables)
        {
            var processInstance = await client
               .NewCreateProcessInstanceCommand()
               .ProcessDefinitionKey(processKey)
               .Variables(variables)
               .WithResult()
               .Send();
            return processInstance.Variables;
        }
        public async Task<long> StartProcess(string bpmnId, DocumentInfo Document, UserProfile User, string EventId,  string variables)
        {
            var processDefinitionId = await documentService.FindByUniqueId(null, bpmnId, Domain.Enumerators.ContentType.Workflow);
            var processDefinition = await documentService.Get(processDefinitionId);
            var image = 0;
            var Owner = User.userId;
            if (processDefinition != null && processDefinition.ImageId.HasValue)
                image = processDefinition.ImageId.Value;
//            if (processDefinition != null)
//                Owner = processDefinition.Owner;
            try
            {
                var processInstance = await client
                   .NewCreateProcessInstanceCommand()
                   .BpmnProcessId(bpmnId)
                   .LatestVersion()
                   .Variables(variables)
                   .Send();

                ProcessInstance P = new ProcessInstance()
                {
                    DocumentId = Document.Id,
                    EventName = EventId,
                    ProcessInstanceId = processInstance.ProcessInstanceKey.ToString(),
                    ProcessEngineId = "Camunda",
                    ProcessKey = bpmnId,
                    StartDate = DateTime.UtcNow,
                    StartUser = User.userId,
                    ProcessDefinitionId = processDefinitionId,
                    ProcessImageId = image
                };
                await processInstanceRepository.Start(P);
                return processInstance.ProcessInstanceKey;
            }
            catch (Exception ex)
            {
                CreateNewTaskMessage task = new CreateNewTaskMessage()
                {
                    Attachments = new List<int>() { Document.Id },
                    CompanyId = Document.Company.Id,
                    Description = $"Non è stato possibile avviare il processo: <b>{processDefinition?.Description}</b>.<br/>Invia al supporto tecnico le informazioni qui di seguito riportate. Li aiuterai a comprendere la causa del problema<br/><br/>Identifictivo Processo: {bpmnId}<br/>Evento: {EventId}<br/>Id Utente: {User}<br/>Errore:" + ex.Message + "<br/><h6>Stack Trace:</h6>" + ex.StackTrace,
                    Title = $"Errore durante l'avvio di un processo'",
                    NotifyTo = new List<string>() { Owner },
                    ProcessDefinitionKey = bpmnId,
                    ProcessDataId = Document.Id,
                    Sender = SpecialUser.SystemUser
                };
                await userTaskService.CreateError(task, UserProfile.SystemUser());
                throw;
            }
        }
        public async Task<long> UpdateProcessVariables(DocumentInfo Document, UserProfile User, string EventId, string variables)
        {

            var ok = 0;
            // recupero i task di tipo evento per l'evento e l'utente corrente
            var tasks = await userTaskService.GetByEvent(Document.Id, EventId, User);
            foreach (var t in tasks)
            {
                await userTaskService.UpdateTaskVariables (t, variables);
                ok++;
            }

            //var processes = await processInstanceRepository.GetByDocumentId(Document.Id);
            //foreach (var process in processes.Where(p=>p.StopDate is null))
            //{

            //    var key = long.Parse(process.ProcessInstanceId);

            //    var response = await client.NewSetVariablesCommand(key)
            //        .Variables(variables)
            //        .Local()
            //        .Send();
            //    ok++;
            //}
            return ok; 
        }

        public async Task CompleteJob(string jobKey, string variables)
        {

            long key = long.Parse(jobKey);
            await client.NewCompleteJobCommand(key).Variables(variables).Send();
            //await client.NewCompleteJobCommand(key).Variables(variables).Send();
        }
        public async Task FailJob(string jobKey, int remainingRetries)
        {
            long key = long.Parse(jobKey);
            await client.NewFailCommand(key).Retries(remainingRetries).Send();
        }

        public async Task ResolveIncident(string incidentKey, string incidentJob, int remainingRetries)
        {
            long key = long.Parse(incidentKey);
            long jobkey = long.Parse(incidentJob);
            if (jobkey > 0) await (client.NewUpdateRetriesCommand(jobkey)).Retries(remainingRetries).Send();
            await (client.NewResolveIncidentCommand(key)).SendWithRetry();
        }

        /// <summary>
        /// Riattiva un task in errore, eseguendone un'altra istanza
        /// </summary>
        /// <param name="processKey"></param>
        /// <param name="elementId"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public async Task<string> ReactivateElement(long processKey, string elementId, string variables = null)
        {
            var processInstance = client
                .NewModifyProcessInstanceCommand(processKey)
                .ActivateElement(elementId);
            if (variables != null)
                processInstance = processInstance.WithVariables(variables, elementId);

            var res = await processInstance.SendWithRetry();
            return res.ToString();
        }


        public async Task<string> StartProcessAndWait(string bpmnId, string variables)
        {
            var processInstance = await client
               .NewCreateProcessInstanceCommand()
               .BpmnProcessId(bpmnId)
               .LatestVersion()
               .Variables(variables)
               .WithResult()
               .Send();
            return processInstance.Variables;
        }

        public async Task CancelProcessInstance(int documentId)
        {
            await userTaskService.DeleteByDocument(documentId);
            var instances = await processInstanceRepository.GetByDocumentId(documentId);
            foreach (var i in instances)
            {
                int key = 0;
                if (int.TryParse(i.ProcessInstanceId, out key))
                {

                    var processInstance = await client
                        .NewCancelInstanceCommand(key)
                       .Send();
                }
            }

        }


        public async Task SetVariables(string elementInstanceKey, string variables, bool Locale = false)
        {
            var key = long.Parse(elementInstanceKey);
            var proc = client.NewSetVariablesCommand(key).Variables(variables);
            var response = (Locale) ? await proc.Local().Send() : await proc.Send();
            
        }






        public async Task<List<ProcessInstance_DTO>> GetAllInstancesByKey(string processDefinitionKey)
        {
            var esUrl = configuration[CONST_ELASTICSEARCH_ENDPOINT];
            if (string.IsNullOrWhiteSpace(esUrl))
                esUrl = ElasticSearchDefaultUrl;

            List<ProcessInstance_DTO> list = new();
            var client = new ElasticsearchClient(new Uri(esUrl));
            var response = await client.SearchAsync<ProcessData>(s => s
                .Index("zeebe-record-process")
                .From(0)
                .Size(100)
                .Query(q => q
                    .Bool(b => b
                        .Filter(f => f
                            .Term(m => m.Field(f => f.value.bpmnProcessId).Value(processDefinitionKey))
                        )
                    )
                )
            );

            if (response.IsValidResponse)
            {
                var docs = response.Documents;
                foreach (var doc in docs)
                {
                    ProcessInstance_DTO pi = new();
                    if (response.Documents.First().value.variables != null)
                        foreach (var v in JObject.Parse(response.Documents.First().value.variables.ToString()))
                        {
                            pi.variables.Add(v.Key, v.Value.ToString());
                        }
                    pi.processInstanceId = doc.value.processInstanceKey.ToString();
                    pi.processDefinitionId = doc.value.processDefinitionKey.ToString();
                    pi.processDefinitionKey = doc.value.bpmnProcessId.ToString();
                    list.Add(pi);
                }
            }
            return list;
        }

        public async Task<ProcessInstance_DTO> GetInstanceById(string processInstanceId)
        {
            var esUrl = configuration[CONST_ELASTICSEARCH_ENDPOINT];
            if (string.IsNullOrWhiteSpace(esUrl))
                esUrl = ElasticSearchDefaultUrl;

            List<ProcessInstance_DTO> list = new();
            try
            {
                var client = new ElasticsearchClient(new Uri(esUrl));
                var response = await client.SearchAsync<ProcessData>(s => s
                    .Index("zeebe-record-process-instance-creation")
                    .From(0)
                    .Size(100)
                    .Query(q => q
                        .Bool(b => b
                            .Filter(f => f
                                .Term(m => m.Field(f => f.value.processInstanceKey).Value(processInstanceId))
                            )
                        )
                    )
                );

                if (response.IsValidResponse)
                {
                    if (response.Documents.Count > 0)
                    {
                        var doc = response.Documents.Last();
                        ProcessInstance_DTO pi = new();
                        //var vars = response.Documents.Where(p => p.value.variables != null);
                        //if (vars != null)
                        //    foreach (var vi in vars)
                        //    {
                        //        foreach (var v in JObject.Parse(vi.value.variables.ToString()))
                        //        {
                        //            if (pi.variables.ContainsKey(v.Key))
                        //                pi.variables[v.Key] = v.Value.ToString();
                        //            else
                        //                pi.variables.Add(v.Key, v.Value.ToString());
                        //        }
                        //    }
                        pi.processInstanceId = doc.value.processInstanceKey.ToString();
                        pi.processDefinitionId = doc.value.processDefinitionKey.ToString();
                        pi.processDefinitionKey = doc.value.bpmnProcessId.ToString();
                        var vresponse = await client.SearchAsync<VariableData>(s => s
                            .Index("zeebe-record-variable*")
                            .From(0)
                            .Size(1024)
                            .Query(q => q
                                .Bool(b => b
                                    .Filter(f => f
                                        .Term(m => m.Field(f => f.value.processInstanceKey).Value(processInstanceId))
                                    )
                                )
                            ).Sort(s => s.Field(f => f.value.name))
                        );
                        if (vresponse.IsValidResponse)
                        {
                            var vars = vresponse.Documents;
                            if (vars != null)
                                foreach (var vi in vars)
                                {
                                    if (pi.variables.ContainsKey(vi.value.name))
                                        pi.variables[vi.value.name] = vi.value.value.ToString();
                                    else
                                        pi.variables.Add(vi.value.name, vi.value.value.ToString());
                                }
                        }


                        var iresponse = await client.SearchAsync<Incident>(s => s
                            .Index("zeebe-record-incident")
                            .From(0)
                            .Size(256)
                            .Query(q => q
                                .Bool(b => b
                                    .Filter(f => f
                                        .Term(m => m.Field(f => f.value.processInstanceKey).Value(processInstanceId))
                                    )
                                )
                            ).Sort(s=>s.Field(f=>f.timestamp))
                        );

                        if (iresponse.IsValidResponse)
                        {
                            var docs = iresponse.Documents;

                            foreach (var d in docs)
                            {
                                var id = d.key;
                                var i = pi.incidents.FirstOrDefault(i => i.Id == id.ToString());
                                if (i == null)
                                {
                                    i = new ProcessIncident();
                                    pi.incidents.Insert(0,i);
                                    i.creationDate = DateTimeOffset.FromUnixTimeMilliseconds(d.timestamp).DateTime;
                                }
                                i.elementId = d.value.elementId;
                                i.elementKey = d.value.elementInstanceKey.ToString();
                                i.lastUpdate = DateTimeOffset.FromUnixTimeMilliseconds(d.timestamp).DateTime;
                                i.errorMessage = d.value.errorMessage;
                                try
                                {
                                    i.jobKey = d.value.jobKey.ToString();
                                }
                                catch (Exception e) { 
                                    i.jobKey =  e.Message;
                                }
                                i.status = d.intent;
                                i.Id = id.ToString();
                                i.errorType = d.value.errorType;
                            }
                        }
                        pi.incidents = pi.incidents.Where(p => p.status != "RESOLVED").ToList();
                        return pi;
                    }
                }
            } catch (Exception ex) {
                ProcessInstance_DTO pi = new();
                var i = new ProcessIncident();
                pi.incidents.Add(i);
                i.elementId = "LETTURA DATI DA ELASTICSEARCH";
                i.lastUpdate = DateTime.Now;
                i.errorMessage = ex.Message;
                i.errorType = "COMUNICAZIONE CON: "+ esUrl;
                i.jobKey = "";
                i.status = "EXCEPTION";
                i.Id = "";
                return pi;
            }
            return null;
        }



        public class IncidentValue
        {
            public decimal processInstanceKey { get; set; }
            public decimal processDefinitionKey { get; set; }
            public decimal elementInstanceKey { get; set; }
           
            public decimal jobKey { get; set; }
            public string errorMessage { get; set; }
            public string bpmnProcessId { get; set; }
            public string elementId { get; set; }
            public string errorType { get; set; }
        }
        public class Incident
        {
            public decimal key { get; set; }
            public IncidentValue value { get; set; } = new();
            public string valueType { get; set; }
            public string intent { get; set; }
            public long timestamp { get; set; }

        }
        public class ProcessValue
        {
            public decimal processInstanceKey { get; set; }
            public decimal processDefinitionKey { get; set; }
            public object variables { get; set; }
            public string bpmnProcessId { get; set; }
        }
        public class ProcessData
        {
            public UInt64 key { get; set; } = 0;

            public ProcessValue value { get; set; } = new();

        }


        public class VariableValue
        {
            public decimal processInstanceKey { get; set; }
            public decimal processDefinitionKey { get; set; }
            public string name { get; set; }
            public string value  { get; set; }
            public string bpmnProcessId { get; set; }
        }
        public class VariableData
        {
            public UInt64 key { get; set; } = 0;

            public VariableValue value { get; set; } = new();

        }
    }
}
