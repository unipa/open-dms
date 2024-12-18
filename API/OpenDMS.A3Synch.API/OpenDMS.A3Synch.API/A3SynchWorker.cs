using A3Synch.Interfacce;
using A3Synch.Models;
using OpenDMS.A3Synch.API.Utility;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;

namespace OpenDMS.Infrastructure.Services.Workers;

public class A3SynchWorker : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<A3SynchWorker> _logger;
    private readonly IConfiguration _configuration;

    private System.Timers.Timer _timer = null;
    private bool IsActive = false;
    private double time = 300; // verifico ogni 30 minuti


    public A3SynchWorker(
        IServiceProvider serviceProvider,
        ILogger<A3SynchWorker> logger,
        IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configuration = configuration;
        SharedVariables.startSynch = DateTime.UtcNow;
        _timer = new System.Timers.Timer ();
        _timer.Elapsed += _timer_Elapsed;
        _timer.AutoReset = false;
    }

    private object LockObject = new object();
    //   private DateTime lastRun = DateTime.UtcNow;

    private void StartThreads()
    {
        if (!IsActive) return;
        lock (LockObject)
        {
            _timer.Start();
        }

    }
    private void StopThreads()
    {
        lock (LockObject)
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Interval = 1000;

                //                _timer = null;
            }
        }
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting A3Synch");
        IsActive = true;

        StartThreads();
        return Task.CompletedTask;
    }

    private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        if (IsActive)
        {
            if (SharedVariables.startSynch < DateTime.UtcNow && !SharedVariables.isSyncing)
            {
                _logger.LogInformation("Start New Day synchronization - Last Update: " + SharedVariables.startSynch.ToString());
                SharedVariables.startSynch = DateTime.UtcNow.AddHours(24);
                DoWork(e);
            }
        }
        _timer.Interval = time * 1000;
        StartThreads();
    }

    private static readonly object _lock = new object();

    public async void DoWork(object state)
    {
        if (!SharedVariables.isSyncing)
        {
            SharedVariables.isSyncing = true;
            try
            {
                int res = 0;
                bool InError = false;
                using (var scope = _serviceProvider.CreateScope())
                {

                    var _utils = scope.ServiceProvider.GetRequiredService<IUtils>();
                    var _accessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
                    var _usergroupsbl = scope.ServiceProvider.GetRequiredService<IUserGroupsBL>();
                    var _organizationnodesBL = scope.ServiceProvider.GetRequiredService<IOrganizationNodesBL>();
                    var _rolesbl = scope.ServiceProvider.GetRequiredService<IRolesBL>();
                    var _usersbl = scope.ServiceProvider.GetRequiredService<IUsersBL>();
                    var _usergroupsrolesbl = scope.ServiceProvider.GetRequiredService<IUserGroupRolesBL>();
                    var _keycloakbl = scope.ServiceProvider.GetRequiredService<IKeycloakBL>();

                    //Inizializza i counter del progresso
                    SharedVariables.synch_error = null;
                    _usergroupsbl.ResetStatus();
                    _organizationnodesBL.ResetStatus();
                    _rolesbl.ResetStatus();
                    //_contactsbl.ResetStatus();
                    _usersbl.ResetStatus();
                    _usergroupsrolesbl.ResetStatus();
                    _keycloakbl.ResetStatus();
                    List<Struttura> AllUnits = new List<Struttura>();

                    try
                    {
                        await _utils.ClearFile();
                    }
                    catch (Exception ex)
                    {
                        InError = true;
                        _logger.LogError($"Errore durante la cancellazione del contenuto del file: {ex.Message}");
                        //await _utils.SendErrorNotification(ex.Message);
                    }

                    try
                    {
                        AllUnits = await _utils.GetAllOrganizationNodes();
                    }
                    catch (Exception ex)
                    {
                        InError = true;
                        _logger.LogError($"Errore: {ex.Message}");
                        SharedVariables.synch_error_UserGroups = "Si è verificato un errore durante la sincronizzazione di UserGroups: " + ex.Message;
                        //await _utils.SendErrorNotification(ex.Message);
                        throw ex;
                    }

                    try
                    {
                        res += await _usergroupsbl.GetAllOrganizationsPages(AllUnits);
                    }
                    catch (Exception ex)
                    {
                        InError = true;
                        _logger.LogError($"Errore: {ex.Message}");
                        SharedVariables.synch_error_UserGroups = "Si è verificato un errore durante la sincronizzazione di UserGroups: " + ex.Message;
                        //await _utils.SendErrorNotification(ex.Message);
                    }
                    try
                    {
                        res += await _organizationnodesBL.SyncOrganigrammaNodes(AllUnits);
                    }
                    catch (Exception ex)
                    {
                        InError = true;
                        _logger.LogError($"Errore: {ex.Message}");
                        SharedVariables.synch_error_Organigramma = "Si è verificato un errore durante la sincronizzazione di Organigramma: " + ex.Message;
                        //await _utils.SendErrorNotification(ex.Message);
                    }


                    List<Members> all_members = await _utils.GetAllMembersInStructures();

                    try
                    {
                        res += await _rolesbl.SynchRolesInDb(all_members);
                    }
                    catch (Exception ex)
                    {
                        InError = true;
                        _logger.LogError($"Errore: {ex.Message}");
                        SharedVariables.synch_error_RoleInDb = "Si è verificato un errore durante la sincronizzazione di Roles: " + ex.Message;
                        //await _utils.SendErrorNotification(ex.Message);
                    }
                    try
                    {
                        res += await _usersbl.SynchUsersInDb(all_members);
                    }
                    catch (Exception ex)
                    {
                        InError = true;
                        _logger.LogError($"Errore: {ex.Message}");
                        SharedVariables.synch_error_UsersInDb = "Si è verificato un errore durante la sincronizzazione di Users: " + ex.Message;
                        //await _utils.SendErrorNotification(ex.Message);
                    }
                    try
                    {
                        res += await _usergroupsrolesbl.SynchUserGroupsRolesInDb(all_members);
                    }
                    catch (Exception ex)
                    {
                        InError = true;
                        _logger.LogError($"Errore: {ex.Message}");
                        SharedVariables.synch_error_UserGroupsRolesInDb = "Si è verificato un errore durante la sincronizzazione di UserGroupRoles: " + ex.Message;
                        //await _utils.SendErrorNotification(ex.Message);
                    }
                    /*try
                    {
                        (SharedVariables.kc_access_token, SharedVariables.kc_refresh_token) = await _keycloakbl.GetTokens();
                        res += await _keycloakbl.SynchAllInKC();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Errore: {ex.Message}");
                        SharedVariables.synch_error_SynchAllInKC = "Si è verificato un errore durante la sincronizzazione su Keycloak: " + ex.Message;
                    }*/
                    if (InError)
                    {
                        SharedVariables.startSynch = DateTime.UtcNow.AddHours(1);
                    }
                    SharedVariables.isSyncing = false;
                }
            }
            catch (Exception ex)
            {
                SharedVariables.startSynch = DateTime.UtcNow.AddHours(1);
                SharedVariables.isSyncing = false;
                SharedVariables.synch_error = "Si è verificato un errore durante la sincronizzazione: " + ex.Message;
                _logger.LogError("Si è verificato un errore nell'API Start del controller A3Synch: " + ex.Message);
            }

        }
        else
        {
            _logger.LogInformation("DoWork yet started!");
        }
    }


    public Task StopAsync(CancellationToken stoppingToken)
    {
        IsActive = false;
        _logger.LogInformation("Stop A3Synch");
        StopThreads();
        return Task.CompletedTask;
    }

    bool disposed = false;

    public void Dispose()
    {
        if (!disposed)
        {
            disposed = true;
            StopThreads();
        }
    }
}