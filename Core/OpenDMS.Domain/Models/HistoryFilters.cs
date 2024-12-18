using OpenDMS.Domain.Enumerators;


namespace OpenDMS.Domain.Models
{
    public class HistoryFilters
    {
        public string Search { get; set; }
        public List<string> Events { get; set; } = new  List<string>();
        public int DocumentId { get; set; }
        public string UserId { get; set; }
        public string RecipientId { get; set; }
        public ProfileType RecipientType { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
