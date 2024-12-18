using Google.Protobuf.WellKnownTypes;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;

namespace OpenDMS.Core.DataTypes
{
    public class UserProfileFieldType : IDataTypeManager
    {
        //private readonly IUserGroupRepository userGroup;
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly IOrganizationUnitService organizationService;
//        private readonly IContactRepository bpRepo;

        public string DataTypeValue => "$us";
        public string ControlType => "lookup";
        public string DataTypeName => "Utenti";
        public string Icon => "icoUserProfileType";
        public bool IsInternal => false;
        public bool IsSearchable => true;
        public bool IsCalculated => false;
        public bool IsBlob => false;
        public bool IsPerson => true;
        public string AdminWebComponent => "AdminProfileInputField";
        public string WebComponent => "ProfileInputField";
        public FieldType[] AvailableFields =>  new [] {
            new FieldType { ControlType="lookup",  Customized=false, DataType = DataTypeValue, Description = "Utenti", Id = "$us", Name = "Utenti", Title = "Utenti", Tag=true  }
        };


        public UserProfileFieldType(/*IUsergr IUserGroupRepository userGroup, */IUserService userService, IRoleService roleService, IOrganizationUnitService organizationService)//, IContactRepository bpRepo)
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
            FV.IsValid = Value.Length > 0;
            FV.FormattedValue = Value;
            FV.Value = Value;
            FV.LookupValue = "?"+ Value;
            FV.Icon = "/internalapi/identity/avatar/0" + Value;// "icoDataTypeUser";
            try
            {
                if (FV.IsValid)
                {
                    FV.IsValid = false;
                    //var ProfileType = (OpenDMS.Domain.Enumerators.ProfileType)int.Parse(Value.Substring(0, 1));
                    string ProfileId = Value;//.Substring(1);
                    var u = await userService.GetById(ProfileId);
                    FV.IsValid = u != null;
                    if (FV.IsValid)
                    {
                        FV.FormattedValue = u.Id;
                        FV.LookupValue = u.Contact.FriendlyName;
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
            return FV;
        }

        public string Validate(FieldType M, string Value)
        {
            return Value.Length < 4000 ? "" : "Testo troppo lungo";
        }

        private async Task<List<FieldTypeValue>> GetUsers(string SearchValue, int MaxResults = 8)
        {
            List<FieldTypeValue> items = new List<FieldTypeValue>();
            foreach (var u in await userService.Find(SearchValue, MaxResults))
            {
                FieldTypeValue FV = new FieldTypeValue();
                FV.IsValid = true;
                FV.Value = u.Id;
                FV.LookupValue = u.Contact.FriendlyName;
                FV.FormattedValue = u.Id;
                //FV.Icon = "icoDataTypeUser";
                FV.Icon = "/internalapi/identity/avatar/0" + u.Id;// "icoDataTypeUser";

                items.Add(FV);
            }
            return items;
        }

  
    }
}
