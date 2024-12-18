namespace OpenDMS.Core.DTOs
{
    public class AddOrUpdateDocumentField
    {
        public int FieldIndex { get; set; } = 0;
        public string FieldName { get; set; } = "";
        public string FieldTypeId { get; set; }
        public string Value { get; set; } = "";
    }
}
