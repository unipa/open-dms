using Newtonsoft.Json.Linq;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;

namespace OpenDMS.Workflow.API.Workers.Documents
{
    public class SetPermissionWorker : BaseWorker
    {
        private readonly ILogger<SetPermissionWorker> logger;
        private readonly IDocumentService documentService;

        public override string JobType { get; set; } = "setPermissionTask";
        public override string TaskLabel { get; set; } = "Crea o Modifica un set di permessi su un Documento";
        public override string Icon { get; set; } = "fa fa-lock";
        public override string GroupName { get; set; } = "Documenti";
        public override string[] AlternativeTasks { get; set; } = { };

        public override string Inputs { get; set; } = "TaskDocumentId,Profiles,Permissions_Granted,Permissions_Removed,Permissions_Denied";
        public override string Outputs { get; set; } = "Granted,Removed,Denied";

        public override int MaxJobs { get; set; } = 1;
        public override int PollingInterval { get; set; } = 1;
        public override int TimeOut { get; set; } = 60;





        public SetPermissionWorker(
            ILogger<SetPermissionWorker> logger,
            IWorkflowEngine engine,
            IDocumentService documentService,
           IAppSettingsRepository appSettings) : base(logger, engine, appSettings)
        {
            this.logger = logger;
            this.documentService = documentService;
        }


        public override async Task<string> Execute(IJob job)
        {
            // business logic
            // 
            var jobKey = job.Key;
            var docId = (int?)JObject.Parse(job.Variables)["TaskDocumentId"] ?? 0;
            // if (docId == 0) docId = (int?)JObject.Parse(job.Variables)["DocumentId"] ?? 0;
            var profiles = (string?)JObject.Parse(job.Variables)["Profiles"];
            var PermissionsGranted = (string?)JObject.Parse(job.Variables)["Permissions_Granted"];
            var PermissionsRemoved = (string?)JObject.Parse(job.Variables)["Permissions_Removed"];
            var PermissionsDenied = (string?)JObject.Parse(job.Variables)["Permissions_Denied"];
            int Granted = 0;
            int Removed = 0;
            int Denied = 0;
            if (docId > 0 && profiles != null && (PermissionsGranted != null || PermissionsDenied != null))
            {
                UserProfile u = UserProfile.SystemUser();
                foreach (var profile in profiles.Split(","))
                {
                    var ProfileId = profile.Substring(1);
                    var ProfileType = (ProfileType)int.Parse(profile.Substring(0, 1));
                    if (PermissionsGranted != null)
                        foreach (var permissionId in PermissionsGranted.Split(","))
                            try
                            {
                                await documentService.SetPermission(docId, u, ProfileId, ProfileType, permissionId, Domain.Enumerators.AuthorizationType.Granted);
                                Granted++;
                            }
                            catch (Exception)
                            {
                            }
                    if (PermissionsRemoved != null)
                        foreach (var permissionId in PermissionsRemoved.Split(","))
                            try
                            {
                                await documentService.SetPermission(docId, u, ProfileId, ProfileType, permissionId, Domain.Enumerators.AuthorizationType.None);
                                Removed++;
                            }
                            catch (Exception)
                            {
                            }
                    if (PermissionsDenied != null)
                        foreach (var permissionId in PermissionsDenied.Split(","))
                            try
                            {
                                await documentService.SetPermission(docId, u, ProfileId, ProfileType, permissionId, Domain.Enumerators.AuthorizationType.Denied);
                                Denied++;
                            }
                            catch (Exception)
                            {
                            }

                }
                logger.LogDebug(TaskLabel + ": " + job.ProcessDefinitionKey + "\\" + job.ElementId + "\\" + jobKey + " - Instance: " + job.ProcessDefinitionKey + "\\" + job.ElementInstanceKey);
            }
            var variables = $"{{ \"Granted\" : {Granted}, \"Removed\" : {Removed}, \"Denied\" : {Denied} }}";
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
                    Label = "Id documento Attività",
                    Description = "Id del documento",
                    Values = "",
                    DefaultValue = "DocumentId"
                },
                new InputParameter
                {
                    Name = "Profiles",
                    Required = true,
                    InputType = 0,
                    Label = "Profilo",
                    Description = "Elenco di profili <tipo><id> separati da virgola (es. \"0admin,2user\")",
                    Values = "",
                    DefaultValue = "\"\""
                },
                new InputParameter
                {
                    Name = "Permissions_Granted",
                    Required = false,
                    InputType = 0,
                    Label = "Permessi Autorizzati",
                    Description = "Lista di permessi autorizzati",
                    Values = "",
                    DefaultValue = "\"Document.AddContent, Document.RemoveContent\""
                },
                new InputParameter
                {
                    Name = "Permission_Removed",
                    Required = false,
                    InputType = 0,
                    Label = "Permessi Rimossi",
                    Description = "Lista dei permessi rimossi",
                    Values = "",
                    DefaultValue = "\"\""
                },
                new InputParameter
                {
                    Name = "Permission_Denied",
                    Required = false,
                    InputType = 0,
                    Label = "Permessi Negati",
                    Description = "Lista dei permessi Negati",
                    Values = "",
                    DefaultValue = "\"\""
                }
            };
            var outputs = new List<OutputParameter>
            {
                //new OutputParameter
                //{
                //    Name = "Granted",
                //    DefaultValue = "",
                //    Required = false,
                //    Label = "Autorizzati",
                //    Description = "Numeri di permessi Autorizzati"
                //},
                //new OutputParameter
                //{
                //    Name = "Removed",
                //    DefaultValue = "",
                //    Required = false,
                //    Label = "Rimossi",
                //    Description = "Numeri di permessi Rimossi"
                //},
                //new OutputParameter
                //{
                //    Name = "Denied",
                //    DefaultValue = "",
                //    Required = false,
                //    Label = "Negati",
                //    Description = "Numeri di permessi Negati"
                //}
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
