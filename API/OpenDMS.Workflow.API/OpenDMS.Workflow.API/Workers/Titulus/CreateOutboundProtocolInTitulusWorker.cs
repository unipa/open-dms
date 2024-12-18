using Core.TitulusIntegration;
using Core.TitulusIntegration.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using OpenDMS.Workflow.API.BusinessLogic.TitulusBL.Interfacce;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;

namespace OpenDMS.Workflow.API.Workers.Titulus
{
    public class CreateOutboundProtocolInTitulusWorker : BaseWorker
    {
        public readonly ITitulusBL _bl;
        private readonly IConfiguration configuration;
        private readonly IUserService userContext;
        private readonly IDocumentService documentService;

        public override string JobType { get; set; } = "CreateOutboundProtocolInTitulus";
        public override string TaskLabel { get; set; } = "Crea un nuovo protocollo in partenza su Titulus";
        public override string Icon { get; set; } = "fa-regular fa-sign-out";
        public override string GroupName { get; set; } = "Titulus";
        public override string[] AlternativeTasks { get; set; } = { };

        public override string Inputs { get; set; } = "debug,documentId,bozza,Oggetto,Tipologia,Note, MezzoTrasmissione, MezzoTrasmissioneValuta, MezzoTrasmissioneCosto VoceIndice, PersoneaInterna, PersoneEsterneCC, StruttureEsterne, StruttureEsterneCC, PersoneEsterne, Attachments";
        public override string Outputs { get; set; } = "newDocument,protocolDate,protocolNumber";
        public override int MaxJobs { get; set; } = 1;
        public override int PollingInterval { get; set; } = 1;
        public override int TimeOut { get; set; } = 60;

        public CreateOutboundProtocolInTitulusWorker(
            ILogger<CreateOutboundProtocolInTitulusWorker> logger,
            IConfiguration configuration,
            IWorkflowEngine engine,
            IUserService userContext,
            IDocumentService documentService,
            ITitulusBL bl,
            IAppSettingsRepository appSettings) : base(logger, engine, appSettings)
        {
            this.configuration = configuration;
            this.userContext = userContext;
            this.documentService = documentService;
            _bl = bl;
        }



        public override async Task<string> Execute(IJob job)
        {
            JObject jsonObject = JObject.Parse(job.Variables);
            int i = 0;
            int.TryParse((string)jsonObject["debug"], out i);
            bool debug = i > 0;

            int.TryParse((string)jsonObject["bozza"], out i);
            bool bozza = i > 0;

            OutboundDocument document = new OutboundDocument()
            {
                CriterioRicercaPersonaInterna = CustomDataEnum.PersoneInterneDaLogin,
                CriterioRicercaPersonaEsterna = CustomDataEnum.PersoneInterneDaLogin,
                CriterioRicercaStrutturaEsterna = CustomDataEnum.StruttureInterneDaCodice
            };

            document.Oggetto = (string)jsonObject["Oggetto"] ?? "";
            while (document.Oggetto.Length < 30) document.Oggetto += ".";

            document.Note = "" + (string)jsonObject["Note"];
            document.Tipologia = "" + (string)jsonObject["Tipologia"];

            var list1 = jsonObject["StruttureEsterne"]?.ToObject<string>() ?? "";
            var list2 = jsonObject["StruttureEsterneCC"]?.ToObject<string>() ?? "";
            var list3 = jsonObject["PersoneEsterne"]?.ToObject<string>() ?? "";
            var list4 = jsonObject["PersoneEsterneCC"]?.ToObject<string>() ?? "";


            document.PersonaInterna = "" + (string)jsonObject["PersonaInterna"]; 
            document.StruttureEsterne = list1.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList(); 
            document.StruttureEsterneCC = list2.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList(); 
            document.PersoneEsterne = list3.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList(); 
            document.PersoneEsterneCC = list4.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

            document.VoceIndice = "" + (string)jsonObject["VoceIndice"];  //"UWEB - Autorizzazione Missione",
            document.MezzoTrasmissione = "" + (string)jsonObject["MezzoTrasmissione"];
            document.MezzoTrasmissioneCosto = "" + (string)jsonObject["MezzoTrasmissioneCosto"];
            document.MezzoTrasmissioneValuta = "" + (string)jsonObject["MezzoTrasmissioneValuta"];
            document.NumeroRepertorio = "" + (string)jsonObject["NumeroRepertorio"];
            document.Repertorio = "" + (string)jsonObject["Repertorio"];
            document.Attachments = new();
            int documentId = 0;
            int.TryParse((string)jsonObject["TaskDocumentId"], out documentId);
            List<int> Lista = new List<int>();
            try
            {
                Lista = jsonObject["Attachments"].ToObject<int[]>().ToList();
            }
            catch (Exception)
            {
                try
                {
                    Lista = (jsonObject["Attachments"].ToObject<string>()).Split(",").ToList().Select<string, int>(s => int.Parse(s)).ToList();
                }
                catch
                {

                }
            }
            if (documentId > 0)
            {
                Lista.Insert(0, documentId);
            }
            foreach (var l in Lista)
            {
                var doc = await documentService.Get(l);
                if (doc.ImageId.HasValue)
                {
                    var info = await documentService.GetContentInfo(doc.ImageId.Value);
                    var fileContent = await documentService.GetContent(doc.ImageId.Value);
                    if (fileContent == null)
                    {
                        throw new FileNotFoundException("Impossibile accedere al file: " + info.FileName);
                    }
                    TitulusIntegration.Titulus.AttachmentBean A = new() { content = fileContent, description = doc.Description+"", fileName = Path.GetFileName(info.FileName) };
                    document.Attachments.Add(A);
                }
            }

            string url = configuration["Services:titulus4_services_url"];
            string username = configuration["Services:username"];
            string password = configuration["Services:password"];

            var TitulusService = new Titulus4(url, username, password);
            var response = TitulusService.SaveOutboundProtocol(document, bozza);

            string variablesJson = "{}";
            if (!String.IsNullOrEmpty(response.IdInterno))
            {
                var u = await userContext.GetUserProfile(SpecialUser.SystemUser);
                ProtocolInfo protocolInfo = new ProtocolInfo();
                protocolInfo.ProtocolUser = response.physdoc; // r.Document.Physdoc;
                int n = 0;
                if (response.NumeroProtocollo > 0) 
                    protocolInfo.Number = response.Protocollo.ToString();
                protocolInfo.ExternalProtocolURL = response.URL;// r.Url;
                protocolInfo.Date = response.DataProtocollo;
                protocolInfo.ProtocolUser = u.userId;

                var documentInfo = await documentService.Protocol(Lista[0], protocolInfo, u);
            }
            var variables = new
            {
                newDocument = response, //r,
                protocolNumber = response.Protocollo,// r.Document.doc.NumProt,
                protocolDate = response.DataProtocollo// DateTime.Now.Date
            };
            // Serializzazione dell'oggetto in formato JSON
            variablesJson = JsonConvert.SerializeObject(variables);
            return variablesJson;
        }

                

        public override async Task<TaskItem> PaletteItem()
        {
            var inputs = new List<InputParameter>
            {
                new InputParameter
                {
                        Name = "debug",
                        Required = false,
                        InputType = 0,
                        Label = "debug",
                        Description = "Debug Flag",
                        Values = "",
                        DefaultValue = "0"
                },
                new InputParameter
                {
                        Name = "TaskDocumentId",
                        Required = false,
                        InputType = 0,
                        Label = "Document Id",
                        Description = "Id del documento",
                        Values = "",
                        DefaultValue = "DocumentId"
                },
                new InputParameter
                {
                        Name = "bozza",
                        Required = false,
                        InputType = 0,
                        Label = "Bozza",
                        Description = "Bozza Flag",
                        Values = "",
                        DefaultValue = "0"
                },
                new InputParameter
                {
                        Name = "Oggetto",
                        Required = true,
                        InputType = 0,
                        Label = "Oggetto",
                        Description = "Oggetto del protocollo",
                        Values = "",
                        DefaultValue = "\"\""
                },
                new InputParameter
                {
                        Name = "Tipologia",
                        Required = false,
                        InputType = 0,
                        Label = "Tipologia",
                        Description = "Tipologia del documento",
                        Values = "",
                        DefaultValue = "\"\""
                },
                new InputParameter
                {
                        Name = "Note",
                        Required = false,
                        InputType = 0,
                        Label = "Note",
                        Description = "Note",
                        Values = "",
                        DefaultValue = "\"\""
                },
                new InputParameter
                {
                        Name = "MezzoTrasmissione",
                        Required = false,
                        InputType = 0,
                        Label = "MezzoTrasmissione",
                        Description = "Mezzo di Trasmissione",
                        Values = "",
                        DefaultValue = "\"\""
                },
                new InputParameter
                {
                        Name = "MezzoTrasmissioneValuta",
                        Required = false,
                        InputType = 0,
                        Label = "MezzoTrasmissioneValuta",
                        Description = "Mezzo di TrasmissioneValuta",
                        Values = "",
                        DefaultValue = "\"\""
                },
                new InputParameter
                {
                        Name = "MezzoTrasmissioneCosto",
                        Required = false,
                        InputType = 0,
                        Label = "MezzoTrasmissioneCosto",
                        Description = "Costo Mezzo di Trasmissione",
                        Values = "",
                        DefaultValue = "\"\""
                },
                new InputParameter
                {
                        Name = "VoceIndice",
                        Required = false,
                        InputType = 0,
                        Label = "VoceIndice",
                        Description = "Voce di Indice",
                        Values = "",
                        DefaultValue = "\"\""
                },
                new InputParameter
                {
                        Name = "PersoneaInterne",
                        Required = false,
                        InputType = 0,
                        Label = "PersoneInterne",
                        Description = "Elenco Persone Interne (Login)",
                        Values = "",
                        DefaultValue = "[]"
                },
                new InputParameter
                {
                        Name = "PersoneEsterneCC",
                        Required = false,
                        InputType = 0,
                        Label = "PersoneEsterneCC",
                        Description = "Elenco Persone Esterne CC (Login)",
                        Values = "",
                        DefaultValue = "[]"
                },
                new InputParameter
                {
                        Name = "StruttureEsterne",
                        Required = false,
                        InputType = 0,
                        Label = "StruttureEsterneCC",
                        Description = "Elenco Strutture Esterne",
                        Values = "",
                        DefaultValue = "[]"
                },
                new InputParameter
                {
                        Name = "StruttureEsterneCC",
                        Required = false,
                        InputType = 0,
                        Label = "StruttureEsterneCC",
                        Description = "Elenco Strutture Esterne CC",
                        Values = "",
                        DefaultValue = "[]"
                },
                new InputParameter
                {
                        Name = "PersoneEsterne",
                        Required = false,
                        InputType = 0,
                        Label = "PersoneEsterne",
                        Description = "Elenco Persone Esterne (Login)",
                        Values = "",
                        DefaultValue = "[]"
                },
                new InputParameter
                {
                       Name = "Attachments",
                        Required = false,
                        InputType = 0,
                        Label = "Allegati",
                        Description = "Allegati",
                        Values = "",
                        DefaultValue = "[]"
                }
            };
            var outputs = new List<OutputParameter>
            {
                new OutputParameter
                {
                    Name = "newDocument",
                    DefaultValue = "newDocument",
                    Required = false,
                    Label = "New Document",
                    Description = "Nuovo documento su Titulus"
                },
                new OutputParameter
                {
                    Name = "protocolDate",
                    DefaultValue = "protocolDate",
                    Required = false,
                    Label = "Protocol Date",
                    Description = "Data del protocollo"
                },
                new OutputParameter
                {
                    Name = "protocolNumber",
                    DefaultValue = "protocolNumber",
                    Required = false,
                    Label = "Protocol Number",
                    Description = "Numero del protocollo"
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
