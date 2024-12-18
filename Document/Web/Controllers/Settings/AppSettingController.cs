using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Filters;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Repositories;
using OpenDMS.MultiTenancy.Interfaces;
using System.Reflection;

namespace Web.Controllers.Settings;

//[Authorize]
[Authorization(":admin")]
[Authorize]
[ApiController]
[Route("internalapi/[controller]")]
public class AppSettingController : ControllerBase
{

    private readonly ILogger<AppSettingController> _logger;
    private readonly IConfiguration configuration;
    private readonly IHostApplicationLifetime hostApplication;
    private readonly IAppSettingsRepository appSettingRepo;
    private readonly IApplicationDbContextFactory connectionFactory;

    public AppSettingController(ILogger<AppSettingController> logger, IConfiguration configuration, IHostApplicationLifetime hostApplication, IAppSettingsRepository appSettingRepo, IApplicationDbContextFactory connectionFactory)
    {
        _logger = logger;
        this.configuration = configuration;
        this.hostApplication = hostApplication;
        this.appSettingRepo = appSettingRepo;
        this.connectionFactory = connectionFactory;
    }


    [HttpGet("Log")]
    public async Task<IActionResult> Log()
    {
        var fileName = Path.Combine(Environment.CurrentDirectory, "Log", $"OpenDMS.FrontEnd.API" + DateTime.Now.ToString("yyyyMMdd") + ".log");
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
                if (configuration[Value] == null)
                    log += ("## - Setting: " + Value);
                else
                    log += ("OK - Setting: " + Value + " = " + configuration[Value].ToString());
                log += Environment.NewLine;
            }
        }
        return log;
    }

    [HttpGet("offline")]
    public async Task Offline (string timeout)
    {
        await appSettingRepo.Set("Offline", timeout);
    }






    [HttpGet("shutdown")]
    public void Shutdown(string pwd)
    {
        if (pwd == DateTime.Now.ToString("yyyyMMdd"))
        {
            hostApplication.StopApplication();
        }
    }


    /// <summary>
    /// Recupera una configurazione del sistema attraverso una chiave. 
    /// E' possibile indicare un CompanyId per ottenere la configurazione di una specifica Company.
    /// Se la configurazione per la company non � presente verr� restituita la configurazione generica
    /// </summary>
    /// <param name="CompanyId">Id della Company</param>
    /// <param name="Key">Nome dela configurazione</param>
    /// <returns>il valore della configurazione</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{CompanyId}/{Key}")]
    public async Task<ActionResult<string>> Get(int CompanyId, string Key)
    {
        try
        {
            var result = await appSettingRepo.Get(CompanyId, Key);
            return string.IsNullOrEmpty(result) ? NotFound("Il Settings indicato non � stato trovato.") : Ok(result);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary>
    /// Recupera una configurazione del sistema attraverso una chiave. 
    /// </summary>
    /// <param name="Key">Nome della configurazione</param>
    /// <returns>il valore della configurazione</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{Key}")]
    public async Task<ActionResult<string>> Get(string Key)
    {
        try
        {
            var result = await appSettingRepo.Get(Key);
            return string.IsNullOrEmpty(result) ? NotFound("Il Settings indicato non � stato trovato.") : Ok(result);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary>
    /// Imposta una configurazione per il sistema o per una company
    /// </summary>
    /// <param name="setting">Oggetto che identifica la company (indicare 0 per impostare una configurazione per l'intero sistema), il Nome e il Valore della configurazione</param>
    /// <returns>Ritorna il valore impostato</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<string>> Set(AppSetting setting)
    {
        try
        {
            await appSettingRepo.Set(setting.CompanyId, setting.Name, setting.Value);
            return Ok(await appSettingRepo.Get(setting.CompanyId, setting.Name));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Ritorna tutte le configurazione di una Company
    /// </summary>
    /// <param name="CompanyId">Id della Company</param>
    /// <returns>Elenco di oggetti AppSetting</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<AppSetting>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("{companyId}")]
    public async Task<ActionResult<List<AppSetting>>> GetAll(int CompanyId = 0)
    {
        try
        {
            return Ok(await appSettingRepo.GetAll(CompanyId));
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

}