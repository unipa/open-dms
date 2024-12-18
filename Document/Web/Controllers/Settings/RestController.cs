using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;

namespace Web.Controllers.Settings;

[Authorize]
[ApiController]
[Route("internalapi/[controller]")]
public class RestController: ControllerBase
{
    private readonly IDataTypeFactory factory;
    private readonly ICustomFieldService customFieldService;
    private readonly ILoggedUserProfile userProfile;

    public RestController(IDataTypeFactory dataTypeFactory, ICustomFieldService customFieldService, ILoggedUserProfile userProfile)
    {
        this.factory = dataTypeFactory;
        this.customFieldService = customFieldService;
        this.userProfile = userProfile;
    }


    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("Query/{Id}")]

    public async Task<ActionResult<FieldTypeValue[]>> Query(string Id, string? Parameters = "")
    {
        try
        {
            var M = await customFieldService.GetById(Id);
            if (M == null) return null;
            Parameters += "," + userProfile.Get().userId +","+ userProfile.Get().LoggedUser;
            var api = await factory.Instance(M.DataType);
            var list = await api.Search(M, Parameters, 200);
            list = list.OrderBy(l => l.LookupValue).ToList();
            return Ok (list);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}