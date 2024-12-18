using Newtonsoft.Json.Linq;

namespace OpenDMS.Domain.QueryBuilder;

public record QueryRow
{

    private Dictionary<string, string> _field = new();

    public string AsString(string alias)
    {
        return _field[alias];
    }
    public int AsInteger(string alias, int defaultValue=0)
    {
        if (int.TryParse(_field[alias], out var value))
            return value;
        else
            return defaultValue;
    }
    public decimal AsDecimal (string alias, decimal defaultValue = 0)
    {
        if (decimal.TryParse(_field[alias], out var value))
            return value;
        else
            return defaultValue;
    }

    public DateTime? AsDateTime(string alias)
    {
        if (DateTime.TryParse(_field[alias], out var value))
            return value;
        else
            return null;

    }

    public DateTime? AsNumericDate(string alias, bool timeIncluded = false)
    {
        if (int.TryParse(_field[alias], out var value))
            if (timeIncluded)
                return new DateTime((int)(value / 100_00_00_00_00), (value / 100_00_00_00) % 100, (value / 100_00_00) % 100, (value / 100_00) % 100, (value / 100) % 100, value % 100);
            else
                return new DateTime(value / 100_00, (value / 100) % 100, value % 100);
        else
            return null;

    }
    public string this[string alias] { get { return _field[alias]; } }

    public string this[int index] { get { return _field["A"+index.ToString()]; } }

    public QueryRow()
    {
        
    }

    public void Add (string alias, string value)
    {
        _field[alias] = value;
    }
}
