using System.ComponentModel.DataAnnotations;

namespace A3Synch.Models
{
    public class UserGroups
    {
        [StringLength(64)]
        public string Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string ShortName { get; set; }

        public DateTime ClosingDate { get; set; }

        public DateTime CreationDate { get; set; }

        [StringLength(64)]
        public string? ClosingUser { get; set; }

        [StringLength(64)]
        public string? CreationUser { get; set; }

        [StringLength(255)]
        public string ExternalId { get; set; }

		[StringLength(255)]
		public string ExternalApp { get; set; }

		public int NotificationStrategy { get; set; }

        public int NotificationStrategyCC { get; set; }

        public bool Visible { get; set; }

        public bool Closed { get; set; }
    }

}