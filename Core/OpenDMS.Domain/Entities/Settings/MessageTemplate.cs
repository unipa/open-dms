using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.Settings;
public class MessageTemplate
{
    [Key]
    [StringLength(255)]
    public string Id { get; set; }

    /// <summary>
    /// Oggetto del messaggio 
    /// </summary>
    [StringLength(255)]
    public string Title { get; set; }

    public string Body { get; set; }

    /// <summary>
    /// Destinatari Sempre inclusi
    /// </summary>
    [StringLength(255)]
    public string CCr { get; set; }


    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
}

