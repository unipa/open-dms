using Microsoft.Maui.Graphics.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.Utilities;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Workflow.API.BusinessLogic;
using OpenDMS.Workflow.API.BusinessLogic.TitulusBL.Interfacce;
using OpenDMS.Workflow.API.DTOs.Palette;
using OpenDMS.Workflow.API.DTOs.Titulus;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;

namespace OpenDMS.Workflow.API.Workers.Titulus
{
    public class CreateDocumentInDMSWorker : ICustomTask
    {
        private readonly IConfiguration config;
        public readonly ITitulusBL _bl;
        private readonly ILogger<CreateDocumentInDMSWorker> logger;
        private readonly IWorkflowEngine client;
        private readonly IDocumentService documentService;
        private readonly IAppSettingsRepository appSettings;

        public string JobType { get; set; } = "CreateDocumentInDMS";
        public string TaskLabel { get; set; } = "Crea un nuovo documento su DMS partendo da un documento Titulus";
        public string Icon { get; set; } = "fa-regular fa-folder";
        public string GroupName { get; set; } = "Titulus";
        public string[] AlternativeTasks { get; set; } = { };

        public string Inputs { get; set; } = "numero_protocollo,documento,template,debug";
        public string Outputs { get; set; } = "dms_id,titulus_document_files";
        public int MaxJobs { get; set; } = 1;
        public int PollingInterval { get; set; } = 1;
        public int TimeOut { get; set; } = 60;

        public CreateDocumentInDMSWorker(
            ILogger<CreateDocumentInDMSWorker> logger,
            IWorkflowEngine engine,
            IAppSettingsRepository appSettings,
            ITitulusBL bl,
            IConfiguration config,
            IDocumentService documentService)
        {
            this.logger = logger;
            client = engine;
            this.appSettings = appSettings;
            _bl = bl;
            this.config = config;
            this.documentService = documentService;
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


                //string jsonDoc = (string)jsonObject["documento"];
                string template = (string)jsonObject["template"];

                //DocSearch titulus = Newtonsoft.Json.JsonConvert.DeserializeObject<DocSearch>(jsonObject["documento"]);

                var  titulus = jsonObject["documento"].ToObject<DocSearch>();
                titulus.DataProt = (titulus.DataProt.Insert(6, "-").Insert(4, "-") + "T00:00:00");

                //template = template.Replace("{documento.DataProt}", )
                template = template.Parse(titulus, "documento");


                //JObject documentFromTitulus = JsonConvert.DeserializeObject<JObject>(jsonDoc);
                string documentFromTitulusType = titulus.Tipo;
                string documentFromTitulusPhysdoc = titulus.Physdoc;
                //string documentFromTitulusFiles = documentFromTitulus["Files"].ToString();
                int i = 0;
                int.TryParse((string)jsonObject["debug"], out i);
                bool debug = i > 0;
                DocumentInfo documentInfo = await CreateDocumentInDms(template, titulus, debug);
                if (documentInfo.Id > 0)
                {
                    var filesFromTitulus = titulus.Files;
                    if (filesFromTitulus != null)
                    {
                        var fileArray = filesFromTitulus.file;

                        if (fileArray.Length > 0 && !debug)
                        {
                            int count = 0;
                            foreach (var file in fileArray)
                            {
                                string fileID = file.Name;
                                string fileName = file.Title;
                                int image = await AddFileInDmsDocument(fileID, fileName, documentInfo.Id, file.Principale);
                                if (image > 0)
                                {
                                    count++;
                                    Console.WriteLine($"File {count} nome: {fileName} id: {fileID} aggiunto al documento dms {documentInfo.Id} con id {image}");
                                }
                            }
                        }
                    }
                    ProtocolInfo protocolInfo = new ProtocolInfo();
                    protocolInfo.ProtocolUser = documentFromTitulusPhysdoc;
                    string nr = titulus.NumProt.ToString();
                    int.TryParse(nr.Substring(nr.Length - 7), out int n);
                    protocolInfo.Number = nr;
                    protocolInfo.ExternalProtocolURL = titulus.titulusDocumentURL;
                    DateTime.TryParse(titulus.DataProt.ToString(), out DateTime dt);
                    protocolInfo.Date = dt;
                    UserProfile u = UserProfile.SystemUser();
                    documentInfo = await documentService.Protocol(documentInfo.Id, protocolInfo, u);

                }
                var variables = new
                {
                    dms_id = documentInfo.Id,
                    titulus_document_files = titulus.Files,
                };

                // Serializzazione dell'oggetto in formato JSON
                string variablesJson = JsonConvert.SerializeObject(variables);

                await jobClient.NewCompleteJobCommand(job.Key)
                     .Variables(variablesJson)
                     .Send();

                Console.WriteLine(GetLogInformation(false, job, variablesJson, documentInfo));
                logger.LogTrace(GetLogInformation(false, job, variablesJson, documentInfo));
            }
            catch (Exception ex)
            {
                logger.LogError(JobType + ": " + job.ProcessDefinitionKey + "\\" + job.ElementId + " - Instance: " + job.ProcessDefinitionKey + "\\" + job.ElementInstanceKey + " - " + ex.Message);
                await client.SetVariables(job.ProcessDefinitionKey.ToString(), JsonConvert.SerializeObject(new { errorMessage = ex.Message }));

                    await jobClient.NewThrowErrorCommand(job.Key)
                       .ErrorCode("EXCEPTION")
                       .ErrorMessage(ex.Message)
                       .Send();

                Console.WriteLine(GetLogInformation(true, job, null, null));
                logger.LogError(GetLogInformation(true, job, null, null));
  //              throw new Exception("Errori nell'attività " + job.Type);
            }
        }

        public async Task<DocumentInfo> CreateDocumentInDms(string template, DocSearch titulus,  bool debug)
        {
            try
            {
                if (!debug)
                {
                    CreateOrUpdateDocument dms_document = new CreateOrUpdateDocument(); ;
                    try
                    {
                        dms_document = Newtonsoft.Json.JsonConvert.DeserializeObject<CreateOrUpdateDocument>(template);
                    } catch
                    {
                        dms_document.Description = "Document imported from Titulus - titulus physdoc: " + titulus.Physdoc;
                        dms_document.DocumentTypeId = titulus.Tipo == "arrivo" ? "NRI" : titulus.Tipo == "partenza" ? "NSP" : titulus.Tipo == "interno" ? "NIN" : "";
                        dms_document.ContentType = ContentType.Document;

                    }
                    UserProfile u = UserProfile.SystemUser();
                    DocumentInfo documentInfo = await documentService.CreateAndRead(dms_document, u);

                    return documentInfo;
                }
                else
                {
                    DocumentInfo documentInfo = new DocumentInfo();
                    return documentInfo;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore: " + ex.Message);
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
                        ContentType = ContentType.Document,
                        Content = fileContent,
                        AttachTo = new[] { dms_id }
                    };
                    var documentInfo = await documentService.CreateAndRead(c, u);
                    return documentInfo.Image.Id;
                } else
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


        public string GetLogInformation(bool isError, IJob job, string? documentFromTitulusFiles, DocumentInfo? documentInfo)
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
                                  "\n" + $"Dms Id: {documentInfo.Id}" +
                                  "\n" + $"Titulus Document Files: {documentFromTitulusFiles}" +
                                  "\n" + $"************************************************************************";
            }

            return logInformation;
        }

        public async Task<TaskItem> PaletteItem()
        {
            return null;
        }
    }
}
