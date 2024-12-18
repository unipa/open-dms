using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.OrganizationUnits;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;

namespace OpenDMS.Core.BusinessLogic
{
    public class OrganizationUnitService : IOrganizationUnitService
    {
        private readonly IOrganizationRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IACLService aclService;
        private readonly IRoleService _roleService;
        private List<Role> roles = null;
        private List<OrganizationNode> nodes = null;
        private Dictionary<int, List<OrganizationNode>> cache = new();

        //TODO: aggiungere i log


        public OrganizationUnitService(
            IOrganizationRepository repo,
            IUserRepository userService,
            IUserGroupRepository userGroupRepository,
            IACLService aclService,
            IRoleService roleRepo)
        {
            this._repository = repo;
            this._userRepository = userService;
            this._roleService = roleRepo;
            this._userGroupRepository = userGroupRepository;
            this.aclService = aclService;
        }

        public async Task<List<UserGroupRole>> GetUsersByFilter(UserFilter filter)
        {
            return await _repository.GetGroupsByRole(filter);
        }


        public async Task<List<OrganizationNodeTree>> GetOrganizationTree(int StartISODate = 0, string rootNode = "")
        {
            if (roles == null) roles = await _roleService.GetAll();
            OrganizationNodeTree tree = new();
            Dictionary<string, OrganizationNodeTree> map = new Dictionary<string, OrganizationNodeTree>();
            var nodes = cache.ContainsKey(StartISODate) ? cache[StartISODate] : null;
            if (nodes== null) nodes = await _repository.GetAll(StartISODate);
            cache[StartISODate] = nodes; 
            foreach (var node in nodes)
            {
                if (node.UserGroup != null)
                {
                    var parent = node.ParentUserGroupId;
                    OrganizationNodeTree parentNode = null;
                    if (!String.IsNullOrEmpty(parent))
                        parentNode = map.ContainsKey(parent) ? map[parent] : null;
                    OrganizationNodeTree N = new();
                    N.Info = new OrganizationNodeInfo(node);
                    //N.Parent = parentNode is null ? tree : parentNode;
                    //var users = await _repository.GetUsersInGroup(StartISODate, node.UserGroupId);
                    //foreach (var user in users)
                    //{
                    //    UserInGroup U = new UserInGroup(user);
                    //    U.RoleName = roles.FirstOrDefault(r => r.Id == user.RoleId)?.RoleName;
                    //    U.UserName = (await _userRepository.GetById(U.UserId)).Contact.FriendlyName;
                    //    U.Id = user.Id;
                    //    N.Users.Add(U);
                    //}
                    map[node.UserGroupId] = N;
                    if (parentNode is null)
                        tree.Nodes.Add(N);
                    else
                        parentNode.Nodes.Add(N);
                }
            }
            return string.IsNullOrEmpty(rootNode) ? tree.Nodes : map.ContainsKey(rootNode) ? map[rootNode].Nodes : null;
        }

        public async Task<UserGroup> GetGroup(string groupId)
        {
            return await _userGroupRepository.GetById(groupId);
        }
        public async Task<List<UserGroup>> Find(string SearchText, int MaxResults = 0)
        {
            return await _userGroupRepository.Find(SearchText, MaxResults);
        }

        public async Task<List<UserInGroup>> GetGroupsByUser(string userId)
        {
            if (roles == null) roles = await _roleService.GetAll();
            List<UserInGroup> groupList = new List<UserInGroup>();
            HashSet<string> groupRead = new HashSet<string>();
            var groups = await _repository.GetGroupsByUser(userId);
            int data =  int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
            if (nodes == null) nodes = await _repository.GetAll(data);

            //var allgroups = await _repository.GetAll(data);
            foreach (var user in groups)
            {
                UserInGroup U = new UserInGroup(user);
                var roleName = roles.FirstOrDefault(r => r.Id == user.RoleId)?.RoleName;
                U.RoleName = roleName;
                U.UserGroup = (await _userGroupRepository.GetById(user.UserGroupId)) ?? new UserGroup() { ShortName="", Name="" };
                U.NodeInfo = nodes.FirstOrDefault (g => g.UserGroupId == user.UserGroupId);
                
                groupRead.Add(user.UserGroupId);
                groupList.Add(U);
                if (user.UserGroupId != null)
                {
                    if (await aclService.GetAuthorization("", U.RoleId, ProfileType.Role, PermissionType.CanViewDown) == Domain.Enumerators.AuthorizationType.Granted)
                    {
                        // Aggiunge i gruppi sottostanti
                        var groupindex = nodes.FindIndex(g => g.UserGroupId == user.UserGroupId);
                        if (groupindex >= 0)
                        {
                            var thisgroup = nodes[groupindex];
                            while (groupindex > 0 && groupindex < nodes.Count - 1)
                            {
                                groupindex++;
                                var group = nodes[groupindex];
                                if (!groupRead.Contains(group.UserGroupId))
                                    if (group.LeftBound > thisgroup.LeftBound && group.RightBound < thisgroup.RightBound)
                                    {
                                        U = new UserInGroup(user);
                                        U.UserGroupId = group.UserGroupId;
                                        U.RoleName = roleName;
                                        U.UserGroup = await _userGroupRepository.GetById(group.UserGroupId);
                                        U.NodeInfo = group;
                                        groupRead.Add(group.UserGroupId);
                                        groupList.Add(U);
                                    }
                                    else groupindex = -1; //  <-- esco
                            }
                        }
                    }
                    if (await aclService.GetAuthorization("", U.RoleId, ProfileType.Role, PermissionType.CanViewUp) == Domain.Enumerators.AuthorizationType.Granted)
                    {
                        // Aggiunge i gruppi superiori
                        var groupindex = nodes.FindIndex(g => g.UserGroupId == user.UserGroupId);
                        if (groupindex >= 0)
                        {
                            var thisgroup = nodes[groupindex];
                            while (groupindex > 0)
                            {
                                groupindex--;
                                var group = nodes[groupindex];
                                if (!groupRead.Contains(group.UserGroupId))
                                    if (group.LeftBound < thisgroup.LeftBound && group.RightBound > thisgroup.RightBound)
                                    {
                                        U = new UserInGroup(user);
                                        U.UserGroupId = group.UserGroupId;
                                        U.RoleName = roleName;
                                        U.UserGroup = await _userGroupRepository.GetById(group.UserGroupId);
                                        U.NodeInfo = group;

                                        groupRead.Add(group.UserGroupId);
                                        groupList.Add(U);
                                    }
                                    else groupindex = -1; //  <-- esco
                            }
                        }
                    }
                    if (await aclService.GetAuthorization("", U.RoleId, ProfileType.Role, PermissionType.CanViewSide) == Domain.Enumerators.AuthorizationType.Granted)
                    {
                        // Aggiunge i gruppi fratelli
                        var groupindex = nodes.FindIndex(g => g.UserGroupId == user.UserGroupId);
                        if (groupindex >= 0)
                        {
                            var saveindex = groupindex;
                            var thisgroup = nodes[groupindex];
                            var lastgroup = thisgroup;
                            // prendo i gruppi a destra
                            while (groupindex >= 0 && groupindex < nodes.Count - 1)
                            {
                                groupindex++;
                                var group = nodes[groupindex];
                                if (!groupRead.Contains(group.UserGroupId))
                                    if (group.LeftBound == lastgroup.RightBound + 1)
                                    {
                                        U = new UserInGroup(user);
                                        U.UserGroupId = group.UserGroupId;
                                        U.RoleName = roleName;
                                        U.UserGroup = await _userGroupRepository.GetById(group.UserGroupId);
                                        U.NodeInfo = group;

                                        groupRead.Add(group.UserGroupId);
                                        groupList.Add(U);
                                        lastgroup = group;
                                    }
                                    else groupindex = -1; //  <-- esco
                            }
                            groupindex = saveindex;
                            // prendo i gruppi a sinistra
                            while (groupindex > 0)
                            {
                                groupindex--;
                                var group = nodes[groupindex];
                                if (!groupRead.Contains(group.UserGroupId))
                                    if (group.RightBound == lastgroup.LeftBound - 1)
                                    {
                                        U = new UserInGroup(user);
                                        U.UserGroupId = group.UserGroupId;
                                        U.RoleName = roleName;
                                        U.UserGroup = await _userGroupRepository.GetById(group.UserGroupId);
                                        U.NodeInfo = group;

                                        groupRead.Add(group.UserGroupId);
                                        groupList.Add(U);
                                        lastgroup = group;
                                    }
                                    else groupindex = -1; //  <-- esco
                            }
                        }
                    }
                }
            }
            return groupList;
        }

        public async Task<OrganizationNodeInfo> GetById(string userGroupId, int StartISODate = 0)
        {
            if (StartISODate < 0) StartISODate = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
            if (userGroupId == null) return null;
            var O = await _repository.GetById(StartISODate, userGroupId);
            if (O == null) return null;
            OrganizationNodeInfo I = new OrganizationNodeInfo(O);
            return I;
        }
        public async Task<OrganizationNodeInfo> GetByName(string shortName, int StartISODate = 0)
        {
            if (StartISODate < 0) StartISODate = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
            if (string.IsNullOrEmpty(shortName)) return null;
            var O = await _repository.GetByName(shortName, StartISODate);
            if (O == null) return null;
            OrganizationNodeInfo I = new OrganizationNodeInfo(O);
            return I;
        }

        public async Task<OrganizationNodeInfo> GetByExternalId(string externalId, int StartISODate = 0)
        {
            if (StartISODate < 0) StartISODate = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
            if (externalId == null) return null;
            var O = await _repository.GetByExternalId(externalId, StartISODate);
            if (O == null) return null;
            OrganizationNodeInfo I = new OrganizationNodeInfo(O);
            return I;
        }
        public async Task<List<UserInGroup>> GetUsers(string userGroupId, int StartISODate = 0)
        {
            if (StartISODate < 0) StartISODate = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
            if (userGroupId == null) return null;
            if (roles == null)
                roles = await _roleService.GetAll();
            var users = await _repository.GetUsersInGroup(StartISODate, userGroupId);
            List<UserInGroup> Userlist = new List<UserInGroup>();
            foreach (var user in users)
            {
                UserInGroup U = new UserInGroup(user);
                U.RoleName = roles.FirstOrDefault(r => r.Id == user.RoleId)?.RoleName;
                U.UserName = (await _userRepository.GetById(user.UserId))?.Contact.FriendlyName;
                Userlist.Add(U);
            }
            return Userlist;
        }

        public async Task<OrganizationNodeInfo> AddOrganizationNode(CreateOrUpdateOrganizationNode organization, string userId)
        {
            OrganizationNode O = new();
            O.UserGroup = new UserGroup();
            O.UserGroup.NotificationProfile = organization.NotificationProfile;
            O.UserGroup.NotificationStrategyCC = organization.NotificationStrategyCC;
            O.UserGroup.ShortName = organization.ShortName;
            O.UserGroup.Name = organization.Name;
            O.UserGroup.ExternalId = organization.ExternalId;
            O.UserGroup.NotificationStrategy = organization.NotificationStrategy;
            O.UserGroup.Visible = organization.Visible;
            O.UserGroup.CreationUser = userId;
            //O.UserGroup.Id = Guid.NewGuid().ToString();
            O.StartISODate = int.Parse(organization.StartDate.ToString("yyyyMMdd"));
            O.EndISODate = int.Parse(organization.EndDate.ToString("yyyyMMdd"));
            O.ParentUserGroupId = organization.ParentUserGroupId;
            O.TaskReallocationProfile = organization.TaskReallocationProfile;
            O.TaskReallocationStrategy = organization.TaskReallocationStrategy;
            //O.UserGroupId = O.UserGroup.Id;
            return (await _repository.Insert(O)) > 0 ? await GetById(O.UserGroupId, O.StartISODate) : null;
        }
        public async Task<OrganizationNodeInfo> UpdateOrganizationNode(string userGroupId, CreateOrUpdateOrganizationNode organization)
        {
            if (organization.EndDate < DateTime.UtcNow.Date) throw new InvalidDataException("Non è possibile modificare una configurazione del passato");

            var NewStartDate = int.Parse(organization.StartDate.ToString("yyyyMMdd"));
            var NewEndDate = int.Parse(organization.EndDate.ToString("yyyyMMdd"));

            OrganizationNode O = await _repository.GetById(NewStartDate, userGroupId);
            if (O == null) throw new ArgumentException(nameof(userGroupId));
            if (O.StartISODate != NewEndDate && organization.StartDate <= DateTime.UtcNow) throw new InvalidDataException("Non è possibile modificare una configurazione del passato");

            var users = await _repository.GetUsersInGroup(O.StartISODate, userGroupId);
            var newUsers = await _repository.GetUsersInGroup(NewStartDate, userGroupId);
            if (users.Count != newUsers.Count) throw new InvalidDataException(string.Format("La nuova data di Inizio Decorrenza si sovrappone ad un altra configurazione per lo stesso nodo"));

            users = await _repository.GetUsersInGroup(O.EndISODate, userGroupId);
            newUsers = await _repository.GetUsersInGroup(NewEndDate, userGroupId);
            if (users.Count != newUsers.Count) throw new InvalidDataException(string.Format("La nuova data di Fine Decorrenza si sovrappone ad un altra configurazione per lo stesso nodo"));

            O.UserGroup.NotificationProfile = organization.NotificationProfile;
            O.UserGroup.NotificationStrategyCC = organization.NotificationStrategyCC;
            O.UserGroup.ShortName = organization.ShortName;
            O.UserGroup.Name = organization.Name;
            O.UserGroup.ExternalId = organization.ExternalId;
            O.UserGroup.NotificationStrategy = organization.NotificationStrategy;
            O.UserGroup.Visible = organization.Visible;
            O.StartISODate = NewStartDate;
            O.EndISODate = NewEndDate;
            O.ParentUserGroupId = organization.ParentUserGroupId;
            O.TaskReallocationProfile = organization.TaskReallocationProfile;
            O.TaskReallocationStrategy = organization.TaskReallocationStrategy;
            return (await _repository.Update(O)) > 0 ? await GetById(O.UserGroupId, O.StartISODate) : null;
        }

        public async Task<int> DeleteOrganizationNode(string userGroupId, int StartISODate, DateTime EndDate)
        {
            //            if (StartDate <= DateTime.UtcNow) throw new InvalidDataException("Non è possibile eliminare una configurazione attiva o del passato");
            var NewStartDate = StartISODate; // int.Parse(StartDate.ToString("yyyyMMdd"));
            OrganizationNode O = await _repository.GetById(NewStartDate, userGroupId);
            if (O == null) throw new ArgumentException(nameof(userGroupId));
            if (O.StartISODate < int.Parse(DateTime.UtcNow.ToString("yyyyMMdd")))
            {
                var NewEndISODate = int.Parse(EndDate.ToString("yyyyMMdd"));
                O.EndISODate = NewEndISODate;
                return await _repository.Update(O);
            }
            else
                return (await _repository.Delete(O));
        }

        public async Task<OrganizationNodeInfo> MoveOrganizationNode(MoveOrganizationNode organization)
        {
            var NewStartDate = int.Parse(organization.StartDate.ToString("yyyyMMdd"));
            var NewEndDate = int.Parse(organization.EndDate.ToString("yyyyMMdd"));
            OrganizationNode O = await _repository.GetById(NewStartDate, organization.userGroupId);
            if (O != null) throw new InvalidDataException(string.Format("La nuova data di Inizio Decorrenza si sovrappone ad un altra configurazione per lo stesso nodo"));

            var newUsers = await _repository.GetUsersInGroup(NewStartDate, organization.userGroupId);
            if (newUsers.Count > 0) throw new InvalidDataException(string.Format("La nuova data di Inizio Decorrenza si sovrappone ad un altra configurazione per lo stesso nodo"));

            newUsers = await _repository.GetUsersInGroup(NewEndDate, organization.userGroupId);
            if (newUsers.Count > 0) throw new InvalidDataException(string.Format("La nuova data di Fine Decorrenza si sovrappone ad un altra configurazione per lo stesso nodo"));



            O.EndISODate = int.Parse(organization.StartDate.AddDays(-1).ToString("yyyyMMdd"));
            OrganizationNode N = new OrganizationNode();
            N.ParentUserGroupId = organization.NewParentUserGroupId;
            N.StartISODate = NewStartDate;
            N.EndISODate = NewEndDate;
            N.ParentUserGroupId = organization.NewParentUserGroupId;
            N.TaskReallocationProfile = organization.TaskReallocationProfile;
            N.TaskReallocationStrategy = organization.TaskReallocationStrategy;
            if (await _repository.Move(O, N) > 0)
                return await GetById(N.UserGroupId, N.StartISODate);
            return null;
        }


        public async Task<int> AddUser(UserInGroup userInGroup)
        {
            //if (String.IsNullOrEmpty(userInGroup.UserGroupId)) throw new ArgumentNullException(nameof(userInGroup.UserGroupId));
            if (String.IsNullOrEmpty(userInGroup.RoleId)) throw new ArgumentNullException(nameof(userInGroup.RoleId));
            if (String.IsNullOrEmpty(userInGroup.UserId)) throw new ArgumentNullException(nameof(userInGroup.UserId));
            var utcdate = DateTime.UtcNow;
            int data = int.Parse(utcdate.ToString("yyyyMMdd"));
            if (userInGroup.StartDate.HasValue) utcdate = userInGroup.StartDate.Value;
            if (!userInGroup.EndDate.HasValue) userInGroup.EndDate = DateTime.MaxValue;
            if ((await _userRepository.GetById(userInGroup.UserId)) is null) throw new ArgumentException(nameof(userInGroup.UserId));
            if ((await _roleService.GetById(userInGroup.RoleId)) is null) throw new ArgumentException(nameof(userInGroup.RoleId));
            
            if (userInGroup.UserGroupId != null && (await _repository.GetById(data, userInGroup.UserGroupId)) is null) throw new ArgumentException(nameof(userInGroup.UserGroupId));
            //if (userInGroup.StartDate == null) throw new ArgumentNullException(nameof(userInGroup.StartDate));
            //if (userInGroup.EndDate == null) throw new ArgumentNullException(nameof(userInGroup.EndDate));

            UserGroupRole UR = new UserGroupRole();
            UR.UserGroupId = userInGroup.UserGroupId;
            UR.UserId = userInGroup.UserId;
            UR.RoleId = userInGroup.RoleId;
            UR.StartISODate = int.Parse(userInGroup.StartDate.Value.ToString("yyyyMMdd"));
            UR.EndISODate = int.Parse(userInGroup.EndDate.Value.ToString("yyyyMMdd"));
            return await _repository.AddUser(UR);
        }

        public async Task<int> EditUser(UserInGroup userInGroup)
        {
            if (String.IsNullOrEmpty(userInGroup.UserGroupId)) throw new ArgumentNullException(nameof(userInGroup.UserGroupId));
            if (String.IsNullOrEmpty(userInGroup.RoleId)) throw new ArgumentNullException(nameof(userInGroup.RoleId));
            if (String.IsNullOrEmpty(userInGroup.UserId)) throw new ArgumentNullException(nameof(userInGroup.UserId));

            if ((await _userRepository.GetById(userInGroup.UserId)) is null) throw new ArgumentException(nameof(userInGroup.UserId));
            if ((await _roleService.GetById(userInGroup.RoleId)) is null) throw new ArgumentException(nameof(userInGroup.RoleId));
            if ((await _repository.GetById(int.Parse(userInGroup.StartDate.Value.ToString("yyyyMMdd")), userInGroup.UserGroupId)) is null) throw new ArgumentException(nameof(userInGroup.UserGroupId));

            if (userInGroup.StartDate == null) throw new ArgumentNullException(nameof(userInGroup.StartDate));
            if (userInGroup.EndDate == null) throw new ArgumentNullException(nameof(userInGroup.EndDate));

            UserGroupRole UR = await _repository.GetUser(userInGroup.UserGroupId, userInGroup.UserId);
            if (UR is null) throw new ArgumentException(nameof(userInGroup.UserGroupId));

            UR.UserGroupId = userInGroup.UserGroupId;
            UR.UserId = userInGroup.UserId;
            UR.RoleId = userInGroup.RoleId;
            UR.StartISODate = int.Parse(userInGroup.StartDate.Value.ToString("yyyyMMdd"));
            UR.EndISODate = int.Parse(userInGroup.EndDate.Value.ToString("yyyyMMdd"));
            return await _repository.UpdateUser(UR);
        }

        public async Task<int> RemoveUser(UserInGroup userInGroup)
        {
            if (String.IsNullOrEmpty(userInGroup.UserGroupId)) throw new ArgumentNullException(nameof(userInGroup.UserGroupId));
            if (String.IsNullOrEmpty(userInGroup.RoleId)) throw new ArgumentNullException(nameof(userInGroup.RoleId));
            if (String.IsNullOrEmpty(userInGroup.UserId)) throw new ArgumentNullException(nameof(userInGroup.UserId));

            if ((await _userRepository.GetById(userInGroup.UserId)) is null) throw new ArgumentException(nameof(userInGroup.UserId));
            if ((await _roleService.GetById(userInGroup.RoleId)) is null) throw new ArgumentException(nameof(userInGroup.RoleId));
            if ((await _repository.GetById(int.Parse(userInGroup.StartDate.Value.ToString("yyyyMMdd")), userInGroup.UserGroupId)) is null) throw new ArgumentException(nameof(userInGroup.UserGroupId));

            if (userInGroup.StartDate == null) throw new ArgumentNullException(nameof(userInGroup.StartDate));
            if (userInGroup.EndDate == null) throw new ArgumentNullException(nameof(userInGroup.EndDate));

            // Se la data di inizio è antecedente ad oggi non è possibile cancellare un utente
            //if (userInGroup.EndDate.Value < DateTime.UtcNow.Date) return 0;

            UserGroupRole UR = await _repository.GetUser(int.Parse(userInGroup.StartDate.Value.ToString("yyyyMMdd")), userInGroup.UserGroupId, userInGroup.UserId);
            if (UR is null) throw new ArgumentException(nameof(userInGroup.UserGroupId));

            UR.UserGroupId = userInGroup.UserGroupId;
            UR.UserId = userInGroup.UserId;
            UR.RoleId = userInGroup.RoleId;
            UR.StartISODate = int.Parse(userInGroup.StartDate.Value.ToString("yyyyMMdd"));
            UR.EndISODate = int.Parse(userInGroup.EndDate.Value.ToString("yyyyMMdd"));
            if (userInGroup.StartDate.Value < DateTime.UtcNow.Date)
                return await _repository.UpdateUser(UR);
            else
                return await _repository.RemoveUser(UR);
        }


    }
}
