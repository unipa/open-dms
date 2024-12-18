using AutoMapper;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Entities.OrganizationUnits;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;

namespace OpenDMS.Core.DataTypes
{
    public class OrganizationProfileFieldType : IDataTypeManager
    {
        //private readonly IUserGroupRepository userGroup;
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly IOrganizationUnitService organizationService;
//        private readonly IContactRepository bpRepo;

        public string DataTypeValue => "$ug";
        public string DataTypeName => "Utenti & Gruppi";
        public string Icon => "icoOrganizationProfileType";
        public string ControlType => "lookup";
        public bool IsInternal => false;
        public bool IsSearchable => true;
        public bool IsCalculated => false;
        public bool IsBlob => false;
        public bool IsPerson => true;
        public string AdminWebComponent => "AdminProfileInputField";
        public string WebComponent => "ProfileInputField";
        public FieldType[] AvailableFields =>  new [] {
            new FieldType { ControlType="lookup",  Customized=false, DataType = DataTypeValue, Description = "Organigramma", Id = "$ug", Name = "Organigramma", Title = "Organigramma", Tag=true  },
        };


        public OrganizationProfileFieldType(/*IUsergr IUserGroupRepository userGroup, */IUserService userService, IRoleService roleService, IOrganizationUnitService organizationService)//, IContactRepository bpRepo)
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
                FV.IsValid = Value.Length > 1;
                FV.FormattedValue = Value;
                FV.Value = Value;
                FV.LookupValue = "?" + Value;
            try
            {
                if (FV.IsValid)
                {
                    FV.IsValid = false;
                    var ProfileType = (OpenDMS.Domain.Enumerators.ProfileType)int.Parse(Value.Substring(0, 1));
                    string ProfileId = Value.Substring(1);
                    if (ProfileType == OpenDMS.Domain.Enumerators.ProfileType.Role)
                    {
                        var RoleId = ProfileId;
                        var GroupId = "";
                        if (ProfileId.IndexOf("\\") >= 0)
                        {
                            RoleId = ProfileId.Substring(0, ProfileId.IndexOf("\\"));
                            GroupId = ProfileId.Substring(ProfileId.IndexOf("\\") + 1);
                        }
                        var r = await roleService.GetById(RoleId);
                        FV.IsValid = r != null;
                        //FV.Icon = "icoDataTypeUserRole";
                        FV.Icon = "/internalapi/identity/avatar/2" + r.RoleName;// "icoDataTypeUser";
                        if (FV.IsValid)
                        {
                            string RoleName = r.RoleName + " ";
                            if (r.Id == "user")
                                RoleName = "Tutti gli utenti ";
                            else
                            {
                                RoleName = RoleName.Replace("a ", "i ").Replace("e ", "i ").Replace("io ", "i ").Replace("o ", "i ");
                            }
                            FV.IsValid = String.IsNullOrEmpty(GroupId);
                            if (!FV.IsValid)
                            {
                                var u = await organizationService.GetGroup(GroupId);
                                // Si tratta di Gruppo\Ruolo
                                FV.IsValid = u != null;
                                if (FV.IsValid) RoleName += " (" + u.ShortName + ")";
                            }
                            if (FV.IsValid)
                            {
                                FV.LookupValue = RoleName;
                            }
                        }
                    }
                    else
                    if (ProfileType == OpenDMS.Domain.Enumerators.ProfileType.User)
                    {
                        var u = await userService.GetById(ProfileId);
                        FV.IsValid = u != null;
                        //FV.Icon = "icoDataTypeUser";
                        FV.Icon = "/internalapi/identity/avatar/0" + u.Id;// "icoDataTypeUser";

                        if (FV.IsValid)
                        {
                            FV.FormattedValue = ((int)OpenDMS.Domain.Enumerators.ProfileType.User).ToString() + u.Id;
                            FV.LookupValue = u.Contact.FriendlyName;
                        }
                    }
                    else
                    if (ProfileType == OpenDMS.Domain.Enumerators.ProfileType.Group)
                    {
                        var u = await organizationService.GetGroup(ProfileId);
                        FV.IsValid = u != null;
                        //FV.Icon = "icoDataTypeGroup";
                        FV.Icon = "/internalapi/identity/avatar/1" + u.Id;// "icoDataTypeUser";
                        if (FV.IsValid)
                        {
                            FV.FormattedValue = ((int)OpenDMS.Domain.Enumerators.ProfileType.Group).ToString() + u.Id;
                            FV.LookupValue = u.ShortName;
                        }
                    }
                    FV.FieldTypeId = M;
                }
            }
            catch {
                FV.IsValid = false;
            };
            return FV;
        }

        public async Task<List<FieldTypeValue>> Search(FieldType M, string SearchValue, int MaxResults = 20)
        {
            List<FieldTypeValue> FV = new List<FieldTypeValue>();
            //            FV.AddRange(await GetOrganizationUnits(SearchValue, MaxResults));
            string FirstLetter = SearchValue.Length > 0 ? SearchValue.Substring(0,1) : "";
            switch (FirstLetter)
            {
                case ":":
                case "?":
                case "*":
                    FV.AddRange(await GetRoles(SearchValue.Substring(1), MaxResults));
                    break;
                case "@":
                    FV.AddRange(await GetUsers(SearchValue.Substring(1), MaxResults));
                    break;
                case "\\":
                case "/":
                    var s = SearchValue.Substring(1);
//                    if (string.IsNullOrEmpty(s)) s = "*";
                    FV.AddRange(await GetGroups(s, MaxResults));
                    break;
                default:

                    FV.AddRange(await GetGroups(SearchValue, MaxResults));
                    var Rest = MaxResults - FV.Count;
                    if (Rest > 0) FV.AddRange(await GetRoles(SearchValue, MaxResults));
                    Rest = MaxResults - FV.Count;
                    if (Rest > 0 ) FV.AddRange(await GetUsers(SearchValue, Rest));
                    break;
            }
            return FV;
        }

        private async Task<List<FieldTypeValue>> GetRoles(string SearchValue, int MaxResults = 8)
        {
            List<FieldTypeValue> items = new List<FieldTypeValue>();
            foreach (var u in await roleService.Find(SearchValue, MaxResults))
            {
                FieldTypeValue FV = new FieldTypeValue();
                FV.IsValid = true;
                FV.Value = u.Id;
                //FV.Icon = "iconDataTypeRole";
                FV.Icon = "/internalapi/identity/avatar/2" + u.Id;// "icoDataTypeUser";
                FV.LookupValue = u.RoleName;
                FV.FormattedValue = ((int)OpenDMS.Domain.Enumerators.ProfileType.Role).ToString()  + u.Id;
                items.Add(FV);
            }
            return items;
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
                //FV.Icon = "iconDataTypeUser";
                FV.Icon = "/internalapi/identity/avatar/0" + u.Id;// "icoDataTypeUser";
                FV.LookupValue = u.Contact.FriendlyName;
                if (string.IsNullOrEmpty(FV.LookupValue)) FV.LookupValue = u.Contact.SearchName;
                if (string.IsNullOrEmpty(FV.LookupValue)) FV.LookupValue = u.Contact.FullName;
                FV.FormattedValue = ((int)OpenDMS.Domain.Enumerators.ProfileType.User).ToString() + u.Id;
                items.Add(FV);
            }
            return items;
        }

        private async Task<List<FieldTypeValue>> GetChildGroups(string parentId)
        {
            List<FieldTypeValue> items = new();
            var tree = await organizationService.GetOrganizationTree(0,parentId);
            if (tree != null)
            {
                foreach (var u in tree)
                {
                    FieldTypeValue FV = new FieldTypeValue();
                    FV.IsValid = true;
                    FV.Value = ((int)OpenDMS.Domain.Enumerators.ProfileType.Group).ToString() + u.Info.UserGroupId;
                    FV.LookupValue = u.Info.Name;
                    FV.FormattedValue = ((int)OpenDMS.Domain.Enumerators.ProfileType.Group).ToString() + u.Info.UserGroupId;
                    //FV.Icon = "icoDataTypeUserGroup";
                    FV.Icon = "/internalapi/identity/avatar/1" + u.Info.UserGroupId;// "icoDataTypeUser";
                    FV.Children = new List<FieldTypeValue>();
                    FV.Children.AddRange(await GetChildGroups(u.Info.UserGroupId));
                    Dictionary<string, FieldTypeValue> roles = new Dictionary<string, FieldTypeValue>();
                    foreach (var R in (await organizationService.GetUsers(u.Info.UserGroupId, -1)))
                    {
                        var Id = R.RoleId.ToLower();
                        FieldTypeValue Role = new FieldTypeValue();
                        if (roles.ContainsKey(Id))
                        {
                            Role = (FieldTypeValue)roles[Id];
                        }
                        else
                        {
                            Role.IsValid = true;
                            Role.Value = ((int)OpenDMS.Domain.Enumerators.ProfileType.Role).ToString() + R.RoleId + "\\" + u.Info.UserGroupId;
                            Role.LookupValue = R.RoleName;
                            Role.FormattedValue = ((int)OpenDMS.Domain.Enumerators.ProfileType.Role).ToString() + R.RoleId + "\\" + u.Info.UserGroupId;
                            //Role.Icon = "icoDataTypeRole";
                            Role.Icon = "/internalapi/identity/avatar/2" + R.RoleId;// "icoDataTypeUser";
                            roles[Id] = Role;
                            FV.Children.Add(Role);
                        }
                        FieldTypeValue FU = new FieldTypeValue();
                        FU.IsValid = true;
                        FU.Value = ((int)OpenDMS.Domain.Enumerators.ProfileType.User).ToString() + R.UserId;
                        FU.LookupValue = R.UserName;
                        //FU.Icon = "icoDataTypeUser";
                        FU.Icon = "/internalapi/identity/avatar/0" + R.UserId;// "icoDataTypeUser";
                        FU.FormattedValue = ((int)OpenDMS.Domain.Enumerators.ProfileType.User).ToString() + R.UserId;
                        Role.Children.Add(FU);
                    }
                    items.Add(FV);
                }
            }
            return items;
        }

    

        private async Task<List<FieldTypeValue>> GetGroups(string SearchValue, int MaxResults = 8)
        {
            List<FieldTypeValue> items = new();
            List<UserGroup> results = new();
            if (string.IsNullOrEmpty(SearchValue))
            {
                return await GetChildGroups("");
            }
            else
            {
                results = await organizationService.Find(SearchValue, MaxResults);
                foreach (var u in results)
                {
                    FieldTypeValue FV = new FieldTypeValue();
                    FV.IsValid = true;
                    FV.Value = ((int)OpenDMS.Domain.Enumerators.ProfileType.Group).ToString() + u.Id;
                    FV.LookupValue = u.Name;
                    FV.FormattedValue = ((int)OpenDMS.Domain.Enumerators.ProfileType.Group).ToString() + u.Id;
                    FV.Icon = "/internalapi/identity/avatar/1" + u.Id;
                    FV.Children = new List<FieldTypeValue>();
                    FV.Children.AddRange(await GetChildGroups(u.Id));
                    items.Add(FV);
                }
                return items;
            }
        }
    }
}
