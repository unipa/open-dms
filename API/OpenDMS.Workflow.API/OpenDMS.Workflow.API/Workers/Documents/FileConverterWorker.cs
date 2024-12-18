using Elmi.Core.FileConverters;
using Newtonsoft.Json.Linq;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;

namespace OpenDMS.Workflow.API.Workers.Documents
{
    public class FileConverterWorker : BaseWorker
    {
        private readonly IDocumentService documentService;
        private readonly IVirtualFileSystemProvider virtualFileSystem;
        private readonly IFileConvertFactory fileConverter;
        private readonly IAppSettingsRepository appSettings;

        public override string JobType { get; set; } = "FileConverterTask";
        public override string TaskLabel { get; set; } = "Converte un file da un formato all'altro (PDF di default)";
        public override string Icon { get; set; } = "fa fa-file-pdf-o";
        public override string GroupName { get; set; } = "Documenti";
        public override string[] AlternativeTasks { get; set; } = { };

        public override string Inputs { get; set; } = "TaskDocumentId, InputFile, OutputExtension, DeleteTempFile";
        public override string Outputs { get; set; } = "OutputFile, errorMessage";

        public override int MaxJobs { get; set; } = 1;
        public override int PollingInterval { get; set; } = 1;
        public override int TimeOut { get; set; } = 60;





        public FileConverterWorker(
            ILogger<FileConverterWorker> logger,
            IWorkflowEngine engine,
            IDocumentService documentService,
            ISearchService documentSearchRepository,
            IVirtualFileSystemProvider virtualFileSystem,
            IFileConvertFactory fileConverter,
           IAppSettingsRepository appSettings) : base(logger, engine, appSettings)
        {
            this.documentService = documentService;
            this.virtualFileSystem = virtualFileSystem;
            this.fileConverter = fileConverter;
            this.appSettings = appSettings;
        }



        public void Parse(string variables, Action<string, string> Callback, string prefix = "")
        {
            foreach (var var in JObject.Parse(variables))
            {
                var vartype = var.Value.Type.ToString();
                var varvalue = var.Value.ToString();
                var varname = prefix + var.Key;
                if (vartype == "Object")
                {
                    Parse(varvalue, Callback, varname + ".");
                }
                else
                if (vartype == "Array")
                {
                    var array = JArray.Parse(varvalue);
                    for (int i = 0; i < array.Count; i++)
                    {
                        Parse(varvalue, Callback, varname + "[" + i + "]");
                    }
                }
                else
                {
                    Callback(varname, varvalue);
                }
            }

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
                    Label = "Id Documento",
                    Description = "Id del documento da Convertire",
                    Values = "",
                    DefaultValue = ""
                },
                new InputParameter
                {
                    Name = "InputFile",
                    Required = true,
                    InputType = 0,
                    Label = "File di Input",
                    Description = "Percorso al file che deve essere convertito",
                    Values = "",
                    DefaultValue = ""
                },
                new InputParameter
                {
                    Name = "OutputExtension",
                    Required = true,
                    InputType = 0,
                    Label = "Estensione di Output",
                    Description = "Nuova estensione del file dopo la conversione",
                    Values = "",
                    DefaultValue = "'.pdf'"
                },
                new InputParameter
                {
                    Name = "DeleteTempFile",
                    Required = false,
                    InputType = 0,
                    Label = "File temporaneo da Eliminare",
                    Description = "Scelta se eliminare il file dopo la conversione",
                    Values = "",
                    DefaultValue = "0"
                },

            };
            var outputs = new List<OutputParameter>
            {
                new OutputParameter
                {
                    Name = "OutputFile",
                    Label = "File di Output",
                    Description = "Percorso al file convertito"
                },
                new OutputParameter
                {
                    Name = "errorMessage",
                    DefaultValue = "",
                    Required = false,
                    Label = "Messaggio di Errore",
                    Description = "Messaggio di errore se il processo fallisce"
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

        public override async Task<string> Execute(IJob job)
        {
            // business logic
            // 
            var jobKey = job.Key;
            byte[] data = null;
            string InputFile = (string?)JObject.Parse(job.Variables)["InputFile"] ?? "";
            var fm = await virtualFileSystem.InstanceOf((await appSettings.Get("Documents.FileSystemType")) + "");
            var basePath = (await appSettings.Get("Documents.TempFolder")) + "";
            var docId = (int?)JObject.Parse(job.Variables)["TaskDocumentId"] ?? 0;
            //if (docId == 0) docId = (int?)JObject.Parse(job.Variables)["DocumentId"] ?? 0;
            if (string.IsNullOrEmpty(basePath))
            {
                basePath = "Temp";
            }
            if (!String.IsNullOrEmpty(InputFile) && await fm.Exists(InputFile))
            {
                data = await fm.ReadAllBytes(InputFile);
                int i = 0;
                int.TryParse((string)JObject.Parse(job.Variables)["DeleteTempFile"], out i);
                bool delete = i > 0;
                if (delete) await fm.Delete(InputFile);
            }
            else
            {
                if (docId > 0)
                {
                    var d = await documentService.Get(docId);
                    var contentinfo = await documentService.GetContentInfo(d.ImageId.Value);
                    data = await documentService.GetContent(contentinfo.Id);
                    InputFile = contentinfo.FileName;
                }
            }
            var InputExtension = Path.GetExtension(InputFile); // JObject.Parse(job.Variables)["InputExtension"]?.ToObject<string?>() ?? "";
            if (String.IsNullOrEmpty(InputExtension)) InputExtension = ".html";
            var OutputExtension = JObject.Parse(job.Variables)["OutputExtension"]?.ToObject<string>() ?? "";
            if (String.IsNullOrEmpty(OutputExtension)) OutputExtension = ".pdf";
            string variables = "";
            if (data != null && data.Length > 0)
            {
                UserProfile u = UserProfile.SystemUser();
                var converter = await fileConverter.Get(InputExtension, OutputExtension);
                if (converter != null)
                {
                    using (var src = new MemoryStream(data))
                    {
                        try
                        {
                            var dest = await converter.Convert(InputExtension, src);
                            if (dest != null)
                            {
                                var outputdata = ((MemoryStream)dest).ToArray();
                                string OutputFile = Path.Combine(basePath, "CONVERT_" + Guid.NewGuid().ToString() + OutputExtension);
                                if (docId > 0)
                                {
                                    OutputFile = Path.ChangeExtension(Path.GetFileName(InputFile), OutputExtension);
                                    FileContent fc = new FileContent()
                                    {
                                        FileName = OutputFile,
                                        FileData = Convert.ToBase64String(outputdata),
                                        DataIsInBase64 = true
                                    };
                                    await documentService.AddContent(docId, UserProfile.SystemUser(), fc, false);
                                }
                                else
                                {
                                    await fm.WriteAllBytes(OutputFile, outputdata);
                                }
                                variables = System.Text.Json.JsonSerializer.Serialize(new { OutputFile = OutputFile });
                            }
                        }
                        catch (Exception ex)
                        {
                            variables = System.Text.Json.JsonSerializer.Serialize(new { OutputFile = "", errorMessage = ex.Message });
                        }
                    }

                }
                else
                    variables = System.Text.Json.JsonSerializer.Serialize(new { OutputFile = "", errorMessage = $"Nessun convertitore da '{InputExtension}' a '{OutputExtension}'" });
            }
            else
                variables = System.Text.Json.JsonSerializer.Serialize(new { OutputFile = "", errorMessage = $"File '{InputFile}' non tovato" });

            return variables;
        }
    }
}
