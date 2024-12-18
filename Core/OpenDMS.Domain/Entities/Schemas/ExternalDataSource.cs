using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenDMS.Domain.Entities.Schemas;

public class ExternalDatasource
{
    [StringLength(64)]
    public string Id { get; set; }

    [Required]
    [StringLength(64)]
    public string Name { get; set; }
    [StringLength(64)]
    [Required]
    public string Driver { get; set; }
    [StringLength(64)]

    public string Provider { get; set; }
    [StringLength(128)]
    [Required]
    public string Connection { get; set; }
    [StringLength(64)]
    public string UserName { get; set; }
    [StringLength(255)]
    public string Password { get; set; }

    [NotMapped]
    public bool IsNew { get { return string.IsNullOrEmpty(Id); } }

    [NotMapped]
    public string ConnectionString
    {
        get
        {
            return Connection + ";" + (string.IsNullOrEmpty(UserName) ? "Integrated Security=SSPI" : "User ID=" + UserName + ";Password=" + Password);
        }
    }

    public ExternalDatasource()
    {
    }



}


