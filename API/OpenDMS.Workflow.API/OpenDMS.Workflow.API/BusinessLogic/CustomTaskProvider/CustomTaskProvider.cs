using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using OpenDMS.Domain.Entities.Workflow;
using OpenDMS.Domain.Repositories;
using OpenDMS.Workflow.API.DTOs.Palette;
using System.Linq;
using System.Text;

namespace OpenDMS.Workflow.API.BusinessLogic.CustomTask
{
    public class CustomTaskProvider : ICustomTaskProvider
    {
        private readonly ITaskEndpointRepository taskEndpointRepository;

        public CustomTaskProvider(ITaskEndpointRepository taskEndpointRepository)
        {
            this.taskEndpointRepository = taskEndpointRepository;
        }


        public List<TaskGroup> GetPalette()
        {
            var all = taskEndpointRepository.GetAll();
            Dictionary<string, TaskGroup> Palette = new();
            foreach (var g in all)
            {
                List<TaskItem> items = System.Text.Json.JsonSerializer.Deserialize<List<TaskItem>>(g.Tasks);
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        var group = new TaskGroup();
                        if (Palette.ContainsKey(item.Group))
                        {
                            group = Palette[item.Group];
                        }
                        else
                        {
                            Palette.Add(item.Group, group);
                            group.Name = item.Group;
                            group.Id = Guid.NewGuid().ToString();
                            group.Description = item.Group;
                        }
                        group.Tasks.Add(item);
                    }
                }
            }
        
            return Palette.Values.ToList();
        }

    public CustomTaskEndpoint GetByURL(string serviceURL)
    {
        return taskEndpointRepository.GetByURL(serviceURL);
    }
    public CustomTaskEndpoint GetById(string serviceId)
    {
        return taskEndpointRepository.GetById(serviceId);
    }
    public void Delete(string serviceId)
    {
        taskEndpointRepository.Delete(serviceId);
    }

    public void Restore(string serviceId)
    {
        taskEndpointRepository.Restore(serviceId);
    }

    public void Update(string serviceId, string Tasks)
    {
        taskEndpointRepository.Update(serviceId, Tasks);
    }


    public bool ImportSwagger(string swaggerURL, byte[] swaggerContent)
    {

        var taskService = taskEndpointRepository.GetByURL(swaggerURL);
        if (taskService != null) return false;

        CustomTaskEndpoint Api = new();
        Dictionary<string, TaskGroup> TaskGroups = new();

        using (var stream = new MemoryStream(swaggerContent))
        {
            var reader = new OpenApiStreamReader().Read(stream, out var diagnotstic);
            Api.EndPointDescriptor = Encoding.UTF8.GetString(swaggerContent);
            Api.Name = reader.Info.Title;
            Api.Description = reader.Info.Description;
            Api.Id = new Guid().ToString();
            Api.Endpoint = swaggerURL;

            if (reader.SecurityRequirements.Count > 0)
                foreach (var kv in reader.SecurityRequirements)
                    foreach (var s in kv)
                    {
                        //                            Console.WriteLine("Security Requirements: " + s.Key);
                        //                            foreach (var v in s.Value)
                        //                                Console.WriteLine("     Value: " + v);
                    }
            //                if (reader.Components != null)
            //                    if (reader.Components.SecuritySchemes != null)
            //                        foreach (var s in reader.Components.SecuritySchemes)
            //                            Console.WriteLine(s.Value.Name + " = " + s.Key + "\n - Scheme:" + s.Value.Scheme + "\n - Type:" + s.Value.Type + "\n - Bearer:" + s.Value.BearerFormat + "\n - Description:" + s.Value.Description + "\n - Url:" + s.Value.OpenIdConnectUrl);
            foreach (var p in reader.Paths)
            {
                foreach (var o in p.Value.Operations)
                {
                    TaskItem EP = new TaskItem();
                    var group = Api.Name;
                    if (o.Value.Tags.Count > 0) group = o.Value.Tags[0].Name;

                    if (TaskGroups.ContainsKey(group))
                        TaskGroups[group].Tasks.Add(EP);
                    else
                    {
                        TaskGroup TG = new TaskGroup();
                        TG.Id = Api.Id;
                        TG.Name = group;
                        TG.Description = "";
                        TG.Tasks = new();
                        TG.Tasks.Add(EP);
                    }

                    var id = o.Value.OperationId;
                    if (string.IsNullOrEmpty(id))
                    {
                        id = o.Key.ToString();
                    }
                    EP.Id = id + p.Key.Replace("/", "-").Replace("{", "").Replace("}", "").Replace(":", "").Replace("[", "").Replace("]", "");
                    EP.Name = o.Value.Summary;
                    if (string.IsNullOrEmpty(EP.Name) || EP.Name.Length > 25)
                    {
                        EP.Name = EP.Id.Split("/").Last(l => !l.Contains("{") & !l.Contains("["));
                    }
                    EP.Group = group;
                    EP.Description = string.IsNullOrEmpty(o.Value.Description) ? EP.Name : o.Value.Description;
                    EP.JobWorker = "ApiRESTWorker";
                    EP.TagType = "bpmn:serviceTask";
                    EP.Label = EP.Name;
                    EP.ColorStroke = "";
                    EP.ColorFill = "";
                    EP.Icon = "fa fa-sitemap";

                    //if (!String.IsNullOrEmpty(o.Value.Summary)) Console.WriteLine("     Summary: " + o.Value.Summary.Replace("\n", ""));
                    //if (!String.IsNullOrEmpty(o.Value.Description)) Console.WriteLine("     Description: " + o.Value.Description.Replace("\n", ""));
                    //if (o.Value.Security.Count > 0) Console.WriteLine("     Security: " + o.Value.Security[0].ToString());

                    foreach (var ip in o.Value.Parameters)
                    {
                        var P = new InputParameter();
                        EP.Inputs.Add(P);
                        P.Name = ip.Name;
                        P.DefaultValue = "";
                        P.Label = P.Name;
                        P.Description = string.IsNullOrEmpty(ip.Description) ? P.Name : ip.Description;
                        P.InputType = ip.Schema.MaxLength > 250 ? 1 : 0; // 1= textarea, 0=input
                        P.Required = ip.Required;
                        P.Values = "";

                        //Console.WriteLine("     > " + ip.Name + (!ip.Required ? "" : "(required)") + "=" + ip.Schema.Type + " : " + ip.Description + " -> " + ip.In.ToString());
                        if (ip.Schema.Enum.Count > 0)
                        {
                            P.InputType = 2; // select
                            P.Values = "[" + string.Join(',', ip.Schema.Enum.Select<IOpenApiAny, string>(e => ((OpenApiInteger)e).Value.ToString() + "=" + e.AnyType.ToString())) + "]";
                            //Console.WriteLine("         > " + ip.Schema.Reference?.Id + " - " + ip.Schema.Title + " - " + ip.Schema.Description + " = " + ip.Schema.Default?.ToString());
                            //foreach (OpenApiInteger e in ip.Schema.Enum)
                            //    Console.WriteLine("            > " + e.Value.ToString());
                        }


                    }
                    var OP = new OutputParameter();
                    EP.Outputs.Add(OP);
                    OP.Name = "StatusCode";
                    OP.Label = OP.Name;
                    string examples = "";
                    string values = "";
                    foreach (var op in o.Value.Responses)
                    {
                        var v = op.Value.Description;
                        if (op.Value.Content.Count > 0)
                        {
                            OpenApiMediaType e = (OpenApiMediaType)op.Value.Content.First().Value;
                            if (e.Example != null)
                            {
                                examples += "<u>StatusCode=" + op.Key + "</u>\n" + ((OpenApiString)e.Example).Value.ToString().Replace("\n", "<br/>");
                            }
                        }
                        values += "    " + op.Key.ToString() + "=" + (string.IsNullOrEmpty(v) ? op.Key : v) + "\n";
                    }
                    OP.DefaultValue = "200";
                    OP.Description = "Ritorna:<br/>" + values.Replace("\n", "<br/>");

                    if (!String.IsNullOrEmpty(examples))
                    {
                        EP.Description += "<b>Esempi:</b><br/>";
                    }
                    //Console.WriteLine("");

                }

            }
            var tasks = TaskGroups.Select(t => t.Value).ToList<TaskGroup>();
            Api.Tasks = System.Text.Json.JsonSerializer.Serialize(tasks);
            taskEndpointRepository.Add(Api);
        }

        return true;

    }


    public async Task ImportPalette(IServiceProvider serviceProvider)
    {
        //Per prima cosa recuperiamo tutte le classi registrate che implementano ICustomTask
        //var customTasktypes = Assembly.GetExecutingAssembly().GetTypes()
        // Utilizziamo reflection per ottenere tutte le classi nell'assembly corrente che implementano l'interfaccia ICustomTask.
        //    .Where(t => typeof(ICustomTask).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
        //Raccogliamo gli oggetti TasKItem per tutte le classi registrate sull'Assembly
        var taskItems = new List<TaskItem>();
        using (var scope = serviceProvider.CreateScope())
        {

            var customTasktypes = scope.ServiceProvider.GetServices<ICustomTask>();
            foreach (var taskType in customTasktypes)
            {
                try
                {
                    var taskItem = await taskType.PaletteItem();
                    if (taskItem != null)
                        taskItems.Add(taskItem);
                }
                catch (Exception)
                {
                }
            }
        }
        // Aggiungi i TaskItem raccolti a taskEndpointRepository
        CustomTaskEndpoint result = new() { Endpoint = "Workflow.API", Description = "Task Workflow API", Id = "Workflow.API", CustomTaskEndpointType = CustomTaskEndpointType.Internal, Name = "API Workflow" };
        result.Tasks = System.Text.Json.JsonSerializer.Serialize(taskItems);

        var Element = taskEndpointRepository.GetById(result.Id);
        if (Element == null)
        {
            // Non vi è nessuna corrispondenza e di conseguenza lo salvo come nuovo
            taskEndpointRepository.Add(result);
        }
        else
        {
            var taskItemJson = System.Text.Json.JsonSerializer.Serialize(taskItems);
            var elementJson = Element.Tasks;
            if (taskItemJson != elementJson)
            {
                taskEndpointRepository.Delete(Element.ToString());
                taskEndpointRepository.Add(result);
            }
        }
    }

}
}
