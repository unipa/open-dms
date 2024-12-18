using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Events.Types;
using System.ComponentModel.DataAnnotations;


namespace OpenDMS.Domain.Entities.Settings
{

    [PrimaryKey(nameof(CompanyId), nameof(CounterId), nameof(Year))]

    public class Counter
    {
        public int CompanyId { get; set; }

        [StringLength(64)]
        public string CounterId { get; set; }

        public int Year { get; set; }
        public int Value { get; set; }

    }
}
