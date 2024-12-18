using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Filters;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Schemas;
using Web.DTOs;

namespace Web.Controllers.Documents;

[Authorize]
[Authorization(":admin")]
[ApiController]
[Route("internalapi/[controller]")]
public class CustomFieldController : ControllerBase
{

    private readonly ICustomFieldService fieldService;

    public CustomFieldController(ICustomFieldService customFieldService)
    {
        fieldService = customFieldService;
    }


    /// <summary>
    /// Metodo per ottenere la lista delle tipologie di dataType
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Dictionary<string, string>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("GetAllTypes")]
    public async Task<ActionResult<List<DataType_DTO>>> GetAllTypes()
    {
        try
        {
            List<FieldType> list = (await fieldService.GetAllTypes()).Where(m=>!m.Customized).ToList();

            List<DataType_DTO> res = new List<DataType_DTO>();

            foreach (FieldType type in list)
            {
                res.Add(new DataType_DTO() { Id = type.Id, Name = type.Name });
            }

            return Ok(res);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Metodo per ottenere un oggetto CustomFieldType(che identifica una metadato) tramite id. 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>

    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(FieldType))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<FieldType>> GetById(string id)
    {
        try
        {
            var result = new FieldType();
            result = await fieldService.GetById(id);
            return result == null ? new FieldType() : Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Metodo per ottenere la lista dei metadati 
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IEnumerable<FieldType>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FieldType>>> GetAll()
    {
        try
        {
            var list = (await fieldService.GetAll()).Where(m => m.Customized);
            return Ok(list);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary> 
    /// Metodo per salvare un metadato passando un oggetto CustomFieldType (metadato) tramite body 
    /// </summary>
    /// <param name="bd"></param>
    /// <returns></returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(FieldType))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    public async Task<ActionResult<FieldType>> Insert(FieldType bd)
    {
        try
        {
            if (await fieldService.GetById(bd.Id) == null)
            {
                var datatype = await fieldService.GetById(bd.DataType);
                bd.ControlType = datatype.ControlType;
                int res = await fieldService.Insert(bd);
                return res > 0 ? Ok(await fieldService.GetById(bd.Id)) : BadRequest("L'inserimento non è andato a buon fine.");
            }
            else return Conflict("Non può essere inserito un CustomFieldType con un id già esistente.");

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Metodo per aggiornare un metadato passando l'oggetto CustomFieldType (metadato) con il relativo id tramite body 
    /// </summary>
    /// <param name="bd"></param>
    /// <returns></returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(FieldType))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut]
    public async Task<ActionResult<int>> Update(FieldType bd)
    {
        try
        {
            var exist = await fieldService.GetById(bd.Id);
            if (exist != null)
            {
                var datatype = await fieldService.GetById(bd.DataType);
                bd.ControlType = datatype.ControlType;
                int res = await fieldService.Update(bd);
                return res;
            }
            else return 0;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// Metodo per cancellare un CustomFieldType(metadato)
    /// </summary>
    /// <param name="Id">Id del CustomFieldType da cancellare</param>
    /// <returns></returns>
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{Id}")]
    public async Task<ActionResult<string>> Delete(string Id)
    {
        try
        {
            var exist = await fieldService.GetById(Id);
            if (exist != null)
            {
                var result = await fieldService.Delete(Id);
                return result > 0 ? Ok("successo") : BadRequest("Eliminazione non riuscita");
            }
            else return NotFound("Il customFieldType selezionato non è stato trovato");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }



}