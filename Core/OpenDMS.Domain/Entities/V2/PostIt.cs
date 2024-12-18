using System.ComponentModel.DataAnnotations;

namespace OpenDMS.Domain.Entities.V2;

public class PostIt
{
    public int Id { get; set; }
    public int DocumentId { get; set; }


    [StringLength(64)]
    public string UserId { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    public string Message { get; set; }


    // VERSIONE 23.04
    public int PageIndex { get; set; }
    public int Left { get; set; }
    public int Top { get; set; }






}