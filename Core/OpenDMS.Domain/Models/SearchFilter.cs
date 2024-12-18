using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Domain.Models
{
    public class SearchFilter
    {
        public string ColumnName { get; set; }
        public OperatorType Operator { get; set; } = OperatorType.EqualTo;

        /// <summary>
        /// Id del tipo di dato gestito dai metadati
        /// </summary>
        public string CustomTypeId { get; set; }
        public List<string> Values { get; set; } = new ();


        public SearchFilter()
        {
            
        }
    }
}
