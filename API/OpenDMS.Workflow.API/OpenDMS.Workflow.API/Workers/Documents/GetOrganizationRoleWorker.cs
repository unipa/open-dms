using Newtonsoft.Json.Linq;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.OrganizationUnits;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;

namespace OpenDMS.Workflow.API.Workers.Documents
{
    public class GetOrganizationRoleWorker : BaseWorker
    {
        private readonly ILogger<GetOrganizationRoleWorker> logger;
        private readonly IWorkflowEngine client;
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly IOrganizationUnitService organizationUnitService;
        private readonly IAppSettingsRepository appSettings;

        public override string JobType { get; set; } = "GetOrganizationRoleTask";
        public override string TaskLabel { get; set; } = "Recupera un nodo o un ruolo di un nodo";
        public override string Icon { get; set; } = "fa fa-sitemap";
        public override string GroupName { get; set; } = "Organigramma";
        public override string[] AlternativeTasks { get; set; } = { };

        public override string Inputs { get; set; } = "UserId,GroupId, GroupExternalId, GroupName, RoleIdToFind, RoleToFind, Skip,Recursive,DefaultRoleId,DefaultGroupId";
        public override string Outputs { get; set; } = "Role, UserGroup";

        public override int MaxJobs { get; set; } = 1;
        public override int PollingInterval { get; set; } = 1;
        public override int TimeOut { get; set; } = 60;



        public GetOrganizationRoleWorker(
            ILogger<GetOrganizationRoleWorker> logger,
            IWorkflowEngine engine,
            IUserService userService,
            IRoleService roleService,
            IOrganizationUnitService organizationUnitService,
           IAppSettingsRepository appSettings) : base(logger, engine, appSettings)
        {
            this.logger = logger;
            this.userService = userService;
            this.roleService = roleService;
            this.organizationUnitService = organizationUnitService;
        }



        public override async Task<string> Execute(IJob job)
        {
            // business logic
            // 
            var jobKey = job.Key;
            var UserId = (string?)JObject.Parse(job.Variables)["UserId"] ?? "";
            var GroupId = (string?)JObject.Parse(job.Variables)["GroupId"] ?? "";
            var GroupEId = (string?)JObject.Parse(job.Variables)["GroupExternalId"] ?? "";
            var GroupName = (string?)JObject.Parse(job.Variables)["GroupName"] ?? "";
            var RoleId = (string?)JObject.Parse(job.Variables)["RoleIdToFind"] ?? "";
            var RoleName = (string?)JObject.Parse(job.Variables)["RoleToFind"] ?? "";
            var Skip = (int?)JObject.Parse(job.Variables)["Skip"] ?? 0;
            var Recursive = (int?)JObject.Parse(job.Variables)["Recursive"] ?? 1;
            var IgnoreUserId = (int?)JObject.Parse(job.Variables)["IgnoreUserId"] ?? 1;

            var DefaultRoleId = (string?)JObject.Parse(job.Variables)["DefaultRoleId"] ?? "";
            var DefaultRoleName = (string?)JObject.Parse(job.Variables)["DefaultRole"] ?? "";
            var DefaultGroupId = (string?)JObject.Parse(job.Variables)["DefaultGroupId"] ?? "";
            //var DefaultUsers = (string?)JObject.Parse(job.Variables)["DefaultUsers"] ?? "";

            // Cerco la struttura di riferimento se non è stata indicata
            OrganizationNodeInfo group = null;
            if (!String.IsNullOrEmpty(GroupId))
            {
                group = await organizationUnitService.GetById(GroupId);
            }
            else
            {
                if (!String.IsNullOrEmpty(GroupEId))
                {
                    group = await organizationUnitService.GetByExternalId(GroupEId);
                }
                else
                {
                    if (!String.IsNullOrEmpty(GroupName))
                    {
                        group = await organizationUnitService.GetByName(GroupName);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(UserId))
                        {
                            var user = await userService.GetUserProfile(UserId);
                            if (user != null)
                            {
                                if (!String.IsNullOrEmpty(DefaultGroupId))
                                {
                                    // Verifico se il gruppo di default è presente tra le strutture dell'utente
                                    var gdid = user.Groups.FirstOrDefault (g => g.Id == DefaultGroupId);
                                    if (gdid != null)
                                    {
                                        GroupId = gdid.Id;
                                        group = await organizationUnitService.GetById(GroupId);
                                    }
                                }
                                if (group == null)
                                {
                                    var gid = user.Groups.LastOrDefault(g => !String.IsNullOrEmpty(g.Description));
                                    if (gid != null)
                                    {
                                        GroupId = gid.Id;
                                        group = await organizationUnitService.GetById(GroupId);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            while (group != null && Skip > 0)
            {
                var parentId = group.ParentUserGroupId;
                group = await organizationUnitService.GetById(parentId);
                Skip--;
            }
            var ok = group != null;
            if (ok)
            {
                // Cerco il ruolo indicato nel primo gruppo trovato
                var users = (await organizationUnitService.GetUsers(group.UserGroupId)).Where(r => r.RoleId.Equals(RoleId,StringComparison.InvariantCultureIgnoreCase) || (!String.IsNullOrEmpty(RoleName) && r.RoleName.Equals(RoleName, StringComparison.InvariantCultureIgnoreCase)));
                // Ok se trovo almeno un utente con il ruolo indicato purchè non sia lo stesso utente
                ok = users.Where(u=>u.UserId.ToLower() != UserId.ToLower() || IgnoreUserId==0).Count() > 0;
                if (ok)
                {
                    var u = users.First();
                    RoleId = u.RoleId;
                    RoleName = u.RoleName;
                }
                var parentGroup = group;
                while (!ok && Recursive > 0 && group != null)
                {
                    var parentId = group.ParentUserGroupId;
                    group = await organizationUnitService.GetById(parentId);
                    if (group != null)
                    {
                        users = (await organizationUnitService.GetUsers(group.UserGroupId)).Where(r => r.RoleId.Equals(RoleId, StringComparison.InvariantCultureIgnoreCase) || (!String.IsNullOrEmpty(RoleName) && r.RoleName.Equals(RoleName, StringComparison.InvariantCultureIgnoreCase)));
                        ok = users.Where(u => u.UserId.ToLower() != UserId.ToLower() || IgnoreUserId == 0).Count() > 0;
                        if (ok)
                        {
                            var u = users.First();
                            RoleId = u.RoleId;
                            RoleName = u.RoleName;
                        }
                    }
                }
                if (!ok && !String.IsNullOrEmpty(DefaultGroupId))
                {
                    // cerco il ruolo nel gruppo di default 
                    group = await organizationUnitService.GetById(DefaultGroupId);
                    if (group != null)
                    {
                        users = (await organizationUnitService.GetUsers(group.UserGroupId)).Where(r => r.RoleId.Equals(RoleId, StringComparison.InvariantCultureIgnoreCase) || (!String.IsNullOrEmpty(RoleName) && r.RoleName.Equals(RoleName, StringComparison.InvariantCultureIgnoreCase)));
                        ok = users.Where(u => u.UserId.ToLower() != UserId.ToLower() || IgnoreUserId == 0).Count() > 0;
                        if (ok)
                        {
                            var u = users.First();
                            RoleId = u.RoleId;
                            RoleName = u.RoleName;
                        }
                    }
                }
                if (!ok && !String.IsNullOrEmpty(DefaultGroupId))
                {
                    // cerco il ruolo nel gruppo di default 
                    group = await organizationUnitService.GetById(DefaultGroupId);
                }
                if (!ok && !String.IsNullOrEmpty(DefaultRoleId) && group != null)
                {
                    // cerco i valori di default (ruolo e gruppo)
                    users = (await organizationUnitService.GetUsers(group.UserGroupId)).Where(r => r.RoleId.Equals(DefaultRoleId, StringComparison.InvariantCultureIgnoreCase));
                    ok = users.Where(u => u.UserId.ToLower() != UserId.ToLower() || IgnoreUserId == 0).Count() > 0;
                    if (ok)
                    {
                        var u = users.First();
                        RoleId = u.RoleId;
                        RoleName = u.RoleName;
                    }
                }
                if (!ok && !String.IsNullOrEmpty(DefaultRoleName) && group != null)
                {
                    // cerco i valori di default (ruolo e gruppo)
                    users = (await organizationUnitService.GetUsers(group.UserGroupId)).Where(r => r.RoleName.Equals(DefaultRoleName, StringComparison.InvariantCultureIgnoreCase));
                    ok = users.Where(u => u.UserId.ToLower() != UserId.ToLower() || IgnoreUserId == 0).Count() > 0;
                    if (ok)
                    {
                        var u = users.First();
                        RoleId = u.RoleId;
                        RoleName = u.RoleName;
                    }
                }
                if (ok)
                {
                    var variables = System.Text.Json.JsonSerializer.Serialize(
                        new
                        {
                            Role = new Role()
                            {
                                Id = RoleId + "\\" + group.UserGroupId,
                                RoleName = RoleName,
                                ExternalApp = RoleId
                            },
                            Group = group,
                        });
                    return variables;
                }
            }
            if (!ok)
            {
                if (!String.IsNullOrEmpty(DefaultGroupId))
                {
                    // cerco il ruolo nel gruppo di default 
                    group = await organizationUnitService.GetById(DefaultGroupId);
                }
                if (group == null)
                    group = await organizationUnitService.GetById(null);
                if (group == null)
                    group = new OrganizationNodeInfo() { UserGroupId = "", ShortName = "", Name = "", ExternalId = "" };

                if (!String.IsNullOrEmpty(DefaultRoleId))
                {
                    var r = await roleService.GetById(DefaultRoleId);
                    if (r != null)
                    {
                        RoleId = DefaultRoleId;
                        RoleName = r.RoleName;
                    }
                }
                else
                if (!String.IsNullOrEmpty(DefaultRoleName))
                {
                    var r = await roleService.GetByName(DefaultRoleName);
                    RoleId = r.Id;
                    RoleName = r.RoleName;
                };
            }
            var Novariables = System.Text.Json.JsonSerializer.Serialize(
            new
            {
                Role = new Role() { Id = RoleId+"\\"+group.UserGroupId, RoleName = RoleName, ExternalApp = RoleId },
                Group = group
            });
            return Novariables;
        }


        public override async Task<TaskItem> PaletteItem()
        {
            var inputs = new List<InputParameter>
            {
                new InputParameter
                {
                    Name = "UserId",
                    Required = true,
                    InputType = 0,
                    Label = "Id Utente",
                    Description = "Id dell'utente",
                    Values = "",
                    DefaultValue = "userId"
                },
                new InputParameter
                {
                    Name = "GroupId",
                    Required = false,
                    InputType = 0,
                    Label = "Id Gruppo",
                    Description = "Id Interno del Gruppo",
                    Values = "",
                    DefaultValue = "\"\""
                },
                new InputParameter
                {
                    Name = "GroupExternalId",
                    Required = false,
                    InputType = 0,
                    Label = "Id Esterno del Gruppo",
                    Description = "Id Esterno del Gruppo",
                    Values = "",
                    DefaultValue = "\"\""
                },
                new InputParameter
                {
                    Name = "GroupName",
                    Required = false,
                    InputType = 0,
                    Label = "Nome Gruppo",
                    Description = "Nome del Gruppo",
                    Values = "",
                    DefaultValue = "\"\""
                },
                new InputParameter
                {
                    Name = "RoleIdToFind",
                    Required = false,
                    InputType = 0,
                    Label = "Id del ruolo da trovare",
                    Description = "Id del ruolo da trovare",
                    Values = "",
                    DefaultValue = "\"user\""
                },
                new InputParameter
                {
                    Name = "RoleToFind",
                    Required = false,
                    InputType = 0,
                    Label = "Ruolo da trovare",
                    Description = "Nome del ruolo da trovare",
                    Values = "",
                    DefaultValue = "\"\""
                },
                new InputParameter
                {
                    Name = "Skip",
                    Required = false,
                    InputType = 0,
                    Label = "Salto",
                    Description = "Numero di strutture superiori da escludere nella ricerca a partire dal gruppo indicato",
                    Values = "",
                    DefaultValue = "0"
                },
                new InputParameter
                {
                    Name = "Recursive",
                    Required = false,
                    InputType = 0,
                    Label = "Ricorsività",
                    Description = "1=Esegue un ricerca ricorsiva nella struttura di organigramma, risalendo fino ai nodi apicali",
                    Values = "",
                    DefaultValue = "1"
                },
                new InputParameter
                {
                    Name = "DefaultRoleId",
                    Required = false,
                    InputType = 0,
                    Label = "Id ruolo di Default",
                    Description = "Id ruolo di Default",
                    Values = "",
                    DefaultValue = "\"\""
                },
                new InputParameter
                {
                    Name = "DefaultGroupId",
                    Required = false,
                    InputType = 0,
                    Label = "Id Default del gruppo",
                    Description = "Id Default del gruppo",
                    Values = "",
                    DefaultValue = "\"\""
                }
            };

            var outputs = new List<OutputParameter>
            {
                new OutputParameter
                {
                    Name = "Role",
                    DefaultValue = "Role",
                    Required = false,
                    Label = "Ruolo",
                    Description = "Ruolo recuperato"
                },
                new OutputParameter
                {
                    Name = "UserGroup",
                    DefaultValue = "UserGroup",
                    Required = false,
                    Label = "Utente del gruppo",
                    Description = "Utente del Gruppo recuperato"
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
