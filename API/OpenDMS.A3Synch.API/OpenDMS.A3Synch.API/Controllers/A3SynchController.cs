using Microsoft.AspNetCore.Mvc;
using OpenDMS.A3Synch.API.Utility;
using OpenDMS.Core.Filters;
using OpenDMS.Infrastructure.Services.Workers;
using System.Net;
using System.Reflection;

namespace A3Synch.Controllers
{
    [Authorization(":admin")]
    [ApiController]
    [Route("/api/a3synch/")]
    public class A3SynchController : ControllerBase
    {

        private readonly ILogger<A3SynchController> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHostApplicationLifetime hostApplication;
        private readonly IConfiguration _config;
        private static readonly object _lock = new object();

        public A3SynchController(ILogger<A3SynchController> logger, IServiceScopeFactory scopeFactory, IHostApplicationLifetime hostApplication, IConfiguration config)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            this.hostApplication = hostApplication;
            _config = config;
        }

        /// <summary>
        /// Questa API inizia la sincronizzazione tra A3 ed i database di DMS.
        /// L'API per essere lanciata necessita che l'utente sia autenticato e che abbia il ruolo admin.
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("Start")]
        public async Task<IActionResult> Start()
        {
             if (SharedVariables.isSyncing)
            {
                return Conflict("Sto gi� eseguendo una sincronizzazione.");
            }

  
            SharedVariables.startSynch = DateTime.UtcNow;
  
            return Ok("Sincronizzazione in avvio entro 5 secondi");
 
        }

        /// <summary>
        /// Questa API ritorna lo stato di avanzamento delle elaborazioni in esecuzione.
        /// L'API per essere lanciata necessita che l'utente sia autenticato e che abbia il ruolo admin.
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("GetStatus")]
        public async Task<IActionResult> GetStatus()
        {
            string[] dataEora = SharedVariables.startSynch.ToString("dd/MM/yyyy HH:mm:ss").Split(' ');
            string startSynchDateTime = "Ultima sincronizzazione avviata il: " + dataEora[0] + " alle ore: " + dataEora[1].Replace(":", ".") + ".\n";
            string IsSynching = "STATO CORRENTE: " + (SharedVariables.isSyncing ? "Sincronizzazione In Corso... " : "In Attesa") + "\n";

            string groups_status = "Operazioni preliminari, sincronizazzione UserGroups non ancora iniziata.\n";
            string nodes_status = "Operazioni preliminari, sincronizazzione OrganizationNodes non ancora iniziata.\n";
            string member_list_status = "Operazioni preliminari, richiesta di tutti gli utenti e ruoli ad A3 non ancora iniziata.\n";
            string roles_status = "Operazioni preliminari, sincronizazzione Roles non ancora iniziata.\n";
            string users_status = "Operazioni preliminari, sincronizazzione Users non ancora iniziata.\n";
            string users_groups_roles_status = "Operazioni preliminari, sincronizazzione UserGroupsRoles non ancora iniziata.\n";
            string users_keycloak_status = "Operazioni preliminari, sincronizazzione Users da keycloak non ancora iniziata.\n";
            string kc_roles_status = "Operazioni preliminari, sincronizazzione Ruoli su keycloak non ancora iniziata.\n";
            string kc_users_status = "Operazioni preliminari, sincronizazzione Users su keycloak non ancora iniziata.\n";
            string kc_role_to_users_status = "Operazioni preliminari, sincronizazzione dei ruoli sugli users keycloak non ancora iniziata.\n";
            string kc_idp_to_users_status = "Operazioni preliminari, sincronizazzione dell'idp sugli users keycloak non ancora iniziata.\n";

            if (SharedVariables.synch_error == null)
            {

                if (SharedVariables.total_groups_counter > 0)
                {
                    groups_status = "Record di Organigramma elaborati: " + SharedVariables.elaborated_groups_counter + " di " + SharedVariables.total_groups_counter + "\n";
                }
                if (SharedVariables.total_nodes_counter > 0)
                {
                    nodes_status = "Record di Strutture elaborati: " + SharedVariables.elaborated_nodes_counter + " di " + SharedVariables.total_nodes_counter + "\n";
                }
                if (SharedVariables.elaborated_member_list > 0)
                {
                    member_list_status = "Recupero Utenti e Ruoli di organigramma: " + SharedVariables.elaborated_member_list + " di " + SharedVariables.total_groups_counter + "\n";
                }
                if (SharedVariables.total_roles_counter > 0)
                {
                    roles_status = "Record di Ruoli elaborati: " + SharedVariables.elaborated_roles_counter + " di " + SharedVariables.total_roles_counter + "\n";
                }
                if (SharedVariables.total_users_counter > 0)
                {
                    users_status = "Record di Utenti elaborati: " + SharedVariables.elaborated_users_counter + " di " + SharedVariables.total_users_counter + "\n";
                }
                if (SharedVariables.total_usergroupsroles_counter > 0)
                {
                    users_groups_roles_status = "Record di Utenti in Strutture elaborati: " + SharedVariables.elaborated_usergroupsroles_counter + " di " + SharedVariables.total_usergroupsroles_counter + "\n";
                }
                if (SharedVariables.total_keycloak_users_counter > 0)
                {
                    users_groups_roles_status = "Record di Utenti da keycloak al db elaborati: " + SharedVariables.elaborated_keycloak_users_counter + " di " + SharedVariables.total_usergroupsroles_counter + "\n";
                }
                if (SharedVariables.kc_roles_total > 0)
                {
                    kc_roles_status = "Ruoli elaborati per inserimento su keycloak: " + SharedVariables.kc_roles_elaborated + " di " + SharedVariables.kc_roles_total + "\n";
                }
                if (SharedVariables.kc_roles_total == -1)
                {
                    kc_roles_status = "Nessuna differenza di ruoli tra quelli presenti sul DB e quelli su Keycloak riscontrata\n";
                }
                if (SharedVariables.kc_users_total > 0)
                {
                    kc_users_status = "Utenti sincronizzati su keycloak: " + SharedVariables.kc_users_elaborated + " di " + SharedVariables.kc_users_total + "\n";
                }
                if (SharedVariables.kc_users_total == -1)
                {
                    kc_users_status = "Nessuna differenza di utenti tra quelli presenti sul DB e quelli su Keycloak riscontrata\n";
                }
                if (SharedVariables.total_addedRole_counter > 0)
                {
                    kc_role_to_users_status = "Ruoli aggiunti agli utenti di keycloak: " + SharedVariables.elaborated_addedRole_counter + " di " + SharedVariables.total_addedRole_counter + "\n";
                }
                if (SharedVariables.total_addedIdp_counter > 0)
                {
                    kc_idp_to_users_status = "IdP aggiunto agli utenti di keycloak: " + SharedVariables.elaborated_addedIdp_counter + " di " + SharedVariables.total_addedIdp_counter + "\n";
                }

                return
                    SharedVariables.isSyncing
                        ? Ok(startSynchDateTime + IsSynching + groups_status + nodes_status + member_list_status + roles_status + users_status + users_groups_roles_status + kc_roles_status + kc_users_status + kc_role_to_users_status + kc_idp_to_users_status)
                        : Ok(startSynchDateTime + IsSynching);
            }
            else
            {
                var errore = "EXCEPTION: " + SharedVariables.synch_error + "\n";

                if (SharedVariables.total_groups_counter > 0 && SharedVariables.synch_error_UserGroups != null)
                {
                    groups_status = "Record UserGroups elaborati: " + SharedVariables.elaborated_groups_counter + " di " + SharedVariables.total_groups_counter + "\n";
                }
                else
                {
                    groups_status = SharedVariables.synch_error_UserGroups;
                }
                if (SharedVariables.total_nodes_counter > 0 && SharedVariables.synch_error_Organigramma != null)
                {
                    nodes_status = "Record OrganizationNodes elaborati: " + SharedVariables.elaborated_nodes_counter + " di " + SharedVariables.total_nodes_counter + "\n";
                }
                else
                {
                    nodes_status = SharedVariables.synch_error_Organigramma;
                }
                if (SharedVariables.elaborated_member_list > 0)
                {
                    member_list_status = "Chiamate ad UserGroup, per ottenere utenti e ruoli, effettuate : " + SharedVariables.elaborated_member_list + " di " + SharedVariables.total_groups_counter + "\n";
                }
                if (SharedVariables.total_roles_counter > 0 && SharedVariables.synch_error_RoleInDb != null)
                {
                    roles_status = "Record Roles elaborati: " + SharedVariables.elaborated_roles_counter + " di " + SharedVariables.total_roles_counter + "\n";
                }
                else
                {
                    roles_status = SharedVariables.synch_error_RoleInDb;
                }
                if (SharedVariables.total_users_counter > 0 && SharedVariables.synch_error_UsersInDb != null)
                {
                    users_status = "Record Users elaborati: " + SharedVariables.elaborated_users_counter + " di " + SharedVariables.total_users_counter + "\n";
                }
                else
                {
                    users_status = SharedVariables.synch_error_UsersInDb;
                }
                if (SharedVariables.total_usergroupsroles_counter > 0 && SharedVariables.synch_error_UserGroupsRolesInDb != null)
                {
                    users_groups_roles_status = "Record UsersGroupsRoles elaborati: " + SharedVariables.elaborated_usergroupsroles_counter + " di " + SharedVariables.total_usergroupsroles_counter + "\n";
                }
                else
                {
                    users_groups_roles_status = SharedVariables.synch_error_UserGroupsRolesInDb;
                }
                if (SharedVariables.kc_roles_total > 0 && SharedVariables.synch_error_SynchAllInKC != null)
                {
                    kc_roles_status = "Ruoli elaborati per inserimento su keycloak: " + SharedVariables.kc_roles_elaborated + " di " + SharedVariables.kc_roles_total + "\n";
                }
                else
                {
                    kc_roles_status = SharedVariables.synch_error_SynchAllInKC;
                }
                if (SharedVariables.kc_roles_total == -1)
                {
                    kc_roles_status = "Nessuna differenza di ruoli tra quelli presenti sul DB e quelli su Keycloak riscontrata\n";
                }
                if (SharedVariables.kc_users_total > 0 && SharedVariables.synch_error_SynchAllInKC != null)
                {
                    kc_users_status = "Utenti sincronizzati su keycloak: " + SharedVariables.kc_users_elaborated + " di " + SharedVariables.kc_users_total + "\n";
                }
                else
                {
                    kc_users_status = SharedVariables.synch_error_SynchAllInKC;
                }
                if (SharedVariables.total_addedRole_counter > 0 && SharedVariables.synch_error_SynchAllInKC != null)
                {
                    kc_role_to_users_status = "Ruoli aggiunti agli utenti di keycloak: " + SharedVariables.elaborated_addedRole_counter + " di " + SharedVariables.total_addedRole_counter + "\n";
                }
                else
                {
                    kc_role_to_users_status = SharedVariables.synch_error_SynchAllInKC;
                }
                if (SharedVariables.total_addedIdp_counter > 0 && SharedVariables.synch_error_SynchAllInKC != null)
                {
                    kc_idp_to_users_status = "IdP aggiunto agli utenti di keycloak: " + SharedVariables.elaborated_addedIdp_counter + " di " + SharedVariables.total_addedIdp_counter + "\n";
                }
                else
                {
                    kc_idp_to_users_status = SharedVariables.synch_error_SynchAllInKC;
                }

                return Ok(startSynchDateTime + IsSynching + errore + groups_status + nodes_status + member_list_status + roles_status + users_status + users_groups_roles_status + kc_roles_status + kc_users_status + kc_role_to_users_status + kc_idp_to_users_status);
            }

        }



        [HttpGet("shutdown")]
        public void Shutdown(string pwd)
        {
            if (pwd == DateTime.Now.ToString("yyyyMMdd"))
            {
                hostApplication.StopApplication();
            }
        }

        [HttpGet("Log")]
        public async Task<IActionResult> Log()
        {
            var fileName = Path.Combine(Environment.CurrentDirectory, "Log", $"OpenDMS.A3Synch.API" + DateTime.Now.ToString("yyyyMMdd") + ".log");
            using (var m = new MemoryStream())
            {
                using (var reader = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    await reader.CopyToAsync(m);
                }
                var fileBytes = m.ToArray();
                return File(fileBytes, "application/octet-stream", fileName);
            }
        }


        /// <summary>
        /// Questa API scarica Il file di log delle chiamate di A3.
        /// L'API per essere lanciata necessita che l'utente sia autenticato e che abbia il ruolo admin.
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("DownloadFile")]
        public async Task<IActionResult> DownloadFile()
        {
            string filePath = _config.GetValue<string>("FileSettings:FilePath");

            if (string.IsNullOrEmpty(filePath))
                return StatusCode(StatusCodes.Status500InternalServerError, "Percorso del file di log non configurato.");

            try
            {
                if (!System.IO.File.Exists(filePath))
                    return NotFound("Il file di log non esiste.");

                byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                string fileName = Path.GetFileName(filePath);

                return File(fileBytes, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore durante il download del file di log: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Errore durante il download del file.");
            }
        }

        /// <summary>
        /// Questa API mostra le varibili d'ambiente configurate.
        /// L'API per essere lanciata necessita che l'utente sia autenticato e che abbia il ruolo admin.
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("Configuration")]
        public async Task<string> Configuration()
        {
            string log = "";

            FieldInfo[] Fields = typeof(OpenDMS.Domain.Constants.StaticConfiguration).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var F in Fields)
            {
                var Value = (string)F.GetRawConstantValue();
                if (Value != null)
                {
                    if (_config[Value] == null)
                        log += ("## - Setting: " + Value);
                    else
                        log += ("OK - Setting: " + Value + " = " + _config[Value].ToString());
                    log += Environment.NewLine;
                }
            }
            return log;
        }
    }
}
