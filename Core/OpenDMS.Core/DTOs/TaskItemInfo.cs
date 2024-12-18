using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Entities.Tasks;
using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Core.DTOs
{
    public class TaskItemInfo
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string ParentTitle { get; set; }
        public TaskType ParentType { get; set; }

        public int CompanyId { get; set; }
        public string Sender { get; set; }
        public string SenderName { get; set; }

        public List<LookupTable> ToList { get; set; } = new List<LookupTable>();
        public List<LookupTable> CCList { get; set; } = new List<LookupTable>();
        public List<DocumentInfo> Attachments { get; set; } = new List<DocumentInfo>();

        public ExecutionStatus Status{ get; set; }

        public LookupTable Process { get; set; } = new LookupTable();
        public FormSchema Form { get; set; } = null;
 
        public LookupTable Event { get; set; } = new LookupTable();

        /// <summary>
        /// Codice e descrizione dei fascicoli di riferimento del Task 
        /// </summary>
        public List<LookupTable> Project { get; set; }

        public string Title { get; set; }
        public LookupTable Category { get; set; } = new LookupTable();
        public string Description { get; set; }

        public TaskType TaskType { get; set; }
        public LookupTable Priority { get; set; } = new LookupTable();

        public decimal Percentage { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? ExecutionDate { get; set; }
        public DateTime? ValidationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        // Evoluzioni future
        public decimal EstimatedPercentage { get; set; }

        public List<TaskProgressInfo> Progress { get; set; } = new List<TaskProgressInfo>();

        public string Template { get; set; }
        public string ExecutionId { get; set; }
        public string ProcessId { get; set; }
        public int ProcessDataId { get; set; }


        public LookupTable Company { get; set; } = new LookupTable();
        // DA RIMUOVERE
        //public string Description { get; set; }
        //public int ParentId { get; set; }
        //public string ParentTitle { get; set; }
        //public TaskType ParentType { get; set; }
        public decimal Duration { get; set; } = 0;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
