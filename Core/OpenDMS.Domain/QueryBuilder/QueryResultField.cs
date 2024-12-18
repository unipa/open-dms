namespace OpenDMS.Domain.QueryBuilder;

public record QueryResultField
{
    public QueryResultField(string value, string lookupValue)
    {
        Value = value;
        LookupValue = lookupValue;
    }

    public string Value { get; }
    public string LookupValue { get; }
}
