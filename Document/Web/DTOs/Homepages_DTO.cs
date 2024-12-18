using OpenDMS.CustomPages.Models;

namespace Web.DTOs
{
    public class Homepages_DTO
    {
        public List<CustomPageDTO> Pages { get; set; } = new();
        public string HomePage { get; set; } = "";
    }
}
