using A3Synch.DAO;
using A3Synch.Interfacce;
using A3Synch.Models;
using A3Synch.Utility;
using AutoMapper.Execution;
using Newtonsoft.Json.Linq;
using OpenDMS.A3Synch.API.Utility;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace A3Synch.BL
{
    public class UserGroupRolesBL : IUserGroupRolesBL
    {
        private readonly IUserGroupsDAO _usergroupsdao;
        private readonly IUserGroupRolesDAO _usergroupsrolesdao;
        private readonly IRolesDAO _rolesdao;
        private IConfiguration _config;
        private readonly IUtils _utils;
        

        public UserGroupRolesBL(IUserGroupsDAO usergroupdao, IConfiguration config, IUtils utils, IUserGroupRolesDAO usergroupsrolesdao, IRolesDAO rolesdao)
        {
            _usergroupsdao = usergroupdao;
            _usergroupsrolesdao = usergroupsrolesdao;
            _config = config;
            _utils = utils;
            _rolesdao = rolesdao;
        }

        //VECCHIO CODICE
        //public async Task<int> SynchUserGroupsRolesInDb(List<Members> all_members)
        //{
        //    // Rimuovi gli utenti duplicati in base a UserId e RoleId
        //    //all_members = all_members.DistinctBy(x => new { x.username, x.id_ruolo }).ToList();
        //    List<UserGroupRoles> userGroupsRoles = new List<UserGroupRoles>();
        //    foreach (var member in all_members)
        //    {
        //        UserGroupRoles user = new UserGroupRoles();
        //        user.UserGroupId = member.UserGroupId;
        //        user.RoleId = member.id_ruolo;
        //        user.UserId = member.username;
        //        user.StartISODate = _utils.ISOnumericDate(member.inizio_validita);
        //        user.EndISODate = _utils.ISOnumericDate(member.fine_validita);
        //        userGroupsRoles.Add(user);
        //    }
        //    int res = await _usergroupsrolesdao.UpdateUserGroupRoles(userGroupsRoles);
        //    return res;
        //}

        //NUOVO CODICE
        public async Task<int> SynchUserGroupsRolesInDb(List<Members> all_members)
        {
            SharedVariables.total_usergroupsroles_counter = all_members.Count;

            // Raggruppa gli utenti per UserId (username)
            var groupedMembers = all_members.GroupBy(x => x.username);
            var allRoles = await _rolesdao.GetAllRoles();
            int totalChanges = 0;

            foreach (var group in groupedMembers)
            {
                string userId = group.Key;
                var incomingUserRoles = group.ToList();

                // Recupera i ruoli dal database per questo UserId
                List<UserGroupRoles> dbUserRoles = await _usergroupsrolesdao.GetUserGroupsRoles(userId);

                // Crea una lista di UserGroupRoles in arrivo dalla lista di input
                List<UserGroupRoles> incomingUserGroupRoles = incomingUserRoles.Select(member => new UserGroupRoles
                {
                    UserGroupId = member.UserGroupId,
                    RoleId = member.id_ruolo,
                    UserId = member.username,
                    StartISODate = _utils.ISOnumericDate(member.inizio_validita),
                    EndISODate = _utils.ISOnumericDate(member.fine_validita)
                }).ToList();

                // Identifica i ruoli da mantenere: presenti sia nel database che in ingresso
                var rolesToKeep = dbUserRoles.Where(dbRole =>
                    incomingUserGroupRoles.Any(inRole =>
                        inRole.RoleId == dbRole.RoleId &&
                        inRole.UserId == dbRole.UserId &&
                        inRole.StartISODate == dbRole.StartISODate &&
                        inRole.EndISODate == dbRole.EndISODate)).ToList();

                // Identifica i ruoli da "scadere": presenti nel DB ma non nella lista in ingresso
                var rolesToExpire = dbUserRoles.Where(dbRole =>
                    !incomingUserGroupRoles.Any(inRole =>
                        inRole.RoleId == dbRole.RoleId &&
                        inRole.UserId == dbRole.UserId)).ToList();

                // Identifica i ruoli da "scadere" il giorno prima dell'inizio di quelli in entrata: presenti nel DB e nella lista in ingresso, ma quelli presenti nella lista hanno una data di inizio diversa
                var rolesToExpireBecauseUpdated = dbUserRoles
                    .Where(dbRole => incomingUserGroupRoles.Any(inRole =>
                        inRole.RoleId == dbRole.RoleId &&
                        inRole.UserId == dbRole.UserId &&
                        inRole.StartISODate != dbRole.StartISODate))
                    .ToList();

                // Identifica i nuovi ruoli da aggiungere: presenti in ingresso ma non nel database
                var rolesToAdd = incomingUserGroupRoles.Where(inRole =>
                    !dbUserRoles.Any(dbRole =>
                        dbRole.RoleId == inRole.RoleId &&
                        dbRole.UserId == inRole.UserId &&
                        dbRole.StartISODate == inRole.StartISODate &&
                        dbRole.EndISODate == inRole.EndISODate)).ToList();

                // Esegui le operazioni sul database
                if (rolesToExpire.Any())
                {
                    foreach (var roleToExpire in rolesToExpire)
                    {
                        // Recupera il UserGroup associato
                        var userGroup = await _usergroupsdao.GetUserGroupsById(roleToExpire.UserGroupId);
                        var role = allRoles.FirstOrDefault(r => r.Id == roleToExpire.RoleId);

                        // Se il valore di ExternalApp sia dello UserGroup che del Role è 'A3Synch', aggiorna la EndISODate
                        if (userGroup != null && userGroup.ExternalApp == Utils.A3Synch && role != null && role.ExternalApp == Utils.A3Synch)
                        {
                            // Converte l'attuale EndISODate in una data per il confronto
                            DateTime currentEndISODate = _utils.ISOnumericToDate(roleToExpire.EndISODate.ToString());

                            // Aggiorna la EndISODate solo se non è già scaduta (cioè se è maggiore di oggi)
                            if (currentEndISODate > DateTime.UtcNow)
                            {
                                // Imposta la EndISODate a ieri
                                roleToExpire.EndISODate = _utils.ISOnumericDate(DateTime.UtcNow.AddDays(-1));
                                totalChanges += await _usergroupsrolesdao.UpdateUserGroupRoles(new List<UserGroupRoles> { roleToExpire });
                            }
                        }
                    }
                }

                if (rolesToExpireBecauseUpdated.Any())
                {
                    foreach (var roleToExpireBecauseUpdated in rolesToExpireBecauseUpdated)
                    {
                        // Recupera il UserGroup associato
                        var userGroup = await _usergroupsdao.GetUserGroupsById(roleToExpireBecauseUpdated.UserGroupId);
                        var role = allRoles.FirstOrDefault(r => r.Id == roleToExpireBecauseUpdated.RoleId);

                        // Se il valore di ExternalApp sia dello UserGroup che del Role è 'A3Synch', aggiorna la EndISODate
                        if (userGroup != null && userGroup.ExternalApp == Utils.A3Synch && role != null && role.ExternalApp == Utils.A3Synch)
                        {
                            // Recupera il ruolo attualmente presente nel database
                            var incomingRole = incomingUserGroupRoles.FirstOrDefault(incomingRole =>
                                incomingRole.RoleId == roleToExpireBecauseUpdated.RoleId &&
                                incomingRole.UserId == roleToExpireBecauseUpdated.UserId &&
                                incomingRole.StartISODate != roleToExpireBecauseUpdated.StartISODate);

                            if (incomingRole != null)
                            {
                                // Converte la StartISODate del ruolo esistente in una data
                                DateTime startISODateIncoming = _utils.ISOnumericToDate(incomingRole.StartISODate.ToString());

                                // Imposta la EndISODate a StartISODate - 1 giorno
                                roleToExpireBecauseUpdated.EndISODate = _utils.ISOnumericDate(startISODateIncoming.AddDays(-1));

                                // Salva i cambiamenti
                                totalChanges += await _usergroupsrolesdao.UpdateUserGroupRoles(new List<UserGroupRoles> { roleToExpireBecauseUpdated });
                            }
                        }
                    }
                }

                if (rolesToAdd.Any())
                {
                    totalChanges += await _usergroupsrolesdao.UpdateUserGroupRoles(rolesToAdd);
                }

                // Aggiorna i ruoli che devono essere mantenuti
                if (rolesToKeep.Any())
                {
                    totalChanges += await _usergroupsrolesdao.UpdateUserGroupRoles(rolesToKeep);
                }
            }

            return totalChanges;
        }


        public void ResetStatus()
        {
            SharedVariables.elaborated_usergroupsroles_counter = 0;
            SharedVariables.total_usergroupsroles_counter = 0;
        }



    }
}
