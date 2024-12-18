using Elmi.Core.FileConverters;
using Newtonsoft.Json.Linq;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.PdfManager;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;

namespace OpenDMS.Workflow.API.Workers.Documents
{
    public class FillTemplateWorker : BaseWorker
    {
        private readonly IDocumentService documentService;
        private readonly IFileConvertFactory fileConverter;
        private readonly IConfiguration config;

        public override string JobType { get; set; } = "fillTemplateTask";
        public override string TaskLabel { get; set; } = "Aggiunge un modello precompilato ad un Documento";
        public override string Icon { get; set; } = "fa fa-file-word-o";
        public override string GroupName { get; set; } = "Documenti";
        public override string[] AlternativeTasks { get; set; } = { };

        public override string Inputs { get; set; } = "TaskDocumentId,TemplateId,Document,OutputExtension";
        public override string Outputs { get; set; } = "NewDocumentId, Image";

        public override int MaxJobs { get; set; } = 1;
        public override int PollingInterval { get; set; } = 1;
        public override int TimeOut { get; set; } = 60;





        public FillTemplateWorker(
            ILogger<FillTemplateWorker> logger,
            IWorkflowEngine engine,
            IDocumentService documentService,
            IFileConvertFactory fileConverter,
            IConfiguration config,
           IAppSettingsRepository appSettings) : base(logger, engine, appSettings)
        {
            this.documentService = documentService;
            this.fileConverter = fileConverter;
            this.config = config;
        }



        public override async Task<string> Execute(IJob job)
        {
            // business logic
            // 
            var jobKey = job.Key;
            var docId = (int?)JObject.Parse(job.Variables)["TaskDocumentId"] ?? 0;
            var docData = (CreateOrUpdateDocument?)JObject.Parse(job.Variables)["NewDocument"]?.ToObject<CreateOrUpdateDocument>();
            var convertTo = (string?)JObject.Parse(job.Variables)["OutputExtension"]?.ToObject<string>();
            var templateKey = (string)JObject.Parse(job.Variables)["TemplateId"] ?? "";
            var templateId = await documentService.FindByUniqueId(null, templateKey, Domain.Enumerators.ContentType.Template);
            string variables = "{}";
            if ((docId > 0 || docData != null) && templateId > 0)
            {
                UserProfile su = UserProfile.SystemUser();
                var d = await documentService.Get(templateId);
                var contentinfo = await documentService.GetContentInfo(d.ImageId.Value);
                byte[] datacontent = null;
                datacontent = await documentService.GetContent(contentinfo.Id);
                if (datacontent == null)
                {
                    throw new FileNotFoundException("Impossibile accedere al file: " + contentinfo.FileName);
                }
                var dataString = System.Text.Encoding.UTF8.GetString(datacontent);
                var outputFilename = Path.GetFileName(contentinfo.OriginalFileName);
                dataString = HtmlTemplateParser.Parse(dataString, job.Variables)
                    .Replace("{{yyyy-MM-dd}}", DateTime.Now.ToString("yyyy-MM-dd"))
                    .Replace("{{YYYY-MM-DD}}", DateTime.Now.ToString("yyyy-MM-dd"))
                    .Replace("{{yyyy-mm-dd}}", DateTime.Now.ToString("yyyy-MM-dd"))
                    .Replace("{{Today}}", DateTime.Now.ToString("dd/MM/yyyy"))
                    .Replace("{{Date}}", DateTime.Now.ToString("dd/MM/yyyy"))
                    .Replace("{{Now}}", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss"));

                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };

                var image = config["Endpoint:FrontEnd"];
                if (image.EndsWith("/")) image = image.Substring(0, image.Length - 1);
                int i = dataString.IndexOf("\"/images/");
                while (i >= 0)
                {
                    string img = "data:image/jpeg;base64, ";
                    var j = dataString.IndexOf("\"", i + 1);
                    var url = dataString.Substring(i + 1, j - i - 1);

                    HttpClient httpClient = new HttpClient(handler);
                    try
                    {
                        var response = await httpClient.GetAsync(image + url);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            img += Convert.ToBase64String(await response.Content.ReadAsByteArrayAsync());
                            dataString = dataString.Substring(0, i) + "\"" + img + "\"" + dataString.Substring(j + 1);
                        }
                        else
                            dataString = dataString.Substring(0, i) + "\"\"" + dataString.Substring(j + 1);
                    }
                    catch (Exception ex)
                    {
                        dataString = dataString.Substring(0, i) + "\"\"" + dataString.Substring(j + 1);
                        logger.LogError("Errore in download logo: " + ex.Message);

                    }

                    i = dataString.IndexOf("\"/images/");
                }
                try
                {
                    HttpClient httpClient2 = new HttpClient(handler);
                    var response2 = await httpClient2.GetAsync(image + "/css/site.css");
                    if (response2.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var css = (await response2.Content.ReadAsStringAsync());
                        dataString = "<!DOCTYPE html><html><head><meta charset='UTF-16'/><style>\n" + css + "</style>\n</head><body>" + dataString + "</body></html>";
                    }
                }
                catch (Exception ex)
                {
                    dataString = "<!DOCTYPE html><html><head><meta charset='UTF-16'/><style>\n</style>\n</head><body>" + dataString + "</body></html>";
                    logger.LogError("Errore in download CSS: " + ex.Message);
                }

                var data = System.Text.Encoding.UTF8.GetBytes(dataString);
                //                    var data2 = System.Text.Encoding.Default.GetBytes(dataString);

                //TODO: conversione in pdf
                if (!String.IsNullOrEmpty(convertTo))
                {
                    var InputExtension = Path.GetExtension(contentinfo.FileName).ToLower();
                    var converter = await fileConverter.Get(InputExtension, convertTo);
                    if (converter != null)
                    {
                        using (var src = new MemoryStream(data))
                        {
                            try
                            {
                                var dest = await converter.Convert(InputExtension, src);
                                if (dest != null)
                                {
                                    var a = ((MemoryStream)dest).ToArray();
                                    if (a.Length > 0)
                                    {
                                        data = a;
                                        outputFilename = Path.ChangeExtension(outputFilename, convertTo);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.LogError("Errore in conversione Template -> PDF: " + ex.Message);
                                //variables = System.Text.Json.JsonSerializer.Serialize(new { OutputFile = "", errorMessage = ex.Message });
                            }
                        }
                    }
                }
                dataString = Convert.ToBase64String(data);
                var content = new FileContent()
                {
                    DataIsInBase64 = true,
                    FileData = dataString,
                    FileName = outputFilename,
                    LinkToContent = false
                };
                DocumentImage imageId = new();
                int newDocumentId = docId;
                if (docData != null)
                {
                    docData.Content = content;
                    newDocumentId = await documentService.Create(docData, su);
                    var doc = await documentService.Get(newDocumentId);
                    if (doc.ImageId != null)
                        imageId = await documentService.GetContentInfo(doc.ImageId.Value);
                }
                else
                {
                    imageId = await documentService.AddContent(docId, su, content, false);
                }
                variables = "{ \"NewDocumentId\" : " + newDocumentId + " , \"Image\" : " + System.Text.Json.JsonSerializer.Serialize(imageId) + " }";
                logger.LogDebug(TaskLabel + ": " + job.ProcessDefinitionKey + "\\" + job.ElementId + "\\" + jobKey + " - Instance: " + job.ProcessDefinitionKey + "\\" + job.ElementInstanceKey);
            }
            else
                variables = "{ \"NewDocumentId\" : " + docId + " , \"Image\" : {} }";
            return variables;
        }

        public override async Task<TaskItem> PaletteItem()
        {
            var inputs = new List<InputParameter>
            {
                new InputParameter
                {
                    Name = "TaskDocumentId",
                    Required = true,
                    InputType = 0,
                    Label = "Id del Documento",
                    Description = "Id del documento per compilare il Template",
                    Values = "",
                    DefaultValue = "DocumentId"
                },
                new InputParameter
                {
                    Name = "TemplateId",
                    Required = true,
                    InputType = 0,
                    Label = " Id Template",
                    Description = "Id del Template che va riempito",
                    Values = "",
                    DefaultValue = "\"\""
                },
                new InputParameter
                {
                    Name = "Document",
                    Required = false,
                    InputType = 0,
                    Label = "Documento",
                    Description = "Contenuto del documento (lasciare vuoto per modificare il contenuto del documento 'TaskDocumentId')",
                    Values = "",
                    DefaultValue = "{}"
                },
                new InputParameter
                {
                    Name = "OutputExtension",
                    Required = false,
                    InputType = 0,
                    Label = "Estensione d'uscita",
                    Description = "Estensione d'uscita dopo il completamento del Template",
                    Values = "",
                    DefaultValue = "\".pdf\""
                },

            };
            var outputs = new List<OutputParameter>
            {
                new OutputParameter
                {
                    Name = "NewDocumentId",
                    DefaultValue = "NewDocumentId",
                    Required = false,
                    Label = "ID nuovo Documento",
                    Description = "Id del Documento dopo il completamento del Template"
                }
            };

            var taskItem = new TaskItem
            {
                Id = this.JobType,
                TaskServiceId = this.JobType,
                Group = this.GroupName,
                Name = this.JobType,
                AuthenticationType = 0,
                JobWorker = this.JobType,
                Label = this.TaskLabel,
                Description = this.TaskLabel,
                Icon = this.Icon,
                ColorStroke = "",
                ColorFill = "",
                Inputs = inputs,
                Outputs = outputs

            };

            return taskItem;
        }

    }
}
