using Microsoft.AspNetCore.Http;

namespace OpenDMS.MultiTenancy.Interfaces;

public class DomainTenantResolver : ITenantResolver
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public DomainTenantResolver(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }
    public async Task<string> ResolveAsync()
    {
        return await Task.FromResult(Resolve());
    }
    public string Resolve()
    {
        return httpContextAccessor.HttpContext.Request.Host.Host;
    }
}
