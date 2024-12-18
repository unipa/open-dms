using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenDMS.Domain.Entities.Users;

public class ContactAddress
{
    public int Id { get; set; }

    [StringLength(64)]
    public string ContactId { get; set; }


    [StringLength(128)]
    public string Name { get; set; }

    [StringLength(128)]
    public string Address { get; set; }

    [StringLength(128)]
    public string City { get; set; }

    [StringLength(64)]
    public string Province { get; set; }

    [StringLength(9)]
    public string CAP { get; set; }

    [StringLength(128)]
    public string SearchName { get; set; }


    [StringLength(64)]
    public string CreationUser { get; set; }

    [StringLength(64)]
    public string LastUpdateUser { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

    public bool Deleted { get; set; }

    [NotMapped]
    public string FormattedAddress
    {
        get
        {
            string s = Address;
            if (!string.IsNullOrEmpty(CAP))
            {
                if (!string.IsNullOrEmpty(s)) s += " - ";
                s += CAP;
            }
            if (!string.IsNullOrEmpty(City))
            {
                if (!string.IsNullOrEmpty(s))
                    if (string.IsNullOrEmpty(CAP))
                        s += " - ";
                    else
                        s += " ";
                s += City;
            }
            if (!string.IsNullOrEmpty(Province))
            {
                if (!string.IsNullOrEmpty(s)) s += " ";
                s += "(" + Province + ")";
            }
            if (string.IsNullOrEmpty(s)) s = "Nessun Indirizzo";
            return s;
        }
    }

}
