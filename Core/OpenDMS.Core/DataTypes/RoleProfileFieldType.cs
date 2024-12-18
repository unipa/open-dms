using Google.Protobuf.WellKnownTypes;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;

namespace OpenDMS.Core.DataTypes
{
    public class RoleProfileFieldType : IDataTypeManager
    {
        //private readonly IUserGroupRepository userGroup;
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly IOrganizationUnitService organizationService;
//        private readonly IContactRepository bpRepo;

        public string DataTypeValue => "$ro";
        public string DataTypeName => "Utenti & Gruppi";
        public string Icon => "icoProfileType";
        public string ControlType => "lookup";
        public bool IsInternal => false;
        public bool IsSearchable => true;
        public bool IsCalculated => false;
        public bool IsBlob => false;
        public bool IsPerson => true;
        public string AdminWebComponent => "AdminProfileInputField";
        public string WebComponent => "ProfileInputField";
        public FieldType[] AvailableFields =>  new [] {
            new FieldType { ControlType="lookup",  Customized=false, DataType = DataTypeValue, Description = "Ruoli", Id = "$ro", Name = "Ruoli", Title = "Ruoli", Tag=true  }
        };


        public RoleProfileFieldType(/*IUsergr IUserGroupRepository userGroup, */IUserService userService, IRoleService roleService, IOrganizationUnitService organizationService)//, IContactRepository bpRepo)
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
            FV.LookupValue = "?"+ Value;
            //FV.Icon = "icoDataTypeRole";
            FV.Icon = "/internalapi/identity/avatar/2" + Value;// "icoDataTypeUser";
            FV.Value = Value;

            if (FV.IsValid)
            {
                FV.IsValid = false;
                string ProfileId = Value;
                var u = await roleService.GetById(ProfileId);
                FV.IsValid = u != null;
                if (FV.IsValid)
                {
                    FV.FormattedValue = u.Id;
                    FV.LookupValue = u.RoleName;
                }
                FV.FieldTypeId = M;
            }
            return FV;
        }

        public async Task<List<FieldTypeValue>> Search(FieldType M, string SearchValue, int MaxResults = 8)
        {
            List<FieldTypeValue> FV =(await GetRoles(SearchValue, MaxResults));
            return FV;
        }

        public string Validate(FieldType M, string Value)
        {
            return Value.Length < 4000 ? "" : "Testo troppo lungo";
        }


        private async Task<List<FieldTypeValue>> GetRoles(string SearchValue, int MaxResults = 8)
        {
            List<FieldTypeValue> items = new List<FieldTypeValue>();
            foreach (var u in await roleService.Find(SearchValue, MaxResults))
            {
                FieldTypeValue FV = new FieldTypeValue();
                FV.IsValid = true;
                FV.Value = u.Id;
                FV.Icon = "/internalapi/identity/avatar/2" + u.Id;// "icoDataTypeUser";
                FV.LookupValue = u.RoleName;
                FV.FormattedValue = u.Id;
                items.Add(FV);
            }
            return items;
        }
   
     
    }
}
