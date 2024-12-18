using Microsoft.EntityFrameworkCore;
using A3Synch.Interfacce;
using A3Synch.Models;
using OpenDMS.A3Synch.API.Utility;
using A3Synch.BL;

namespace A3Synch.DAO
{
    public class UsersDAO : IUsersDAO
    {
        private readonly IContext _ctx;
        private readonly ILogger<UsersDAO> _logger;

        public UsersDAO(IContext ctx, ILogger<UsersDAO> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public async Task<int> Update(Contacts contact, bool isKeycloakBL = false)
        {
            try
            {
                _ctx.Contacts.Update(contact);
                if (!isKeycloakBL)
                {
                    SharedVariables.elaborated_users_counter++;
                }
                else
                {
                    SharedVariables.elaborated_keycloak_users_counter++;
                }
                await _ctx.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore in UsersDAO, errore: " + ex.Message);
                throw new Exception("Errore in UsersDAO, errore: " + ex.Message);
            }
            finally
            {
                // Detach tutti gli oggetti
                _ctx.Entry(contact).State = EntityState.Detached;
            }

        }

        public async Task<int> Add(Users user, Contacts contact, bool isKeycloakBL = false)
        {
            try
            {
                _ctx.Users.Add(user);
                _ctx.Contacts.Add(contact);

                if (!isKeycloakBL)
                {
                    SharedVariables.elaborated_users_counter++;
                }
                else
                {
                    SharedVariables.elaborated_keycloak_users_counter++;
                }
                await _ctx.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore in UsersDAO, errore: " + ex.Message);
                throw new Exception("Errore in UsersDAO, errore: " + ex.Message);
            }
            finally
            {
                // Detach tutti gli oggetti
                _ctx.Entry(user).State = EntityState.Detached;
                _ctx.Entry(contact).State = EntityState.Detached;
            }

        }

        public async Task<int> Add(Contacts contact, bool isKeycloakBL = false)
        {
            try
            {
                _ctx.Contacts.Add(contact);
                if (!isKeycloakBL)
                {
                    SharedVariables.elaborated_users_counter++;
                }
                else
                {
                    SharedVariables.elaborated_keycloak_users_counter++;
                }
                await _ctx.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore in UsersDAO, errore: " + ex.Message);
                throw new Exception("Errore in UsersDAO, errore: " + ex.Message);
            }
            finally
            {
                // Detach tutti gli oggetti
                _ctx.Entry(contact).State = EntityState.Detached;
            }

        }



        //public async Task<int> UpdateUsers(List<Users> users)
        //{
        //    try
        //    {
        //        SharedVariables.total_users_counter += users.Count;

        //        foreach (var user in users)
        //        {
        //            // Controlla se esiste un record con lo stesso id nel database
        //            var existingUser = await _ctx.Users.FindAsync(user.Id);

        //            if (existingUser != null)
        //            {
        //                // Aggiorna i campi dell'oggetto esistente
        //                existingUser.Deleted = user.Deleted;
        //                existingUser.DeletionDate = user.DeletionDate;
        //                existingUser.LastUpdate = user.LastUpdate;
        //                SharedVariables.elaborated_users_counter++;
        //            }
        //            else
        //            {
        //                // Inserisci il nuovo ruolo
        //                _ctx.Users.Add(user);
        //                SharedVariables.elaborated_users_counter++;
        //            }
        //        }

        //        await _ctx.SaveChangesAsync();

        //        // Detach tutti gli oggetti
        //        foreach (var user in users)
        //        {
        //            _ctx.Entry(user).State = EntityState.Detached;
        //        }

        //        return 1;
        //    }
        //    catch(Exception ex)
        //    {
        //        _logger.LogError("Errore in UsersDAO, errore: " + ex.Message);
        //        throw new Exception("Errore in UsersDAO, errore: " + ex.Message);
        //    }

        //}

        public async Task<List<Users>> GetAllUsers()
        {
            try
            {
                var result = await _ctx.Users
                    .Select(u => new Users
                    {
                        Id = u.Id,
                        ContactId = u.ContactId,
                        CreationDate = u.CreationDate,
                        Deleted = u.Deleted,
                        DeletionDate = u.DeletionDate,
                        LastUpdate = u.LastUpdate
                    })
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore in UsersDAO, errore: " + ex.Message);
                throw new Exception("Errore in UsersDAO, errore: " + ex.Message);
            }
        }

        //public async Task<string> GetContactId(string id)
        //{
        //    try
        //    {
        //        var user = _ctx.Users.FirstOrDefault(user => user.Id == id);
        //        return user?.ContactId ?? null;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Errore in UserDAO, errore: " + ex.Message);
        //        throw new Exception("Errore in UserDAO, errore: " + ex.Message);
        //    }
        //}
        public async Task<Users?> GetUser(string id)
        {
            return await _ctx.Users.FirstOrDefaultAsync(user => user.Id == id);
        }
        public async Task<Contacts?> GetContact(string id)
        {
            try
            {
                Contacts contact = _ctx.Contacts.FirstOrDefault(user => user.Id == id);
                return contact;
            }
            catch
            {
                Contacts contact = new Contacts() { Annotation="ERROR"};
                return contact;
            }
                
        }
    }
}
