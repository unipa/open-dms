using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using System.Text.RegularExpressions;

namespace OpenDMS.Core.DataTypes
{
    public class MailAddressFieldType : IDataTypeManager
    {
        //private readonly IUserGroupRepository userGroup;
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly IOrganizationUnitService organizationService;
//        private readonly IContactRepository bpRepo;

        public string DataTypeValue => "$$@";
        public string DataTypeName => "Indirizzo Email";
        public string Icon => "icoMailAddressType";
        public string ControlType => "lookup";
        public bool IsInternal => false;
        public bool IsSearchable => true;
        public bool IsBlob => false;
        public bool IsCalculated => false;
        public bool IsPerson => true;
        public string AdminWebComponent => "AdminMailInputField";
        public string WebComponent => "MailInputField";
        public FieldType[] AvailableFields =>  new [] {
            new FieldType { ControlType="lookup",  Customized=false, DataType = DataTypeValue, Description = "Indirizzo Email", Id = "$$@", Name = "Email", Title = "Email", Tag=true  }
        };


        public MailAddressFieldType(/*IUsergr IUserGroupRepository userGroup, */IUserService userService, IRoleService roleService, IOrganizationUnitService organizationService)//, IContactRepository bpRepo)
        {
            //this.userGroup = userGroup;
            this.userService = userService;
            this.roleService = roleService;
            this.organizationService = organizationService;
            //this.bpRepo = bpRepo;
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


        public async Task<FieldTypeValue> Lookup(FieldType M, string Value)
        {
            //TODO: Cercare tra gli utenti e i gruppi
            FieldTypeValue FV = new FieldTypeValue();
            FV.IsValid = Value.Length > 0 && Value.Contains("@");
            FV.FormattedValue = "";
            FV.Value = "";
            FV.LookupValue = "";
            FV.Icon = "icoDataTypeMail";
            try
            {
                if (FV.IsValid)
                {
                    if (Value.IndexOf("<") >= 0)
                    {
                        Value = Value.Substring(Value.IndexOf("<") + 1);
                        if (Value.IndexOf(">") >= 0)
                        {
                            Value = Value.Substring(0, Value.IndexOf(">"));
                        }
                    }
                    FV.IsValid = Regex.IsMatch(Value, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                    if (FV.IsValid)
                    {
                        FV.Value = Value;
                        FV.LookupValue = Value;
                        FV.FormattedValue = Value;
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
                List<FieldTypeValue> FV = (await GetUsers(SearchValue, MaxResults));
            if (FV.Count == 0) FV.Add(await  Lookup(M, SearchValue));
            return FV;
        }

        public string Validate(FieldType M, string Value)
        {
            return Value.Length < 4000 ? "" : "Testo troppo lungo";
        }

        private async Task<List<FieldTypeValue>> GetUsers(string SearchValue, int MaxResults = 8)
        {
            List<FieldTypeValue> items = new List<FieldTypeValue>();
            foreach (var u in await userService.FindMailAddresses (SearchValue, MaxResults))
            {
                FieldTypeValue FV = new FieldTypeValue();
                FV.IsValid = true;
                FV.Value = u.SearchName + "<" + u.Address + ">";
                FV.LookupValue = u.SearchName+"<"+u.Address+">";
                FV.FormattedValue = u.SearchName + "<" + u.Address + ">";
                FV.Icon = "icoDataTypeMail";

                items.Add(FV);
            }
            return items;
        }

  
    }
}
