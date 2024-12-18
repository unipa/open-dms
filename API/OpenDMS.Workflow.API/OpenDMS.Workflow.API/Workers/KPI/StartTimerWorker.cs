using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Repositories;
using OpenDMS.Workflow.API.BusinessLogic;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;

namespace OpenDMS.Workflow.API.Workers.Database
{
    public class StartTimerWorker : BaseWorker
    {

        public override string JobType { get; set; } = "StartTimer";
        public override string TaskLabel { get; set; } = "Avvia Timer";
        public override string Icon { get; set; } = "fa fa-clock-o";
        public override string GroupName { get; set; } = "Analisi";
        public override string[] AlternativeTasks { get; set; } = new[] { "" };

        public override string Inputs { get; set; } = "";
        public override string Outputs { get; set; } = "TimerId";

        public override int MaxJobs { get; set; } = 1;
        public override int PollingInterval { get; set; } = 1;
        public override int TimeOut { get; set; } = 30;



        public StartTimerWorker(
            ILogger<StartTimerWorker> logger,
            IWorkflowEngine engine,
          IAppSettingsRepository appSettings) : base(logger, engine, appSettings)
        {
        }



        public override  async Task<TaskItem> PaletteItem()
        {
            var inputs = new List<InputParameter>();

            var outputs = new List<OutputParameter>
            {
                new OutputParameter
                {
                    Name = "CurrentTime",
                    DefaultValue = "CurrentTime",
                    Required = false,
                    Label = "Orario Corrente",
                    Description = "Orario Corrente (UTC)"
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
            var variable = "{ \"CurrentTime\" : \"" + DateTime.UtcNow.ToUniversalTime().ToString("u").Replace(" ", "T") + "\" }";
            return variable;
        }
    }
}
