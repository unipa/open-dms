using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Domain.Models
{
    public class TaskListFilter
    {
        public string FreeText { get; set; } = "";

        public string Sender { get; set; } = "";
        public string Recipient { get; set; } = "";
        public List<ExecutionStatus> Status { get; set; } = new List<ExecutionStatus>();
        public TaskType[] TaskType { get; set; } = null;
        public string CategoryId { get; set; } = "";
        public int ParentId { get; set; } = 0;
        public int TaskItemId { get; set; } = 0;
        public string FormKey { get; set; } = "";
        public int DocumentId { get; set; } = 0;
        public string  EventId { get; set; } = "";
        public DateTime? FromCreationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }


        public int ProcessDefinitionId { get; set; } = 0;
        public int ProjectId { get; set; } = 0;
        public string ProcessInstanceId { get; set; } = "";
        public string ProcessDefinitionKey { get; set; } = "";


        /// <summary>
        /// Indica che per il filtro deve prendere i task per documento,processo e progetto
        /// </summary>
//        public bool ProcessTask { get; set; } = true;

        public int ProcessDataId { get; set; } = 0;

        public bool Received { get; set; } = false;


        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 50;

    }
}
