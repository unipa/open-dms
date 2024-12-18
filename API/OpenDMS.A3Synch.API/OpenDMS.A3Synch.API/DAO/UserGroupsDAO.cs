using Microsoft.EntityFrameworkCore;
using A3Synch.Interfacce;
using A3Synch.Models;
using OpenDMS.A3Synch.API.Utility;
using A3Synch.Utility;

namespace A3Synch.DAO
{
    public class UserGroupsDAO : IUserGroupsDAO
    {
        private readonly IContext _ctx;
        private readonly ILogger<UserGroupsDAO> _logger;

        public UserGroupsDAO(IContext ctx, ILogger<UserGroupsDAO> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public async Task<int> UpdateUserGroups(List<UserGroups> userGroupsList)
        {
            try
            {
                SharedVariables.total_groups_counter = userGroupsList.Count;
                foreach (var userGroup in userGroupsList)
                {
                    // Controlla se esiste un record con lo stesso ExternalId nel database
                    var group = await _ctx.UserGroups
                        .FirstOrDefaultAsync(ug => ug.ExternalId == userGroup.ExternalId);

                    // Aggiorna i campi dell'oggetto se il record esiste già
                    if (group != null)
                    {
                        group.Name = userGroup.Name;
                        group.ShortName = userGroup.ShortName;
                        group.CreationDate = userGroup.CreationDate;
                        group.ClosingDate = userGroup.ClosingDate;
                    }
                    // Inseriscilo se non esiste
                    else
                    {
                        _ctx.UserGroups.Add(userGroup);
                    }
                    SharedVariables.elaborated_groups_counter++;
                }
                await _ctx.SaveChangesAsync();

                // Detach tutti gli oggetti
                foreach (var userGroup in userGroupsList)
                {
                    _ctx.Entry(userGroup).State = EntityState.Detached;
                }

                return 1;
            }
            catch(Exception ex)
            {
                _logger.LogError("Errore in UserGroupsDAO, errore: " + ex.Message);
                throw new Exception("Errore in UserGroupsDAO, errore: " + ex.Message);
            }
            finally
            {
                foreach (var userGroupRole in userGroupsList)
                {
                    _ctx.Entry(userGroupRole).State = EntityState.Detached;
                }
            }

        }

        public async Task<List<UserGroups>> GetAllUserGroups()
        {
            try {
                return await _ctx.UserGroups.AsNoTracking().Where(u => !string.IsNullOrEmpty(u.ExternalId) && u.ExternalApp == Utils.A3Synch)
                    .Select(u => new UserGroups
                    {
                        Id = u.Id,
                        Name = u.Name ?? "",
                        ShortName = u.ShortName ?? "",
                        ClosingUser = u.ClosingUser ?? "",
                        CreationUser = u.CreationUser ?? "",
                        ExternalId = u.ExternalId ?? ""
                    })
                    .ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError("Errore in UserGroupsDAO, errore: " + ex.Message);
                throw new Exception("Errore in UserGroupsDAO, errore: " + ex.Message);
            }
        }

        public async Task<List<string>> GetAllExternalIds()
        {
            try
            {
                return await _ctx.UserGroups.AsNoTracking()
                    .Where(u => !string.IsNullOrEmpty(u.ExternalId) && u.ExternalApp == Utils.A3Synch)
                    .Select(u => u.ExternalId) // Restituisci il valore di ExternalId
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore in UserGroupsDAO, errore: " + ex.Message);
                throw new Exception("Errore in UserGroupsDAO, errore: " + ex.Message);
            }
        }

        public async Task<UserGroups> GetUserGroupsById(string UserGroupsId)
        {
            try
            {
                return await _ctx.UserGroups
                    .AsNoTracking()
                    .Where(u => u.Id == UserGroupsId)
                    .Select(u => new UserGroups
                    {
                        Id = u.Id,
                        Name = u.Name ?? "",
                        ShortName = u.ShortName ?? "",
                        ClosingUser = u.ClosingUser ?? "",
                        CreationUser = u.CreationUser ?? "",
                        ExternalId = u.ExternalId ?? "",
                        ExternalApp = u.ExternalApp ?? ""
                    })
                    .FirstOrDefaultAsync(); // Usa FirstOrDefaultAsync per ottenere un singolo oggetto
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore in UserGroupsDAO, errore: " + ex.Message);
                throw new Exception("Errore in UserGroupsDAO, errore: " + ex.Message);
            }
        }

        public string GetUserGroupsId(string externalId)
        {
            try
            {
                var node = _ctx.UserGroups.AsNoTracking().FirstOrDefault(node => node.ExternalId == externalId && node.ExternalApp== Utils.A3Synch);
                return node?.Id ?? null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore in UserGroupsDAO, errore: " + ex.Message);
                throw new Exception("Errore in UserGroupsDAO, errore: " + ex.Message);
            } 
        }

        public string GetExternalId(string id)
        {
            try
            {
                var node = _ctx.UserGroups.AsNoTracking().FirstOrDefault(node => node.Id == id);
                return node?.ExternalId ?? null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore in UserGroupsDAO, errore: " + ex.Message);
                throw new Exception("Errore in UserGroupsDAO, errore: " + ex.Message);
            }
        }

		public DateTime? GetCreationDate(string externalId)
		{
			try
			{
				var node = _ctx.UserGroups.AsNoTracking().FirstOrDefault(node => node.ExternalId == externalId);
				return node?.CreationDate ?? null;
			}
			catch (Exception ex)
			{
				_logger.LogError("Errore in UserGroupsDAO, errore: " + ex.Message);
				throw new Exception("Errore in UserGroupsDAO, errore: " + ex.Message);
			}
		}

		public DateTime? GetClosingDate(string externalId)
		{
			try
			{
				var node = _ctx.UserGroups.AsNoTracking().FirstOrDefault(node => node.ExternalId == externalId);
				return node?.ClosingDate ?? null;
			}
			catch (Exception ex)
			{
				_logger.LogError("Errore in UserGroupsDAO, errore: " + ex.Message);
				throw new Exception("Errore in UserGroupsDAO, errore: " + ex.Message);
			}
		}
	}
}
