using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Repositories;
using OpenDMS.Workflow.API.BusinessLogic;
using OpenDMS.Workflow.API.BusinessLogic.MaximoBL.Interfacce;
using OpenDMS.Workflow.API.DTOs.Maximo;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;

namespace OpenDMS.Workflow.API.Workers.Maximo
{
    public class OpenSRWorker : ICustomTask
    {
        public readonly IMaximoSRBL _bl;
        private readonly ILogger<OpenSRWorker> logger;
        private readonly IWorkflowEngine client;
        private readonly IAppSettingsRepository appSettings;

        public string JobType { get; set; } = "MaximoOpenSR";
        public string TaskLabel { get; set; } = "Apre una Service Request su Maximo";
        public string Icon { get; set; } = "fa-solid fa-gear";
        public string GroupName { get; set; } = "Maximo";
        public string[] AlternativeTasks { get; set; } = { };

        public string Inputs { get; set; } = "debug,assetnum,description,reportedpriority,studente";
        public string Outputs { get; set; } = "maximoResponse";
        public int MaxJobs { get; set; } = 1;
        public int PollingInterval { get; set; } = 1;
        public int TimeOut { get; set; } = 86400;

        public OpenSRWorker(
            ILogger<OpenSRWorker> logger,
            IWorkflowEngine engine,
            IAppSettingsRepository appSettings,
            IMaximoSRBL bl,
            IDocumentService documentService)
        {
            this.logger = logger;
            client = engine;
            this.appSettings = appSettings;
            _bl = bl;
        }

        public async Task Initialize()
        {
            if (!int.TryParse(await appSettings.Get(Constants.CONST_DOCUMENT_WORKER_MAXJOBS), out int i))
                i = 1;
             MaxJobs = i;

            if (!int.TryParse(await appSettings.Get(Constants.CONST_DOCUMENT_WORKER_POLLING), out int i2))
                i2 = 1;
             PollingInterval = i2;

            if (!int.TryParse(await appSettings.Get(Constants.CONST_DOCUMENT_WORKER_TIMEOUT), out int i3))
                i3 = 10;
             TimeOut = i3;

           // await client.AddWorker(JobType, HandleJob, MaxJobs, PollingInterval, TimeOut);
        }
        public async Task<TaskItem> PaletteItem()
        {
            return null;
        }

        public async Task HandleJob(IJobClient jobClient, IJob job)
        {
            try
            {

                JObject jsonObject = JObject.Parse(job.Variables.ToLower());

                int i = 0;
                int.TryParse((string)jsonObject["debug"], out i);
                bool debug = i > 0;

                string studente = (string)jsonObject["studente"];
                string reportedpriority = (string)jsonObject["reportedpriority"];
                string description = (string)jsonObject["description"];
                string assetnum = (string)jsonObject["assetnum"];

                SR sr = new SR(studente,reportedpriority,description,assetnum);
       
                string maximoResponse = await _bl.PostSrToMaximo(sr);

                var variables = new
                {
                    Response = maximoResponse
                };


                // Serializzazione dell'oggetto in formato JSON
                string variablesJson = JsonConvert.SerializeObject(variables);

                await jobClient.NewCompleteJobCommand(job.Key)
                     .Variables(variablesJson)
                     .Send();

                //Console.WriteLine(GetLogInformation(false, job, maximoResponse));
                //logger.LogTrace(GetLogInformation(false, job, maximoResponse));
            }
            catch (Exception ex)
            {
                await client.SetVariables(job.ElementInstanceKey.ToString(), JsonConvert.SerializeObject(new { errorMessage = ex.Message }));
                if (job.Retries > 1)
                    await jobClient.NewFailCommand(job.ProcessDefinitionKey).Retries(job.Retries - 1)
                    .ErrorMessage(ex.Message)
                    .Send();
                else
                    await jobClient.NewThrowErrorCommand(job.Key)
                       .ErrorCode("EXCEPTION")
                       .ErrorMessage(ex.Message)
                       .Send();

                Console.WriteLine(GetLogInformation(true, job, null));
                logger.LogError(GetLogInformation(true, job, null));
                //throw new Exception("Errori nell'attività " + job.Type);
            }
        }

        public string GetLogInformation(bool isError, IJob job, string? maximoResponse)
        {
            string logInformation;

            if (isError)
            {
                logInformation =
                                  "\n" + $"************************** {job.Type} **********************************" +
                                  "\n" + $"Attività di tipo  {job.Type} non è stata completata a causa di errori" +
                                  "\n" + $"************************************************************************";
            }
            else
            {
                logInformation =
                                 "\n" + $"************************** {job.Type} **********************************" +
                                 "\n" + $"L'attività di tipo  {job.Type} è stata completata con successo!" +
                                 "\n" + $"Variabili di Output: " +
                                 "\n" + $" Maximo Response: {maximoResponse}" +
                                 "\n" + $"************************************************************************";
            }

            return logInformation;
        }
    }
}
