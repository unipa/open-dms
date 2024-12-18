using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.OrganizationUnits;
using OpenDMS.Domain.Models;
using Web.BL.Interface;
using Web.Model.Admin;

namespace Web.BL
{
    public class OrganigrammaBL : IOrganigrammaBL
    {
        private readonly IConfiguration _config;
        private readonly string Host;
        private readonly IOrganizationUnitService service;
        private readonly ILoggedUserProfile userContext;

        public OrganigrammaBL(IConfiguration config, IOrganizationUnitService service, ILoggedUserProfile userContext)
        {
            _config = config;
            Host = (string)_config.GetValue(typeof(string), "Endpoint:AdminService");
            this.service = service;
            this.userContext = userContext;
        }


        public async Task<List<UserGroupRole>> GetAllByRole(string roleId, string filter)
        {
            UserFilter f = new UserFilter() { roleId = roleId, filter = filter };
            List<UserGroupRole> users = await service.GetUsersByFilter(f);
            return users;
        }



        public async Task<IEnumerable<OrganizationNodeTree>> GetOrganizationTree(int StartISODate = 0)
        {
            return await service.GetOrganizationTree(StartISODate);
        }

        private async Task<OrganizationNodeInfo> GetById(string userGroupId, int StartISODate = 0)
        {
            return await service.GetById(userGroupId, StartISODate);
        }

        private async Task<UserInGroup> GetUserById(string userGroupId, int Id, int StartISODate = 0)
        {
            var users = await service.GetUsers(userGroupId, StartISODate);
            return users.FirstOrDefault(u => u.Id == Id && u.StartDate != null);
        }

        public async Task<OrganizationNodeInfo> MoveOrganizationNode(MoveOrganizationNode_DTO bd)
        {
            //controlli
            if (bd.EndDate == null) bd.EndDate = DateTime.MaxValue;
            if ((bd.TaskReallocationStrategy == 2 || bd.TaskReallocationStrategy == 3) && string.IsNullOrEmpty(bd.TaskReallocationProfile))
                throw new Exception("Nel caso di riallocazione task ad un gruppo diverso dal gruppo padre è necessario specificare un gruppo destinatario o un ruolo destinatario.");

            if (bd.StartDate > bd.EndDate) throw new Exception("La Data di fine non può essere precedente della data di inizio.");
            //if (bd.StartDate < DateTime.UtcNow || bd.EndDate < DateTime.UtcNow) throw new Exception("Una o entrambe le date di decorrenza sono minori della data attuale.");

            var config = new MapperConfiguration(cfg => cfg.CreateMap<MoveOrganizationNode_DTO, MoveOrganizationNode>());
            Mapper mp = new Mapper(config);
            var organizationNode = mp.Map<MoveOrganizationNode>(bd);

            return await service.MoveOrganizationNode(organizationNode);
        }

        public async Task<OrganizationNodeInfo> UpdateOrganizationNode(string userGroupId, CreateOrUpdateOrganizationNode_DTO bd)
        {

            var node = await GetById(userGroupId);
            
            node = new OrganizationNodeInfo(); // ?????????????????????????!!!!!!!!!!

            //if (node == null) throw new Exception("Nodo non trovato.");

            //controlli
            if (!string.IsNullOrEmpty(bd.ExternalId)) throw new Exception("Non è possibile modificare un nodo con un ExternalId assegnato.");

            if ((bd.TaskReallocationStrategy == 2 || bd.TaskReallocationStrategy == 3) && string.IsNullOrEmpty(bd.TaskReallocationProfile))
                throw new Exception("Nel caso di riallocazione task ad un gruppo diverso dal gruppo padre è necessario specificare un gruppo destinatario o un ruolo destinatario.");

            if (!(bd.StartDate == DateTime.MinValue) || !(bd.EndDate == DateTime.MinValue))
            {
                if (bd.StartDate > bd.EndDate) throw new Exception("La Data di fine non può essere precedente della data di inizio.");
                if ((bd.StartDate < DateTime.UtcNow || bd.EndDate < DateTime.UtcNow) && bd.StartDate == DateTime.MinValue && bd.EndDate == DateTime.MinValue)
                    throw new Exception("Una o entrambe le date di decorrenza sono minori della data attuale.");
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<OrganizationNodeInfo, CreateOrUpdateOrganizationNode>());
            Mapper mp2 = new Mapper(config);
            var new_node = mp2.Map<CreateOrUpdateOrganizationNode>(node);

            new_node.ShortName = bd.ShortName;
            new_node.Name = bd.Name;
            new_node.StartDate = bd.StartDate == DateTime.MinValue ? new_node.StartDate : bd.StartDate;
            new_node.EndDate = bd.EndDate == DateTime.MinValue ? new_node.EndDate : bd.EndDate;
            new_node.Visible = bd.Visible;
            new_node.TaskReallocationStrategy = bd.TaskReallocationStrategy;
            new_node.TaskReallocationProfile = bd.TaskReallocationProfile;
            new_node.ExternalId = bd.ExternalId;
            new_node.NotificationStrategy = bd.NotificationStrategy;
            new_node.NotificationProfile = bd.NotificationProfile;
            //new_node.NotificationStrategyCC = bd.NotificationStrategyCC;

            return await service.UpdateOrganizationNode(userGroupId, new_node);

        }

        public async Task<OrganizationNodeInfo> CreateOrganizationNode(CreateOrUpdateOrganizationNode_DTO bd)
        {

            if ((bd.TaskReallocationStrategy == 2 || bd.TaskReallocationStrategy == 3) && string.IsNullOrEmpty(bd.TaskReallocationProfile))
                throw new Exception("Nel caso di riallocazione task ad un gruppo diverso dal gruppo padre è necessario specificare un gruppo destinatario o un ruolo destinatario.");

            if (bd.StartDate > bd.EndDate) throw new Exception("La Data di fine non può essere precedente della data di inizio.");
            if (bd.EndDate < DateTime.UtcNow && bd.StartDate != DateTime.MinValue && bd.EndDate != DateTime.MinValue)
                throw new Exception("La data di fine è minore della data attuale.");

            if (string.IsNullOrEmpty(bd.ParentUserGroupId)) bd.ParentUserGroupId = null;

            bd.ExternalId = null;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<CreateOrUpdateOrganizationNode_DTO, CreateOrUpdateOrganizationNode>());
            Mapper mp2 = new Mapper(config);
            var new_node = mp2.Map<CreateOrUpdateOrganizationNode>(bd);

            return await service.AddOrganizationNode(new_node, userContext.Get().userId);

        }

        public async Task<int> RemoveOrganizationNode(string userGroupId, DateTime EndDate, int StartISODate)
        {
            return await service.DeleteOrganizationNode(userGroupId, StartISODate, EndDate);
        }

        public async Task RemoveUser(UserInGroup_DTO bd)
        {
            //controlli
            var node = await GetById(bd.UserGroupId);

            if (node == null) throw new Exception("Nodo non trovato.");

            //if (!string.IsNullOrEmpty(node.ExternalId)) throw new Exception("Non è possibile modificare un nodo con un ExternalId assegnato.");

            var user = await GetUserById(bd.UserGroupId, bd.Id, 0 /*startISODate*/);

            if (user == null) throw new Exception("User non trovato.");

            bd.StartDate = user.StartDate ?? DateTime.Today;
            //(await service.GetUsers(node.UserGroupId)).FirstOrDefault(u=>u.UserId == bd.UserId && u.RoleId == bd.RoleId).
            if (!bd.EndDate.HasValue && bd.EndDate > DateTime.UtcNow || (bd.EndDate < bd.StartDate)) throw new Exception("Non è stata inserita una data valida.");

            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserInGroup_DTO, UserInGroup>());
            Mapper mp2 = new Mapper(config);
            var userInGroup = mp2.Map<UserInGroup>(bd);

            var inserted = await service.RemoveUser(userInGroup);
            if (inserted == 0) throw new Exception("La data di inizio è antecedente ad oggi, non è possibile cancellare l'utente");
        }

        public async Task ChangeRoleUser(UserInGroup_DTO bd)
        {
            //controlli
            var node = await GetById(bd.UserGroupId);

            if (node == null) throw new Exception("Nodo non trovato.");

            //if (!string.IsNullOrEmpty(node.ExternalId)) throw new Exception("Non è possibile modificare un nodo con un ExternalId assegnato.");

            var user = await GetUserById(bd.UserGroupId, bd.Id);

            if (user == null) throw new Exception("User non trovato.");

            bd.StartDate = user.StartDate > DateTime.UtcNow ? bd.StartDate : user.StartDate;
            bd.EndDate = user.EndDate;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserInGroup_DTO, UserInGroup>());
            Mapper mp2 = new Mapper(config);
            var userInGroup = mp2.Map<UserInGroup>(bd);

            await service.EditUser(userInGroup);
        }

        public async Task AddUser(UserInGroup_DTO bd)
        {
            //controlli 
            if (bd.EndDate == null) bd.EndDate = DateTime.MaxValue;
            if (!bd.StartDate.HasValue) throw new Exception("La data di inizio non è stata inserita.");
            if (bd.StartDate > bd.EndDate) throw new Exception("La data di fine non può essere precedente della data di inizio.");

            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserInGroup_DTO, UserInGroup>());
            Mapper mp2 = new Mapper(config);
            var userInGroup = mp2.Map<UserInGroup>(bd);

            await service.AddUser(userInGroup);
        }

    }
}
