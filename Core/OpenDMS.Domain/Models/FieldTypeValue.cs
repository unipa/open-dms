using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Entities.Settings;

namespace OpenDMS.Domain.Models;

public class FieldTypeValue
{
    public FieldType FieldTypeId { get; set; }
    public bool IsValid { get; set; }
    public string Value { get; set; }
    public string FormattedValue { get; set; }
    public string LookupValue { get; set; }
    public List<LookupTable> Fields { get; set; } = new List<LookupTable>();
    public string Icon { get; set; }
//    public Dictionary<string, string> Fields { get; set; } = new Dictionary<string, string>();
    public List<FieldTypeValue> Children { get; set; } = new List<FieldTypeValue>();

}
