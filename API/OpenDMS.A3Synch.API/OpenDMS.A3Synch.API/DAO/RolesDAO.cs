using Microsoft.EntityFrameworkCore;
using A3Synch.Interfacce;
using A3Synch.Models;
using OpenDMS.A3Synch.API.Utility;

namespace A3Synch.DAO
{
    public class RolesDAO : IRolesDAO
    {
        private readonly IContext _ctx;
        private readonly ILogger<RolesDAO> _logger;

        public RolesDAO(IContext ctx, ILogger<RolesDAO> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public async Task<int> UpdateRoles(List<Roles> roles)
        {
            try
            {
                SharedVariables.total_roles_counter = roles.Count;
                foreach (var role in roles)
                {
                    // Controlla se esiste un record con lo stesso id nel database
                    var existingRole = await _ctx.Roles.FindAsync(role.Id);
                    if (existingRole != null)
                    {
                        // Aggiorna i campi dell'oggetto esistente
                        existingRole.RoleName = role.RoleName;
                        existingRole.Deleted = role.Deleted;
                        existingRole.DeletionDate = role.DeletionDate;
                        existingRole.LastUpdate = role.LastUpdate;
                        existingRole.ExternalApp = role.ExternalApp;
                    }
                    else
                    {
                        // Inserisci il nuovo ruolo
                        role.CreationDate = DateTime.UtcNow;
                        role.LastUpdate = DateTime.UtcNow;
                        _ctx.Roles.Add(role);
                    }
                    SharedVariables.elaborated_roles_counter++;
                }

                await _ctx.SaveChangesAsync();                

                return 1;
            }
            catch(Exception ex)
            {
                _logger.LogError("Errore in RolesDAO, errore: " + ex.Message);
                throw new Exception("Errore in RolesDAO, errore: " + ex.Message);
            }
            finally
            {
                foreach (var role in roles)
                {
                    _ctx.Entry(role).State = EntityState.Detached;
                }
            }
        }

        public async Task<List<Roles>> GetAllRoles()
        {
            try
            {
                var result = await _ctx.Roles.ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore in RolesDAO, errore: " + ex.Message);
                throw new Exception("Errore in RolesDAO, errore: " + ex.Message);
            }
        }
    }
}
