using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.CustomPages.Models;
using OpenDMS.Core.Interfaces;
using OpenDMS.CustomPages;

namespace OpenDMS.DocumentManager.Controllers;

//[Authorize]
[ApiController]
[Route("/api/ui/application")]
public class CustomPageController : ControllerBase
{
    private readonly ICustomPageProvider customPages;
    private readonly ILoggedUserProfile userProfile;

    public CustomPageController(
        ICustomPageProvider customPageProvider,
        ILoggedUserProfile userProfile
        )
    {
        this.customPages = customPageProvider;
        this.userProfile = userProfile;
    }

   
    [Authorize]
    [HttpGet()]
    public async Task<ActionResult<List<CustomPageDTO>>> GetRootPages()
    {
        var u = userProfile.Get();
        return Ok (await customPages.GetPages(u));
    }

    [Authorize]
    [HttpGet("{pageId}")]
    public async Task<ActionResult<List<CustomPageDTO>>> GetPages(string pageId)
    {
        var u = userProfile.Get();
        return Ok(await customPages.GetPages(u, pageId));
    }


}