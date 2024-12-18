using Microsoft.EntityFrameworkCore;
using A3Synch.Interfacce;
using A3Synch.Models;
using OpenDMS.A3Synch.API.Utility;
using OpenDMS.Domain.Entities;

namespace A3Synch.DAO
{
    public class UserGroupRolesDAO : IUserGroupRolesDAO
    {
        private readonly IContext _ctx;
        private readonly ILogger<UserGroupRolesDAO> _logger;

        public UserGroupRolesDAO(IContext ctx, ILogger<UserGroupRolesDAO> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public async Task<int> UpdateUserGroupRoles(List<UserGroupRoles> userGroupsRolesList)
        {
            try
            {
                foreach (var userGroupRole in userGroupsRolesList)
                {
                    // Controlla se esiste un record con lo stesso UserGroupId,RoleId,StartISODate e UserId nel database
                    var member = await _ctx.UserGroupRoles
                                .FirstOrDefaultAsync
                                (ug =>
                                   ug.UserGroupId == userGroupRole.UserGroupId &&
                                   ug.RoleId == userGroupRole.RoleId &&
                                   ug.StartISODate == userGroupRole.StartISODate &&
                                   ug.UserId == userGroupRole.UserId
                                 );

                    // Inserisci l'oggetto se il record non esiste già
                    if (member == null)
                    {
                        Console.WriteLine("UserGroupsRole: " + userGroupRole.RoleId);
                        _ctx.UserGroupRoles.Add(userGroupRole);
                        await _ctx.SaveChangesAsync();
                    }
                    else
                    {
                        member.StartISODate = userGroupRole.StartISODate;
                        member.EndISODate = userGroupRole.EndISODate;
                    }
                    SharedVariables.elaborated_usergroupsroles_counter++;
                }
                await _ctx.SaveChangesAsync();                

                return 1;
            }
            catch (Exception ex)
            {

                _logger.LogError("Errore in UserGroupsRolesDAO, errore: " + ex.Message);
                throw new Exception("Errore in UserGroupsRolesDAO, errore: " + ex.Message);
            }
            finally
            {
                foreach (var userGroupRole in userGroupsRolesList)
                {
                    _ctx.Entry(userGroupRole).State = EntityState.Detached;
                }
            }

        }

        public async Task<List<UserGroupRoles>> GetAllUserGroupsRoles()
        {
            return await _ctx.UserGroupRoles.ToListAsync();
        }

        public async Task<List<UserGroupRoles>> GetUserGroupsRoles(string UserId)
        {
            try
            {
                List<UserGroupRoles> group = await _ctx.UserGroupRoles.Where(ugr => ugr.UserId == UserId).ToListAsync();
                return group;
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore in UserGroupRolesDAO, errore: " + ex.Message);
                throw new Exception("Errore in UserGroupRolesDAO, errore: " + ex.Message);
            }
        }
    }
}
