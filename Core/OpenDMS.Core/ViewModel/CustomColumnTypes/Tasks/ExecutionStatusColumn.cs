using OpenDMS.Core.DTOs;
using OpenDMS.Domain.QueryBuilder;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Services;
using OpenDMS.Core.ViewModel.ColumnTypes;

namespace OpenDMS.Core.ViewModel.CustomColumnTypes.Tasks
{
    public class ExecutionStatusColumn : GenericIconColumn
    {
        public static List<string> values = new() {
             "<i class='icoTaskStatusUnassignedCC'></i>",
             "<i class='icoTaskStatusUnassigned'></i>",
             "<i class='icoTaskStatusAssignedCC'></i>",
             "<i class='icoTaskStatusAssigned'></i>",
             "<i class='icoTaskStatusRunning'></i>",
             "<i class='icoTaskStatusExecuted'></i>",
             "<i class='icoTaskStatusValidated'></i>",
             "<i class='icoTaskStatusDeleted'></i>"}
        ;
        public static List<string> tooltips = new() {
            "Non Assegnata",
            "Non Assegnata",
            "Assegnata",
            "Assegnata",
            "In Corso",
            "Eseguita",
            "Convalidata",
            "Annullata" };


        public ExecutionStatusColumn(
            string id,
            string title,
            string description,
            string category,
            string tooltip
            )
            : base(TaskColumn.Status, "", "Stato", "Attività", tooltips, values, 0)
        {
        }

        public override async Task<SearchResultColumn> Render(string[] fields)
        {
            var b = (ExecutionStatus)int.Parse(fields[0]);
            var r = fields[1];
            var g = fields[2];
            var index = 1;
            switch (b)
            {
                case ExecutionStatus.Assigned:
                    if (!string.IsNullOrEmpty(g + r))
                        index = 2;
                    else
                        index = 3;
                    break;
                case ExecutionStatus.Running:
                    index = 4;
                    break;
                case ExecutionStatus.Suspended:
                case ExecutionStatus.Executed:
                    index = 5;
                    break;
                case ExecutionStatus.Validated:
                    index = 6;
                    break;
                case ExecutionStatus.Aborted:
                case ExecutionStatus.Deleted:
                    index = 7;
                    break;
                default:
                    // Se non è assegnata a me ...
                    if (!string.IsNullOrEmpty(g + r))
                        index = 0;
                    else
                        index = 1;
                    break;
            }
            return new SearchResultColumn() { Value = b.ToString(), Description = index.ToString(), Tooltip = index.ToString() };
        }
    }
}
