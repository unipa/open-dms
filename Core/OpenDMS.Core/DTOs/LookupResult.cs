namespace OpenDMS.Core.DTOs;

public class LookupResult
{
    public string Icon { get; set; }
    public string Value { get; set; }
    public string LookupValue { get; set; }
    public bool Selectable { get; set; }
    public List<LookupResult> Details { get; set; }
    public List<LookupResult> Items { get; set; }
}
