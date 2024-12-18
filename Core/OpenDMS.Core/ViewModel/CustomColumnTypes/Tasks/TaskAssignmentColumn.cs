using OpenDMS.Core.DTOs;
using OpenDMS.Core.ViewModel.ColumnTypes;
using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Core.ViewModel.CustomColumnTypes.Tasks
{
    public class TaskAssignmentColumn : GenericIconColumn
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


        public TaskAssignmentColumn(
            string id,
            string title,
            string description,
            string category,
            string tooltip
            )
            : base(TaskColumn.Assignment, "", "Tipo Destinatario", "Attività", tooltips, values, 0)
        {
        }

        public override async Task<SearchResultColumn> Render(string[] fields)
        {
            string a = "";
            string b = "";
            var u = fields[2];
            var r = fields[1];
            var g = fields[0];

            if (!string.IsNullOrEmpty(u))
            {
                a = "0";
                b = "Inviata ad un utente";
            }
            else
                if (!string.IsNullOrEmpty(r))
            {
                a = "1";
                b = "Inviata ad un Ruolo";
            }
            else
                if (!string.IsNullOrEmpty(g))
            {
                a = "2";
                b = "Inviata ad un Gruppo";
            }

            return new SearchResultColumn() { Value = a, Description = b, Tooltip = "" };
        }
    }
}
