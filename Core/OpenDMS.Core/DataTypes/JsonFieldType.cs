using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;

namespace OpenDMS.Core.DataTypes
{
    public class JsonFieldType : IDataTypeManager
    {
        public string DataTypeValue => "$$j";
        public string DataTypeName => "JSon";
        public string ControlType => "textbox";
        public string Icon => "icoJsonDataType";
        public bool IsSearchable => false;
        public bool IsCalculated => false;
        public bool IsInternal => true;
        public bool IsPerson => false;
        public bool IsBlob => true;
        public string AdminWebComponent => "AdminJsonInputField";
        public string WebComponent => "JsonInputField";
        public FieldType[] AvailableFields =>  new [] {
            new FieldType { Customized=false, DataType = DataTypeValue, Description = "Oggetto Json", Id = DataTypeValue, Name = "Json", Title = "Json" }
        };

        public string Deserialize(string Value, bool Cifrato = false)
        {
            return Value;
        }

        public async Task<FieldTypeValue> Calculate(FieldType M, string Value, Document document)
        {
            throw new NotImplementedException();
        }
        public async Task<FieldTypeValue> Lookup(FieldType M, string Value)
        {

            FieldTypeValue FV = new FieldTypeValue();
            FV.FormattedValue = Value;
            FV.LookupValue = Value;
            FV.Value = Value;
            FV.IsValid = true;
            FV.FieldTypeId = M;
            return FV;
        }

        public async Task<List<FieldTypeValue>> Search(FieldType M, string SearchValue, int MaxResults = 8)
        {
            return null;
        }

        public string Serialize(string Value, bool Cifrato = false)
        {
            return Value;
        }

        public string Validate(FieldType M, string Value)
        {
            return "";
        }
 
    }
}
