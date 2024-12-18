using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Services.BusinessLogic;
using OpenDMS.Workflow.API.BusinessLogic;
using OpenDMS.Workflow.API.BusinessLogic.TitulusBL.Interfacce;
using OpenDMS.Workflow.API.DTOs.Palette;
using OpenDMS.Workflow.API.DTOs.Titulus;
using org.quartz;
using System.Text.Json;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace OpenDMS.Workflow.API.Workers.Titulus
{
    public class GetDocumentFromTitulusWorker : ICustomTask
    {
        public readonly ITitulusBL _bl;
        private readonly ILogger<GetDocumentFromTitulusWorker> logger;
        private readonly IWorkflowEngine client;
        private readonly IAppSettingsRepository appSettings;

        public string JobType { get; set; } = "GetDocumentFromTitulus";
        public string TaskLabel { get; set; } = "Ottiene il documento da Titulus";
        public string Icon { get; set; } = "fa-regular fa-folder";
        public string GroupName { get; set; } = "Titulus";
        public string[] AlternativeTasks { get; set; } = { };

        public string Inputs { get; set; } = "numero_protocollo, debug";
        public string Outputs { get; set; } = "documento";
        public int MaxJobs { get; set; } = 1;
        public int PollingInterval { get; set; } = 1;
        public int TimeOut { get; set; } = 60;

        public GetDocumentFromTitulusWorker(
            ILogger<GetDocumentFromTitulusWorker> logger,
            IWorkflowEngine engine,
            IAppSettingsRepository appSettings,
            ITitulusBL bl)
        {
            this.logger = logger;
            client = engine;
            this.appSettings = appSettings;
            _bl = bl;
        }
        public async Task<TaskItem> PaletteItem()
        {
            return null;
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

            //await client.AddWorker(JobType, HandleJob, MaxJobs, PollingInterval, TimeOut);
        }

        public async Task HandleJob(IJobClient jobClient, IJob job)
        {
            try
            {

                JObject jsonObject = JObject.Parse(job.Variables);
                string numero_protocollo = (string)jsonObject["numero_protocollo"];
                int i = 0;
                int.TryParse((string)jsonObject["debug"], out i);
                bool debug = i > 0;

                string jsonString = _bl.GetDocumentFromProtocol(numero_protocollo);
                if (String.IsNullOrEmpty(jsonString))
                    throw new Exception($"Il protocollo '{numero_protocollo}' non è stato trovato");
                JsonElement jsonElement = JsonSerializer.Deserialize<JsonElement>(jsonString);

                var variables = new
                {
                    documento = JsonSerializer.Deserialize<DocSearch>(jsonString)
                };
                // Serializzazione dell'oggetto in formato JSON
                string variablesJson = JsonConvert.SerializeObject(variables);

                await jobClient.NewCompleteJobCommand(job.Key)
                     .Variables(variablesJson)
                     .Send();

                Console.WriteLine(GetLogInformation(false, job, jsonString));
                logger.LogTrace(GetLogInformation(false, job, jsonString));

            }
            catch (Exception ex)
            {
                logger.LogError(JobType + ": " + job.ProcessDefinitionKey + "\\" + job.ElementId + " - Instance: " + job.ProcessDefinitionKey + "\\" + job.ElementInstanceKey + " - " + ex.Message);
                await client.SetVariables(job.ProcessDefinitionKey.ToString(), JsonConvert.SerializeObject(new { errorMessage = ex.Message }));
                if (job.Retries > 1)
                    await jobClient.NewFailCommand(job.ElementInstanceKey).Retries(job.Retries - 1)
                    .ErrorMessage(ex.Message)
                    .Send();
                else
                    await jobClient.NewThrowErrorCommand(job.Key)
                       .ErrorCode("EXCEPTION")
                       .ErrorMessage(ex.Message)
                       .Send();

                Console.WriteLine(GetLogInformation(true, job, null));
                logger.LogError(GetLogInformation(true, job, null));
  //              throw new Exception($"Errori nell'attività di tipo {job.Type}, errore: {ex.Message}");
            }
        }

        public string GetLogInformation(bool isError, IJob job, string? document)
        {
            string logInformation;

            if (isError)
            {
                logInformation = "\n" + $"************************** {job.Type} **********************************" +
                                 "\n" + $"Attività di tipo  {job.Type} non è stata completata a causa di errori" +
                                 "\n" + $"************************************************************************";
            }
            else
            {
                logInformation =
                                   "\n" + $"************************** {job.Type} **********************************" +
                                   "\n" + $"L'attività di tipo  {job.Type} è stata completata con successo!" +
                                   "\n" + $"Variabili di Output:" +
                                   "\n" + $"Documento di Titulus: {document}" +
                                   "\n" + $"************************************************************************";
            }

            return logInformation;
        }
    }
}
