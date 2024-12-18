using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;

namespace OpenDMS.Core.DataTypes
{
    public class ParagraphFieldType : IDataTypeManager
    {
        public string DataTypeValue => "$$p";
        public string DataTypeName => "Testo multiriga";
        public string Icon => "icoParagraphDataType";
        public string ControlType => "textbox";
        public bool IsInternal => false;
        public bool IsSearchable => false;
        public bool IsCalculated => false;
        public bool IsBlob => false;
        public bool IsPerson => false;
        public string AdminWebComponent => "AdminParagraphInputField";
        public string WebComponent => "ParagraphInputField";
        public FieldType[] AvailableFields =>  new [] {
            new FieldType { Customized=false, DataType = DataTypeValue, Description = "Campo Testo Multiriga", Id = DataTypeValue, Name = "Testo Multiriga", Title = "Testo Multiriga" }
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
            FV.IsValid = Value.Length < 4000;
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
            return Value.Length < 4000 ? "" : "Testo troppo lungo";
        }

     

    }
}
