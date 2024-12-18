using Microsoft.AspNetCore.Http;

namespace OpenDMS.MultiTenancy.Interfaces;
public class PathResolver : ITenantResolver
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public PathResolver(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }
    public async Task<string> ResolveAsync()
    {
        return await Task.FromResult(Resolve());
    }
    public string Resolve()
    {
        var segments = httpContextAccessor.HttpContext.Request.Path.ToString().Split('/');
        if (segments.Length > 1)
        {
            httpContextAccessor.HttpContext.Request.PathBase = "/" + segments[1];
            return segments[1];
        }
        else
            return "";
    }
}
