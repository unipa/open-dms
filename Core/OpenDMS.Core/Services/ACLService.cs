using AutoMapper;
using Microsoft.Extensions.Logging;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Schemas;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.MultiTenancy.Interfaces;

namespace OpenDMS.Core.BusinessLogic;

/// <summary>
/// Le ACL definizione liste di autorizzazioni da associare alle tipologie documentali.
/// Ogni tipologia documentale è associata ad una ACL che definisce quali utenti, ruoli o gruppi (profili) possono utilizzarla
/// Una ACL può essere associata a più tipologie. 
/// I permessi di utilizzo della tipologia (es. visibilità, creazione, modifica, ecc..) vengono definiti sul ruolo associato
/// Associando un gruppo si concedono i permessi agli utenti presenti nel gruppo
/// Associando un utente si concedono tutti i permessi sulla tipologia
/// </summary>

public class ACLService : IACLService
{

    private readonly ILogger<IACLService> _logger;
    private readonly IACLRepository _aclRepository;
    private readonly IApplicationDbContextFactory _connectionFactory;

    /// <summary>
    /// Le ACL definizione liste di autorizzazioni da associare alle tipologie documentali.
    /// Ogni tipologia documentale è associata ad una ACL che definisce quali utenti, ruoli o gruppi (profili) possono utilizzarla
    /// Una ACL può essere associata a più tipologie. 
    /// I permessi di utilizzo della tipologia (es. visibilità, creazione, modifica, ecc..) vengono definiti sul ruolo associato
    /// Associando un gruppo si concedono i permessi agli utenti presenti nel gruppo
    /// Associando un utente si concedono tutti i permessi sulla tipologia
    /// </summary>
    /// 
    public ACLService(ILogger<IACLService> logger, IACLRepository aclRepository)
    {
        this._logger = logger;
        this._aclRepository = aclRepository;
    }


    /// <summary>
    /// Recupera il nome di una ACL attraverso il codice Identificativo.
    /// Se l'ACL ha degli utenti/ruoli/gruppi associati, vengono restituiti anche questi
    /// </summary>
    /// <param name="Id">Identificativo della ACL da cercare</param>
    /// <returns></returns>
    public async Task<ACL> GetById(string Id)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(Id);
            return await _aclRepository.GetById(Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetById", Id);
            throw;
        }
    }

    /// <summary>
    /// Recupera l'elenco di tutte le ACL definite sul tenant
    /// </summary>
    /// <returns>Elenco di ACL con i rispettivi utenti/ruoli/gruppi autorizzati</returns>
    public async Task<List<ACL>> GetAll()
    {
        try
        {
            var list = await _aclRepository.GetAll();
            return list;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAll");
            throw;
        }
    }

    /// <summary> 
    /// Crea una nuova ACL fornendo un Identificativo e un Nome
    /// </summary>
    /// <param name="acl">Identificativo e Nome della ACL da creare</param>
    /// <returns>Oggetto ACL creato</returns>

    public async Task<ACL> Insert(CreateOrUpdateACL acl)
    {
        ArgumentNullException.ThrowIfNull(acl);
        try
        {
            ACL A = new();
            A.Id = acl.Id;
            A.Name = acl.Name;
            A.Description = acl.Name;
            A.CreationDate = DateTime.UtcNow;
            int res = await _aclRepository.Insert(A);
            return res > 0 ? A : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Insert", acl);
            throw;
        }
    }

    /// <summary>
    /// Aggiorna il nome di una ACL
    /// </summary>
    /// <param name="acl">Identificativo e Nome della ACL da rinominare</param>
    /// <returns>Oggetto ACL aggiornato</returns>
    public async Task<ACL> Update(CreateOrUpdateACL acl)
    {
        ArgumentNullException.ThrowIfNull(acl);
        try
        {
            ACL exist = new();
            exist.Id = acl.Id;
            exist.Name = acl.Name;
            int res = await _aclRepository.Update(exist);
            return res > 0 ? exist : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Update", acl);
            throw;
        }
    }


    /// <summary>
    /// Cancella una ACL con tutti i riferimenti ad utenti/ruoli/gruppi associati.
    /// Una ACL associata ad almeno una Tipologia non può essere cancellata.
    /// </summary>
    /// <param name="Id">Identificativo della ACL da cancellare</param>
    /// <returns>Oggetto ACL eliminato</returns>
    public async Task<ACL> Delete(string Id)
    {
        ArgumentNullException.ThrowIfNull(Id);
        try
        {
            var exist = await _aclRepository.GetById(Id);
            if (exist != null)
            {
                var result = await _aclRepository.Delete(exist);
                return result > 0 ? exist : null;
            }
            else return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Delete", Id);
            throw;
        }
    }

    /// <summary>
    /// Aggiunge un utente/ruolo/gruppo ad una ACL
    /// </summary>
    /// <param name="aclauth">Identificativo della ACL, Codice Profile e Tipo Profilo da autorizzare</param>
    /// <returns>Oggetto ACLPermission aggiunto</returns>
    public async Task<int> AddPermission(ACLPermission aclauth)
    {
        ArgumentNullException.ThrowIfNull(aclauth);
        try
        {
            if (await _aclRepository.GetPermission(aclauth.ACLId, aclauth.ProfileId, aclauth.ProfileType, aclauth.PermissionId) == null)
            {
                int res = await _aclRepository.AddPermission(aclauth);
                return res;
            }
            else return 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RemovePermission", aclauth);
            return 0;
        }
    }


    /// <summary>
    /// Modifica un utente/ruolo/gruppo ad una ACL
    /// </summary>
    /// <param name="aclauth">Identificativo della ACL, Codice Profile e Tipo Profilo da autorizzare</param>
    /// <returns>Oggetto ACLPermission aggiunto</returns>
    public async Task<int> ChangePermission(ACLPermission aclauth)
    {
        ArgumentNullException.ThrowIfNull(aclauth);
        try
        {
            var exist = await _aclRepository.GetPermission(aclauth.ACLId, aclauth.ProfileId, aclauth.ProfileType, aclauth.PermissionId);
            if (exist != null)
            {
                exist.Authorization = aclauth.Authorization;
                int res = await _aclRepository.ChangePermission(exist);
                return res;
            }
            else return 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ChangePermission", aclauth);
            return 0;
        }
    }

    /// <summary>
    /// Rimuove un profilo autorizzativo da una ACL
    /// </summary>
    /// <param name="aclauth">Identificativo della ACL, Codice Profile e Tipo Profilo da autorizzare</param>
    /// <returns>Oggetto ACLPermission aggiunto</returns>
    public async Task<int> RemovePermission(ACLPermission aclauth)
    {
        ArgumentNullException.ThrowIfNull(aclauth);
        try
        {
            var exist = await _aclRepository.GetPermission(aclauth.ACLId, aclauth.ProfileId, aclauth.ProfileType, aclauth.PermissionId);
            if (exist != null)
            {
                var result = await _aclRepository.RemovePermission(exist);
                return result;
            }
            else return 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RemovePermission", aclauth);
            return 0;
        }
    }



    /// <summary>
    /// Ritorna l'elenco dei profili abilitati su una ACL.
    /// </summary>
    /// <param name="aclId">Identificativo univoco della ACL</param>
    /// <returns>Elenco di oggetti ACLPermission</returns>

    public async Task<List<ACLPermission>> GetAllPermissions(string aclId)
    {
        ArgumentNullException.ThrowIfNull(aclId);
        try
        {
            var result = await _aclRepository.GetAllPermissions(aclId);
            return (result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAllPermissions", aclId);
            return null;
        }
    }

    public async Task<AuthorizationType> GetAuthorization(string aclId, UserProfile userInfo, string permissionId)
    {
        var A = AuthorizationType.None;
        if (userInfo != null && !String.IsNullOrEmpty(userInfo.userId))
        {
            if (!String.IsNullOrEmpty(aclId))
            {
                var p = await _aclRepository.GetAuthorization(aclId, userInfo.userId, ProfileType.User, permissionId);
                if (p == AuthorizationType.Granted)
                    A = p;
            }
            if (A == AuthorizationType.None && aclId != "$GLOBAL$")
            {
                var p1 = await _aclRepository.GetAuthorization("$GLOBAL$", userInfo.userId, ProfileType.User, permissionId);
                if (p1 == AuthorizationType.Granted)
                    A = p1;
            }
            if (A == AuthorizationType.None)
            {
                // verifico se almeno uno dei ruoli dell'utente è autorizzato
                foreach (var r in userInfo.Roles)
                {
                    var pr = await _aclRepository.GetAuthorization(aclId, r.Id, ProfileType.Role, permissionId);
                    if (pr == AuthorizationType.Granted)
                    {
                        A = pr;
                        break;
                    }
                    else
                    {
                        var i = r.Id.IndexOf("\\");
                        if (i > 0)
                        {
                            var rr = r.Id.Substring(0, i);
                            var pr2 = await _aclRepository.GetAuthorization(aclId, rr, Domain.Enumerators.ProfileType.Role, permissionId);
                            if (pr2 == AuthorizationType.Granted)
                            {
                                A = pr2;
                                break;
                            }
                        }
                    }

                    pr = await _aclRepository.GetAuthorization("$GLOBAL$", r.Id, ProfileType.Role, permissionId);
                    if (pr == AuthorizationType.Granted)
                    {
                        A = pr;
                        break;
                    }
                    else
                    {
                        var i = r.Id.IndexOf("\\");
                        if (i > 0)
                        {
                            var rr = r.Id.Substring(0, i);
                            var pr2 = await _aclRepository.GetAuthorization("$GLOBAL$", rr, Domain.Enumerators.ProfileType.Role, permissionId);
                            if (pr2 == AuthorizationType.Granted)
                            {
                                A = pr2;
                                break;
                            }
                        }

                    }
                }
            }
            if (A == AuthorizationType.None)
            {
                // verifico se almeno uno dei gruppi dell'utente è autorizzato
                foreach (var g in userInfo.Groups)
                {
                    var pg = await _aclRepository.GetAuthorization(aclId, g.Id, ProfileType.Group, permissionId);
                    if (pg == AuthorizationType.Granted)
                    {
                        A = pg;
                        break;
                    }
                    else
                    if (aclId != "$GLOBAL$")
                    {
                        pg = await _aclRepository.GetAuthorization("$GLOBAL$", g.Id, Domain.Enumerators.ProfileType.Group, permissionId);
                        if (pg == AuthorizationType.Granted)
                        {
                            A = pg;
                            break;
                        }
                    }

                }
            }
        }
        return A;
    }
    public async Task<AuthorizationType> GetAuthorization(string aclId, string profileId, ProfileType profileType, string permissionId)
    {
        var A = AuthorizationType.None;
        if (!String.IsNullOrEmpty(aclId))
        {
            var p = await _aclRepository.GetAuthorization(aclId, profileId, profileType, permissionId);
            if (p == AuthorizationType.Granted)
                A = p;
            else
            {
                var i = profileId.IndexOf("\\");
                if (profileType == ProfileType.Role && i > 0)
                {
                    var r = profileId.Substring(0, i);
                    var pr = await _aclRepository.GetAuthorization(aclId, r, Domain.Enumerators.ProfileType.Role, permissionId);
                    if (pr == AuthorizationType.Granted)
                    {
                        A = pr;
                    }
                }

            }
        }
        if (A == AuthorizationType.None && (aclId != "$GLOBAL$"))
        {
            var p1 = await _aclRepository.GetAuthorization("$GLOBAL$", profileId, profileType, permissionId);
            if (p1 == AuthorizationType.Granted)
                A = p1;
            else
            {
                var i = profileId.IndexOf("\\");
                if (profileType == ProfileType.Role && i > 0)
                {
                    var r = profileId.Substring(0, i);
                    var pr = await _aclRepository.GetAuthorization("$GLOBAL$", r, Domain.Enumerators.ProfileType.Role, permissionId);
                    if (pr == AuthorizationType.Granted)
                    {
                        A = pr;
                    }
                }

            }
        }
        return A;
    }

    public async Task<ACLPermission> GetPermission(string aclId, string profileId, ProfileType profileType, string permissionId)
    {
        try
        {
            var result = await _aclRepository.GetPermission(aclId, profileId, profileType, permissionId);
            if (result == null)
                result = new ACLPermission() { ProfileId = profileId, ProfileType = profileType,    PermissionId = permissionId, Authorization=AuthorizationType.None };  
            return (result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetPermission", aclId);
            return null;
        }
    }


    /// <summary>
    /// Verifica se un profilo è presente in una ACL 
    /// </summary>
    /// <param name="aclId">Identificativo univoco della ACL</param>
    /// <param name="profileId">Identificativo univoco del profilo</param>
    /// <param name="profileType">Tipo di profilo</param>
    /// <returns>true=profilo trovato</returns>

    public async Task<bool> HasPermission(string aclId, string profileId, ProfileType profileType, string permissionId)
    {
        try
        {
            var result = new ACLPermission();
            result = await _aclRepository.GetPermission(aclId, profileId, profileType, permissionId);
            return (result != null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "HasPermission", aclId, profileId, profileType);
            return false;
        }
    }


}


