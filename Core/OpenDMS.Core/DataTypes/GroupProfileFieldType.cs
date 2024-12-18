using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;

namespace OpenDMS.Core.DataTypes
{
    public class GroupProfileFieldType : IDataTypeManager
    {
        //private readonly IUserGroupRepository userGroup;
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly IOrganizationUnitService organizationService;
//        private readonly IContactRepository bpRepo;

        public string DataTypeValue => "$gr";
        public string DataTypeName => "Gruppi";
        public string Icon => "icoGroupProfileType";
        public string ControlType => "lookup";
        public bool IsInternal => false;
        public bool IsSearchable => true;
        public bool IsBlob => false;
        public bool IsCalculated => false;
        public bool IsPerson => true;
        public string AdminWebComponent => "AdminProfileInputField";
        public string WebComponent => "ProfileInputField";
        public FieldType[] AvailableFields =>  new [] {
            new FieldType { ControlType="lookup", Customized=false,  DataType = DataTypeValue, Description = "Gruppi", Id = "$gr", Name = "Gruppi", Title = "Gruppi", Tag=true  }
        };


        public GroupProfileFieldType(/*IUsergr IUserGroupRepository userGroup, */IUserService userService, IRoleService roleService, IOrganizationUnitService organizationService)//, IContactRepository bpRepo)
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
            //FV.Icon = "icoDataTypeUserGroup";
            FV.Icon = "/internalapi/identity/avatar/1" + Value;// "icoDataTypeUser";

            if (FV.IsValid)
            {
                FV.IsValid = false;
                string ProfileId = Value; //.Substring(1);
                var u = await organizationService.GetGroup(ProfileId);

                FV.IsValid = u != null;
                if (FV.IsValid) {
                    FV.FormattedValue = u.Id;
                    FV.LookupValue = u.Name;
                }        
                FV.FieldTypeId = M;
            }
            return FV;
        }

        public async Task<List<FieldTypeValue>> Search(FieldType M, string SearchValue, int MaxResults = 8)
        {
            List<FieldTypeValue> FV = (await GetGroups(SearchValue, MaxResults));
            return FV;
        }

        public string Validate(FieldType M, string Value)
        {
            return Value.Length < 4000 ? "" : "Testo troppo lungo";
        }

        private async Task<List<FieldTypeValue>> GetGroups(string SearchValue, int MaxResults = 8)
        {
            //TODO: Recuperare L'organigramma se non viene indicato un SearchValue
            List<FieldTypeValue> items = new List<FieldTypeValue>();
            foreach (var u in await organizationService.Find(SearchValue, MaxResults))
            {
                FieldTypeValue FV = new FieldTypeValue();
                FV.IsValid = true;
                FV.Value = u.Id;
                FV.LookupValue = u.Name;
                //FV.Icon = "icoDataTypeUserGroup";
                FV.Icon = "/internalapi/identity/avatar/1" + u.Id;// "icoDataTypeUser";
                FV.FormattedValue = u.Id;
                FV.Children = new List<FieldTypeValue>();
                items.Add(FV);
            }
            return items;
        }

   

    
    }
}
