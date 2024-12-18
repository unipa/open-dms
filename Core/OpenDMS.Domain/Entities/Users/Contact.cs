using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Domain.Entities.Users;


[Index(nameof(ParentId), IsUnique = false)]
[Index(nameof(SearchName), IsUnique = false)]
[Index(nameof(CountryCode), nameof(LicTradNum), IsUnique = false)]
[Index(nameof(FiscalCode), IsUnique = false)]


public class Contact
{
    [StringLength(64)]

    public string Id { get; set; }

    [StringLength(64)]
    public string ParentId { get; set; }

    public ContactType ContactType { get; set; } = ContactType.Undefined;

    [StringLength(255)]
    public string FullName { get; set; }

    [StringLength(128)]
    public string FriendlyName { get; set; }

    [StringLength(128)]
    public string SearchName { get; set; }

    [StringLength(6)]
    public string CountryCode { get; set; }

    [StringLength(16)]
    public string LicTradNum { get; set; }

    [StringLength(16)]
    public string FiscalCode { get; set; }


    [StringLength(8)]
    public string IPACode { get; set; }
    [StringLength(255)]
    public string Avatar { get; set; }


    [StringLength(1)]
    public string Sex { get; set; }       // G: M=Maschio, F=Femmina, =Impresa | @: = Email/PEC, A=Address, P=Phone,

    [StringLength(64)]
    public string SurName { get; set; }

    [StringLength(64)]
    public string FirstName { get; set; }



    [StringLength(64)]
    public string CreationUser { get; set; }

    [StringLength(64)]
    public string LastUpdateUser { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdate { get; set; } = DateTime.UtcNow;


    [StringLength(255)]
    public string UpdateErrors { get; set; }
    public bool Deleted { get; set; }

    [StringLength(255)]
    public string Annotation { get; set; }




    public virtual Contact Parent { get; set; }
    public virtual List<Contact> Contacts { get; set; } = new List<Contact>();

    public virtual List<ContactAddress> Addresses { get; set; } = new List<ContactAddress>();
    public virtual List<ContactDigitalAddress> DigitalAddresses { get; set; } = new List<ContactDigitalAddress>();


}


/*
 *   modelBuilder.Entity<Contact>()
            .HasMany(j => j.Contacts)
            .WithOne(j => j.Parent)
            .HasForeignKey(j => j.ParentId)
 */