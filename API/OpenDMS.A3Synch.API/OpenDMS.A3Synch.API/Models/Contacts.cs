using Org.BouncyCastle.Utilities.Encoders;
using System.ComponentModel.DataAnnotations;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using System.Reflection.Emit;

namespace A3Synch.Models
{
    public class Contacts
    {
       
        [StringLength(64)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [StringLength(64)]
        public string? ParentId { get; set; } = null;

        public int ContactType { get; set; } = 1;

        [StringLength(255)]
        public string FullName  { get; set; }
       
        [StringLength(128)]
        public string? FriendlyName { get; set; }

        [StringLength(128)]
        public string SearchName { get; set; }

        [StringLength(6)]
        public string? CountryCode { get; set; }

        [StringLength(16)]
        public string? LicTradNum { get; set; }

        [StringLength(16)]
        public string? FiscalCode { get; set; }

        [StringLength(8)]
        public string? IPACode { get; set; }

        [StringLength(255)]
        public string? Avatar { get; set; }

        [StringLength(1)]
        public string? Sex { get; set; }

        [StringLength(64)]
        public string? SurName { get; set; }

        [StringLength(64)]
        public string? FirstName { get; set; }

        [StringLength(64)]
        public string? CreationUser { get; set; }

        [StringLength(64)]
        public string? LastUpdateUser { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdate { get; set; }

        [StringLength(255)]
        public string? UpdateErrors { get; set; }

        public bool Deleted { get; set; } = false;

        [StringLength(255)]
        public string? Annotation { get; set; }

    }

}