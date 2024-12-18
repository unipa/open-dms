using Microsoft.Extensions.Logging;


namespace OpenDMS.MultiTenancy.Interfaces;

public class NullTenantResolver : ITenantResolver
{
    private readonly ITenantContext<Tenant> tenantGetter;
    private readonly ILogger<NullTenantResolver> logger;

    public NullTenantResolver(ITenantContext<Tenant> tenantGetter, ILogger<NullTenantResolver> logger)
    {
        this.tenantGetter = tenantGetter;
        this.logger = logger;
    }

    public string Resolve()
    {
        return tenantGetter.Tenant.Id;
    }
    public async Task<string> ResolveAsync()
    {
        return await Task.FromResult( tenantGetter.Tenant.Id);
    }

}
