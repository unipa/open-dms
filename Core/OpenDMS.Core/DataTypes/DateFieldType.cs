using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;

namespace OpenDMS.Core.DataTypes
{
    public class DateFieldType : IDataTypeManager
    {
        public string DataTypeValue => "$$d";
        public string DataTypeName => "Data";
        public string Icon => "icoDateDataType";
        public string ControlType => "date";
        public bool IsSearchable => false;
        public bool IsInternal => false;
        public bool IsBlob => false;
        public bool IsCalculated => false;
        public bool IsPerson => false;
        public string AdminWebComponent => "AdminDateInputField";
        public string WebComponent => "DateInputField";
        public FieldType[] AvailableFields =>  new [] {
            new FieldType { Customized=false, DataType =  DataTypeValue, Description = "Campo Data", Id =DataTypeValue, ColumnWidth="160px", Name = "Data", Title = "Data", ControlType="date" }
        };

        public async Task<FieldTypeValue> Calculate(FieldType M, string Value, Document document)
        {
            throw new NotImplementedException();
        }
        public string Deserialize(string Value, bool Cifrato = false)
        {
            DateTime dt = DateTime.MinValue;
            if (DateTime.TryParse(Value, out DateTime i))
            {
                //dt = new DateTime(i / 10000, (i / 100) % 100, i % 100);
                return i.ToString("yyyy-MM-dd");
            }
            else return "";
        }

        public async Task<FieldTypeValue> Lookup(FieldType M, string Value)
        {
            FieldTypeValue FV = new FieldTypeValue();

            DateTime dt = DateTime.MinValue;
            FV.IsValid = DateTime.TryParse(Value, out dt);
            FV.FormattedValue = dt.ToString("yyyy-MM-dd");
            FV.Value = dt.ToString("yyyy-MM-dd");
            FV.LookupValue = dt.Date == DateTime.UtcNow ? "Oggi" : dt.ToString("dd/MM/yyyy");
            FV.FieldTypeId = M;
            return FV;
        }

        public async Task<List<FieldTypeValue>> Search(FieldType M, string SearchValue, int MaxResults = 8)
        {
            return null;
        }

        public string Serialize(string Value, bool Cifrato = false)
        {
            DateTime dt = DateTime.MinValue;
            if (DateTime.TryParse(Value, out dt))
                return dt.ToString("yyyy-MM-dd");
            else
                return "";
        }

        public string Validate(FieldType M, string Value)
        {
            DateTime dt = DateTime.MinValue;
            return  (DateTime.TryParse(Value, out dt)) ? "" : "Data non valida";
        }


    }
}
