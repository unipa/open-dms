
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using System.Text.Json;

namespace OpenDMS.Core.BusinessLogic
{
    public class LoggedUserProfile : ILoggedUserProfile
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserService userService;
        private readonly IDistributedCache cache;

        public LoggedUserProfile(IHttpContextAccessor httpContextAccessor, IUserService userService,  IDistributedCache cache)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userService = userService;
            this.cache = cache;
        }


        public UserProfile Get()
        {
            if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                UserProfile user = null;
                var u = httpContextAccessor.HttpContext.User.Identity.Name;
                if (!string.IsNullOrEmpty(u))
                {
                    var bytes = cache.Get("userProfile-" + u);
                    user = (bytes != null) && (bytes.Length > 8) ? JsonSerializer.Deserialize<UserProfile>(bytes) : userService.GetUserProfile(u).GetAwaiter().GetResult(); 
                }
                if (user == null) user = userService.GetUserProfile(u).GetAwaiter().GetResult();
                return user;
            }
            else
                return null;
        }



    }
}
