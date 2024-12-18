using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Workflow.API.BusinessLogic;
using OpenDMS.Workflow.API.BusinessLogic.TitulusBL.Interfacce;
using OpenDMS.Workflow.API.DTOs.Palette;
using org.quartz;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;

namespace OpenDMS.Workflow.API.Workers.Titulus
{
    public class AddFilesToDocumentInDmsWorker : ICustomTask
    {
        public readonly ITitulusBL _bl;
        private readonly ILogger<AddFilesToDocumentInDmsWorker> logger;
        private readonly IWorkflowEngine client;
        private readonly IDocumentService documentService;
        private readonly IAppSettingsRepository appSettings;

        public string JobType { get; set; } = "AddFilesToDocumentInDMS";
        public string TaskLabel { get; set; } = "Aggiungi files provenienti da Titulus al corrispettivo documento DMS";
        public string Icon { get; set; } = "fa-regular fa-folder";
        public string GroupName { get; set; } = "Titulus";
        public string[] AlternativeTasks { get; set; } = { };

        public string Inputs { get; set; } = "dms_id,titulus_document_files,debug";
        public string Outputs { get; set; } = "dms_id";
        public int MaxJobs { get; set; } = 1;
        public int PollingInterval { get; set; } = 1;
        public int TimeOut { get; set; } = 60;

        public AddFilesToDocumentInDmsWorker(
            ILogger<AddFilesToDocumentInDmsWorker> logger,
            IWorkflowEngine engine,
            IAppSettingsRepository appSettings,
            ITitulusBL bl,
            IDocumentService documentService)
        {
            this.logger = logger;
            client = engine;
            this.appSettings = appSettings;
            _bl = bl;
            this.documentService = documentService;
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

           // await client.AddWorker(JobType, HandleJob, MaxJobs, PollingInterval, TimeOut);
        }

        public async Task HandleJob(IJobClient jobClient, IJob job)
        {
            try
            {

                JObject jsonObject = JObject.Parse(job.Variables);
                int dms_id = (int)jsonObject["dms_id"];
                string titulus_document_files = (string)jsonObject["titulus_document_files"];
                int i = 0;
                int.TryParse((string)jsonObject["debug"], out i);
                bool debug = i > 0;


                JObject documentFromTitulus = JsonConvert.DeserializeObject<JObject>(titulus_document_files);
                JArray fileArray = (JArray)documentFromTitulus["file"];

                if (fileArray.Count > 0 && !debug)
                {
                    int count = 0;
                    foreach (JObject file in fileArray)
                    {
                        string fileID = file["Name"].Value<string>();
                        string fileName = file["Title"].Value<string>();
                        var img = await AddFileInDmsDocument(fileID, fileName, dms_id, file["principale"].Value<bool>());
                        count++;
                        Console.WriteLine($"File {count} nome: {fileName} id: {fileID} aggiunto al documento dms {dms_id} con id {img}");
                    }
                }

                var variables = new
                {
                    dms_id
                };


                // Serializzazione dell'oggetto in formato JSON
                string variablesJson = JsonConvert.SerializeObject(variables);

                await jobClient.NewCompleteJobCommand(job.Key)
                     //.Variables(variablesJson)
                     .Send();

                Console.WriteLine(GetLogInformation(false, job, dms_id.ToString()));
                logger.LogTrace(GetLogInformation(false, job, dms_id.ToString()));
            }
            catch (Exception ex)
            {
                logger.LogError(JobType + ": " + job.ProcessDefinitionKey + "\\" + job.ElementId +  " - Instance: " + job.ProcessDefinitionKey + "\\" + job.ElementInstanceKey + " - " + ex.Message);
                await client.SetVariables(job.ElementInstanceKey.ToString(), JsonConvert.SerializeObject(new { errorMessage = ex.Message }));

                    await jobClient.NewThrowErrorCommand(job.Key)
                       .ErrorCode("EXCEPTION")
                       .ErrorMessage(ex.Message)
                       .Send();

                Console.WriteLine(GetLogInformation(true, job, null));
                logger.LogError(GetLogInformation(true, job, null));
//                throw new Exception("Errori nell'attività " + job.Type);
            }
        }

        public async Task<int> AddFileInDmsDocument(string fileID, string fileName, int dms_id, bool principale = false)
        {
            try
            {
                string newFileId = "";
                string fileData = _bl.GetFileBase64(fileID);

                FileContent fileContent = new FileContent();
                fileContent.FileName = fileName;
                fileContent.FileData = fileData;

                UserProfile u = UserProfile.SystemUser();
                int allegato = 0;
                if (!principale)
                {
                    CreateOrUpdateDocument c = new CreateOrUpdateDocument()
                    {
                        Description = fileName,
                        ContentType = Domain.Enumerators.ContentType.Document,
                        Content = fileContent,
                        AttachTo = new[] { dms_id }
                    };
                    var documentInfo = await documentService.CreateAndRead(c, u);
                    return documentInfo.Image.Id;
                }
                else
                {
                    DocumentImage documentInfo = await documentService.AddContent(principale ? dms_id : allegato, u, fileContent);
                    return documentInfo.Id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore: " + ex.Message);
            }
        }

        public string GetLogInformation(bool isError, IJob job, string? dms_id)
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
                                 "\n" + $" Dms Id: {dms_id}" +
                                 "\n" + $"************************************************************************";
            }

            return logInformation;
        }
    }
}
