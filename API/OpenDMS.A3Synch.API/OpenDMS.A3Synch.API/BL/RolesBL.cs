using A3Synch.DAO;
using A3Synch.Interfacce;
using A3Synch.Models;
using A3Synch.Utility;
using OpenDMS.A3Synch.API.Utility;

namespace A3Synch.BL
{
    public class RolesBL : IRolesBL
    {
        private readonly IUserGroupsDAO _usergroupsdao;
        private readonly IUserGroupRolesDAO _usergroupsrolesdao;
        private readonly IRolesDAO _rolesdao;
        private IConfiguration _config;
        private readonly IUtils _utils;
        

        public RolesBL(IUserGroupsDAO usergroupdao, IConfiguration config, IUtils utils, IUserGroupRolesDAO usergroupsrolesdao, IRolesDAO rolesdao)
        {
            _usergroupsdao = usergroupdao;
            _usergroupsrolesdao = usergroupsrolesdao;
            _rolesdao = rolesdao;
            _config = config;
            _utils = utils;
        }

        public async Task<int> SynchRolesInDb(List<Members> all_members)
        {
            List<Roles> roles = all_members.DistinctBy(r=>r.id_ruolo).Select(member => new Roles()
            {
                Id = member.id_ruolo,
                RoleName = member.denominazione,
                ExternalApp = Utils.A3Synch
            }).ToList();

            int res = await _rolesdao.UpdateRoles(roles);
            return res;
        }

        public void ResetStatus()
        {
            SharedVariables.total_roles_counter = 0;
            SharedVariables.elaborated_roles_counter = 0;
        }





    }
}
