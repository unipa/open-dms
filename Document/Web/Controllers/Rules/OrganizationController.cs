using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Filters;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;

namespace Web.Controllers.Security;

[Authorize]
[Authorization(":admin")]
[ApiController]
[Route("internalapi/[controller]")]
public class OrganizationController : ControllerBase
{


    private readonly ILogger<OrganizationController> _logger;
    private readonly IUserService userContext;
    private readonly IOrganizationUnitService service;

    public OrganizationController(ILogger<OrganizationController> logger, IUserService userContext, IOrganizationUnitService service)
    {
        _logger = logger;
        this.userContext = userContext;
        this.service = service;
    }

    /// <summary>
    /// Ritorna l'albero della struttura dei nodi e degli utenti in essi contenuti
    /// La struttura riporta tutti i nodi e tutti gli utenti, attivi, futuri e dismessi. 
    /// E' possibile ottenere la struttura attiva in una data, indicando un valore diverso da 0 (nel formato YYYYMMDD) vengono estratti solo i nodi e gli uteti attivi della struttura nella data indicata
    /// </summary>
    /// <param name="StartISODate"> </param>
    /// <returns></returns>
    [HttpGet("Nodes/{StartISODate:int}")]
    public async Task<List<OrganizationNodeTree>> GetOrganizationTree(int StartISODate = 0)
    {
        return await service.GetOrganizationTree(StartISODate);
    }

    /// <summary>
    /// Ritorna le informazioni di un nodo nella data indicata (0 = data corrente)
    /// </summary>
    /// <param name="userGroupId"></param>
    /// <param name="StartISODate"></param>
    /// <returns></returns>
    [HttpGet("{userGroupId}/{StartISODate:int}")]
    public async Task<OrganizationNodeInfo> GetById(string userGroupId, int StartISODate = 0)
    {
        return await service.GetById(userGroupId, StartISODate);
    }



    /// <summary>
    /// Ritorna le informazioni di un nodo nella data indicata tramite l'Identificativo esterno(0 = data corrente)
    /// </summary>
    /// <param name="externalId">Identificativo esterno del nodo della struttura</param>
    /// <param name="StartISODate">Data di riferimento (0=data ordierna)</param>
    /// <returns></returns>
    [HttpGet("GetByExternalId/{externalId}/{StartISODate:int}")]
    public async Task<OrganizationNodeInfo> GetByExternalId(string externalId, int StartISODate = 0)
    {
        return await service.GetByExternalId(externalId, StartISODate);
    }



    /// <summary>
    /// Ritorna gli utenti di un nodo nella data indicata (0 = data corrente)
    /// </summary>
    /// <param name="userGroupId"></param>
    /// <param name="StartISODate"></param>
    /// <returns></returns>
    [HttpGet("Users/{userGroupId}/{StartISODate:int}")]
    public async Task<List<UserInGroup>> GetUsers(string userGroupId, int StartISODate = 0)
    {
        return await service.GetUsers(userGroupId, StartISODate);
    }

    /// <summary>
    /// Aggiunge un nodo alla struttura.
    /// I nodi creati con data di inizio decorrenza uguale o antecedente alla data corrente non potranno essere cancellati
    /// </summary>
    /// <param name="organization">Parametri del nodo</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<OrganizationNodeInfo> AddOrganizationNode(CreateOrUpdateOrganizationNode organization)
    {
        return await service.AddOrganizationNode(organization, User.Identity.Name);
    }

    /// <summary>
    /// Aggiorna le informazioni di un nodo attivo nella data indicata come inizio decorrenza.
    /// Non è possibile modificare un nodo del passato
    /// Se un nodo è attivo ma ha una data di decorrenza nel passato non è possibile indicare una data differente.
    /// </summary>
    /// <param name="userGroupId"></param>
    /// <param name="organization"></param>
    /// <returns></returns>
    [HttpPut("Update/{userGroupId}")]
    public async Task<OrganizationNodeInfo> UpdateOrganizationNode(string userGroupId, CreateOrUpdateOrganizationNode organization)
    {
        return await service.UpdateOrganizationNode(userGroupId, organization);

    }

    /// <summary>
    /// Tenta di cancellare un nodo. Se il nodo è del passato o la data di decorrenza è uguale alla data odierna non è possibile cancellarlo. 
    /// </summary>
    /// <param name="userGroupId">Identificativo interno del nodo da chiudere</param>
    /// <param name="startISODate">Data in cui è attivo il nodo</param>
    /// <param name="endDate">Data di chiusura del nodo</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<int> DeleteOrganizationNode(string userGroupId, int startISODate, DateTime endDate)
    {
        return await service.DeleteOrganizationNode(userGroupId, startISODate, endDate);

    }

    /// <summary>
    /// Sposta il gruppo indicato su un nuovo nodo. 
    /// La data di cessazione della configuraione corrente viene posta al giorno precedente a quella di spostamento del nodo
    /// Non è possibile spostare un nodo se le date indicate si sovrappongono con altre configurazioni dello stesso gruppo 
    /// </summary>
    /// <param name="organization"></param>
    /// <returns></returns>
    [HttpPatch]
    public async Task<OrganizationNodeInfo> MoveOrganizationNode(MoveOrganizationNode organization)
    {
        return await service.MoveOrganizationNode(organization);

    }


    /// <summary>
    /// Aggiunge un utente su un nodo
    /// </summary>
    /// <param name="userInGroup"></param>
    /// <returns></returns>
    [HttpPost("Users/Add")]
    public async Task<int> AddUser(UserInGroup userInGroup)
    {
        return await service.AddUser(userInGroup);
    }

    /// <summary>
    /// Aggiorna le informazioni di un utente su un nodo (ruolo, date di decorrenza e cessazione)
    /// </summary>
    /// <param name="userInGroup"></param>
    /// <returns></returns>
    [HttpPut("Users/Update")]
    public async Task<int> EditUser(UserInGroup userInGroup)
    {
        return await service.EditUser(userInGroup);

    }

    /// <summary>
    /// Tenta di rimuovere un utente da un gruppo.
    /// Non è possibile rimuovere un utente se la data di decorrenza è uguale a quella corrente o nel passato 
    /// </summary>
    /// <param name="userInGroup"></param>
    /// <returns></returns>
    [HttpDelete("Users/Remove")]
    public async Task<int> RemoveUser(UserInGroup userInGroup)
    {
        return await service.RemoveUser(userInGroup);

    }

}