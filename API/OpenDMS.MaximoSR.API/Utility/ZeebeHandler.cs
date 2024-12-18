using Newtonsoft.Json.Linq;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;
using Zeebe.Client;
using NLog.Extensions.Logging;
using OpenDMS.MaximoSR.API.Utility.Interfacce;
using OpenDMS.MaximoSR.API.Models;

namespace OpenDMS.MaximoSR.API.Utility
{
    public class ZeebeHandler : IZeebeHandler
    {
        private readonly IConfiguration _config;

        public ZeebeHandler(IConfiguration config)
        {
            _config = config;

        }
        public async void Main(SR sr)
        {
            try
            {
                //Dichiarazione variabili
                string zeebeUrl = _config["ZEEBE_URL"];
                string bpmnFilePath = _config["BPMN_FILE_PATH"];
                string bpmnContent = File.ReadAllText(bpmnFilePath);

                //Inizializza il client Zebee
                IZeebeClient client = await ClientBuilder(zeebeUrl);

                //Deploy del processo
                string BpmnProcessId = await DeployProcess(client, bpmnFilePath, bpmnContent);

                //Avvio dell'istanza di processo
                long processInstanceKey = await StartProcessInstance(client, BpmnProcessId, sr.Studente, sr.Reportedpriority, sr.Description, sr.Assetnum);

                //Avvio del jobWorker
                using var signal = new EventWaitHandle(false, EventResetMode.AutoReset);
                client.NewWorker()
                           .JobType("received_data_from_frontend")
                           .Handler(HandlerJob1Async)
                           .MaxJobsActive(5)
                           .Name("received_data_from_frontend")
                           .AutoCompletion()
                           .PollInterval(TimeSpan.FromSeconds(1))
                           .PollingTimeout(TimeSpan.FromSeconds(1))
                           .Timeout(TimeSpan.FromSeconds(10))
                           .Open();

                signal.WaitOne();              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private async Task<IZeebeClient> ClientBuilder(string zeebeUrl)
        {
            try
            {
                var client = ZeebeClient.Builder()
                .UseLoggerFactory(new NLogLoggerFactory())
                .UseGatewayAddress(zeebeUrl)
                .UsePlainText()
                .Build();
                Console.WriteLine("*******************CONNESSIONE ALL'ENGINE*********************");
                var topology = await client.TopologyRequest()
                        .Send();
                Console.WriteLine(topology);
                Console.WriteLine("**************************************************************");
                return client;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Errore: "+ ex.Message);
            }
            
        }

        private async Task<string> DeployProcess(IZeebeClient client, string bpmnFilePath, string bpmnContent)
        {
            try
            {
                // Distribuisci il processo BPMN
                var deployResponse = await client.NewDeployCommand()
                    .AddResourceString(bpmnContent, System.Text.Encoding.UTF8, Path.GetFileName(bpmnFilePath))
                    .Send();
                Console.WriteLine("        Il file .bpmn è stato deployato correttamente!        ");
                Console.WriteLine("**************************************************************");

                // Recupera l'id del processo
                var BpmnProcessId = deployResponse.Processes[0].BpmnProcessId;

                return BpmnProcessId;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Errore: " + ex.Message);
            }
            
        }

        public async Task<long> StartProcessInstance(IZeebeClient client, string bpmnProcessId, string studente, string reportedpriority, string description, string assetnum)
        {
            try
            {
                var processInstanceResponse = await client
                            .NewCreateProcessInstanceCommand()
                            .BpmnProcessId(bpmnProcessId)
                            .LatestVersion()
                            .Variables("{\"studente\":\"" + studente + "\",\"reportedpriority\":\"" + reportedpriority + "\",\"description\":\"" + description + "\",\"assetnum\":\"" + assetnum + "\"}")
                            .Send();
                var processInstanceKey = processInstanceResponse.ProcessInstanceKey;
                Console.WriteLine("L'istanza di processo è stata avviata con id: " + processInstanceKey);
                Console.WriteLine("**************************************************************");
                return processInstanceKey;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Errore: " + ex.Message);
            }
            

            
        }

        public async Task HandlerJob1Async(IJobClient jobClient, IJob job)
        {
            try
            {
                JObject jsonObject = JObject.Parse(job.Variables);
                string assetnum = (string)jsonObject["assetnum"];
                string description = (string)jsonObject["description"];
                string reportedpriority = (string)jsonObject["reportedpriority"];
                string studente = (string)jsonObject["studente"];

                Console.WriteLine("In lavoro sul task di tipo received_data_from_frontend");
                SR sr = new(studente, reportedpriority, description, assetnum);
                Console.WriteLine("**************************SR**********************************");
                Console.WriteLine("Assetnum: " + sr.Assetnum +
                                  "\nDescription: " + sr.Description +
                                  "\nReportedpriority: " + sr.Reportedpriority +
                                  "\nStudente: " + sr.Studente +
                                  "\nOrigine: " + sr.Origine +
                                  "\nAssetorgid: " + sr.Assetorgid +
                                  "\nAssetsiteid: " + sr.Assetsiteid +
                                  "\nWorktype: " + sr.Worktype +
                                  "\nIddms: " + sr.Iddms
                                  );
                Console.WriteLine("**************************************************************");
                var res = await PostSrToMaximo(sr);
                await jobClient.NewCompleteJobCommand(job.Key)
                     .Variables("{\"result\": \"1\"}")
                     .Send();
                Console.WriteLine("Attività recuperata completata!");
                Console.WriteLine("**************************************************************");
            }
            catch
            {
                jobClient.NewThrowErrorCommand(job.Key);
                Console.WriteLine("L'Attività recuperata non è stata completata a causa di errori!");
                Console.WriteLine("**************************************************************");
                throw new Exception("Errori nell'attività di inserimento dell'SR su Maximo");
            }
        }
        public async Task<int> PostSrToMaximo(SR sr)
        {
            string url = _config["Endpoint:INSERT_SR_API_URL"];
            var content = new Dictionary<string, string>
                            {
                                { "ASSETNUM", sr.Assetnum },
                                { "ASSETORGID", sr.Assetorgid },
                                { "ASSETSITEID", sr.Assetsiteid },
                                { "DESCRIPTION", sr.Description },
                                { "REPORTEDPRIORITY", sr.Reportedpriority },
                                { "WORKTYPE", sr.Worktype },
                                { "STUDENTE", sr.Studente },
                                { "ORIGINE", sr.Origine },
                                { "IDDMS", sr.Iddms }
                             };
            List<Tuple<string, string>> headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("Authorization", "Basic "+_config["Maximo:Credentials"]),
                new Tuple<string, string>("Accept", "*/*"),
                
        };

            var response = await HttpCallHandler.PostAsyncCallUrlEncoded(url, content, headers);
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    return 1;
                }
                else
                {
                    throw new Exception("Errore durante la chiamata API a Maximo");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Errore durante la chiamata API a Maximo, errore: " + ex.Message);
            }
        }
    }
}



