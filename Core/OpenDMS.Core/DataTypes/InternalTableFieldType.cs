using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;

namespace OpenDMS.Core.DataTypes
{
    public class InternalTableFieldType : IDataTypeManager
    {
        private readonly ILookupTableService lookupTableService;

        public string DataTypeValue => "$tb";
        public string ControlType => "lookup";
        public string DataTypeName => "Tabella Interna";
        public string Icon => "icoCustomTableType";
        public bool IsInternal => false;
        public bool IsBlob => false;
        public bool IsSearchable => true;
        public bool IsCalculated => false;
        public bool IsPerson => true;
        public string AdminWebComponent => "AdminProfileInputField";
        public string WebComponent => "ProfileInputField";
        public FieldType[] AvailableFields =>  new [] {
            new FieldType { ControlType="lookup",  Customized=false, DataType = DataTypeValue, Description = "Tabella Interna", Id = "$tb", Name = "Tabella Interna", Title = "Tabella Interna", Tag=false }
        };


        public InternalTableFieldType(ILookupTableService lookupTableService)//, IContactRepository bpRepo)
        {
            this.lookupTableService = lookupTableService;
        }
        public string Deserialize(string Value, bool Cifrato = false)
        {
            return Value;
        }
        public string Serialize(string Value, bool Cifrato = false)
        {
            return Value;
        }

        public async Task<FieldTypeValue> Calculate(FieldType M, string Value, Document document)
        {
            throw new NotImplementedException();
        }

        private class lookuptableproperties
        {
            public string ConnectionId { get; set; }
        }

        public async Task<FieldTypeValue> Lookup(FieldType M, string Value)
        {
            lookuptableproperties props = Newtonsoft.Json.JsonConvert.DeserializeObject<lookuptableproperties>(M.CustomProperties);
            //TODO: Cercare tra gli utenti e i gruppi
            FieldTypeValue FV = new FieldTypeValue();
            FV.IsValid = Value.Length > 0;
            FV.FormattedValue = Value;
            FV.Value = Value;
            FV.LookupValue = "?"+ Value;
            FV.Icon = "icoDataTypeTable";
            try
            {
                if (FV.IsValid)
                {
                    FV.IsValid = false;
                    //var ProfileType = (OpenDMS.Domain.Enumerators.ProfileType)int.Parse(Value.Substring(0, 1));
                    string ProfileId = Value;//.Substring(1);
                    var u = await lookupTableService.GetById(props.ConnectionId, Value);
                    FV.IsValid = u != null;
                    if (FV.IsValid)
                    {
                        FV.FormattedValue = u.Id;
                        FV.LookupValue = u.Description;
                    }
                    FV.FieldTypeId = M;
                }
            }
            catch {
                FV.IsValid = false;
            };
            return FV;
        }

        public async Task<List<FieldTypeValue>> Search(FieldType M, string SearchValue, int MaxResults = 8)
        {
            List<FieldTypeValue> FV = (await GetItems(M, SearchValue, MaxResults));
            return FV;
        }

        public string Validate(FieldType M, string Value)
        {
            return Value.Length < 4000 ? "" : "Testo troppo lungo";
        }

        private async Task<List<FieldTypeValue>> GetItems(FieldType M, string SearchValue, int MaxResults = 8)
        {
            lookuptableproperties props = Newtonsoft.Json.JsonConvert.DeserializeObject<lookuptableproperties>(M.CustomProperties);

            List<FieldTypeValue> items = new List<FieldTypeValue>();
            foreach (var u in (await lookupTableService.GetAll(props.ConnectionId)).Where(l=>l.Description.ToLower().Contains(SearchValue.ToLower())))
            {
                FieldTypeValue FV = new FieldTypeValue();
                FV.IsValid = true;
                FV.Value = u.Id;
                FV.LookupValue = u.Description;
                FV.FormattedValue = u.Id;
                FV.Icon = "icoDataTypeTable";

                items.Add(FV);
            }
            return items;
        }

  
    }
}
