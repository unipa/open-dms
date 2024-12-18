using System.ComponentModel.DataAnnotations;

namespace OpenDMS.MultiTenancy.Interfaces;
public interface ITenant
{
    [StringLength(64)]
    string Id { get; set; }

    [StringLength(250)]
    string ConnectionString { get; set; }
    [StringLength(50)]
    string Provider { get; set; }
}
