using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using System.Security.Claims;
using System.Text.Json;

namespace OpenDMS.Startup
{
    public class ClaimsTransformation : IClaimsTransformation
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUpdateIdentityService userService;
        private readonly IAppSettingsRepository appSettingsRepository;
        private readonly IDistributedCache cache;

        public ClaimsTransformation(
            IHttpContextAccessor httpContextAccessor, 
            IUpdateIdentityService userService, 
            IAppSettingsRepository appSettingsRepository,
            IDistributedCache cache)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userService = userService;
            this.appSettingsRepository = appSettingsRepository;
            this.cache = cache;
        }

        private object thisLock = new object();
        public async Task<ClaimsPrincipal?> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal != null)
                if (principal.Identity != null)
                    if (principal.Identity.IsAuthenticated)
                    {
                        lock (thisLock)
                        {
                            var id = principal?.Identity?.Name;
                            byte[]? u = null;
                            if (!string.IsNullOrEmpty(id))
                            {
                                u = cache.Get("userProfile-" + id);
                            }
                            if (u == null)
                            {
                                UserProfile userProfile = userService.Update(principal).GetAwaiter().GetResult();
                                if (userProfile != null)
                                {
                                    id = userProfile.userId;
                                    cache.Set("userProfile-" + id, JsonSerializer.SerializeToUtf8Bytes(userProfile), new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromSeconds(60) });
                                }
                            }
                        }
                        if (httpContextAccessor.HttpContext != null)
                        {
                            var offline = await appSettingsRepository.Get("Offline");
                            if (!String.IsNullOrEmpty(offline) && !principal.Claims.Any(w => w.Value == "admin") && httpContextAccessor.HttpContext.Request.Path != "/Offline")
                            {
                                if (DateTime.TryParse(offline, out DateTime timeout))
                                {
                                    if (DateTime.Now.TimeOfDay < timeout.TimeOfDay)
                                    {
                                        httpContextAccessor.HttpContext.Response.Redirect("/Offline");
                                    }
                                    else
                                    {
                                        await appSettingsRepository.Set("Offline", "");
                                    }
                                }
                                else
                                {
                                    await appSettingsRepository.Set("Offline", "");
                                }
                            }
                        }
                    }
            return principal;
        }
    }
}
