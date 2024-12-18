using Newtonsoft.Json;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Repositories;
using OpenDMS.Workflow.API.BusinessLogic;
using OpenDMS.Workflow.API.DTOs.Palette;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;


namespace OpenDMS.Workflow.API.Workers
{
    public class BaseWorker : ICustomTask
    {
        protected readonly ILogger<BaseWorker> logger;
        protected readonly IWorkflowEngine engine;
        protected readonly IAppSettingsRepository appSettings;

        public virtual string JobType { get; set; } = "";
        public virtual string TaskLabel { get; set; } = "";
        public virtual string Icon { get; set; } = "";
        public virtual string GroupName { get; set; } = "";
        public virtual string[] AlternativeTasks { get; set; } = null;

        public virtual int MaxJobs { get; set; } = 1;
        public virtual int PollingInterval { get; set; } = 1;
        public virtual int TimeOut { get; set; } = 86400;

        public virtual string Inputs { get; set; } = "";
        public virtual string Outputs { get; set; } = "";


        public BaseWorker(
            ILogger<BaseWorker> logger,
            IWorkflowEngine engine,
            IAppSettingsRepository appSettings
            )
        {
            this.logger = logger;
            this.engine = engine;
            this.appSettings = appSettings;
        }


        public async Task Initialize()
        {
            //if (!int.TryParse(await appSettings.Get(Constants.CONST_USERTASK_WORKER_MAXJOBS), out int i))
            //    i = 1;
            //MaxJobs = i;

            //if (!int.TryParse(await appSettings.Get(Constants.CONST_USERTASK_WORKER_POLLING), out int i2))
            //    i2 = 1;
            //PollingInterval = i2;

            //if (!int.TryParse(await appSettings.Get(Constants.CONST_USERTASK_WORKER_TIMEOUT), out int i3))
            //    i3 = 86400;
            //TimeOut = i3;

            //  await client.AddWorker(JobType, HandleJob, MaxJobs, PollingInterval, TimeOut);
            // await client.AddWorker("RequestForApproval", HandleJob, MaxJobs, PollingInterval, TimeOut);
        }

        public DateTime ParseDate(string Date, DateTime DefaultValue)
        {
            if (!String.IsNullOrEmpty(Date))
            {
                var addD = 0;
                var addM = 0;
                var addY = 0;
                string[] parts = Date.Split("/");
                if (parts.Length > 1)
                {
                    if (parts[0].StartsWith("+"))
                    {
                        addD = int.Parse(parts[0]
                            .Replace("+", ""));
                        parts[0] = "**";
                    }
                    if (parts[1].StartsWith("+"))
                    {
                        addM = int.Parse(parts[1]
                            .Replace("+", ""));
                        parts[1] = "**";
                    }
                    if (parts[2].StartsWith("+"))
                    {
                        addY = int.Parse(parts[2]
                            .Replace("+", ""));
                        parts[2] = "**";
                    }
                    parts[0] = parts[0]
                        .Replace("*", DateTime.UtcNow.Day.ToString("00"))
                        .Replace("**", DateTime.UtcNow.Day.ToString("00"));
                    parts[1] = parts[1]
                        .Replace("*", DateTime.UtcNow.Month.ToString("00"))
                        .Replace("**", DateTime.UtcNow.Month.ToString("00"));
                    parts[2] = parts[2]
                        .Replace("*", DateTime.UtcNow.Year.ToString("0000"))
                        .Replace("**", DateTime.UtcNow.Year.ToString("0000"))
                        .Replace("****", DateTime.UtcNow.Year.ToString("0000"));
                } else
                {
                    parts = Date.Split("-").Reverse().ToArray();
                }
                DateTime dt = DateTime.MaxValue;

                if (parts.Length > 2 && DateTime.TryParse(parts[2] + "-" + parts[1] + "-" + parts[0], out dt))
                {
                    dt = dt.AddDays(addD).AddMonths(addM).AddYears(addY);
                    return dt;
                }
            }
            return DefaultValue;

        }

        public virtual async Task HandleJob(IJobClient jobClient, IJob job)
        {
            try
            {
                var variables = await Execute(job);
                try
                {
                    if (!string.IsNullOrEmpty(variables))
                    {
                        await engine.SetVariables(job.ElementInstanceKey.ToString(), variables, true);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(JobType + "-SET_VARIABLES: " + job.ProcessInstanceKey + " - ERROR:" + ex.Message+" - STACK:"+ex.StackTrace);
                }
                if (await Complete(job))
                {
                    var command = jobClient.NewCompleteJobCommand(job.Key);
                    if (!string.IsNullOrEmpty(variables))
                        command.Variables(variables);
                    await command.Send();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    await engine.SetVariables(job.ProcessInstanceKey.ToString(), JsonConvert.SerializeObject(new { errorMessage = ex.Message }));
                    logger.LogError(JobType + "-EXECUTE: " + job.ProcessInstanceKey + " - ERROR:" + ex.Message + " - STACK:" + ex.StackTrace);
                }
                catch (Exception ex2)
                {
                    try
                    {
                        logger.LogError(JobType + "-SET_ERROR_VARIABLE: " + job.ProcessInstanceKey + " - ERROR:" + ex2.Message + " - STACK:" + ex2.StackTrace);
                    }
                    catch { };
                }

                if (job.Retries > 1)
                    await jobClient.NewFailCommand(job.Key).Retries(job.Retries - 1)
                    .ErrorMessage(ex.Message)
                    .Send();
                else
                    await jobClient.NewThrowErrorCommand(job.Key)
                        .ErrorCode("EXCEPTION")
                        .ErrorMessage(ex.Message+" - STACK:"+ex.StackTrace)
                        .Send();
            }
        }

        public virtual async Task<string> Execute(IJob job)
        {
            return null;
        }
        public virtual async Task<bool> Complete(IJob job)
        {
            return true;
        }

        public virtual async Task<TaskItem> PaletteItem()
        {
            return null;
        }


    }
}
