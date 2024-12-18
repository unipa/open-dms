using Microsoft.AspNetCore.Mvc;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;

namespace OpenDMS.UserManager.API.Controllers
{
    [Route("api/identity/[controller]")]
    [ApiController]
    public class AvatarController : ControllerBase
    {
        private readonly ILogger<AvatarController> logger;
        private readonly IUserService userContext;

        public AvatarController(ILogger<AvatarController> logger, IUserService userContext)
        {
            this.logger = logger;
            this.userContext = userContext;
        }

        [HttpGet("{subjectId}")]

        public async Task<FileResult> GetImage(string subjectId)
        {
            ProfileType profileType = (ProfileType)int.Parse(subjectId.Substring(0, 1));
            string profileId = subjectId.Substring(1);
            var data = await userContext.GetAvatar(profileId, profileType);
            return File(data, System.Net.Mime.MediaTypeNames.Image.Jpeg, subjectId + ".png");
        }

    }
}
