using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Filters;
using OpenDMS.MultiTenancy.Exceptions;
using OpenDMS.MultiTenancy.Interfaces;
using OpenDMS.TenantManager.API.DTOs;
using System.Data.Common;

namespace OpenDMS.TenantManager.API.Controllers
{

    [Authorization(":admin")]
    /// <summary>
    /// [Authorize]
    /// </summary>
    [ApiController]
    [Route("api/tenants/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IConfiguration config;
        private readonly ITenantRegistryRepository<Tenant> tenantRepo;
        private readonly string? userId;

        public TenantController(ILogger<TenantController> logger,
                                IConfiguration config,
                                ITenantRegistryRepository<Tenant> tenantRepository)
        {
            this.logger = logger;
            this.config = config;
            this.tenantRepo = tenantRepository;
        }

        /// <summary>
        /// Restituisce tutti i tenant registrati.
        /// E' possibile filtrare solo i tenant attivi impostando il parametro <paramref name="OnlyOnlineTenants"/>=true
        /// </summary>
        /// <param name="OnlyOnlineTenants">Mostra tutti i tenant registrati o solo quelli attivi</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Tenant?> GetAll(bool OnlyOnlineTenants = false)
        {
            return tenantRepo.GetAll(OnlyOnlineTenants);
        }

        /// <summary>
        /// Restituisce un tenant in base al suo Identificativo.
        /// </summary>
        /// <param name="id">Identificativo del tenant da restituire</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode:StatusCodes.Status200OK, type: typeof (Tenant))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<Tenant> GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest(nameof(id)+" non può essere vuoto");

            var t = tenantRepo.GetById(id);
            return t!= null ? Ok(t) : NotFound();
        }

        /// <summary>
        /// Testa la connessione del database di un tenant.
        /// </summary>
        /// <param name="id">Identificativo del tenant da restituire</param>
        /// <returns>true = il database è raggiungibile</returns>
        /// <exception cref="TenantNotFoundException"></exception>
        [ProducesResponseType(statusCode:StatusCodes.Status200OK, type: typeof (bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("Test/{id}")]
        public async Task<ActionResult<bool>> TestConnectionById(string id)
        {
            var T = await tenantRepo.GetByIdAsync(id);
            if (T == null) throw new TenantNotFoundException(id);
            return Ok(await tenantRepo.TestConnection(T));
        }

        /// <summary>
        /// Crea un nuovo tenant.
        /// Il parametro "ConnectionString" rappresenta la stringa di connessione al nuovo database
        /// Se l'istanza di database è la stessa del registro dei tenant, è possibile indicare anche solamente il nome del unovo database
        /// Se il parametro viene lasciato vuoto, il sistema userà, come nome di database, l'identificativo del tenant 
        /// Indicando la modalità di collegamento "Create", il database indicato nella stringa di connessione viene creato. Se il database esiste viene restituito un errore.
        /// Indicando la modalità di collegamento "Connect" il database indicato nella stringa di connessione deve esistere altrimenti si riceverà un errore.
        /// Indicando la modalità di collegamento "ConnectOrCreate" il database indicato nella stringa di connessione viene creato se non trovato.
        /// </summary>
        /// <param name="newTenant">Dati del tenant da creare</param>
        /// <returns>I dati del tenant creato</returns>
        /// <exception cref="TenantDatabaseNotFoundException"></exception>
        /// <exception cref="DuplicateTenantDatabaseException"></exception>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<Tenant>> Create(TenantCreationDTO newTenant)
        {
            if (string.IsNullOrEmpty(newTenant.Id))
            {
                return BadRequest(nameof(newTenant.Id) + " non può essere vuoto");
            }
            if (string.IsNullOrWhiteSpace(newTenant.Description))
                newTenant.Description = newTenant.Id;
            if (string.IsNullOrWhiteSpace(newTenant.RootFolder))
                newTenant.Description = tenantRepo.GetDefault().RootFolder + "/" + newTenant.Id;
            Tenant T = new Tenant(newTenant.Description + "", newTenant.Provider, newTenant.ConnectionString, newTenant.Offline);
            T.Id = newTenant.Id;
            T.OpenIdConnectClientId = newTenant.ClientId + "";
            T.OpenIdConnectClientSecret = newTenant.ClientSecret + "";
            T.OpenIdConnectAuthority = newTenant.Realm + "";
            T.URL = newTenant.URL + "";
            T.RootFolder = newTenant.RootFolder;
            if (string.IsNullOrEmpty(T.Provider))
            {
                T.Provider = tenantRepo.GetDefault().Provider;
            }
            else
            {
                if (!T.Provider.Equals(tenantRepo.GetDefault().Provider, StringComparison.InvariantCultureIgnoreCase) && String.IsNullOrEmpty(T.ConnectionString))
                    throw new ArgumentNullException(nameof(T.ConnectionString));
            }
            if (string.IsNullOrEmpty(T.ConnectionString))
            {
                if (T.Provider.ToLower() != "memory")
                {
                    DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
                    builder.ConnectionString = tenantRepo.GetDefault().ConnectionString;

                    builder["database"] = config["TenantPrefix"]  + T.Id;
                    T.ConnectionString = builder.ConnectionString;
                } else
                {
                    T.ConnectionString = T.Id;
                }
            }
            else
            {
                if (T.ConnectionString.IndexOf(";") < 0)
                {
                    if (T.Provider.ToLower() != "memory")
                    {
                        DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
                        builder.ConnectionString = tenantRepo.GetDefault().ConnectionString;
                        builder["database"] = T.ConnectionString;
                        T.ConnectionString = builder.ConnectionString;
                    }
                    else
                    {
                        T.ConnectionString = T.Id;
                    }
                }


            }


            if (newTenant.DatabaseConnectionStrategy == DatabaseConnectionMode.Connect)
            {
                await tenantRepo.TryConnect(T);
            }
            else
            if (newTenant.DatabaseConnectionStrategy == DatabaseConnectionMode.ConnectOrCreate)
            {
                await tenantRepo.ConnectOrCreate(T);
            }
            else
            {
                await tenantRepo.Create(T);
            }
            return this.CreatedAtAction(nameof (GetById), new { T.Id }, await tenantRepo.GetByIdAsync(T.Id));
        }

        /// <summary>
        /// Aggiorna i dati di un tenant. E' possibile aggiornare l'identificativo, la descrizione e lo stato di attivazione.
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        /// <exception cref="TenantNotFoundException"></exception>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] TenantUpdateModel tenant)
        {
            if (string.IsNullOrEmpty(tenant.Id))
            {
                return BadRequest(nameof(tenant.Id) + " non può essere vuoto");
            }
            var Tenant = tenantRepo.GetById(tenant.Id);
            if (Tenant == null) throw new TenantNotFoundException(tenant.Id);
            if (string.IsNullOrWhiteSpace(tenant.RootFolder))
                tenant.Description = tenantRepo.GetDefault().RootFolder + "/" + tenant.Id;
            Tenant.Description = tenant.Description;
            Tenant.Offline = tenant.Offline;
            Tenant.OpenIdConnectClientId = tenant.ClientId;
            Tenant.OpenIdConnectClientSecret = tenant.ClientSecret;
            Tenant.OpenIdConnectAuthority = tenant.Realm;
            Tenant.RootFolder = tenant.RootFolder;
            Tenant.URL = tenant.URL;
            await tenantRepo.Update(Tenant);
            return this.NoContent();
        }

        /// <summary>
        /// Cancella un tenant dal registro. Il database associato non viene cancellato.
        /// </summary>
        /// <param name="Id">Identificativo del tenant da cancellare</param>
        /// <returns></returns>
        /// <exception cref="TenantNotFoundException"></exception>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return BadRequest(nameof(Id) + " non può essere vuoto");
            }
            var T = tenantRepo.GetById(Id);
            if (T == null) throw new TenantNotFoundException(Id);
            await tenantRepo.Delete(T);
            return this.Ok();
        }


        /// <summary>
        /// Aggiorna i database di tutti i tenant.
        /// </summary>
        /// <returns>Ritorna il numero di tenant aggiornati</returns>
        [ProducesResponseType(statusCode:StatusCodes.Status200OK, type: typeof(int))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("UpdateTenants")]
        public async Task<ActionResult<int>> UpdateTenants()
        {
            int updated = 0;
            foreach (var T in tenantRepo.GetAll())
            {
                updated += ((await tenantRepo.CreateOrUpdateDatabase(T)) ? 1: 0);
            }
            return this.Ok(updated);
        }

    }
}
