using Elmi.Core.DataAccess;
using Newtonsoft.Json.Linq;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Repositories;
using OpenDMS.Workflow.API.DTOs.Palette;
using org.apache.poi.ss.formula.functions;
using System.Data;
using System.Data.Common;
using Zeebe.Client.Api.Responses;

namespace OpenDMS.Workflow.API.Workers.Database
{
    public class SqlReaderWorker : BaseWorker
    {
        private readonly IDataSourceProvider dsProvider;

        public override string JobType { get; set; } = "sqlReaderTask";
        public override string TaskLabel { get; set; } = "SQL SELECT";
        public override string Icon { get; set; } = "fa fa-table";
        public override string GroupName { get; set; } = "Database";
        public override string[] AlternativeTasks { get; set; } = new[] { "" };

        public override string Inputs { get; set; } = "DatabaseConnectionId,SelectSQL";
        public override string Outputs { get; set; } = "Rows";
        public override int MaxJobs { get; set; } = 1;
        public override int PollingInterval { get; set; } = 1;
        public override int TimeOut { get; set; } = 120;



        public SqlReaderWorker(
            ILogger<SqlReaderWorker> logger,
            IWorkflowEngine engine,
            IDataSourceProvider dsProvider,
         IAppSettingsRepository appSettings) : base(logger, engine, appSettings)
        {
            this.dsProvider = dsProvider;
        }

        public override async Task<TaskItem> PaletteItem()
        {
            var inputs = new List<InputParameter>
            {
                new InputParameter
                {
                    Name = "DatabaseConnectionId",
                    Required = true,
                    InputType = 0,
                    Label = "Id Connessione al Database",
                    Description = "Id della Connessione al Database",
                    Values = "",
                    DefaultValue = "''"
                },
                new InputParameter
                {
                    Name = "SelectSQL",
                    Required = true,
                    InputType = 0,
                    Label = "Select SQL",
                    Description = "Query SQL da eseguire",
                    Values = "",
                    DefaultValue = "''"
                }
            };
            var outputs = new List<OutputParameter>
            {
                new OutputParameter
                {
                    Name = "Rows",
                    DefaultValue = "",
                    Required = false,
                    Label = "Righe",
                    Description = "Righe risultanti dalla Query SQL"
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
            var processId = job.Key.ToString();
            var processkey = job.ProcessInstanceKey.ToString();//.BpmnProcessId;//.Key;            
            var dbConnectionId = (string)(JObject.Parse(job.Variables)["DatabaseConnectionId"] ?? "");
            var SQL = (string)(JObject.Parse(job.Variables)["SelectSQL"] ?? "");
            if (!string.IsNullOrWhiteSpace(SQL) && !String.IsNullOrEmpty(dbConnectionId))
            {
                logger.LogDebug(TaskLabel + ": " + job.ProcessDefinitionKey + "\\" + job.ElementId + "\\" + job.Key + " - Instance: " + job.ProcessDefinitionKey + "\\" + job.ElementInstanceKey);
                ExternalDatasource EDS = await dsProvider.Get(dbConnectionId);
                DataSource DS = new DataSource(EDS.ConnectionString, EDS.Driver);

                List<IDataParameter> parameters = new();
                int from = 0;
                while (from < SQL.Length)
                {
                    int i = SQL.IndexOf("@", from);
                    if (i >= 0)
                    {
                        from = i + 1;
                        int end = SQL.IndexOfAny(new[] { ' ', ',', ';', ')', '\n', '\r', '+', '|' }, from);
                        if (end < 0) end = SQL.Length + 1;
                        var token = SQL.Substring(from, end - from);
                        var tokenvalue = (string)(JObject.Parse(job.Variables)[token] ?? "");
                        parameters.Add(DS.Parameter(token, tokenvalue));
                    }
                    else
                        from = SQL.Length;
                }
                var rowCount = 0;
                var rows = "";
                using (DbDataReader reader = (DbDataReader)DS.Select(SQL, parameters.ToArray()))
                {
                    while (reader != null && reader.Read())
                    {
                        rowCount += 1;
                        string row = (rowCount > 1 ? ", " : "") + "{ ";
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (i > 0) row += ",";
                            row += "\"" + reader.GetName(i) + "\" : \"" + reader[i].ToString().Replace("\"","\"\"") + "\"";
                        }
                        row += " }";
                        rows += row;
                    }
                }
                var variables = "{ \"Rows\" : [" + rows + "] }";
                return variables;
            }
            return "{ \"Rows\" : [] }";
        }
    }
}
