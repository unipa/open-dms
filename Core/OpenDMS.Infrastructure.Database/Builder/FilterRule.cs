using OpenDMS.Domain.Enumerators;


namespace OpenDMS.Infrastructure.Database.Builder
{

        public record FilterRule
        {
            public string Element { get; }
            public OperatorType OperatorType { get; }
            public string[] Values { get; }
            public string FirstValue { get { return Values.Length > 0 ? Values[0] : null; } }
            public string LastValue { get { return Values.Length > 1 ? Values[1] : FirstValue; } }

            public FilterRule(string element, OperatorType operatorType, string[] values)
            {
                Element = element;
                OperatorType = operatorType;
                Values = values;
            }
        }
}
