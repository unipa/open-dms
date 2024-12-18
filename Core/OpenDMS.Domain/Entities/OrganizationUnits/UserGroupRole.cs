using Microsoft.EntityFrameworkCore;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace OpenDMS.Domain.Entities.OrganizationUnits;


[PrimaryKey(nameof(Id))]
[Index(nameof(StartISODate), nameof(UserId), nameof(UserGroupId),  nameof(RoleId), IsUnique = true)]
[Index(nameof(StartISODate), nameof(UserId), nameof(RoleId), IsUnique = false)]
[Index(nameof(StartISODate), nameof(UserId), nameof(UserGroupId), IsUnique = false)]
public class UserGroupRole
{
    [Key]
    public int Id { get; set; }

    [StringLength(64)]
    public string UserGroupId { get; set; }
    [StringLength(64)]
    public string UserId { get; set; }

    /// <summary>
    /// Data Inizio Validità dell'utente nel gruppo (YYYYMMDD)
    /// </summary>
    public int StartISODate { get; set; } = 0;

    /// <summary>
    /// Data Fine Validità dell'utente nel gruppo (YYYYMMDD) (0 oppure 99999999 per non avere fine)
    /// </summary>
    public int EndISODate { get; set; } = 0;


    [StringLength(64)]
    public string RoleId { get; set; }

  //  [JsonIgnore]
    public virtual UserGroup UserGroup { get; set; }

//    [JsonIgnore]
    public virtual User User { get; set; }
//    [JsonIgnore]
    public virtual Role Role { get; set; }
}

