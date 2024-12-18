//using Elmi.Core.DataAccess;
//using Newtonsoft.Json.Linq;
//using OpenDMS.Core.BusinessLogic;
//using OpenDMS.Core.DTOs;
//using OpenDMS.Core.Interfaces;
//using OpenDMS.Domain.Constants;
//using OpenDMS.Domain.Entities;
//using OpenDMS.Domain.Models;
//using OpenDMS.Domain.Repositories;
//using OpenDMS.Domain.Services;
//using OpenDMS.Infrastructure.Services.BusinessLogic;
//using OpenDMS.Workflow.API.BusinessLogic;
//using org.quartz;
//using System.Data;
//using System.DirectoryServices.Protocols;
//using Zeebe.Client.Api.Responses;
//using Zeebe.Client.Api.Worker;

//namespace OpenDMS.Workflow.API.Workers.Database
//{
//    public class SaveVariablesWorker : ICustomTask
//    {
//        private readonly ILogger<SaveVariablesWorker> logger;
//        private readonly IWorkflowEngine client;
//        private readonly IProcessDataService processDataService;
//        private readonly ICustomFieldService customFieldService;
//        private readonly IDataTypeFactory dataTypeFactory;
//        private readonly IAppSettingsRepository appSettings;

//        public string JobType { get; set; } = "StoreProcessData";
//        public string TaskLabel { get; set; } = "Variabili per Analisi";
//        public string Icon { get; set; } = "fa fa-hdd-o";
//        public string GroupName { get; set; } = "Analisi";
//        public string[] AlternativeTasks { get; set; } = new[] { "" };

//        public string Inputs { get; set; } = "Variables";
//        public string Outputs { get; set; } = "";

//        public int MaxJobs { get; set; } = 1;
//        public int PollingInterval { get; set; } = 1;
//        public int TimeOut { get; set; } = 86400;



//        public class ProcessVariable
//        {
//            public string GlobalId { get; set; } = "";
//            public string LocalName { get; set; } = "";
//            public string Value { get; set; } = "";
//        }


//        public SaveVariablesWorker(
//            ILogger<SaveVariablesWorker> logger,
//            IWorkflowEngine engine,
//            IProcessDataService processDataService,
//            ICustomFieldService customFieldService,
//            IDataTypeFactory dataTypeFactory,
//            IAppSettingsRepository appSettings)
//        {
//            this.logger = logger;
//            client = engine;
//            this.processDataService = processDataService;
//            this.customFieldService = customFieldService;
//            this.dataTypeFactory = dataTypeFactory;
//            this.appSettings = appSettings;
//        }


//        public async Task Initialize()
//        {
//            if (!int.TryParse(await appSettings.Get(Constants.CONST_USERTASK_WORKER_MAXJOBS), out int i))
//                i = 1;
//             MaxJobs = i;

//            if (!int.TryParse(await appSettings.Get(Constants.CONST_USERTASK_WORKER_POLLING), out int i2))
//                i2 = 1;
//             PollingInterval = i2;

//            if (!int.TryParse(await appSettings.Get(Constants.CONST_USERTASK_WORKER_TIMEOUT), out int i3))
//                i3 = 86400;
//             TimeOut = i3;

//            //await client.AddWorker(JobType, HandleJob, MaxJobs, PollingInterval, TimeOut);
//        }



//        public async Task HandleJob(IJobClient jobClient, IJob job)
//        {
//            // business logic
//            // 
//            var jobKey = job.Key;
//            var processId = job.Key.ToString();
//            var processkey = job.ProcessInstanceKey.ToString();//.BpmnProcessId;//.Key;            

//            logger.LogDebug(TaskLabel + ": " + job.ProcessDefinitionKey + "\\" + job.ElementId + "\\" + job.Key + " - Instance: " + job.ProcessDefinitionKey + "\\" + job.ElementInstanceKey);
//            var Variables = JObject.Parse(job.Variables)["Variables"]?.ToObject<ProcessVariable[]>();
//            if (Variables != null && Variables.Length > 0)
//            {
//                foreach (var v in Variables)
//                {
//                    try
//                    {
//                        var vValue = v.Value;
//                        if (!String.IsNullOrWhiteSpace(v.GlobalId))
//                        {
//                            var fieldType = await customFieldService.GetById(v.GlobalId);
//                            var dataManager = await dataTypeFactory.Instance(fieldType.DataType);
//                            var fieldTypeValue  = await dataManager.Lookup(fieldType, vValue);
//                            if (fieldTypeValue != null) {
//                                vValue = fieldTypeValue.LookupValue;
//                            }
//                        }
//                        var data = new ProcessInstanceVariable()
//                        {
//                            ProcessId = job.BpmnProcessId,
//                            ProcessInstanceId = processkey,
//                            VariableId = v.GlobalId,
//                            VariableName = v.LocalName,
//                            Value = v.Value,
//                            LookupValue = vValue
//                        };
//                        await processDataService.SaveVariable(data);
//                    }
//                    catch (Exception ex)
//                    {
//                        logger.LogError(ex, $"SaveVariablesWorker (PID:{job.BpmnProcessId}, PIK:{job.ProcessInstanceKey})");
//                    }
//                }
//            }

//            try
//            {
//                await jobClient.NewCompleteJobCommand(job).Send();
//            }
//            catch (Exception ex)
//            {
//                await jobClient.NewThrowErrorCommand(jobKey)
//                    .ErrorCode(ex.Message)
//                    .Send();
//            }

//        }
//    }
//}
