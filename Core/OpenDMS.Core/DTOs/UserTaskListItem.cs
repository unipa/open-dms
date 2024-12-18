using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace OpenDMS.Core.DTOs
{
    public class UserTaskListItem
    {
        public int Id { get; set; }
        public int TaskItemId { get; set; }

        public string CompanyId { get; set; }
        public string Sender { get; set; }
        public string SenderName { get; set; }

        public List<LookupTable> ToList { get; set; } = new List<LookupTable>();
        public List<LookupTable> CCList { get; set; } = new List<LookupTable>();
        public List<int> Attachments { get; set; } = new List<int>();

        public ExecutionStatus Status { get; set; }

        public LookupTable Process { get; set; } = new LookupTable();
        public LookupTable Category { get; set; } = new LookupTable();
        public LookupTable Priority { get; set; } = new LookupTable();
        /// <summary>
        /// Codice e descrizione dei fascicoli di riferimento del Task 
        /// </summary>
        public List<LookupTable> Project { get; set; } =new List<LookupTable>();

        public string Title { get; set; }
        public string Description { get; set; }

        public TaskType TaskType { get; set; }

        public decimal Percentage { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? ExecutionDate { get; set; }
        public DateTime? ValidationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? NotificationDate { get; set; }
        public DateTime? ClaimDate { get; set; }

        public string GroupId { get; set; }
        public string RoleId { get; set; }
        public string UserId { get; set; }

        public bool IsCC { get; set; }
        public string LastProgressDescription { get; set; }

        public string ManagerId { get; set; }
        public NotificationType NotificationType { get; set; }

    }
}
