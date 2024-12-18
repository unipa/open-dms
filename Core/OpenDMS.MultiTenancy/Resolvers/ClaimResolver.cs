using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;


namespace OpenDMS.MultiTenancy.Interfaces;

public class ClaimResolver : ITenantResolver
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IConfiguration config;

    public ClaimResolver(IHttpContextAccessor httpContextAccessor, IConfiguration config)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.config = config;
    }
    public async Task<string> ResolveAsync()
    {
        return await Task.FromResult(Resolve());
    }
    public string Resolve()
    {
        var tenantKey = config["TenantKey"];
        if (httpContextAccessor.HttpContext.User == null) return "";
        return "" + httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == tenantKey).Value;
    }

}
