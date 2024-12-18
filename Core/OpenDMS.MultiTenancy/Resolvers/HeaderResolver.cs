
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace OpenDMS.MultiTenancy.Interfaces;

public class HeaderResolver : ITenantResolver
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IConfiguration config;

    public HeaderResolver(IHttpContextAccessor httpContextAccessor, IConfiguration config)
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
        return httpContextAccessor.HttpContext.Request.Headers.ContainsKey(tenantKey) ? httpContextAccessor.HttpContext.Request.Headers[tenantKey] : "";
    }
}
