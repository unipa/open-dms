using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Domain.Enumerators
{
    public static class TaskColumn
    {
        public const string Permissions = "Task.Permissions";

        public const string Destinatario = "Task.Destinatario";
        public const string Attachment = "Task.Attachment";
        public const string Direction = "Task.Direction";
        public const string EventId = "Task.EventId";
        public const string FormKey = "Task.FormKey";

        public const string Id = "Task.Id";
        public const string Assignment= "Task.Assignment";
        public const string Category = "Task.Category";
        public const string Company = "Task.Company";
        public const string CreationDate = "Task.CreationDate";
        public const string Description = "Task.Description";
        public const string ExpirationDate = "Task.ExpirationDate";
        public const string FromUser = "Task.FromUser";
        public const string Percentage = "Task.Percentage";
        public const string UserPercentage = "Task.UserPercentage";
        public const string Group = "Task.Group";
        public const string Priority = "Task.Priority";
        public const string Role = "Task.Role";
        public const string Status = "Task.Status";
        public const string TaskType = "Task.TaskType";
        public const string Title = "Task.Title";
        public const string User = "Task.User";

        public const string FreeText = "Task.Freetext";
        public const string Parent = "Task.Parent";


        public static List<string> JobStatusTooltips = new() {
            "In Attesa",
            "In Corso",
            "Completata",
            "Fallita",
            "Annullata",
            "Ignorata",
            "Non Necessaria"
        };
        public static List<string> TaskStatusTooltips = new() {
            "Non Assegnata",
            "Assegnata",
            "In Corso",
            "Sospesa",
            "Eseguita"
        };
    }
}
