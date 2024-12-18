using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Repositories;
using OpenDMS.MultiTenancy.Interfaces;

namespace Web.Controllers.Settings;

//[Authorize]
[Authorize]

[ApiController]
[Route("internalapi/[controller]")]
public class TemplateController : ControllerBase
{

    public TemplateController()
    {
    }


    /// <summary>
    /// Recupera i file presenti in una sotto cartella della cartella template
    /// </summary>
    /// <param name="Folder">Nome del fascicolo</param>
    /// <param name="Key">Nome dela configurazione</param>
    /// <returns>il valore della configurazione</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{Folder}")]
    public async Task<ActionResult<string[]>> GetAll(string Folder)
    {
        try
        {
            var files = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "wwwroot", "templates", Folder));
            return files.Select(f => Path.GetFileName(f)).ToArray();
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    /// <summary>
    /// Recupera i file presenti in una sotto cartella della cartella template
    /// </summary>
    /// <param name="Folder">Nome del fascicolo</param>
    /// <param name="Key">Nome dela configurazione</param>
    /// <returns>il valore della configurazione</returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{Folder}/{TemplateName}")]
    public async Task<ActionResult<string>> Get(string Folder, string TemplateName)
    {
        try
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "templates", Folder, TemplateName);
            var data = Convert.ToBase64String(System.IO.File.ReadAllBytes(filePath));
            return data;
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }


}