using Microsoft.Identity.Client;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;

namespace OpenDMS.Core.DataTypes
{
    public class IntegerFieldType : IDataTypeManager
    {
        public string DataTypeValue => "$$i";
        public string DataTypeName => "Numero Intero";
        public string Icon => "icoIntegerDataType";
        public string ControlType => "number";
        public bool IsSearchable => false;
        public bool IsInternal => false;
        public bool IsBlob => false;
        public bool IsCalculated => false;
        public bool IsPerson => false;
        public string AdminWebComponent => "AdminDateInputField";
        public string WebComponent => "NumberInputField";
        public FieldType[] AvailableFields =>  new [] {
            new FieldType { Customized=false, DataType =  DataTypeValue, Description = "Numero Intero", Id =DataTypeValue, Name = "Numero Intero", ColumnWidth="160px", Title = "Numero Intero", ControlType="number" }
        };

        public async Task<FieldTypeValue> Calculate(FieldType M, string Value, Document document)
        {
            throw new NotImplementedException();
        }
        public string Deserialize(string Value, bool Cifrato = false)
        {
            return Value.Trim();
        }

        public async Task<FieldTypeValue> Lookup(FieldType M, string Value)
        {
            NumberProperties props = M.CustomProperties is null ? new NumberProperties() : Newtonsoft.Json.JsonConvert.DeserializeObject<NumberProperties>(M.CustomProperties);
            FieldTypeValue FV = new FieldTypeValue();

            FV.IsValid = int.TryParse(Value, out var i);
            FV.FormattedValue = Serialize( Math.Truncate((decimal)(i << props.DecimalDigit)).ToString());
            FV.Value = i.ToString();
            FV.LookupValue = (props.Prefix + " " +  i.ToString((!String.IsNullOrEmpty(props.FormatMask) ? props.FormatMask : "#")+(props.DecimalDigit > 0 ? "."+("".PadRight(props.DecimalDigit, '0')) : "")).PadLeft(props.Length, props.Filler) + " " + props.Suffix).Trim();
            FV.FieldTypeId = M;
            return FV;
        }

        public async Task<List<FieldTypeValue>> Search(FieldType M, string SearchValue, int MaxResults = 8)
        {
            return null;
        }

        public string Serialize(string Value, bool Cifrato = false)
        {
            return Value.PadLeft(64, ' ');
        }

        public string Validate(FieldType M, string Value)
        {
            NumberProperties props = Newtonsoft.Json.JsonConvert.DeserializeObject<NumberProperties>(M.CustomProperties);
            return (int.TryParse(Value, out var i) && i >= props.MinValue && i <= props.MaxValue) ? "" : "Numero non valido";
        }


        public class NumberProperties
        {
            public string Prefix { get; set; } = "";
            public string Suffix { get; set; } = "";
            public string FormatMask { get; set; } = "#.###";
            public int DecimalDigit { get; set; } = 0;
            public decimal MinValue { get; set; } = 0;
            public decimal MaxValue { get; set; } = 999999999;
            public char Filler { get; set; } = ' ';
            public int Length { get; set; } = 0;

        }

    }
}
