using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Core.DTOs
{
    public class UserTaskInfo 
    {
        public int Id { get; set; }

        public TaskItemInfo TaskItemInfo { get; set; } = new();

        public string GroupId { get; set; }
        public string RoleId { get; set; }
        public string UserId { get; set; }

        public bool IsCC { get; set; }
//        public string LastProgressDescription { get; set; }

        public string ManagerId { get; set; }
        public decimal Percentage { get; set; }
        public NotificationType NotificationType { get; set; }
        public ExecutionStatus Status { get; set; }
        public DateTime? NotificationDate { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? ClaimDate { get; set; }
        public DateTime? FirstExecutionDate { get; set; }
        public DateTime? LastExecutionDate { get; set; }
        public DateTime? ValidationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public bool Received { get; set; }


        public string GroupName { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public List<UserTaskInfo> SubTasks { get; set; } = new List<UserTaskInfo>();


        //// V2
        //        public string Sender { get; set; }
        //        public string SenderName { get; set; }
        //        public string Description { get; set; }

        //        public int ParentUserTaskId { get; set; }

        //        public FormSchema Form { get; set; } = null;
        //        public TaskType TaskType { get; set; }
        //        public UserTaskInfo ParentUserTask { get; set; }

    }
}
